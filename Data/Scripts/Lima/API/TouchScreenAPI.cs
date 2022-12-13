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

      DelegatorBase.SetApi(this);
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
      DelegatorBase.SetApi(null);
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
      AssignMethod(delegates, "TouchScreen_GetScreenRotate", ref TouchScreen_GetScreenRotate);
      AssignMethod(delegates, "TouchScreen_CompareWithBlockAndSurface", ref TouchScreen_CompareWithBlockAndSurface);
      AssignMethod(delegates, "TouchScreen_Dispose", ref TouchScreen_Dispose);
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
      AssignMethod(delegates, "FancyCursor_Dispose", ref FancyCursor_Dispose);
      AssignMethod(delegates, "FancyTheme_GetColorBg", ref FancyTheme_GetColorBg);
      AssignMethod(delegates, "FancyTheme_GetColorWhite", ref FancyTheme_GetColorWhite);
      AssignMethod(delegates, "FancyTheme_GetColorMain", ref FancyTheme_GetColorMain);
      AssignMethod(delegates, "FancyTheme_GetColorMainDarker", ref FancyTheme_GetColorMainDarker);
      AssignMethod(delegates, "FancyTheme_MeasureStringInPixels", ref FancyTheme_MeasureStringInPixels);
      AssignMethod(delegates, "FancyTheme_GetScale", ref FancyTheme_GetScale);
      AssignMethod(delegates, "FancyTheme_SetScale", ref FancyTheme_SetScale);
      AssignMethod(delegates, "FancyElementBase_GetEnabled", ref FancyElementBase_GetEnabled);
      AssignMethod(delegates, "FancyElementBase_SetEnabled", ref FancyElementBase_SetEnabled);
      AssignMethod(delegates, "FancyElementBase_GetPosition", ref FancyElementBase_GetPosition);
      AssignMethod(delegates, "FancyElementBase_SetPosition", ref FancyElementBase_SetPosition);
      AssignMethod(delegates, "FancyElementBase_GetMargin", ref FancyElementBase_GetMargin);
      AssignMethod(delegates, "FancyElementBase_SetMargin", ref FancyElementBase_SetMargin);
      AssignMethod(delegates, "FancyElementBase_GetScale", ref FancyElementBase_GetScale);
      AssignMethod(delegates, "FancyElementBase_SetScale", ref FancyElementBase_SetScale);
      AssignMethod(delegates, "FancyElementBase_GetPixels", ref FancyElementBase_GetPixels);
      AssignMethod(delegates, "FancyElementBase_SetPixels", ref FancyElementBase_SetPixels);
      AssignMethod(delegates, "FancyElementBase_GetSize", ref FancyElementBase_GetSize);
      AssignMethod(delegates, "FancyElementBase_GetApp", ref FancyElementBase_GetApp);
      AssignMethod(delegates, "FancyElementBase_GetParent", ref FancyElementBase_GetParent);
      AssignMethod(delegates, "FancyElementBase_GetSprites", ref FancyElementBase_GetSprites);
      AssignMethod(delegates, "FancyElementBase_InitElements", ref FancyElementBase_InitElements);
      AssignMethod(delegates, "FancyElementBase_Update", ref FancyElementBase_Update);
      AssignMethod(delegates, "FancyElementBase_Dispose", ref FancyElementBase_Dispose);
      AssignMethod(delegates, "FancyElementContainerBase_GetChildren", ref FancyElementContainerBase_GetChildren);
      AssignMethod(delegates, "FancyElementContainerBase_AddChild", ref FancyElementContainerBase_AddChild);
      AssignMethod(delegates, "FancyElementContainerBase_RemoveChild", ref FancyElementContainerBase_RemoveChild);
      AssignMethod(delegates, "FancyView_NewV", ref FancyView_NewV);
      AssignMethod(delegates, "FancyView_GetDirection", ref FancyView_GetDirection);
      AssignMethod(delegates, "FancyView_SetDirection", ref FancyView_SetDirection);
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
      AssignMethod(delegates, "FancyApp_New", ref FancyApp_New);
      AssignMethod(delegates, "FancyApp_GetScreen", ref FancyApp_GetScreen);
      AssignMethod(delegates, "FancyApp_GetViewport", ref FancyApp_GetViewport);
      AssignMethod(delegates, "FancyApp_GetCursor", ref FancyApp_GetCursor);
      AssignMethod(delegates, "FancyApp_GetTheme", ref FancyApp_GetTheme);
      AssignMethod(delegates, "FancyApp_InitApp", ref FancyApp_InitApp);
      AssignMethod(delegates, "FancyButtonBase_GetHandler", ref FancyButtonBase_GetHandler);
      AssignMethod(delegates, "FancyButton_New", ref FancyButton_New);
      AssignMethod(delegates, "FancyButton_GetText", ref FancyButton_GetText);
      AssignMethod(delegates, "FancyButton_SetText", ref FancyButton_SetText);
      AssignMethod(delegates, "FancyButton_SetOnChange", ref FancyButton_SetOnChange);
      AssignMethod(delegates, "FancyButton_GetAlignment", ref FancyButton_GetAlignment);
      AssignMethod(delegates, "FancyButton_SetAlignment", ref FancyButton_SetAlignment);
      AssignMethod(delegates, "FancyLabel_New", ref FancyLabel_New);
      AssignMethod(delegates, "FancyLabel_GetText", ref FancyLabel_GetText);
      AssignMethod(delegates, "FancyLabel_SetText", ref FancyLabel_SetText);
      AssignMethod(delegates, "FancyLabel_SetFontSize", ref FancyLabel_SetFontSize);
      AssignMethod(delegates, "FancyLabel_GetAlignment", ref FancyLabel_GetAlignment);
      AssignMethod(delegates, "FancyLabel_SetAlignment", ref FancyLabel_SetAlignment);
      AssignMethod(delegates, "FancyProgressBar_New", ref FancyProgressBar_New);
      AssignMethod(delegates, "FancyProgressBar_GetValue", ref FancyProgressBar_GetValue);
      AssignMethod(delegates, "FancyProgressBar_SetValue", ref FancyProgressBar_SetValue);
      AssignMethod(delegates, "FancySelector_New", ref FancySelector_New);
      AssignMethod(delegates, "FancySelector_SetOnChange", ref FancySelector_SetOnChange);
      AssignMethod(delegates, "FancySeparator_New", ref FancySeparator_New);
      AssignMethod(delegates, "FancySlider_New", ref FancySlider_New);
      AssignMethod(delegates, "FancySlider_GetRange", ref FancySlider_GetRange);
      AssignMethod(delegates, "FancySlider_SetRange", ref FancySlider_SetRange);
      AssignMethod(delegates, "FancySlider_GetValue", ref FancySlider_GetValue);
      AssignMethod(delegates, "FancySlider_SetValue", ref FancySlider_SetValue);
      AssignMethod(delegates, "FancySlider_SetOnChange", ref FancySlider_SetOnChange);
      AssignMethod(delegates, "FancySlider_GetIsInteger", ref FancySlider_GetIsInteger);
      AssignMethod(delegates, "FancySlider_SetIsInteger", ref FancySlider_SetIsInteger);
      AssignMethod(delegates, "FancySlider_GetAllowInput", ref FancySlider_GetAllowInput);
      AssignMethod(delegates, "FancySlider_SetAllowInput", ref FancySlider_SetAllowInput);
      AssignMethod(delegates, "FancySliderRange_NewR", ref FancySliderRange_NewR);
      AssignMethod(delegates, "FancySliderRange_GetValueLower", ref FancySliderRange_GetValueLower);
      AssignMethod(delegates, "FancySliderRange_SetValueLower", ref FancySliderRange_SetValueLower);
      AssignMethod(delegates, "FancySliderRange_SetOnChangeR", ref FancySliderRange_SetOnChangeR);

      AssignMethod(delegates, "FancySwitch_New", ref FancySwitch_New);
      AssignMethod(delegates, "FancySwitch_GetIndex", ref FancySwitch_GetIndex);
      AssignMethod(delegates, "FancySwitch_SetIndex", ref FancySwitch_SetIndex);
      AssignMethod(delegates, "FancySwitch_GetTabNames", ref FancySwitch_GetTabNames);
      AssignMethod(delegates, "FancySwitch_GetTabName", ref FancySwitch_GetTabName);
      AssignMethod(delegates, "FancySwitch_SetTabName", ref FancySwitch_SetTabName);
      AssignMethod(delegates, "FancySwitch_SetOnChange", ref FancySwitch_SetOnChange);

      AssignMethod(delegates, "FancyTextField_New", ref FancyTextField_New);
      AssignMethod(delegates, "FancyTextField_GetText", ref FancyTextField_GetText);
      AssignMethod(delegates, "FancyTextField_SetText", ref FancyTextField_SetText);
      AssignMethod(delegates, "FancyTextField_SetOnChange", ref FancyTextField_SetOnChange);
      AssignMethod(delegates, "FancyTextField_GetIsNumeric", ref FancyTextField_GetIsNumeric);
      AssignMethod(delegates, "FancyTextField_SetIsNumeric", ref FancyTextField_SetIsNumeric);
      AssignMethod(delegates, "FancyTextField_GetIsInteger", ref FancyTextField_GetIsInteger);
      AssignMethod(delegates, "FancyTextField_SetIsInteger", ref FancyTextField_SetIsInteger);
      AssignMethod(delegates, "FancyTextField_GetAllowNegative", ref FancyTextField_GetAllowNegative);
      AssignMethod(delegates, "FancyTextField_SetAllowNegative", ref FancyTextField_SetAllowNegative);
      AssignMethod(delegates, "FancyTextField_GetAlignment", ref FancyTextField_GetAlignment);
      AssignMethod(delegates, "FancyTextField_SetAlignment", ref FancyTextField_SetAlignment);
      AssignMethod(delegates, "FancyWindowBar_New", ref FancyWindowBar_New);
      AssignMethod(delegates, "FancyWindowBar_GetText", ref FancyWindowBar_GetText);
      AssignMethod(delegates, "FancyWindowBar_SetText", ref FancyWindowBar_SetText);
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
    public Func<object, int> TouchScreen_GetScreenRotate;
    public Func<object, IMyCubeBlock, IMyTextSurface, bool> TouchScreen_CompareWithBlockAndSurface;
    public Action<object> TouchScreen_Dispose;

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
    public Action<object> FancyCursor_Dispose;

    public Func<object, Color> FancyTheme_GetColorBg;
    public Func<object, Color> FancyTheme_GetColorWhite;
    public Func<object, Color> FancyTheme_GetColorMain;
    public Func<object, int, Color> FancyTheme_GetColorMainDarker;
    public Func<object, String, string, float, Vector2> FancyTheme_MeasureStringInPixels;
    public Func<object, float> FancyTheme_GetScale;
    public Action<object, float> FancyTheme_SetScale;

    public Func<object, bool> FancyElementBase_GetEnabled;
    public Action<object, bool> FancyElementBase_SetEnabled;
    public Func<object, Vector2> FancyElementBase_GetPosition;
    public Action<object, Vector2> FancyElementBase_SetPosition;
    public Func<object, Vector4> FancyElementBase_GetMargin;
    public Action<object, Vector4> FancyElementBase_SetMargin;
    public Func<object, Vector2> FancyElementBase_GetScale;
    public Action<object, Vector2> FancyElementBase_SetScale;
    public Func<object, Vector2> FancyElementBase_GetPixels;
    public Action<object, Vector2> FancyElementBase_SetPixels;
    public Func<object, Vector2> FancyElementBase_GetSize;
    public Func<object, object> FancyElementBase_GetApp;
    public Func<object, object> FancyElementBase_GetParent;
    public Func<object, List<MySprite>> FancyElementBase_GetSprites;
    public Action<object> FancyElementBase_InitElements;
    public Action<object> FancyElementBase_Update;
    public Action<object> FancyElementBase_Dispose;

    public Func<object, List<object>> FancyElementContainerBase_GetChildren;
    public Action<object, object> FancyElementContainerBase_AddChild;
    public Action<object, object> FancyElementContainerBase_RemoveChild;

    public Func<int, object> FancyView_NewV;
    public Func<object, int> FancyView_GetDirection;
    public Action<object, int> FancyView_SetDirection;
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

    public Func<object> FancyApp_New;
    public Func<object, object> FancyApp_GetScreen;
    public Func<object, RectangleF> FancyApp_GetViewport;
    public Func<object, object> FancyApp_GetCursor;
    public Func<object, object> FancyApp_GetTheme;
    public Action<object, MyCubeBlock, Sandbox.ModAPI.Ingame.IMyTextSurface> FancyApp_InitApp;

    public Func<object, object> FancyButtonBase_GetHandler;

    public Func<string, Action, object> FancyButton_New;
    public Func<object, string> FancyButton_GetText;
    public Action<object, string> FancyButton_SetText;
    public Action<object, Action> FancyButton_SetOnChange;
    public Func<object, TextAlignment> FancyButton_GetAlignment;
    public Action<object, TextAlignment> FancyButton_SetAlignment;

    public Func<string, float, TextAlignment, object> FancyLabel_New;
    public Func<object, string> FancyLabel_GetText;
    public Action<object, string> FancyLabel_SetText;
    public Action<object, float> FancyLabel_SetFontSize;
    public Func<object, TextAlignment> FancyLabel_GetAlignment;
    public Action<object, TextAlignment> FancyLabel_SetAlignment;

    public Func<float, float, bool, object> FancyProgressBar_New;
    public Func<object, float> FancyProgressBar_GetValue;
    public Action<object, float> FancyProgressBar_SetValue;

    public Func<List<string>, Action<int, string>, bool, object> FancySelector_New;
    public Action<object, Action<int, string>> FancySelector_SetOnChange;

    public Func<object> FancySeparator_New;

    public Func<float, float, Action<float>, object> FancySlider_New;
    public Func<object, Vector2> FancySlider_GetRange;
    public Action<object, Vector2> FancySlider_SetRange;
    public Func<object, float> FancySlider_GetValue;
    public Action<object, float> FancySlider_SetValue;
    public Action<object, Action<float>> FancySlider_SetOnChange;
    public Func<object, bool> FancySlider_GetIsInteger;
    public Action<object, bool> FancySlider_SetIsInteger;
    public Func<object, bool> FancySlider_GetAllowInput;
    public Action<object, bool> FancySlider_SetAllowInput;

    public Func<float, float, Action<float, float>, object> FancySliderRange_NewR;
    public Func<object, float> FancySliderRange_GetValueLower;
    public Action<object, float> FancySliderRange_SetValueLower;
    public Action<object, Action<float, float>> FancySliderRange_SetOnChangeR;

    public Func<string[], int, Action<int>, object> FancySwitch_New;
    public Func<object, int> FancySwitch_GetIndex;
    public Action<object, int> FancySwitch_SetIndex;
    public Func<object, string[]> FancySwitch_GetTabNames;
    public Func<object, int, string> FancySwitch_GetTabName;
    public Action<object, int, string> FancySwitch_SetTabName;
    public Action<object, Action<int>> FancySwitch_SetOnChange;

    public Func<string, Action<string>, object> FancyTextField_New;
    public Func<object, string> FancyTextField_GetText;
    public Action<object, string> FancyTextField_SetText;
    public Action<object, Action<string>> FancyTextField_SetOnChange;
    public Func<object, bool> FancyTextField_GetIsNumeric;
    public Action<object, bool> FancyTextField_SetIsNumeric;
    public Func<object, bool> FancyTextField_GetIsInteger;
    public Action<object, bool> FancyTextField_SetIsInteger;
    public Func<object, bool> FancyTextField_GetAllowNegative;
    public Action<object, bool> FancyTextField_SetAllowNegative;
    public Func<object, TextAlignment> FancyTextField_GetAlignment;
    public Action<object, TextAlignment> FancyTextField_SetAlignment;

    public Func<string, object> FancyWindowBar_New;
    public Func<object, string> FancyWindowBar_GetText;
    public Action<object, string> FancyWindowBar_SetText;
  }

  public class DelegatorBase
  {
    static protected TouchScreenAPI Api;
    internal static void SetApi(TouchScreenAPI api) => Api = api;
  }
  public class TouchScreen : DelegatorBase
  {
    internal object internalObj;
    public TouchScreen(object internalObject) { internalObj = internalObject; }
    public IMyCubeBlock GetBlock() => Api.TouchScreen_GetBlock.Invoke(internalObj);
    public IMyTextSurface GetSurface() => Api.TouchScreen_GetSurface.Invoke(internalObj);
    public int GetIndex() => Api.TouchScreen_GetIndex.Invoke(internalObj);
    public bool IsOnScreen() => Api.TouchScreen_IsOnScreen.Invoke(internalObj);
    public Vector2 GetCursorPosition() => Api.TouchScreen_GetCursorPosition.Invoke(internalObj);
    public float GetInteractiveDistance() => Api.TouchScreen_GetInteractiveDistance.Invoke(internalObj);
    public void SetInteractiveDistance(float distance) => Api.TouchScreen_SetInteractiveDistance.Invoke(internalObj, distance);
    public int GetScreenRotate() => Api.TouchScreen_GetScreenRotate.Invoke(internalObj);
    public bool CompareWithBlockAndSurface(IMyCubeBlock block, IMyTextSurface surface) => Api.TouchScreen_CompareWithBlockAndSurface.Invoke(internalObj, block, surface);
    public void Dispose() => Api.TouchScreen_Dispose.Invoke(internalObj);
  }
  public class ClickHandler : DelegatorBase
  {
    internal object internalObj;
    public ClickHandler() { internalObj = Api.ClickHandler_New(); }
    public ClickHandler(object internalObject) { internalObj = internalObject; }
    public Vector4 GetHitArea() => Api.ClickHandler_GetHitArea.Invoke(internalObj);
    public void SetHitArea(Vector4 hitArea) => Api.ClickHandler_SetHitArea.Invoke(internalObj, hitArea);
    public bool IsMouseReleased() => Api.ClickHandler_IsMouseReleased.Invoke(internalObj);
    public bool IsMouseOver() => Api.ClickHandler_IsMouseOver.Invoke(internalObj);
    public bool IsMousePressed() => Api.ClickHandler_IsMousePressed.Invoke(internalObj);
    public bool JustReleased() => Api.ClickHandler_JustReleased.Invoke(internalObj);
    public bool JustPressed() => Api.ClickHandler_JustPressed.Invoke(internalObj);
    public void UpdateStatus(TouchScreen screen) => Api.ClickHandler_UpdateStatus.Invoke(internalObj, screen.internalObj);
  }
  public class FancyCursor : DelegatorBase
  {
    internal object internalObj;
    public FancyCursor(TouchScreen screen) { internalObj = Api.FancyCursor_New(screen.internalObj); }
    public FancyCursor(object internalObject) { internalObj = internalObject; }
    public bool GetActive() => Api.FancyCursor_GetActive.Invoke(internalObj);
    public void SetActive(bool active) => Api.FancyCursor_SetActive.Invoke(internalObj, active);
    public Vector2 GetPosition() => Api.FancyCursor_GetPosition.Invoke(internalObj);
    public bool IsInsideArea(float x, float y, float z, float w) => Api.FancyCursor_IsInsideArea.Invoke(internalObj, x, y, z, w);
    public List<MySprite> GetSprites() => Api.FancyCursor_GetSprites.Invoke(internalObj);
    public void Dispose() => Api.FancyCursor_Dispose.Invoke(internalObj);
  }
  public class FancyTheme : DelegatorBase
  {
    internal object internalObj;
    public FancyTheme(object internalObject) { internalObj = internalObject; }
    public Color GetColorBg() => Api.FancyTheme_GetColorBg.Invoke(internalObj);
    public Color GetColorWhite() => Api.FancyTheme_GetColorWhite.Invoke(internalObj);
    public Color GetColorMain() => Api.FancyTheme_GetColorMain.Invoke(internalObj);
    public Color GetColorMainDarker(int value) => Api.FancyTheme_GetColorMainDarker.Invoke(internalObj, value);
    public Vector2 MeasureStringInPixels(String text, string font, float scale) => Api.FancyTheme_MeasureStringInPixels.Invoke(internalObj, text, font, scale);
    public float GetScale() => Api.FancyTheme_GetScale.Invoke(internalObj);
    public void SetScale(float scale) => Api.FancyTheme_SetScale.Invoke(internalObj, scale);
  }
  public class FancyElementBase : DelegatorBase
  {
    protected FancyApp App;
    protected FancyElementContainerBase Parent;
    internal object internalObj;
    public FancyElementBase(object internalObject) { internalObj = internalObject; }
    public bool GetEnabled() => Api.FancyElementBase_GetEnabled.Invoke(internalObj);
    public void SetEnabled(bool enabled) => Api.FancyElementBase_SetEnabled.Invoke(internalObj, enabled);
    public Vector2 GetPosition() => Api.FancyElementBase_GetPosition.Invoke(internalObj);
    public void SetPosition(Vector2 position) => Api.FancyElementBase_SetPosition.Invoke(internalObj, position);
    public Vector4 GetMargin() => Api.FancyElementBase_GetMargin.Invoke(internalObj);
    public void SetMargin(Vector4 margin) => Api.FancyElementBase_SetMargin.Invoke(internalObj, margin);
    public Vector2 GetScale() => Api.FancyElementBase_GetScale.Invoke(internalObj);
    public void SetScale(Vector2 scale) => Api.FancyElementBase_SetScale.Invoke(internalObj, scale);
    public Vector2 GetPixels() => Api.FancyElementBase_GetPixels.Invoke(internalObj);
    public void SetPixels(Vector2 pixels) => Api.FancyElementBase_SetPixels.Invoke(internalObj, pixels);
    public Vector2 GetSize() => Api.FancyElementBase_GetSize.Invoke(internalObj);
    public FancyApp GetApp() { if (App == null) App = new FancyApp(Api.FancyElementBase_GetApp.Invoke(internalObj)); return App; }
    public FancyElementContainerBase GetParent() { if (Parent == null) Parent = new FancyApp(Api.FancyElementBase_GetParent.Invoke(internalObj)); return Parent; }
    public List<MySprite> GetSprites() => Api.FancyElementBase_GetSprites.Invoke(internalObj);
    public void InitElements() => Api.FancyElementBase_InitElements.Invoke(internalObj);
    public void Update() => Api.FancyElementBase_Update.Invoke(internalObj);
    public void Dispose() => Api.FancyElementBase_Dispose.Invoke(internalObj);
  }
  public class FancyElementContainerBase : FancyElementBase
  {
    public FancyElementContainerBase(object internalObject) : base(internalObject) { }
    public List<object> GetChildren() => Api.FancyElementContainerBase_GetChildren.Invoke(internalObj);
    public void AddChild(FancyElementBase child) => Api.FancyElementContainerBase_AddChild.Invoke(internalObj, child.internalObj);
    public void RemoveChild(FancyElementBase child) => Api.FancyElementContainerBase_RemoveChild.Invoke(internalObj, child.internalObj);
  }
  public class FancyView : FancyElementContainerBase
  {
    public enum ViewDirection : int { None = 0, Row = 1, Column = 2 }
    public FancyView(ViewDirection direction = ViewDirection.Column) : base(Api.FancyView_NewV((int)direction)) { }
    public FancyView(object internalObject) : base(internalObject) { }
    public ViewDirection GetDirection() => (ViewDirection)Api.FancyView_GetDirection.Invoke(internalObj);
    public void SetDirection(ViewDirection direction) => Api.FancyView_SetDirection.Invoke(internalObj, (int)direction);
    public Color GetBgColor() => Api.FancyView_GetBgColor.Invoke(internalObj);
    public void SetBgColor(Color bgColor) => Api.FancyView_SetBgColor.Invoke(internalObj, bgColor);
    public Color GetBorderColor() => Api.FancyView_GetBorderColor.Invoke(internalObj);
    public void SetBorderColor(Color borderColor) => Api.FancyView_SetBorderColor.Invoke(internalObj, borderColor);
    public Vector4 GetBorder() => Api.FancyView_GetBorder.Invoke(internalObj);
    public void SetBorder(Vector4 border) => Api.FancyView_SetBorder.Invoke(internalObj, border);
    public Vector4 GetPadding() => Api.FancyView_GetPadding.Invoke(internalObj);
    public void SetPadding(Vector4 padding) => Api.FancyView_SetPadding.Invoke(internalObj, padding);
    public int GetGap() => Api.FancyView_GetGap.Invoke(internalObj);
    public void SetGap(int gap) => Api.FancyView_SetGap.Invoke(internalObj, gap);
  }
  public class FancyApp : FancyView
  {
    protected TouchScreen Screen;
    protected FancyCursor Cursor;
    protected FancyTheme Theme;
    public FancyApp() : base(Api.FancyApp_New()) { }
    public FancyApp(object internalObject) : base(internalObject) { }
    public TouchScreen GetScreen() { if (Screen == null) Screen = new TouchScreen(Api.FancyApp_GetScreen.Invoke(internalObj)); return Screen; }
    public RectangleF GetViewport() => Api.FancyApp_GetViewport.Invoke(internalObj);
    public FancyCursor GetCursor() { if (Cursor == null) Cursor = new FancyCursor(Api.FancyApp_GetCursor.Invoke(internalObj)); return Cursor; }
    public FancyTheme GetTheme() { if (Theme == null) Theme = new FancyTheme(Api.FancyApp_GetTheme.Invoke(internalObj)); return Theme; }
    public void InitApp(MyCubeBlock block, Sandbox.ModAPI.Ingame.IMyTextSurface surface) => Api.FancyApp_InitApp.Invoke(internalObj, block, surface);
  }
  public class FancyButtonBase : FancyElementBase
  {
    protected ClickHandler Handler;
    public FancyButtonBase(object internalObject) : base(internalObject) { }
    public ClickHandler GetHandler() { if (Handler == null) Handler = new ClickHandler(Api.FancyButtonBase_GetHandler.Invoke(internalObj)); return Handler; }
  }
  public class FancyButton : FancyButtonBase
  {
    public FancyButton(string text, Action onChange) : base(Api.FancyButton_New(text, onChange)) { }
    public FancyButton(object internalObject) : base(internalObject) { }
    public string GetText() => Api.FancyButton_GetText.Invoke(internalObj);
    public void SetText(string text) => Api.FancyButton_SetText.Invoke(internalObj, text);
    public void SetOnChange(Action onChange) => Api.FancyButton_SetOnChange.Invoke(internalObj, onChange);
    public TextAlignment GetAlignment() => Api.FancyButton_GetAlignment.Invoke(internalObj);
    public void SetAlignment(TextAlignment alignment) => Api.FancyButton_SetAlignment.Invoke(internalObj, alignment);
  }
  public class FancyLabel : FancyElementBase
  {
    public FancyLabel(string text, float fontSize = 0.5f, TextAlignment alignment = TextAlignment.CENTER) : base(Api.FancyLabel_New(text, fontSize, alignment)) { }
    public FancyLabel(object internalObject) : base(internalObject) { }
    public string GetText() => Api.FancyLabel_GetText.Invoke(internalObj);
    public void SetText(string text) => Api.FancyLabel_SetText.Invoke(internalObj, text);
    public void SetFontSize(float fontSize) => Api.FancyLabel_SetFontSize.Invoke(internalObj, fontSize);
    public TextAlignment GetAlignment() => Api.FancyLabel_GetAlignment.Invoke(internalObj);
    public void SetAlignment(TextAlignment alignment) => Api.FancyLabel_SetAlignment.Invoke(internalObj, alignment);
  }
  public class FancyProgressBar : FancyElementBase
  {
    public FancyProgressBar(float min, float max, bool bars = true) : base(Api.FancyProgressBar_New(min, max, bars)) { }
    public FancyProgressBar(object internalObject) : base(internalObject) { }
    public float GetValue() => Api.FancyProgressBar_GetValue.Invoke(internalObj);
    public void SetValue(float value) => Api.FancyProgressBar_SetValue.Invoke(internalObj, value);
  }
  public class FancySelector : FancyButtonBase
  {
    public FancySelector(List<string> labels, Action<int, string> onChange, bool loop = true) : base(Api.FancySelector_New(labels, onChange, loop)) { }
    public FancySelector(object internalObject) : base(internalObject) { }
    public void SetOnChange(Action<int, string> onChange) => Api.FancySelector_SetOnChange.Invoke(internalObj, onChange);
  }
  public class FancySeparator : FancyElementBase
  {
    public FancySeparator() : base(Api.FancySeparator_New()) { }
    public FancySeparator(object internalObject) : base(internalObject) { }
  }
  public class FancySlider : FancyButtonBase
  {
    public FancySlider(float min, float max, Action<float> onChange) : base(Api.FancySlider_New(min, max, onChange)) { }
    public FancySlider(object internalObject) : base(internalObject) { }
    public Vector2 GetRange() => Api.FancySlider_GetRange.Invoke(internalObj);
    public void SetRange(Vector2 range) => Api.FancySlider_SetRange.Invoke(internalObj, range);
    public float GetValue() => Api.FancySlider_GetValue.Invoke(internalObj);
    public void SetValue(float value) => Api.FancySlider_SetValue.Invoke(internalObj, value);
    public void SetOnChange(Action<float> onChange) => Api.FancySlider_SetOnChange.Invoke(internalObj, onChange);
    public bool GetIsInteger() => Api.FancySlider_GetIsInteger.Invoke(internalObj);
    public void SetIsInteger(bool isInterger) => Api.FancySlider_SetIsInteger.Invoke(internalObj, isInterger);
    public bool GetAllowInput() => Api.FancySlider_GetAllowInput.Invoke(internalObj);
    public void SetAllowInput(bool allowInput) => Api.FancySlider_SetAllowInput.Invoke(internalObj, allowInput);
  }
  public class FancySliderRange : FancySlider
  {
    public FancySliderRange(float min, float max, Action<float, float> onChange) : base(Api.FancySliderRange_NewR(min, max, onChange)) { }
    public FancySliderRange(object internalObject) : base(internalObject) { }
    public float GetValueLower() => Api.FancySliderRange_GetValueLower.Invoke(internalObj);
    public void SetValueLower(float value) => Api.FancySliderRange_SetValueLower.Invoke(internalObj, value);
    public void SetOnChangeRange(Action<float, float> onChange) => Api.FancySliderRange_SetOnChangeR.Invoke(internalObj, onChange);
  }
  public class FancySwitch : FancyButtonBase
  {
    public FancySwitch(string[] tabNames, int index = 0, Action<int> onChange = null) : base(Api.FancySwitch_New(tabNames, index, onChange)) { }
    public FancySwitch(object internalObject) : base(internalObject) { }
    public int GetIndex() => Api.FancySwitch_GetIndex.Invoke(internalObj);
    public void SetIndex(int index) => Api.FancySwitch_SetIndex.Invoke(internalObj, index);
    public string[] GetTabNames() => Api.FancySwitch_GetTabNames.Invoke(internalObj);
    public string GetTabName(int index) => Api.FancySwitch_GetTabName.Invoke(internalObj, index);
    public void SetTabName(int index, string text) => Api.FancySwitch_SetTabName.Invoke(internalObj, index, text);
    public void SetOnChange(Action<int> onChange) => Api.FancySwitch_SetOnChange.Invoke(internalObj, onChange);
  }
  public class FancyTextField : FancyButtonBase
  {
    public FancyTextField(string text, Action<string> onChange) : base(Api.FancyTextField_New(text, onChange)) { }
    public FancyTextField(object internalObject) : base(internalObject) { }
    public string GetText() => Api.FancyTextField_GetText.Invoke(internalObj);
    public void SetText(string text) => Api.FancyTextField_SetText.Invoke(internalObj, text);
    public void SetOnChange(Action<string> onChange) => Api.FancyTextField_SetOnChange.Invoke(internalObj, onChange);
    public bool GetIsNumeric() => Api.FancyTextField_GetIsNumeric.Invoke(internalObj);
    public void SetIsNumeric(bool isNumeric) => Api.FancyTextField_SetIsNumeric.Invoke(internalObj, isNumeric);
    public bool GetIsInteger() => Api.FancyTextField_GetIsInteger.Invoke(internalObj);
    public void SetIsInteger(bool isInterger) => Api.FancyTextField_SetIsInteger.Invoke(internalObj, isInterger);
    public bool GetAllowNegative() => Api.FancyTextField_GetAllowNegative.Invoke(internalObj);
    public void SetAllowNegative(bool allowNegative) => Api.FancyTextField_SetAllowNegative.Invoke(internalObj, allowNegative);
    public TextAlignment GetAlignment() => Api.FancyTextField_GetAlignment.Invoke(internalObj);
    public void SetAlignment(TextAlignment alignment) => Api.FancyTextField_SetAlignment.Invoke(internalObj, alignment);
  }
  public class FancyWindowBar : FancyElementBase
  {
    public FancyWindowBar(string text) : base(Api.FancyWindowBar_New(text)) { }
    public FancyWindowBar(object internalObject) : base(internalObject) { }
    public string GetText() => Api.FancyWindowBar_GetText.Invoke(internalObj);
    public void SetText(string text) => Api.FancyWindowBar_SetText.Invoke(internalObj, text);
  }
}