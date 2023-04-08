using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Scene
    {
        public readonly Shader _point;
        public readonly Shader _direct;
        public readonly Shader _spotlight;
        private Timer _timer;
        public float GetElapsedTime()
        {
            _timer.Stop();
            _timer.Start();
            return _timer.Get();
        }
        public Scene(Actor actor, Camera camera,Shader point, Shader direct,Shader spotlight)
        {
            _timer = new Timer();
            _point = point;
            _direct = direct;  
            _spotlight = spotlight;
            _actor = actor;
            _camera = camera; 
        }
        public Actor _actor;
        public Camera _camera;
        public int lightMode = 0;
        public int N = 10000;
        public int n = 1;
    }
}
