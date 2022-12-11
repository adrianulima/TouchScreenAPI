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
    private MySprite bgSprite;

    public event Action UpdateEvent;

    public TouchScreen Screen { get; private set; }
    public FancyCursor Cursor { get; private set; }
    public FancyTheme Theme { get; private set; }

    public FancyApp() { _app = this; }

    public virtual void InitApp(MyCubeBlock block, Sandbox.ModAPI.Ingame.IMyTextSurface surface)
    {
      Screen = new TouchScreen(block, surface as Sandbox.ModAPI.IMyTextSurface);
      TouchSession.Instance.TouchMan.RemoveScreen(block, surface as Sandbox.ModAPI.IMyTextSurface);
      TouchSession.Instance.TouchMan.Screens.Add(Screen);
      Screen.UpdateEvent += UpdateAfterSimulation;

      Cursor = new FancyCursor(Screen);
      Theme = new FancyTheme(surface);

      _viewport = new RectangleF(
        (surface.TextureSize - surface.SurfaceSize) / 2f,
        surface.SurfaceSize
      );

      _size = new Vector2(_viewport.Width, _viewport.Height);
      Position = new Vector2(_viewport.X / _size.X, _viewport.Y / _size.Y);
    }

    public virtual void UpdateAfterSimulation()
    {
      UpdateEvent?.Invoke();
    }

    public override void Update()
    {
      base.Update();

      bgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "Grid",//"MyObjectBuilder_Component/MetalGrid",
        RotationOrScale = 0,
        Color = App.Theme.Main_10
      };

      sprites.Clear();

      bgSprite.Position = Position + new Vector2(0, Size.Y / 2);
      var s = Math.Min(Size.X * 2, Size.Y * 2);
      bgSprite.Size = new Vector2(s);

      sprites.Add(bgSprite);
    }

    public override List<MySprite> GetSprites()
    {
      Update();
      base.GetSprites();

      sprites.AddRange(Cursor.GetSprites());

      return sprites;
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
