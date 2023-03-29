using Dear_ImGui_Sample;
using Engine;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Xml.Linq;
using ImGuiNET;


using (Simulation.Simulation game = new Simulation.Simulation(800, 600, "Bruh"))
{
   
    GL.Enable(EnableCap.DepthTest);
    Console.WriteLine("Opening window!");
    game.Run();
}
namespace Simulation
{

    public class Simulation : GameWindow
    {
        Engine.Geometry cube = new Engine.Geometry();
        Engine.Shader shader;
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
            if (input.IsKeyDown(Keys.W))
            {
                camera.position += camera.front * camera.speed * (float)e.Time; //Forward 
            }

            if (input.IsKeyDown(Keys.S))
            {
                camera.position -= camera.front * camera.speed * (float)e.Time; //Backwards
            }

            if (input.IsKeyDown(Keys.A))
            {
                camera.position -= Vector3.Normalize(Vector3.Cross(camera.front, camera.up)) * camera.speed * (float)e.Time; //Left
            }

            if (input.IsKeyDown(Keys.D))
            {
                camera.position += Vector3.Normalize(Vector3.Cross(camera.front, camera.up)) * camera.speed * (float)e.Time; //Right
            }

            if (input.IsKeyDown(Keys.Space))
            {
                camera.position += camera.up * camera.speed * (float)e.Time; //Up 
            }

            if (input.IsKeyDown(Keys.LeftShift))
            {
                camera.position -= camera.up * camera.speed * (float)e.Time; //Down
            }
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
                    camera.lastPos = new Vector2(MousePosition.X, MousePosition.Y);
                    firstMove = false;
                }
                else
                {
                    float deltaX = MousePosition.X - camera.lastPos.X;
                    float deltaY = MousePosition.Y - camera.lastPos.Y;
                    camera.lastPos = new Vector2(MousePosition.X, MousePosition.Y);

                    camera.Yaw += deltaX * camera.Sensitivity;
                    //Camera Clamping
                    if (camera.pitch - deltaY * camera.Sensitivity < 90 &&  camera.pitch - deltaY * camera.Sensitivity > -90)
                        camera.pitch -= deltaY * camera.Sensitivity;
                }

                camera.front.X = (float)Math.Cos(MathHelper.DegreesToRadians(camera.pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(camera.  Yaw));
                camera.front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(camera.pitch));
                camera.front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(camera.pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(camera.Yaw));
                camera.front = Vector3.Normalize(camera.front);
            }
        }
    
        protected override void OnUnload()
        {
            base.OnUnload();
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);
            shader.Dispose();
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            _controller = new ImGuiController(ClientSize.X, ClientSize.Y);
            GL.ClearColor(0.0f, 0.3f, 0.0f, 1.0f);

            //start logic
            CursorState = CursorState.Grabbed;
            shader = new Shader("D:\\ProgrammingStuff\\OpenTKRender\\SimpleLighting.vert", "D:\\ProgrammingStuff\\OpenTKRender\\SimpleLighting.frag");
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
            Vector3 cameraDirection = Vector3.Normalize(camera.position - cameraTarget);
            Vector3 up = Vector3.UnitY;
            Vector3 cameraRight = Vector3.Normalize(Vector3.Cross(up, cameraDirection));
            Vector3 cameraUp = Vector3.Cross(cameraDirection, cameraRight);

            Matrix4 view = Matrix4.LookAt(camera.position, camera.position + camera.front, up);

            shader.Use();
            //Uniforms
            int location = GL.GetUniformLocation(shader.Handle, "ourColor");
            GL.Uniform4(location, 0.0f, 1.0f, 0.0f, 1.0f);
            location = GL.GetUniformLocation(shader.Handle, "camPos");
            GL.Uniform3(location, camera.position);
            location = GL.GetUniformLocation(shader.Handle, "model");
            GL.UniformMatrix4(location, true, ref model);
            location = GL.GetUniformLocation(shader.Handle, "view");
            GL.UniformMatrix4(location, true, ref view);
            location = GL.GetUniformLocation(shader.Handle, "projection");
            GL.UniformMatrix4(location, true, ref projection);

            location = GL.GetUniformLocation(shader.Handle, "model");
            GL.BindVertexArray(VertexArrayObject);


            for (int i=0;i<n;i+=3)
            {
                model = Matrix4.CreateTranslation(new Vector3(2*MathF.Log(i*i+1)*MathF.Sin((float)i/10), 0, 2 * MathF.Log(i*i + 1) * MathF.Cos((float)i / 10)));
                GL.UniformMatrix4(location, true, ref model);
                GL.DrawElements(PrimitiveType.Triangles, cube.indices.Length, DrawElementsType.UnsignedInt, 0);
            }
            //GUI RENDER
            _controller.Update(this, (float)e.Time);
            ImGui.Begin("Info");
            ImGui.SetWindowSize(new System.Numerics.Vector2(200,100));
            ImGui.Text("Objects rendered:"+n.ToString());
            ImGui.SliderInt("int", ref n, 0, 5000, "!!!");
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
