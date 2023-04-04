#version 420 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;


out vec4 color;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec3 camPos;

vec4 pointLight()
{	
	vec3 crntPos = vec3(vec4(aPos, 1.0) * model);
    vec3 Normal = (vec4(aNormal, 1.0f)*model).xyz;

	vec3 ObjectColor = vec3(1,0,0);
	vec3 lightPos = vec3(10,10,0);
	vec3 lightVec = lightPos - crntPos;

	float dist = length(lightVec)/1000.0f;
	float a = 3.0;
	float b = 0.7;
	float inten = 1.0f / (a * dist * dist + b * dist + 1.0f);


	float ambient = 0.20f;


	vec3 normal = normalize(Normal);
	vec3 lightDirection = normalize(lightVec);
	float diffuse = max(dot(normal, lightDirection), 0.0f);


	float specularLight = 0.10f;
	vec3 viewDirection = normalize(camPos - crntPos);	
	vec3 reflectionDirection = reflect(-lightDirection, normal);
	float specAmount = pow(max(dot(viewDirection, reflectionDirection), 0.0f),2);
	float specular = specAmount * specularLight;

	return vec4(ObjectColor*(specular+diffuse+ambient) ,1.0f) ;
}


void main()
{
	gl_Position =  vec4(aPos, 1.0) * model * view * projection;
	color = pointLight();
}
