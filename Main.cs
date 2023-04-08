using Dear_ImGui_Sample;
using Engine;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using ImGuiNET;
using System.Timers;
using System.Linq.Expressions;
using System;

using (Simulation.Simulation game = new Simulation.Simulation(800, 600, "Simulation"))
{
    GL.Enable(EnableCap.DepthTest);
    Console.WriteLine("Opening window!");
    game.Run();
}
namespace Simulation
{
    public class Simulation : GameWindow
    {
        Camera camera;
        WindowManager windowManager;
        SceneManager sceneManager;
        RenderBuffer buffer;
        Renderer renderer;
        public Simulation(int width, int height, string title) :
        base(GameWindowSettings.Default, new NativeWindowSettings()
        {
            Size = (width, height),
            Title = title
        })
        {
            renderer = new Renderer();
            camera = new Camera();
            sceneManager = new SceneManager(camera);
            windowManager = new WindowManager(this, camera);
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            windowManager.Resize(e);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            windowManager.ProcessInput(e);
        }
        protected override void OnUnload()
        {
            base.OnUnload();
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.0f, 0.3f, 0.0f, 1.0f);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            buffer = new RenderBuffer(sceneManager.CurrentScene);
            buffer.ChooseShader(sceneManager.CurrentScene.lightMode);           
            renderer.RenderScene(buffer);
            windowManager.RenderUI(sceneManager, e);
            SwapBuffers();
        }
        protected override void OnTextInput(TextInputEventArgs e)
        {
            base.OnTextInput(e);
            windowManager.Text(e);
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            windowManager.MouseWheel(e.Offset);
        }
    }

}
