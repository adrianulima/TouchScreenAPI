using Lima.Touch;
using Sandbox.Game;
using Sandbox.ModAPI;
using System.Collections.Generic;
using System;
using VRage.Input;
using VRage.Utils;

namespace Lima.Utils
{
  internal static class InputUtils
  {
    private static MyKeys[] _keys;
    public static MyKeys[] Keys
    {
      get
      {
        if (_keys == null)
          _keys = Enum.GetValues(typeof(MyKeys)) as MyKeys[];
        return _keys;
      }
    }

    private static List<string> _controlUseIDs;
    private static List<string> _controlIDs;
    public static List<string> ControlIDs
    {
      get
      {
        if (_controlIDs == null)
        {
          _controlIDs = new List<string>(Keys.Length);
          foreach (MyKeys key in Keys)
          {
            if (key == MyKeys.Alt)
              continue;
            MyStringId? id = MyAPIGateway.Input.GetControl(key)?.GetGameControlEnum();
            string stringID = id?.ToString();
            if (stringID != null && stringID.Length > 0)
              _controlIDs.Add(stringID);
          }
          _controlIDs.Add(MyControlsSpace.TOGGLE_HUD.String);
          _controlIDs.Add(MyControlsSpace.COLOR_PICKER.String);
          _controlIDs.Add(MyControlsSpace.USE.String);
          _controlIDs.Add(MyControlsSpace.CROUCH.String);
        }
        return _controlIDs;
      }
    }
    internal static void SetPlayerKeyboardBlacklistState(bool blocked)
    {
      // TODO: Consider setting the value back to what it was before instead of `blocked`
      if (MyAPIGateway.Session?.Player != null)
        TouchSession.Instance?.BlacklistStateHandler?.SetPlayerInputBlacklistState(ControlIDs, MyAPIGateway.Session.Player.IdentityId, !blocked);
    }
    internal static void SetPlayerUseBlacklistState(bool blocked)
    {
      if (MyAPIGateway.Session?.Player != null)
      {
        if (_controlUseIDs == null)
        {
          _controlUseIDs = new List<string>(2);
          // _controlUseIDs.Add(MyControlsSpace.USE.String);
          _controlUseIDs.Add(MyControlsSpace.PRIMARY_TOOL_ACTION.String);
          _controlUseIDs.Add(MyControlsSpace.SECONDARY_TOOL_ACTION.String);
        }

        TouchSession.Instance?.BlacklistStateHandler?.SetPlayerInputBlacklistState(_controlUseIDs, MyAPIGateway.Session.Player.IdentityId, !blocked);
      }
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