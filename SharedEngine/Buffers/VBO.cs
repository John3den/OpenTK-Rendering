using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class VBO : GLObject
    {
        public VBO(Geometry geometry)
        {
            _handle = GL.GenBuffer();
            Bind();
            GL.BufferData(BufferTarget.ArrayBuffer, geometry.vertices.Length * sizeof(float), geometry.vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        }
        public override void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _handle);
        }
    }
}
