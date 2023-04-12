#version 420 core
out vec4 FragColor;

in vec4 clr;
uniform int isLight;
void main()
{
	if(isLight == 1)
	{
		FragColor = vec4(1,1,1,1);
	}
	else
	{
	FragColor = clr;
	}
}
