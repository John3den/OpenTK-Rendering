#version 420 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;


out vec4 color;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec3 camPos;

vec4 directLight()
{	
	vec3 crntPos = vec3(vec4(aPos, 1.0) * model);
    vec3 Normal = (vec4(aNormal, 1.0f)*model).xyz;

	vec3 ObjectColor = vec3(1,0,0);
	// ambient lighting
	float ambient = 0.20f;

	// diffuse lighting
	vec3 normal = normalize(Normal);
	vec3 lightDirection = normalize(vec3(1.0f, 1.0f, 0.0f));
	float diffuse = max(dot(normal, lightDirection), 0.0f);

	// specular lighting
	float specularLight = 0.50f;
	vec3 viewDirection = normalize(camPos - crntPos);
	vec3 reflectionDirection = reflect(-lightDirection, normal);
	float specAmount = pow(max(dot(viewDirection, reflectionDirection), 0.0f), 16);
	float specular = specAmount * specularLight;

	return vec4(ObjectColor*(diffuse + specular + ambient),1.0f);
}


void main()
{
	gl_Position =  vec4(aPos, 1.0) * model * view * projection;
	color = directLight();
}
