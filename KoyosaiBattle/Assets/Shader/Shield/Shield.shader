// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shield"
{
	Properties
	{
		_MatCap_Outside("MatCap_Outside", 2D) = "white" {}
		_MatCap_Inside("MatCap_Inside", 2D) = "white" {}
		_Shield_Emission("Shield_Emission", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}
		_Offset_Intensity("Offset_Intensity", Range( 0 , 1)) = 0
		_Offset_Speed("Offset_Speed", Float) = 0
		_Shape_Scale("Shape_Scale", Range( 0 , 1)) = 0
		_Noise_Scale("Noise_Scale", Float) = 0
		_Normal_Switch("Normal_Switch", Range( 0 , 1)) = 0
		_Transform("Transform", Range( -0.1 , 1)) = 0
		_Opacity("Opacity", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Wire_Color("Wire_Color", Color) = (0,0,0,0)
		_Wire_Emission("Wire_Emission", Float) = 0
		_Edge_color("Edge_color", Color) = (0,0,0,0)
		_Emission_Intensity("Emission_Intensity", Float) = 0
		_Emission_Speed("Emission_Speed", Float) = 0
		[HideInInspector] _texcoord3( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Background"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float2 uv3_texcoord3;
			float2 uv_texcoord;
			float3 worldNormal;
			half ASEVFace : VFACE;
			float2 uv2_texcoord2;
		};

		uniform float _Offset_Intensity;
		uniform float _Offset_Speed;
		uniform float _Noise_Scale;
		uniform float _Transform;
		uniform float _Shape_Scale;
		uniform float4 _Edge_color;
		uniform sampler2D _Shield_Emission;
		uniform float _Emission_Intensity;
		uniform sampler2D _MatCap_Inside;
		uniform sampler2D _Normal;
		uniform float _Normal_Switch;
		uniform sampler2D _MatCap_Outside;
		uniform float _Emission_Speed;
		uniform float _Wire_Emission;
		uniform float4 _Wire_Color;
		uniform float _Smoothness;
		uniform float _Opacity;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		float4 VectorRejection27( float4 a, float4 b )
		{
			float dotLeft = dot( a, b );
			float dotRight = dot( b , b );
			float4 retVec = a - (dotLeft/dotRight) * b;
			return retVec;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldNormal = UnityObjectToWorldNormal( v.normal );
			float4 transform82 = mul(unity_WorldToObject,float4( ase_worldNormal , 0.0 ));
			float mulTime107 = _Time.y * _Offset_Speed;
			float simplePerlin2D105 = snoise( v.texcoord.xy*_Noise_Scale );
			simplePerlin2D105 = simplePerlin2D105*0.5 + 0.5;
			float temp_output_97_0 = ( sin( mulTime107 ) * simplePerlin2D105 );
			float clampResult24 = clamp( (0.0 + (v.texcoord1.xy.y - _Transform) * (1.0 - 0.0) / (( 0.1 + _Transform ) - _Transform)) , 0.0 , 1.0 );
			float Transform32 = clampResult24;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 worldToObj84 = mul( unity_WorldToObject, float4( ase_worldPos, 1 ) ).xyz;
			float4 a27 = float4( worldToObj84 , 0.0 );
			float4 b27 = transform82;
			float4 localVectorRejection27 = VectorRejection27( a27 , b27 );
			v.vertex.xyz += ( ( transform82 * ( _Offset_Intensity * temp_output_97_0 ) * Transform32 ) + ( _Shape_Scale * localVectorRejection27 * -1.0 * Transform32 ) ).xyz;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 tex2DNode48 = tex2D( _Shield_Emission, i.uv3_texcoord3 );
			float3 temp_cast_0 = (0.0).xxx;
			float3 temp_cast_1 = (-1.0).xxx;
			float3 ase_worldNormal = i.worldNormal;
			float4 transform82 = mul(unity_WorldToObject,float4( ase_worldNormal , 0.0 ));
			float4 lerpResult46 = lerp( float4( (temp_cast_1 + (UnpackNormal( tex2D( _Normal, i.uv_texcoord ) ) - temp_cast_0) * (float3( 1,1,1 ) - temp_cast_1) / (float3( 1,1,1 ) - temp_cast_0)) , 0.0 ) , transform82 , _Normal_Switch);
			float3 worldToViewDir9 = mul( UNITY_MATRIX_V, float4( lerpResult46.xyz, 0 ) ).xyz;
			float2 clampResult15 = clamp( (float2( 0,0 ) + ((worldToViewDir9).xy - float2( -1,-1 )) * (float2( 1,1 ) - float2( 0,0 )) / (float2( 1,1 ) - float2( -1,-1 ))) , float2( 0,0 ) , float2( 1,1 ) );
			float switchResult38 = (((i.ASEVFace>0)?(1.0):(0.0)));
			float4 lerpResult39 = lerp( tex2D( _MatCap_Inside, clampResult15 ) , tex2D( _MatCap_Outside, clampResult15 ) , switchResult38);
			float mulTime57 = _Time.y * _Emission_Speed;
			float clampResult65 = clamp( (0.0 + (( i.uv_texcoord.x + sin( ( ( mulTime57 * -4.0 ) + ( i.uv2_texcoord2.y * -4.0 ) ) ) ) - 1.3) * (1.0 - 0.0) / (2.0 - 1.3)) , 0.0 , 1.0 );
			o.Emission = ( ( _Edge_color * tex2DNode48.r * _Emission_Intensity ) + lerpResult39 + ( clampResult65 * tex2DNode48.g * _Wire_Emission * _Wire_Color ) ).rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = _Opacity;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 customPack1 : TEXCOORD1;
				float2 customPack2 : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
				float3 worldNormal : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv3_texcoord3;
				o.customPack1.xy = v.texcoord2;
				o.customPack1.zw = customInputData.uv_texcoord;
				o.customPack1.zw = v.texcoord;
				o.customPack2.xy = customInputData.uv2_texcoord2;
				o.customPack2.xy = v.texcoord1;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv3_texcoord3 = IN.customPack1.xy;
				surfIN.uv_texcoord = IN.customPack1.zw;
				surfIN.uv2_texcoord2 = IN.customPack2.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
205;73;912;469;-841.8126;1903.616;2.399088;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;50;-1531.09,-926.758;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;2;-1307.243,265.3313;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;44;-1083.059,-662.35;Inherit;False;Constant;_Float12;Float 12;6;0;Create;True;0;0;0;False;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;263.9032,-1704.969;Inherit;False;Property;_Emission_Speed;Emission_Speed;17;0;Create;True;0;0;0;False;0;False;0;0.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;41;-1206.422,-940.1422;Inherit;True;Property;_Normal;Normal;4;0;Create;True;0;0;0;False;0;False;-1;a1a3064659f939440a14a814636de25e;a1a3064659f939440a14a814636de25e;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;43;-1088.059,-746.35;Inherit;False;Constant;_Float4;Float 4;6;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;82;-910.7725,224.0016;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;42;-864.5369,-831.1057;Inherit;True;5;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,1,1;False;3;FLOAT3;0,0,0;False;4;FLOAT3;1,1,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-1003.135,-545.3588;Inherit;False;Property;_Normal_Switch;Normal_Switch;9;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;56;427.1673,-1543.789;Inherit;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;57;478.5388,-1655.871;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;805.4453,-1625.516;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-4;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;46;-520.3874,-818.3715;Inherit;True;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-21.34538,45.15722;Inherit;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;0;False;0;False;0.1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;704.5138,-1475.648;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;-4;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-119.8049,155.6919;Inherit;False;Property;_Transform;Transform;10;0;Create;True;0;0;0;False;0;False;0;1;-0.1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-233.7421,-219.0663;Inherit;True;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;111;-369.5836,442.513;Inherit;False;Property;_Offset_Speed;Offset_Speed;6;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;59;982.9091,-1564.804;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TransformDirectionNode;9;-504.8375,-480.2526;Inherit;True;World;View;False;Fast;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;21;180.6439,42.23578;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;10;-205.4577,-472.7639;Inherit;True;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;107;-182.1145,448.0214;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;23;362.935,10.9105;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;60;1235.094,-1522.773;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;110;-328.4334,857.0037;Inherit;False;Property;_Noise_Scale;Noise_Scale;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;104;-318.846,674.4678;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;62;1147.622,-1770.291;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;63;1444.55,-1610.407;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;25;-76.54066,1106.624;Inherit;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TFHCRemapNode;14;128.7769,-465.8066;Inherit;True;5;0;FLOAT2;0,0;False;1;FLOAT2;-1,-1;False;2;FLOAT2;1,1;False;3;FLOAT2;0,0;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SinOpNode;109;27.71725,445.4669;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;24;647.0182,14.62987;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;105;-25.90703,671.2784;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;20,20;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;32;894.6919,50.92593;Inherit;False;Transform;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;15;423.0275,-455.6967;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;64;1794.953,-1575.561;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;1.3;False;2;FLOAT;2;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;163.0016,323.8504;Inherit;False;Property;_Offset_Intensity;Offset_Intensity;5;0;Create;True;0;0;0;False;0;False;0;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;49;399.8623,-964.1899;Inherit;False;2;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TransformPositionNode;84;232.4198,1092.251;Inherit;False;World;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;320.3095,491.6425;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;33;590.7555,1018.099;Inherit;False;32;Transform;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;68;2085.341,-941.7623;Inherit;False;Property;_Wire_Color;Wire_Color;13;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,1,0.3058825,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;30;600.1026,907.0034;Inherit;False;Constant;_Float3;Float 3;5;0;Create;False;0;0;0;False;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;830.9981,-726.8826;Inherit;False;Property;_Emission_Intensity;Emission_Intensity;16;0;Create;True;0;0;0;False;0;False;0;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;74;700.7129,-473.9433;Inherit;True;Property;_MatCap_Inside;MatCap_Inside;2;0;Create;True;0;0;0;False;0;False;-1;ecee5a680c27252428f7fc95ca8e50dc;a6eee83a259b9db409fe29157d8e74b7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;67;2086.993,-1035.938;Inherit;False;Property;_Wire_Emission;Wire_Emission;14;0;Create;True;0;0;0;False;0;False;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;51;789.8123,-1175.08;Inherit;False;Property;_Edge_color;Edge_color;15;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0.550797,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;34;587.6961,502.1418;Inherit;False;32;Transform;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;540.3479,656.8852;Inherit;False;Property;_Shape_Scale;Shape_Scale;7;0;Create;True;0;0;0;False;0;False;0;0.275;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;65;2176.921,-1422.884;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;48;730.882,-971.0572;Inherit;True;Property;_Shield_Emission;Shield_Emission;3;0;Create;True;0;0;0;False;0;False;-1;cb701408f27c1bd408164f81eb24431b;cb701408f27c1bd408164f81eb24431b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CustomExpressionNode;27;592.2538,785.9139;Float;False;float dotLeft = dot( a, b )@$float dotRight = dot( b , b )@$float4 retVec = a - (dotLeft/dotRight) * b@$return retVec@;4;Create;2;False;a;FLOAT4;0,0,0,0;In;;Float;False;False;b;FLOAT4;0,0,0,0;In;;Float;False;VectorRejection;True;False;0;;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;564.7953,385.4066;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;73;705.8151,-247.6628;Inherit;True;Property;_MatCap_Outside;MatCap_Outside;0;0;Create;True;0;0;0;False;0;False;-1;a6eee83a259b9db409fe29157d8e74b7;ecee5a680c27252428f7fc95ca8e50dc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SwitchByFaceNode;38;1026.005,-355.1285;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;1211.36,-990.9556;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;841.8585,722.1974;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;833.6154,326.5855;Inherit;True;3;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;2355.487,-1171.113;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;39;1315.879,-465.6714;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TransformDirectionNode;26;223.9382,791.9242;Inherit;True;World;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;31;1090.571,405.5279;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;2472.71,-954.6025;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;112;564.8273,256.8322;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;113;2808.81,-965.8355;Inherit;False;Property;_Smoothness;Smoothness;12;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;2816.872,-870.9137;Inherit;False;Property;_Opacity;Opacity;11;0;Create;True;0;0;0;False;0;False;0;0.429;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3089.222,-1079.725;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Shield;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Background;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;41;1;50;0
WireConnection;82;0;2;0
WireConnection;42;0;41;0
WireConnection;42;1;43;0
WireConnection;42;3;44;0
WireConnection;57;0;114;0
WireConnection;58;0;57;0
WireConnection;46;0;42;0
WireConnection;46;1;82;0
WireConnection;46;2;47;0
WireConnection;61;0;56;2
WireConnection;59;0;58;0
WireConnection;59;1;61;0
WireConnection;9;0;46;0
WireConnection;21;0;7;0
WireConnection;21;1;22;0
WireConnection;10;0;9;0
WireConnection;107;0;111;0
WireConnection;23;0;5;2
WireConnection;23;1;22;0
WireConnection;23;2;21;0
WireConnection;60;0;59;0
WireConnection;63;0;62;1
WireConnection;63;1;60;0
WireConnection;14;0;10;0
WireConnection;109;0;107;0
WireConnection;24;0;23;0
WireConnection;105;0;104;0
WireConnection;105;1;110;0
WireConnection;32;0;24;0
WireConnection;15;0;14;0
WireConnection;64;0;63;0
WireConnection;84;0;25;0
WireConnection;97;0;109;0
WireConnection;97;1;105;0
WireConnection;74;1;15;0
WireConnection;65;0;64;0
WireConnection;48;1;49;0
WireConnection;27;0;84;0
WireConnection;27;1;82;0
WireConnection;98;0;3;0
WireConnection;98;1;97;0
WireConnection;73;1;15;0
WireConnection;52;0;51;0
WireConnection;52;1;48;1
WireConnection;52;2;53;0
WireConnection;28;0;29;0
WireConnection;28;1;27;0
WireConnection;28;2;30;0
WireConnection;28;3;33;0
WireConnection;4;0;82;0
WireConnection;4;1;98;0
WireConnection;4;2;34;0
WireConnection;66;0;65;0
WireConnection;66;1;48;2
WireConnection;66;2;67;0
WireConnection;66;3;68;0
WireConnection;39;0;74;0
WireConnection;39;1;73;0
WireConnection;39;2;38;0
WireConnection;26;0;2;0
WireConnection;31;0;4;0
WireConnection;31;1;28;0
WireConnection;54;0;52;0
WireConnection;54;1;39;0
WireConnection;54;2;66;0
WireConnection;112;0;3;0
WireConnection;112;1;97;0
WireConnection;0;2;54;0
WireConnection;0;4;113;0
WireConnection;0;9;72;0
WireConnection;0;11;31;0
ASEEND*/
//CHKSM=B4D4B79E5E99F98D7A0365676D1D2B4C4111DF44