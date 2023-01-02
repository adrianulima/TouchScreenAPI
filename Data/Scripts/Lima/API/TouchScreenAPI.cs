using System;
using System.Collections.Generic;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Lima.API
{
  public class TouchScreenAPI
  {
    private const long _channel = 2668820525;
    private bool _apiInit;
    private bool _isRegistered;
    public bool IsReady { get; private set; }

    private Func<IMyCubeBlock, IMyTextSurface, object> _createTouchScreen;
    private Action<IMyCubeBlock, IMyTextSurface> _removeTouchScreen;
    private Func<float> _getMaxInteractiveDistance;
    private Action<float> _setMaxInteractiveDistance;
    private Action<string> _addSurfaceCoords;
    private Action<string> _removeSurfaceCoords;

    public object CreateTouchScreen(IMyCubeBlock block, IMyTextSurface surface) => _createTouchScreen?.Invoke(block, surface);
    public void RemoveTouchScreen(IMyCubeBlock block, IMyTextSurface surface) => _removeTouchScreen?.Invoke(block, surface);
    public void AddSurfaceCoords(string coords) => _addSurfaceCoords?.Invoke(coords);
    public void RemoveSurfaceCoords(string coords) => _removeSurfaceCoords?.Invoke(coords);
    public float GetMaxInteractiveDistance() => _getMaxInteractiveDistance?.Invoke() ?? -1f;
    public void SetMaxInteractiveDistance(float distance) => _setMaxInteractiveDistance?.Invoke(distance);

    public bool Load()
    {
      if (!_isRegistered)
      {
        _isRegistered = true;
        MyAPIGateway.Utilities.RegisterMessageHandler(_channel, HandleMessage);
      }
      if (!IsReady && !_apiInit)
        MyAPIGateway.Utilities.SendModMessage(_channel, "ApiEndpointRequest");

      WrapperBase.SetApi(this);
      return IsReady;
    }
    public void Unload()
    {
      if (_isRegistered)
        MyAPIGateway.Utilities.UnregisterMessageHandler(_channel, HandleMessage);
      ApiDelegates(null);
      _isRegistered = false;
      _apiInit = false;
      IsReady = false;
      WrapperBase.SetApi(null);
    }
    private void HandleMessage(object msg)
    {
      if (_apiInit || msg is string)
        return;
      try
      {
        var dict = msg as IReadOnlyDictionary<string, Delegate>;
        if (dict == null)
          return;
        ApiDelegates(dict);
        IsReady = true;
      }
      catch (Exception e)
      {
        MyLog.Default.WriteLineAndConsole("TouchScreenAPI Failed To Load For Client: " + MyAPIGateway.Utilities.GamePaths.ModScopeName);
        MyLog.Default.WriteLineAndConsole($"{e.Message}\n{e.StackTrace}");
      }
    }
    public void ApiDelegates(IReadOnlyDictionary<string, Delegate> delegates)
    {
      _apiInit = delegates != null;
      AssignMethod(delegates, "CreateTouchScreen", ref _createTouchScreen);
      AssignMethod(delegates, "RemoveTouchScreen", ref _removeTouchScreen);
      AssignMethod(delegates, "AddSurfaceCoords", ref _addSurfaceCoords);
      AssignMethod(delegates, "RemoveSurfaceCoords", ref _removeSurfaceCoords);
      AssignMethod(delegates, "GetMaxInteractiveDistance", ref _getMaxInteractiveDistance);
      AssignMethod(delegates, "SetMaxInteractiveDistance", ref _setMaxInteractiveDistance);
      AssignMethod(delegates, "TouchScreen_GetBlock", ref TouchScreen_GetBlock);
      AssignMethod(delegates, "TouchScreen_GetSurface", ref TouchScreen_GetSurface);
      AssignMethod(delegates, "TouchScreen_GetIndex", ref TouchScreen_GetIndex);
      AssignMethod(delegates, "TouchScreen_IsOnScreen", ref TouchScreen_IsOnScreen);
      AssignMethod(delegates, "TouchScreen_GetCursorPosition", ref TouchScreen_GetCursorPosition);
      AssignMethod(delegates, "TouchScreen_GetInteractiveDistance", ref TouchScreen_GetInteractiveDistance);
      AssignMethod(delegates, "TouchScreen_SetInteractiveDistance", ref TouchScreen_SetInteractiveDistance);
      AssignMethod(delegates, "TouchScreen_GetRotation", ref TouchScreen_GetRotation);
      AssignMethod(delegates, "TouchScreen_CompareWithBlockAndSurface", ref TouchScreen_CompareWithBlockAndSurface);
      AssignMethod(delegates, "TouchScreen_ForceDispose", ref TouchScreen_ForceDispose);
      AssignMethod(delegates, "ClickHandler_New", ref ClickHandler_New);
      AssignMethod(delegates, "ClickHandler_GetHitArea", ref ClickHandler_GetHitArea);
      AssignMethod(delegates, "ClickHandler_SetHitArea", ref ClickHandler_SetHitArea);
      AssignMethod(delegates, "ClickHandler_IsMouseReleased", ref ClickHandler_IsMouseReleased);
      AssignMethod(delegates, "ClickHandler_IsMouseOver", ref ClickHandler_IsMouseOver);
      AssignMethod(delegates, "ClickHandler_IsMousePressed", ref ClickHandler_IsMousePressed);
      AssignMethod(delegates, "ClickHandler_JustReleased", ref ClickHandler_JustReleased);
      AssignMethod(delegates, "ClickHandler_JustPressed", ref ClickHandler_JustPressed);
      AssignMethod(delegates, "ClickHandler_UpdateStatus", ref ClickHandler_UpdateStatus);
      AssignMethod(delegates, "FancyCursor_New", ref FancyCursor_New);
      AssignMethod(delegates, "FancyCursor_GetActive", ref FancyCursor_GetActive);
      AssignMethod(delegates, "FancyCursor_SetActive", ref FancyCursor_SetActive);
      AssignMethod(delegates, "FancyCursor_GetPosition", ref FancyCursor_GetPosition);
      AssignMethod(delegates, "FancyCursor_IsInsideArea", ref FancyCursor_IsInsideArea);
      AssignMethod(delegates, "FancyCursor_GetSprites", ref FancyCursor_GetSprites);
      AssignMethod(delegates, "FancyCursor_ForceDispose", ref FancyCursor_ForceDispose);
      AssignMethod(delegates, "FancyTheme_GetBgColor", ref FancyTheme_GetBgColor);
      AssignMethod(delegates, "FancyTheme_GetWhiteColor", ref FancyTheme_GetWhiteColor);
      AssignMethod(delegates, "FancyTheme_GetMainColor", ref FancyTheme_GetMainColor);
      AssignMethod(delegates, "FancyTheme_GetMainColorDarker", ref FancyTheme_GetMainColorDarker);
      AssignMethod(delegates, "FancyTheme_MeasureStringInPixels", ref FancyTheme_MeasureStringInPixels);
      AssignMethod(delegates, "FancyTheme_GetScale", ref FancyTheme_GetScale);
      AssignMethod(delegates, "FancyTheme_SetScale", ref FancyTheme_SetScale);
      AssignMethod(delegates, "FancyTheme_GetFont", ref FancyTheme_GetFont);
      AssignMethod(delegates, "FancyTheme_SetFont", ref FancyTheme_SetFont);
      AssignMethod(delegates, "FancyElementBase_GetEnabled", ref FancyElementBase_GetEnabled);
      AssignMethod(delegates, "FancyElementBase_SetEnabled", ref FancyElementBase_SetEnabled);
      AssignMethod(delegates, "FancyElementBase_GetAbsolute", ref FancyElementBase_GetAbsolute);
      AssignMethod(delegates, "FancyElementBase_SetAbsolute", ref FancyElementBase_SetAbsolute);
      AssignMethod(delegates, "FancyElementBase_GetSelfAlignment", ref FancyElementBase_GetSelfAlignment);
      AssignMethod(delegates, "FancyElementBase_SetSelfAlignment", ref FancyElementBase_SetSelfAlignment);
      AssignMethod(delegates, "FancyElementBase_GetPosition", ref FancyElementBase_GetPosition);
      AssignMethod(delegates, "FancyElementBase_SetPosition", ref FancyElementBase_SetPosition);
      AssignMethod(delegates, "FancyElementBase_GetMargin", ref FancyElementBase_GetMargin);
      AssignMethod(delegates, "FancyElementBase_SetMargin", ref FancyElementBase_SetMargin);
      AssignMethod(delegates, "FancyElementBase_GetScale", ref FancyElementBase_GetScale);
      AssignMethod(delegates, "FancyElementBase_SetScale", ref FancyElementBase_SetScale);
      AssignMethod(delegates, "FancyElementBase_GetPixels", ref FancyElementBase_GetPixels);
      AssignMethod(delegates, "FancyElementBase_SetPixels", ref FancyElementBase_SetPixels);
      AssignMethod(delegates, "FancyElementBase_GetSize", ref FancyElementBase_GetSize);
      AssignMethod(delegates, "FancyElementBase_GetBoundaries", ref FancyElementBase_GetBoundaries);
      AssignMethod(delegates, "FancyElementBase_GetApp", ref FancyElementBase_GetApp);
      AssignMethod(delegates, "FancyElementBase_GetParent", ref FancyElementBase_GetParent);
      AssignMethod(delegates, "FancyElementBase_GetSprites", ref FancyElementBase_GetSprites);
      AssignMethod(delegates, "FancyElementBase_ForceUpdate", ref FancyElementBase_ForceUpdate);
      AssignMethod(delegates, "FancyElementBase_ForceDispose", ref FancyElementBase_ForceDispose);
      AssignMethod(delegates, "FancyElementBase_RegisterUpdate", ref FancyElementBase_RegisterUpdate);
      AssignMethod(delegates, "FancyElementBase_UnregisterUpdate", ref FancyElementBase_UnregisterUpdate);
      AssignMethod(delegates, "FancyContainerBase_GetChildren", ref FancyContainerBase_GetChildren);
      AssignMethod(delegates, "FancyContainerBase_GetFlexSize", ref FancyContainerBase_GetFlexSize);
      AssignMethod(delegates, "FancyContainerBase_AddChild", ref FancyContainerBase_AddChild);
      AssignMethod(delegates, "FancyContainerBase_AddChildAt", ref FancyContainerBase_AddChildAt);
      AssignMethod(delegates, "FancyContainerBase_RemoveChild", ref FancyContainerBase_RemoveChild);
      AssignMethod(delegates, "FancyContainerBase_RemoveChildAt", ref FancyContainerBase_RemoveChildAt);
      AssignMethod(delegates, "FancyContainerBase_MoveChild", ref FancyContainerBase_MoveChild);
      AssignMethod(delegates, "FancyView_New", ref FancyView_New);
      AssignMethod(delegates, "FancyView_GetOverflow", ref FancyView_GetOverflow);
      AssignMethod(delegates, "FancyView_SetOverflow", ref FancyView_SetOverflow);
      AssignMethod(delegates, "FancyView_GetDirection", ref FancyView_GetDirection);
      AssignMethod(delegates, "FancyView_SetDirection", ref FancyView_SetDirection);
      AssignMethod(delegates, "FancyView_GetAlignment", ref FancyView_GetAlignment);
      AssignMethod(delegates, "FancyView_SetAlignment", ref FancyView_SetAlignment);
      AssignMethod(delegates, "FancyView_GetAnchor", ref FancyView_GetAnchor);
      AssignMethod(delegates, "FancyView_SetAnchor", ref FancyView_SetAnchor);
      AssignMethod(delegates, "FancyView_GetUseThemeColors", ref FancyView_GetUseThemeColors);
      AssignMethod(delegates, "FancyView_SetUseThemeColors", ref FancyView_SetUseThemeColors);
      AssignMethod(delegates, "FancyView_GetBgColor", ref FancyView_GetBgColor);
      AssignMethod(delegates, "FancyView_SetBgColor", ref FancyView_SetBgColor);
      AssignMethod(delegates, "FancyView_GetBorderColor", ref FancyView_GetBorderColor);
      AssignMethod(delegates, "FancyView_SetBorderColor", ref FancyView_SetBorderColor);
      AssignMethod(delegates, "FancyView_GetBorder", ref FancyView_GetBorder);
      AssignMethod(delegates, "FancyView_SetBorder", ref FancyView_SetBorder);
      AssignMethod(delegates, "FancyView_GetPadding", ref FancyView_GetPadding);
      AssignMethod(delegates, "FancyView_SetPadding", ref FancyView_SetPadding);
      AssignMethod(delegates, "FancyView_GetGap", ref FancyView_GetGap);
      AssignMethod(delegates, "FancyView_SetGap", ref FancyView_SetGap);
      AssignMethod(delegates, "FancyScrollView_New", ref FancyScrollView_New);
      AssignMethod(delegates, "FancyScrollView_GetScroll", ref FancyScrollView_GetScroll);
      AssignMethod(delegates, "FancyScrollView_SetScroll", ref FancyScrollView_SetScroll);
      AssignMethod(delegates, "FancyScrollView_GetScrollAlwaysVisible", ref FancyScrollView_GetScrollAlwaysVisible);
      AssignMethod(delegates, "FancyScrollView_SetScrollAlwaysVisible", ref FancyScrollView_SetScrollAlwaysVisible);
      AssignMethod(delegates, "FancyScrollView_GetScrollBar", ref FancyScrollView_GetScrollBar);
      AssignMethod(delegates, "FancyApp_New", ref FancyApp_New);
      AssignMethod(delegates, "FancyApp_GetScreen", ref FancyApp_GetScreen);
      AssignMethod(delegates, "FancyApp_GetViewport", ref FancyApp_GetViewport);
      AssignMethod(delegates, "FancyApp_GetCursor", ref FancyApp_GetCursor);
      AssignMethod(delegates, "FancyApp_GetTheme", ref FancyApp_GetTheme);
      AssignMethod(delegates, "FancyApp_GetDefaultBg", ref FancyApp_GetDefaultBg);
      AssignMethod(delegates, "FancyApp_SetDefaultBg", ref FancyApp_SetDefaultBg);
      AssignMethod(delegates, "FancyApp_InitApp", ref FancyApp_InitApp);
      AssignMethod(delegates, "FancyEmptyButton_New", ref FancyEmptyButton_New);
      AssignMethod(delegates, "FancyEmptyButton_GetHandler", ref FancyEmptyButton_GetHandler);
      AssignMethod(delegates, "FancyEmptyButton_SetOnChange", ref FancyEmptyButton_SetOnChange);
      AssignMethod(delegates, "FancyButton_New", ref FancyButton_New);
      AssignMethod(delegates, "FancyButton_GetLabel", ref FancyButton_GetLabel);
      AssignMethod(delegates, "FancyCheckbox_New", ref FancyCheckbox_New);
      AssignMethod(delegates, "FancyCheckbox_GetValue", ref FancyCheckbox_GetValue);
      AssignMethod(delegates, "FancyCheckbox_SetValue", ref FancyCheckbox_SetValue);
      AssignMethod(delegates, "FancyCheckbox_SetOnChange", ref FancyCheckbox_SetOnChange);
      AssignMethod(delegates, "FancyCheckbox_GetCheckMark", ref FancyCheckbox_GetCheckMark);
      AssignMethod(delegates, "FancyLabel_New", ref FancyLabel_New);
      AssignMethod(delegates, "FancyLabel_GetOverflow", ref FancyLabel_GetOverflow);
      AssignMethod(delegates, "FancyLabel_SetOverflow", ref FancyLabel_SetOverflow);
      AssignMethod(delegates, "FancyLabel_GetIsShortened", ref FancyLabel_GetIsShortened);
      AssignMethod(delegates, "FancyLabel_GetText", ref FancyLabel_GetText);
      AssignMethod(delegates, "FancyLabel_SetText", ref FancyLabel_SetText);
      AssignMethod(delegates, "FancyLabel_GetTextColor", ref FancyLabel_GetTextColor);
      AssignMethod(delegates, "FancyLabel_SetTextColor", ref FancyLabel_SetTextColor);
      AssignMethod(delegates, "FancyLabel_GetFontSize", ref FancyLabel_GetFontSize);
      AssignMethod(delegates, "FancyLabel_SetFontSize", ref FancyLabel_SetFontSize);
      AssignMethod(delegates, "FancyLabel_GetAlignment", ref FancyLabel_GetAlignment);
      AssignMethod(delegates, "FancyLabel_SetAlignment", ref FancyLabel_SetAlignment);
      AssignMethod(delegates, "FancyBarContainer_New", ref FancyBarContainer_New);
      AssignMethod(delegates, "FancyBarContainer_GetIsVertical", ref FancyBarContainer_GetIsVertical);
      AssignMethod(delegates, "FancyBarContainer_SetIsVertical", ref FancyBarContainer_SetIsVertical);
      AssignMethod(delegates, "FancyBarContainer_GetRatio", ref FancyBarContainer_GetRatio);
      AssignMethod(delegates, "FancyBarContainer_SetRatio", ref FancyBarContainer_SetRatio);
      AssignMethod(delegates, "FancyBarContainer_GetOffset", ref FancyBarContainer_GetOffset);
      AssignMethod(delegates, "FancyBarContainer_SetOffset", ref FancyBarContainer_SetOffset);
      AssignMethod(delegates, "FancyBarContainer_GetBar", ref FancyBarContainer_GetBar);
      AssignMethod(delegates, "FancyProgressBar_New", ref FancyProgressBar_New);
      AssignMethod(delegates, "FancyProgressBar_GetValue", ref FancyProgressBar_GetValue);
      AssignMethod(delegates, "FancyProgressBar_SetValue", ref FancyProgressBar_SetValue);
      AssignMethod(delegates, "FancyProgressBar_GetMaxValue", ref FancyProgressBar_GetMaxValue);
      AssignMethod(delegates, "FancyProgressBar_SetMaxValue", ref FancyProgressBar_SetMaxValue);
      AssignMethod(delegates, "FancyProgressBar_GetMinValue", ref FancyProgressBar_GetMinValue);
      AssignMethod(delegates, "FancyProgressBar_SetMinValue", ref FancyProgressBar_SetMinValue);
      AssignMethod(delegates, "FancyProgressBar_GetBarsGap", ref FancyProgressBar_GetBarsGap);
      AssignMethod(delegates, "FancyProgressBar_SetBarsGap", ref FancyProgressBar_SetBarsGap);
      AssignMethod(delegates, "FancyProgressBar_GetLabel", ref FancyProgressBar_GetLabel);
      AssignMethod(delegates, "FancySelector_New", ref FancySelector_New);
      AssignMethod(delegates, "FancySelector_GetLoop", ref FancySelector_GetLoop);
      AssignMethod(delegates, "FancySelector_SetLoop", ref FancySelector_SetLoop);
      AssignMethod(delegates, "FancySelector_GetSelected", ref FancySelector_GetSelected);
      AssignMethod(delegates, "FancySelector_SetSelected", ref FancySelector_SetSelected);
      AssignMethod(delegates, "FancySelector_SetOnChange", ref FancySelector_SetOnChange);
      AssignMethod(delegates, "FancySlider_New", ref FancySlider_New);
      AssignMethod(delegates, "FancySlider_GetMaxValue", ref FancySlider_GetMaxValue);
      AssignMethod(delegates, "FancySlider_SetMaxValue", ref FancySlider_SetMaxValue);
      AssignMethod(delegates, "FancySlider_GetValue", ref FancySlider_GetValue);
      AssignMethod(delegates, "FancySlider_SetValue", ref FancySlider_SetValue);
      AssignMethod(delegates, "FancySlider_SetOnChange", ref FancySlider_SetOnChange);
      AssignMethod(delegates, "FancySlider_GetIsInteger", ref FancySlider_GetIsInteger);
      AssignMethod(delegates, "FancySlider_SetIsInteger", ref FancySlider_SetIsInteger);
      AssignMethod(delegates, "FancySlider_GetAllowInput", ref FancySlider_GetAllowInput);
      AssignMethod(delegates, "FancySlider_SetAllowInput", ref FancySlider_SetAllowInput);
      AssignMethod(delegates, "FancySlider_GetBar", ref FancySlider_GetBar);
      AssignMethod(delegates, "FancySlider_GetThumb", ref FancySlider_GetThumb);
      AssignMethod(delegates, "FancySlider_GetTextInput", ref FancySlider_GetTextInput);
      AssignMethod(delegates, "FancySliderRange_NewR", ref FancySliderRange_NewR);
      AssignMethod(delegates, "FancySliderRange_GetValueLower", ref FancySliderRange_GetValueLower);
      AssignMethod(delegates, "FancySliderRange_SetValueLower", ref FancySliderRange_SetValueLower);
      AssignMethod(delegates, "FancySliderRange_SetOnChangeR", ref FancySliderRange_SetOnChangeR);
      AssignMethod(delegates, "FancySliderRange_GetThumbLower", ref FancySliderRange_GetThumbLower);
      AssignMethod(delegates, "FancySwitch_New", ref FancySwitch_New);
      AssignMethod(delegates, "FancySwitch_GetIndex", ref FancySwitch_GetIndex);
      AssignMethod(delegates, "FancySwitch_SetIndex", ref FancySwitch_SetIndex);
      AssignMethod(delegates, "FancySwitch_GetButtons", ref FancySwitch_GetButtons);
      AssignMethod(delegates, "FancySwitch_SetOnChange", ref FancySwitch_SetOnChange);
      AssignMethod(delegates, "FancyTextField_New", ref FancyTextField_New);
      AssignMethod(delegates, "FancyTextField_GetIsEditing", ref FancyTextField_GetIsEditing);
      AssignMethod(delegates, "FancyTextField_GetText", ref FancyTextField_GetText);
      AssignMethod(delegates, "FancyTextField_SetText", ref FancyTextField_SetText);
      AssignMethod(delegates, "FancyTextField_SetOnChange", ref FancyTextField_SetOnChange);
      AssignMethod(delegates, "FancyTextField_GetIsNumeric", ref FancyTextField_GetIsNumeric);
      AssignMethod(delegates, "FancyTextField_SetIsNumeric", ref FancyTextField_SetIsNumeric);
      AssignMethod(delegates, "FancyTextField_GetIsInteger", ref FancyTextField_GetIsInteger);
      AssignMethod(delegates, "FancyTextField_SetIsInteger", ref FancyTextField_SetIsInteger);
      AssignMethod(delegates, "FancyTextField_GetAllowNegative", ref FancyTextField_GetAllowNegative);
      AssignMethod(delegates, "FancyTextField_SetAllowNegative", ref FancyTextField_SetAllowNegative);
      AssignMethod(delegates, "FancyTextField_GetLabel", ref FancyTextField_GetLabel);
      AssignMethod(delegates, "FancyWindowBar_New", ref FancyWindowBar_New);
      AssignMethod(delegates, "FancyWindowBar_GetLabel", ref FancyWindowBar_GetLabel);
      AssignMethod(delegates, "FancyChart_New", ref FancyChart_New);
      AssignMethod(delegates, "FancyChart_GetDataSets", ref FancyChart_GetDataSets);
      AssignMethod(delegates, "FancyChart_GetDataColors", ref FancyChart_GetDataColors);
      AssignMethod(delegates, "FancyChart_GetGridHorizontalLines", ref FancyChart_GetGridHorizontalLines);
      AssignMethod(delegates, "FancyChart_SetGridHorizontalLines", ref FancyChart_SetGridHorizontalLines);
      AssignMethod(delegates, "FancyChart_GetGridVerticalLines", ref FancyChart_GetGridVerticalLines);
      AssignMethod(delegates, "FancyChart_SetGridVerticalLines", ref FancyChart_SetGridVerticalLines);
      AssignMethod(delegates, "FancyChart_GetMaxValue", ref FancyChart_GetMaxValue);
      AssignMethod(delegates, "FancyChart_GetMinValue", ref FancyChart_GetMinValue);
      AssignMethod(delegates, "FancyChart_GetGridColor", ref FancyChart_GetGridColor);
      AssignMethod(delegates, "FancyChart_SetGridColor", ref FancyChart_SetGridColor);
      AssignMethod(delegates, "FancyEmptyElement_New", ref FancyEmptyElement_New);
    }
    private void AssignMethod<T>(IReadOnlyDictionary<string, Delegate> delegates, string name, ref T field) where T : class
    {
      if (delegates == null)
      {
        field = null;
        return;
      }
      Delegate del;
      if (!delegates.TryGetValue(name, out del))
        throw new Exception($"{GetType().Name} :: Couldn't find {name} delegate of type {typeof(T)}");
      field = del as T;
      if (field == null)
        throw new Exception($"{GetType().Name} :: Delegate {name} is not type {typeof(T)}, instead it's: {del.GetType()}");
    }

    public Func<object, IMyCubeBlock> TouchScreen_GetBlock;
    public Func<object, IMyTextSurface> TouchScreen_GetSurface;
    public Func<object, int> TouchScreen_GetIndex;
    public Func<object, bool> TouchScreen_IsOnScreen;
    public Func<object, Vector2> TouchScreen_GetCursorPosition;
    public Func<object, float> TouchScreen_GetInteractiveDistance;
    public Action<object, float> TouchScreen_SetInteractiveDistance;
    public Func<object, int> TouchScreen_GetRotation;
    public Func<object, IMyCubeBlock, IMyTextSurface, bool> TouchScreen_CompareWithBlockAndSurface;
    public Action<object> TouchScreen_ForceDispose;

    public Func<object> ClickHandler_New;
    public Func<object, Vector4> ClickHandler_GetHitArea;
    public Action<object, Vector4> ClickHandler_SetHitArea;
    public Func<object, bool> ClickHandler_IsMouseReleased;
    public Func<object, bool> ClickHandler_IsMouseOver;
    public Func<object, bool> ClickHandler_IsMousePressed;
    public Func<object, bool> ClickHandler_JustReleased;
    public Func<object, bool> ClickHandler_JustPressed;
    public Action<object, object> ClickHandler_UpdateStatus;

    public Func<object, object> FancyCursor_New;
    public Func<object, bool> FancyCursor_GetActive;
    public Action<object, bool> FancyCursor_SetActive;
    public Func<object, Vector2> FancyCursor_GetPosition;
    public Func<object, float, float, float, float, bool> FancyCursor_IsInsideArea;
    public Func<object, List<MySprite>> FancyCursor_GetSprites;
    public Action<object> FancyCursor_ForceDispose;

    public Func<object, Color> FancyTheme_GetBgColor;
    public Func<object, Color> FancyTheme_GetWhiteColor;
    public Func<object, Color> FancyTheme_GetMainColor;
    public Func<object, int, Color> FancyTheme_GetMainColorDarker;
    public Func<object, string, string, float, Vector2> FancyTheme_MeasureStringInPixels;
    public Func<object, float> FancyTheme_GetScale;
    public Action<object, float> FancyTheme_SetScale;
    public Func<object, string> FancyTheme_GetFont;
    public Action<object, string> FancyTheme_SetFont;

    public Func<object, bool> FancyElementBase_GetEnabled;
    public Action<object, bool> FancyElementBase_SetEnabled;
    public Func<object, bool> FancyElementBase_GetAbsolute;
    public Action<object, bool> FancyElementBase_SetAbsolute;
    public Func<object, byte> FancyElementBase_GetSelfAlignment;
    public Action<object, byte> FancyElementBase_SetSelfAlignment;
    public Func<object, Vector2> FancyElementBase_GetPosition;
    public Action<object, Vector2> FancyElementBase_SetPosition;
    public Func<object, Vector4> FancyElementBase_GetMargin;
    public Action<object, Vector4> FancyElementBase_SetMargin;
    public Func<object, Vector2> FancyElementBase_GetScale;
    public Action<object, Vector2> FancyElementBase_SetScale;
    public Func<object, Vector2> FancyElementBase_GetPixels;
    public Action<object, Vector2> FancyElementBase_SetPixels;
    public Func<object, Vector2> FancyElementBase_GetSize;
    public Func<object, Vector2> FancyElementBase_GetBoundaries;
    public Func<object, object> FancyElementBase_GetApp;
    public Func<object, object> FancyElementBase_GetParent;
    public Func<object, List<MySprite>> FancyElementBase_GetSprites;
    public Action<object> FancyElementBase_ForceUpdate;
    public Action<object> FancyElementBase_ForceDispose;
    public Action<object, Action> FancyElementBase_RegisterUpdate;
    public Action<object, Action> FancyElementBase_UnregisterUpdate;

    public Func<object, List<object>> FancyContainerBase_GetChildren;
    public Func<object, Vector2> FancyContainerBase_GetFlexSize;
    public Action<object, object> FancyContainerBase_AddChild;
    public Action<object, object, int> FancyContainerBase_AddChildAt;
    public Action<object, object> FancyContainerBase_RemoveChild;
    public Action<object, int> FancyContainerBase_RemoveChildAt;
    public Action<object, object, int> FancyContainerBase_MoveChild;

    public Func<int, Color?, object> FancyView_New;
    public Func<object, bool> FancyView_GetOverflow;
    public Action<object, bool> FancyView_SetOverflow;
    public Func<object, int> FancyView_GetDirection;
    public Action<object, int> FancyView_SetDirection;
    public Func<object, byte> FancyView_GetAlignment;
    public Action<object, byte> FancyView_SetAlignment;
    public Func<object, byte> FancyView_GetAnchor;
    public Action<object, byte> FancyView_SetAnchor;
    public Func<object, bool> FancyView_GetUseThemeColors;
    public Action<object, bool> FancyView_SetUseThemeColors;
    public Func<object, Color> FancyView_GetBgColor;
    public Action<object, Color> FancyView_SetBgColor;
    public Func<object, Color> FancyView_GetBorderColor;
    public Action<object, Color> FancyView_SetBorderColor;
    public Func<object, Vector4> FancyView_GetBorder;
    public Action<object, Vector4> FancyView_SetBorder;
    public Func<object, Vector4> FancyView_GetPadding;
    public Action<object, Vector4> FancyView_SetPadding;
    public Func<object, int> FancyView_GetGap;
    public Action<object, int> FancyView_SetGap;

    public Func<int, Color?, object> FancyScrollView_New;
    public Func<object, float> FancyScrollView_GetScroll;
    public Action<object, float> FancyScrollView_SetScroll;
    public Func<object, bool> FancyScrollView_GetScrollAlwaysVisible;
    public Action<object, bool> FancyScrollView_SetScrollAlwaysVisible;
    public Func<object, object> FancyScrollView_GetScrollBar;

    public Func<object> FancyApp_New;
    public Func<object, object> FancyApp_GetScreen;
    public Func<object, RectangleF> FancyApp_GetViewport;
    public Func<object, object> FancyApp_GetCursor;
    public Func<object, object> FancyApp_GetTheme;
    public Func<object, bool> FancyApp_GetDefaultBg;
    public Action<object, bool> FancyApp_SetDefaultBg;
    public Action<object, MyCubeBlock, Sandbox.ModAPI.Ingame.IMyTextSurface> FancyApp_InitApp;

    public Func<Action, object> FancyEmptyButton_New;
    public Func<object, object> FancyEmptyButton_GetHandler;
    public Action<object, Action> FancyEmptyButton_SetOnChange;

    public Func<string, Action, object> FancyButton_New;
    public Func<object, object> FancyButton_GetLabel;

    public Func<Action<bool>, bool, object> FancyCheckbox_New;
    public Func<object, bool> FancyCheckbox_GetValue;
    public Action<object, bool> FancyCheckbox_SetValue;
    public Action<object, Action<bool>> FancyCheckbox_SetOnChange;
    public Func<object, object> FancyCheckbox_GetCheckMark;

    public Func<string, float, TextAlignment, object> FancyLabel_New;
    public Func<object, bool> FancyLabel_GetOverflow;
    public Action<object, bool> FancyLabel_SetOverflow;
    public Func<object, bool> FancyLabel_GetIsShortened;
    public Func<object, string> FancyLabel_GetText;
    public Action<object, string> FancyLabel_SetText;
    public Func<object, Color?> FancyLabel_GetTextColor;
    public Action<object, Color> FancyLabel_SetTextColor;
    public Func<object, float> FancyLabel_GetFontSize;
    public Action<object, float> FancyLabel_SetFontSize;
    public Func<object, TextAlignment> FancyLabel_GetAlignment;
    public Action<object, TextAlignment> FancyLabel_SetAlignment;

    public Func<bool, object> FancyBarContainer_New;
    public Func<object, bool> FancyBarContainer_GetIsVertical;
    public Action<object, bool> FancyBarContainer_SetIsVertical;
    public Func<object, float> FancyBarContainer_GetRatio;
    public Action<object, float> FancyBarContainer_SetRatio;
    public Func<object, float> FancyBarContainer_GetOffset;
    public Action<object, float> FancyBarContainer_SetOffset;
    public Func<object, object> FancyBarContainer_GetBar;

    public Func<float, float, bool, float, object> FancyProgressBar_New;
    public Func<object, float> FancyProgressBar_GetValue;
    public Action<object, float> FancyProgressBar_SetValue;
    public Func<object, float> FancyProgressBar_GetMaxValue;
    public Action<object, float> FancyProgressBar_SetMaxValue;
    public Func<object, float> FancyProgressBar_GetMinValue;
    public Action<object, float> FancyProgressBar_SetMinValue;
    public Func<object, float> FancyProgressBar_GetBarsGap;
    public Action<object, float> FancyProgressBar_SetBarsGap;
    public Func<object, object> FancyProgressBar_GetLabel;

    public Func<List<string>, Action<int, string>, bool, object> FancySelector_New;
    public Func<object, bool> FancySelector_GetLoop;
    public Action<object, bool> FancySelector_SetLoop;
    public Func<object, int> FancySelector_GetSelected;
    public Action<object, int> FancySelector_SetSelected;
    public Action<object, Action<int, string>> FancySelector_SetOnChange;

    public Func<float, float, Action<float>, object> FancySlider_New;
    public Func<object, float> FancySlider_GetMaxValue;
    public Action<object, float> FancySlider_SetMaxValue;
    public Func<object, float> FancySlider_GetMinValue;
    public Action<object, float> FancySlider_SetMinValue;
    public Func<object, float> FancySlider_GetValue;
    public Action<object, float> FancySlider_SetValue;
    public Action<object, Action<float>> FancySlider_SetOnChange;
    public Func<object, bool> FancySlider_GetIsInteger;
    public Action<object, bool> FancySlider_SetIsInteger;
    public Func<object, bool> FancySlider_GetAllowInput;
    public Action<object, bool> FancySlider_SetAllowInput;
    public Func<object, object> FancySlider_GetBar;
    public Func<object, object> FancySlider_GetThumb;
    public Func<object, object> FancySlider_GetTextInput;

    public Func<float, float, Action<float, float>, object> FancySliderRange_NewR;
    public Func<object, float> FancySliderRange_GetValueLower;
    public Action<object, float> FancySliderRange_SetValueLower;
    public Action<object, Action<float, float>> FancySliderRange_SetOnChangeR;
    public Func<object, object> FancySliderRange_GetThumbLower;

    public Func<string[], int, Action<int>, object> FancySwitch_New;
    public Func<object, int> FancySwitch_GetIndex;
    public Action<object, int> FancySwitch_SetIndex;
    public Func<object, object[]> FancySwitch_GetButtons;
    public Action<object, Action<int>> FancySwitch_SetOnChange;

    public Func<string, Action<string, bool>, object> FancyTextField_New;
    public Func<object, bool> FancyTextField_GetIsEditing;
    public Func<object, string> FancyTextField_GetText;
    public Action<object, string> FancyTextField_SetText;
    public Action<object, Action<string, bool>> FancyTextField_SetOnChange;
    public Func<object, bool> FancyTextField_GetIsNumeric;
    public Action<object, bool> FancyTextField_SetIsNumeric;
    public Func<object, bool> FancyTextField_GetIsInteger;
    public Action<object, bool> FancyTextField_SetIsInteger;
    public Func<object, bool> FancyTextField_GetAllowNegative;
    public Action<object, bool> FancyTextField_SetAllowNegative;
    public Func<object, object> FancyTextField_GetLabel;

    public Func<string, object> FancyWindowBar_New;
    public Func<object, object> FancyWindowBar_GetLabel;

    public Func<int, object> FancyChart_New;
    public Func<object, List<float[]>> FancyChart_GetDataSets;
    public Func<object, List<Color>> FancyChart_GetDataColors;
    public Func<object, int> FancyChart_GetGridHorizontalLines;
    public Action<object, int> FancyChart_SetGridHorizontalLines;
    public Func<object, int> FancyChart_GetGridVerticalLines;
    public Action<object, int> FancyChart_SetGridVerticalLines;
    public Func<object, float> FancyChart_GetMaxValue;
    public Func<object, float> FancyChart_GetMinValue;
    public Func<object, Color?> FancyChart_GetGridColor;
    public Action<object, Color> FancyChart_SetGridColor;

    public Func<object> FancyEmptyElement_New;
  }

  public abstract class WrapperBase
  {
    static protected TouchScreenAPI Api;
    internal static void SetApi(TouchScreenAPI api) => Api = api;

    static protected T Wrap<T>(object obj, Func<object, T> ctor) where T : WrapperBase
    {
      return (obj == null) ? null : ctor(obj);
    }

    static protected T[] WrapArray<T>(object[] objArray, Func<object, T> ctor) where T : WrapperBase
    {
      var newArray = new T[objArray.Length];
      for (int i = 0; i < objArray.Length; i++)
        newArray[i] = Wrap<T>(objArray[i], ctor);

      return newArray;
    }

    internal object InternalObj { get; private set; }
    public WrapperBase(object internalObject) { InternalObj = internalObject; }
  }
  public class TouchScreen : WrapperBase
  {
    public TouchScreen(object internalObject) : base(internalObject) { }
    public IMyCubeBlock Block { get { return Api.TouchScreen_GetBlock.Invoke(InternalObj); } }
    public IMyTextSurface Surface { get { return Api.TouchScreen_GetSurface.Invoke(InternalObj); } }
    public int Index { get { return Api.TouchScreen_GetIndex.Invoke(InternalObj); } }
    public bool IsOnScreen { get { return Api.TouchScreen_IsOnScreen.Invoke(InternalObj); } }
    public Vector2 CursorPosition { get { return Api.TouchScreen_GetCursorPosition.Invoke(InternalObj); } }
    public float InteractiveDistance { get { return Api.TouchScreen_GetInteractiveDistance.Invoke(InternalObj); } }
    public void SetInteractiveDistance(float distance) => Api.TouchScreen_SetInteractiveDistance.Invoke(InternalObj, distance);
    public int Rotation { get { return Api.TouchScreen_GetRotation.Invoke(InternalObj); } }
    public bool CompareWithBlockAndSurface(IMyCubeBlock block, IMyTextSurface surface) => Api.TouchScreen_CompareWithBlockAndSurface.Invoke(InternalObj, block, surface);
    public void ForceDispose() => Api.TouchScreen_ForceDispose.Invoke(InternalObj);
  }
  public class ClickHandler : WrapperBase
  {
    public ClickHandler() : base(Api.ClickHandler_New()) { }
    public ClickHandler(object internalObject) : base(internalObject) { }
    public Vector4 HitArea { get { return Api.ClickHandler_GetHitArea.Invoke(InternalObj); } set { Api.ClickHandler_SetHitArea.Invoke(InternalObj, value); } }
    public bool IsMouseReleased { get { return Api.ClickHandler_IsMouseReleased.Invoke(InternalObj); } }
    public bool IsMouseOver { get { return Api.ClickHandler_IsMouseOver.Invoke(InternalObj); } }
    public bool IsMousePressed { get { return Api.ClickHandler_IsMousePressed.Invoke(InternalObj); } }
    public bool JustReleased { get { return Api.ClickHandler_JustReleased.Invoke(InternalObj); } }
    public bool JustPressed { get { return Api.ClickHandler_JustPressed.Invoke(InternalObj); } }
    public void UpdateStatus(TouchScreen screen) => Api.ClickHandler_UpdateStatus.Invoke(InternalObj, screen.InternalObj);
  }
  public class FancyCursor : WrapperBase
  {
    public FancyCursor(TouchScreen screen) : base(Api.FancyCursor_New(screen.InternalObj)) { }
    public FancyCursor(object internalObject) : base(internalObject) { }
    public bool Active { get { return Api.FancyCursor_GetActive.Invoke(InternalObj); } set { Api.FancyCursor_SetActive.Invoke(InternalObj, value); } }
    public Vector2 Position { get { return Api.FancyCursor_GetPosition.Invoke(InternalObj); } }
    public bool IsInsideArea(float x, float y, float z, float w) => Api.FancyCursor_IsInsideArea.Invoke(InternalObj, x, y, z, w);
    public List<MySprite> GetSprites() => Api.FancyCursor_GetSprites.Invoke(InternalObj);
    public void ForceDispose() => Api.FancyCursor_ForceDispose.Invoke(InternalObj);
  }
  public class FancyTheme : WrapperBase
  {
    public FancyTheme(object internalObject) : base(internalObject) { }
    public Color BgColor { get { return Api.FancyTheme_GetBgColor.Invoke(InternalObj); } }
    public Color WhiteColor { get { return Api.FancyTheme_GetWhiteColor.Invoke(InternalObj); } }
    public Color MainColor { get { return Api.FancyTheme_GetMainColor.Invoke(InternalObj); } }
    public Color GetMainColorDarker(int value) => Api.FancyTheme_GetMainColorDarker.Invoke(InternalObj, value);
    public float Scale { get { return Api.FancyTheme_GetScale.Invoke(InternalObj); } set { Api.FancyTheme_SetScale.Invoke(InternalObj, value); } }
    public string Font { get { return Api.FancyTheme_GetFont.Invoke(InternalObj); } set { Api.FancyTheme_SetFont.Invoke(InternalObj, value); } }
    public Vector2 MeasureStringInPixels(string text, string font, float scale) => Api.FancyTheme_MeasureStringInPixels.Invoke(InternalObj, text, font, scale);
  }
  public abstract class FancyElementBase : WrapperBase
  {
    private FancyApp _app;
    private FancyView _parent;
    public FancyElementBase(object internalObject) : base(internalObject) { }
    public bool Enabled { get { return Api.FancyElementBase_GetEnabled.Invoke(InternalObj); } set { Api.FancyElementBase_SetEnabled.Invoke(InternalObj, value); } }
    public bool Absolute { get { return Api.FancyElementBase_GetAbsolute.Invoke(InternalObj); } set { Api.FancyElementBase_SetAbsolute.Invoke(InternalObj, value); } }
    public ViewAlignment SelfAlignment { get { return (ViewAlignment)Api.FancyElementBase_GetSelfAlignment.Invoke(InternalObj); } set { Api.FancyElementBase_SetSelfAlignment.Invoke(InternalObj, (byte)value); } }
    public Vector2 Position { get { return Api.FancyElementBase_GetPosition.Invoke(InternalObj); } set { Api.FancyElementBase_SetPosition.Invoke(InternalObj, value); } }
    public Vector4 Margin { get { return Api.FancyElementBase_GetMargin.Invoke(InternalObj); } set { Api.FancyElementBase_SetMargin.Invoke(InternalObj, value); } }
    public Vector2 Scale { get { return Api.FancyElementBase_GetScale.Invoke(InternalObj); } set { Api.FancyElementBase_SetScale.Invoke(InternalObj, value); } }
    public Vector2 Pixels { get { return Api.FancyElementBase_GetPixels.Invoke(InternalObj); } set { Api.FancyElementBase_SetPixels.Invoke(InternalObj, value); } }
    public FancyApp App { get { return _app ?? (_app = Wrap<FancyApp>(Api.FancyElementBase_GetApp.Invoke(InternalObj), (obj) => new FancyApp(obj))); } }
    public FancyView Parent { get { return _parent ?? (_parent = Wrap<FancyView>(Api.FancyElementBase_GetParent.Invoke(InternalObj), (obj) => new FancyView(obj))); } }
    public List<MySprite> GetSprites() => Api.FancyElementBase_GetSprites.Invoke(InternalObj);
    public Vector2 GetSize() => Api.FancyElementBase_GetSize.Invoke(InternalObj);
    public Vector2 GetBoundaries() => Api.FancyElementBase_GetBoundaries.Invoke(InternalObj);
    public void ForceUpdate() => Api.FancyElementBase_ForceUpdate.Invoke(InternalObj);
    public void ForceDispose() => Api.FancyElementBase_ForceDispose.Invoke(InternalObj);
    public void RegisterUpdate(Action update) => Api.FancyElementBase_RegisterUpdate.Invoke(InternalObj, update);
    public void UnregisterUpdate(Action update) => Api.FancyElementBase_UnregisterUpdate.Invoke(InternalObj, update);
  }
  public abstract class FancyContainerBase : FancyElementBase
  {
    public FancyContainerBase(object internalObject) : base(internalObject) { }
    public List<object> Children { get { return Api.FancyContainerBase_GetChildren.Invoke(InternalObj); } }
    public Vector2 GetFlexSize() => Api.FancyContainerBase_GetFlexSize.Invoke(InternalObj);
    public void AddChild(FancyElementBase child) => Api.FancyContainerBase_AddChild.Invoke(InternalObj, child.InternalObj);
    public void AddChild(FancyElementBase child, int index) => Api.FancyContainerBase_AddChildAt.Invoke(InternalObj, child.InternalObj, index);
    public void RemoveChild(FancyElementBase child) => Api.FancyContainerBase_RemoveChild.Invoke(InternalObj, child.InternalObj);
    public void RemoveChild(object child) => Api.FancyContainerBase_RemoveChild.Invoke(InternalObj, child);
    public void RemoveChild(int index) => Api.FancyContainerBase_RemoveChildAt.Invoke(InternalObj, index);
    public void MoveChild(FancyElementBase child, int index) => Api.FancyContainerBase_MoveChild.Invoke(InternalObj, child.InternalObj, index);
  }
  public enum ViewDirection : byte { None = 0, Row = 1, Column = 2, RowReverse = 3, ColumnReverse = 4 }
  public enum ViewAlignment : byte { Start = 0, Center = 1, End = 2 }
  public enum ViewAnchor : byte { Start = 0, Center = 1, End = 2, SpaceBetween = 3, SpaceAround = 4 }
  public class FancyView : FancyContainerBase
  {
    public FancyView(ViewDirection direction = ViewDirection.Column, Color? bgColor = null) : base(Api.FancyView_New((int)direction, bgColor)) { }
    public FancyView(object internalObject) : base(internalObject) { }
    public bool Overflow { get { return Api.FancyView_GetOverflow.Invoke(InternalObj); } set { Api.FancyView_SetOverflow.Invoke(InternalObj, value); } }
    public ViewDirection Direction { get { return (ViewDirection)Api.FancyView_GetDirection.Invoke(InternalObj); } set { Api.FancyView_SetDirection.Invoke(InternalObj, (byte)value); } }
    public ViewAlignment Alignment { get { return (ViewAlignment)Api.FancyView_GetAlignment.Invoke(InternalObj); } set { Api.FancyView_SetAlignment.Invoke(InternalObj, (byte)value); } }
    public ViewAnchor Anchor { get { return (ViewAnchor)Api.FancyView_GetAnchor.Invoke(InternalObj); } set { Api.FancyView_SetAnchor.Invoke(InternalObj, (byte)value); } }
    public bool UseThemeColors { get { return Api.FancyView_GetUseThemeColors.Invoke(InternalObj); } set { Api.FancyView_SetUseThemeColors.Invoke(InternalObj, value); } }
    public Color BgColor { get { return Api.FancyView_GetBgColor.Invoke(InternalObj); } set { Api.FancyView_SetBgColor.Invoke(InternalObj, value); } }
    public Color BorderColor { get { return Api.FancyView_GetBorderColor.Invoke(InternalObj); } set { Api.FancyView_SetBorderColor.Invoke(InternalObj, value); } }
    public Vector4 Border { get { return Api.FancyView_GetBorder.Invoke(InternalObj); } set { Api.FancyView_SetBorder.Invoke(InternalObj, value); } }
    public Vector4 Padding { get { return Api.FancyView_GetPadding.Invoke(InternalObj); } set { Api.FancyView_SetPadding.Invoke(InternalObj, value); } }
    public int Gap { get { return Api.FancyView_GetGap.Invoke(InternalObj); } set { Api.FancyView_SetGap.Invoke(InternalObj, value); } }
  }
  public class FancyScrollView : FancyView
  {
    private FancyBarContainer _scrollBar;
    public FancyScrollView(ViewDirection direction = ViewDirection.Column, Color? bgColor = null) : base(Api.FancyScrollView_New((int)direction, bgColor)) { }
    public float Scroll { get { return Api.FancyScrollView_GetScroll.Invoke(InternalObj); } set { Api.FancyScrollView_SetScroll.Invoke(InternalObj, value); } }
    public bool ScrollAlwaysVisible { get { return Api.FancyScrollView_GetScrollAlwaysVisible.Invoke(InternalObj); } set { Api.FancyScrollView_SetScrollAlwaysVisible.Invoke(InternalObj, value); } }
    public FancyBarContainer ScrollBar { get { return _scrollBar ?? (_scrollBar = Wrap<FancyBarContainer>(Api.FancyScrollView_GetScrollBar.Invoke(InternalObj), (obj) => new FancyBarContainer(obj))); } }
  }
  public class FancyApp : FancyView
  {
    private TouchScreen _screen;
    private FancyCursor _cursor;
    private FancyTheme _theme;
    public FancyApp() : base(Api.FancyApp_New()) { }
    public FancyApp(object internalObject) : base(internalObject) { }
    public TouchScreen Screen { get { return _screen ?? (_screen = Wrap<TouchScreen>(Api.FancyApp_GetScreen.Invoke(InternalObj), (obj) => new TouchScreen(obj))); } }
    public RectangleF Viewport { get { return Api.FancyApp_GetViewport.Invoke(InternalObj); } }
    public FancyCursor Cursor { get { return _cursor ?? (_cursor = Wrap<FancyCursor>(Api.FancyApp_GetCursor.Invoke(InternalObj), (obj) => new FancyCursor(obj))); } }
    public FancyTheme Theme { get { return _theme ?? (_theme = Wrap<FancyTheme>(Api.FancyApp_GetTheme.Invoke(InternalObj), (obj) => new FancyTheme(obj))); } }
    public bool DefaultBg { get { return Api.FancyApp_GetDefaultBg.Invoke(InternalObj); } set { Api.FancyApp_SetDefaultBg.Invoke(InternalObj, value); } }
    public virtual void InitApp(MyCubeBlock block, Sandbox.ModAPI.Ingame.IMyTextSurface surface) => Api.FancyApp_InitApp.Invoke(InternalObj, block, surface);
  }
  public class FancyEmptyButton : FancyView
  {
    private ClickHandler _handler;
    public FancyEmptyButton(Action onChange) : base(Api.FancyEmptyButton_New(onChange)) { }
    public FancyEmptyButton(object internalObject) : base(internalObject) { }
    public ClickHandler Handler { get { return _handler ?? (_handler = Wrap<ClickHandler>(Api.FancyEmptyButton_GetHandler.Invoke(InternalObj), (obj) => new ClickHandler(obj))); } }
    public Action OnChange { set { Api.FancyEmptyButton_SetOnChange.Invoke(InternalObj, value); } }
  }
  public class FancyButton : FancyEmptyButton
  {
    private FancyLabel _label;
    public FancyButton(string text, Action onChange) : base(Api.FancyButton_New(text, onChange)) { }
    public FancyButton(object internalObject) : base(internalObject) { }
    public FancyLabel Label { get { return _label ?? (_label = Wrap<FancyLabel>(Api.FancyButton_GetLabel.Invoke(InternalObj), (obj) => new FancyLabel(obj))); } }
  }
  public class FancyCheckbox : FancyView
  {
    private FancyEmptyElement _checkMark;
    public FancyCheckbox(Action<bool> onChange, bool value = false) : base(Api.FancyCheckbox_New(onChange, value)) { }
    public FancyCheckbox(object internalObject) : base(internalObject) { }
    public bool Value { get { return Api.FancyCheckbox_GetValue.Invoke(InternalObj); } set { Api.FancyCheckbox_SetValue.Invoke(InternalObj, value); } }
    public Action<bool> OnChange { set { Api.FancyCheckbox_SetOnChange.Invoke(InternalObj, value); } }
    public FancyEmptyElement CheckMark { get { return _checkMark ?? (_checkMark = Wrap<FancyEmptyElement>(Api.FancyCheckbox_GetCheckMark.Invoke(InternalObj), (obj) => new FancyEmptyElement(obj))); } }
  }
  public class FancyLabel : FancyElementBase
  {
    public FancyLabel(string text, float fontSize = 0.5f, TextAlignment alignment = TextAlignment.CENTER) : base(Api.FancyLabel_New(text, fontSize, alignment)) { }
    public FancyLabel(object internalObject) : base(internalObject) { }
    public bool Overflow { get { return Api.FancyLabel_GetOverflow.Invoke(InternalObj); } set { Api.FancyLabel_SetOverflow.Invoke(InternalObj, value); } }
    public bool IsShortened { get { return Api.FancyLabel_GetIsShortened.Invoke(InternalObj); } }
    public string Text { get { return Api.FancyLabel_GetText.Invoke(InternalObj); } set { Api.FancyLabel_SetText.Invoke(InternalObj, value); } }
    public Color? TextColor { get { return Api.FancyLabel_GetTextColor.Invoke(InternalObj); } set { Api.FancyLabel_SetTextColor.Invoke(InternalObj, (Color)value); } }
    public float FontSize { get { return Api.FancyLabel_GetFontSize.Invoke(InternalObj); } set { Api.FancyLabel_SetFontSize.Invoke(InternalObj, value); } }
    public TextAlignment Alignment { get { return Api.FancyLabel_GetAlignment.Invoke(InternalObj); } set { Api.FancyLabel_SetAlignment.Invoke(InternalObj, value); } }
  }
  public class FancyBarContainer : FancyView
  {
    private FancyView _bar;
    public FancyBarContainer(bool vertical = false) : base(Api.FancyBarContainer_New(vertical)) { }
    public FancyBarContainer(object internalObject) : base(internalObject) { }
    public bool IsVertical { get { return Api.FancyBarContainer_GetIsVertical.Invoke(InternalObj); } set { Api.FancyBarContainer_SetIsVertical.Invoke(InternalObj, value); } }
    public float Ratio { get { return Api.FancyBarContainer_GetRatio.Invoke(InternalObj); } set { Api.FancyBarContainer_SetRatio.Invoke(InternalObj, value); } }
    public float Offset { get { return Api.FancyBarContainer_GetOffset.Invoke(InternalObj); } set { Api.FancyBarContainer_SetOffset.Invoke(InternalObj, value); } }
    public FancyView Bar { get { return _bar ?? (_bar = Wrap<FancyView>(Api.FancyBarContainer_GetBar.Invoke(InternalObj), (obj) => new FancyView(obj))); } }
  }
  public class FancyProgressBar : FancyBarContainer
  {
    private FancyLabel _label;
    public FancyProgressBar(float min, float max, bool vertical = false, float barsGap = 0) : base(Api.FancyProgressBar_New(min, max, vertical, barsGap)) { }
    public FancyProgressBar(object internalObject) : base(internalObject) { }
    public float Value { get { return Api.FancyProgressBar_GetValue.Invoke(InternalObj); } set { Api.FancyProgressBar_SetValue.Invoke(InternalObj, value); } }
    public float MaxValue { get { return Api.FancyProgressBar_GetMaxValue.Invoke(InternalObj); } set { Api.FancyProgressBar_SetMaxValue.Invoke(InternalObj, value); } }
    public float MinValue { get { return Api.FancyProgressBar_GetMinValue.Invoke(InternalObj); } set { Api.FancyProgressBar_SetMinValue.Invoke(InternalObj, value); } }
    public float BarsGap { get { return Api.FancyProgressBar_GetBarsGap.Invoke(InternalObj); } set { Api.FancyProgressBar_SetBarsGap.Invoke(InternalObj, value); } }
    public FancyLabel Label { get { return _label ?? (_label = Wrap<FancyLabel>(Api.FancyProgressBar_GetLabel.Invoke(InternalObj), (obj) => new FancyLabel(obj))); } }
  }
  public class FancySelector : FancyView
  {
    public FancySelector(List<string> labels, Action<int, string> onChange, bool loop = true) : base(Api.FancySelector_New(labels, onChange, loop)) { }
    public FancySelector(object internalObject) : base(internalObject) { }
    public bool Loop { get { return Api.FancySelector_GetLoop.Invoke(InternalObj); } set { Api.FancySelector_SetLoop.Invoke(InternalObj, value); } }
    public int Selected { get { return Api.FancySelector_GetSelected.Invoke(InternalObj); } set { Api.FancySelector_SetSelected.Invoke(InternalObj, value); } }
    public Action<int, string> OnChange { set { Api.FancySelector_SetOnChange.Invoke(InternalObj, value); } }
  }
  public class FancySlider : FancyView
  {
    private FancyBarContainer _bar;
    private FancyEmptyElement _thumb;
    private FancyTextField _textInput;
    public FancySlider(float min, float max, Action<float> onChange) : base(Api.FancySlider_New(min, max, onChange)) { }
    public FancySlider(object internalObject) : base(internalObject) { }
    public float MaxValue { get { return Api.FancySlider_GetMaxValue.Invoke(InternalObj); } set { Api.FancySlider_SetMaxValue.Invoke(InternalObj, value); } }
    public float MinValue { get { return Api.FancySlider_GetMinValue.Invoke(InternalObj); } set { Api.FancySlider_SetMinValue.Invoke(InternalObj, value); } }
    public float Value { get { return Api.FancySlider_GetValue.Invoke(InternalObj); } set { Api.FancySlider_SetValue.Invoke(InternalObj, value); } }
    public bool IsInteger { get { return Api.FancySlider_GetIsInteger.Invoke(InternalObj); } set { Api.FancySlider_SetIsInteger.Invoke(InternalObj, value); } }
    public bool AllowInput { get { return Api.FancySlider_GetAllowInput.Invoke(InternalObj); } set { Api.FancySlider_SetAllowInput.Invoke(InternalObj, value); } }
    public Action<float> OnChange { set { Api.FancySlider_SetOnChange.Invoke(InternalObj, value); } }
    public FancyBarContainer Bar { get { return _bar ?? (_bar = Wrap<FancyBarContainer>(Api.FancySlider_GetBar.Invoke(InternalObj), (obj) => new FancyBarContainer(obj))); } }
    public FancyEmptyElement Thumb { get { return _thumb ?? (_thumb = Wrap<FancyEmptyElement>(Api.FancySlider_GetThumb.Invoke(InternalObj), (obj) => new FancyEmptyElement(obj))); } }
    public FancyTextField TextInput { get { return _textInput ?? (_textInput = Wrap<FancyTextField>(Api.FancySlider_GetTextInput.Invoke(InternalObj), (obj) => new FancyTextField(obj))); } }
  }
  public class FancySliderRange : FancySlider
  {
    private FancyEmptyElement _thumbLower;
    public FancySliderRange(float min, float max, Action<float, float> onChange) : base(Api.FancySliderRange_NewR(min, max, onChange)) { }
    public FancySliderRange(object internalObject) : base(internalObject) { }
    public float ValueLower { get { return Api.FancySliderRange_GetValueLower.Invoke(InternalObj); } set { Api.FancySliderRange_SetValueLower.Invoke(InternalObj, value); } }
    public Action<float, float> OnChangeRange { set { Api.FancySliderRange_SetOnChangeR.Invoke(InternalObj, value); } }
    public FancyEmptyElement ThumbLower { get { return _thumbLower ?? (_thumbLower = Wrap<FancyEmptyElement>(Api.FancySliderRange_GetThumbLower.Invoke(InternalObj), (obj) => new FancyEmptyElement(obj))); } }
  }
  public class FancySwitch : FancyView
  {
    private FancyButton[] _buttons;
    public FancySwitch(string[] labels, int index = 0, Action<int> onChange = null) : base(Api.FancySwitch_New(labels, index, onChange)) { }
    public FancySwitch(object internalObject) : base(internalObject) { }
    public int Index { get { return Api.FancySwitch_GetIndex.Invoke(InternalObj); } set { Api.FancySwitch_SetIndex.Invoke(InternalObj, value); } }
    public FancyButton[] Buttons { get { return _buttons ?? (_buttons = WrapArray<FancyButton>(Api.FancySwitch_GetButtons.Invoke(InternalObj), (obj) => new FancyButton(obj))); } }
    public Action<int> OnChange { set { Api.FancySwitch_SetOnChange.Invoke(InternalObj, value); } }
  }
  public class FancyTextField : FancyView
  {
    private FancyLabel _label;
    public FancyTextField(string text, Action<string, bool> onChange) : base(Api.FancyTextField_New(text, onChange)) { }
    public FancyTextField(object internalObject) : base(internalObject) { }
    public bool IsEditing { get { return Api.FancyTextField_GetIsEditing.Invoke(InternalObj); } }
    public string Text { get { return Api.FancyTextField_GetText.Invoke(InternalObj); } set { Api.FancyTextField_SetText.Invoke(InternalObj, value); } }
    public bool IsNumeric { get { return Api.FancyTextField_GetIsNumeric.Invoke(InternalObj); } set { Api.FancyTextField_SetIsNumeric.Invoke(InternalObj, value); } }
    public bool IsInteger { get { return Api.FancyTextField_GetIsInteger.Invoke(InternalObj); } set { Api.FancyTextField_SetIsInteger.Invoke(InternalObj, value); } }
    public bool AllowNegative { get { return Api.FancyTextField_GetAllowNegative.Invoke(InternalObj); } set { Api.FancyTextField_SetAllowNegative.Invoke(InternalObj, value); } }
    public FancyLabel Label { get { return _label ?? (_label = Wrap<FancyLabel>(Api.FancyTextField_GetLabel.Invoke(InternalObj), (obj) => new FancyLabel(obj))); } }
    public Action<string, bool> OnChange { set { Api.FancyTextField_SetOnChange.Invoke(InternalObj, value); } }
  }
  public class FancyWindowBar : FancyView
  {
    private FancyLabel _label;
    public FancyWindowBar(string text) : base(Api.FancyWindowBar_New(text)) { }
    public FancyWindowBar(object internalObject) : base(internalObject) { }
    public FancyLabel Label { get { return _label ?? (_label = Wrap<FancyLabel>(Api.FancyWindowBar_GetLabel.Invoke(InternalObj), (obj) => new FancyLabel(obj))); } }
  }
  public class FancyChart : FancyElementBase
  {
    public FancyChart(int intervals) : base(Api.FancyChart_New(intervals)) { }
    public FancyChart(object internalObject) : base(internalObject) { }
    public List<float[]> DataSets { get { return Api.FancyChart_GetDataSets.Invoke(InternalObj); } }
    public List<Color> DataColors { get { return Api.FancyChart_GetDataColors.Invoke(InternalObj); } }
    public int GridHorizontalLines { get { return Api.FancyChart_GetGridHorizontalLines.Invoke(InternalObj); } set { Api.FancyChart_SetGridHorizontalLines.Invoke(InternalObj, value); } }
    public int GridVerticalLines { get { return Api.FancyChart_GetGridVerticalLines.Invoke(InternalObj); } set { Api.FancyChart_SetGridVerticalLines.Invoke(InternalObj, value); } }
    public float MaxValue { get { return Api.FancyChart_GetMaxValue.Invoke(InternalObj); } }
    public float MinValue { get { return Api.FancyChart_GetMinValue.Invoke(InternalObj); } }
    public Color? GridColor { get { return Api.FancyChart_GetGridColor.Invoke(InternalObj); } set { Api.FancyChart_SetGridColor.Invoke(InternalObj, (Color)value); } }
  }
  public class FancyEmptyElement : FancyElementBase
  {
    public FancyEmptyElement() : base(Api.FancyEmptyElement_New()) { }
    public FancyEmptyElement(object internalObject) : base(internalObject) { }
  }
}