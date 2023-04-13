using Engine;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

class Programm
{
    static void Main(string[] args)
    {
        Application app = new Application(800, 600, "Simulation");
        GL.Enable(EnableCap.DepthTest);
        Console.WriteLine("Opening window!");
        app.Run();
    }
}