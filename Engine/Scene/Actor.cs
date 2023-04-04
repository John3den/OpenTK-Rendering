using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Actor
    {
        public Actor(Geometry geo)
        {
            _geometry = geo;
            _vao = new VAO();
            _vbo = new VBO(geo);
            _ebo = new EBO(geo);
        }
        private Matrix4 _transform;
        private Geometry _geometry;
        private VAO _vao;
        private VBO _vbo;
        private EBO _ebo;

        public Geometry GetGeometry()
        {
            return _geometry;
        }
        public Matrix4 Transform 
        { 
            get { return _transform; }
            set { _transform = value; }
        }
        public void Set()
        {
            _vao.Bind();
            _vbo.Bind();
            _ebo.Bind();
        }
    }
}
