#version 420 core
out vec4 FragColor;


in vec3 Normal;
in vec3 crntPos;

uniform vec3 camPos;
uniform vec3 lightPos;
uniform int isLight;

uniform float ambient;
uniform float specularLight;
uniform vec3 m_color;
uniform int reflectivity;
vec4 pointLight()
{	
	vec3 lightVec = lightPos - crntPos;

	float dist = length(lightVec);
	float a = 0.1;
	float b = 0.4;
	float inten =1.0f / (a * dist * dist + b * dist + 1.0f);



	vec3 normal = normalize(Normal);
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
	if(isLight == 1)
	{
		FragColor = vec4(1,1,1,1);
	}
	else
	{
		FragColor = pointLight();
	}
}
