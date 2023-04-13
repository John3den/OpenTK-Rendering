#version 420 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;





out vec3 Normal;
out vec3 crntPos;
out float amb;
out float spe;
out vec3 col;
out int refl;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 rotation;

uniform float ambient;
uniform float specularLight;
uniform vec3 m_color;
uniform int reflectivity;
void main()
{
	col = m_color;
	refl = reflectivity;
	amb = ambient;
	spe = specularLight;
	crntPos = vec3(vec4(aPos, 1.0) * model);
	gl_Position =  vec4(aPos, 1.0) * model * view * projection;
	Normal = vec3(vec4(aNormal, 1.0f)* rotation);
}