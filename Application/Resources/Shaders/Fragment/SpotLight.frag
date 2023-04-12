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
vec4 spotLight()
{
	float outerCone = 0.90f;
	float innerCone = 0.95f;

	vec3 normal = normalize(Normal);
	vec3 lightDirection = normalize(lightPos - crntPos);
	float diffuse = max(dot(normal, lightDirection), 0.0f);

	vec3 viewDirection = normalize(camPos - crntPos);
	vec3 reflectionDirection = reflect(-lightDirection, normal);
	float specAmount = pow(max(dot(viewDirection, reflectionDirection), 0.0f), reflectivity);
	float specular = specAmount * specularLight;

	float angle = dot(vec3(0.0f, -1.0f, 0.0f), -lightDirection);
	float inten = clamp((angle - outerCone) / (innerCone - outerCone), 0.0f, 1.0f);

	return vec4(m_color * (diffuse * inten + ambient + specular * inten), 1.0f);
}
void main()
{
	if(isLight == 1)
	{
		FragColor = vec4(1,1,1,1);
	}
	else
	{
	FragColor = spotLight();
	}
}

