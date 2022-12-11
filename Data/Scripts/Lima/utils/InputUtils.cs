using System;
using System.Collections.Generic;
using Sandbox.ModAPI;
using VRage.Input;
using VRage.Utils;
using Sandbox.Game;

namespace Lima.Utils
{
  internal static class InputUtils
  {
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
    internal static void SetPlayerKeyboardBlacklistState(bool blocked)
    {
      if (MyAPIGateway.Session?.Player != null)
        foreach (string control in ControlIDs)
          MyVisualScriptLogicProvider.SetPlayerInputBlacklistState(control, MyAPIGateway.Session.Player.IdentityId, !blocked);
    }

    internal static bool CheckNumericInput(string text, char ch, bool allowNegative = true, bool isInt = false)
    {
      if (ch == '-')
        return allowNegative && text.Length == 0;

      if (ch == '.')
        return !isInt && text.Length > 0 && !text.Contains(".");

      return char.IsDigit(ch);
    }
  }
}