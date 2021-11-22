using System;
using System.Collections.Generic;
using System.Linq;
using Lima.Fancy;
using Lima.Fancy.Elements;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRageMath;

namespace Lima.Touch
{
  public static class TouchDelegates
  {
    // TODO: Replace with proper TouchAPI mod id
    private const long _channel = 123;

    public static void SendApiToMods()
    {
      //Create a dictionary of delegates that point to methods in the Touch API code
      //Send the dictionary to other mods that registered to this ID
      MyAPIGateway.Utilities.SendModMessage(_channel, GetApiDictionary());
    }

    // TODO: Register this method
    private static void HandleMessage(object msg)
    {
      if ((msg as string) == "ApiEndpointRequest")
        MyAPIGateway.Utilities.SendModMessage(_channel, GetApiDictionary());
    }

    public static Dictionary<string, Delegate> GetApiDictionary()
    {
      var dict = new Dictionary<string, Delegate>();

      dict.Add("GetMaxInteractiveDistance", new Func<float>(GetMaxInteractiveDistance));
      dict.Add("SetMaxInteractiveDistance", new Action<float>(SetMaxInteractiveDistance));
      dict.Add("CreateTouchScreen", new Func<IMyCubeBlock, IMyTextSurface, TouchScreen>(CreateTouchScreen));
      dict.Add("RemoveTouchScreen", new Action<IMyCubeBlock, IMyTextSurface>(RemoveTouchScreen));
      dict.Add("AddSurfaceCoords", new Action<string>(AddSurfaceCoords));
      dict.Add("RemoveSurfaceCoords", new Action<string>(RemoveSurfaceCoords));

      dict.Add("TouchScreen_GetBlock", new Func<object, IMyCubeBlock>(TouchScreen_GetBlock));
      dict.Add("TouchScreen_GetSurface", new Func<object, IMyTextSurface>(TouchScreen_GetSurface));
      dict.Add("TouchScreen_GetIndex", new Func<object, int>(TouchScreen_GetIndex));
      dict.Add("TouchScreen_IsOnScreen", new Func<object, bool>(TouchScreen_IsOnScreen));
      dict.Add("TouchScreen_GetCursorPosition", new Func<object, Vector2>(TouchScreen_GetCursorPosition));
      dict.Add("TouchScreen_GetInteractiveDistance", new Func<object, float>(TouchScreen_GetInteractiveDistance));
      dict.Add("TouchScreen_SetInteractiveDistance", new Action<object, float>(TouchScreen_SetInteractiveDistance));
      dict.Add("TouchScreen_GetScreenRotate", new Func<object, int>(TouchScreen_GetScreenRotate));
      dict.Add("TouchScreen_CompareWithBlockAndSurface", new Func<object, IMyCubeBlock, IMyTextSurface, bool>(TouchScreen_CompareWithBlockAndSurface));
      dict.Add("TouchScreen_Dispose", new Action<object>(TouchScreen_Dispose));

      dict.Add("FancyElementBase_GetPosition", new Func<object, Vector2>(FancyElementBase_GetPosition));
      dict.Add("FancyElementBase_SetPosition", new Action<object, Vector2>(FancyElementBase_SetPosition));
      dict.Add("FancyElementBase_GetMargin", new Func<object, Vector4>(FancyElementBase_GetMargin));
      dict.Add("FancyElementBase_SetMargin", new Action<object, Vector4>(FancyElementBase_SetMargin));
      dict.Add("FancyElementBase_GetScale", new Func<object, Vector2>(FancyElementBase_GetScale));
      dict.Add("FancyElementBase_SetScale", new Action<object, Vector2>(FancyElementBase_SetScale));
      dict.Add("FancyElementBase_GetPixels", new Func<object, Vector2>(FancyElementBase_GetPixels));
      dict.Add("FancyElementBase_SetPixels", new Action<object, Vector2>(FancyElementBase_SetPixels));
      dict.Add("FancyElementBase_GetSize", new Func<object, Vector2>(FancyElementBase_GetSize));
      dict.Add("FancyElementBase_GetViewport", new Func<object, RectangleF>(FancyElementBase_GetViewport));
      dict.Add("FancyElementBase_GetApp", new Func<object, FancyApp>(FancyElementBase_GetApp));
      dict.Add("FancyElementBase_GetParent", new Func<object, FancyElementContainerBase>(FancyElementBase_GetParent));
      dict.Add("FancyElementBase_GetOffset", new Func<object, Vector2>(FancyElementBase_GetOffset));
      dict.Add("FancyElementBase_GetSprites", new Func<object, List<MySprite>>(FancyElementBase_GetSprites));
      dict.Add("FancyElementBase_InitElements", new Action<object>(FancyElementBase_InitElements));
      dict.Add("FancyElementBase_Update", new Action<object>(FancyElementBase_Update));
      dict.Add("FancyElementBase_Dispose", new Action<object>(FancyElementBase_Dispose));

      dict.Add("FancyElementContainerBase_GetChildren", new Func<object, List<object>>(FancyElementContainerBase_GetChildren));
      dict.Add("FancyElementContainerBase_AddChild", new Action<object, object>(FancyElementContainerBase_AddChild));
      dict.Add("FancyElementContainerBase_RemoveChild", new Action<object, object>(FancyElementContainerBase_RemoveChild));

      dict.Add("FancyView_NewV", new Func<int, FancyView>(FancyView_NewV));
      dict.Add("FancyView_GetDirection", new Func<object, int>(FancyView_GetDirection));
      dict.Add("FancyView_SetDirection", new Action<object, int>(FancyView_SetDirection));

      dict.Add("FancyApp_New", new Func<FancyApp>(FancyApp_New));
      dict.Add("FancyApp_GetScreen", new Func<object, TouchScreen>(FancyApp_GetScreen));
      dict.Add("FancyApp_GetCursor", new Func<object, FancyCursor>(FancyApp_GetCursor));
      dict.Add("FancyApp_GetTheme", new Func<object, FancyTheme>(FancyApp_GetTheme));
      dict.Add("FancyApp_InitApp", new Action<object, MyCubeBlock, Sandbox.ModAPI.Ingame.IMyTextSurface>(FancyApp_InitApp));

      dict.Add("FancyCursor_GetActive", new Func<object, bool>(FancyCursor_GetActive));
      dict.Add("FancyCursor_SetActive", new Action<object, bool>(FancyCursor_SetActive));
      dict.Add("FancyCursor_GetPosition", new Func<object, Vector2>(FancyCursor_GetPosition));
      dict.Add("FancyCursor_IsInsideArea", new Func<object, float, float, float, float, bool>(FancyCursor_IsInsideArea));

      dict.Add("FancyTheme_GetColorBg", new Func<object, Color>(FancyTheme_GetColorBg));
      dict.Add("FancyTheme_GetColorWhite", new Func<object, Color>(FancyTheme_GetColorWhite));
      dict.Add("FancyTheme_GetColorMain", new Func<object, Color>(FancyTheme_GetColorMain));
      dict.Add("FancyTheme_GetColorMainDarker", new Func<object, int, Color>(FancyTheme_GetColorMainDarker));
      dict.Add("FancyTheme_MeasureStringInPixels", new Func<object, String, string, float, Vector2>(FancyTheme_MeasureStringInPixels));
      dict.Add("FancyTheme_GetScale", new Func<object, float>(FancyTheme_GetScale));
      dict.Add("FancyTheme_SetScale", new Action<object, float>(FancyTheme_SetScale));

      dict.Add("FancyButtonBase_GetHitArea", new Func<object, Vector4>(FancyButtonBase_GetHitArea));
      dict.Add("FancyButtonBase_SetHitArea", new Action<object, Vector4>(FancyButtonBase_SetHitArea));
      dict.Add("FancyButtonBase_IsMouseReleased", new Func<object, bool>(FancyButtonBase_IsMouseReleased));
      dict.Add("FancyButtonBase_IsMouseOver", new Func<object, bool>(FancyButtonBase_IsMouseOver));
      dict.Add("FancyButtonBase_IsMousePressed", new Func<object, bool>(FancyButtonBase_IsMousePressed));
      dict.Add("FancyButtonBase_JustReleased", new Func<object, bool>(FancyButtonBase_JustReleased));
      dict.Add("FancyButtonBase_JustPressed", new Func<object, bool>(FancyButtonBase_JustPressed));

      dict.Add("FancyButton_New", new Func<string, Action, object>(FancyButton_New));
      dict.Add("FancyButton_GetText", new Func<object, string>(FancyButton_GetText));
      dict.Add("FancyButton_SetText", new Action<object, string>(FancyButton_SetText));
      dict.Add("FancyButton_SetAction", new Action<object, Action>(FancyButton_SetAction));

      dict.Add("FancyLabel_New", new Func<string, float, object>(FancyLabel_New));
      dict.Add("FancyLabel_GetText", new Func<object, string>(FancyLabel_GetText));
      dict.Add("FancyLabel_SetText", new Action<object, string>(FancyLabel_SetText));
      dict.Add("FancyLabel_SetFontSize", new Action<object, float>(FancyLabel_SetFontSize));

      dict.Add("FancyPanel_New", new Func<object>(FancyPanel_New));

      dict.Add("FancyProgressBar_New", new Func<float, float, bool, object>(FancyProgressBar_New));
      dict.Add("FancyProgressBar_GetValue", new Func<object, float>(FancyProgressBar_GetValue));
      dict.Add("FancyProgressBar_SetValue", new Action<object, float>(FancyProgressBar_SetValue));

      dict.Add("FancySelector_New", new Func<List<string>, Action<int, string>, bool, object>(FancySelector_New));
      dict.Add("FancySelector_SetAction", new Action<object, Action<int, string>>(FancySelector_SetAction));

      dict.Add("FancySeparator_New", new Func<object>(FancySeparator_New));

      dict.Add("FancySlider_New", new Func<float, float, Action<float>, object>(FancySlider_New));
      dict.Add("FancySlider_GetRange", new Func<object, Vector2>(FancySlider_GetRange));
      dict.Add("FancySlider_SetRange", new Action<object, Vector2>(FancySlider_SetRange));
      dict.Add("FancySlider_GetValue", new Func<object, float>(FancySlider_GetValue));
      dict.Add("FancySlider_SetValue", new Action<object, float>(FancySlider_SetValue));
      dict.Add("FancySlider_SetAction", new Action<object, Action<float>>(FancySlider_SetAction));
      dict.Add("FancySlider_GetIsInteger", new Func<object, bool>(FancySlider_GetIsInteger));
      dict.Add("FancySlider_SetIsInteger", new Action<object, bool>(FancySlider_SetIsInteger));
      dict.Add("FancySlider_GetAllowInput", new Func<object, bool>(FancySlider_GetAllowInput));
      dict.Add("FancySlider_SetAllowInput", new Action<object, bool>(FancySlider_SetAllowInput));

      dict.Add("FancySliderRange_NewR", new Func<float, float, Action<float, float>, object>(FancySliderRange_NewR));
      dict.Add("FancySliderRange_GetValueLower", new Func<object, float>(FancySliderRange_GetValueLower));
      dict.Add("FancySliderRange_SetValueLower", new Action<object, float>(FancySliderRange_SetValueLower));
      dict.Add("FancySliderRange_SetActionR", new Action<object, Action<float, float>>(FancySliderRange_SetActionR));

      dict.Add("FancySwitch_New", new Func<Action<bool>, string, string, object>(FancySwitch_New));
      dict.Add("FancySwitch_GetTextOn", new Func<object, string>(FancySwitch_GetTextOn));
      dict.Add("FancySwitch_SetTextOn", new Action<object, string>(FancySwitch_SetTextOn));
      dict.Add("FancySwitch_GetTextOff", new Func<object, string>(FancySwitch_GetTextOff));
      dict.Add("FancySwitch_SetTextOff", new Action<object, string>(FancySwitch_SetTextOff));
      dict.Add("FancySwitch_GetValue", new Func<object, bool>(FancySwitch_GetValue));
      dict.Add("FancySwitch_SetValue", new Action<object, bool>(FancySwitch_SetValue));
      dict.Add("FancySwitch_SetAction", new Action<object, Action<bool>>(FancySwitch_SetAction));

      dict.Add("FancyTextField_New", new Func<string, Action<string>, object>(FancyTextField_New));
      dict.Add("FancyTextField_GetText", new Func<object, string>(FancyTextField_GetText));
      dict.Add("FancyTextField_SetText", new Action<object, string>(FancyTextField_SetText));
      dict.Add("FancyTextField_SetAction", new Action<object, Action<string>>(FancyTextField_SetAction));
      dict.Add("FancyTextField_GetIsNumeric", new Func<object, bool>(FancyTextField_GetIsNumeric));
      dict.Add("FancyTextField_SetIsNumeric", new Action<object, bool>(FancyTextField_SetIsNumeric));
      dict.Add("FancyTextField_GetIsInteger", new Func<object, bool>(FancyTextField_GetIsInteger));
      dict.Add("FancyTextField_SetIsInteger", new Action<object, bool>(FancyTextField_SetIsInteger));
      dict.Add("FancyTextField_GetAllowNegative", new Func<object, bool>(FancyTextField_GetAllowNegative));
      dict.Add("FancyTextField_SetAllowNegative", new Action<object, bool>(FancyTextField_SetAllowNegative));

      dict.Add("FancyWindowBar_New", new Func<string, object>(FancyWindowBar_New));
      dict.Add("FancyWindowBar_GetText", new Func<object, string>(FancyWindowBar_GetText));
      dict.Add("FancyWindowBar_SetText", new Action<object, string>(FancyWindowBar_SetText));

      return dict;
    }

    public static TouchScreen CreateTouchScreen(IMyCubeBlock block, IMyTextSurface surface)
    {
      var screen = new TouchScreen(block, surface);
      TouchManager.Instance.Screens.Add(screen);
      return screen;
    }

    public static void RemoveTouchScreen(IMyCubeBlock block, IMyTextSurface surface) => TouchManager.Instance.RemoveScreen(block, surface);
    public static List<TouchScreen> GetTouchScreensList() => TouchManager.Instance.Screens;
    public static TouchScreen GetTargetTouchScreen() => TouchManager.Instance.CurrentScreen;
    public static float GetMaxInteractiveDistance() => TouchManager.Instance.MaxInteractiveDistance;
    public static void SetMaxInteractiveDistance(float distance) => TouchManager.Instance.MaxInteractiveDistance = distance;
    public static void AddSurfaceCoords(string coords) => SurfaceCoordsManager.Instance.AddSurfaceCoords(coords);
    public static void RemoveSurfaceCoords(string coords)
    {
      var index = SurfaceCoordsManager.Instance.CoordsList.IndexOf(coords);
      if (index >= 0)
        SurfaceCoordsManager.Instance.CoordsList.RemoveAt(index);
    }

    static public IMyCubeBlock TouchScreen_GetBlock(object obj) => (obj as TouchScreen).Block;
    static public IMyTextSurface TouchScreen_GetSurface(object obj) => (obj as TouchScreen).Surface;
    static public int TouchScreen_GetIndex(object obj) => (obj as TouchScreen).Index;
    static public bool TouchScreen_IsOnScreen(object obj) => (obj as TouchScreen).IsOnScreen;
    static public Vector2 TouchScreen_GetCursorPosition(object obj) => (obj as TouchScreen).CursorPos;
    static public float TouchScreen_GetInteractiveDistance(object obj) => (obj as TouchScreen).InteractiveDistance;
    static public void TouchScreen_SetInteractiveDistance(object obj, float distance) => (obj as TouchScreen).InteractiveDistance = distance;
    static public int TouchScreen_GetScreenRotate(object obj) => (obj as TouchScreen).ScreenRotate;
    static public bool TouchScreen_CompareWithBlockAndSurface(object obj, IMyCubeBlock block, IMyTextSurface surface) => (obj as TouchScreen).CompareWithBlockAndSurface(block, surface);
    static public void TouchScreen_Dispose(object obj) => (obj as TouchScreen).Dispose();

    static public Vector2 FancyElementBase_GetPosition(object obj) => (obj as FancyElementBase).Position;
    static public void FancyElementBase_SetPosition(object obj, Vector2 position) => (obj as FancyElementBase).Position = position;
    static public Vector4 FancyElementBase_GetMargin(object obj) => (obj as FancyElementBase).Margin;
    static public void FancyElementBase_SetMargin(object obj, Vector4 margin) => (obj as FancyElementBase).Margin = margin;
    static public Vector2 FancyElementBase_GetScale(object obj) => (obj as FancyElementBase).Scale;
    static public void FancyElementBase_SetScale(object obj, Vector2 scale) => (obj as FancyElementBase).Scale = scale;
    static public Vector2 FancyElementBase_GetPixels(object obj) => (obj as FancyElementBase).Pixels;
    static public void FancyElementBase_SetPixels(object obj, Vector2 pixels) => (obj as FancyElementBase).Pixels = pixels;
    static public Vector2 FancyElementBase_GetSize(object obj) => (obj as FancyElementBase).Size;
    static public RectangleF FancyElementBase_GetViewport(object obj) => (obj as FancyElementBase).Viewport;
    static public FancyApp FancyElementBase_GetApp(object obj) => (obj as FancyElementBase).App;
    static public FancyElementContainerBase FancyElementBase_GetParent(object obj) => (obj as FancyElementBase).Parent;
    static public Vector2 FancyElementBase_GetOffset(object obj) => (obj as FancyElementBase).Offset;
    static public List<MySprite> FancyElementBase_GetSprites(object obj) => (obj as FancyElementBase).GetSprites();
    static public void FancyElementBase_InitElements(object obj) => (obj as FancyElementBase).InitElements();
    static public void FancyElementBase_Update(object obj) => (obj as FancyElementBase).Update();
    static public void FancyElementBase_Dispose(object obj) => (obj as FancyElementBase).Dispose();

    static public List<object> FancyElementContainerBase_GetChildren(object obj) => (obj as FancyElementContainerBase).children.Cast<object>().ToList();
    static public void FancyElementContainerBase_AddChild(object obj, object child) => (obj as FancyElementContainerBase).AddChild((FancyElementBase)child);
    static public void FancyElementContainerBase_RemoveChild(object obj, object child) => (obj as FancyElementContainerBase).RemoveChild((FancyElementBase)child);

    static public FancyView FancyView_NewV(int direction) => new FancyView((FancyView.ViewDirection)direction);
    static public int FancyView_GetDirection(object obj) => (int)(obj as FancyView).Direction;
    static public void FancyView_SetDirection(object obj, int direction) => (obj as FancyView).Direction = (FancyView.ViewDirection)direction;

    static public FancyApp FancyApp_New() => new FancyApp();
    static public TouchScreen FancyApp_GetScreen(object obj) => (obj as FancyApp).Screen;
    static public FancyCursor FancyApp_GetCursor(object obj) => (obj as FancyApp).Cursor;
    static public FancyTheme FancyApp_GetTheme(object obj) => (obj as FancyApp).Theme;
    static public void FancyApp_InitApp(object obj, MyCubeBlock block, Sandbox.ModAPI.Ingame.IMyTextSurface surface) => (obj as FancyApp).InitApp(block, surface);

    static public bool FancyCursor_GetActive(object obj) => (obj as FancyCursor).Active;
    static public void FancyCursor_SetActive(object obj, bool active) => (obj as FancyCursor).Active = active;
    static public Vector2 FancyCursor_GetPosition(object obj) => (obj as FancyCursor).Position;
    static public bool FancyCursor_IsInsideArea(object obj, float x, float y, float z, float w) => (obj as FancyCursor).IsInsideArea(x, y, z, w);

    static public Color FancyTheme_GetColorBg(object obj) => (obj as FancyTheme).Bg;
    static public Color FancyTheme_GetColorWhite(object obj) => (obj as FancyTheme).White;
    static public Color FancyTheme_GetColorMain(object obj) => (obj as FancyTheme).Main;
    static public Color FancyTheme_GetColorMainDarker(object obj, int value)
    {
      var theme = (obj as FancyTheme);
      if (value <= 1) return theme.Main_10;
      else if (value <= 2) return theme.Main_20;
      else if (value <= 3) return theme.Main_30;
      else if (value <= 4) return theme.Main_40;
      else if (value <= 5) return theme.Main_50;
      else if (value <= 6) return theme.Main_60;
      else if (value <= 7) return theme.Main_70;
      else if (value <= 8) return theme.Main_80;
      return theme.Main_90;
    }
    static public Vector2 FancyTheme_MeasureStringInPixels(object obj, String text, string font, float scale) => (obj as FancyTheme).MeasureStringInPixels(text, font, scale);
    static public float FancyTheme_GetScale(object obj) => (obj as FancyTheme).Scale;
    static public void FancyTheme_SetScale(object obj, float scale) => (obj as FancyTheme).Scale = scale;

    static public Vector4 FancyButtonBase_GetHitArea(object obj) => (obj as FancyButtonBase).hitArea;
    static public void FancyButtonBase_SetHitArea(object obj, Vector4 hitArea) => (obj as FancyButtonBase).hitArea = hitArea;
    static public bool FancyButtonBase_IsMouseReleased(object obj) => (obj as FancyButtonBase).IsMouseReleased;
    static public bool FancyButtonBase_IsMouseOver(object obj) => (obj as FancyButtonBase).IsMouseOver;
    static public bool FancyButtonBase_IsMousePressed(object obj) => (obj as FancyButtonBase).IsMousePressed;
    static public bool FancyButtonBase_JustReleased(object obj) => (obj as FancyButtonBase).JustReleased;
    static public bool FancyButtonBase_JustPressed(object obj) => (obj as FancyButtonBase).JustPressed;

    static public FancyButton FancyButton_New(string text, Action action) => new FancyButton(text, action);
    static public string FancyButton_GetText(object obj) => (obj as FancyButton).Text;
    static public void FancyButton_SetText(object obj, string text) => (obj as FancyButton).Text = text;
    static public void FancyButton_SetAction(object obj, Action action) => (obj as FancyButton)._action = action;

    static public FancyLabel FancyLabel_New(string text, float fontSize = 0.5f) => new FancyLabel(text, fontSize);
    static public string FancyLabel_GetText(object obj) => (obj as FancyLabel).Text;
    static public void FancyLabel_SetText(object obj, string text) => (obj as FancyLabel).Text = text;
    static public void FancyLabel_SetFontSize(object obj, float fontSize) => (obj as FancyLabel).FontSize = fontSize;

    static public FancyPanel FancyPanel_New() => new FancyPanel();

    static public FancyProgressBar FancyProgressBar_New(float min, float max, bool bars = true) => new FancyProgressBar(min, max, bars);
    static public float FancyProgressBar_GetValue(object obj) => (obj as FancyProgressBar).Value;
    static public void FancyProgressBar_SetValue(object obj, float value) => (obj as FancyProgressBar).Value = value;

    static public FancySelector FancySelector_New(List<string> labels, Action<int, string> action, bool loop = true) => new FancySelector(labels, action, loop);
    static public void FancySelector_SetAction(object obj, Action<int, string> action) => (obj as FancySelector)._action = action;

    static public FancySeparator FancySeparator_New() => new FancySeparator();

    static public FancySlider FancySlider_New(float min, float max, Action<float> action) => new FancySlider(min, max, action);
    static public Vector2 FancySlider_GetRange(object obj) => (obj as FancySlider).Range;
    static public void FancySlider_SetRange(object obj, Vector2 range) => (obj as FancySlider).Range = range;
    static public float FancySlider_GetValue(object obj) => (obj as FancySlider).Value;
    static public void FancySlider_SetValue(object obj, float value) => (obj as FancySlider).Value = value;
    static public void FancySlider_SetAction(object obj, Action<float> action) => (obj as FancySlider)._action = action;
    static public bool FancySlider_GetIsInteger(object obj) => (obj as FancySlider).IsInteger;
    static public void FancySlider_SetIsInteger(object obj, bool interger) => (obj as FancySlider).IsInteger = interger;
    static public bool FancySlider_GetAllowInput(object obj) => (obj as FancySlider).AllowInput;
    static public void FancySlider_SetAllowInput(object obj, bool allowInput) => (obj as FancySlider).AllowInput = allowInput;

    static public FancySliderRange FancySliderRange_NewR(float min, float max, Action<float, float> action) => new FancySliderRange(min, max, action);
    static public float FancySliderRange_GetValueLower(object obj) => (obj as FancySliderRange).ValueLower;
    static public void FancySliderRange_SetValueLower(object obj, float value) => (obj as FancySliderRange).ValueLower = value;
    static public void FancySliderRange_SetActionR(object obj, Action<float, float> action) => (obj as FancySliderRange)._actionR = action;

    static public FancySwitch FancySwitch_New(Action<bool> action, string textOn = "On", string textOff = "Off") => new FancySwitch(action, textOn, textOff);
    static public string FancySwitch_GetTextOn(object obj) => (obj as FancySwitch).TextOn;
    static public void FancySwitch_SetTextOn(object obj, string text) => (obj as FancySwitch).TextOn = text;
    static public string FancySwitch_GetTextOff(object obj) => (obj as FancySwitch).TextOff;
    static public void FancySwitch_SetTextOff(object obj, string text) => (obj as FancySwitch).TextOff = text;
    static public bool FancySwitch_GetValue(object obj) => (obj as FancySwitch).Value;
    static public void FancySwitch_SetValue(object obj, bool value) => (obj as FancySwitch).Value = value;
    static public void FancySwitch_SetAction(object obj, Action<bool> action) => (obj as FancySwitch)._action = action;

    static public FancyTextField FancyTextField_New(string text, Action<string> action) => new FancyTextField(text, action);
    static public string FancyTextField_GetText(object obj) => (obj as FancyTextField).Text;
    static public void FancyTextField_SetText(object obj, string text) => (obj as FancyTextField).Text = text;
    static public void FancyTextField_SetAction(object obj, Action<string> action) => (obj as FancyTextField)._action = action;
    static public bool FancyTextField_GetIsNumeric(object obj) => (obj as FancyTextField).IsNumeric;
    static public void FancyTextField_SetIsNumeric(object obj, bool isNumeric) => (obj as FancyTextField).IsNumeric = isNumeric;
    static public bool FancyTextField_GetIsInteger(object obj) => (obj as FancyTextField).IsInteger;
    static public void FancyTextField_SetIsInteger(object obj, bool isInterger) => (obj as FancyTextField).IsInteger = isInterger;
    static public bool FancyTextField_GetAllowNegative(object obj) => (obj as FancyTextField).AllowNegative;
    static public void FancyTextField_SetAllowNegative(object obj, bool allowNegative) => (obj as FancyTextField).AllowNegative = allowNegative;

    static public FancyWindowBar FancyWindowBar_New(string text) => new FancyWindowBar(text);
    static public string FancyWindowBar_GetText(object obj) => (obj as FancyWindowBar).Text;
    static public void FancyWindowBar_SetText(object obj, string text) => (obj as FancyWindowBar).Text = text;
  }
}