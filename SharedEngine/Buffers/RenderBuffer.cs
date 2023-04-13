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
        private Actor _lightSource;
        private Material _currentMaterial;
        private int _maxState = 1;
        private int _state = 0;
        private const float DENSITY = 0.01f;
        private const float MAX_RENDER_DISTANCE = 1000.0f;
        private const float MIN_RENDER_DISTANCE = 0.1f;
        private const int RESOLUTION_X = 800;
        private const int RESOLUTION_Y = 600;
        public Shader ActiveShader { get; set; }
        
        public void SetUniforms(Camera camera)
        {
            Matrix4 model = Matrix4.CreateTranslation(new Vector3(0, 0, 0));
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), RESOLUTION_X / RESOLUTION_Y, MIN_RENDER_DISTANCE, MAX_RENDER_DISTANCE);
            Vector3 up = Vector3.UnitY;
            Matrix4 view = Matrix4.LookAt(camera.GetPosition(), camera.GetPosition() + camera.Front, up);
            Matrix4 rotation = Matrix4.CreateRotationY(_scene.CurrentTime());

            int location = GL.GetUniformLocation(ActiveShader.GetHandle(), "isLight");
            GL.Uniform1(location,0);
            location = GL.GetUniformLocation(ActiveShader.GetHandle(), "camPos");
            GL.Uniform3(location, camera.GetPosition());
            location = GL.GetUniformLocation(ActiveShader.GetHandle(), "lightPos");
            GL.Uniform3(location, GetLightSource().Transform.ExtractTranslation());
            location = GL.GetUniformLocation(ActiveShader.GetHandle(), "model");
            GL.UniformMatrix4(location, true, ref model);
            location = GL.GetUniformLocation(ActiveShader.GetHandle(), "view");
            GL.UniformMatrix4(location, true, ref view);
            location = GL.GetUniformLocation(ActiveShader.GetHandle(), "projection");
            GL.UniformMatrix4(location, true, ref projection);
            location = GL.GetUniformLocation(ActiveShader.GetHandle(), "rotation");
            GL.UniformMatrix4(location, true, ref rotation);
            //material
            location = GL.GetUniformLocation(ActiveShader.GetHandle(), "specularLight");
            GL.Uniform1(location, _currentMaterial._spec);
            location = GL.GetUniformLocation(ActiveShader.GetHandle(), "ambient");
            GL.Uniform1(location, _currentMaterial._ambient);
            location = GL.GetUniformLocation(ActiveShader.GetHandle(), "m_color");
            GL.Uniform3(location, _currentMaterial._color);
            location = GL.GetUniformLocation(ActiveShader.GetHandle(), "reflectivity");
            GL.Uniform1(location, _currentMaterial._reflectivity);
        }
        public RenderBuffer(Scene scene,Material mat)
        {
            _currentMaterial = mat;
            _scene = scene;
            _lightSource = scene._lightSource;
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
        public void ActivateLightShader()
        {
            ActiveShader = _scene._light;
        }
        public void NextActor()
        {
            _state++;
            float radius = MathF.Floor(_state * 10 / 10);
            float angle = (float)_state / 10;
            _actor.Transform =Matrix4.CreateRotationY(_scene.CurrentTime()) * Matrix4.CreateTranslation(new Vector3(DENSITY * (radius) * MathF.Sin(angle),
                                                                     0,
                                                                     DENSITY * (radius) * MathF.Cos(angle)));
        }
        public Actor GetActor()
        {
            return _actor;
        }
        public Actor GetLightSource()
        {
            return _lightSource;
        }
        public bool IsEmpty()
        {
            return _maxState == _state;
        }

    }
}