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
using VRageRender.ExternalApp;
using VRage.ObjectBuilders;
using Sandbox.Common.ObjectBuilders;
using Lima.Touch;

namespace Lima.Fancy
{
  [MyTextSurfaceScript("FancyUI", "FancyUI")]
  public class FancyUITSS : MyTSSCommon
  {
    public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;

    IMyCubeBlock _block;
    IMyTerminalBlock _terminalBlock;
    IMyTextSurface _surface;

    bool _init = false;
    int ticks = 0;

    public FancyUI fancyUI { get; private set; }

    public FancyUITSS(IMyTextSurface surface, IMyCubeBlock block, Vector2 size) : base(surface, block, size)
    {
      _block = block;
      _surface = surface;
      _terminalBlock = (IMyTerminalBlock)block;

      int c = FancyUtils.GetRandomInt(0, ColorsList.colors.Count - 1);
      surface.ScriptBackgroundColor = Color.Black;
      Surface.ScriptForegroundColor = ColorsList.colors[c];
    }

    public void Init()
    {
      if (_init)
        return;
      _init = true;

      fancyUI = new FancyUI(this);

      SampleApp sampleApp = fancyUI.appManager.AddApp<SampleApp>();
      sampleApp.CreateElements();
      sampleApp.InitElements();
      fancyUI.appManager.Current = sampleApp;

      _terminalBlock.OnMarkForClose += BlockMarkedForClose;

      FancyUISession.Instance?.AddSurfaceUI(_surface, fancyUI);
    }

    public override void Dispose()
    {
      base.Dispose();
      fancyUI?.Dispose();

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
        if (!_init && ticks++ < (6 * 2)) // 2 secs
          return;

        Init();

        if (fancyUI == null)
          return;

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