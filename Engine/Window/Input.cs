using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Input
    {
        Camera _camera;
        public Input(Camera camera)
        {
            _camera = camera;
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
        }
    }
}
