using Dear_ImGui_Sample;
using ImGuiNET;
using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Text;
using static System.Formats.Asn1.AsnWriter;

namespace Engine
{

    public class UI
    {
        private const int TIMER_RESOLUTION = 1000000;
        private byte[] _N_str = new byte[128];
        int _lastUpdate = 0;
        int _lastFPS = 0;
        float _lastElapsed = 0;
        private string[] _lightingModes = { "point", "direct", "spot" };
        private ImGuiController _controller;
        public UI(int w, int h)
        {
            _controller = new ImGuiController(w, h); 
        }
        private void Update(SceneManager manager)
        {
            _lastUpdate = (int)MathF.Floor(manager.GlobalTime());
        }
        private bool IsUpdateRequired(SceneManager manager)
        {
            int thisUpdate = (int)MathF.Floor(manager.GlobalTime());
            if (thisUpdate != _lastUpdate)
            {
                return true;
            }
            return false;
        }
        private float FPS(SceneManager manager, float elapsed)
        {
            if(IsUpdateRequired(manager))
            {
                _lastFPS = (int)(TIMER_RESOLUTION / elapsed);
            }
            return _lastFPS;
        }
        private float Elapsed(SceneManager manager, float elapsed)
        {
            if (IsUpdateRequired(manager))
            {
                _lastElapsed = elapsed/1000.0f;
            }
            return _lastElapsed;
        }
        public void Resize(int w, int h)
        {
            _controller.WindowResized(w, h);
        }
        public void InputText(TextInputEventArgs e)
        {
            _controller.PressChar((char)e.Unicode);
        }
        public void MouseWheel(Vector2 offset)
        {
            _controller.MouseScroll(offset);
        }
        public void Render(GameWindow window, float elapsed, float globalTime, Scene scene, SceneManager manager)
        {
            int n = scene.n;
            string[] materials = { "red metal", "jade", "red rubber","ceramic","silver","bronze"};
            _N_str = new byte[128];
            _controller.Update(window, globalTime);
            ImGui.Begin("Info");
            ImGui.SetWindowSize(new System.Numerics.Vector2(300, 500));
            ImGui.Text("Controls: w/a/s/d, space/shift");
            ImGui.Text("Light Controls: i/j/k/l, u/o");
            ImGui.Text("Objects rendered:" + n.ToString());
            ImGui.Text("Render latency:" + Elapsed(manager,elapsed).ToString() + " ms");
            ImGui.Text("Frames per second:" + FPS(manager, elapsed).ToString());
            ImGui.SliderInt(" ", ref n, 0, Scene.N, "objects");
            ImGui.InputText("objects", _N_str, 128);
            ImGui.Text("Light Mode: " + _lightingModes[scene.GetLightMode()]);
            if (ImGui.Button("change light"))
                scene.NextLightMode();
            ImGui.Text("Light position:");
            ImGui.Text(scene._lightSource.Transform.ExtractTranslation().ToString());
            ImGui.Text("Scene: task " + (4 - manager.GetSceneNumber()));
            if (ImGui.Button("change scene"))
                manager.NextScene();
            ImGui.Checkbox("Rotate", ref manager._isRotating);
            ImGui.SliderFloat("red", ref manager.GetMaterial()._color.X, 0, 1, "r");
            ImGui.SliderFloat("green", ref manager.GetMaterial()._color.Y, 0, 1, "g");
            ImGui.SliderFloat("blue", ref manager.GetMaterial()._color.Z, 0, 1, "b");
            ImGui.SliderFloat("ambient", ref manager.GetMaterial()._ambient, 0, 1, "ambient");
            ImGui.SliderFloat("specular", ref manager.GetMaterial()._spec, 0, 5, "spe");
            ImGui.SliderInt("reflect", ref manager.GetMaterial()._reflectivity, 0, 128, "ref");
            ImGui.Combo("material",ref manager._material,materials,6);
            ImGui.EndCombo();
            ImGui.End();
            _controller.Render();
            ImGuiController.CheckGLError("End of frame");
            Update(manager);
            if(scene.n != n)
            {
                scene.n = n;
                _N_str = new byte[128];
            }
            else
            {
                string str = System.Text.Encoding.Default.GetString(_N_str);
                try
                {
                    if (str[0] != '\0')
                    {
                        int result = Int32.Parse(str);
                        if (result <= Scene.N)
                            scene.n = result;
                    }
                }
                catch (FormatException) { }
                catch (OverflowException) { }
            }
        }
    }
}
