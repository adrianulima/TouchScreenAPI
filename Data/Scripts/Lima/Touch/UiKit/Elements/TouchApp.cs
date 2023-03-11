using Lima.Utils;
using Sandbox.ModAPI;
using System.Collections.Generic;
using System;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRageMath;

namespace Lima.Touch.UiKit.Elements
{
  public class TouchApp : View
  {
    public event Action UpdateAtSimulationEvent;

    public TouchScreen Screen { get; private set; }
    public Cursor Cursor { get; private set; }
    public Theme Theme { get; private set; }
    public bool DefaultBg = false;

    private RectangleF _viewport;
    public RectangleF Viewport
    {
      get { return _viewport; }
      protected set { _viewport = value; }
    }

    private AlertPanel _alertPanel;

    public TouchApp(IMyCubeBlock block, IMyTextSurface surface)
    {
      Screen = new TouchScreen(block, surface);
      TouchSession.Instance.TouchMan.RemoveScreen(block, surface);
      TouchSession.Instance.TouchMan.Screens.Add(Screen);
      Screen.UpdateAtSimulationEvent += UpdateAtSimulation;

      Cursor = new Cursor(Screen);
      Theme = new Theme(surface);

      Viewport = new RectangleF(
        (surface.TextureSize - surface.SurfaceSize) / 2f,
        surface.SurfaceSize
      );

      Position = new Vector2(Viewport.X, Viewport.Y);
      Pixels = new Vector2(Viewport.Width, Viewport.Height);

      App = this;

      CreateAlertPanel();
    }

    protected virtual void CreateAlertPanel()
    {
      if (Screen.Coords == SurfaceCoords.Zero)
      {
        _alertPanel = new AlertPanel("Use 'Screen Calibration' app to calibrate this screen");
        AddChild(_alertPanel);
      }
    }

    public void UpdateClick(bool simLoop = false)
    {
      Screen.WasPressedSinceLastUpdate = simLoop && Screen.Mouse1.IsPressed;
      Screen.WasPressed2SinceLastUpdate = simLoop && Screen.Mouse2.IsPressed;
      Screen.WasPressed3SinceLastUpdate = simLoop && Screen.Mouse3.IsPressed;
    }

    public virtual void UpdateAtSimulation()
    {
      UpdateClick(true);
      UpdateAtSimulationEvent?.Invoke();
    }

    public override void Update()
    {
      base.Update();
      UpdateClick(false);

      if (_alertPanel != null)
        MoveChild(_alertPanel, Children.Count - 1);

      if (DefaultBg)
      {
        BgSprite = new MySprite()
        {
          Type = SpriteType.TEXTURE,
          Data = "Grid",
          RotationOrScale = 0,
          Color = App.Theme.MainColor_2
        };

        Sprites.Clear();

        var size = GetSize();

        BgSprite.Position = Position + new Vector2(0, size.Y / 2);
        BgSprite.Size = new Vector2(Math.Min(size.X * 2, size.Y * 2));

        Sprites.Add(BgSprite);
      }
    }

    public override List<MySprite> GetSprites()
    {
      base.GetSprites();

      if (Screen.IsOnScreen)
        Sprites.AddRange(Cursor.GetSprites());

      return Sprites;
    }

    public override void Dispose()
    {
      if (Screen != null)
      {
        Screen.UpdateAtSimulationEvent -= UpdateAtSimulation;
        Screen.Dispose();
      }
      Cursor?.Dispose();

      TouchSession.Instance.TouchMan.Screens.Remove(Screen);
      UpdateAtSimulationEvent = null;
      InputUtils.SetPlayerKeyboardBlacklistState(false);

      base.Dispose();
    }

  }
}
