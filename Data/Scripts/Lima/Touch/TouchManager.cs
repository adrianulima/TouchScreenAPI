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
  public class TouchManager
  {
    public static TouchManager Instance;

    public float MaxInteractiveDistance = 50f;
    public float DefaultInteractiveDistance = 10f;

    public readonly List<TouchScreen> Screens = new List<TouchScreen>();

    public TouchScreen CurrentScreen;

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
        CurrentScreen = null;

        if (!TouchSession.ModEnabled
        || MyAPIGateway.Session == null
        || !MyAPIGateway.Session.CameraController.IsInFirstPersonView
        || MyAPIGateway.Gui.IsCursorVisible)
        {
          return;
        }

        var camera = MyAPIGateway.Session.Camera;
        var cameraMatrix = camera.WorldMatrix;
        var camPos = cameraMatrix.Translation;

        float closestDist = -1;
        foreach (var screen in Screens)
        {
          if (!screen.Active)
            continue;

          var coords = screen.Coords;
          if (coords.IsEmpty())
            continue;

          Vector3D intersection = screen.CalculateIntersection(cameraMatrix);
          if (intersection == Vector3D.Zero)
            continue;

          var dist = Vector3.Distance(camPos, intersection);
          if (dist > screen.InteractiveDistance)
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

    public void RemoveScreen(IMyCubeBlock block, IMyTextSurface surface)
    {
      var screen = TouchManager.Instance.Screens.SingleOrDefault(s => s.CompareWithBlockAndSurface(block, surface));
      if (screen != null)
        Screens.Remove(screen);
    }
  }
}