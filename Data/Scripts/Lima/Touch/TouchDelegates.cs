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
        { "TouchScreen_GetRotation", new Func<object, int>(TouchScreen_GetRotation) },
        { "TouchScreen_CompareWithBlockAndSurface", new Func<object, IMyCubeBlock, IMyTextSurface, bool>(TouchScreen_CompareWithBlockAndSurface) },
        { "TouchScreen_ForceDispose", new Action<object>(TouchScreen_ForceDispose) },

        { "FancyElementBase_GetEnabled", new Func<object, bool>(FancyElementBase_GetEnabled) },
        { "FancyElementBase_SetEnabled", new Action<object, bool>(FancyElementBase_SetEnabled) },
        { "FancyElementBase_GetAbsolute", new Func<object, bool>(FancyElementBase_GetAbsolute) },
        { "FancyElementBase_SetAbsolute", new Action<object, bool>(FancyElementBase_SetAbsolute) },
        { "FancyElementBase_GetSelfAlignment", new Func<object, byte>(FancyElementBase_GetSelfAlignment) },
        { "FancyElementBase_SetSelfAlignment", new Action<object, byte>(FancyElementBase_SetSelfAlignment) },
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
        { "FancyElementBase_ForceUpdate", new Action<object>(FancyElementBase_ForceUpdate) },
        { "FancyElementBase_ForceDispose", new Action<object>(FancyElementBase_ForceDispose) },
        { "FancyElementBase_RegisterUpdate", new Action<object, Action>(FancyElementBase_RegisterUpdate) },
        { "FancyElementBase_UnregisterUpdate", new Action<object, Action>(FancyElementBase_UnregisterUpdate) },

        { "FancyContainerBase_GetChildren", new Func<object, List<object>>(FancyContainerBase_GetChildren) },
        { "FancyContainerBase_GetFlexSize", new Func<object, Vector2>(FancyContainerBase_GetFlexSize) },
        { "FancyContainerBase_AddChild", new Action<object, object>(FancyContainerBase_AddChild) },
        { "FancyContainerBase_AddChildAt", new Action<object, object, int>(FancyContainerBase_AddChildAt) },
        { "FancyContainerBase_RemoveChild", new Action<object, object>(FancyContainerBase_RemoveChild) },
        { "FancyContainerBase_RemoveChildAt", new Action<object, int>(FancyContainerBase_RemoveChildAt) },
        { "FancyContainerBase_MoveChild", new Action<object, object, int>(FancyContainerBase_MoveChild) },

        { "FancyView_New", new Func<int, Color?, FancyView>(FancyView_New) },
        { "FancyView_GetOverflow", new Func<object, bool>(FancyView_GetOverflow) },
        { "FancyView_SetOverflow", new Action<object, bool>(FancyView_SetOverflow) },
        { "FancyView_GetDirection", new Func<object, int>(FancyView_GetDirection) },
        { "FancyView_SetDirection", new Action<object, int>(FancyView_SetDirection) },
        { "FancyView_GetAlignment", new Func<object, byte>(FancyView_GetAlignment) },
        { "FancyView_SetAlignment", new Action<object, byte>(FancyView_SetAlignment) },
        { "FancyView_GetAnchor", new Func<object, byte>(FancyView_GetAnchor) },
        { "FancyView_SetAnchor", new Action<object, byte>(FancyView_SetAnchor) },
        { "FancyView_GetUseThemeColors", new Func<object, bool>(FancyView_GetUseThemeColors) },
        { "FancyView_SetUseThemeColors", new Action<object, bool>(FancyView_SetUseThemeColors) },
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
        { "FancyScrollView_GetScroll", new Func<object, float>(FancyScrollView_GetScroll) },
        { "FancyScrollView_SetScroll", new Action<object, float>(FancyScrollView_SetScroll) },
        { "FancyScrollView_GetScrollAlwaysVisible", new Func<object, bool>(FancyScrollView_GetScrollAlwaysVisible) },
        { "FancyScrollView_SetScrollAlwaysVisible", new Action<object, bool>(FancyScrollView_SetScrollAlwaysVisible) },
        { "FancyScrollView_GetScrollBar", new Func<object, object>(FancyScrollView_GetScrollBar) },

        { "FancyApp_New", new Func<FancyApp>(FancyApp_New) },
        { "FancyApp_GetScreen", new Func<object, TouchScreen>(FancyApp_GetScreen) },
        { "FancyApp_GetViewport", new Func<object, RectangleF>(FancyApp_GetViewport) },
        { "FancyApp_GetCursor", new Func<object, FancyCursor>(FancyApp_GetCursor) },
        { "FancyApp_GetTheme", new Func<object, FancyTheme>(FancyApp_GetTheme) },
        { "FancyApp_GetDefaultBg", new Func<object, bool>(FancyApp_GetDefaultBg) },
        { "FancyApp_SetDefaultBg", new Action<object, bool>(FancyApp_SetDefaultBg) },
        { "FancyApp_InitApp", new Action<object, MyCubeBlock, Sandbox.ModAPI.Ingame.IMyTextSurface>(FancyApp_InitApp) },

        { "FancyCursor_New", new Func<object, FancyCursor>(FancyCursor_New) },
        { "FancyCursor_GetActive", new Func<object, bool>(FancyCursor_GetActive) },
        { "FancyCursor_SetActive", new Action<object, bool>(FancyCursor_SetActive) },
        { "FancyCursor_GetPosition", new Func<object, Vector2>(FancyCursor_GetPosition) },
        { "FancyCursor_IsInsideArea", new Func<object, float, float, float, float, bool>(FancyCursor_IsInsideArea) },
        { "FancyCursor_GetSprites", new Func<object, List<MySprite>>(FancyCursor_GetSprites) },
        { "FancyCursor_ForceDispose", new Action<object>(FancyCursor_ForceDispose) },

        { "FancyTheme_GetBgColor", new Func<object, Color>(FancyTheme_GetBgColor) },
        { "FancyTheme_GetWhiteColor", new Func<object, Color>(FancyTheme_GetWhiteColor) },
        { "FancyTheme_GetMainColor", new Func<object, Color>(FancyTheme_GetMainColor) },
        { "FancyTheme_GetMainColorDarker", new Func<object, int, Color>(FancyTheme_GetMainColorDarker) },
        { "FancyTheme_MeasureStringInPixels", new Func<object, string, string, float, Vector2>(FancyTheme_MeasureStringInPixels) },
        { "FancyTheme_GetScale", new Func<object, float>(FancyTheme_GetScale) },
        { "FancyTheme_SetScale", new Action<object, float>(FancyTheme_SetScale) },
        { "FancyTheme_GetFont", new Func<object, string>(FancyTheme_GetFont) },
        { "FancyTheme_SetFont", new Action<object, string>(FancyTheme_SetFont) },

        { "ClickHandler_New", new Func<object>(ClickHandler_New) },
        { "ClickHandler_GetHitArea", new Func<object, Vector4>(ClickHandler_GetHitArea) },
        { "ClickHandler_SetHitArea", new Action<object, Vector4>(ClickHandler_SetHitArea) },
        { "ClickHandler_IsMouseReleased", new Func<object, bool>(ClickHandler_IsMouseReleased) },
        { "ClickHandler_IsMouseOver", new Func<object, bool>(ClickHandler_IsMouseOver) },
        { "ClickHandler_IsMousePressed", new Func<object, bool>(ClickHandler_IsMousePressed) },
        { "ClickHandler_JustReleased", new Func<object, bool>(ClickHandler_JustReleased) },
        { "ClickHandler_JustPressed", new Func<object, bool>(ClickHandler_JustPressed) },
        { "ClickHandler_UpdateStatus", new Action<object, object>(ClickHandler_UpdateStatus) },

        { "FancyEmptyButton_New", new Func<Action, object>(FancyEmptyButton_New) },
        { "FancyEmptyButton_GetHandler", new Func<object, object>(FancyEmptyButton_GetHandler) },
        { "FancyEmptyButton_SetOnChange", new Action<object, Action>(FancyEmptyButton_SetOnChange) },

        { "FancyButton_New", new Func<string, Action, object>(FancyButton_New) },
        { "FancyButton_GetLabel", new Func<object, object>(FancyButton_GetLabel) },

        { "FancyCheckbox_New", new Func<Action<bool>, bool, object>(FancyCheckbox_New) },
        { "FancyCheckbox_GetValue", new Func<object, bool>(FancyCheckbox_GetValue) },
        { "FancyCheckbox_SetValue", new Action<object, bool>(FancyCheckbox_SetValue) },
        { "FancyCheckbox_SetOnChange", new Action<object, Action<bool>>(FancyCheckbox_SetOnChange) },
        { "FancyCheckbox_GetCheckMark", new Func<object, object>(FancyCheckbox_GetCheckMark) },

        { "FancyLabel_New", new Func<string, float, TextAlignment, object>(FancyLabel_New) },
        { "FancyLabel_GetAutoBreakLine", new Func<object, bool>(FancyLabel_GetAutoBreakLine) },
        { "FancyLabel_SetAutoBreakLine", new Action<object, bool>(FancyLabel_SetAutoBreakLine) },
        { "FancyLabel_GetOverflow", new Func<object, bool>(FancyLabel_GetOverflow) },
        { "FancyLabel_SetOverflow", new Action<object, bool>(FancyLabel_SetOverflow) },
        { "FancyLabel_GetIsShortened", new Func<object, bool>(FancyLabel_GetIsShortened) },
        { "FancyLabel_GetText", new Func<object, string>(FancyLabel_GetText) },
        { "FancyLabel_SetText", new Action<object, string>(FancyLabel_SetText) },
        { "FancyLabel_GetTextColor", new Func<object, Color?>(FancyLabel_GetTextColor) },
        { "FancyLabel_SetTextColor", new Action<object, Color>(FancyLabel_SetTextColor) },
        { "FancyLabel_GetFontSize", new Func<object, float>(FancyLabel_GetFontSize) },
        { "FancyLabel_SetFontSize", new Action<object, float>(FancyLabel_SetFontSize) },
        { "FancyLabel_GetAlignment", new Func<object, TextAlignment>(FancyLabel_GetAlignment) },
        { "FancyLabel_SetAlignment", new Action<object, TextAlignment>(FancyLabel_SetAlignment) },

        { "FancyBarContainer_New", new Func<bool, object>(FancyBarContainer_New) },
        { "FancyBarContainer_GetIsVertical", new Func<object, bool>(FancyBarContainer_GetIsVertical) },
        { "FancyBarContainer_SetIsVertical", new Action<object, bool>(FancyBarContainer_SetIsVertical) },
        { "FancyBarContainer_GetRatio", new Func<object, float>(FancyBarContainer_GetRatio) },
        { "FancyBarContainer_SetRatio", new Action<object, float>(FancyBarContainer_SetRatio) },
        { "FancyBarContainer_GetOffset", new Func<object, float>(FancyBarContainer_GetOffset) },
        { "FancyBarContainer_SetOffset", new Action<object, float>(FancyBarContainer_SetOffset) },
        { "FancyBarContainer_GetBar", new Func<object, object>(FancyBarContainer_GetBar) },

        { "FancyProgressBar_New", new Func<float, float, bool, float, object>(FancyProgressBar_New) },
        { "FancyProgressBar_GetValue", new Func<object, float>(FancyProgressBar_GetValue) },
        { "FancyProgressBar_SetValue", new Action<object, float>(FancyProgressBar_SetValue) },
        { "FancyProgressBar_GetMaxValue", new Func<object, float>(FancyProgressBar_GetMaxValue) },
        { "FancyProgressBar_SetMaxValue", new Action<object, float>(FancyProgressBar_SetMaxValue) },
        { "FancyProgressBar_GetMinValue", new Func<object, float>(FancyProgressBar_GetMinValue) },
        { "FancyProgressBar_SetMinValue", new Action<object, float>(FancyProgressBar_SetMinValue) },
        { "FancyProgressBar_GetBarsGap", new Func<object, float>(FancyProgressBar_GetBarsGap) },
        { "FancyProgressBar_SetBarsGap", new Action<object, float>(FancyProgressBar_SetBarsGap) },
        { "FancyProgressBar_GetLabel", new Func<object, object>(FancyProgressBar_GetLabel) },

        { "FancySelector_New", new Func<List<string>, Action<int, string>, bool, object>(FancySelector_New) },
        { "FancySelector_GetLoop", new Func<object, bool>(FancySelector_GetLoop) },
        { "FancySelector_SetLoop", new Action<object, bool>(FancySelector_SetLoop) },
        { "FancySelector_GetSelected", new Func<object, int>(FancySelector_GetSelected) },
        { "FancySelector_SetSelected", new Action<object, int>(FancySelector_SetSelected) },
        { "FancySelector_SetOnChange", new Action<object, Action<int, string>>(FancySelector_SetOnChange) },

        { "FancySlider_New", new Func<float, float, Action<float>, object>(FancySlider_New) },
        { "FancySlider_GetMaxValue", new Func<object, float>(FancySlider_GetMaxValue) },
        { "FancySlider_SetMaxValue", new Action<object, float>(FancySlider_SetMaxValue) },
        { "FancySlider_GetMinValue", new Func<object, float>(FancySlider_GetMinValue) },
        { "FancySlider_SetMinValue", new Action<object, float>(FancySlider_SetMinValue) },
        { "FancySlider_GetValue", new Func<object, float>(FancySlider_GetValue) },
        { "FancySlider_SetValue", new Action<object, float>(FancySlider_SetValue) },
        { "FancySlider_SetOnChange", new Action<object, Action<float>>(FancySlider_SetOnChange) },
        { "FancySlider_GetIsInteger", new Func<object, bool>(FancySlider_GetIsInteger) },
        { "FancySlider_SetIsInteger", new Action<object, bool>(FancySlider_SetIsInteger) },
        { "FancySlider_GetAllowInput", new Func<object, bool>(FancySlider_GetAllowInput) },
        { "FancySlider_SetAllowInput", new Action<object, bool>(FancySlider_SetAllowInput) },
        { "FancySlider_GetBar", new Func<object, object>(FancySlider_GetBar) },
        { "FancySlider_GetThumb", new Func<object, object>(FancySlider_GetThumb) },
        { "FancySlider_GetTextInput", new Func<object, object>(FancySlider_GetTextInput) },

        { "FancySliderRange_NewR", new Func<float, float, Action<float, float>, object>(FancySliderRange_NewR) },
        { "FancySliderRange_GetValueLower", new Func<object, float>(FancySliderRange_GetValueLower) },
        { "FancySliderRange_SetValueLower", new Action<object, float>(FancySliderRange_SetValueLower) },
        { "FancySliderRange_SetOnChangeR", new Action<object, Action<float, float>>(FancySliderRange_SetOnChangeR) },
        { "FancySliderRange_GetThumbLower", new Func<object, object>(FancySliderRange_GetThumbLower) },

        { "FancySwitch_New", new Func<string[], int, Action<int>, object>(FancySwitch_New) },
        { "FancySwitch_GetIndex", new Func<object, int>(FancySwitch_GetIndex) },
        { "FancySwitch_SetIndex", new Action<object, int>(FancySwitch_SetIndex) },
        { "FancySwitch_GetButtons", new Func<object, FancyButton[]>(FancySwitch_GetButtons) },
        { "FancySwitch_SetOnChange", new Action<object, Action<int>>(FancySwitch_SetOnChange) },

        { "FancyTextField_New", new Func<string, Action<string, bool>, object>(FancyTextField_New) },
        { "FancyTextField_GetIsEditing", new Func<object, bool>(FancyTextField_GetIsEditing) },
        { "FancyTextField_GetText", new Func<object, string>(FancyTextField_GetText) },
        { "FancyTextField_SetText", new Action<object, string>(FancyTextField_SetText) },
        { "FancyTextField_SetOnChange", new Action<object, Action<string, bool>>(FancyTextField_SetOnChange) },
        { "FancyTextField_GetIsNumeric", new Func<object, bool>(FancyTextField_GetIsNumeric) },
        { "FancyTextField_SetIsNumeric", new Action<object, bool>(FancyTextField_SetIsNumeric) },
        { "FancyTextField_GetIsInteger", new Func<object, bool>(FancyTextField_GetIsInteger) },
        { "FancyTextField_SetIsInteger", new Action<object, bool>(FancyTextField_SetIsInteger) },
        { "FancyTextField_GetAllowNegative", new Func<object, bool>(FancyTextField_GetAllowNegative) },
        { "FancyTextField_SetAllowNegative", new Action<object, bool>(FancyTextField_SetAllowNegative) },
        { "FancyTextField_GetLabel", new Func<object, object>(FancyTextField_GetLabel) },

        { "FancyWindowBar_New", new Func<string, object>(FancyWindowBar_New) },
        { "FancyWindowBar_GetLabel", new Func<object, object>(FancyWindowBar_GetLabel) },

        { "FancyChart_New", new Func<int, object>(FancyChart_New) },
        { "FancyChart_GetDataSets", new Func<object, List<float[]>>(FancyChart_GetDataSets) },
        { "FancyChart_GetDataColors", new Func<object, List<Color>>(FancyChart_GetDataColors) },
        { "FancyChart_GetGridHorizontalLines", new Func<object, int>(FancyChart_GetGridHorizontalLines) },
        { "FancyChart_SetGridHorizontalLines", new Action<object, int>(FancyChart_SetGridHorizontalLines) },
        { "FancyChart_GetGridVerticalLines", new Func<object, int>(FancyChart_GetGridVerticalLines) },
        { "FancyChart_SetGridVerticalLines", new Action<object, int>(FancyChart_SetGridVerticalLines) },
        { "FancyChart_GetMaxValue", new Func<object, float>(FancyChart_GetMaxValue) },
        { "FancyChart_GetMinValue", new Func<object, float>(FancyChart_GetMinValue) },
        { "FancyChart_GetGridColor", new Func<object, Color?>(FancyChart_GetGridColor) },
        { "FancyChart_SetGridColor", new Action<object, Color>(FancyChart_SetGridColor) },

        { "FancyEmptyElement_New", new Func<object>(FancyEmptyElement_New) }
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
    private Vector2 TouchScreen_GetCursorPosition(object obj) => (obj as TouchScreen).CursorPosition;
    private float TouchScreen_GetInteractiveDistance(object obj) => (obj as TouchScreen).InteractiveDistance;
    private void TouchScreen_SetInteractiveDistance(object obj, float distance) => (obj as TouchScreen).InteractiveDistance = distance;
    private int TouchScreen_GetRotation(object obj) => (obj as TouchScreen).Rotation;
    private bool TouchScreen_CompareWithBlockAndSurface(object obj, IMyCubeBlock block, IMyTextSurface surface) => (obj as TouchScreen).CompareWithBlockAndSurface(block, surface);
    private void TouchScreen_ForceDispose(object obj) => (obj as TouchScreen).Dispose();

    private bool FancyElementBase_GetEnabled(object obj) => (obj as FancyElementBase).Enabled;
    private void FancyElementBase_SetEnabled(object obj, bool enabled) => (obj as FancyElementBase).Enabled = enabled;
    private bool FancyElementBase_GetAbsolute(object obj) => (obj as FancyElementBase).Absolute;
    private void FancyElementBase_SetAbsolute(object obj, bool absolute) => (obj as FancyElementBase).Absolute = absolute;
    private byte FancyElementBase_GetSelfAlignment(object obj) => (byte)(obj as FancyElementBase).SelfAlignment;
    private void FancyElementBase_SetSelfAlignment(object obj, byte alignment) => (obj as FancyElementBase).SelfAlignment = (ViewAlignment)alignment;
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
    private void FancyElementBase_ForceUpdate(object obj) => (obj as FancyElementBase).Update();
    private void FancyElementBase_ForceDispose(object obj) => (obj as FancyElementBase).Dispose();
    private void FancyElementBase_RegisterUpdate(object obj, Action update) => (obj as FancyElementBase).UpdateEvent += update;
    private void FancyElementBase_UnregisterUpdate(object obj, Action update) => (obj as FancyElementBase).UpdateEvent -= update;

    private List<object> FancyContainerBase_GetChildren(object obj) => (obj as FancyContainerBase).Children.Cast<object>().ToList();
    private Vector2 FancyContainerBase_GetFlexSize(object obj) => (obj as FancyContainerBase).GetFlexSize();
    private void FancyContainerBase_AddChild(object obj, object child) => (obj as FancyContainerBase).AddChild((FancyElementBase)child);
    private void FancyContainerBase_AddChildAt(object obj, object child, int index) => (obj as FancyContainerBase).AddChild((FancyElementBase)child, index);
    private void FancyContainerBase_RemoveChild(object obj, object child) => (obj as FancyContainerBase).RemoveChild((FancyElementBase)child);
    private void FancyContainerBase_RemoveChildAt(object obj, int index) => (obj as FancyContainerBase).RemoveChild(index);
    private void FancyContainerBase_MoveChild(object obj, object child, int index) => (obj as FancyContainerBase).MoveChild((FancyElementBase)child, index);

    private FancyView FancyView_New(int direction, Color? bgColor = null) => new FancyView((ViewDirection)direction, bgColor);
    private bool FancyView_GetOverflow(object obj) => (obj as FancyView).Overflow;
    private void FancyView_SetOverflow(object obj, bool overflow) => (obj as FancyView).Overflow = overflow;
    private int FancyView_GetDirection(object obj) => (int)(obj as FancyView).Direction;
    private void FancyView_SetDirection(object obj, int direction) => (obj as FancyView).Direction = (ViewDirection)direction;
    private byte FancyView_GetAlignment(object obj) => (byte)(obj as FancyView).Alignment;
    private void FancyView_SetAlignment(object obj, byte alignment) => (obj as FancyView).Alignment = (ViewAlignment)alignment;
    private byte FancyView_GetAnchor(object obj) => (byte)(obj as FancyView).Anchor;
    private void FancyView_SetAnchor(object obj, byte anchor) => (obj as FancyView).Anchor = (ViewAnchor)anchor;
    private bool FancyView_GetUseThemeColors(object obj) => (obj as FancyView).UseThemeColors;
    private void FancyView_SetUseThemeColors(object obj, bool useTheme) => (obj as FancyView).UseThemeColors = useTheme;
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

    private FancyScrollView FancyScrollView_New(int direction, Color? bgColor = null) => new FancyScrollView((ViewDirection)direction, bgColor);
    private float FancyScrollView_GetScroll(object obj) => (obj as FancyScrollView).Scroll;
    private void FancyScrollView_SetScroll(object obj, float scroll) => (obj as FancyScrollView).Scroll = scroll;
    private bool FancyScrollView_GetScrollAlwaysVisible(object obj) => (obj as FancyScrollView).ScrollAlwaysVisible;
    private void FancyScrollView_SetScrollAlwaysVisible(object obj, bool visible) => (obj as FancyScrollView).ScrollAlwaysVisible = visible;
    private FancyBarContainer FancyScrollView_GetScrollBar(object obj) => (obj as FancyScrollView).ScrollBar;

    private FancyApp FancyApp_New() => new FancyApp();
    private TouchScreen FancyApp_GetScreen(object obj) => (obj as FancyApp).Screen;
    private RectangleF FancyApp_GetViewport(object obj) => (obj as FancyApp).Viewport;
    private FancyCursor FancyApp_GetCursor(object obj) => (obj as FancyApp).Cursor;
    private FancyTheme FancyApp_GetTheme(object obj) => (obj as FancyApp).Theme;
    private bool FancyApp_GetDefaultBg(object obj) => (obj as FancyApp).DefaultBg;
    private void FancyApp_SetDefaultBg(object obj, bool defaultBg) => (obj as FancyApp).DefaultBg = defaultBg;
    private void FancyApp_InitApp(object obj, MyCubeBlock block, Sandbox.ModAPI.Ingame.IMyTextSurface surface) => (obj as FancyApp).InitApp(block, surface);

    private FancyCursor FancyCursor_New(object screen) => new FancyCursor(screen as TouchScreen);
    private bool FancyCursor_GetActive(object obj) => (obj as FancyCursor).Active;
    private void FancyCursor_SetActive(object obj, bool active) => (obj as FancyCursor).Active = active;
    private Vector2 FancyCursor_GetPosition(object obj) => (obj as FancyCursor).Position;
    private bool FancyCursor_IsInsideArea(object obj, float x, float y, float z, float w) => (obj as FancyCursor).IsInsideArea(x, y, z, w);
    private List<MySprite> FancyCursor_GetSprites(object obj) => (obj as FancyCursor).GetSprites();
    private void FancyCursor_ForceDispose(object obj) => (obj as FancyCursor).Dispose();

    private Color FancyTheme_GetBgColor(object obj) => (obj as FancyTheme).BgColor;
    private Color FancyTheme_GetWhiteColor(object obj) => (obj as FancyTheme).WhiteColor;
    private Color FancyTheme_GetMainColor(object obj) => (obj as FancyTheme).MainColor;
    private Color FancyTheme_GetMainColorDarker(object obj, int value)
    {
      var theme = (obj as FancyTheme);
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
    private Vector2 FancyTheme_MeasureStringInPixels(object obj, string text, string font, float scale) => (obj as FancyTheme).MeasureStringInPixels(text, font, scale);
    private float FancyTheme_GetScale(object obj) => (obj as FancyTheme).Scale;
    private void FancyTheme_SetScale(object obj, float scale) => (obj as FancyTheme).Scale = scale;
    private string FancyTheme_GetFont(object obj) => (obj as FancyTheme).Font;
    private void FancyTheme_SetFont(object obj, string font) => (obj as FancyTheme).Font = font;

    private ClickHandler ClickHandler_New() => new ClickHandler();
    private Vector4 ClickHandler_GetHitArea(object obj) => (obj as ClickHandler).HitArea;
    private void ClickHandler_SetHitArea(object obj, Vector4 hitArea) => (obj as ClickHandler).HitArea = hitArea;
    private bool ClickHandler_IsMouseReleased(object obj) => (obj as ClickHandler).IsMouseReleased;
    private bool ClickHandler_IsMouseOver(object obj) => (obj as ClickHandler).IsMouseOver;
    private bool ClickHandler_IsMousePressed(object obj) => (obj as ClickHandler).IsMousePressed;
    private bool ClickHandler_JustReleased(object obj) => (obj as ClickHandler).JustReleased;
    private bool ClickHandler_JustPressed(object obj) => (obj as ClickHandler).JustPressed;
    private void ClickHandler_UpdateStatus(object obj, object screen) => (obj as ClickHandler).UpdateStatus(screen as TouchScreen);

    private FancyEmptyButton FancyEmptyButton_New(Action onChange) => new FancyEmptyButton(onChange);
    private ClickHandler FancyEmptyButton_GetHandler(object obj) => (obj as FancyEmptyButton).Handler;
    private void FancyEmptyButton_SetOnChange(object obj, Action onChange) => (obj as FancyEmptyButton).OnChange = onChange;

    private FancyButton FancyButton_New(string text, Action onChange) => new FancyButton(text, onChange);
    private FancyLabel FancyButton_GetLabel(object obj) => (obj as FancyButton).Label;

    private FancyCheckbox FancyCheckbox_New(Action<bool> onChange, bool value = false) => new FancyCheckbox(onChange, value);
    private bool FancyCheckbox_GetValue(object obj) => (obj as FancyCheckbox).Value;
    private void FancyCheckbox_SetValue(object obj, bool value) => (obj as FancyCheckbox).Value = value;
    private void FancyCheckbox_SetOnChange(object obj, Action<bool> onChange) => (obj as FancyCheckbox).OnChange = onChange;
    private FancyEmptyElement FancyCheckbox_GetCheckMark(object obj) => (obj as FancyCheckbox).CheckMark;

    private FancyLabel FancyLabel_New(string text, float fontSize = 0.5f, TextAlignment alignment = TextAlignment.CENTER) => new FancyLabel(text, fontSize, alignment);
    private bool FancyLabel_GetAutoBreakLine(object obj) => (obj as FancyLabel).AutoBreakLine;
    private void FancyLabel_SetAutoBreakLine(object obj, bool breakLine) => (obj as FancyLabel).AutoBreakLine = breakLine;
    private bool FancyLabel_GetOverflow(object obj) => (obj as FancyLabel).Overflow;
    private void FancyLabel_SetOverflow(object obj, bool overflow) => (obj as FancyLabel).Overflow = overflow;
    private bool FancyLabel_GetIsShortened(object obj) => (obj as FancyLabel).IsShortened;
    private string FancyLabel_GetText(object obj) => (obj as FancyLabel).Text;
    private void FancyLabel_SetText(object obj, string text) => (obj as FancyLabel).Text = text;
    private Color? FancyLabel_GetTextColor(object obj) => (obj as FancyLabel).TextColor;
    private void FancyLabel_SetTextColor(object obj, Color color) => (obj as FancyLabel).TextColor = color;
    private float FancyLabel_GetFontSize(object obj) => (obj as FancyLabel).FontSize;
    private void FancyLabel_SetFontSize(object obj, float fontSize) => (obj as FancyLabel).FontSize = fontSize;
    private TextAlignment FancyLabel_GetAlignment(object obj) => (obj as FancyLabel).Alignment;
    private void FancyLabel_SetAlignment(object obj, TextAlignment alignment) => (obj as FancyLabel).Alignment = alignment;

    private FancyBarContainer FancyBarContainer_New(bool vertical = false) => new FancyBarContainer(vertical);
    private bool FancyBarContainer_GetIsVertical(object obj) => (obj as FancyBarContainer).IsVertical;
    private void FancyBarContainer_SetIsVertical(object obj, bool vertical) => (obj as FancyBarContainer).IsVertical = vertical;
    private float FancyBarContainer_GetRatio(object obj) => (obj as FancyBarContainer).Ratio;
    private void FancyBarContainer_SetRatio(object obj, float ratio) => (obj as FancyBarContainer).Ratio = ratio;
    private float FancyBarContainer_GetOffset(object obj) => (obj as FancyBarContainer).Offset;
    private void FancyBarContainer_SetOffset(object obj, float offset) => (obj as FancyBarContainer).Offset = offset;
    private FancyView FancyBarContainer_GetBar(object obj) => (obj as FancyBarContainer).Bar;

    private FancyProgressBar FancyProgressBar_New(float min, float max, bool vertical = false, float barsGap = 0) => new FancyProgressBar(min, max, vertical, barsGap);
    private float FancyProgressBar_GetValue(object obj) => (obj as FancyProgressBar).Value;
    private void FancyProgressBar_SetValue(object obj, float value) => (obj as FancyProgressBar).Value = value;
    private float FancyProgressBar_GetMaxValue(object obj) => (obj as FancyProgressBar).MaxValue;
    private void FancyProgressBar_SetMaxValue(object obj, float max) => (obj as FancyProgressBar).MaxValue = max;
    private float FancyProgressBar_GetMinValue(object obj) => (obj as FancyProgressBar).MinValue;
    private void FancyProgressBar_SetMinValue(object obj, float min) => (obj as FancyProgressBar).MinValue = min;
    private float FancyProgressBar_GetBarsGap(object obj) => (obj as FancyProgressBar).BarsGap;
    private void FancyProgressBar_SetBarsGap(object obj, float gap) => (obj as FancyProgressBar).BarsGap = gap;
    private FancyLabel FancyProgressBar_GetLabel(object obj) => (obj as FancyProgressBar).Label;

    private FancySelector FancySelector_New(List<string> labels, Action<int, string> onChange, bool loop = true) => new FancySelector(labels, onChange, loop);
    private bool FancySelector_GetLoop(object obj) => (obj as FancySelector).Loop;
    private void FancySelector_SetLoop(object obj, bool loop) => (obj as FancySelector).Loop = loop;
    private int FancySelector_GetSelected(object obj) => (obj as FancySelector).Selected;
    private void FancySelector_SetSelected(object obj, int selected) => (obj as FancySelector).Selected = selected;
    private void FancySelector_SetOnChange(object obj, Action<int, string> onChange) => (obj as FancySelector).OnChange = onChange;

    private FancySlider FancySlider_New(float min, float max, Action<float> onChange) => new FancySlider(min, max, onChange);
    private float FancySlider_GetMaxValue(object obj) => (obj as FancySlider).MaxValue;
    private void FancySlider_SetMaxValue(object obj, float max) => (obj as FancySlider).MaxValue = max;
    private float FancySlider_GetMinValue(object obj) => (obj as FancySlider).MinValue;
    private void FancySlider_SetMinValue(object obj, float min) => (obj as FancySlider).MinValue = min;
    private float FancySlider_GetValue(object obj) => (obj as FancySlider).Value;
    private void FancySlider_SetValue(object obj, float value) => (obj as FancySlider).Value = value;
    private void FancySlider_SetOnChange(object obj, Action<float> onChange) => (obj as FancySlider).OnChange = onChange;
    private bool FancySlider_GetIsInteger(object obj) => (obj as FancySlider).IsInteger;
    private void FancySlider_SetIsInteger(object obj, bool interger) => (obj as FancySlider).IsInteger = interger;
    private bool FancySlider_GetAllowInput(object obj) => (obj as FancySlider).AllowInput;
    private void FancySlider_SetAllowInput(object obj, bool allowInput) => (obj as FancySlider).AllowInput = allowInput;
    private FancyBarContainer FancySlider_GetBar(object obj) => (obj as FancySlider).Bar;
    private FancyEmptyElement FancySlider_GetThumb(object obj) => (obj as FancySlider).Thumb;
    private FancyTextField FancySlider_GetTextInput(object obj) => (obj as FancySlider).InnerTextField;

    private FancySliderRange FancySliderRange_NewR(float min, float max, Action<float, float> onChange) => new FancySliderRange(min, max, onChange);
    private float FancySliderRange_GetValueLower(object obj) => (obj as FancySliderRange).ValueLower;
    private void FancySliderRange_SetValueLower(object obj, float value) => (obj as FancySliderRange).ValueLower = value;
    private void FancySliderRange_SetOnChangeR(object obj, Action<float, float> onChange) => (obj as FancySliderRange).OnChangeR = onChange;
    private FancyEmptyElement FancySliderRange_GetThumbLower(object obj) => (obj as FancySliderRange).ThumbLower;

    private FancySwitch FancySwitch_New(string[] labels, int index = 0, Action<int> onChange = null) => new FancySwitch(labels, index, onChange);
    private int FancySwitch_GetIndex(object obj) => (obj as FancySwitch).Index;
    private void FancySwitch_SetIndex(object obj, int index) => (obj as FancySwitch).Index = index;
    private FancyButton[] FancySwitch_GetButtons(object obj) => (obj as FancySwitch).Buttons;
    private void FancySwitch_SetOnChange(object obj, Action<int> onChange) => (obj as FancySwitch).OnChange = onChange;

    private FancyTextField FancyTextField_New(string text, Action<string, bool> onChange) => new FancyTextField(text, onChange);
    private bool FancyTextField_GetIsEditing(object obj) => (obj as FancyTextField).IsEditing;
    private string FancyTextField_GetText(object obj) => (obj as FancyTextField).Text;
    private void FancyTextField_SetText(object obj, string text) => (obj as FancyTextField).Text = text;
    private void FancyTextField_SetOnChange(object obj, Action<string, bool> onChange) => (obj as FancyTextField).OnChange = onChange;
    private bool FancyTextField_GetIsNumeric(object obj) => (obj as FancyTextField).IsNumeric;
    private void FancyTextField_SetIsNumeric(object obj, bool isNumeric) => (obj as FancyTextField).IsNumeric = isNumeric;
    private bool FancyTextField_GetIsInteger(object obj) => (obj as FancyTextField).IsInteger;
    private void FancyTextField_SetIsInteger(object obj, bool isInterger) => (obj as FancyTextField).IsInteger = isInterger;
    private bool FancyTextField_GetAllowNegative(object obj) => (obj as FancyTextField).AllowNegative;
    private void FancyTextField_SetAllowNegative(object obj, bool allowNegative) => (obj as FancyTextField).AllowNegative = allowNegative;
    private FancyLabel FancyTextField_GetLabel(object obj) => (obj as FancyTextField).Label;

    private FancyWindowBar FancyWindowBar_New(string text) => new FancyWindowBar(text);
    private FancyLabel FancyWindowBar_GetLabel(object obj) => (obj as FancyWindowBar).Label;

    private FancyChart FancyChart_New(int intervals) => new FancyChart(intervals);
    private List<float[]> FancyChart_GetDataSets(object obj) => (obj as FancyChart).DataSets;
    private List<Color> FancyChart_GetDataColors(object obj) => (obj as FancyChart).DataColors;
    private int FancyChart_GetGridHorizontalLines(object obj) => (obj as FancyChart).GridHorizontalLines;
    private void FancyChart_SetGridHorizontalLines(object obj, int lines) => (obj as FancyChart).GridHorizontalLines = lines;
    private int FancyChart_GetGridVerticalLines(object obj) => (obj as FancyChart).GridVerticalLines;
    private void FancyChart_SetGridVerticalLines(object obj, int lines) => (obj as FancyChart).GridVerticalLines = lines;
    private float FancyChart_GetMaxValue(object obj) => (obj as FancyChart).MaxValue;
    private float FancyChart_GetMinValue(object obj) => (obj as FancyChart).MinValue;
    private Color? FancyChart_GetGridColor(object obj) => (obj as FancyChart).GridColor;
    private void FancyChart_SetGridColor(object obj, Color color) => (obj as FancyChart).GridColor = color;

    private FancyEmptyElement FancyEmptyElement_New() => new FancyEmptyElement();
  }
}