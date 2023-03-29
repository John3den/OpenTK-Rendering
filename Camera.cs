using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Camera
    {
        public Vector3 position = new Vector3(0.0f, 0.0f, 30.0f);
        public Vector3 front = new Vector3(0.0f, 1.0f, -1.0f);
        public Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
        public Vector2 lastPos;
        public float Yaw = -90;
        public float pitch = 0;
        public float Sensitivity = 0.1f;
        public float speed = 15f;
    }
}
