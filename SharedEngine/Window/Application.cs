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
        WindowManager _windowManager;
        SceneManager _sceneManager;
        Renderer _renderer;
        public Application(int width, int height, string title) :
        base(GameWindowSettings.Default, new NativeWindowSettings()
        {
            Size = (width, height),
            Title = title
        })
        {
            _renderer = new Renderer();
            _sceneManager = new SceneManager(_renderer);
            _windowManager = new WindowManager(this, _sceneManager, _renderer);
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            _windowManager.Resize(e);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            _windowManager.ProcessInput(e);
            _windowManager.TimeTick(e);
        }
        protected override void OnUnload()
        {
            base.OnUnload();
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.04f, 0.04f, 0.04f, 1.0f);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            RenderBuffer buffer = new RenderBuffer(_sceneManager.CurrentScene, _sceneManager.GetMaterial(), ClientSize);
            buffer.ChooseShader(_sceneManager.CurrentScene.GetLightMode());
            _renderer.RenderScene(buffer);
            _windowManager.RenderUI(_sceneManager, e);
            SwapBuffers();
        }
        protected override void OnTextInput(TextInputEventArgs e)
        {
            base.OnTextInput(e);
            _windowManager.Text(e);
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            _windowManager.MouseWheel(e.Offset);
        }
    }

}
