using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI;
using System;
using VRage.Game.ModAPI;
using VRage.Game;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRage.Game.GUI.TextPanel;
using Lima.Utils;

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

      _app = new ScreenCalibrationApp(_block, _surface);
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
      InputUtils.SetPlayerUseBlacklistState(false);
      Dispose();
    }

    private MySprite[] GetProgressSprite(float ratio)
    {
      var viewport = (_surface.TextureSize - _surface.SurfaceSize) / 2f;
      var angle = MathHelper.TwoPi * ratio;
      var size = new Vector2(MathHelper.Min(_surface.SurfaceSize.X, _surface.SurfaceSize.Y)) * 0.5f;
      var pos = new Vector2(viewport.X + (_surface.SurfaceSize.X - size.X) * 0.5f, viewport.Y + _surface.SurfaceSize.Y * 0.5f);

      var circ1 = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "Screen_LoadingBar",
        RotationOrScale = angle,
        Color = _surface.ScriptForegroundColor,
        Position = pos,
        Size = size
      };

      var circ2 = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "Screen_LoadingBar",
        RotationOrScale = MathHelper.Pi * -angle,
        Color = _surface.ScriptForegroundColor,
        Position = new Vector2(viewport.X + (_surface.SurfaceSize.X - size.X * 0.5f) * 0.5f, pos.Y),
        Size = size * 0.5f
      };

      return new MySprite[] { circ2, circ1 };
    }

    public override void Run()
    {
      if (ticks == 0)
      {
        ticks++;
        base.Run();
        return;
      }

      try
      {
        var loading = !_init && ticks++ < (2 + 6); // 1 second

        if (loading)
        {
          base.Run();
          using (var frame = m_surface.DrawFrame())
          {
            frame.AddRange(GetProgressSprite((float)(ticks - 2) / 6f));
          }
          return;
        }

        if (!_init)
          Init();

        if (_app == null)
          return;

        base.Run();

        using (var frame = m_surface.DrawFrame())
        {
          _app.Update();
          frame.AddRange(_app.GetSprites());
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