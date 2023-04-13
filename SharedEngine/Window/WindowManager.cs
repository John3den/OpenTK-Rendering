using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Engine
{
    public class WindowManager
    {
        public UI _gui;
        public Input _inputController;
        public SceneManager _sceneManager;
        private GameWindow _window;
        public WindowManager(GameWindow window,SceneManager sceneManager,Renderer rend)
        {
            _sceneManager = sceneManager;
            _window = window;
            _inputController = new Input(rend._camera);
            _gui = new UI(window.ClientSize.X, window.ClientSize.Y);
            _window.CursorState = CursorState.Grabbed;
        }
        public void ProcessInput(FrameEventArgs e)
        {
            ProcessKeyboard(e);
            _inputController.GrabCursor(_window);
            _inputController.MoveCamera(_window);
        }
        public void RenderUI(SceneManager sceneManager, FrameEventArgs e)
        {
            _gui.Render(_window, sceneManager.CurrentScene.GetElapsedTime(), (float)e.Time, sceneManager.CurrentScene, sceneManager);
        }
        public void Resize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            _gui.Resize(_window.ClientSize.X, _window.ClientSize.Y);
        }
        public void MouseWheel(Vector2 offset)
        {
            _gui.MouseWheel(offset);
        }
        public void Text(TextInputEventArgs e)
        {
            _gui.InputText(e);
        }
        public void TimeTick(FrameEventArgs e)
        {
            if(_sceneManager != null)
            {
                if (_sceneManager._isRotating)
                {
                    _sceneManager.CurrentScene.UpdateTotalTime((float)e.Time);
                }
                _sceneManager.GlobalTimeTick((float)e.Time);
            }
        }
        public void ProcessKeyboard(FrameEventArgs e)
        {
            KeyboardState input = _window.KeyboardState;
            if (input.IsKeyDown(Keys.Escape))
            {
                _window.Close();
            }
            _inputController.HandleControls(input, (float)e.Time, _sceneManager);
        }
    }
}
