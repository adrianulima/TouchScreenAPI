using System;
using System.Collections.Generic;
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
  public class FancyAppManager
  {
    private readonly List<FancyApp> apps = new List<FancyApp>();
    private MyTSSCommon _tss;
    public MyTSSCommon Tss { get { return _tss; } }

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

    public RectangleF Viewport { get; protected set; }
    public FancyCursor Cursor { get; protected set; }
    public FancyTheme Theme { get; protected set; }

    public FancyAppManager(MyTSSCommon tss, RectangleF viewport, FancyCursor cursor, FancyTheme theme)
    {
      _tss = tss;
      Viewport = viewport;
      Cursor = cursor;
      Theme = theme;
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

  }
}