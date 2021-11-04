using System;
using System.Collections.Generic;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using Lima.Fancy;
using Lima.Fancy.Elements;

namespace Lima
{
  public class SampleApp : FancyApp
  {
    public SampleApp()
    {
      // CreateElements();
    }

    public void CreateElements()
    {
      var window = new FancyView(FancyView.ViewDirection.Row)
      {
        Position = new Vector2(0, 0),
        Scale = new Vector2(1, 1)
      };

      var windowBar = new FancyWindowBar(AppManager.Tss.Block.DisplayNameText)
      // var windowBar = new FancyWindowBar("Sample App")
      {
        Position = new Vector2(0, 0),
        Scale = new Vector2(1, 0),
        Pixels = new Vector2(0, 24)
      };

      var col1 = new FancyView()
      {
        Position = new Vector2(0, 0),
        Scale = new Vector2(0.5f, 1)
      };

      var col2 = new FancyView()
      {
        Position = new Vector2(0.5f, 0),
        Scale = new Vector2(0.5f, 1)
      };

      var header1 = new FancyLabel("Column 1", 0.6f)
      {
        Position = new Vector2(0, 0),
        Scale = new Vector2(1, 0),
        Margin = new Vector4(8, 8, 8, 0),
        Pixels = new Vector2(0, 16)
      };

      var header2 = new FancyLabel("Column 2", 0.6f)
      {
        Position = new Vector2(0, 0),
        Scale = new Vector2(1, 0),
        Margin = new Vector4(8, 8, 8, 0),
        Pixels = new Vector2(0, 16)
      };

      var separator1 = new FancySeparator()
      {
        Position = new Vector2(0, 0),
        Scale = new Vector2(1, 0),
        Margin = new Vector4(8, 2, 8, 0),
        Pixels = new Vector2(0, 2)
      };

      var separator2 = new FancySeparator()
      {
        Position = new Vector2(0, 0),
        Scale = new Vector2(1, 0),
        Margin = new Vector4(8, 2, 8, 0),
        Pixels = new Vector2(0, 2)
      };

      var labelSlider = new FancyLabel("A Fancy Slider")
      {
        Position = new Vector2(0, 0),
        Scale = new Vector2(1, 0),
        Margin = new Vector4(8, 8, 8, 0),
        Pixels = new Vector2(0, 16)
      };

      var slider = new FancySlider(-100, 100)
      {
        Position = new Vector2(0, 0),
        Scale = new Vector2(1, 0),
        Margin = new Vector4(8, 0, 8, 0),
        Pixels = new Vector2(0, 24)
      };

      var labelSliderRange = new FancyLabel("Range Slider")
      {
        Position = new Vector2(0, 0),
        Scale = new Vector2(1, 0),
        Margin = new Vector4(8, 8, 8, 0),
        Pixels = new Vector2(0, 16)
      };

      var sliderRange = new FancySliderRange(-100, 100)
      {
        Position = new Vector2(0, 0),
        Scale = new Vector2(1, 0),
        Margin = new Vector4(8, 0, 8, 0),
        Pixels = new Vector2(0, 24)
      };

      var labelProgressBar = new FancyLabel("Progress Bar")
      {
        Position = new Vector2(0, 0),
        Scale = new Vector2(1, 0),
        Margin = new Vector4(8, 8, 8, 0),
        Pixels = new Vector2(0, 16)
      };

      var progressBar = new FancyProgressBar(-100, 100)
      {
        Position = new Vector2(0, 0),
        Scale = new Vector2(1, 0),
        Margin = new Vector4(8, 0, 8, 0),
        Pixels = new Vector2(0, 24)
      };

      var labelSelector = new FancyLabel("Color")
      {
        Position = new Vector2(0, 0),
        Scale = new Vector2(1, 0),
        Margin = new Vector4(8, 8, 8, 0),
        Pixels = new Vector2(0, 16)
      };

      var selector = new FancySelector(ColorsList.names, (int i, string color) =>
      {
        AppManager.Tss.Surface.ScriptForegroundColor = ColorsList.colors[i];
      })
      {
        Position = new Vector2(0, 0),
        Scale = new Vector2(1, 0),
        Margin = new Vector4(8, 0, 8, 0),
        Pixels = new Vector2(0, 24)
      };

      var labelSwitch = new FancyLabel("Toggle: Off")
      {
        Position = new Vector2(0, 0),
        Scale = new Vector2(1, 0),
        Margin = new Vector4(8, 8, 8, 0),
        Pixels = new Vector2(0, 16)
      };

      var switcher = new FancySwitch((bool v) =>
      {
        if (v)
          labelSwitch.Text = "Toggle: On";
        else
          labelSwitch.Text = "Toggle: Off";
      })
      {
        Position = new Vector2(0, 0),
        Scale = new Vector2(1, 0),
        Margin = new Vector4(8, 0, 8, 0),
        Pixels = new Vector2(0, 24)
      };

      var button = new FancyButton("A Fancy Button", () =>
     {
       Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessage("Test", "LCD");
     })
      {
        Position = new Vector2(0, 0),
        Scale = new Vector2(1, 0),
        Margin = new Vector4(8, 8, 8, 0),
        Pixels = new Vector2(0, 24)
      };

      var labelTextField = new FancyLabel("Text Field")
      {
        Position = new Vector2(0, 0),
        Scale = new Vector2(1, 0),
        Margin = new Vector4(8, 8, 8, 0),
        Pixels = new Vector2(0, 16)
      };

      var textField = new FancyTextField("", (string text) =>
      {
        Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessage(text, "LCD");
      })
      {
        Position = new Vector2(0, 0),
        Scale = new Vector2(1, 0),
        Margin = new Vector4(8, 0, 8, 0),
        Pixels = new Vector2(0, 24)
      };

      col1.AddChild(header1);
      col1.AddChild(separator1);
      col1.AddChild(labelSlider);
      col1.AddChild(slider);
      col1.AddChild(labelProgressBar);
      col1.AddChild(progressBar);
      col1.AddChild(labelSwitch);
      col1.AddChild(switcher);
      col1.AddChild(button);
      col2.AddChild(header2);
      col2.AddChild(separator2);
      col2.AddChild(labelSliderRange);
      col2.AddChild(sliderRange);
      col2.AddChild(labelSelector);
      col2.AddChild(selector);
      col2.AddChild(labelTextField);
      col2.AddChild(textField);
      window.AddChild(col1);
      window.AddChild(col2);
      AddChild(windowBar);
      AddChild(window);
    }
  }
}