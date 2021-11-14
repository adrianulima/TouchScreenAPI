using System;
using System.Collections.Generic;
using Lima.Touch;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI.Ingame;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Utils;
using VRageMath;

namespace Lima.Fancy
{
  public class FancyAppManager
  {
    private readonly List<MySprite> sprites = new List<MySprite>();
    private readonly List<FancyApp> apps = new List<FancyApp>();

    public TouchScreen Screen { get; private set; }
    public MyCubeBlock Block { get; private set; }
    public Sandbox.ModAPI.Ingame.IMyTextSurface Surface { get; private set; }
    public RectangleF Viewport { get; private set; }
    public FancyCursor Cursor { get; private set; }
    public FancyTheme Theme { get; private set; }

    private int _currentIndex = 0;
    public int CurrentIndex
    {
      get { return _currentIndex; }
      set { if ((value >= 0) && (value < apps.Count)) _currentIndex = value; }
    }
    public FancyApp Current
    {
      get { return apps[_currentIndex]; }
      set { CurrentIndex = apps.IndexOf(value); }
    }

    public FancyAppManager(MyCubeBlock block, Sandbox.ModAPI.Ingame.IMyTextSurface surface)
    {
      Block = block;
      Surface = surface;

      // TODO: Move to Init()
      Screen = new TouchScreen(block, Surface as Sandbox.ModAPI.IMyTextSurface);
      TouchManager.Instance.RemoveScreen(block, Surface as Sandbox.ModAPI.IMyTextSurface);
      TouchManager.Instance.Screens.Add(Screen);
      Screen.UpdateEvent += UpdateAfterSimulation;

      Cursor = new FancyCursor(Screen);
      Theme = new FancyTheme(Surface);

      Viewport = new RectangleF(
        (surface.TextureSize - surface.SurfaceSize) / 2f,
        surface.SurfaceSize
      );

      var homeApp = new FancyApp();
      homeApp.InitApp(this);
      apps.Add(homeApp);
    }

    public void UpdateAfterSimulation()
    {
      foreach (var app in apps)
      {
        app.UpdateAfterSimulation();
      }
    }

    public T AddApp<T>() where T : FancyApp, new()
    {
      var app = new T();
      app.InitApp(this);
      apps.Add(app);
      return app;
    }

    public List<MySprite> GetSprites()
    {
      sprites.Clear();

      sprites.AddRange(Current.GetSprites());
      sprites.AddRange(Cursor.GetSprites());

      return sprites;
    }

    public void Dispose()
    {
      Screen.UpdateEvent -= UpdateAfterSimulation;
      Screen.Dispose();
      TouchManager.Instance.Screens.Remove(Screen);
      sprites.Clear();
      Cursor.Dispose();
    }

  }
}