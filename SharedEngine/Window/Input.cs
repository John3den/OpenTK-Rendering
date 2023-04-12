using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Engine
{
    public class Input
    {
        Vector2 _cursor;
        Camera _camera;
        SceneManager _sceneManager;
        bool firstMove = true;
        public Input(Camera camera, SceneManager sceneManager)
        {
            _camera = camera;
            _sceneManager = sceneManager;
        }
        public void GrabCursor(GameWindow window)
        {
            if (window.IsMouseButtonPressed(MouseButton.Right))
            {
                if (window.CursorState == CursorState.Grabbed)
                    window.CursorState = CursorState.Normal;
                else
                {

                    window.CursorState = CursorState.Grabbed;
                }

            }
        }
        public void MoveCamera(GameWindow window)
        {
            if (window.CursorState == CursorState.Grabbed)
            {
                if (firstMove)
                {
                    _cursor = new Vector2(window.MousePosition.X, window.MousePosition.Y);
                    firstMove = false;
                }
                else
                {
                    float deltaX = window.MousePosition.X - _cursor.X;
                    float deltaY = window.MousePosition.Y - _cursor.Y;
                    _cursor = new Vector2(window.MousePosition.X, window.MousePosition.Y);
                    _camera.Yaw += deltaX * _camera.Sensitivity;
                    _camera.Pitch -= deltaY * _camera.Sensitivity;

                }
                _camera.UpdateDirection();
            }
        }
        public void HandleControls(KeyboardState input, float time)
        {
            Vector3 movementVector = new Vector3(0, 0, 0);
            if (input.IsKeyDown(Keys.W))
            {
                movementVector += _camera.Front * _camera.Speed * time; //Forward 
            }

            if (input.IsKeyDown(Keys.S))
            {
                movementVector -= _camera.Front * _camera.Speed * time; //Backwards
            }

            if (input.IsKeyDown(Keys.A))
            {
                movementVector -= Vector3.Normalize(Vector3.Cross(_camera.Front, _camera.Up)) * _camera.Speed * time; //Left
            }

            if (input.IsKeyDown(Keys.D))
            {
                movementVector += Vector3.Normalize(Vector3.Cross(_camera.Front, _camera.Up)) * _camera.Speed * time; //Right
            }

            if (input.IsKeyDown(Keys.Space))
            {
                movementVector += _camera.Up * _camera.Speed * time; //Up 
            }

            if (input.IsKeyDown(Keys.LeftShift))
            {
                movementVector -= _camera.Up * _camera.Speed * time; //Down
            }
            _camera.Move(movementVector);
            movementVector = new Vector3(0, 0, 0);
            float LS_speed = 5.0f;
            if (input.IsKeyDown(Keys.I))
            {
                movementVector += new Vector3(LS_speed, 0, 0) * time; //Forward 
            }

            if (input.IsKeyDown(Keys.K))
            {
                movementVector -= new Vector3(LS_speed, 0, 0) * time; //Backwards
            }

            if (input.IsKeyDown(Keys.J))
            {
                movementVector -= new Vector3(0, 0, LS_speed) * time; //Left
            }

            if (input.IsKeyDown(Keys.L))
            {
                movementVector += new Vector3(0, 0, LS_speed) * time; //Right
            }

            if (input.IsKeyDown(Keys.U))
            {
                movementVector += new Vector3(0, LS_speed, 0) * time; //Up 
            }

            if (input.IsKeyDown(Keys.O))
            {
                movementVector -= new Vector3(0, LS_speed, 0) * time; //Down
            }
            if(movementVector.Length!=0)
                _sceneManager.CurrentScene._lightSource.Transform *= Matrix4.CreateTranslation(movementVector);
        }
    }
}
