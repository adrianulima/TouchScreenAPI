using Lima.Utils;
using Sandbox.Game.Entities;
using System.Collections.Generic;
using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Touch.UiKit.Elements
{
  public class TouchApp : TouchView
  {
    public event Action UpdateAtSimulationEvent;

    public TouchScreen Screen { get; private set; }
    public TouchCursor Cursor { get; private set; }
    public TouchTheme Theme { get; private set; }

    private RectangleF _viewport;
    public RectangleF Viewport
    {
      get { return _viewport; }
      protected set { _viewport = value; }
    }

    public bool DefaultBg = false;

    public TouchApp() { }

    public virtual void InitApp(MyCubeBlock block, Sandbox.ModAPI.Ingame.IMyTextSurface surface)
    {
      Screen = new TouchScreen(block, surface as Sandbox.ModAPI.IMyTextSurface);
      TouchSession.Instance.TouchMan.RemoveScreen(block, surface as Sandbox.ModAPI.IMyTextSurface);
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
    }

    public virtual void UpdateAtSimulation()
    {
      UpdateAtSimulationEvent?.Invoke();
    }

    public override void Update()
    {
      base.Update();

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
