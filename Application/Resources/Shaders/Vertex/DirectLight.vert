#version 420 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;

out vec4 clr;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 rotation;

uniform vec3 camPos;
uniform float ambient;
uniform float specularLight;
uniform int reflectivity;
uniform vec3 m_color;
uniform int isLight;
vec4 directLight()
{	
	vec3 crntPos = vec3(vec4(aPos, 1.0) * model);
	vec3 Normal = vec3(vec4(aNormal,1.0f)*rotation);
	vec3 normal = normalize(Normal);
	vec3 lightDirection = normalize(vec3(1.0f, 1.0f, 0.0f));
	float diffuse = max(dot(normal, lightDirection), 0.0f);
	vec3 viewDirection = normalize(camPos - crntPos);
	vec3 reflectionDirection = reflect(-lightDirection, normal);
	float specAmount = pow(max(dot(viewDirection, reflectionDirection), 0.0f), reflectivity);
	float specular = specAmount * specularLight;
	return vec4(m_color*(diffuse + specular + ambient),1.0f);
}


void main()
{
	gl_Position =  vec4(aPos, 1.0) * model * view * projection;
	if(isLight == 1)
	{
	clr = vec4(1,1,1,1);
	}
	else
	{
	clr = directLight();
	}
}
