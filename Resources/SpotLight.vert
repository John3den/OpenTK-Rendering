#version 420 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;


out vec4 color;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec3 camPos;

vec4 spotLight()
{	
	vec3 crntPos = vec3(vec4(aPos, 1.0) * model);
	vec3 ObjectColor = vec3(1,0,0);

	vec3 lightPos = vec3(0,10,0);
	float outerCone = 0.90f;
	float innerCone = 0.95f;
	float ambient = 0.20f;

	vec3 normal = normalize(aNormal);
	vec3 lightDirection = normalize(lightPos - crntPos);
	float diffuse = max(dot(normal, lightDirection), 0.0f);

	float specularLight = 0.50f;
	vec3 viewDirection = normalize(camPos - crntPos);
	vec3 reflectionDirection = reflect(-lightDirection, normal);
	float specAmount = pow(max(dot(viewDirection, reflectionDirection), 0.0f), 16);
	float specular = specAmount * specularLight;

	float angle = dot(vec3(0.0f, -1.0f, 0.0f), -lightDirection);
	float inten = clamp((angle - outerCone) / (innerCone - outerCone), 0.0f, 1.0f);

	return vec4(ObjectColor * (diffuse * inten + ambient + specular * inten), 1.0f);
}


void main()
{
	gl_Position =  vec4(aPos, 1.0) * model * view * projection;
	color = spotLight();
}
