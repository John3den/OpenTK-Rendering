using Dear_ImGui_Sample;
using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    
    public class UI
    {
        private string[] LightingModes = { "point", "spot", "direct" };
        private ImGuiController _controller;
        public UI(int w, int h)
        {
            _controller = new ImGuiController(w, h);
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
        public void Render(GameWindow window, float elapsed, float globalTime, ref int n, int N, ref int lightMode, ref int sceneNumber)
        {
            _controller.Update(window, globalTime);
            ImGui.Begin("Info");
            ImGui.SetWindowSize(new System.Numerics.Vector2(200, 400));
            ImGui.Text("Objects rendered:" + n.ToString());
            ImGui.Text("Time elapsed:" + elapsed.ToString() + " ms");
            ImGui.Text("Frames per second:" + (int)(1000 / elapsed));
            ImGui.SliderInt("int", ref n, 0, N, "objects");
            ImGui.Text("Light Mode: " + LightingModes[lightMode]);
            if (ImGui.Button("change light"))
                lightMode = (lightMode + 1) % 3;
            ImGui.Text("Scene: task " + (3+ sceneNumber));
            if (ImGui.Button("change scene"))
                sceneNumber = (sceneNumber + 1) % 2;
            ImGui.End();
            _controller.Render();
            ImGuiController.CheckGLError("End of frame");
        }
    }
}
