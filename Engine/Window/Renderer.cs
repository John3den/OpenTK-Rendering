using Dear_ImGui_Sample;
using Engine;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using ImGuiNET;
using System.Timers;
using System.Linq.Expressions;

namespace Engine
{
    public class Renderer
    {
       
        public void RenderScene(RenderBuffer buffer)
        {
            Actor currentActor = buffer.GetActor();
            int location = GL.GetUniformLocation(buffer.ActiveShader.Handle, "model");
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
