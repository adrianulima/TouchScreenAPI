using Lima.Utils;
using Sandbox.ModAPI;
using System.Collections.Generic;
using System;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRageMath;

namespace Lima.Touch.UiKit.Elements
{
  public class TouchApp : TouchView
  {
    public event Action UpdateAtSimulationEvent;

    public TouchScreen Screen { get; private set; }
    public TouchCursor Cursor { get; private set; }
    public TouchTheme Theme { get; private set; }
    public bool DefaultBg = false;

    private RectangleF _viewport;
    public RectangleF Viewport
    {
      get { return _viewport; }
      protected set { _viewport = value; }
    }

    private TouchView _alertView;

    public TouchApp() { }

    public virtual void InitApp(IMyCubeBlock block, IMyTextSurface surface)
    {
      Screen = new TouchScreen(block, surface);
      TouchSession.Instance.TouchMan.RemoveScreen(block, surface);
      TouchSession.Instance.TouchMan.Screens.Add(Screen);
      Screen.UpdateAtSimulationEvent += UpdateAtSimulation;

      Cursor = new TouchCursor(Screen);
      Theme = new TouchTheme(surface);

      Viewport = new RectangleF(
        (surface.TextureSize - surface.SurfaceSize) / 2f,
        surface.SurfaceSize
      );

      Position = new Vector2(Viewport.X, Viewport.Y);
      Pixels = new Vector2(Viewport.Width, Viewport.Height);

      App = this;

      if (Screen.Coords == SurfaceCoords.Zero)
      {
        _alertView = new TouchView();
        _alertView.Absolute = true;
        _alertView.Alignment = ViewAlignment.Center;
        _alertView.Anchor = ViewAnchor.Center;
        _alertView.Border = new Vector4(2);
        _alertView.BorderColor = Color.DarkRed;
        _alertView.BgColor = Color.Black;
        _alertView.Pixels = new Vector2(200, 50);
        _alertView.Scale = Vector2.Zero;
        _alertView.Position = Position + new Vector2(Pixels.X * 0.5f - 100, Pixels.Y * 0.5f - 25);

        var label = new TouchLabel("Use 'Screen Calibration' app\nto calibrate this screen");
        _alertView.AddChild(label);
        AddChild(_alertView);
      }
    }

    public virtual void UpdateAtSimulation()
    {
      UpdateAtSimulationEvent?.Invoke();
    }

    public override void Update()
    {
      base.Update();

      if (_alertView != null)
        MoveChild(_alertView, Children.Count - 1);

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
