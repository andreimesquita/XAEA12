Shader "Custom/CurvedWorld" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
 
        CGPROGRAM
        #pragma surface surf Lambert vertex:vert addshadow
 
        uniform sampler2D _MainTex;
        
        // Global variables
        uniform float4 CHARACTER_WORLD_POS = float4(0.0f, 0.0f, 0.0f, 0.0f);
        uniform float MIN_DISPLACEMENT_DISTANCE = 0.0f;
        uniform float CURVATURE = 0.0007f;
 
        struct Input {
            float2 uv_MainTex : TEXCOORD0;
        };
 
        void vert(inout appdata_full v) {
            // Transform the vertex coordinates from model space into world space
            float4 vv = mul( unity_ObjectToWorld, v.vertex );
 
            // Now adjust the coordinates to be relative to the camera position
            vv.xyz -= CHARACTER_WORLD_POS.xyz;
 
            // Reduce the y coordinate (i.e. lower the "height") of each vertex based
            // on the square of the distance from the camera in the z axis, multiplied
            // by the chosen curvature factor
            float distance = vv.z * vv.z;
            distance -= MIN_DISPLACEMENT_DISTANCE;
            distance = max(distance, 0.0f);
            float displacedY = distance * - CURVATURE;
            vv = float4( 0.0f, displacedY, 0.0f, 0.0f );
 
            // Now apply the offset back to the vertices in model space
            v.vertex += mul(unity_WorldToObject, vv);
        }
 
        void surf (Input IN, inout SurfaceOutput o) {
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
}