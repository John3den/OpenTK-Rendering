using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine
{
    public class Renderer
    {
       
        public void RenderScene(RenderBuffer buffer)
        {
            buffer.ActiveShader.Use();
            Actor currentActor = buffer.GetActor();
            int location = GL.GetUniformLocation(buffer.ActiveShader.GetHandle(), "model");
            while (!buffer.IsEmpty())
            {
                Matrix4 model = currentActor.Transform;
                GL.UniformMatrix4(location, true, ref model);
                GL.DrawElements(PrimitiveType.Triangles, currentActor.GetGeometry().indices.Length, DrawElementsType.UnsignedInt, 0);
                buffer.NextActor();
            }
        }
    }
}
