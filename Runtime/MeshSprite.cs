using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Valax321.MeshSprites
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class MeshSprite : MonoBehaviour
    {
        private static class ShaderProps
        {
            public static int MainTex = Shader.PropertyToID("_MainTex");
            public static int Color = Shader.PropertyToID("_RendererColor");
            public static int Cutoff = Shader.PropertyToID("_Cutoff");
            public static int Flip = Shader.PropertyToID("_Flip");
        }

        // We cache all generated meshes in play mode to avoid rebuilding them repeatedly.
        private static Dictionary<Sprite, Mesh> s_spriteMeshCache = new Dictionary<Sprite, Mesh>();

        #region Public stuff
        
        public Sprite sprite
        {
            get => m_sprite;
            set
            {
                if (value != m_sprite)
                {
                    GenerateMeshForSprite(m_filter, value);
                    UpdateProps(m_renderer, value, m_propBlock);
                }

                m_sprite = value;
            }
        }

        public Color color
        {
            get => m_color;
            set
            {
                m_color = value;
                m_propBlock?.SetColor(ShaderProps.Color, value);
                if (m_renderer)
                {
                    m_renderer.SetPropertyBlock(m_propBlock);
                }
            }
        }
        
        #endregion

        [SerializeField] private Sprite m_sprite;
        [SerializeField] private Color m_color = Color.white;
        [SerializeField, Range(0, 1)] private float m_alphaCutoff = 0.5f;
        [SerializeField] private bool m_flipX;
        [SerializeField] private bool m_flipY;
        [FormerlySerializedAs("m_shadows")] 
        [SerializeField] private ShadowCastingMode m_shadowCastingMode = ShadowCastingMode.On;
        [SerializeField] private bool m_receiveShadows = true;
        [SerializeField] private LightProbeUsage m_lightProbeUsage = LightProbeUsage.BlendProbes;
        [SerializeField] private Material m_material;

        private MeshFilter m_filter;
        private MeshRenderer m_renderer;
        private MaterialPropertyBlock m_propBlock;

        private void Awake()
        {
            if (!Application.isPlaying)
                return;
            
            m_filter = GetComponent<MeshFilter>();
            m_renderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            if (!Application.isPlaying)
                return;
            
            m_propBlock = new MaterialPropertyBlock();
            m_renderer.shadowCastingMode = m_shadowCastingMode;
            m_renderer.receiveShadows = m_receiveShadows;
            m_renderer.sharedMaterial = m_material;
            m_renderer.lightProbeUsage = m_lightProbeUsage;
            m_renderer.GetPropertyBlock(m_propBlock);
            GenerateMeshForSprite(m_filter, sprite);
            UpdateProps(m_renderer, sprite, m_propBlock);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (m_propBlock == null)
                m_propBlock = new MaterialPropertyBlock();

            var filter = GetComponent<MeshFilter>();
            var renderer = GetComponent<MeshRenderer>();
            
            renderer.sharedMaterial = m_material;
            renderer.shadowCastingMode = m_shadowCastingMode;
            renderer.receiveShadows = m_receiveShadows;
            renderer.lightProbeUsage = m_lightProbeUsage;
            
            renderer.GetPropertyBlock(m_propBlock);
            GenerateMeshForSprite(filter, sprite);
            UpdateProps(renderer, sprite, m_propBlock);
        }

        [ContextMenu("Regenerate Mesh")]
        private void Regenerate()
        {
            OnValidate();
        }
#endif

        private void GenerateMeshForSprite(MeshFilter filter, Sprite newSprite)
        {
            if (!filter)
                return;

            if (!newSprite)
            {
                filter.sharedMesh = null;
                return;
            }

            if (Application.isPlaying && s_spriteMeshCache.ContainsKey(newSprite))
            {
                filter.sharedMesh = s_spriteMeshCache[newSprite];
                return;
            }

            // In the editor we actually just destroy the old mesh each time
            if (filter.sharedMesh && !Application.isPlaying)
            {
                DestroyImmediate(filter.sharedMesh);
            }

            var ppu = newSprite.pixelsPerUnit;
            var mesh = new Mesh
            {
                name = $"{newSprite.name} Sprite Mesh",
                vertices = newSprite.vertices.Convert(x => new Vector3(x.x, x.y, 0)),
                triangles = newSprite.triangles.Convert(x => (int) x),
                uv = newSprite.uv
            };

            // The update flags are a 2020+ feature
#if UNITY_2020_1_OR_NEWER
            mesh.RecalculateNormals(MeshUpdateFlags.DontRecalculateBounds);
            mesh.RecalculateTangents(MeshUpdateFlags.DontRecalculateBounds);
#else
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
#endif
            mesh.RecalculateBounds();

            mesh.UploadMeshData(true);

            if (Application.isPlaying)
            {
                s_spriteMeshCache.Add(newSprite, mesh);
            }

            filter.sharedMesh = mesh;
        }

        private void UpdateProps(Renderer renderer, Sprite newSprite, MaterialPropertyBlock propBlock)
        {
            if (!newSprite)
                return;

            propBlock?.SetTexture(ShaderProps.MainTex, newSprite.texture);
            propBlock?.SetColor(ShaderProps.Color, m_color);
            propBlock?.SetFloat(ShaderProps.Cutoff, m_alphaCutoff);
            propBlock?.SetVector(ShaderProps.Flip, new Vector4(m_flipX ? -1 : 1, m_flipY ? -1 : 1));
            renderer.SetPropertyBlock(propBlock);
        }
    }
}
