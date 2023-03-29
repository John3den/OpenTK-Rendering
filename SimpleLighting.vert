#version 420 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;


out vec3 color;

out vec3 Normal;

out vec3 crntPos;


uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
	crntPos = vec3(model * vec4(aPos, 1.0f));
	gl_Position =  vec4(aPos, 1.0) * model * view * projection;

	color = vec3(1,0,0);
	Normal = aNormal;
}