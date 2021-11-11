using System;
using System.Collections.Generic;
using System.Linq;
using Lima.Fancy;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Lima.Touch
{
  public static class TouchMessages
  {
    // TODO: Replace with proper TouchAPI mod id
    private const long _channel = 123;

    public static void SendApiToMods()
    {
      //Create a dictionary of delegates that point to methods in the Touch API code
      //Send the dictionary to other mods that registered to this ID
      MyAPIGateway.Utilities.SendModMessage(_channel, GetApiDictionary());
    }

    // TODO: Register this method
    private static void HandleMessage(object msg)
    {
      if ((msg as string) == "ApiEndpointRequest")
        MyAPIGateway.Utilities.SendModMessage(_channel, GetApiDictionary());
    }

    public static Dictionary<string, Delegate> GetApiDictionary()
    {
      var dict = new Dictionary<string, Delegate>();

      dict.Add("GetMaxInteractiveDistance", new Func<float>(GetMaxInteractiveDistance));
      dict.Add("SetMaxInteractiveDistance", new Action<float>(SetMaxInteractiveDistance));
      dict.Add("CreateTouchScreen", new Func<IMyCubeBlock, IMyTextSurface, ITouchScreen>(CreateTouchScreen));
      dict.Add("RemoveTouchScreen", new Action<IMyCubeBlock, IMyTextSurface>(RemoveTouchScreen));
      dict.Add("AddSurfaceCoords", new Action<string>(AddSurfaceCoords));
      dict.Add("RemoveSurfaceCoords", new Action<string>(RemoveSurfaceCoords));

      return dict;
    }

    public static ITouchScreen CreateTouchScreen(IMyCubeBlock block, IMyTextSurface surface)
    {
      var screen = new TouchScreen(block, surface);
      TouchManager.Instance.Screens.Add(screen);
      return (ITouchScreen)screen;
    }

    public static void RemoveTouchScreen(IMyCubeBlock block, IMyTextSurface surface)
    {
      TouchManager.Instance.RemoveScreen(block, surface);
    }

    public static List<ITouchScreen> GetTouchScreensList()
    {
      return TouchManager.Instance.Screens.ToList<ITouchScreen>();
    }

    public static ITouchScreen GetTargetTouchScreen()
    {
      return TouchManager.Instance.CurrentScreen;
    }

    public static float GetMaxInteractiveDistance()
    {
      return TouchManager.Instance.MaxInteractiveDistance;
    }

    public static void SetMaxInteractiveDistance(float distance)
    {
      TouchManager.Instance.MaxInteractiveDistance = distance;
    }

    public static void AddSurfaceCoords(string coords)
    {
      SurfaceCoordsManager.Instance.AddSurfaceCoords(coords);
    }

    public static void RemoveSurfaceCoords(string coords)
    {
      var index = SurfaceCoordsManager.Instance.CoordsList.IndexOf(coords);
      if (index >= 0)
        SurfaceCoordsManager.Instance.CoordsList.RemoveAt(index);
    }

  }
}