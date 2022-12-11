using System;
using System.Collections.Generic;
using Lima.Fancy.Elements;
using Lima.Touch;
using Sandbox.Game.Entities;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy
{
  public class FancyApp : FancyView
  {
    public event Action UpdateEvent;

    public TouchScreen Screen { get; private set; }
    public FancyCursor Cursor { get; private set; }
    public FancyTheme Theme { get; private set; }

    public FancyApp() { App = this; }

    public virtual void InitApp(MyCubeBlock block, Sandbox.ModAPI.Ingame.IMyTextSurface surface)
    {
      Screen = new TouchScreen(block, surface as Sandbox.ModAPI.IMyTextSurface);
      TouchSession.Instance.TouchMan.RemoveScreen(block, surface as Sandbox.ModAPI.IMyTextSurface);
      TouchSession.Instance.TouchMan.Screens.Add(Screen);
      Screen.UpdateEvent += UpdateAfterSimulation;

      Cursor = new FancyCursor(Screen);
      Theme = new FancyTheme(surface);

      Viewport = new RectangleF(
        (surface.TextureSize - surface.SurfaceSize) / 2f,
        surface.SurfaceSize
      );

      Size = new Vector2(Viewport.Width, Viewport.Height);
      Position = new Vector2(Viewport.X / Size.X, Viewport.Y / Size.Y);
    }

    public virtual void UpdateAfterSimulation()
    {
      UpdateEvent?.Invoke();
    }

    public override void Update()
    {
      base.Update();

      BgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "Grid",
        RotationOrScale = 0,
        Color = App.Theme.Main_10
      };

      Sprites.Clear();

      BgSprite.Position = Position + new Vector2(0, Size.Y / 2);
      var s = Math.Min(Size.X * 2, Size.Y * 2);
      BgSprite.Size = new Vector2(s);

      Sprites.Add(BgSprite);
    }

    public override List<MySprite> GetSprites()
    {
      Update();
      base.GetSprites();

      Sprites.AddRange(Cursor.GetSprites());

      return Sprites;
    }

    public override void Dispose()
    {
      Screen.UpdateEvent -= UpdateAfterSimulation;
      Screen.Dispose();
      Cursor.Dispose();
      TouchSession.Instance.TouchMan.Screens.Remove(Screen);
      UpdateEvent = null;

      base.Dispose();
    }

  }
}
