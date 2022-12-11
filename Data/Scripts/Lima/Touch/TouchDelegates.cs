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
  public class TouchDelegates
  {
    private const long _channel = 2668820525;
    private bool _isRegistered;
    public bool IsReady { get; private set; }

    public void Load()
    {
      if (!_isRegistered)
      {
        _isRegistered = true;
        MyAPIGateway.Utilities.RegisterMessageHandler(_channel, HandleMessage);
      }
      IsReady = true;
      MyAPIGateway.Utilities.SendModMessage(_channel, GetApiDictionary());
    }

    public void Unload()
    {
      if (_isRegistered)
      {
        _isRegistered = false;
        MyAPIGateway.Utilities.UnregisterMessageHandler(_channel, HandleMessage);
      }
      IsReady = false;
      MyAPIGateway.Utilities.SendModMessage(_channel, new Dictionary<string, Delegate>());
    }

    private void HandleMessage(object msg)
    {
      if ((msg as string) == "ApiEndpointRequest")
        MyAPIGateway.Utilities.SendModMessage(_channel, GetApiDictionary());
    }

    private Dictionary<string, Delegate> GetApiDictionary()
    {
      var dict = new Dictionary<string, Delegate>();

      dict.Add("CreateTouchScreen", new Func<IMyCubeBlock, IMyTextSurface, TouchScreen>(CreateTouchScreen));
      dict.Add("RemoveTouchScreen", new Action<IMyCubeBlock, IMyTextSurface>(RemoveTouchScreen));
      dict.Add("AddSurfaceCoords", new Action<string>(AddSurfaceCoords));
      dict.Add("RemoveSurfaceCoords", new Action<string>(RemoveSurfaceCoords));
      dict.Add("GetMaxInteractiveDistance", new Func<float>(GetMaxInteractiveDistance));
      dict.Add("SetMaxInteractiveDistance", new Action<float>(SetMaxInteractiveDistance));

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
      dict.Add("FancyView_GetBgColor", new Func<object, Color>(FancyView_GetBgColor));
      dict.Add("FancyView_SetBgColor", new Action<object, Color>(FancyView_SetBgColor));

      dict.Add("FancyApp_New", new Func<FancyApp>(FancyApp_New));
      dict.Add("FancyApp_GetScreen", new Func<object, TouchScreen>(FancyApp_GetScreen));
      dict.Add("FancyApp_GetCursor", new Func<object, FancyCursor>(FancyApp_GetCursor));
      dict.Add("FancyApp_GetTheme", new Func<object, FancyTheme>(FancyApp_GetTheme));
      dict.Add("FancyApp_InitApp", new Action<object, MyCubeBlock, Sandbox.ModAPI.Ingame.IMyTextSurface>(FancyApp_InitApp));

      dict.Add("FancyCursor_New", new Func<object, FancyCursor>(FancyCursor_New));
      dict.Add("FancyCursor_GetActive", new Func<object, bool>(FancyCursor_GetActive));
      dict.Add("FancyCursor_SetActive", new Action<object, bool>(FancyCursor_SetActive));
      dict.Add("FancyCursor_GetPosition", new Func<object, Vector2>(FancyCursor_GetPosition));
      dict.Add("FancyCursor_IsInsideArea", new Func<object, float, float, float, float, bool>(FancyCursor_IsInsideArea));
      dict.Add("FancyCursor_GetSprites", new Func<object, List<MySprite>>(FancyCursor_GetSprites));
      dict.Add("FancyCursor_Dispose", new Action<object>(FancyCursor_Dispose));

      dict.Add("FancyTheme_GetColorBg", new Func<object, Color>(FancyTheme_GetColorBg));
      dict.Add("FancyTheme_GetColorWhite", new Func<object, Color>(FancyTheme_GetColorWhite));
      dict.Add("FancyTheme_GetColorMain", new Func<object, Color>(FancyTheme_GetColorMain));
      dict.Add("FancyTheme_GetColorMainDarker", new Func<object, int, Color>(FancyTheme_GetColorMainDarker));
      dict.Add("FancyTheme_MeasureStringInPixels", new Func<object, String, string, float, Vector2>(FancyTheme_MeasureStringInPixels));
      dict.Add("FancyTheme_GetScale", new Func<object, float>(FancyTheme_GetScale));
      dict.Add("FancyTheme_SetScale", new Action<object, float>(FancyTheme_SetScale));

      dict.Add("FancyButtonBase_GetHandler", new Func<object, ClickHandler>(FancyButtonBase_GetHandler));

      dict.Add("ClickHandler_New", new Func<object>(ClickHandler_New));
      dict.Add("ClickHandler_GetHitArea", new Func<object, Vector4>(ClickHandler_GetHitArea));
      dict.Add("ClickHandler_SetHitArea", new Action<object, Vector4>(ClickHandler_SetHitArea));
      dict.Add("ClickHandler_IsMouseReleased", new Func<object, bool>(ClickHandler_IsMouseReleased));
      dict.Add("ClickHandler_IsMouseOver", new Func<object, bool>(ClickHandler_IsMouseOver));
      dict.Add("ClickHandler_IsMousePressed", new Func<object, bool>(ClickHandler_IsMousePressed));
      dict.Add("ClickHandler_JustReleased", new Func<object, bool>(ClickHandler_JustReleased));
      dict.Add("ClickHandler_JustPressed", new Func<object, bool>(ClickHandler_JustPressed));
      dict.Add("ClickHandler_UpdateStatus", new Action<object, object>(ClickHandler_UpdateStatus));

      dict.Add("FancyButton_New", new Func<string, Action, object>(FancyButton_New));
      dict.Add("FancyButton_GetText", new Func<object, string>(FancyButton_GetText));
      dict.Add("FancyButton_SetText", new Action<object, string>(FancyButton_SetText));
      dict.Add("FancyButton_SetOnChange", new Action<object, Action>(FancyButton_SetOnChange));
      dict.Add("FancyButton_GetAlignment", new Func<object, TextAlignment>(FancyButton_GetAlignment));
      dict.Add("FancyButton_SetAlignment", new Action<object, TextAlignment>(FancyButton_SetAlignment));

      dict.Add("FancyLabel_New", new Func<string, float, object>(FancyLabel_New));
      dict.Add("FancyLabel_GetText", new Func<object, string>(FancyLabel_GetText));
      dict.Add("FancyLabel_SetText", new Action<object, string>(FancyLabel_SetText));
      dict.Add("FancyLabel_SetFontSize", new Action<object, float>(FancyLabel_SetFontSize));
      dict.Add("FancyLabel_GetAlignment", new Func<object, TextAlignment>(FancyLabel_GetAlignment));
      dict.Add("FancyLabel_SetAlignment", new Action<object, TextAlignment>(FancyLabel_SetAlignment));

      dict.Add("FancyProgressBar_New", new Func<float, float, bool, object>(FancyProgressBar_New));
      dict.Add("FancyProgressBar_GetValue", new Func<object, float>(FancyProgressBar_GetValue));
      dict.Add("FancyProgressBar_SetValue", new Action<object, float>(FancyProgressBar_SetValue));

      dict.Add("FancySelector_New", new Func<List<string>, Action<int, string>, bool, object>(FancySelector_New));
      dict.Add("FancySelector_SetOnChange", new Action<object, Action<int, string>>(FancySelector_SetOnChange));

      dict.Add("FancySeparator_New", new Func<object>(FancySeparator_New));

      dict.Add("FancySlider_New", new Func<float, float, Action<float>, object>(FancySlider_New));
      dict.Add("FancySlider_GetRange", new Func<object, Vector2>(FancySlider_GetRange));
      dict.Add("FancySlider_SetRange", new Action<object, Vector2>(FancySlider_SetRange));
      dict.Add("FancySlider_GetValue", new Func<object, float>(FancySlider_GetValue));
      dict.Add("FancySlider_SetValue", new Action<object, float>(FancySlider_SetValue));
      dict.Add("FancySlider_SetOnChange", new Action<object, Action<float>>(FancySlider_SetOnChange));
      dict.Add("FancySlider_GetIsInteger", new Func<object, bool>(FancySlider_GetIsInteger));
      dict.Add("FancySlider_SetIsInteger", new Action<object, bool>(FancySlider_SetIsInteger));
      dict.Add("FancySlider_GetAllowInput", new Func<object, bool>(FancySlider_GetAllowInput));
      dict.Add("FancySlider_SetAllowInput", new Action<object, bool>(FancySlider_SetAllowInput));

      dict.Add("FancySliderRange_NewR", new Func<float, float, Action<float, float>, object>(FancySliderRange_NewR));
      dict.Add("FancySliderRange_GetValueLower", new Func<object, float>(FancySliderRange_GetValueLower));
      dict.Add("FancySliderRange_SetValueLower", new Action<object, float>(FancySliderRange_SetValueLower));
      dict.Add("FancySliderRange_SetOnChangeR", new Action<object, Action<float, float>>(FancySliderRange_SetOnChangeR));

      dict.Add("FancySwitch_New", new Func<Action<bool>, string, string, object>(FancySwitch_New));
      dict.Add("FancySwitch_GetTextOn", new Func<object, string>(FancySwitch_GetTextOn));
      dict.Add("FancySwitch_SetTextOn", new Action<object, string>(FancySwitch_SetTextOn));
      dict.Add("FancySwitch_GetTextOff", new Func<object, string>(FancySwitch_GetTextOff));
      dict.Add("FancySwitch_SetTextOff", new Action<object, string>(FancySwitch_SetTextOff));
      dict.Add("FancySwitch_GetValue", new Func<object, bool>(FancySwitch_GetValue));
      dict.Add("FancySwitch_SetValue", new Action<object, bool>(FancySwitch_SetValue));
      dict.Add("FancySwitch_SetOnChange", new Action<object, Action<bool>>(FancySwitch_SetOnChange));

      dict.Add("FancyTextField_New", new Func<string, Action<string>, object>(FancyTextField_New));
      dict.Add("FancyTextField_GetText", new Func<object, string>(FancyTextField_GetText));
      dict.Add("FancyTextField_SetText", new Action<object, string>(FancyTextField_SetText));
      dict.Add("FancyTextField_SetOnChange", new Action<object, Action<string>>(FancyTextField_SetOnChange));
      dict.Add("FancyTextField_GetIsNumeric", new Func<object, bool>(FancyTextField_GetIsNumeric));
      dict.Add("FancyTextField_SetIsNumeric", new Action<object, bool>(FancyTextField_SetIsNumeric));
      dict.Add("FancyTextField_GetIsInteger", new Func<object, bool>(FancyTextField_GetIsInteger));
      dict.Add("FancyTextField_SetIsInteger", new Action<object, bool>(FancyTextField_SetIsInteger));
      dict.Add("FancyTextField_GetAllowNegative", new Func<object, bool>(FancyTextField_GetAllowNegative));
      dict.Add("FancyTextField_SetAllowNegative", new Action<object, bool>(FancyTextField_SetAllowNegative));
      dict.Add("FancyTextField_GetAlignment", new Func<object, TextAlignment>(FancyTextField_GetAlignment));
      dict.Add("FancyTextField_SetAlignment", new Action<object, TextAlignment>(FancyTextField_SetAlignment));

      dict.Add("FancyWindowBar_New", new Func<string, object>(FancyWindowBar_New));
      dict.Add("FancyWindowBar_GetText", new Func<object, string>(FancyWindowBar_GetText));
      dict.Add("FancyWindowBar_SetText", new Action<object, string>(FancyWindowBar_SetText));

      return dict;
    }

    private TouchScreen CreateTouchScreen(IMyCubeBlock block, IMyTextSurface surface)
    {
      var screen = new TouchScreen(block, surface);
      TouchSession.Instance.TouchMan.Screens.Add(screen);
      return screen;
    }
    private void RemoveTouchScreen(IMyCubeBlock block, IMyTextSurface surface) => TouchSession.Instance.TouchMan.RemoveScreen(block, surface);
    private List<TouchScreen> GetTouchScreensList() => TouchSession.Instance.TouchMan.Screens;
    private TouchScreen GetTargetTouchScreen() => TouchSession.Instance.TouchMan.CurrentScreen;
    private float GetMaxInteractiveDistance() => TouchSession.Instance.TouchMan.MaxInteractiveDistance;
    private void SetMaxInteractiveDistance(float distance) => TouchSession.Instance.TouchMan.MaxInteractiveDistance = distance;
    private void AddSurfaceCoords(string coords) => TouchSession.Instance.SurfaceCoordsMan.AddSurfaceCoords(coords);
    private void RemoveSurfaceCoords(string coords)
    {
      var index = TouchSession.Instance.SurfaceCoordsMan.CoordsList.IndexOf(coords);
      if (index >= 0)
        TouchSession.Instance.SurfaceCoordsMan.CoordsList.RemoveAt(index);
    }

    private IMyCubeBlock TouchScreen_GetBlock(object obj) => (obj as TouchScreen).Block;
    private IMyTextSurface TouchScreen_GetSurface(object obj) => (obj as TouchScreen).Surface;
    private int TouchScreen_GetIndex(object obj) => (obj as TouchScreen).Index;
    private bool TouchScreen_IsOnScreen(object obj) => (obj as TouchScreen).IsOnScreen;
    private Vector2 TouchScreen_GetCursorPosition(object obj) => (obj as TouchScreen).CursorPos;
    private float TouchScreen_GetInteractiveDistance(object obj) => (obj as TouchScreen).InteractiveDistance;
    private void TouchScreen_SetInteractiveDistance(object obj, float distance) => (obj as TouchScreen).InteractiveDistance = distance;
    private int TouchScreen_GetScreenRotate(object obj) => (obj as TouchScreen).ScreenRotate;
    private bool TouchScreen_CompareWithBlockAndSurface(object obj, IMyCubeBlock block, IMyTextSurface surface) => (obj as TouchScreen).CompareWithBlockAndSurface(block, surface);
    private void TouchScreen_Dispose(object obj) => (obj as TouchScreen).Dispose();

    private Vector2 FancyElementBase_GetPosition(object obj) => (obj as FancyElementBase).Position;
    private void FancyElementBase_SetPosition(object obj, Vector2 position) => (obj as FancyElementBase).Position = position;
    private Vector4 FancyElementBase_GetMargin(object obj) => (obj as FancyElementBase).Margin;
    private void FancyElementBase_SetMargin(object obj, Vector4 margin) => (obj as FancyElementBase).Margin = margin;
    private Vector2 FancyElementBase_GetScale(object obj) => (obj as FancyElementBase).Scale;
    private void FancyElementBase_SetScale(object obj, Vector2 scale) => (obj as FancyElementBase).Scale = scale;
    private Vector2 FancyElementBase_GetPixels(object obj) => (obj as FancyElementBase).Pixels;
    private void FancyElementBase_SetPixels(object obj, Vector2 pixels) => (obj as FancyElementBase).Pixels = pixels;
    private Vector2 FancyElementBase_GetSize(object obj) => (obj as FancyElementBase).Size;
    private RectangleF FancyElementBase_GetViewport(object obj) => (obj as FancyElementBase).Viewport;
    private FancyApp FancyElementBase_GetApp(object obj) => (obj as FancyElementBase).App;
    private FancyElementContainerBase FancyElementBase_GetParent(object obj) => (obj as FancyElementBase).Parent;
    private Vector2 FancyElementBase_GetOffset(object obj) => (obj as FancyElementBase).Offset;
    private List<MySprite> FancyElementBase_GetSprites(object obj) => (obj as FancyElementBase).GetSprites();
    private void FancyElementBase_InitElements(object obj) => (obj as FancyElementBase).InitElements();
    private void FancyElementBase_Update(object obj) => (obj as FancyElementBase).Update();
    private void FancyElementBase_Dispose(object obj) => (obj as FancyElementBase).Dispose();

    private List<object> FancyElementContainerBase_GetChildren(object obj) => (obj as FancyElementContainerBase).children.Cast<object>().ToList();
    private void FancyElementContainerBase_AddChild(object obj, object child) => (obj as FancyElementContainerBase).AddChild((FancyElementBase)child);
    private void FancyElementContainerBase_RemoveChild(object obj, object child) => (obj as FancyElementContainerBase).RemoveChild((FancyElementBase)child);

    private FancyView FancyView_NewV(int direction) => new FancyView((FancyView.ViewDirection)direction);
    private int FancyView_GetDirection(object obj) => (int)(obj as FancyView).Direction;
    private void FancyView_SetDirection(object obj, int direction) => (obj as FancyView).Direction = (FancyView.ViewDirection)direction;
    private Color FancyView_GetBgColor(object obj) => (Color)(obj as FancyView).BgColor;
    private void FancyView_SetBgColor(object obj, Color bgColor) => (obj as FancyView).BgColor = bgColor;

    private FancyApp FancyApp_New() => new FancyApp();
    private TouchScreen FancyApp_GetScreen(object obj) => (obj as FancyApp).Screen;
    private FancyCursor FancyApp_GetCursor(object obj) => (obj as FancyApp).Cursor;
    private FancyTheme FancyApp_GetTheme(object obj) => (obj as FancyApp).Theme;
    private void FancyApp_InitApp(object obj, MyCubeBlock block, Sandbox.ModAPI.Ingame.IMyTextSurface surface) => (obj as FancyApp).InitApp(block, surface);

    private FancyCursor FancyCursor_New(object screen) => new FancyCursor(screen as TouchScreen);
    private bool FancyCursor_GetActive(object obj) => (obj as FancyCursor).Active;
    private void FancyCursor_SetActive(object obj, bool active) => (obj as FancyCursor).Active = active;
    private Vector2 FancyCursor_GetPosition(object obj) => (obj as FancyCursor).Position;
    private bool FancyCursor_IsInsideArea(object obj, float x, float y, float z, float w) => (obj as FancyCursor).IsInsideArea(x, y, z, w);
    private List<MySprite> FancyCursor_GetSprites(object obj) => (obj as FancyCursor).GetSprites();
    private void FancyCursor_Dispose(object obj) => (obj as FancyCursor).Dispose();

    private Color FancyTheme_GetColorBg(object obj) => (obj as FancyTheme).Bg;
    private Color FancyTheme_GetColorWhite(object obj) => (obj as FancyTheme).White;
    private Color FancyTheme_GetColorMain(object obj) => (obj as FancyTheme).Main;
    private Color FancyTheme_GetColorMainDarker(object obj, int value)
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
    private Vector2 FancyTheme_MeasureStringInPixels(object obj, String text, string font, float scale) => (obj as FancyTheme).MeasureStringInPixels(text, font, scale);
    private float FancyTheme_GetScale(object obj) => (obj as FancyTheme).Scale;
    private void FancyTheme_SetScale(object obj, float scale) => (obj as FancyTheme).Scale = scale;

    private ClickHandler FancyButtonBase_GetHandler(object obj) => (obj as FancyButtonBase).handler;

    private ClickHandler ClickHandler_New() => new ClickHandler();
    private Vector4 ClickHandler_GetHitArea(object obj) => (obj as ClickHandler).HitArea;
    private void ClickHandler_SetHitArea(object obj, Vector4 hitArea) => (obj as ClickHandler).HitArea = hitArea;
    private bool ClickHandler_IsMouseReleased(object obj) => (obj as ClickHandler).IsMouseReleased;
    private bool ClickHandler_IsMouseOver(object obj) => (obj as ClickHandler).IsMouseOver;
    private bool ClickHandler_IsMousePressed(object obj) => (obj as ClickHandler).IsMousePressed;
    private bool ClickHandler_JustReleased(object obj) => (obj as ClickHandler).JustReleased;
    private bool ClickHandler_JustPressed(object obj) => (obj as ClickHandler).JustPressed;
    private void ClickHandler_UpdateStatus(object obj, object screen) => (obj as ClickHandler).UpdateStatus(screen as TouchScreen);

    private FancyButton FancyButton_New(string text, Action onChange) => new FancyButton(text, onChange);
    private string FancyButton_GetText(object obj) => (obj as FancyButton).Text;
    private void FancyButton_SetText(object obj, string text) => (obj as FancyButton).Text = text;
    private void FancyButton_SetOnChange(object obj, Action onChange) => (obj as FancyButton).OnChange = onChange;
    private TextAlignment FancyButton_GetAlignment(object obj) => (obj as FancyButton).Alignment;
    private void FancyButton_SetAlignment(object obj, TextAlignment alignment) => (obj as FancyButton).Alignment = alignment;

    private FancyLabel FancyLabel_New(string text, float fontSize = 0.5f) => new FancyLabel(text, fontSize);
    private string FancyLabel_GetText(object obj) => (obj as FancyLabel).Text;
    private void FancyLabel_SetText(object obj, string text) => (obj as FancyLabel).Text = text;
    private void FancyLabel_SetFontSize(object obj, float fontSize) => (obj as FancyLabel).FontSize = fontSize;
    private TextAlignment FancyLabel_GetAlignment(object obj) => (obj as FancyLabel).Alignment;
    private void FancyLabel_SetAlignment(object obj, TextAlignment alignment) => (obj as FancyLabel).Alignment = alignment;

    private FancyProgressBar FancyProgressBar_New(float min, float max, bool bars = true) => new FancyProgressBar(min, max, bars);
    private float FancyProgressBar_GetValue(object obj) => (obj as FancyProgressBar).Value;
    private void FancyProgressBar_SetValue(object obj, float value) => (obj as FancyProgressBar).Value = value;

    private FancySelector FancySelector_New(List<string> labels, Action<int, string> onChange, bool loop = true) => new FancySelector(labels, onChange, loop);
    private void FancySelector_SetOnChange(object obj, Action<int, string> onChange) => (obj as FancySelector).OnChange = onChange;

    private FancySeparator FancySeparator_New() => new FancySeparator();

    private FancySlider FancySlider_New(float min, float max, Action<float> onChange) => new FancySlider(min, max, onChange);
    private Vector2 FancySlider_GetRange(object obj) => (obj as FancySlider).Range;
    private void FancySlider_SetRange(object obj, Vector2 range) => (obj as FancySlider).Range = range;
    private float FancySlider_GetValue(object obj) => (obj as FancySlider).Value;
    private void FancySlider_SetValue(object obj, float value) => (obj as FancySlider).Value = value;
    private void FancySlider_SetOnChange(object obj, Action<float> onChange) => (obj as FancySlider).OnChange = onChange;
    private bool FancySlider_GetIsInteger(object obj) => (obj as FancySlider).IsInteger;
    private void FancySlider_SetIsInteger(object obj, bool interger) => (obj as FancySlider).IsInteger = interger;
    private bool FancySlider_GetAllowInput(object obj) => (obj as FancySlider).AllowInput;
    private void FancySlider_SetAllowInput(object obj, bool allowInput) => (obj as FancySlider).AllowInput = allowInput;

    private FancySliderRange FancySliderRange_NewR(float min, float max, Action<float, float> onChange) => new FancySliderRange(min, max, onChange);
    private float FancySliderRange_GetValueLower(object obj) => (obj as FancySliderRange).ValueLower;
    private void FancySliderRange_SetValueLower(object obj, float value) => (obj as FancySliderRange).ValueLower = value;
    private void FancySliderRange_SetOnChangeR(object obj, Action<float, float> onChange) => (obj as FancySliderRange).OnChangeR = onChange;

    private FancySwitch FancySwitch_New(Action<bool> onChange, string textOn = "On", string textOff = "Off") => new FancySwitch(onChange, textOn, textOff);
    private string FancySwitch_GetTextOn(object obj) => (obj as FancySwitch).TextOn;
    private void FancySwitch_SetTextOn(object obj, string text) => (obj as FancySwitch).TextOn = text;
    private string FancySwitch_GetTextOff(object obj) => (obj as FancySwitch).TextOff;
    private void FancySwitch_SetTextOff(object obj, string text) => (obj as FancySwitch).TextOff = text;
    private bool FancySwitch_GetValue(object obj) => (obj as FancySwitch).Value;
    private void FancySwitch_SetValue(object obj, bool value) => (obj as FancySwitch).Value = value;
    private void FancySwitch_SetOnChange(object obj, Action<bool> onChange) => (obj as FancySwitch).OnChange = onChange;

    private FancyTextField FancyTextField_New(string text, Action<string> onChange) => new FancyTextField(text, onChange);
    private string FancyTextField_GetText(object obj) => (obj as FancyTextField).Text;
    private void FancyTextField_SetText(object obj, string text) => (obj as FancyTextField).Text = text;
    private void FancyTextField_SetOnChange(object obj, Action<string> onChange) => (obj as FancyTextField).OnChange = onChange;
    private bool FancyTextField_GetIsNumeric(object obj) => (obj as FancyTextField).IsNumeric;
    private void FancyTextField_SetIsNumeric(object obj, bool isNumeric) => (obj as FancyTextField).IsNumeric = isNumeric;
    private bool FancyTextField_GetIsInteger(object obj) => (obj as FancyTextField).IsInteger;
    private void FancyTextField_SetIsInteger(object obj, bool isInterger) => (obj as FancyTextField).IsInteger = isInterger;
    private bool FancyTextField_GetAllowNegative(object obj) => (obj as FancyTextField).AllowNegative;
    private void FancyTextField_SetAllowNegative(object obj, bool allowNegative) => (obj as FancyTextField).AllowNegative = allowNegative;
    private TextAlignment FancyTextField_GetAlignment(object obj) => (obj as FancyTextField).Alignment;
    private void FancyTextField_SetAlignment(object obj, TextAlignment alignment) => (obj as FancyTextField).Alignment = alignment;

    private FancyWindowBar FancyWindowBar_New(string text) => new FancyWindowBar(text);
    private string FancyWindowBar_GetText(object obj) => (obj as FancyWindowBar).Text;
    private void FancyWindowBar_SetText(object obj, string text) => (obj as FancyWindowBar).Text = text;
  }
}