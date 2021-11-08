using System;
using System.Collections.Generic;
using Lima.Touch;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Lima.Fancy
{
  public class FancyUI
  {
    protected readonly List<MySprite> sprites = new List<MySprite>();
    private FancyCursor _cursor;

    MyCubeBlock _block;
    RectangleF _viewport;
    TouchableScreen _screen;

    public MyTSSCommon Tss { get; private set; }
    public FancyTheme Theme { get; private set; }
    public FancyAppManager appManager { get; private set; }

    public FancyUI(MyTSSCommon tss)
    {
      Tss = tss;

      _block = tss.Block as MyCubeBlock;
      _viewport = new RectangleF(
        (tss.Surface.TextureSize - tss.Surface.SurfaceSize) / 2f,
        tss.Surface.SurfaceSize
      );
      _screen = new TouchableScreen(_block, tss.Surface as Sandbox.ModAPI.IMyTextSurface);

      TouchManager.Instance.Screens.Add(_screen);

      Theme = new FancyTheme(Tss);
      _cursor = new FancyCursor(_screen, Theme);
      appManager = new FancyAppManager(Tss, _viewport, _cursor, Theme);
    }

    public void UpdateAfterSimulation()
    {
      appManager.UpdateAfterSimulation();
    }

    public List<MySprite> GetSprites()
    {
      sprites.Clear();

      // Add cursor sprites
      sprites.AddRange(appManager.Current.GetSprites());
      sprites.AddRange(_cursor.GetSprites());

      return sprites;
    }

    public void Dispose()
    {
      TouchManager.Instance.Screens.Remove(_screen);
      sprites.Clear();
      _cursor.Dispose();
    }
  }
}