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
        const int N = 20;
        Actor whole;
        Actor split;
        Engine.Geometry normalCube = new Engine.Geometry(0);
        Engine.Geometry splitCube = new Engine.Geometry(1);
        Engine.Shader pointLightShader;
        Engine.Shader spotLightShader;
        Engine.Shader directLightShader;

        Engine.Shader vertPointLightShader;
        Engine.Shader vertSpotLightShader;
        Engine.Shader vertDirectLightShader;
        Engine.Shader activeShader;
        Engine.CursorData cursor = new CursorData();
        Engine.Camera camera = new Engine.Camera();
        bool firstMove = true;
        int sceneNumber=0;
        int n = 1;
        UI gui;
        Timer timer;
        Input inputController;
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
            inputController.HandleControls(input, (float)e.Time);
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            gui.Resize(ClientSize.X, ClientSize.Y);
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
            //GL.DeleteBuffer(VertexBufferObject);
            spotLightShader.Dispose();
            directLightShader.Dispose();
            pointLightShader.Dispose();
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            inputController = new Input(camera);
            timer = new Timer();
            gui = new UI(ClientSize.X, ClientSize.Y);
            GL.ClearColor(0.0f, 0.3f, 0.0f, 1.0f);
            //start logic
            CursorState = CursorState.Grabbed;
            // Going up directories: net6.0 -> Debug -> bin -> solutionFolder
            pointLightShader = new Shader("../../../Resources\\Generic.vert", "../../../Resources\\PointLight.frag");
            directLightShader = new Shader("../../../Resources\\Generic.vert", "../../../Resources\\DirectLight.frag");
            spotLightShader = new Shader("../../../Resources\\Generic.vert", "../../../Resources\\SpotLight.frag");

            vertPointLightShader = new Shader("../../../Resources\\PointLight.vert", "../../../Resources\\Generic.frag");
            vertDirectLightShader = new Shader("../../../Resources\\DirectLight.vert", "../../../Resources\\Generic.frag");
            vertSpotLightShader = new Shader("../../../Resources\\SpotLight.vert", "../../../Resources\\Generic.frag");

            whole = new Actor(normalCube);
            split = new Actor(splitCube);
            //end logic
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            switch (lightMode + 3*sceneNumber)
            {
                case 0:
                    activeShader = vertPointLightShader;
                    break;
                case 1:
                    activeShader = vertSpotLightShader;
                    break;
                case 2:
                    activeShader = vertDirectLightShader;
                    break;
                case 3:
                    activeShader = pointLightShader;
                    break;
                case 4:
                    activeShader = spotLightShader;
                    break;
                case 5:
                    activeShader = directLightShader;
                    break;
            }

            activeShader.Use();
            //Uniforms
            RenderBuffer buffer;
            if (sceneNumber == 0)
            {
                buffer = new RenderBuffer(camera, whole, activeShader, n);
            }
            else
            {
                buffer = new RenderBuffer(camera, split, activeShader, n);
            }
            Renderer renderer = new Renderer();
            renderer.RenderScene(buffer);
            timer.Stop();
            gui.Render(this,timer,(float)e.Time,ref n,N,ref lightMode, ref sceneNumber);
            timer.Start();
            SwapBuffers();
        }
        protected override void OnTextInput(TextInputEventArgs e)
        {
            base.OnTextInput(e);


            gui.InputText(e);
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            gui.MouseWheel(e.Offset);
        }
    }

}
