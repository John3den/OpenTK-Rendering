using Engine;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Xml.Linq;


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
        float Yaw = -90;
        float pitch = 0;
        float Sensitivity = 0.1f;
        bool firstMove = true;
        Vector2 lastPos;
        float speed = 15f;
        Vector3 position = new Vector3(0.0f, 0.0f, 30.0f);
        Vector3 front = new Vector3(0.0f, 1.0f, -1.0f);
        Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
        int VertexBufferObject;
        int VertexArrayObject;
        int ElementBufferObject;
        Engine.Shader shader;
        private readonly float[] vertices =
          {
      // positions        // colors
      0.5f,  0.5f, 0.5f,  1.0f, 0.0f, 0.0f,   // TOP
     -0.5f,  0.5f, -0.5f,  1.0f, 0.0f, 0.0f,   //
      0.5f,  0.5f, -0.5f,  1.0f, 0.0f, 0.0f,    // 
      -0.5f,  0.5f, 0.5f,  1.0f, 0.0f, 0.0f,   // 
      0.5f,  -0.5f, 0.5f,  0.0f, 0.0f, 1.0f,   // BOTTOM
     -0.5f,  -.5f, -0.5f,  0.0f, 0.0f, 1.0f,   //
      0.5f,  -0.5f, -0.5f,  0.0f, 0.0f, 1.0f,    // 
      -0.5f, -0.5f, 0.5f,  0.0f, 0.0f, 1.0f,   // 

    };
        uint[] indices = {  
    0, 2, 1,
    0, 3, 1,

    4, 5, 7,
    4, 5, 6,

    0, 2, 6,
    0, 4, 6,

    1, 3, 7,
    1, 5, 7,

    1, 5, 6,
    1, 2, 6,

    3, 0, 4,
    3, 7, 4
};
        public Simulation(int width, int height, string title) :
        base(GameWindowSettings.Default, new NativeWindowSettings()
        {
            Size = (width, height),
            Title = title
        }){}
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            
            KeyboardState input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            if (input.IsKeyDown(Keys.W))
            {
                position += front * speed * (float)e.Time; //Forward 
            }

            if (input.IsKeyDown(Keys.S))
            {
                position -= front * speed * (float)e.Time; //Backwards
            }

            if (input.IsKeyDown(Keys.A))
            {
                position -= Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)e.Time; //Left
            }

            if (input.IsKeyDown(Keys.D))
            {
                position += Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)e.Time; //Right
            }

            if (input.IsKeyDown(Keys.Space))
            {
                position += up * speed * (float)e.Time; //Up 
            }

            if (input.IsKeyDown(Keys.LeftShift))
            {
                position -= up * speed * (float)e.Time; //Down
            }
            if (firstMove)
            {
                lastPos = new Vector2(MousePosition.X, MousePosition.Y);
                firstMove = false;
            }
            else
            {
                float deltaX = MousePosition.X - lastPos.X;
                float deltaY = MousePosition.Y - lastPos.Y;
                lastPos = new Vector2(MousePosition.X, MousePosition.Y);

                Yaw += deltaX * Sensitivity;
                //Camera Clamping
                if (pitch - deltaY * Sensitivity < 90 && pitch - deltaY * Sensitivity > -90)
                    pitch -= deltaY * Sensitivity;
            }

            front.X = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(Yaw));
            front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
            front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(Yaw));
            front = Vector3.Normalize(front);
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
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            //EBO
            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            //end logic
        }
        float t = 0;
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            

            //start logic
            t += 0.5f;
            Matrix4.CreateOrthographicOffCenter(0.0f, 800.0f, 0.0f, 600.0f, 0.1f, 100.0f);
            Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)800 / (float)600, 0.1f, 100.0f);
            Matrix4 model =Matrix4.CreateScale(5f)*Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0));
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 800 / 600, 0.1f, 100.0f);

            Vector3 cameraTarget = Vector3.Zero;
            Vector3 cameraDirection = Vector3.Normalize(position - cameraTarget);
            Vector3 up = Vector3.UnitY;
            Vector3 cameraRight = Vector3.Normalize(Vector3.Cross(up, cameraDirection));
            Vector3 cameraUp = Vector3.Cross(cameraDirection, cameraRight);

            Matrix4 view = Matrix4.LookAt(position, position + front, up);

            shader.Use();
            //Uniforms
            int vertexColorLocation = GL.GetUniformLocation(shader.Handle, "ourColor");
            GL.Uniform4(vertexColorLocation, 0.0f, 1.0f, 0.0f, 1.0f);

            int location = GL.GetUniformLocation(shader.Handle, "model");
            GL.UniformMatrix4(location, true, ref model);
            location = GL.GetUniformLocation(shader.Handle, "view");
            GL.UniformMatrix4(location, true, ref view);
            location = GL.GetUniformLocation(shader.Handle, "projection");
            GL.UniformMatrix4(location, true, ref projection    );
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            //end logic

            SwapBuffers();
        }

        static void Main(string[] args)
        {
            
        }
    }

}
