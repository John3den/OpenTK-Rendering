﻿using Dear_ImGui_Sample;
using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Engine
{

    public class UI
    {
        private string[] LightingModes = { "point", "direct", "spot" };
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
        public void Render(GameWindow window, float elapsed, float globalTime, Scene scene, SceneManager manager)
        {
            int n = scene.n;
            _controller.Update(window, globalTime);
            ImGui.Begin("Info");
            ImGui.SetWindowSize(new System.Numerics.Vector2(200, 400));
            ImGui.Text("Objects rendered:" + n.ToString());
            ImGui.Text("Time elapsed:" + elapsed.ToString() + " ms");
            ImGui.Text("Frames per second:" + (int)(1000 / elapsed));
            ImGui.SliderInt("int", ref n, 0, Scene.N, "objects");
            ImGui.Text("Light Mode: " + LightingModes[scene.GetLightMode()]);
            if (ImGui.Button("change light"))
                scene.NextLightMode();
            ImGui.Text("Scene: task " + (4 - manager.GetSceneNumber()));
            if (ImGui.Button("change scene"))
                manager.NextScene();
            ImGui.End();
            _controller.Render();
            ImGuiController.CheckGLError("End of frame");
            scene.n = n;
        }
    }
}
