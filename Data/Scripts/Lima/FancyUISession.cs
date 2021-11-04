using System;
using System.Collections.Generic;
using Lima.Fancy;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;

namespace Lima
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  public class FancyUISession : MySessionComponentBase
  {
    public static FancyUISession Instance;

    private readonly Dictionary<IMyTextSurface, FancyUI> _surfaceUIDict = new Dictionary<IMyTextSurface, FancyUI>();

    public override void LoadData()
    {
      Instance = this;
    }

    protected override void UnloadData()
    {
      Instance = null;
    }

    public override void UpdateAfterSimulation()
    {
      if (MyAPIGateway.Utilities?.IsDedicated == true)
        return;

      try
      {
        foreach (KeyValuePair<IMyTextSurface, FancyUI> entry in _surfaceUIDict)
        {
          // entry.Key
          entry.Value?.UpdateAfterSimulation();
        }
      }
      catch (Exception e)
      {
        MyLog.Default.WriteLineAndConsole($"{e.Message}\n{e.StackTrace}");

        if (MyAPIGateway.Session?.Player != null)
          MyAPIGateway.Utilities.ShowNotification($"[ ERROR: {GetType().FullName}: {e.Message} ]", 2000, MyFontEnum.Red);
      }
    }

    public void AddSurfaceUI(IMyTextSurface surface, FancyUI ui)
    {
      _surfaceUIDict[surface] = ui;
    }

    public FancyUI GetUI(IMyTextSurface surface)
    {
      return _surfaceUIDict[surface];
    }

    public void RemoveSurfaceUI(IMyTextSurface surface)
    {
      _surfaceUIDict.Remove(surface);
    }

  }
}