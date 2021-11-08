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
  public class TouchManager
  {
    public static TouchManager Instance;

    public float InteractiveDistance = 10f;

    public readonly List<TouchableScreen> Screens = new List<TouchableScreen>();

    public TouchableScreen CurrentScreen;

    public TouchManager()
    {
      if (Instance != null)
        return;

      Instance = this;
    }

    public void Update()
    {
      try
      {
        if (!TouchSession.ModEnabled
        || MyAPIGateway.Session == null
        || !MyAPIGateway.Session.CameraController.IsInFirstPersonView
        || MyAPIGateway.Gui.IsCursorVisible)
        {
          CurrentScreen = null;
          return;
        }

        var camera = MyAPIGateway.Session.Camera;
        var cameraMatrix = camera.WorldMatrix;
        var camPos = cameraMatrix.Translation;

        float closestDist = -1;
        foreach (var screen in Screens)
        {
          var coords = screen.Coords;
          if (coords.IsEmpty())
            continue;

          Vector3D intersection = screen.CalculateIntersection(cameraMatrix);
          var dist = Vector3.Distance(camPos, intersection);
          if (dist >= InteractiveDistance)
            continue;

          screen.UpdateScreenCoord();

          if (screen.IsOnScreen && (closestDist < 0 || dist < closestDist))
          {
            closestDist = dist;
            CurrentScreen = screen;
          }
        }
      }
      catch (Exception e)
      {
        MyLog.Default.WriteLineAndConsole($"{e.Message}\n{e.StackTrace}");

        if (MyAPIGateway.Session?.Player != null)
          MyAPIGateway.Utilities.ShowNotification($"[ ERROR: {GetType().FullName}: {e.Message} ]", 5000, MyFontEnum.Red);
      }

    }
  }
}