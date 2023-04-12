using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Engine;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Engine
{
    public class Application : GameWindow
    {
        WindowManager windowManager;
        SceneManager sceneManager;
        Renderer renderer;
        public Application(int width, int height, string title) :
        base(GameWindowSettings.Default, new NativeWindowSettings()
        {
            Size = (width, height),
            Title = title
        })
        {
            renderer = new Renderer();
            sceneManager = new SceneManager(renderer);
            windowManager = new WindowManager(this, sceneManager, renderer);
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
            RenderBuffer buffer = new RenderBuffer(sceneManager.CurrentScene, sceneManager.GetMaterial());
            buffer.ChooseShader(sceneManager.CurrentScene.GetLightMode());
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
