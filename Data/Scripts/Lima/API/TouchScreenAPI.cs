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
      AssignMethod(delegates, "FancyElementBase_GetPosition", ref FancyElementBase_GetPosition);
      AssignMethod(delegates, "FancyElementBase_SetPosition", ref FancyElementBase_SetPosition);
      AssignMethod(delegates, "FancyElementBase_GetMargin", ref FancyElementBase_GetMargin);
      AssignMethod(delegates, "FancyElementBase_SetMargin", ref FancyElementBase_SetMargin);
      AssignMethod(delegates, "FancyElementBase_GetScale", ref FancyElementBase_GetScale);
      AssignMethod(delegates, "FancyElementBase_SetScale", ref FancyElementBase_SetScale);
      AssignMethod(delegates, "FancyElementBase_GetPixels", ref FancyElementBase_GetPixels);
      AssignMethod(delegates, "FancyElementBase_SetPixels", ref FancyElementBase_SetPixels);
      AssignMethod(delegates, "FancyElementBase_GetSize", ref FancyElementBase_GetSize);
      AssignMethod(delegates, "FancyElementBase_GetViewport", ref FancyElementBase_GetViewport);
      AssignMethod(delegates, "FancyElementBase_GetApp", ref FancyElementBase_GetApp);
      AssignMethod(delegates, "FancyElementBase_GetParent", ref FancyElementBase_GetParent);
      AssignMethod(delegates, "FancyElementBase_GetOffset", ref FancyElementBase_GetOffset);
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
      AssignMethod(delegates, "FancyApp_New", ref FancyApp_New);
      AssignMethod(delegates, "FancyApp_GetScreen", ref FancyApp_GetScreen);
      AssignMethod(delegates, "FancyApp_GetCursor", ref FancyApp_GetCursor);
      AssignMethod(delegates, "FancyApp_GetTheme", ref FancyApp_GetTheme);
      AssignMethod(delegates, "FancyApp_InitApp", ref FancyApp_InitApp);
      AssignMethod(delegates, "FancyButtonBase_GetHandler", ref FancyButtonBase_GetHandler);
      AssignMethod(delegates, "FancyButton_New", ref FancyButton_New);
      AssignMethod(delegates, "FancyButton_GetText", ref FancyButton_GetText);
      AssignMethod(delegates, "FancyButton_SetText", ref FancyButton_SetText);
      AssignMethod(delegates, "FancyButton_SetAction", ref FancyButton_SetAction);
      AssignMethod(delegates, "FancyButton_GetAlignment", ref FancyButton_GetAlignment);
      AssignMethod(delegates, "FancyButton_SetAlignment", ref FancyButton_SetAlignment);
      AssignMethod(delegates, "FancyLabel_New", ref FancyLabel_New);
      AssignMethod(delegates, "FancyLabel_GetText", ref FancyLabel_GetText);
      AssignMethod(delegates, "FancyLabel_SetText", ref FancyLabel_SetText);
      AssignMethod(delegates, "FancyLabel_SetFontSize", ref FancyLabel_SetFontSize);
      AssignMethod(delegates, "FancyLabel_GetAlignment", ref FancyLabel_GetAlignment);
      AssignMethod(delegates, "FancyLabel_SetAlignment", ref FancyLabel_SetAlignment);
      AssignMethod(delegates, "FancyPanel_New", ref FancyPanel_New);
      AssignMethod(delegates, "FancyProgressBar_New", ref FancyProgressBar_New);
      AssignMethod(delegates, "FancyProgressBar_GetValue", ref FancyProgressBar_GetValue);
      AssignMethod(delegates, "FancyProgressBar_SetValue", ref FancyProgressBar_SetValue);
      AssignMethod(delegates, "FancySelector_New", ref FancySelector_New);
      AssignMethod(delegates, "FancySelector_SetAction", ref FancySelector_SetAction);
      AssignMethod(delegates, "FancySeparator_New", ref FancySeparator_New);
      AssignMethod(delegates, "FancySlider_New", ref FancySlider_New);
      AssignMethod(delegates, "FancySlider_GetRange", ref FancySlider_GetRange);
      AssignMethod(delegates, "FancySlider_SetRange", ref FancySlider_SetRange);
      AssignMethod(delegates, "FancySlider_GetValue", ref FancySlider_GetValue);
      AssignMethod(delegates, "FancySlider_SetValue", ref FancySlider_SetValue);
      AssignMethod(delegates, "FancySlider_SetAction", ref FancySlider_SetAction);
      AssignMethod(delegates, "FancySlider_GetIsInteger", ref FancySlider_GetIsInteger);
      AssignMethod(delegates, "FancySlider_SetIsInteger", ref FancySlider_SetIsInteger);
      AssignMethod(delegates, "FancySlider_GetAllowInput", ref FancySlider_GetAllowInput);
      AssignMethod(delegates, "FancySlider_SetAllowInput", ref FancySlider_SetAllowInput);
      AssignMethod(delegates, "FancySliderRange_NewR", ref FancySliderRange_NewR);
      AssignMethod(delegates, "FancySliderRange_GetValueLower", ref FancySliderRange_GetValueLower);
      AssignMethod(delegates, "FancySliderRange_SetValueLower", ref FancySliderRange_SetValueLower);
      AssignMethod(delegates, "FancySliderRange_SetActionR", ref FancySliderRange_SetActionR);
      AssignMethod(delegates, "FancySwitch_New", ref FancySwitch_New);
      AssignMethod(delegates, "FancySwitch_GetTextOn", ref FancySwitch_GetTextOn);
      AssignMethod(delegates, "FancySwitch_SetTextOn", ref FancySwitch_SetTextOn);
      AssignMethod(delegates, "FancySwitch_GetTextOff", ref FancySwitch_GetTextOff);
      AssignMethod(delegates, "FancySwitch_SetTextOff", ref FancySwitch_SetTextOff);
      AssignMethod(delegates, "FancySwitch_GetValue", ref FancySwitch_GetValue);
      AssignMethod(delegates, "FancySwitch_SetValue", ref FancySwitch_SetValue);
      AssignMethod(delegates, "FancySwitch_SetAction", ref FancySwitch_SetAction);
      AssignMethod(delegates, "FancyTextField_New", ref FancyTextField_New);
      AssignMethod(delegates, "FancyTextField_GetText", ref FancyTextField_GetText);
      AssignMethod(delegates, "FancyTextField_SetText", ref FancyTextField_SetText);
      AssignMethod(delegates, "FancyTextField_SetAction", ref FancyTextField_SetAction);
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

    public Func<object, Vector2> FancyElementBase_GetPosition;
    public Action<object, Vector2> FancyElementBase_SetPosition;
    public Func<object, Vector4> FancyElementBase_GetMargin;
    public Action<object, Vector4> FancyElementBase_SetMargin;
    public Func<object, Vector2> FancyElementBase_GetScale;
    public Action<object, Vector2> FancyElementBase_SetScale;
    public Func<object, Vector2> FancyElementBase_GetPixels;
    public Action<object, Vector2> FancyElementBase_SetPixels;
    public Func<object, Vector2> FancyElementBase_GetSize;
    public Func<object, RectangleF> FancyElementBase_GetViewport;
    public Func<object, object> FancyElementBase_GetApp;
    public Func<object, object> FancyElementBase_GetParent;
    public Func<object, Vector2> FancyElementBase_GetOffset;
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

    public Func<object> FancyApp_New;
    public Func<object, object> FancyApp_GetScreen;
    public Func<object, object> FancyApp_GetCursor;
    public Func<object, object> FancyApp_GetTheme;
    public Action<object, MyCubeBlock, Sandbox.ModAPI.Ingame.IMyTextSurface> FancyApp_InitApp;

    public Func<object, object> FancyButtonBase_GetHandler;

    public Func<string, Action, object> FancyButton_New;
    public Func<object, string> FancyButton_GetText;
    public Action<object, string> FancyButton_SetText;
    public Action<object, Action> FancyButton_SetAction;
    public Func<object, TextAlignment> FancyButton_GetAlignment;
    public Action<object, TextAlignment> FancyButton_SetAlignment;

    public Func<string, float, object> FancyLabel_New;
    public Func<object, string> FancyLabel_GetText;
    public Action<object, string> FancyLabel_SetText;
    public Action<object, float> FancyLabel_SetFontSize;
    public Func<object, TextAlignment> FancyLabel_GetAlignment;
    public Action<object, TextAlignment> FancyLabel_SetAlignment;

    public Func<object> FancyPanel_New;

    public Func<float, float, bool, object> FancyProgressBar_New;
    public Func<object, float> FancyProgressBar_GetValue;
    public Action<object, float> FancyProgressBar_SetValue;

    public Func<List<string>, Action<int, string>, bool, object> FancySelector_New;
    public Action<object, Action<int, string>> FancySelector_SetAction;

    public Func<object> FancySeparator_New;

    public Func<float, float, Action<float>, object> FancySlider_New;
    public Func<object, Vector2> FancySlider_GetRange;
    public Action<object, Vector2> FancySlider_SetRange;
    public Func<object, float> FancySlider_GetValue;
    public Action<object, float> FancySlider_SetValue;
    public Action<object, Action<float>> FancySlider_SetAction;
    public Func<object, bool> FancySlider_GetIsInteger;
    public Action<object, bool> FancySlider_SetIsInteger;
    public Func<object, bool> FancySlider_GetAllowInput;
    public Action<object, bool> FancySlider_SetAllowInput;

    public Func<float, float, Action<float, float>, object> FancySliderRange_NewR;
    public Func<object, float> FancySliderRange_GetValueLower;
    public Action<object, float> FancySliderRange_SetValueLower;
    public Action<object, Action<float, float>> FancySliderRange_SetActionR;

    public Func<Action<bool>, string, string, object> FancySwitch_New;
    public Func<object, string> FancySwitch_GetTextOn;
    public Action<object, string> FancySwitch_SetTextOn;
    public Func<object, string> FancySwitch_GetTextOff;
    public Action<object, string> FancySwitch_SetTextOff;
    public Func<object, bool> FancySwitch_GetValue;
    public Action<object, bool> FancySwitch_SetValue;
    public Action<object, Action<bool>> FancySwitch_SetAction;

    public Func<string, Action<string>, object> FancyTextField_New;
    public Func<object, string> FancyTextField_GetText;
    public Action<object, string> FancyTextField_SetText;
    public Action<object, Action<string>> FancyTextField_SetAction;
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
    static protected TouchScreenAPI _api;
    internal static void SetApi(TouchScreenAPI api) => _api = api;
  }
  public class TouchScreen : DelegatorBase
  {
    internal object internalObj;
    public TouchScreen(object internalObject) { internalObj = internalObject; }
    public IMyCubeBlock GetBlock() => _api.TouchScreen_GetBlock.Invoke(internalObj);
    public IMyTextSurface GetSurface() => _api.TouchScreen_GetSurface.Invoke(internalObj);
    public int GetIndex() => _api.TouchScreen_GetIndex.Invoke(internalObj);
    public bool IsOnScreen() => _api.TouchScreen_IsOnScreen.Invoke(internalObj);
    public Vector2 GetCursorPosition() => _api.TouchScreen_GetCursorPosition.Invoke(internalObj);
    public float GetInteractiveDistance() => _api.TouchScreen_GetInteractiveDistance.Invoke(internalObj);
    public void SetInteractiveDistance(float distance) => _api.TouchScreen_SetInteractiveDistance.Invoke(internalObj, distance);
    public int GetScreenRotate() => _api.TouchScreen_GetScreenRotate.Invoke(internalObj);
    public bool CompareWithBlockAndSurface(IMyCubeBlock block, IMyTextSurface surface) => _api.TouchScreen_CompareWithBlockAndSurface.Invoke(internalObj, block, surface);
    public void Dispose() => _api.TouchScreen_Dispose.Invoke(internalObj);
  }
  public class ClickHandler : DelegatorBase
  {
    internal object internalObj;
    public ClickHandler() { internalObj = _api.ClickHandler_New(); }
    public ClickHandler(object internalObject) { internalObj = internalObject; }
    public Vector4 GetHitArea() => _api.ClickHandler_GetHitArea.Invoke(internalObj);
    public void SetHitArea(Vector4 hitArea) => _api.ClickHandler_SetHitArea.Invoke(internalObj, hitArea);
    public bool IsMouseReleased() => _api.ClickHandler_IsMouseReleased.Invoke(internalObj);
    public bool IsMouseOver() => _api.ClickHandler_IsMouseOver.Invoke(internalObj);
    public bool IsMousePressed() => _api.ClickHandler_IsMousePressed.Invoke(internalObj);
    public bool JustReleased() => _api.ClickHandler_JustReleased.Invoke(internalObj);
    public bool JustPressed() => _api.ClickHandler_JustPressed.Invoke(internalObj);
    public void UpdateStatus(TouchScreen screen) => _api.ClickHandler_UpdateStatus.Invoke(internalObj, screen.internalObj);
  }
  public class FancyCursor : DelegatorBase
  {
    internal object internalObj;
    public FancyCursor(TouchScreen screen) { internalObj = _api.FancyCursor_New(screen.internalObj); }
    public FancyCursor(object internalObject) { internalObj = internalObject; }
    public bool GetActive() => _api.FancyCursor_GetActive.Invoke(internalObj);
    public void SetActive(bool active) => _api.FancyCursor_SetActive.Invoke(internalObj, active);
    public Vector2 GetPosition() => _api.FancyCursor_GetPosition.Invoke(internalObj);
    public bool IsInsideArea(float x, float y, float z, float w) => _api.FancyCursor_IsInsideArea.Invoke(internalObj, x, y, z, w);
    public List<MySprite> GetSprites() => _api.FancyCursor_GetSprites.Invoke(internalObj);
    public void Dispose() => _api.FancyCursor_Dispose.Invoke(internalObj);
  }
  public class FancyTheme : DelegatorBase
  {
    internal object internalObj;
    public FancyTheme(object internalObject) { internalObj = internalObject; }
    public Color GetColorBg() => _api.FancyTheme_GetColorBg.Invoke(internalObj);
    public Color GetColorWhite() => _api.FancyTheme_GetColorWhite.Invoke(internalObj);
    public Color GetColorMain() => _api.FancyTheme_GetColorMain.Invoke(internalObj);
    public Color GetColorMainDarker(int value) => _api.FancyTheme_GetColorMainDarker.Invoke(internalObj, value);
    public Vector2 MeasureStringInPixels(String text, string font, float scale) => _api.FancyTheme_MeasureStringInPixels.Invoke(internalObj, text, font, scale);
    public float GetScale() => _api.FancyTheme_GetScale.Invoke(internalObj);
    public void SetScale(float scale) => _api.FancyTheme_SetScale.Invoke(internalObj, scale);
  }
  public class FancyElementBase : DelegatorBase
  {
    protected FancyApp _app;
    protected FancyElementContainerBase _parent;
    internal object internalObj;
    public FancyElementBase(object internalObject) { internalObj = internalObject; }
    public Vector2 GetPosition() => _api.FancyElementBase_GetPosition.Invoke(internalObj);
    public void SetPosition(Vector2 position) => _api.FancyElementBase_SetPosition.Invoke(internalObj, position);
    public Vector4 GetMargin() => _api.FancyElementBase_GetMargin.Invoke(internalObj);
    public void SetMargin(Vector4 margin) => _api.FancyElementBase_SetMargin.Invoke(internalObj, margin);
    public Vector2 GetScale() => _api.FancyElementBase_GetScale.Invoke(internalObj);
    public void SetScale(Vector2 scale) => _api.FancyElementBase_SetScale.Invoke(internalObj, scale);
    public Vector2 GetPixels() => _api.FancyElementBase_GetPixels.Invoke(internalObj);
    public void SetPixels(Vector2 pixels) => _api.FancyElementBase_SetPixels.Invoke(internalObj, pixels);
    public Vector2 GetSize() => _api.FancyElementBase_GetSize.Invoke(internalObj);
    public RectangleF GetViewport() => _api.FancyElementBase_GetViewport.Invoke(internalObj);
    public FancyApp GetApp() { if (_app == null) _app = new FancyApp(_api.FancyElementBase_GetApp.Invoke(internalObj)); return _app; }
    public FancyElementContainerBase GetParent() { if (_parent == null) _parent = new FancyApp(_api.FancyElementBase_GetParent.Invoke(internalObj)); return _parent; }
    public Vector2 GetOffset() => _api.FancyElementBase_GetOffset.Invoke(internalObj);
    public List<MySprite> GetSprites() => _api.FancyElementBase_GetSprites.Invoke(internalObj);
    public void InitElements() => _api.FancyElementBase_InitElements.Invoke(internalObj);
    public void Update() => _api.FancyElementBase_Update.Invoke(internalObj);
    public void Dispose() => _api.FancyElementBase_Dispose.Invoke(internalObj);
  }
  public class FancyElementContainerBase : FancyElementBase
  {
    public FancyElementContainerBase(object internalObject) : base(internalObject) { }
    public List<object> GetChildren() => _api.FancyElementContainerBase_GetChildren.Invoke(internalObj);
    public void AddChild(FancyElementBase child) => _api.FancyElementContainerBase_AddChild.Invoke(internalObj, child.internalObj);
    public void RemoveChild(FancyElementBase child) => _api.FancyElementContainerBase_RemoveChild.Invoke(internalObj, child.internalObj);
  }
  public class FancyView : FancyElementContainerBase
  {
    public enum ViewDirection : int { None = 0, Row = 1, Column = 2 }
    public FancyView(ViewDirection direction = ViewDirection.Column) : base(_api.FancyView_NewV((int)direction)) { }
    public FancyView(object internalObject) : base(internalObject) { }
    public ViewDirection GetDirection() => (ViewDirection)_api.FancyView_GetDirection.Invoke(internalObj);
    public void SetDirection(ViewDirection direction) => _api.FancyView_SetDirection.Invoke(internalObj, (int)direction);
  }
  public class FancyApp : FancyView
  {
    protected TouchScreen _screen;
    protected FancyCursor _cursor;
    protected FancyTheme _theme;
    public FancyApp() : base(_api.FancyApp_New()) { }
    public FancyApp(object internalObject) : base(internalObject) { }
    public TouchScreen GetScreen() { if (_screen == null) _screen = new TouchScreen(_api.FancyApp_GetScreen.Invoke(internalObj)); return _screen; }
    public FancyCursor GetCursor() { if (_cursor == null) _cursor = new FancyCursor(_api.FancyApp_GetCursor.Invoke(internalObj)); return _cursor; }
    public FancyTheme GetTheme() { if (_theme == null) _theme = new FancyTheme(_api.FancyApp_GetTheme.Invoke(internalObj)); return _theme; }
    public void InitApp(MyCubeBlock block, Sandbox.ModAPI.Ingame.IMyTextSurface surface) => _api.FancyApp_InitApp.Invoke(internalObj, block, surface);
  }
  public class FancyButtonBase : FancyElementBase
  {
    protected ClickHandler _handler;
    public FancyButtonBase(object internalObject) : base(internalObject) { }
    public ClickHandler GetHandler() { if (_handler == null) _handler = new ClickHandler(_api.FancyButtonBase_GetHandler.Invoke(internalObj)); return _handler; }
  }
  public class FancyButton : FancyButtonBase
  {
    public FancyButton(string text, Action action) : base(_api.FancyButton_New(text, action)) { }
    public FancyButton(object internalObject) : base(internalObject) { }
    public string GetText() => _api.FancyButton_GetText.Invoke(internalObj);
    public void SetText(string text) => _api.FancyButton_SetText.Invoke(internalObj, text);
    public void SetAction(Action action) => _api.FancyButton_SetAction.Invoke(internalObj, action);
    public TextAlignment GetAlignment() => _api.FancyButton_GetAlignment.Invoke(internalObj);
    public void SetAlignment(TextAlignment alignment) => _api.FancyButton_SetAlignment.Invoke(internalObj, alignment);
  }
  public class FancyLabel : FancyElementBase
  {
    public FancyLabel(string text, float fontSize = 0.5f) : base(_api.FancyLabel_New(text, fontSize)) { }
    public FancyLabel(object internalObject) : base(internalObject) { }
    public string GetText() => _api.FancyLabel_GetText.Invoke(internalObj);
    public void SetText(string text) => _api.FancyLabel_SetText.Invoke(internalObj, text);
    public void SetFontSize(float fontSize) => _api.FancyLabel_SetFontSize.Invoke(internalObj, fontSize);
    public TextAlignment GetAlignment() => _api.FancyLabel_GetAlignment.Invoke(internalObj);
    public void SetAlignment(TextAlignment alignment) => _api.FancyLabel_SetAlignment.Invoke(internalObj, alignment);
  }
  public class FancyPanel : FancyView
  {
    public FancyPanel() : base(_api.FancyPanel_New()) { }
    public FancyPanel(object internalObject) : base(internalObject) { }
  }
  public class FancyProgressBar : FancyElementBase
  {
    public FancyProgressBar(float min, float max, bool bars = true) : base(_api.FancyProgressBar_New(min, max, bars)) { }
    public FancyProgressBar(object internalObject) : base(internalObject) { }
    public float GetValue() => _api.FancyProgressBar_GetValue.Invoke(internalObj);
    public void SetValue(float value) => _api.FancyProgressBar_SetValue.Invoke(internalObj, value);
  }
  public class FancySelector : FancyButtonBase
  {
    public FancySelector(List<string> labels, Action<int, string> action, bool loop = true) : base(_api.FancySelector_New(labels, action, loop)) { }
    public FancySelector(object internalObject) : base(internalObject) { }
    public void SetAction(Action<int, string> action) => _api.FancySelector_SetAction.Invoke(internalObj, action);
  }
  public class FancySeparator : FancyElementBase
  {
    public FancySeparator() : base(_api.FancySeparator_New()) { }
    public FancySeparator(object internalObject) : base(internalObject) { }
  }
  public class FancySlider : FancyButtonBase
  {
    public FancySlider(float min, float max, Action<float> action) : base(_api.FancySlider_New(min, max, action)) { }
    public FancySlider(object internalObject) : base(internalObject) { }
    public Vector2 GetRange() => _api.FancySlider_GetRange.Invoke(internalObj);
    public void SetRange(Vector2 range) => _api.FancySlider_SetRange.Invoke(internalObj, range);
    public float GetValue() => _api.FancySlider_GetValue.Invoke(internalObj);
    public void SetValue(float value) => _api.FancySlider_SetValue.Invoke(internalObj, value);
    public void SetAction(Action<float> action) => _api.FancySlider_SetAction.Invoke(internalObj, action);
    public bool GetIsInteger() => _api.FancySlider_GetIsInteger.Invoke(internalObj);
    public void SetIsInteger(bool isInterger) => _api.FancySlider_SetIsInteger.Invoke(internalObj, isInterger);
    public bool GetAllowInput() => _api.FancySlider_GetAllowInput.Invoke(internalObj);
    public void SetAllowInput(bool allowInput) => _api.FancySlider_SetAllowInput.Invoke(internalObj, allowInput);
  }
  public class FancySliderRange : FancySlider
  {
    public FancySliderRange(float min, float max, Action<float, float> action) : base(_api.FancySliderRange_NewR(min, max, action)) { }
    public FancySliderRange(object internalObject) : base(internalObject) { }
    public float GetValueLower() => _api.FancySliderRange_GetValueLower.Invoke(internalObj);
    public void SetValueLower(float value) => _api.FancySliderRange_SetValueLower.Invoke(internalObj, value);
    public void SetActionRange(Action<float, float> action) => _api.FancySliderRange_SetActionR.Invoke(internalObj, action);
  }
  public class FancySwitch : FancyButtonBase
  {
    public FancySwitch(Action<bool> action, string textOn = "On", string textOff = "Off") : base(_api.FancySwitch_New(action, textOn, textOff)) { }
    public FancySwitch(object internalObject) : base(internalObject) { }
    public string GetTextOn() => _api.FancySwitch_GetTextOn.Invoke(internalObj);
    public void SetTextOn(string text) => _api.FancySwitch_SetTextOn.Invoke(internalObj, text);
    public string GetTextOff() => _api.FancySwitch_GetTextOff.Invoke(internalObj);
    public void SetTextOff(string text) => _api.FancySwitch_SetTextOff.Invoke(internalObj, text);
    public bool GetValue() => _api.FancySwitch_GetValue.Invoke(internalObj);
    public void SetValue(bool value) => _api.FancySwitch_SetValue.Invoke(internalObj, value);
    public void SetAction(Action<bool> action) => _api.FancySwitch_SetAction.Invoke(internalObj, action);
  }
  public class FancyTextField : FancyButtonBase
  {
    public FancyTextField(string text, Action<string> action) : base(_api.FancyTextField_New(text, action)) { }
    public FancyTextField(object internalObject) : base(internalObject) { }
    public string GetText() => _api.FancyTextField_GetText.Invoke(internalObj);
    public void SetText(string text) => _api.FancyTextField_SetText.Invoke(internalObj, text);
    public void SetAction(Action<string> action) => _api.FancyTextField_SetAction.Invoke(internalObj, action);
    public bool GetIsNumeric() => _api.FancyTextField_GetIsNumeric.Invoke(internalObj);
    public void SetIsNumeric(bool isNumeric) => _api.FancyTextField_SetIsNumeric.Invoke(internalObj, isNumeric);
    public bool GetIsInteger() => _api.FancyTextField_GetIsInteger.Invoke(internalObj);
    public void SetIsInteger(bool isInterger) => _api.FancyTextField_SetIsInteger.Invoke(internalObj, isInterger);
    public bool GetAllowNegative() => _api.FancyTextField_GetAllowNegative.Invoke(internalObj);
    public void SetAllowNegative(bool allowNegative) => _api.FancyTextField_SetAllowNegative.Invoke(internalObj, allowNegative);
    public TextAlignment GetAlignment() => _api.FancyTextField_GetAlignment.Invoke(internalObj);
    public void SetAlignment(TextAlignment alignment) => _api.FancyTextField_SetAlignment.Invoke(internalObj, alignment);
  }
  public class FancyWindowBar : FancyElementBase
  {
    public FancyWindowBar(string text) : base(_api.FancyWindowBar_New(text)) { }
    public FancyWindowBar(object internalObject) : base(internalObject) { }
    public string GetText() => _api.FancyWindowBar_GetText.Invoke(internalObj);
    public void SetText(string text) => _api.FancyWindowBar_SetText.Invoke(internalObj, text);
  }
}