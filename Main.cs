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
        int lightMode = 0;
        const int N = 10000;
        Stopwatch watch;
        Engine.Geometry cube = new Engine.Geometry();
        Engine.Shader pointLightShader;
        Engine.Shader spotLightShader;
        Engine.Shader directLightShader;
        Engine.Shader activeShader;
        Engine.CursorData cursor = new CursorData();
        Engine.Camera camera = new Engine.Camera();
        ImGuiController _controller;
        bool firstMove = true;
        int VertexBufferObject;
        int VertexArrayObject;
        int ElementBufferObject;
        float t = 0;
        int n = 1;

        public Simulation(int width, int height, string title) :
        base(GameWindowSettings.Default, new NativeWindowSettings()
        {
            Size = (width, height),
            Title = title
        }){}
        void ProcessKeyboard(FrameEventArgs e)
        {
            KeyboardState input = KeyboardState;
            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            Vector3 movementVector = new Vector3(0,0,0);
            if (input.IsKeyDown(Keys.W))
            {
                movementVector += camera.Front * camera.Speed * (float)e.Time; //Forward 
            }
            
            if (input.IsKeyDown(Keys.S))
            {
                movementVector -= camera.Front * camera.Speed * (float)e.Time; //Backwards
            }

            if (input.IsKeyDown(Keys.A))
            {
                movementVector -= Vector3.Normalize(Vector3.Cross(camera.Front, camera.Up)) * camera.Speed * (float)e.Time; //Left
            }

            if (input.IsKeyDown(Keys.D))
            {
                movementVector += Vector3.Normalize(Vector3.Cross(camera.Front, camera.Up)) * camera.Speed * (float)e.Time; //Right
            }

            if (input.IsKeyDown(Keys.Space))
            {
                movementVector += camera.Up * camera.Speed * (float)e.Time; //Up 
            }

            if (input.IsKeyDown(Keys.LeftShift))
            {
                movementVector -= camera.Up * camera.Speed * (float)e.Time; //Down
            }
            camera.Move(movementVector);
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            _controller.WindowResized(ClientSize.X, ClientSize.Y);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            ProcessKeyboard(e);

            if (IsMouseButtonPressed(MouseButton.Right))
            {
                if(CursorState == CursorState.Grabbed)
                    CursorState = CursorState.Normal;
                else
                {

                    CursorState = CursorState.Grabbed;
                }

            }
           
            if (CursorState == CursorState.Grabbed)
            {
                if (firstMove)
                {
                    cursor.LastPos = new Vector2(MousePosition.X, MousePosition.Y);
                    firstMove = false;
                }
                else
                {
                    float deltaX = MousePosition.X - cursor.LastPos.X;
                    float deltaY = MousePosition.Y - cursor.LastPos.Y;
                    cursor.LastPos = new Vector2(MousePosition.X, MousePosition.Y);
                    camera.Yaw += deltaX * camera.Sensitivity;
                    camera.Pitch -= deltaY * camera.Sensitivity;

                }
                camera.UpdateDirection();
            }
        }
    
        protected override void OnUnload()
        {
            base.OnUnload();
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);
            spotLightShader.Dispose();
            directLightShader.Dispose();
            pointLightShader.Dispose();
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            watch = System.Diagnostics.Stopwatch.StartNew();
            _controller = new ImGuiController(ClientSize.X, ClientSize.Y);
            GL.ClearColor(0.0f, 0.3f, 0.0f, 1.0f);

            //start logic
            CursorState = CursorState.Grabbed;
            // Going up directories: net6.0 -> Debug -> bin -> solutionFolder
            pointLightShader = new Shader("../../../Resources\\SimpleLighting.vert", "../../../Resources\\PointLight.frag");
            directLightShader = new Shader("../../../Resources\\SimpleLighting.vert", "../../../Resources\\DirectLight.frag");
            spotLightShader = new Shader("../../../Resources\\SimpleLighting.vert", "../../../Resources\\SpotLight.frag");
            //VAO
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);
            //VBO
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, cube.vertices.Length * sizeof(float), cube.vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            //EBO
            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, cube.indices.Length * sizeof(uint), cube.indices, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            //end logic
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            

            //start logic
            Matrix4.CreateOrthographicOffCenter(0.0f, 800.0f, 0.0f, 600.0f, 0.1f, 1000.0f);
            Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)800 / (float)600, 0.1f, 1000.0f);
            Matrix4 model =Matrix4.CreateTranslation(new Vector3(0,0,0));
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 800 / 600, 0.1f, 1000.0f);

            Vector3 cameraTarget = Vector3.Zero;
            Vector3 cameraDirection = Vector3.Normalize(camera.GetPosition() - cameraTarget);
            Vector3 up = Vector3.UnitY;
            Vector3 cameraRight = Vector3.Normalize(Vector3.Cross(up, cameraDirection));
            Vector3 cameraUp = Vector3.Cross(cameraDirection, cameraRight);

            Matrix4 view = Matrix4.LookAt(camera.GetPosition(), camera.GetPosition() + camera.Front, up);
            switch(lightMode)
            {
                case 0:
                    activeShader = pointLightShader;
                    break;
                case 1:
                    activeShader = spotLightShader;
                    break;
                case 2:
                    activeShader = directLightShader;
                    break;
            }

            activeShader.Use();
            //Uniforms
            int location = GL.GetUniformLocation(activeShader.Handle, "ourColor");
            GL.Uniform4(location, 0.0f, 1.0f, 0.0f, 1.0f);
            location = GL.GetUniformLocation(activeShader.Handle, "camPos");
            GL.Uniform3(location, camera.GetPosition());
            location = GL.GetUniformLocation(activeShader.Handle, "model");
            GL.UniformMatrix4(location, true, ref model);
            location = GL.GetUniformLocation(activeShader.Handle, "view");
            GL.UniformMatrix4(location, true, ref view);
            location = GL.GetUniformLocation(activeShader.Handle, "projection");
            GL.UniformMatrix4(location, true, ref projection);

            location = GL.GetUniformLocation(activeShader.Handle, "model");
            GL.BindVertexArray(VertexArrayObject);
            if(watch.IsRunning)
                watch.Stop();
            long elapsed = watch.ElapsedMilliseconds;
            elapsed = elapsed != 0 ? elapsed : 1;
            watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i=0;i<n;i+=3)
            {
                model = Matrix4.CreateTranslation(new Vector3(0.1f*(MathF.Floor(i/10))*MathF.Sin((float)i/10), 0, 0.1f * (MathF.Floor(i / 10)) * MathF.Cos((float)i / 10)));
                GL.UniformMatrix4(location, true, ref model);
                GL.DrawElements(PrimitiveType.Triangles, cube.indices.Length, DrawElementsType.UnsignedInt, 0);
            }

            //GUI RENDER
            _controller.Update(this, (float)e.Time);
            ImGui.Begin("Info");
            ImGui.SetWindowSize(new System.Numerics.Vector2(200,400));
            ImGui.Text("Objects rendered:"+n.ToString());
            ImGui.Text("Time elapsed:" + elapsed.ToString()+" ms");
            ImGui.Text("Frames per second:" + 1000/elapsed);
            ImGui.SliderInt("int", ref n, 0, N, "objects");
            ImGui.Text("Light Mode: " + lightMode);
            if (ImGui.Button("change"))
                lightMode = (lightMode + 1) % 3;
            ImGui.End();
            _controller.Render();
            ImGuiController.CheckGLError("End of frame");
            //end logic

            SwapBuffers();
        }
        protected override void OnTextInput(TextInputEventArgs e)
        {
            base.OnTextInput(e);


            _controller.PressChar((char)e.Unicode);
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            _controller.MouseScroll(e.Offset);
        }
    }

}
