using System;
using System.Collections.Generic;
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

//Change namespace to your mod's namespace
namespace Lima.Touch
{
  public class TouchAPI
  {
    private const long _touchApiModId = 123;
    private static TouchAPI instance;

    public bool TouchApiReady;

    private Func<IHitInfo> _getRaycastStrike;
    private Func<float> _getRaycastStrikeDistance;

    public TouchAPI()
    {
      if (instance != null)
        return;

      MyAPIGateway.Utilities.RegisterMessageHandler(_touchApiModId, TouchAPIListener);
      instance = this;
    }

    public void Unload()
    {
      MyAPIGateway.Utilities.UnregisterMessageHandler(_touchApiModId, TouchAPIListener);

      if (instance == this)
        instance = null;
    }

    // Raycast API
    public IHitInfo GetRaycastStrike() => _getRaycastStrike?.Invoke();
    public float GetRaycastStrikeDistance() => _getRaycastStrikeDistance?.Invoke() ?? -1f;

    public void TouchAPIListener(object data)
    {
      try
      {
        var dict = data as Dictionary<string, Delegate>;

        if (dict == null)
          return;

        TouchApiReady = true;
        _getRaycastStrike = (Func<IHitInfo>)dict["GetRaycastStrike"];
        _getRaycastStrikeDistance = (Func<float>)dict["GetRaycastStrikeDistance"];

      }
      catch (Exception e)
      {
        MyLog.Default.WriteLineAndConsole("TouchAPI Failed To Load For Client: " + MyAPIGateway.Utilities.GamePaths.ModScopeName);
      }
    }
  }
}