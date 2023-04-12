#version 420 core
out vec4 FragColor;


uniform float ambient;
uniform float specularLight;
uniform vec3 m_color;
uniform int reflectivity;

in vec3 Normal;
in vec3 crntPos;

uniform vec3 camPos;
uniform int isLight;

vec4 directLight()
{
	// ambient lighting

	// diffuse lighting
	vec3 normal = normalize(Normal);
	vec3 lightDirection = normalize(vec3(1.0f, 1.0f, 0.0f));
	float diffuse = max(dot(normal, lightDirection), 0.0f);

	// specular lighting
	vec3 viewDirection = normalize(camPos - crntPos);
	vec3 reflectionDirection = reflect(-lightDirection, normal);
	float specAmount = pow(max(dot(viewDirection, reflectionDirection), 0.0f), reflectivity);
	float specular = specAmount * specularLight;
	return vec4(m_color*(diffuse + specular +ambient ),1.0f);
}

void main()
{
	FragColor =directLight();
}


