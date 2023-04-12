using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine
{
    public class Renderer
    {
        public readonly Camera _camera;
        public Renderer()
        {
            _camera = new Camera();
        }
        public void RenderScene(RenderBuffer buffer)
        {
            //Rendering actors
            buffer.ActiveShader.Use();
            Actor currentActor = buffer.GetActor();
            int location = GL.GetUniformLocation(buffer.ActiveShader.GetHandle(), "model");
            Matrix4 model;
            while (!buffer.IsEmpty())
            {
                model = currentActor.Transform;
                GL.UniformMatrix4(location, true, ref model);
                GL.DrawElements(PrimitiveType.Triangles, currentActor.GetGeometry().indices.Length, DrawElementsType.UnsignedInt, 0);
                buffer.NextActor();
            }
            location = GL.GetUniformLocation(buffer.ActiveShader.GetHandle(), "isLight");
            GL.Uniform1(location, 1);
            currentActor = buffer.GetLightSource();
            location = GL.GetUniformLocation(buffer.ActiveShader.GetHandle(), "model");
            model = currentActor.Transform;
            GL.UniformMatrix4(location, true, ref model);
            GL.DrawElements(PrimitiveType.Triangles, currentActor.GetGeometry().indices.Length, DrawElementsType.UnsignedInt, 0);

        }
    }
}
