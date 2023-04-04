﻿using Dear_ImGui_Sample;
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
        ImGuiController _controller;
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
        public void Render(GameWindow window, Timer timer, float globalTime, ref int n, int N, ref int lightMode)
        {
            _controller.Update(window, globalTime);
            ImGui.Begin("Info");
            ImGui.SetWindowSize(new System.Numerics.Vector2(200, 400));
            ImGui.Text("Objects rendered:" + n.ToString());
            ImGui.Text("Time elapsed:" + timer.Get().ToString() + " ms");
            ImGui.Text("Frames per second:" + (int)(1000 / timer.Get()));
            ImGui.SliderInt("int", ref n, 0, N, "objects");
            ImGui.Text("Light Mode: " + lightMode);
            if (ImGui.Button("change"))
                lightMode = (lightMode + 1) % 3;
            ImGui.End();
            _controller.Render();
            ImGuiController.CheckGLError("End of frame");
        }
    }
}