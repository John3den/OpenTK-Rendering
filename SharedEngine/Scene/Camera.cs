using OpenTK.Mathematics;

namespace Engine
{
    public class Camera
    {
        private Vector3 _position = new Vector3(0.0f, 0.0f, 30.0f);
        private float _yaw;
        private float _pitch = 0;
        public float Speed { get; private set; }
        public float Sensitivity { get; private set; }
        public Vector3 Up { get; private set; }
        public Vector3 Front { get; set; }
        private const float MAX_YAW = 360.0f;
        private const float MIN_YAW = 0.0f;
        public float Yaw
        {
            get { return _yaw; }
            set
            {
                if (value > MAX_YAW)
                {
                    _yaw = value - MAX_YAW;
                }
                else if (value < MIN_YAW)
                {
                    _yaw = value + MAX_YAW;
                }
                else
                {
                    _yaw = value;
                }
            }
        }
        private const float MAX_PITCH = 90;
        private const float MIN_PITCH = -90f;
        private const float EPSILON = 1.0f;
        public float Pitch
        {
            get { return _pitch; }
            set
            {
                if (value > MAX_PITCH - EPSILON) 
                {
                    _pitch = MAX_PITCH - EPSILON;
                }
                else if (value < MIN_PITCH + EPSILON)
                {
                    _pitch = MIN_PITCH + EPSILON;
                }
                else
                {
                    _pitch = value;
                }
            }
        }
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
    }
}
