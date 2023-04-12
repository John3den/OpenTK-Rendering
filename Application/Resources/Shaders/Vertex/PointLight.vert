#version 420 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;


out vec4 clr;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec3 camPos;
uniform vec3 lightPos;

uniform float ambient;
uniform float specularLight;
uniform vec3 m_color;
uniform int reflectivity;
vec4 pointLight()
{	
	vec3 crntPos = vec3(vec4(aPos, 1.0) * model);

	vec3 lightVec = lightPos - crntPos;

	float dist = length(lightVec);
	float a = 3.0;
	float b = 0.7;
	float inten =10.0f / (a * dist * dist/2 + b * dist + 1.0f);



	vec3 normal = normalize(aNormal);
	vec3 lightDirection = normalize(lightVec);
	float diffuse = max(dot(normal, lightDirection), 0.0f);

	vec3 viewDirection = normalize(camPos - crntPos);	
	vec3 reflectionDirection = reflect(-lightDirection, normal);
	float specAmount = pow(max(dot(viewDirection, reflectionDirection), 0.0f),reflectivity);
	float specular = specAmount * specularLight;


	return vec4(m_color*((specular + diffuse)*inten + ambient) ,1.0f) ;
}


void main()
{
	gl_Position =  vec4(aPos, 1.0) * model * view * projection;
	clr = pointLight();
}
