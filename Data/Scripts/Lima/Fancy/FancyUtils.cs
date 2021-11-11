using System;
using System.Collections.Generic;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using Sandbox.Game;

namespace Lima.Fancy
{
  internal static class FancyUtils
  {
    public static void Log(string text, int ms = 166)
    {
      MyAPIGateway.Utilities.ShowNotification(text, ms, MyFontEnum.Red);
    }

    public static Vector3D GetLinePlaneIntersection(Vector3D planePoint, Vector3D planeNormal, Vector3D linePoint, Vector3D lineDirection, bool ignoreDirection = false)
    {
      var planeDotLine = planeNormal.Dot(Vector3D.Normalize(lineDirection));
      if (planeDotLine == 0 || (!ignoreDirection && planeDotLine >= 0))
      {
        return Vector3D.Zero;
      }

      double t = (planeNormal.Dot(planePoint) - planeNormal.Dot(linePoint)) / planeDotLine;
      if (!ignoreDirection && t <= 0)
      {
        return Vector3D.Zero;
      }
      return linePoint + (Vector3D.Normalize(lineDirection) * t);
    }

    public static Vector2 GetPointOnPlane(Vector3D point, Vector3D planePoint, Vector3D planeUp, Vector3D planeRight)
    {
      var x = planeRight.Dot(planePoint - point);
      var y = planeUp.Dot(planePoint - point);
      return new Vector2((float)x, (float)y);
    }

    public static int GetSurfaceIndex(Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider provider, IMyTextSurface surface)
    {
      var count = provider.SurfaceCount;
      for (int i = 0; i < count; i++)
      {
        if (provider.GetSurface(i) == surface)
          return i;
      }
      return -1;
    }

    public static Vector3 LocalToGlobal(Vector3 localPos, MatrixD worldMatrix)
    {
      return Vector3D.Transform(localPos, worldMatrix);
    }

    public static Vector3 GlobalToLocal(Vector3D globalPos, MatrixD worldMatrix)
    {
      return Vector3D.TransformNormal(globalPos - worldMatrix.Translation, MatrixD.Transpose(worldMatrix));
    }

    public static Vector3 GlobalToLocalSlowerAlternative(Vector3D globalPos, MatrixD worldMatrixNormalizedInv)
    {
      return Vector3D.Transform(globalPos, worldMatrixNormalizedInv);
    }

    public static Color Opac(Color color)
    {
      return new Color(color, 1);
    }

    private static Random random = new Random();
    public static int GetRandomInt(int min, int max)
    {
      return random.Next(min, max);
    }

    private static MyKeys[] _keys;
    private static MyKeys[] Keys
    {
      get
      {
        if (_keys == null)
          _keys = Enum.GetValues(typeof(MyKeys)) as MyKeys[];
        return _keys;
      }
    }

    private static List<string> _controlIDs;
    private static List<string> ControlIDs
    {
      get
      {
        if (_controlIDs == null)
        {
          _controlIDs = new List<string>(Keys.Length);
          foreach (MyKeys key in Keys)
          {
            MyStringId? id = MyAPIGateway.Input.GetControl(key)?.GetGameControlEnum();
            string stringID = id?.ToString();
            if (stringID != null && stringID.Length > 0)
              _controlIDs.Add(stringID);
          }
          _controlIDs.Add(MyControlsSpace.TOGGLE_HUD.ToString());
          _controlIDs.Add(MyControlsSpace.COLOR_PICKER.ToString());
        }
        return _controlIDs;
      }
    }
    public static void SetPlayerKeyboardBlacklistState(bool blocked)
    {
      if (MyAPIGateway.Session?.Player != null)
        foreach (string control in ControlIDs)
          MyVisualScriptLogicProvider.SetPlayerInputBlacklistState(control, MyAPIGateway.Session.Player.IdentityId, !blocked);
    }

    public static bool CheckNumericInput(string text, char ch, bool allowNegative = true, bool isInt = false)
    {
      if (ch == '-')
        return allowNegative && text.Length == 0;

      if (ch == '.')
        return !isInt && text.Length > 0 && !text.Contains(".");

      return char.IsDigit(ch);
    }

    public static Vector2 RotateScreenCoord(Vector2 coord, int rotate)
    {
      if (rotate == 0)
        return coord;
      else if (rotate == 1)
        return new Vector2(coord.Y, 1 - coord.X);
      else if (rotate == 2)
        return new Vector2(1 - coord.X, 1 - coord.Y);
      else
        return new Vector2(1 - coord.Y, coord.X);
    }
  }
}