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
      var dict = new Dictionary<string, Delegate>
      {
        { "CreateTouchScreen", new Func<IMyCubeBlock, IMyTextSurface, TouchScreen>(CreateTouchScreen) },
        { "RemoveTouchScreen", new Action<IMyCubeBlock, IMyTextSurface>(RemoveTouchScreen) },
        { "AddSurfaceCoords", new Action<string>(AddSurfaceCoords) },
        { "RemoveSurfaceCoords", new Action<string>(RemoveSurfaceCoords) },
        { "GetMaxInteractiveDistance", new Func<float>(GetMaxInteractiveDistance) },
        { "SetMaxInteractiveDistance", new Action<float>(SetMaxInteractiveDistance) },

        { "TouchScreen_GetBlock", new Func<object, IMyCubeBlock>(TouchScreen_GetBlock) },
        { "TouchScreen_GetSurface", new Func<object, IMyTextSurface>(TouchScreen_GetSurface) },
        { "TouchScreen_GetIndex", new Func<object, int>(TouchScreen_GetIndex) },
        { "TouchScreen_IsOnScreen", new Func<object, bool>(TouchScreen_IsOnScreen) },
        { "TouchScreen_GetCursorPosition", new Func<object, Vector2>(TouchScreen_GetCursorPosition) },
        { "TouchScreen_GetInteractiveDistance", new Func<object, float>(TouchScreen_GetInteractiveDistance) },
        { "TouchScreen_SetInteractiveDistance", new Action<object, float>(TouchScreen_SetInteractiveDistance) },
        { "TouchScreen_GetScreenRotate", new Func<object, int>(TouchScreen_GetScreenRotate) },
        { "TouchScreen_CompareWithBlockAndSurface", new Func<object, IMyCubeBlock, IMyTextSurface, bool>(TouchScreen_CompareWithBlockAndSurface) },
        { "TouchScreen_Dispose", new Action<object>(TouchScreen_Dispose) },

        { "FancyElementBase_GetEnabled", new Func<object, bool>(FancyElementBase_GetEnabled) },
        { "FancyElementBase_SetEnabled", new Action<object, bool>(FancyElementBase_SetEnabled) },
        { "FancyElementBase_GetPosition", new Func<object, Vector2>(FancyElementBase_GetPosition) },
        { "FancyElementBase_SetPosition", new Action<object, Vector2>(FancyElementBase_SetPosition) },
        { "FancyElementBase_GetMargin", new Func<object, Vector4>(FancyElementBase_GetMargin) },
        { "FancyElementBase_SetMargin", new Action<object, Vector4>(FancyElementBase_SetMargin) },
        { "FancyElementBase_GetScale", new Func<object, Vector2>(FancyElementBase_GetScale) },
        { "FancyElementBase_SetScale", new Action<object, Vector2>(FancyElementBase_SetScale) },
        { "FancyElementBase_GetPixels", new Func<object, Vector2>(FancyElementBase_GetPixels) },
        { "FancyElementBase_SetPixels", new Action<object, Vector2>(FancyElementBase_SetPixels) },
        { "FancyElementBase_GetSize", new Func<object, Vector2>(FancyElementBase_GetSize) },
        { "FancyElementBase_GetBoundaries", new Func<object, Vector2>(FancyElementBase_GetBoundaries) },
        { "FancyElementBase_GetApp", new Func<object, FancyApp>(FancyElementBase_GetApp) },
        { "FancyElementBase_GetParent", new Func<object, FancyContainerBase>(FancyElementBase_GetParent) },
        { "FancyElementBase_GetSprites", new Func<object, List<MySprite>>(FancyElementBase_GetSprites) },
        { "FancyElementBase_InitElements", new Action<object>(FancyElementBase_InitElements) },
        { "FancyElementBase_Update", new Action<object>(FancyElementBase_Update) },
        { "FancyElementBase_Dispose", new Action<object>(FancyElementBase_Dispose) },

        { "FancyContainerBase_GetChildren", new Func<object, List<object>>(FancyContainerBase_GetChildren) },
        { "FancyContainerBase_GetFlexSize", new Func<object, Vector2>(FancyContainerBase_GetFlexSize) },
        { "FancyContainerBase_AddChild", new Action<object, object>(FancyContainerBase_AddChild) },
        { "FancyContainerBase_RemoveChild", new Action<object, object>(FancyContainerBase_RemoveChild) },

        { "FancyView_New", new Func<int, Color?, FancyView>(FancyView_New) },
        { "FancyView_GetDirection", new Func<object, int>(FancyView_GetDirection) },
        { "FancyView_SetDirection", new Action<object, int>(FancyView_SetDirection) },
        { "FancyView_GetBgColor", new Func<object, Color>(FancyView_GetBgColor) },
        { "FancyView_SetBgColor", new Action<object, Color>(FancyView_SetBgColor) },
        { "FancyView_GetBorderColor", new Func<object, Color>(FancyView_GetBorderColor) },
        { "FancyView_SetBorderColor", new Action<object, Color>(FancyView_SetBorderColor) },
        { "FancyView_GetBorder", new Func<object, Vector4>(FancyView_GetBorder) },
        { "FancyView_SetBorder", new Action<object, Vector4>(FancyView_SetBorder) },
        { "FancyView_GetPadding", new Func<object, Vector4>(FancyView_GetPadding) },
        { "FancyView_SetPadding", new Action<object, Vector4>(FancyView_SetPadding) },
        { "FancyView_GetGap", new Func<object, int>(FancyView_GetGap) },
        { "FancyView_SetGap", new Action<object, int>(FancyView_SetGap) },

        { "FancyScrollView_New", new Func<int, Color?, FancyScrollView>(FancyScrollView_New) },
        { "FancyScrollView_GetBarWidth", new Func<object, int>(FancyScrollView_GetBarWidth) },
        { "FancyScrollView_SetBarWidth", new Action<object, int>(FancyScrollView_SetBarWidth) },
        { "FancyScrollView_GetScroll", new Func<object, float>(FancyScrollView_GetScroll) },
        { "FancyScrollView_SetScroll", new Action<object, float>(FancyScrollView_SetScroll) },
        { "FancyScrollView_GetScrollAlwaysVisible", new Func<object, bool>(FancyScrollView_GetScrollAlwaysVisible) },
        { "FancyScrollView_SetScrollAlwaysVisible", new Action<object, bool>(FancyScrollView_SetScrollAlwaysVisible) },

        { "FancyApp_New", new Func<FancyApp>(FancyApp_New) },
        { "FancyApp_GetScreen", new Func<object, TouchScreen>(FancyApp_GetScreen) },
        { "FancyApp_GetViewport", new Func<object, RectangleF>(FancyApp_GetViewport) },
        { "FancyApp_GetCursor", new Func<object, FancyCursor>(FancyApp_GetCursor) },
        { "FancyApp_GetTheme", new Func<object, FancyTheme>(FancyApp_GetTheme) },
        { "FancyApp_InitApp", new Action<object, MyCubeBlock, Sandbox.ModAPI.Ingame.IMyTextSurface>(FancyApp_InitApp) },

        { "FancyCursor_New", new Func<object, FancyCursor>(FancyCursor_New) },
        { "FancyCursor_GetActive", new Func<object, bool>(FancyCursor_GetActive) },
        { "FancyCursor_SetActive", new Action<object, bool>(FancyCursor_SetActive) },
        { "FancyCursor_GetPosition", new Func<object, Vector2>(FancyCursor_GetPosition) },
        { "FancyCursor_IsInsideArea", new Func<object, float, float, float, float, bool>(FancyCursor_IsInsideArea) },
        { "FancyCursor_GetSprites", new Func<object, List<MySprite>>(FancyCursor_GetSprites) },
        { "FancyCursor_Dispose", new Action<object>(FancyCursor_Dispose) },

        { "FancyTheme_GetColorBg", new Func<object, Color>(FancyTheme_GetColorBg) },
        { "FancyTheme_GetColorWhite", new Func<object, Color>(FancyTheme_GetColorWhite) },
        { "FancyTheme_GetColorMain", new Func<object, Color>(FancyTheme_GetColorMain) },
        { "FancyTheme_GetColorMainDarker", new Func<object, int, Color>(FancyTheme_GetColorMainDarker) },
        { "FancyTheme_MeasureStringInPixels", new Func<object, String, string, float, Vector2>(FancyTheme_MeasureStringInPixels) },
        { "FancyTheme_GetScale", new Func<object, float>(FancyTheme_GetScale) },
        { "FancyTheme_SetScale", new Action<object, float>(FancyTheme_SetScale) },

        { "FancyButtonBase_GetHandler", new Func<object, ClickHandler>(FancyButtonBase_GetHandler) },

        { "ClickHandler_New", new Func<object>(ClickHandler_New) },
        { "ClickHandler_GetHitArea", new Func<object, Vector4>(ClickHandler_GetHitArea) },
        { "ClickHandler_SetHitArea", new Action<object, Vector4>(ClickHandler_SetHitArea) },
        { "ClickHandler_IsMouseReleased", new Func<object, bool>(ClickHandler_IsMouseReleased) },
        { "ClickHandler_IsMouseOver", new Func<object, bool>(ClickHandler_IsMouseOver) },
        { "ClickHandler_IsMousePressed", new Func<object, bool>(ClickHandler_IsMousePressed) },
        { "ClickHandler_JustReleased", new Func<object, bool>(ClickHandler_JustReleased) },
        { "ClickHandler_JustPressed", new Func<object, bool>(ClickHandler_JustPressed) },
        { "ClickHandler_UpdateStatus", new Action<object, object>(ClickHandler_UpdateStatus) },

        { "FancyButton_New", new Func<string, Action, object>(FancyButton_New) },
        { "FancyButton_GetText", new Func<object, string>(FancyButton_GetText) },
        { "FancyButton_SetText", new Action<object, string>(FancyButton_SetText) },
        { "FancyButton_SetOnChange", new Action<object, Action>(FancyButton_SetOnChange) },
        { "FancyButton_GetAlignment", new Func<object, TextAlignment>(FancyButton_GetAlignment) },
        { "FancyButton_SetAlignment", new Action<object, TextAlignment>(FancyButton_SetAlignment) },

        { "FancyLabel_New", new Func<string, float, TextAlignment, object>(FancyLabel_New) },
        { "FancyLabel_GetText", new Func<object, string>(FancyLabel_GetText) },
        { "FancyLabel_SetText", new Action<object, string>(FancyLabel_SetText) },
        { "FancyLabel_SetFontSize", new Action<object, float>(FancyLabel_SetFontSize) },
        { "FancyLabel_GetAlignment", new Func<object, TextAlignment>(FancyLabel_GetAlignment) },
        { "FancyLabel_SetAlignment", new Action<object, TextAlignment>(FancyLabel_SetAlignment) },

        { "FancyProgressBar_New", new Func<float, float, bool, object>(FancyProgressBar_New) },
        { "FancyProgressBar_GetValue", new Func<object, float>(FancyProgressBar_GetValue) },
        { "FancyProgressBar_SetValue", new Action<object, float>(FancyProgressBar_SetValue) },

        { "FancySelector_New", new Func<List<string>, Action<int, string>, bool, object>(FancySelector_New) },
        { "FancySelector_SetOnChange", new Action<object, Action<int, string>>(FancySelector_SetOnChange) },

        { "FancySeparator_New", new Func<object>(FancySeparator_New) },

        { "FancySlider_New", new Func<float, float, Action<float>, object>(FancySlider_New) },
        { "FancySlider_GetRange", new Func<object, Vector2>(FancySlider_GetRange) },
        { "FancySlider_SetRange", new Action<object, Vector2>(FancySlider_SetRange) },
        { "FancySlider_GetValue", new Func<object, float>(FancySlider_GetValue) },
        { "FancySlider_SetValue", new Action<object, float>(FancySlider_SetValue) },
        { "FancySlider_SetOnChange", new Action<object, Action<float>>(FancySlider_SetOnChange) },
        { "FancySlider_GetIsInteger", new Func<object, bool>(FancySlider_GetIsInteger) },
        { "FancySlider_SetIsInteger", new Action<object, bool>(FancySlider_SetIsInteger) },
        { "FancySlider_GetAllowInput", new Func<object, bool>(FancySlider_GetAllowInput) },
        { "FancySlider_SetAllowInput", new Action<object, bool>(FancySlider_SetAllowInput) },

        { "FancySliderRange_NewR", new Func<float, float, Action<float, float>, object>(FancySliderRange_NewR) },
        { "FancySliderRange_GetValueLower", new Func<object, float>(FancySliderRange_GetValueLower) },
        { "FancySliderRange_SetValueLower", new Action<object, float>(FancySliderRange_SetValueLower) },
        { "FancySliderRange_SetOnChangeR", new Action<object, Action<float, float>>(FancySliderRange_SetOnChangeR) },

        { "FancySwitch_New", new Func<string[], int, Action<int>, object>(FancySwitch_New) },
        { "FancySwitch_GetIndex", new Func<object, int>(FancySwitch_GetIndex) },
        { "FancySwitch_SetIndex", new Action<object, int>(FancySwitch_SetIndex) },
        { "FancySwitch_GetTabNames", new Func<object, string[]>(FancySwitch_GetTabNames) },
        { "FancySwitch_GetTabName", new Func<object, int, string>(FancySwitch_GetTabName) },
        { "FancySwitch_SetTabName", new Action<object, int, string>(FancySwitch_SetTabName) },
        { "FancySwitch_SetOnChange", new Action<object, Action<int>>(FancySwitch_SetOnChange) },

        { "FancyTextField_New", new Func<string, Action<string>, object>(FancyTextField_New) },
        { "FancyTextField_GetText", new Func<object, string>(FancyTextField_GetText) },
        { "FancyTextField_SetText", new Action<object, string>(FancyTextField_SetText) },
        { "FancyTextField_SetOnChange", new Action<object, Action<string>>(FancyTextField_SetOnChange) },
        { "FancyTextField_GetIsNumeric", new Func<object, bool>(FancyTextField_GetIsNumeric) },
        { "FancyTextField_SetIsNumeric", new Action<object, bool>(FancyTextField_SetIsNumeric) },
        { "FancyTextField_GetIsInteger", new Func<object, bool>(FancyTextField_GetIsInteger) },
        { "FancyTextField_SetIsInteger", new Action<object, bool>(FancyTextField_SetIsInteger) },
        { "FancyTextField_GetAllowNegative", new Func<object, bool>(FancyTextField_GetAllowNegative) },
        { "FancyTextField_SetAllowNegative", new Action<object, bool>(FancyTextField_SetAllowNegative) },
        { "FancyTextField_GetAlignment", new Func<object, TextAlignment>(FancyTextField_GetAlignment) },
        { "FancyTextField_SetAlignment", new Action<object, TextAlignment>(FancyTextField_SetAlignment) },

        { "FancyWindowBar_New", new Func<string, object>(FancyWindowBar_New) },
        { "FancyWindowBar_GetText", new Func<object, string>(FancyWindowBar_GetText) },
        { "FancyWindowBar_SetText", new Action<object, string>(FancyWindowBar_SetText) }
      };

      return dict;
    }

    private TouchScreen CreateTouchScreen(IMyCubeBlock block, IMyTextSurface surface)
    {
      var screen = new TouchScreen(block, surface);
      TouchSession.Instance.TouchMan.Screens.Add(screen);
      return screen;
    }
    private void RemoveTouchScreen(IMyCubeBlock block, IMyTextSurface surface) => TouchSession.Instance.TouchMan.RemoveScreen(block, surface);
    // private List<TouchScreen> GetTouchScreensList() => TouchSession.Instance.TouchMan.Screens;
    // private TouchScreen GetTargetTouchScreen() => TouchSession.Instance.TouchMan.CurrentScreen;
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

    private bool FancyElementBase_GetEnabled(object obj) => (obj as FancyElementBase).Enabled;
    private void FancyElementBase_SetEnabled(object obj, bool enabled) => (obj as FancyElementBase).Enabled = enabled;
    private Vector2 FancyElementBase_GetPosition(object obj) => (obj as FancyElementBase).Position;
    private void FancyElementBase_SetPosition(object obj, Vector2 position) => (obj as FancyElementBase).Position = position;
    private Vector4 FancyElementBase_GetMargin(object obj) => (obj as FancyElementBase).Margin;
    private void FancyElementBase_SetMargin(object obj, Vector4 margin) => (obj as FancyElementBase).Margin = margin;
    private Vector2 FancyElementBase_GetScale(object obj) => (obj as FancyElementBase).Scale;
    private void FancyElementBase_SetScale(object obj, Vector2 scale) => (obj as FancyElementBase).Scale = scale;
    private Vector2 FancyElementBase_GetPixels(object obj) => (obj as FancyElementBase).Pixels;
    private void FancyElementBase_SetPixels(object obj, Vector2 pixels) => (obj as FancyElementBase).Pixels = pixels;
    private Vector2 FancyElementBase_GetSize(object obj) => (obj as FancyElementBase).GetSize();
    private Vector2 FancyElementBase_GetBoundaries(object obj) => (obj as FancyElementBase).GetBoundaries();
    private FancyApp FancyElementBase_GetApp(object obj) => (obj as FancyElementBase).App;
    private FancyContainerBase FancyElementBase_GetParent(object obj) => (obj as FancyElementBase).Parent;
    private List<MySprite> FancyElementBase_GetSprites(object obj) => (obj as FancyElementBase).GetSprites();
    private void FancyElementBase_InitElements(object obj) => (obj as FancyElementBase).InitElements();
    private void FancyElementBase_Update(object obj) => (obj as FancyElementBase).Update();
    private void FancyElementBase_Dispose(object obj) => (obj as FancyElementBase).Dispose();

    private List<object> FancyContainerBase_GetChildren(object obj) => (obj as FancyContainerBase).Children.Cast<object>().ToList();
    private Vector2 FancyContainerBase_GetFlexSize(object obj) => (obj as FancyContainerBase).GetFlexSize();
    private void FancyContainerBase_AddChild(object obj, object child) => (obj as FancyContainerBase).AddChild((FancyElementBase)child);
    private void FancyContainerBase_RemoveChild(object obj, object child) => (obj as FancyContainerBase).RemoveChild((FancyElementBase)child);

    private FancyView FancyView_New(int direction, Color? bgColor = null) => new FancyView((FancyView.ViewDirection)direction, bgColor);
    private int FancyView_GetDirection(object obj) => (int)(obj as FancyView).Direction;
    private void FancyView_SetDirection(object obj, int direction) => (obj as FancyView).Direction = (FancyView.ViewDirection)direction;
    private Color FancyView_GetBgColor(object obj) => (Color)(obj as FancyView).BgColor;
    private void FancyView_SetBgColor(object obj, Color bgColor) => (obj as FancyView).BgColor = bgColor;
    private Color FancyView_GetBorderColor(object obj) => (Color)(obj as FancyView).BorderColor;
    private void FancyView_SetBorderColor(object obj, Color borderColor) => (obj as FancyView).BorderColor = borderColor;
    private Vector4 FancyView_GetBorder(object obj) => (Vector4)(obj as FancyView).Border;
    private void FancyView_SetBorder(object obj, Vector4 border) => (obj as FancyView).Border = border;
    private Vector4 FancyView_GetPadding(object obj) => (Vector4)(obj as FancyView).Padding;
    private void FancyView_SetPadding(object obj, Vector4 padding) => (obj as FancyView).Padding = padding;
    private int FancyView_GetGap(object obj) => (int)(obj as FancyView).Gap;
    private void FancyView_SetGap(object obj, int gap) => (obj as FancyView).Gap = gap;

    private FancyScrollView FancyScrollView_New(int direction, Color? bgColor = null) => new FancyScrollView((FancyView.ViewDirection)direction, bgColor);
    private int FancyScrollView_GetBarWidth(object obj) => (int)(obj as FancyScrollView).BarWidth;
    private void FancyScrollView_SetBarWidth(object obj, int width) => (obj as FancyScrollView).BarWidth = width;
    private float FancyScrollView_GetScroll(object obj) => (obj as FancyScrollView).Scroll;
    private void FancyScrollView_SetScroll(object obj, float scroll) => (obj as FancyScrollView).Scroll = scroll;
    private bool FancyScrollView_GetScrollAlwaysVisible(object obj) => (obj as FancyScrollView).ScrollAlwaysVisible;
    private void FancyScrollView_SetScrollAlwaysVisible(object obj, bool visible) => (obj as FancyScrollView).ScrollAlwaysVisible = visible;

    private FancyApp FancyApp_New() => new FancyApp();
    private TouchScreen FancyApp_GetScreen(object obj) => (obj as FancyApp).Screen;
    private RectangleF FancyApp_GetViewport(object obj) => (obj as FancyApp).Viewport;
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

    private ClickHandler FancyButtonBase_GetHandler(object obj) => (obj as FancyButtonBase).Handler;

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

    private FancyLabel FancyLabel_New(string text, float fontSize = 0.5f, TextAlignment alignment = TextAlignment.CENTER) => new FancyLabel(text, fontSize, alignment);
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

    private FancySwitch FancySwitch_New(string[] tabNames, int index = 0, Action<int> onChange = null) => new FancySwitch(tabNames, index, onChange);
    private int FancySwitch_GetIndex(object obj) => (obj as FancySwitch).Index;
    private void FancySwitch_SetIndex(object obj, int index) => (obj as FancySwitch).Index = index;
    private string[] FancySwitch_GetTabNames(object obj) => (obj as FancySwitch).TabNames;
    private string FancySwitch_GetTabName(object obj, int index) => (obj as FancySwitch).TabNames[index];
    private void FancySwitch_SetTabName(object obj, int index, string text) => (obj as FancySwitch).TabNames[index] = text;
    private void FancySwitch_SetOnChange(object obj, Action<int> onChange) => (obj as FancySwitch).OnChange = onChange;

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