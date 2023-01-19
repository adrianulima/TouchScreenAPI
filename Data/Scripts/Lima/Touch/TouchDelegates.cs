using Lima.Touch.UiKit.Elements;
using Lima.Touch.UiKit;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System.Collections.Generic;
using System.Linq;
using System;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRageMath;
using VRage.Game.Entity;

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
      MyAPIGateway.Utilities.SendModMessage(_channel, GetTouchAndUiApiDictionary());
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
      if ((msg as string) == "ApiRequestTouch")
        MyAPIGateway.Utilities.SendModMessage(_channel, GetTouchApiDictionary());
      else if ((msg as string) == "ApiRequestTouchAndUi")
        MyAPIGateway.Utilities.SendModMessage(_channel, GetTouchAndUiApiDictionary());
    }

    private Dictionary<string, Delegate> GetTouchAndUiApiDictionary()
    {
      var dict = new Dictionary<string, Delegate>
      {
        { "TouchTheme_GetBgColor", new Func<object, Color>(TouchTheme_GetBgColor) },
        { "TouchTheme_GetWhiteColor", new Func<object, Color>(TouchTheme_GetWhiteColor) },
        { "TouchTheme_GetMainColor", new Func<object, Color>(TouchTheme_GetMainColor) },
        { "TouchTheme_GetMainColorDarker", new Func<object, int, Color>(TouchTheme_GetMainColorDarker) },
        { "TouchTheme_MeasureStringInPixels", new Func<object, string, string, float, Vector2>(TouchTheme_MeasureStringInPixels) },
        { "TouchTheme_GetScale", new Func<object, float>(TouchTheme_GetScale) },
        { "TouchTheme_SetScale", new Action<object, float>(TouchTheme_SetScale) },
        { "TouchTheme_GetFont", new Func<object, string>(TouchTheme_GetFont) },
        { "TouchTheme_SetFont", new Action<object, string>(TouchTheme_SetFont) },

        { "TouchElementBase_GetEnabled", new Func<object, bool>(TouchElementBase_GetEnabled) },
        { "TouchElementBase_SetEnabled", new Action<object, bool>(TouchElementBase_SetEnabled) },
        { "TouchElementBase_GetAbsolute", new Func<object, bool>(TouchElementBase_GetAbsolute) },
        { "TouchElementBase_SetAbsolute", new Action<object, bool>(TouchElementBase_SetAbsolute) },
        { "TouchElementBase_GetSelfAlignment", new Func<object, byte>(TouchElementBase_GetSelfAlignment) },
        { "TouchElementBase_SetSelfAlignment", new Action<object, byte>(TouchElementBase_SetSelfAlignment) },
        { "TouchElementBase_GetPosition", new Func<object, Vector2>(TouchElementBase_GetPosition) },
        { "TouchElementBase_SetPosition", new Action<object, Vector2>(TouchElementBase_SetPosition) },
        { "TouchElementBase_GetMargin", new Func<object, Vector4>(TouchElementBase_GetMargin) },
        { "TouchElementBase_SetMargin", new Action<object, Vector4>(TouchElementBase_SetMargin) },
        { "TouchElementBase_GetScale", new Func<object, Vector2>(TouchElementBase_GetScale) },
        { "TouchElementBase_SetScale", new Action<object, Vector2>(TouchElementBase_SetScale) },
        { "TouchElementBase_GetPixels", new Func<object, Vector2>(TouchElementBase_GetPixels) },
        { "TouchElementBase_SetPixels", new Action<object, Vector2>(TouchElementBase_SetPixels) },
        { "TouchElementBase_GetSize", new Func<object, Vector2>(TouchElementBase_GetSize) },
        { "TouchElementBase_GetBoundaries", new Func<object, Vector2>(TouchElementBase_GetBoundaries) },
        { "TouchElementBase_GetApp", new Func<object, TouchApp>(TouchElementBase_GetApp) },
        { "TouchElementBase_GetParent", new Func<object, TouchContainerBase>(TouchElementBase_GetParent) },
        { "TouchElementBase_GetSprites", new Func<object, List<MySprite>>(TouchElementBase_GetSprites) },
        { "TouchElementBase_ForceUpdate", new Action<object>(TouchElementBase_ForceUpdate) },
        { "TouchElementBase_ForceDispose", new Action<object>(TouchElementBase_ForceDispose) },
        { "TouchElementBase_RegisterUpdate", new Action<object, Action>(TouchElementBase_RegisterUpdate) },
        { "TouchElementBase_UnregisterUpdate", new Action<object, Action>(TouchElementBase_UnregisterUpdate) },

        { "TouchContainerBase_GetChildren", new Func<object, List<object>>(TouchContainerBase_GetChildren) },
        { "TouchContainerBase_GetFlexSize", new Func<object, Vector2>(TouchContainerBase_GetFlexSize) },
        { "TouchContainerBase_AddChild", new Action<object, object>(TouchContainerBase_AddChild) },
        { "TouchContainerBase_AddChildAt", new Action<object, object, int>(TouchContainerBase_AddChildAt) },
        { "TouchContainerBase_RemoveChild", new Action<object, object>(TouchContainerBase_RemoveChild) },
        { "TouchContainerBase_RemoveChildAt", new Action<object, int>(TouchContainerBase_RemoveChildAt) },
        { "TouchContainerBase_MoveChild", new Action<object, object, int>(TouchContainerBase_MoveChild) },

        { "TouchView_New", new Func<byte, Color?, TouchView>(TouchView_New) },
        { "TouchView_GetOverflow", new Func<object, bool>(TouchView_GetOverflow) },
        { "TouchView_SetOverflow", new Action<object, bool>(TouchView_SetOverflow) },
        { "TouchView_GetDirection", new Func<object, byte>(TouchView_GetDirection) },
        { "TouchView_SetDirection", new Action<object, byte>(TouchView_SetDirection) },
        { "TouchView_GetAlignment", new Func<object, byte>(TouchView_GetAlignment) },
        { "TouchView_SetAlignment", new Action<object, byte>(TouchView_SetAlignment) },
        { "TouchView_GetAnchor", new Func<object, byte>(TouchView_GetAnchor) },
        { "TouchView_SetAnchor", new Action<object, byte>(TouchView_SetAnchor) },
        { "TouchView_GetUseThemeColors", new Func<object, bool>(TouchView_GetUseThemeColors) },
        { "TouchView_SetUseThemeColors", new Action<object, bool>(TouchView_SetUseThemeColors) },
        { "TouchView_GetBgColor", new Func<object, Color>(TouchView_GetBgColor) },
        { "TouchView_SetBgColor", new Action<object, Color>(TouchView_SetBgColor) },
        { "TouchView_GetBorderColor", new Func<object, Color>(TouchView_GetBorderColor) },
        { "TouchView_SetBorderColor", new Action<object, Color>(TouchView_SetBorderColor) },
        { "TouchView_GetBorder", new Func<object, Vector4>(TouchView_GetBorder) },
        { "TouchView_SetBorder", new Action<object, Vector4>(TouchView_SetBorder) },
        { "TouchView_GetPadding", new Func<object, Vector4>(TouchView_GetPadding) },
        { "TouchView_SetPadding", new Action<object, Vector4>(TouchView_SetPadding) },
        { "TouchView_GetGap", new Func<object, int>(TouchView_GetGap) },
        { "TouchView_SetGap", new Action<object, int>(TouchView_SetGap) },

        { "TouchScrollView_New", new Func<int, Color?, TouchScrollView>(TouchScrollView_New) },
        { "TouchScrollView_GetScroll", new Func<object, float>(TouchScrollView_GetScroll) },
        { "TouchScrollView_SetScroll", new Action<object, float>(TouchScrollView_SetScroll) },
        { "TouchScrollView_GetScrollAlwaysVisible", new Func<object, bool>(TouchScrollView_GetScrollAlwaysVisible) },
        { "TouchScrollView_SetScrollAlwaysVisible", new Action<object, bool>(TouchScrollView_SetScrollAlwaysVisible) },
        { "TouchScrollView_GetScrollBar", new Func<object, object>(TouchScrollView_GetScrollBar) },

        { "TouchApp_New", new Func<TouchApp>(TouchApp_New) },
        { "TouchApp_GetScreen", new Func<object, TouchScreen>(TouchApp_GetScreen) },
        { "TouchApp_GetViewport", new Func<object, RectangleF>(TouchApp_GetViewport) },
        { "TouchApp_GetCursor", new Func<object, TouchCursor>(TouchApp_GetCursor) },
        { "TouchApp_GetTheme", new Func<object, TouchTheme>(TouchApp_GetTheme) },
        { "TouchApp_GetDefaultBg", new Func<object, bool>(TouchApp_GetDefaultBg) },
        { "TouchApp_SetDefaultBg", new Action<object, bool>(TouchApp_SetDefaultBg) },
        { "TouchApp_InitApp", new Action<object, MyCubeBlock, Sandbox.ModAPI.Ingame.IMyTextSurface>(TouchApp_InitApp) },

        { "TouchEmptyButton_New", new Func<Action, object>(TouchEmptyButton_New) },
        { "TouchEmptyButton_GetHandler", new Func<object, object>(TouchEmptyButton_GetHandler) },
        { "TouchEmptyButton_SetOnChange", new Action<object, Action>(TouchEmptyButton_SetOnChange) },

        { "TouchButton_New", new Func<string, Action, object>(TouchButton_New) },
        { "TouchButton_GetLabel", new Func<object, object>(TouchButton_GetLabel) },

        { "TouchCheckbox_New", new Func<Action<bool>, bool, object>(TouchCheckbox_New) },
        { "TouchCheckbox_GetValue", new Func<object, bool>(TouchCheckbox_GetValue) },
        { "TouchCheckbox_SetValue", new Action<object, bool>(TouchCheckbox_SetValue) },
        { "TouchCheckbox_SetOnChange", new Action<object, Action<bool>>(TouchCheckbox_SetOnChange) },
        { "TouchCheckbox_GetCheckMark", new Func<object, object>(TouchCheckbox_GetCheckMark) },

        { "TouchLabel_New", new Func<string, float, TextAlignment, object>(TouchLabel_New) },
        { "TouchLabel_GetAutoBreakLine", new Func<object, bool>(TouchLabel_GetAutoBreakLine) },
        { "TouchLabel_SetAutoBreakLine", new Action<object, bool>(TouchLabel_SetAutoBreakLine) },
        { "TouchLabel_GetAutoEllipsis", new Func<object, byte>(TouchLabel_GetAutoEllipsis) },
        { "TouchLabel_SetAutoEllipsis", new Action<object, byte>(TouchLabel_SetAutoEllipsis) },
        { "TouchLabel_GetHasEllipsis", new Func<object, bool>(TouchLabel_GetHasEllipsis) },
        { "TouchLabel_GetText", new Func<object, string>(TouchLabel_GetText) },
        { "TouchLabel_SetText", new Action<object, string>(TouchLabel_SetText) },
        { "TouchLabel_GetTextColor", new Func<object, Color?>(TouchLabel_GetTextColor) },
        { "TouchLabel_SetTextColor", new Action<object, Color>(TouchLabel_SetTextColor) },
        { "TouchLabel_GetFontSize", new Func<object, float>(TouchLabel_GetFontSize) },
        { "TouchLabel_SetFontSize", new Action<object, float>(TouchLabel_SetFontSize) },
        { "TouchLabel_GetAlignment", new Func<object, TextAlignment>(TouchLabel_GetAlignment) },
        { "TouchLabel_SetAlignment", new Action<object, TextAlignment>(TouchLabel_SetAlignment) },

        { "TouchBarContainer_New", new Func<bool, object>(TouchBarContainer_New) },
        { "TouchBarContainer_GetIsVertical", new Func<object, bool>(TouchBarContainer_GetIsVertical) },
        { "TouchBarContainer_SetIsVertical", new Action<object, bool>(TouchBarContainer_SetIsVertical) },
        { "TouchBarContainer_GetRatio", new Func<object, float>(TouchBarContainer_GetRatio) },
        { "TouchBarContainer_SetRatio", new Action<object, float>(TouchBarContainer_SetRatio) },
        { "TouchBarContainer_GetOffset", new Func<object, float>(TouchBarContainer_GetOffset) },
        { "TouchBarContainer_SetOffset", new Action<object, float>(TouchBarContainer_SetOffset) },
        { "TouchBarContainer_GetBar", new Func<object, object>(TouchBarContainer_GetBar) },

        { "TouchProgressBar_New", new Func<float, float, bool, float, object>(TouchProgressBar_New) },
        { "TouchProgressBar_GetValue", new Func<object, float>(TouchProgressBar_GetValue) },
        { "TouchProgressBar_SetValue", new Action<object, float>(TouchProgressBar_SetValue) },
        { "TouchProgressBar_GetMaxValue", new Func<object, float>(TouchProgressBar_GetMaxValue) },
        { "TouchProgressBar_SetMaxValue", new Action<object, float>(TouchProgressBar_SetMaxValue) },
        { "TouchProgressBar_GetMinValue", new Func<object, float>(TouchProgressBar_GetMinValue) },
        { "TouchProgressBar_SetMinValue", new Action<object, float>(TouchProgressBar_SetMinValue) },
        { "TouchProgressBar_GetBarsGap", new Func<object, float>(TouchProgressBar_GetBarsGap) },
        { "TouchProgressBar_SetBarsGap", new Action<object, float>(TouchProgressBar_SetBarsGap) },
        { "TouchProgressBar_GetLabel", new Func<object, object>(TouchProgressBar_GetLabel) },

        { "TouchSelector_New", new Func<List<string>, Action<int, string>, bool, object>(TouchSelector_New) },
        { "TouchSelector_GetLoop", new Func<object, bool>(TouchSelector_GetLoop) },
        { "TouchSelector_SetLoop", new Action<object, bool>(TouchSelector_SetLoop) },
        { "TouchSelector_GetSelected", new Func<object, int>(TouchSelector_GetSelected) },
        { "TouchSelector_SetSelected", new Action<object, int>(TouchSelector_SetSelected) },
        { "TouchSelector_SetOnChange", new Action<object, Action<int, string>>(TouchSelector_SetOnChange) },

        { "TouchSlider_New", new Func<float, float, Action<float>, object>(TouchSlider_New) },
        { "TouchSlider_GetMaxValue", new Func<object, float>(TouchSlider_GetMaxValue) },
        { "TouchSlider_SetMaxValue", new Action<object, float>(TouchSlider_SetMaxValue) },
        { "TouchSlider_GetMinValue", new Func<object, float>(TouchSlider_GetMinValue) },
        { "TouchSlider_SetMinValue", new Action<object, float>(TouchSlider_SetMinValue) },
        { "TouchSlider_GetValue", new Func<object, float>(TouchSlider_GetValue) },
        { "TouchSlider_SetValue", new Action<object, float>(TouchSlider_SetValue) },
        { "TouchSlider_SetOnChange", new Action<object, Action<float>>(TouchSlider_SetOnChange) },
        { "TouchSlider_GetIsInteger", new Func<object, bool>(TouchSlider_GetIsInteger) },
        { "TouchSlider_SetIsInteger", new Action<object, bool>(TouchSlider_SetIsInteger) },
        { "TouchSlider_GetAllowInput", new Func<object, bool>(TouchSlider_GetAllowInput) },
        { "TouchSlider_SetAllowInput", new Action<object, bool>(TouchSlider_SetAllowInput) },
        { "TouchSlider_GetBar", new Func<object, object>(TouchSlider_GetBar) },
        { "TouchSlider_GetThumb", new Func<object, object>(TouchSlider_GetThumb) },
        { "TouchSlider_GetTextInput", new Func<object, object>(TouchSlider_GetTextInput) },

        { "TouchSliderRange_NewR", new Func<float, float, Action<float, float>, object>(TouchSliderRange_NewR) },
        { "TouchSliderRange_GetValueLower", new Func<object, float>(TouchSliderRange_GetValueLower) },
        { "TouchSliderRange_SetValueLower", new Action<object, float>(TouchSliderRange_SetValueLower) },
        { "TouchSliderRange_SetOnChangeR", new Action<object, Action<float, float>>(TouchSliderRange_SetOnChangeR) },
        { "TouchSliderRange_GetThumbLower", new Func<object, object>(TouchSliderRange_GetThumbLower) },

        { "TouchSwitch_New", new Func<string[], int, Action<int>, object>(TouchSwitch_New) },
        { "TouchSwitch_GetIndex", new Func<object, int>(TouchSwitch_GetIndex) },
        { "TouchSwitch_SetIndex", new Action<object, int>(TouchSwitch_SetIndex) },
        { "TouchSwitch_GetButtons", new Func<object, TouchButton[]>(TouchSwitch_GetButtons) },
        { "TouchSwitch_SetOnChange", new Action<object, Action<int>>(TouchSwitch_SetOnChange) },

        { "TouchTextField_New", new Func<string, Action<string, bool>, object>(TouchTextField_New) },
        { "TouchTextField_GetIsEditing", new Func<object, bool>(TouchTextField_GetIsEditing) },
        { "TouchTextField_GetText", new Func<object, string>(TouchTextField_GetText) },
        { "TouchTextField_SetText", new Action<object, string>(TouchTextField_SetText) },
        { "TouchTextField_SetOnChange", new Action<object, Action<string, bool>>(TouchTextField_SetOnChange) },
        { "TouchTextField_GetIsNumeric", new Func<object, bool>(TouchTextField_GetIsNumeric) },
        { "TouchTextField_SetIsNumeric", new Action<object, bool>(TouchTextField_SetIsNumeric) },
        { "TouchTextField_GetIsInteger", new Func<object, bool>(TouchTextField_GetIsInteger) },
        { "TouchTextField_SetIsInteger", new Action<object, bool>(TouchTextField_SetIsInteger) },
        { "TouchTextField_GetAllowNegative", new Func<object, bool>(TouchTextField_GetAllowNegative) },
        { "TouchTextField_SetAllowNegative", new Action<object, bool>(TouchTextField_SetAllowNegative) },
        { "TouchTextField_GetLabel", new Func<object, object>(TouchTextField_GetLabel) },

        { "TouchWindowBar_New", new Func<string, object>(TouchWindowBar_New) },
        { "TouchWindowBar_GetLabel", new Func<object, object>(TouchWindowBar_GetLabel) },

        { "TouchChart_New", new Func<int, object>(TouchChart_New) },
        { "TouchChart_GetDataSets", new Func<object, List<float[]>>(TouchChart_GetDataSets) },
        { "TouchChart_GetDataColors", new Func<object, List<Color>>(TouchChart_GetDataColors) },
        { "TouchChart_GetGridHorizontalLines", new Func<object, int>(TouchChart_GetGridHorizontalLines) },
        { "TouchChart_SetGridHorizontalLines", new Action<object, int>(TouchChart_SetGridHorizontalLines) },
        { "TouchChart_GetGridVerticalLines", new Func<object, int>(TouchChart_GetGridVerticalLines) },
        { "TouchChart_SetGridVerticalLines", new Action<object, int>(TouchChart_SetGridVerticalLines) },
        { "TouchChart_GetMaxValue", new Func<object, float>(TouchChart_GetMaxValue) },
        { "TouchChart_GetMinValue", new Func<object, float>(TouchChart_GetMinValue) },
        { "TouchChart_GetGridColor", new Func<object, Color?>(TouchChart_GetGridColor) },
        { "TouchChart_SetGridColor", new Action<object, Color>(TouchChart_SetGridColor) },

        { "TouchEmptyElement_New", new Func<object>(TouchEmptyElement_New) }
      };

      return GetTouchApiDictionary().Concat(dict).ToLookup(x => x.Key, x => x.Value).ToDictionary(x => x.Key, g => g.First());
    }

    private Dictionary<string, Delegate> GetTouchApiDictionary()
    {
      var dict = new Dictionary<string, Delegate>
      {
        { "CreateTouchScreen", new Func<IMyCubeBlock, IMyTextSurface, TouchScreen>(CreateTouchScreen) },
        { "RemoveTouchScreen", new Action<IMyCubeBlock, IMyTextSurface>(RemoveTouchScreen) },
        { "AddSurfaceCoords", new Action<string>(AddSurfaceCoords) },
        { "RemoveSurfaceCoords", new Action<string>(RemoveSurfaceCoords) },

        { "TouchScreen_GetBlock", new Func<object, IMyCubeBlock>(TouchScreen_GetBlock) },
        { "TouchScreen_GetSurface", new Func<object, IMyTextSurface>(TouchScreen_GetSurface) },
        { "TouchScreen_GetIndex", new Func<object, int>(TouchScreen_GetIndex) },
        { "TouchScreen_IsOnScreen", new Func<object, bool>(TouchScreen_IsOnScreen) },
        { "TouchScreen_GetCursorPosition", new Func<object, Vector2>(TouchScreen_GetCursorPosition) },
        { "TouchScreen_GetInteractiveDistance", new Func<object, float>(TouchScreen_GetInteractiveDistance) },
        { "TouchScreen_SetInteractiveDistance", new Action<object, float>(TouchScreen_SetInteractiveDistance) },
        { "TouchScreen_GetRotation", new Func<object, int>(TouchScreen_GetRotation) },
        { "TouchScreen_CompareWithBlockAndSurface", new Func<object, IMyCubeBlock, IMyTextSurface, bool>(TouchScreen_CompareWithBlockAndSurface) },
        { "TouchScreen_ForceDispose", new Action<object>(TouchScreen_ForceDispose) },

        { "TouchCursor_New", new Func<object, TouchCursor>(TouchCursor_New) },
        { "TouchCursor_GetActive", new Func<object, bool>(TouchCursor_GetActive) },
        { "TouchCursor_SetActive", new Action<object, bool>(TouchCursor_SetActive) },
        { "TouchCursor_GetScale", new Func<object, float>(TouchCursor_GetScale) },
        { "TouchCursor_SetScale", new Action<object, float>(TouchCursor_SetScale) },
        { "TouchCursor_GetPosition", new Func<object, Vector2>(TouchCursor_GetPosition) },
        { "TouchCursor_IsInsideArea", new Func<object, float, float, float, float, bool>(TouchCursor_IsInsideArea) },
        { "TouchCursor_GetSprites", new Func<object, List<MySprite>>(TouchCursor_GetSprites) },
        { "TouchCursor_ForceDispose", new Action<object>(TouchCursor_ForceDispose) },

        { "ClickHandler_New", new Func<object>(ClickHandler_New) },
        { "ClickHandler_GetHitArea", new Func<object, Vector4>(ClickHandler_GetHitArea) },
        { "ClickHandler_SetHitArea", new Action<object, Vector4>(ClickHandler_SetHitArea) },
        { "ClickHandler_IsMouseReleased", new Func<object, bool>(ClickHandler_IsMouseReleased) },
        { "ClickHandler_IsMouseOver", new Func<object, bool>(ClickHandler_IsMouseOver) },
        { "ClickHandler_IsMousePressed", new Func<object, bool>(ClickHandler_IsMousePressed) },
        { "ClickHandler_JustReleased", new Func<object, bool>(ClickHandler_JustReleased) },
        { "ClickHandler_JustPressed", new Func<object, bool>(ClickHandler_JustPressed) },
        { "ClickHandler_UpdateStatus", new Action<object, object>(ClickHandler_UpdateStatus) },
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
    private Vector2 TouchScreen_GetCursorPosition(object obj) => (obj as TouchScreen).CursorPosition;
    private float TouchScreen_GetInteractiveDistance(object obj) => (obj as TouchScreen).InteractiveDistance;
    private void TouchScreen_SetInteractiveDistance(object obj, float distance) => (obj as TouchScreen).InteractiveDistance = distance;
    private int TouchScreen_GetRotation(object obj) => (obj as TouchScreen).Rotation;
    private bool TouchScreen_CompareWithBlockAndSurface(object obj, IMyCubeBlock block, IMyTextSurface surface) => (obj as TouchScreen).CompareWithBlockAndSurface(block, surface);
    private void TouchScreen_ForceDispose(object obj) => (obj as TouchScreen).Dispose();

    private bool TouchElementBase_GetEnabled(object obj) => (obj as TouchElementBase).Enabled;
    private void TouchElementBase_SetEnabled(object obj, bool enabled) => (obj as TouchElementBase).Enabled = enabled;
    private bool TouchElementBase_GetAbsolute(object obj) => (obj as TouchElementBase).Absolute;
    private void TouchElementBase_SetAbsolute(object obj, bool absolute) => (obj as TouchElementBase).Absolute = absolute;
    private byte TouchElementBase_GetSelfAlignment(object obj) => (byte)(obj as TouchElementBase).SelfAlignment;
    private void TouchElementBase_SetSelfAlignment(object obj, byte alignment) => (obj as TouchElementBase).SelfAlignment = (ViewAlignment)alignment;
    private Vector2 TouchElementBase_GetPosition(object obj) => (obj as TouchElementBase).Position;
    private void TouchElementBase_SetPosition(object obj, Vector2 position) => (obj as TouchElementBase).Position = position;
    private Vector4 TouchElementBase_GetMargin(object obj) => (obj as TouchElementBase).Margin;
    private void TouchElementBase_SetMargin(object obj, Vector4 margin) => (obj as TouchElementBase).Margin = margin;
    private Vector2 TouchElementBase_GetScale(object obj) => (obj as TouchElementBase).Scale;
    private void TouchElementBase_SetScale(object obj, Vector2 scale) => (obj as TouchElementBase).Scale = scale;
    private Vector2 TouchElementBase_GetPixels(object obj) => (obj as TouchElementBase).Pixels;
    private void TouchElementBase_SetPixels(object obj, Vector2 pixels) => (obj as TouchElementBase).Pixels = pixels;
    private Vector2 TouchElementBase_GetSize(object obj) => (obj as TouchElementBase).GetSize();
    private Vector2 TouchElementBase_GetBoundaries(object obj) => (obj as TouchElementBase).GetBoundaries();
    private TouchApp TouchElementBase_GetApp(object obj) => (obj as TouchElementBase).App;
    private TouchContainerBase TouchElementBase_GetParent(object obj) => (obj as TouchElementBase).Parent;
    private List<MySprite> TouchElementBase_GetSprites(object obj) => (obj as TouchElementBase).GetSprites();
    private void TouchElementBase_ForceUpdate(object obj) => (obj as TouchElementBase).Update();
    private void TouchElementBase_ForceDispose(object obj) => (obj as TouchElementBase).Dispose();
    private void TouchElementBase_RegisterUpdate(object obj, Action update) => (obj as TouchElementBase).UpdateEvent += update;
    private void TouchElementBase_UnregisterUpdate(object obj, Action update) => (obj as TouchElementBase).UpdateEvent -= update;

    private List<object> TouchContainerBase_GetChildren(object obj) => (obj as TouchContainerBase).Children.Cast<object>().ToList();
    private Vector2 TouchContainerBase_GetFlexSize(object obj) => (obj as TouchContainerBase).GetFlexSize();
    private void TouchContainerBase_AddChild(object obj, object child) => (obj as TouchContainerBase).AddChild((TouchElementBase)child);
    private void TouchContainerBase_AddChildAt(object obj, object child, int index) => (obj as TouchContainerBase).AddChild((TouchElementBase)child, index);
    private void TouchContainerBase_RemoveChild(object obj, object child) => (obj as TouchContainerBase).RemoveChild((TouchElementBase)child);
    private void TouchContainerBase_RemoveChildAt(object obj, int index) => (obj as TouchContainerBase).RemoveChild(index);
    private void TouchContainerBase_MoveChild(object obj, object child, int index) => (obj as TouchContainerBase).MoveChild((TouchElementBase)child, index);

    private TouchView TouchView_New(byte direction, Color? bgColor = null) => new TouchView((ViewDirection)direction, bgColor);
    private bool TouchView_GetOverflow(object obj) => (obj as TouchView).Overflow;
    private void TouchView_SetOverflow(object obj, bool overflow) => (obj as TouchView).Overflow = overflow;
    private byte TouchView_GetDirection(object obj) => (byte)(obj as TouchView).Direction;
    private void TouchView_SetDirection(object obj, byte direction) => (obj as TouchView).Direction = (ViewDirection)direction;
    private byte TouchView_GetAlignment(object obj) => (byte)(obj as TouchView).Alignment;
    private void TouchView_SetAlignment(object obj, byte alignment) => (obj as TouchView).Alignment = (ViewAlignment)alignment;
    private byte TouchView_GetAnchor(object obj) => (byte)(obj as TouchView).Anchor;
    private void TouchView_SetAnchor(object obj, byte anchor) => (obj as TouchView).Anchor = (ViewAnchor)anchor;
    private bool TouchView_GetUseThemeColors(object obj) => (obj as TouchView).UseThemeColors;
    private void TouchView_SetUseThemeColors(object obj, bool useTheme) => (obj as TouchView).UseThemeColors = useTheme;
    private Color TouchView_GetBgColor(object obj) => (Color)(obj as TouchView).BgColor;
    private void TouchView_SetBgColor(object obj, Color bgColor) => (obj as TouchView).BgColor = bgColor;
    private Color TouchView_GetBorderColor(object obj) => (Color)(obj as TouchView).BorderColor;
    private void TouchView_SetBorderColor(object obj, Color borderColor) => (obj as TouchView).BorderColor = borderColor;
    private Vector4 TouchView_GetBorder(object obj) => (Vector4)(obj as TouchView).Border;
    private void TouchView_SetBorder(object obj, Vector4 border) => (obj as TouchView).Border = border;
    private Vector4 TouchView_GetPadding(object obj) => (Vector4)(obj as TouchView).Padding;
    private void TouchView_SetPadding(object obj, Vector4 padding) => (obj as TouchView).Padding = padding;
    private int TouchView_GetGap(object obj) => (int)(obj as TouchView).Gap;
    private void TouchView_SetGap(object obj, int gap) => (obj as TouchView).Gap = gap;

    private TouchScrollView TouchScrollView_New(int direction, Color? bgColor = null) => new TouchScrollView((ViewDirection)direction, bgColor);
    private float TouchScrollView_GetScroll(object obj) => (obj as TouchScrollView).Scroll;
    private void TouchScrollView_SetScroll(object obj, float scroll) => (obj as TouchScrollView).Scroll = scroll;
    private bool TouchScrollView_GetScrollAlwaysVisible(object obj) => (obj as TouchScrollView).ScrollAlwaysVisible;
    private void TouchScrollView_SetScrollAlwaysVisible(object obj, bool visible) => (obj as TouchScrollView).ScrollAlwaysVisible = visible;
    private TouchBarContainer TouchScrollView_GetScrollBar(object obj) => (obj as TouchScrollView).ScrollBar;

    private TouchApp TouchApp_New() => new TouchApp();
    private TouchScreen TouchApp_GetScreen(object obj) => (obj as TouchApp).Screen;
    private RectangleF TouchApp_GetViewport(object obj) => (obj as TouchApp).Viewport;
    private TouchCursor TouchApp_GetCursor(object obj) => (obj as TouchApp).Cursor;
    private TouchTheme TouchApp_GetTheme(object obj) => (obj as TouchApp).Theme;
    private bool TouchApp_GetDefaultBg(object obj) => (obj as TouchApp).DefaultBg;
    private void TouchApp_SetDefaultBg(object obj, bool defaultBg) => (obj as TouchApp).DefaultBg = defaultBg;
    private void TouchApp_InitApp(object obj, MyCubeBlock block, Sandbox.ModAPI.Ingame.IMyTextSurface surface) => (obj as TouchApp).InitApp(block, surface);

    private TouchCursor TouchCursor_New(object screen) => new TouchCursor(screen as TouchScreen);
    private bool TouchCursor_GetActive(object obj) => (obj as TouchCursor).Active;
    private void TouchCursor_SetActive(object obj, bool active) => (obj as TouchCursor).Active = active;
    private float TouchCursor_GetScale(object obj) => (obj as TouchCursor).Scale;
    private void TouchCursor_SetScale(object obj, float scale) => (obj as TouchCursor).Scale = scale;
    private Vector2 TouchCursor_GetPosition(object obj) => (obj as TouchCursor).Position;
    private bool TouchCursor_IsInsideArea(object obj, float x, float y, float z, float w) => (obj as TouchCursor).IsInsideArea(x, y, z, w);
    private List<MySprite> TouchCursor_GetSprites(object obj) => (obj as TouchCursor).GetSprites();
    private void TouchCursor_ForceDispose(object obj) => (obj as TouchCursor).Dispose();

    private Color TouchTheme_GetBgColor(object obj) => (obj as TouchTheme).BgColor;
    private Color TouchTheme_GetWhiteColor(object obj) => (obj as TouchTheme).WhiteColor;
    private Color TouchTheme_GetMainColor(object obj) => (obj as TouchTheme).MainColor;
    private Color TouchTheme_GetMainColorDarker(object obj, int value)
    {
      var theme = (obj as TouchTheme);
      if (value <= 1) return theme.MainColor_1;
      else if (value <= 2) return theme.MainColor_2;
      else if (value <= 3) return theme.MainColor_3;
      else if (value <= 4) return theme.MainColor_4;
      else if (value <= 5) return theme.MainColor_5;
      else if (value <= 6) return theme.MainColor_6;
      else if (value <= 7) return theme.MainColor_7;
      else if (value <= 8) return theme.MainColor_8;
      return theme.MainColor_9;
    }
    private Vector2 TouchTheme_MeasureStringInPixels(object obj, string text, string font, float scale) => (obj as TouchTheme).MeasureStringInPixels(text, font, scale);
    private float TouchTheme_GetScale(object obj) => (obj as TouchTheme).Scale;
    private void TouchTheme_SetScale(object obj, float scale) => (obj as TouchTheme).Scale = scale;
    private string TouchTheme_GetFont(object obj) => (obj as TouchTheme).Font;
    private void TouchTheme_SetFont(object obj, string font) => (obj as TouchTheme).Font = font;

    private ClickHandler ClickHandler_New() => new ClickHandler();
    private Vector4 ClickHandler_GetHitArea(object obj) => (obj as ClickHandler).HitArea;
    private void ClickHandler_SetHitArea(object obj, Vector4 hitArea) => (obj as ClickHandler).HitArea = hitArea;
    private bool ClickHandler_IsMouseReleased(object obj) => (obj as ClickHandler).IsMouseReleased;
    private bool ClickHandler_IsMouseOver(object obj) => (obj as ClickHandler).IsMouseOver;
    private bool ClickHandler_IsMousePressed(object obj) => (obj as ClickHandler).IsMousePressed;
    private bool ClickHandler_JustReleased(object obj) => (obj as ClickHandler).JustReleased;
    private bool ClickHandler_JustPressed(object obj) => (obj as ClickHandler).JustPressed;
    private void ClickHandler_UpdateStatus(object obj, object screen) => (obj as ClickHandler).UpdateStatus(screen as TouchScreen);

    private TouchEmptyButton TouchEmptyButton_New(Action onChange) => new TouchEmptyButton(onChange);
    private ClickHandler TouchEmptyButton_GetHandler(object obj) => (obj as TouchEmptyButton).Handler;
    private void TouchEmptyButton_SetOnChange(object obj, Action onChange) => (obj as TouchEmptyButton).OnChange = onChange;

    private TouchButton TouchButton_New(string text, Action onChange) => new TouchButton(text, onChange);
    private TouchLabel TouchButton_GetLabel(object obj) => (obj as TouchButton).Label;

    private TouchCheckbox TouchCheckbox_New(Action<bool> onChange, bool value = false) => new TouchCheckbox(onChange, value);
    private bool TouchCheckbox_GetValue(object obj) => (obj as TouchCheckbox).Value;
    private void TouchCheckbox_SetValue(object obj, bool value) => (obj as TouchCheckbox).Value = value;
    private void TouchCheckbox_SetOnChange(object obj, Action<bool> onChange) => (obj as TouchCheckbox).OnChange = onChange;
    private TouchEmptyElement TouchCheckbox_GetCheckMark(object obj) => (obj as TouchCheckbox).CheckMark;

    private TouchLabel TouchLabel_New(string text, float fontSize = 0.5f, TextAlignment alignment = TextAlignment.CENTER) => new TouchLabel(text, fontSize, alignment);
    private bool TouchLabel_GetAutoBreakLine(object obj) => (obj as TouchLabel).AutoBreakLine;
    private void TouchLabel_SetAutoBreakLine(object obj, bool breakLine) => (obj as TouchLabel).AutoBreakLine = breakLine;
    private byte TouchLabel_GetAutoEllipsis(object obj) => (byte)(obj as TouchLabel).AutoEllipsis;
    private void TouchLabel_SetAutoEllipsis(object obj, byte overflow) => (obj as TouchLabel).AutoEllipsis = (LabelEllipsis)overflow;
    private bool TouchLabel_GetHasEllipsis(object obj) => (obj as TouchLabel).HasEllipsis;
    private string TouchLabel_GetText(object obj) => (obj as TouchLabel).Text;
    private void TouchLabel_SetText(object obj, string text) => (obj as TouchLabel).Text = text;
    private Color? TouchLabel_GetTextColor(object obj) => (obj as TouchLabel).TextColor;
    private void TouchLabel_SetTextColor(object obj, Color color) => (obj as TouchLabel).TextColor = color;
    private float TouchLabel_GetFontSize(object obj) => (obj as TouchLabel).FontSize;
    private void TouchLabel_SetFontSize(object obj, float fontSize) => (obj as TouchLabel).FontSize = fontSize;
    private TextAlignment TouchLabel_GetAlignment(object obj) => (obj as TouchLabel).Alignment;
    private void TouchLabel_SetAlignment(object obj, TextAlignment alignment) => (obj as TouchLabel).Alignment = alignment;

    private TouchBarContainer TouchBarContainer_New(bool vertical = false) => new TouchBarContainer(vertical);
    private bool TouchBarContainer_GetIsVertical(object obj) => (obj as TouchBarContainer).IsVertical;
    private void TouchBarContainer_SetIsVertical(object obj, bool vertical) => (obj as TouchBarContainer).IsVertical = vertical;
    private float TouchBarContainer_GetRatio(object obj) => (obj as TouchBarContainer).Ratio;
    private void TouchBarContainer_SetRatio(object obj, float ratio) => (obj as TouchBarContainer).Ratio = ratio;
    private float TouchBarContainer_GetOffset(object obj) => (obj as TouchBarContainer).Offset;
    private void TouchBarContainer_SetOffset(object obj, float offset) => (obj as TouchBarContainer).Offset = offset;
    private TouchView TouchBarContainer_GetBar(object obj) => (obj as TouchBarContainer).Bar;

    private TouchProgressBar TouchProgressBar_New(float min, float max, bool vertical = false, float barsGap = 0) => new TouchProgressBar(min, max, vertical, barsGap);
    private float TouchProgressBar_GetValue(object obj) => (obj as TouchProgressBar).Value;
    private void TouchProgressBar_SetValue(object obj, float value) => (obj as TouchProgressBar).Value = value;
    private float TouchProgressBar_GetMaxValue(object obj) => (obj as TouchProgressBar).MaxValue;
    private void TouchProgressBar_SetMaxValue(object obj, float max) => (obj as TouchProgressBar).MaxValue = max;
    private float TouchProgressBar_GetMinValue(object obj) => (obj as TouchProgressBar).MinValue;
    private void TouchProgressBar_SetMinValue(object obj, float min) => (obj as TouchProgressBar).MinValue = min;
    private float TouchProgressBar_GetBarsGap(object obj) => (obj as TouchProgressBar).BarsGap;
    private void TouchProgressBar_SetBarsGap(object obj, float gap) => (obj as TouchProgressBar).BarsGap = gap;
    private TouchLabel TouchProgressBar_GetLabel(object obj) => (obj as TouchProgressBar).Label;

    private TouchSelector TouchSelector_New(List<string> labels, Action<int, string> onChange, bool loop = true) => new TouchSelector(labels, onChange, loop);
    private bool TouchSelector_GetLoop(object obj) => (obj as TouchSelector).Loop;
    private void TouchSelector_SetLoop(object obj, bool loop) => (obj as TouchSelector).Loop = loop;
    private int TouchSelector_GetSelected(object obj) => (obj as TouchSelector).Selected;
    private void TouchSelector_SetSelected(object obj, int selected) => (obj as TouchSelector).Selected = selected;
    private void TouchSelector_SetOnChange(object obj, Action<int, string> onChange) => (obj as TouchSelector).OnChange = onChange;

    private TouchSlider TouchSlider_New(float min, float max, Action<float> onChange) => new TouchSlider(min, max, onChange);
    private float TouchSlider_GetMaxValue(object obj) => (obj as TouchSlider).MaxValue;
    private void TouchSlider_SetMaxValue(object obj, float max) => (obj as TouchSlider).MaxValue = max;
    private float TouchSlider_GetMinValue(object obj) => (obj as TouchSlider).MinValue;
    private void TouchSlider_SetMinValue(object obj, float min) => (obj as TouchSlider).MinValue = min;
    private float TouchSlider_GetValue(object obj) => (obj as TouchSlider).Value;
    private void TouchSlider_SetValue(object obj, float value) => (obj as TouchSlider).Value = value;
    private void TouchSlider_SetOnChange(object obj, Action<float> onChange) => (obj as TouchSlider).OnChange = onChange;
    private bool TouchSlider_GetIsInteger(object obj) => (obj as TouchSlider).IsInteger;
    private void TouchSlider_SetIsInteger(object obj, bool interger) => (obj as TouchSlider).IsInteger = interger;
    private bool TouchSlider_GetAllowInput(object obj) => (obj as TouchSlider).AllowInput;
    private void TouchSlider_SetAllowInput(object obj, bool allowInput) => (obj as TouchSlider).AllowInput = allowInput;
    private TouchBarContainer TouchSlider_GetBar(object obj) => (obj as TouchSlider).Bar;
    private TouchEmptyElement TouchSlider_GetThumb(object obj) => (obj as TouchSlider).Thumb;
    private TouchTextField TouchSlider_GetTextInput(object obj) => (obj as TouchSlider).InnerTextField;

    private TouchSliderRange TouchSliderRange_NewR(float min, float max, Action<float, float> onChange) => new TouchSliderRange(min, max, onChange);
    private float TouchSliderRange_GetValueLower(object obj) => (obj as TouchSliderRange).ValueLower;
    private void TouchSliderRange_SetValueLower(object obj, float value) => (obj as TouchSliderRange).ValueLower = value;
    private void TouchSliderRange_SetOnChangeR(object obj, Action<float, float> onChange) => (obj as TouchSliderRange).OnChangeR = onChange;
    private TouchEmptyElement TouchSliderRange_GetThumbLower(object obj) => (obj as TouchSliderRange).ThumbLower;

    private TouchSwitch TouchSwitch_New(string[] labels, int index = 0, Action<int> onChange = null) => new TouchSwitch(labels, index, onChange);
    private int TouchSwitch_GetIndex(object obj) => (obj as TouchSwitch).Index;
    private void TouchSwitch_SetIndex(object obj, int index) => (obj as TouchSwitch).Index = index;
    private TouchButton[] TouchSwitch_GetButtons(object obj) => (obj as TouchSwitch).Buttons;
    private void TouchSwitch_SetOnChange(object obj, Action<int> onChange) => (obj as TouchSwitch).OnChange = onChange;

    private TouchTextField TouchTextField_New(string text, Action<string, bool> onChange) => new TouchTextField(text, onChange);
    private bool TouchTextField_GetIsEditing(object obj) => (obj as TouchTextField).IsEditing;
    private string TouchTextField_GetText(object obj) => (obj as TouchTextField).Text;
    private void TouchTextField_SetText(object obj, string text) => (obj as TouchTextField).Text = text;
    private void TouchTextField_SetOnChange(object obj, Action<string, bool> onChange) => (obj as TouchTextField).OnChange = onChange;
    private bool TouchTextField_GetIsNumeric(object obj) => (obj as TouchTextField).IsNumeric;
    private void TouchTextField_SetIsNumeric(object obj, bool isNumeric) => (obj as TouchTextField).IsNumeric = isNumeric;
    private bool TouchTextField_GetIsInteger(object obj) => (obj as TouchTextField).IsInteger;
    private void TouchTextField_SetIsInteger(object obj, bool isInterger) => (obj as TouchTextField).IsInteger = isInterger;
    private bool TouchTextField_GetAllowNegative(object obj) => (obj as TouchTextField).AllowNegative;
    private void TouchTextField_SetAllowNegative(object obj, bool allowNegative) => (obj as TouchTextField).AllowNegative = allowNegative;
    private TouchLabel TouchTextField_GetLabel(object obj) => (obj as TouchTextField).Label;

    private TouchWindowBar TouchWindowBar_New(string text) => new TouchWindowBar(text);
    private TouchLabel TouchWindowBar_GetLabel(object obj) => (obj as TouchWindowBar).Label;

    private TouchChart TouchChart_New(int intervals) => new TouchChart(intervals);
    private List<float[]> TouchChart_GetDataSets(object obj) => (obj as TouchChart).DataSets;
    private List<Color> TouchChart_GetDataColors(object obj) => (obj as TouchChart).DataColors;
    private int TouchChart_GetGridHorizontalLines(object obj) => (obj as TouchChart).GridHorizontalLines;
    private void TouchChart_SetGridHorizontalLines(object obj, int lines) => (obj as TouchChart).GridHorizontalLines = lines;
    private int TouchChart_GetGridVerticalLines(object obj) => (obj as TouchChart).GridVerticalLines;
    private void TouchChart_SetGridVerticalLines(object obj, int lines) => (obj as TouchChart).GridVerticalLines = lines;
    private float TouchChart_GetMaxValue(object obj) => (obj as TouchChart).MaxValue;
    private float TouchChart_GetMinValue(object obj) => (obj as TouchChart).MinValue;
    private Color? TouchChart_GetGridColor(object obj) => (obj as TouchChart).GridColor;
    private void TouchChart_SetGridColor(object obj, Color color) => (obj as TouchChart).GridColor = color;

    private TouchEmptyElement TouchEmptyElement_New() => new TouchEmptyElement();
  }
}