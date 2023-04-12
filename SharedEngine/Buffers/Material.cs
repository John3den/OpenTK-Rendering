using OpenTK.Mathematics;

namespace Engine
{
    public class Material
    {
        public Material(float a, float s,int r, Vector3 clr)
        {
            _color = clr;
            _ambient = a;
            _spec = s;
            _reflectivity = r;
        }
        public Vector3 _color;
        public float _ambient;
        public float _spec;
        public int _reflectivity;
    }
}
