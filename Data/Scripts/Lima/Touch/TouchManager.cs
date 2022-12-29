using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Lima.Touch
{
  public class TouchManager
  {
    public float MaxInteractiveDistance = 50f;
    public float DefaultInteractiveDistance = 10f;

    // TODO: Implement Dispose to clear this array and possible more garbage
    public readonly List<TouchScreen> Screens = new List<TouchScreen>();

    public TouchScreen CurrentScreen;

    public void UpdateAfterSimulation()
    {
      try
      {
        CurrentScreen = null;

        if (!TouchSession.Instance.ModEnabled
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
          {
            screen.UpdateAfterSimulation();
            continue;
          }

          var coords = screen.Coords;
          if (coords.IsEmpty())
          {
            screen.UpdateAfterSimulation();
            continue;
          }

          Vector3D intersection = screen.CalculateIntersection(cameraMatrix);
          if (intersection == Vector3D.Zero)
          {
            screen.UpdateAfterSimulation();
            continue;
          }

          var dist = Vector3.Distance(camPos, intersection);
          if (dist > screen.InteractiveDistance)
          {
            screen.UpdateAfterSimulation();
            continue;
          }

          screen.UpdateScreenCoord();

          if (screen.IsOnScreen && (closestDist < 0 || dist < closestDist))
          {
            closestDist = dist;
            CurrentScreen = screen;
          }

          screen.UpdateAfterSimulation();
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
      var screen = TouchSession.Instance.TouchMan.Screens.SingleOrDefault(s => s.CompareWithBlockAndSurface(block, surface));
      if (screen != null)
      {
        screen.Dispose();
        Screens.Remove(screen);
      }
    }
  }
}