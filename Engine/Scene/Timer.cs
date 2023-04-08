using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Timer
    {
        private Stopwatch watch;
        private float _time;
        public Timer()
        {
            watch = System.Diagnostics.Stopwatch.StartNew();
        }
        public float Get()
        {
            return _time;
        }
        public void Stop()
        {
            if (watch.IsRunning)
                watch.Stop();
            long elapsed = watch.ElapsedMilliseconds;
            elapsed = elapsed != 0 ? elapsed : 1;
            _time = elapsed;
        }
        public void Start()
        {
            watch = System.Diagnostics.Stopwatch.StartNew();
        }
    }
}
