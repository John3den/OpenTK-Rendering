﻿
using OpenTK.Mathematics;
using System.Reflection;

namespace Engine
{
    public class Scene
    {
        public readonly Shader _point;
        public readonly Shader _direct;
        public readonly Shader _spotlight;
        public readonly Shader _light;
        private int _lightMode = 0;
        public readonly Actor _actor;
        public readonly Actor _lightSource;
        public readonly Camera _camera;
        public const int N = 100000;
        private int _n = 1;
        private const int LIGHTMODES = 3;
        private Timer _timer;
        private float _totalTime = 0;
        public float CurrentTime()
        {
            return _totalTime;
        }
        public void UpdateTotalTime(float delta)
        {
            _totalTime += delta;
        }
        public int n
        {
            get { return _n; }
            set { if (value >= 0) _n = value; }
        }

        public int GetLightMode()
        {
            return _lightMode;
        }
        public void NextObjectCap()
        {
            n = (n + 1) % N;
        }
        public void ResetLightMode()
        {
            _lightMode = 0;
        }
        public void NextLightMode()
        {
            _lightMode = (_lightMode + 1) % LIGHTMODES;
        }
        public float GetElapsedTime()
        {
            _timer.Stop();
            _timer.Start();
            return _timer.Get();
        }
        public Scene(Actor actor, Camera camera,Shader point, Shader direct,Shader spotlight)
        {
            _lightSource = new Actor(new Geometry(1));
            _lightSource.Transform = Matrix4.CreateTranslation(0,3,0);
            _light = new Shader("../../../Resources\\Shaders\\Vertex\\Generic.vert", "../../../Resources\\Shaders\\Fragment\\PointLight.frag");
            _timer = new Timer();
            _point = point;
            _direct = direct;  
            _spotlight = spotlight;
            _actor = actor;
            _camera = camera; 
        }
    }
}
