using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class VAO : GLObject
    {
        public VAO() 
        {
            _handle = GL.GenVertexArray();
            Bind();
        }
        public override void Bind()
        {
            GL.BindVertexArray(_handle);
        }
    }
}
