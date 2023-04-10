
namespace Engine
{
    public class Scene
    {
        public readonly Shader _point;
        public readonly Shader _direct;
        public readonly Shader _spotlight;
        private int _lightMode = 0;
        public readonly Actor _actor;
        public readonly Camera _camera;
        public const int N = 10000;
        private int _n = 1;
        public int n
        {
            get { return _n; }
            set { if (value >= 0) _n = value; }
        }
        private Timer _timer;
        public int GetLightMode()
        {
            return _lightMode;
        }
        public void NextObjectCap()
        {
            n = (n + 1) % N;
        }
        public void NextLightMode()
        {
            _lightMode = (_lightMode + 1) % 3;
        }
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
    }
}
