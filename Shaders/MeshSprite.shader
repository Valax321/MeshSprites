// Upgrade NOTE: upgraded instancing buffer 'SpritesMeshSprite' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Sprites/Mesh Sprite"
{
	Properties
	{
		[PerRendererData][NoScaleOffset]_MainTex("Sprite Texture", 2D) = "white" {}
		[PerRendererData]_RendererColor("Renderer Color", Color) = (1,1,1,1)
		_Color("Color", Color) = (1,1,1,1)
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[PerRendererData]_Flip("Flip", Vector) = (1,1,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" "PreviewType"="Plane" }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf Lambert keepalpha addshadow fullforwardshadows nolightmap  nodirlightmap nometa vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _MainTex;
		uniform half4 _Color;
		uniform float _Cutoff = 0.5;

		UNITY_INSTANCING_BUFFER_START(SpritesMeshSprite)
			UNITY_DEFINE_INSTANCED_PROP(half4, _RendererColor)
#define _RendererColor_arr SpritesMeshSprite
			UNITY_DEFINE_INSTANCED_PROP(half2, _Flip)
#define _Flip_arr SpritesMeshSprite
		UNITY_INSTANCING_BUFFER_END(SpritesMeshSprite)

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			half2 _Flip_Instance = UNITY_ACCESS_INSTANCED_PROP(_Flip_arr, _Flip);
			half3 appendResult21 = (half3(( (ase_vertex3Pos).xy * _Flip_Instance ) , ase_vertex3Pos.z));
			v.vertex.xyz = appendResult21;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_MainTex8 = i.uv_texcoord;
			half4 _RendererColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_RendererColor_arr, _RendererColor);
			half4 temp_output_13_0 = ( tex2D( _MainTex, uv_MainTex8 ) * _Color * _RendererColor_Instance );
			o.Albedo = temp_output_13_0.rgb;
			o.Alpha = 1;
			clip( temp_output_13_0.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=18910
0;84;1436;629;1324.861;-263.1209;1;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;7;-1290.074,-101.5133;Inherit;True;Property;_MainTex;Sprite Texture;0;2;[PerRendererData];[NoScaleOffset];Create;False;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PosVertexDataNode;16;-914.1224,537.7484;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;8;-939.8811,-90.36127;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;15;-1105.453,336.827;Inherit;False;InstancedProperty;_RendererColor;Renderer Color;1;1;[PerRendererData];Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;12;-1117.886,126.2812;Inherit;False;Property;_Color;Color;2;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;19;-722.7968,556.8041;Inherit;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;17;-773.7802,690.2306;Inherit;False;InstancedProperty;_Flip;Flip;4;1;[PerRendererData];Create;True;0;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-596.567,-30.09885;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-464.6299,572.4969;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;14;-330.3475,70.07063;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;21;-271.6299,608.4969;Inherit;False;FLOAT3;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Half;False;True;-1;2;;0;0;Lambert;Sprites/Mesh Sprite;False;False;False;False;False;False;True;False;True;False;True;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Absolute;0;Diffuse;3;-1;-1;-1;1;PreviewType=Plane;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;7;0
WireConnection;19;0;16;0
WireConnection;13;0;8;0
WireConnection;13;1;12;0
WireConnection;13;2;15;0
WireConnection;20;0;19;0
WireConnection;20;1;17;0
WireConnection;14;0;13;0
WireConnection;21;0;20;0
WireConnection;21;2;16;3
WireConnection;0;0;13;0
WireConnection;0;10;14;3
WireConnection;0;11;21;0
ASEEND*/
//CHKSM=45095A6A11FDAF85130BB4E21DEE54AAE81DD512