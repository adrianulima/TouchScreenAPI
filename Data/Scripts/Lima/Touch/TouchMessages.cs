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

namespace Lima.Touch
{
  public static class TouchMessages
  {
    private const long _touchApiModId = 123;

    public static void SendApiToMods()
    {
      //Create a dictionary of delegates that point to methods in the Touch API code
      //Send the dictionary to other mods that registered to this ID
      MyAPIGateway.Utilities.SendModMessage(_touchApiModId, GetApiDictionary());
    }

    public static Dictionary<string, Delegate> GetApiDictionary()
    {
      var dict = new Dictionary<string, Delegate>();
      dict.Add("GetRaycastStrike", new Func<IHitInfo>(GetRaycastStrike));
      dict.Add("GetRaycastStrikeDistance", new Func<float>(GetRaycastStrikeDistance));

      return dict;
    }

    public static IHitInfo GetRaycastStrike()
    {
      return null;
    }

    public static float GetRaycastStrikeDistance()
    {
      return 0f;
    }


  }
}