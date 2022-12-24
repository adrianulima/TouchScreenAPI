using System;
using System.Collections.Generic;
using Lima.Fancy.Elements;
using Lima.Touch;
using Sandbox.Game.Entities;
// using Sandbox.ModAPI;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy
{
  public class FancyApp : FancyView
  {
    public event Action UpdateAfterSimulationEvent;

    public TouchScreen Screen { get; private set; }
    public FancyCursor Cursor { get; private set; }
    public FancyTheme Theme { get; private set; }

    private RectangleF _viewport;
    public RectangleF Viewport
    {
      get { return _viewport; }
      protected set { _viewport = value; }
    }

    public FancyApp() { App = this; }

    public virtual void InitApp(MyCubeBlock block, Sandbox.ModAPI.Ingame.IMyTextSurface surface)
    {
      Screen = new TouchScreen(block, surface as Sandbox.ModAPI.IMyTextSurface);
      TouchSession.Instance.TouchMan.RemoveScreen(block, surface as Sandbox.ModAPI.IMyTextSurface);
      TouchSession.Instance.TouchMan.Screens.Add(Screen);
      Screen.UpdateAfterSimulationEvent += UpdateAfterSimulation;

      Cursor = new FancyCursor(Screen);
      Theme = new FancyTheme(surface);

      Viewport = new RectangleF(
        (surface.TextureSize - surface.SurfaceSize) / 2f,
        surface.SurfaceSize
      );

      Position = new Vector2(Viewport.X, Viewport.Y);
      Pixels = new Vector2(Viewport.Width, Viewport.Height);
    }

    public virtual void UpdateAfterSimulation()
    {
      UpdateAfterSimulationEvent?.Invoke();
    }

    // int prevScroll = 0;
    public override void Update()
    {
      base.Update();

      // var newScroll = MyAPIGateway.Input.MouseScrollWheelValue();
      // if (newScroll - prevScroll != 0)
      // {
      //   this.Theme.Scale += Math.Sign(newScroll - prevScroll) * 0.05f;
      //   this.Theme.Scale = Math.Max(this.Theme.Scale, 0.5f);
      //   prevScroll = newScroll;
      // }

      BgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "Grid",
        RotationOrScale = 0,
        Color = App.Theme.Main_20
      };

      Sprites.Clear();

      var size = GetSize();

      BgSprite.Position = Position + new Vector2(0, size.Y / 2);
      BgSprite.Size = new Vector2(Math.Min(size.X * 2, size.Y * 2));

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
      Screen.UpdateAfterSimulationEvent -= UpdateAfterSimulation;
      Screen.Dispose();
      Cursor.Dispose();
      TouchSession.Instance.TouchMan.Screens.Remove(Screen);
      UpdateAfterSimulationEvent = null;

      base.Dispose();
    }

  }
}
