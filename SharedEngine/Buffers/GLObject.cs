using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public abstract class GLObject
    {
        protected int _handle;
        public abstract void Bind();
    }
}
