using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class WindowManager
    {
        public UI gui;
        public Input inputController;
        private GameWindow _window;
        public WindowManager(GameWindow window,Camera camera)
        {
            _window = window;
            inputController = new Input(camera);
            gui = new UI(window.ClientSize.X, window.ClientSize.Y);
            _window.CursorState = CursorState.Grabbed;
        }
        public void ProcessInput(FrameEventArgs e)
        {
            ProcessKeyboard(e);
            inputController.GrabCursor(_window);
            inputController.MoveCamera(_window);
        }
        public void RenderUI(SceneManager sceneManager, FrameEventArgs e)
        {
            gui.Render(_window, sceneManager.CurrentScene.GetElapsedTime(), (float)e.Time, ref sceneManager.CurrentScene.n, sceneManager.CurrentScene.N, ref sceneManager.CurrentScene.lightMode, ref sceneManager._sceneNumber);
        }
        public void Resize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            gui.Resize(_window.ClientSize.X, _window.ClientSize.Y);
        }
        public void MouseWheel(Vector2 offset)
        {
            gui.MouseWheel(offset);
        }
        public void Text(TextInputEventArgs e)
        {
            gui.InputText(e);
        }
        public void ProcessKeyboard(FrameEventArgs e)
        {
            KeyboardState input = _window.KeyboardState;
            if (input.IsKeyDown(Keys.Escape))
            {
                _window.Close();
            }
            inputController.HandleControls(input, (float)e.Time);
        }
    }
}
