using Sandbox.ModAPI;
using System.Collections.Generic;
using System.Linq;
using System;
using VRage.Game.ModAPI;
using VRage.Game;
using VRage.Utils;
using VRageMath;
using Lima.Utils;

namespace Lima.Touch
{
  public class TouchManager
  {
    public readonly List<TouchScreen> Screens = new List<TouchScreen>();

    public TouchScreen CurrentScreen;
    private bool _blockClick = false;

    public void UpdateAtSimulation()
    {
      var blockClick = false;
      try
      {
        CurrentScreen = null;

        if (!TouchSession.Instance.ModEnabled
        || MyAPIGateway.Session == null
        // || !MyAPIGateway.Session.CameraController.IsInFirstPersonView
        || MyAPIGateway.Gui.IsCursorVisible)
        {
          if (_blockClick != blockClick)
          {
            InputUtils.SetPlayerUseBlacklistState(blockClick);
            _blockClick = blockClick;
          }
          return;
        }

        var camera = MyAPIGateway.Session.Camera;
        var cameraMatrix = camera.WorldMatrix;
        var camPos = cameraMatrix.Translation;

        double closestDist = -1;
        foreach (var screen in Screens)
        {
          if (!screen.Active)
          {
            screen.UpdateAtSimulation();
            continue;
          }

          var coords = screen.Coords;
          if (coords.IsEmpty())
          {
            screen.UpdateAtSimulation();
            continue;
          }

          Vector3D intersection = screen.CalculateIntersection(cameraMatrix);
          if (intersection == Vector3D.Zero)
          {
            screen.UpdateAtSimulation();
            continue;
          }

          var dist = Vector3D.Distance(camPos, intersection);
          if (dist > screen.InteractiveDistance)
          {
            screen.UpdateAtSimulation();
            continue;
          }

          screen.UpdateScreenCoord();

          if (screen.IsOnScreen && (closestDist < 0 || dist < closestDist))
          {
            closestDist = dist;
            CurrentScreen = screen;
            blockClick = true;
          }

          screen.UpdateAtSimulation();
        }
      }
      catch (Exception e)
      {
        MyLog.Default.WriteLineAndConsole($"{e.Message}\n{e.StackTrace}");

        if (MyAPIGateway.Session?.Player != null)
          MyAPIGateway.Utilities.ShowNotification($"[ ERROR: {GetType().FullName}: {e.Message} ]", 5000, MyFontEnum.Red);
      }

      if (_blockClick != blockClick)
      {
        InputUtils.SetPlayerUseBlacklistState(blockClick);
        _blockClick = blockClick;
      }
    }

    public void RemoveScreen(IMyCubeBlock block, IMyTextSurface surface)
    {
      var screen = Screens.SingleOrDefault(s => s.CompareWithBlockAndSurface(block, surface));
      if (screen != null)
      {
        screen.Dispose();
        Screens.Remove(screen);
      }
    }

    public void Dispose()
    {
      foreach (var screen in Screens)
        screen.Dispose();

      Screens.Clear();
    }
  }
}