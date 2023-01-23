using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI;
using System;
using VRage.Game.ModAPI;
using VRage.Game;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Lima.Apps.ScreenCalibration
{
  [MyTextSurfaceScript("Touch_Calibration", "Screen Calibration")]
  public class CalibrateTSS : MyTSSCommon
  {
    public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;

    IMyCubeBlock _block;
    IMyTerminalBlock _terminalBlock;
    IMyTextSurface _surface;

    ScreenCalibrationApp _app;

    bool _init = false;
    int ticks = 0;

    public CalibrateTSS(IMyTextSurface surface, IMyCubeBlock block, Vector2 size) : base(surface, block, size)
    {
      _block = block;
      _surface = surface;
      _terminalBlock = (IMyTerminalBlock)block;

      surface.ScriptBackgroundColor = Color.Black;
      Surface.ScriptForegroundColor = Color.DarkRed;
    }

    public void Init()
    {
      if (_init)
        return;
      _init = true;

      _app = new ScreenCalibrationApp();
      _app.InitApp(_block, _surface);
      _app.CreateElements();
      _app.Theme.Scale = Math.Min(Math.Max(Math.Min(this.Surface.SurfaceSize.X, this.Surface.SurfaceSize.Y) / 512, 0.75f), 2);
      _app.Cursor.Scale = _app.Theme.Scale;

      _terminalBlock.OnMarkForClose += BlockMarkedForClose;
    }

    public override void Dispose()
    {
      base.Dispose();

      _app?.Dispose();
      _app = null;
      _terminalBlock.OnMarkForClose -= BlockMarkedForClose;
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

        if (_app == null)
          return;

        base.Run();

        using (var frame = m_surface.DrawFrame())
        {
          _app.Update();
          frame.AddRange(_app.GetSprites());
          frame.Dispose();
        }
      }
      catch (Exception e)
      {
        _app = null;
        MyLog.Default.WriteLineAndConsole($"{e.Message}\n{e.StackTrace}");

        if (MyAPIGateway.Session?.Player != null)
          MyAPIGateway.Utilities.ShowNotification($"[ ERROR: {GetType().FullName}: {e.Message} ]", 5000, MyFontEnum.Red);
      }
    }
  }
}