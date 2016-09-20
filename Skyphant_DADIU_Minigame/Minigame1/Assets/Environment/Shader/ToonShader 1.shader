Shader "Custom/ToonShader 1" {
	Properties{
		//Underscored variables can be adjusted within unity
		
		//The two color variables for lit and unlit parts of the object 
		_FrontColor ("Front Color", Color) = (1,5,1,1)
		_BackColor ("Back Color", Color) = (0.5,0.5,0.5,1)
		
		//Control the line between the shading (Lit and Unlit)
		_DiffThreshold("Lighting Threshold", Range(-1.1,1)) = 0.1
		//How blurry the edge between lit and unlit is
		_Diff ("Diffuse", Range (0,0.99))=0.0
		
		//What color the specular highlight is
		_SpecColor ("Specular Color", Color) = (1,1,1,1)
		//To define the strength of the specular highlight
		_Shininess ("Shininess", Range(0.5,1)) = 1
		//How blurry the edge between the specular highlight and the rest of the lit area is
		_SpecDiff ("Specular Diffusion", Range (0,0.99))= 0.0
		
		//The outline properties, color, thickness and blurryness/diffusion
		_OutColor ("Outline Color", Color) = (0,0,0,1)
		_OutThickness ("Outline Thickness", Range(0,1))= 0.1
		_OutDiff ("Outline Diffusion", Range(0,1))= 0.0
		
	}
	
	//In subshaders we can edit what the shader does
	SubShader{
		Pass {
			Tags {"LightMode" = "ForwardBase"}
			
			//Start the CG program
			CGPROGRAM
			
			//Progress - Assign some initial variables
			//A pragma is an instruction - Tells unity what to look for and where
			//So the pragma vertex looks for the vert function we created (if we called it bob
			//the pragma should say #pragma vertex bob
			#pragma vertex vert
			#pragma fragment frag
			
			
			//user defined variables - uniform keyword is CG specific (not required in unity)
			// These just create all the variables we assigned in the properties. 
			// Some are colors (fixed4), and some are numbers we use to adjust the defferent variables(fixed, half).
			//The colors to lit and unlit areas:
			uniform fixed4 _FrontColor;
			uniform fixed4 _BackColor;
			
			//The threshold(Where the lit and unlit separates, and the diffusion of it
			uniform fixed _DiffThreshold;
			uniform fixed _Diff;
			
			//The specular highlight variables
			uniform fixed4 _SpecColor;
			uniform fixed _Shininess;
			uniform half _SpecDiff;
			
			//The outline variables
			uniform fixed4 _OutColor;
			uniform fixed _OutThickness;
			uniform fixed _OutDiff;
			
			//Unity defined variables
			uniform half4 _LightColor0;
			
			
			//base input structs - used as small base classes
			//A struct to take the vertex inputs
			struct vertexInput{
				//Our object comes with a whole bunch of semantics attached to it such as:
				//tangents, vertex POSITION, normals, texture coordinates, UV maps
				half4 vertex : POSITION;
				half3 normal : NORMAL;
			};
			
			struct vertexOutput{
				//used to hold POSITION from the input vertex when it has
				//been modified in the Vertex Function below
				half4 pos : SV_POSITION;
				fixed3 normalDir : TEXCOORD0;
				fixed4 lightDir : TEXCOORD1;
				fixed3 viewDir : TEXCOORD2;			
			};
			
			//Vertex function
			vertexOutput vert(vertexInput v){
				vertexOutput o;
				
				o.normalDir = normalize( mul(half4(v.normal,0.0), _World2Object ).xyz);
				//Taking a vertex position, multiplying it into the UNITY_MATRIX_MVP
				//Basically moving the vertex position we get from the object into the unity matrix position
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				//The above line explained:
				//UNITY_MATRIX_MVP xyzw = a coordinate
				//v.vertex xyzw = a coordinate
				
				//The matrix has 4 float4's inside it which we multiply with with the v.vertex coordinates 
				//to get a single float4 vector:
				//UNITY_MATRIX_MVP1.xyzw * v.vertex.x
				//UNITY_MATRIX_MVP2.xyzw * v.vertex.y
				//UNITY_MATRIX_MVP3.xyzw * v.vertex.z
				//UNITY_MATRIX_MVP4.xyzw * v.vertex.w
				
				//world position
				half4 posWorld = mul(_Object2World, v.vertex);
				//view direction
				o.viewDir = normalize (_WorldSpaceCameraPos.xyz - posWorld.xyz);
				//light direction
				half3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - posWorld.xyz;
				o.lightDir = fixed4(
					normalize(lerp(_WorldSpaceLightPos0.xyz , fragmentToLightSource, _WorldSpaceLightPos0.w)),
					lerp(1.0,1.0/length(fragmentToLightSource), _WorldSpaceLightPos0.w)
				);
				
				
				return o;
			}
			
			
			//fragment function
			fixed4 frag(vertexOutput i) : COLOR
			{
				//Lighting
				//Dot product
				fixed nDotL = saturate(dot(i.normalDir, i.lightDir.xyz));
				
				
				// Cutoff is masking (0-1 value) - hence the keyword "saturate"
				// We take the maximum of the diffuse threshold vs. the nDotL and returns the highest
				// The we multiply it by 2-diffusion to the power of 10 to get a stronger value
				// and the diffusion will then soften if by blurr
				fixed diffuseCutoff = saturate( (max(_DiffThreshold, nDotL) - _DiffThreshold)*pow((2-_Diff), 10) );
				fixed specularCutoff = saturate( (max(_Shininess, dot(reflect(-i.lightDir.xyz, i.normalDir), i.viewDir))-_Shininess)*pow((2-_SpecDiff),10 ));
				
				
				//calculate outlines' thickness and diffusion:
				//fixed outlineStrength = saturate ((dot(i.normalDir, i.viewDir) -_OutThickness)*pow(  (2-_OutDiff), 10) + _OutThickness );
				//To color the outline:
				//fixed3 outlineOverlay = (_OutColor.xyz * (1-outlineStrength)) + outlineStrength;
				
				
				//This sets the color of the unlit area defined by the BackColor and the line made by diffuse Cutoff
				fixed3 ambientLight = (1-diffuseCutoff) * _BackColor.xyz;
				
				//Color the area where the light hits on the object
				fixed3 diffuseReflection = (1-specularCutoff) * _FrontColor.xyz * diffuseCutoff;
				
				//The highligt of a surface - what shines when light hits it. The shiny highlight of a surface is called specularity
				fixed3 specularReflection = _SpecColor.xyz * specularCutoff;
				
				//All the calculations to be outputted in unity and adjusted in the interface put together
				fixed3 lightFinal = (ambientLight + diffuseReflection) + specularReflection;
				
				//When it is a fixed4 we need 4 values to return, in this case we return a fixed3(lightfinal) and a number(1.0)
				return fixed4(lightFinal, 1.0);
				
			}
			
			//End the CG program
			ENDCG
		}
	}
	//fallback comment out during development
	//Fallback is a diffuse that will run if no other shader can be found
	//Fallback "Specular"
}
