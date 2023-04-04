using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Camera
    {
        public Camera()
        {
            Up = new Vector3(0.0f, 1.0f, 0.0f);
            Front = new Vector3(0.0f, 1.0f, -1.0f);
            Yaw = -90;
            Pitch = 0;
            Speed = 15f;
            Sensitivity = 0.1f;
        }
        public void UpdateDirection()
        {
            float X = (float)Math.Cos(MathHelper.DegreesToRadians(Pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(Yaw));
            float Y = (float)Math.Sin(MathHelper.DegreesToRadians(Pitch));
            float Z = (float)Math.Cos(MathHelper.DegreesToRadians(Pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(Yaw));
            Front = Vector3.Normalize(new Vector3(X, Y, Z));
        }
        public void Move(Vector3 delta)
        {
            _position += delta;
        }
        public Vector3 GetPosition()
        {
            return _position;
        }
        private Vector3 _position = new Vector3(0.0f, 0.0f, 30.0f);
        private float _yaw;
        private float _pitch = 0;
        public float Speed { get; private set; }
        public float Sensitivity { get; private set; }
        public Vector3 Up { get; private set; }
        public Vector3 Front { get; set; }
        public float Yaw
        {
            get { return _yaw; }
            set
            {
                if (value > 360)
                {
                    _yaw = value - 360;
                }
                else if(value < 0)
                {
                    _yaw = value + 360;
                }
                else
                {
                    _yaw = value;
                }
            }
        }
        public float Pitch
        {
            get { return _pitch; }
            set
            {
                if (value > 89)
                {
                    _pitch = 89;
                }
                else if (value < -89)
                {
                    _pitch = -89;
                }
                else
                {
                    _pitch = value;
                }
            }
        }
    }
}
