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
    public class RenderBuffer
    {
        private Scene _scene;
        private Actor _actor;
        private int _maxState = 1;
        private int _state = 0;
        public Shader ActiveShader { get; set; }
        
        public void SetUniforms(Camera camera)
        {
            Matrix4.CreateOrthographicOffCenter(0.0f, 800.0f, 0.0f, 600.0f, 0.1f, 1000.0f);
            Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)800 / (float)600, 0.1f, 1000.0f);
            Matrix4 model = Matrix4.CreateTranslation(new Vector3(0, 0, 0));
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 800 / 600, 0.1f, 1000.0f);
            Vector3 cameraTarget = Vector3.Zero;
            Vector3 cameraDirection = Vector3.Normalize(camera.GetPosition() - cameraTarget);
            Vector3 up = Vector3.UnitY;
            Vector3 cameraRight = Vector3.Normalize(Vector3.Cross(up, cameraDirection));
            Vector3 cameraUp = Vector3.Cross(cameraDirection, cameraRight);
            Matrix4 view = Matrix4.LookAt(camera.GetPosition(), camera.GetPosition() + camera.Front, up);

            int location = GL.GetUniformLocation(ActiveShader.GetHandle(), "ourColor");
            GL.Uniform4(location, 0.0f, 1.0f, 0.0f, 1.0f);
            location = GL.GetUniformLocation(ActiveShader.GetHandle(), "camPos");
            GL.Uniform3(location, camera.GetPosition());
            location = GL.GetUniformLocation(ActiveShader.GetHandle(), "model");
            GL.UniformMatrix4(location, true, ref model);
            location = GL.GetUniformLocation(ActiveShader.GetHandle(), "view");
            GL.UniformMatrix4(location, true, ref view);
            location = GL.GetUniformLocation(ActiveShader.GetHandle(), "projection");
            GL.UniformMatrix4(location, true, ref projection);
            location = GL.GetUniformLocation(ActiveShader.GetHandle(), "model");
        }
        public RenderBuffer(Scene scene)
        {
            _scene = scene;
            ActiveShader = _scene._point;
            _maxState = _scene.n;
            _actor = scene._actor;
            SetUniforms(scene._camera);
            _actor.Set();
        }
        public void ChooseShader(int mode)
        {
            switch (mode)
            {
                case 0:
                    ActiveShader = _scene._point;
                    break;
                case 1:
                    ActiveShader = _scene._direct;
                    break;
                case 2:
                    ActiveShader = _scene._spotlight;
                    break;
            }
        }
        public void NextActor()
        {
            _state++;
            _actor.Transform = Matrix4.CreateTranslation(new Vector3(0.03f * (MathF.Floor(_state*10 / 10)) * MathF.Sin((float)_state / 10), 0, 0.03f * (MathF.Floor(_state*10 / 10)) * MathF.Cos((float)_state / 10)));
        }
        public Actor GetActor()
        {
            return _actor;
        }
        public bool IsEmpty()
        {
            return _maxState == _state;
        }

    }
}