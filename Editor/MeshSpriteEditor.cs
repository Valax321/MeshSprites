using System;
using UnityEditor;
using UnityEngine;

namespace Valax321.MeshSprites.Editor
{
    [CustomEditor(typeof(MeshSprite))]
    public class MeshSpriteEditor : UnityEditor.Editor
    {
        private static GUIContent[] s_flipLabels =
        {
            new GUIContent("X"), 
            new GUIContent("Y")
        };
        
        private SerializedProperty m_sprite;
        private SerializedProperty m_color;
        private SerializedProperty m_alphaCutoff;
        private SerializedProperty m_flipX;
        private SerializedProperty m_flipY;
        private SerializedProperty m_shadowCastingMode;
        private SerializedProperty m_receiveShadows;
        private SerializedProperty m_lightProbeUsage;
        private SerializedProperty m_material;
        
        private void OnEnable()
        {
            m_sprite = serializedObject.FindProperty(nameof(m_sprite));
            m_color = serializedObject.FindProperty(nameof(m_color));
            m_alphaCutoff = serializedObject.FindProperty(nameof(m_alphaCutoff));
            m_flipX = serializedObject.FindProperty(nameof(m_flipX));
            m_flipY = serializedObject.FindProperty(nameof(m_flipY));
            m_shadowCastingMode = serializedObject.FindProperty(nameof(m_shadowCastingMode));
            m_receiveShadows = serializedObject.FindProperty(nameof(m_receiveShadows));
            m_lightProbeUsage = serializedObject.FindProperty(nameof(m_lightProbeUsage));
            m_material = serializedObject.FindProperty(nameof(m_material));

            var cmp = serializedObject.targetObject as MeshSprite;
            if (cmp)
            {
                var filter = cmp.GetComponent<MeshFilter>();
                var renderer = cmp.GetComponent<MeshRenderer>();
                filter.hideFlags |= HideFlags.NotEditable;
                renderer.hideFlags |= HideFlags.NotEditable;
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            using (var foldout = new FoldoutHeaderGroupScope("Appearance", "MeshSpriteEditor:appearance"))
            {
                if (foldout.show)
                {
                    EditorGUILayout.PropertyField(m_sprite);
                    EditorGUILayout.PropertyField(m_color);
                    EditorGUILayout.PropertyField(m_alphaCutoff);
                    
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Flip");
                    EditorGUILayout.LabelField("X", GUILayout.Width(40), GUILayout.ExpandWidth(false));
                    m_flipX.boolValue = EditorGUILayout.Toggle(m_flipX.boolValue);
                    EditorGUILayout.LabelField("Y", GUILayout.Width(40), GUILayout.ExpandWidth(false));
                    m_flipY.boolValue = EditorGUILayout.Toggle(m_flipY.boolValue);
                    EditorGUILayout.EndHorizontal();
                }
            }

            using (var foldout = new FoldoutHeaderGroupScope("Rendering", "MeshSpriteEditor:rendering"))
            {
                if (foldout.show)
                {
                    EditorGUILayout.PropertyField(m_lightProbeUsage);
                    EditorGUILayout.PropertyField(m_shadowCastingMode);
                    EditorGUILayout.PropertyField(m_receiveShadows);
                    EditorGUILayout.PropertyField(m_material);
                }
            }

            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }
    }
}