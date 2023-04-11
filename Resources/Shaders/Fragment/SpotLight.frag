#version 420 core
out vec4 FragColor;

in vec3 color;
in vec3 Normal;
in vec3 crntPos;

uniform vec3 camPos;


vec4 spotLight()
{
	vec3 lightPos = vec3(0,10,0);
	float outerCone = 0.90f;
	float innerCone = 0.95f;
	float ambient = 0.20f;

	vec3 normal = normalize(Normal);
	vec3 lightDirection = normalize(lightPos - crntPos);
	float diffuse = max(dot(normal, lightDirection), 0.0f);

	float specularLight = 0.50f;
	vec3 viewDirection = normalize(camPos - crntPos);
	vec3 reflectionDirection = reflect(-lightDirection, normal);
	float specAmount = pow(max(dot(viewDirection, reflectionDirection), 0.0f), 16);
	float specular = specAmount * specularLight;

	float angle = dot(vec3(0.0f, -1.0f, 0.0f), -lightDirection);
	float inten = clamp((angle - outerCone) / (innerCone - outerCone), 0.0f, 1.0f);

	return vec4(color * (diffuse * inten + ambient + specular * inten), 1.0f);
}
void main()
{
	FragColor = spotLight();
}

