using System;
using System.Collections.Generic;
using Lima.Fancy.Elements;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI.Ingame;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Lima.Fancy
{
  public class FancyApp : FancyView
  {
    private MySprite bgSprite;

    public event Action UpdateEvent;

    public FancyAppManager AppManager;
    public FancyCursor Cursor { get { return AppManager.Cursor; } }
    public FancyTheme Theme { get { return AppManager.Theme; } }

    public FancyApp() { _app = this; }

    public virtual void InitApp(FancyAppManager appManager)
    {
      AppManager = appManager;

      _viewport = appManager.Viewport;
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
      return base.GetSprites();
    }

  }
}
