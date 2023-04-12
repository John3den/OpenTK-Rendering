using System.Diagnostics;

namespace Engine
{
    public class Timer
    {
        private Stopwatch _watch;
        private float _time;
        public Timer()
        {
            
            _watch = System.Diagnostics.Stopwatch.StartNew();
        }
        public float Get()
        {
            return _time;
        }
        public void Stop()
        {
            if (_watch.IsRunning)
                _watch.Stop();
            long elapsed = _watch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));
            elapsed = elapsed != 0 ? elapsed : 1;
            _time = elapsed;
        }
        public void Start()
        {
            _watch = System.Diagnostics.Stopwatch.StartNew();
        }
    }
}
