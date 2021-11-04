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
using VRage.Input;
using Lima.Fancy.Elements;
using System.Text;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character.Components;
using VRage.Game.Entity.UseObject;

namespace Lima
{
  [MyTextSurfaceScript("FancyUI", "FancyUI")]
  public class FancyUITSS : MyTSSCommon
  {
    public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;

    IMyTerminalBlock _terminalBlock;
    IMyTextSurface _surface;

    public FancyUI fancyUI { get; private set; }

    public FancyUITSS(IMyTextSurface surface, IMyCubeBlock block, Vector2 size) : base(surface, block, size)
    {
      _surface = surface;

      int c = FancyUtils.GetRandomInt(0, ColorsList.colors.Count - 1);
      surface.ScriptBackgroundColor = Color.Black;
      Surface.ScriptForegroundColor = ColorsList.colors[c];

      fancyUI = new FancyUI(this);

      SampleApp sampleApp = fancyUI.appManager.AddApp<SampleApp>();
      sampleApp.CreateElements();
      sampleApp.InitElements();
      fancyUI.appManager.Current = sampleApp;

      _terminalBlock = (IMyTerminalBlock)block;
      _terminalBlock.OnMarkForClose += BlockMarkedForClose;

      FancyUISession.Instance?.AddSurfaceUI(surface, fancyUI);
    }

    public override void Dispose()
    {
      base.Dispose();
      fancyUI.Dispose();

      _terminalBlock.OnMarkForClose -= BlockMarkedForClose;
      FancyUISession.Instance?.RemoveSurfaceUI(_surface);
    }

    void BlockMarkedForClose(IMyEntity ent)
    {
      Dispose();
    }

    public override void Run()
    {
      try
      {
        base.Run();

        using (var frame = m_surface.DrawFrame())
        {
          frame.AddRange(fancyUI.GetSprites());
          frame.Dispose();
        }
      }
      catch (Exception e)
      {
        MyLog.Default.WriteLineAndConsole($"{e.Message}\n{e.StackTrace}");

        if (MyAPIGateway.Session?.Player != null)
          MyAPIGateway.Utilities.ShowNotification($"[ ERROR: {GetType().FullName}: {e.Message} ]", 5000, MyFontEnum.Red);
      }
    }
  }
}