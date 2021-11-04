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

    public static Color Opac(Color color)
    {
      return new Color(color, 1);
    }

    static Random random = new Random();
    public static int GetRandomInt(int min, int max)
    {
      return random.Next(min, max);
    }


    static MyKeys[] _keys;
    static MyKeys[] Keys
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
  }
}