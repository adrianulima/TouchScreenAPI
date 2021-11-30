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

    private Func<float> _getMaxInteractiveDistance;
    private Action<float> _setMaxInteractiveDistance;
    private Action<string> _addSurfaceCoords;
    private Action<string> _removeSurfaceCoords;
    private Func<IMyCubeBlock, IMyTextSurface, object> _createTouchScreen;
    private Action<IMyCubeBlock, IMyTextSurface> _removeTouchScreen;

    public float GetMaxInteractiveDistance() => _getMaxInteractiveDistance?.Invoke() ?? -1f;
    public void SetMaxInteractiveDistance(float distance) => _setMaxInteractiveDistance?.Invoke(distance);
    public void AddSurfaceCoords(string coords) => _addSurfaceCoords?.Invoke(coords);
    public void RemoveSurfaceCoords(string coords) => _removeSurfaceCoords?.Invoke(coords);
    public object CreateTouchScreen(IMyCubeBlock block, IMyTextSurface surface) => _createTouchScreen?.Invoke(block, surface);
    public void RemoveTouchScreen(IMyCubeBlock block, IMyTextSurface surface) => _removeTouchScreen?.Invoke(block, surface);

    public TouchScreenAPI() { }
    public bool Load()
    {
      if (!_isRegistered)
      {
        _isRegistered = true;
        MyAPIGateway.Utilities.RegisterMessageHandler(_channel, HandleMessage);
      }
      if (!IsReady && !_apiInit)
        MyAPIGateway.Utilities.SendModMessage(_channel, "ApiEndpointRequest");
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
    }
    public void HandleMessage(object msg)
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
      AssignMethod(delegates, "GetMaxInteractiveDistance", ref _getMaxInteractiveDistance);
      AssignMethod(delegates, "SetMaxInteractiveDistance", ref _setMaxInteractiveDistance);
      AssignMethod(delegates, "AddSurfaceCoords", ref _addSurfaceCoords);
      AssignMethod(delegates, "RemoveSurfaceCoords", ref _removeSurfaceCoords);
      AssignMethod(delegates, "CreateTouchScreen", ref _createTouchScreen);
      AssignMethod(delegates, "RemoveTouchScreen", ref _removeTouchScreen);
      AssignMethod(delegates, "TouchScreen_GetBlock", ref TouchScreen._getBlock);
      AssignMethod(delegates, "TouchScreen_GetSurface", ref TouchScreen._getSurface);
      AssignMethod(delegates, "TouchScreen_GetIndex", ref TouchScreen._getIndex);
      AssignMethod(delegates, "TouchScreen_IsOnScreen", ref TouchScreen._isOnScreen);
      AssignMethod(delegates, "TouchScreen_GetCursorPosition", ref TouchScreen._getCursorPosition);
      AssignMethod(delegates, "TouchScreen_GetInteractiveDistance", ref TouchScreen._getInteractiveDistance);
      AssignMethod(delegates, "TouchScreen_SetInteractiveDistance", ref TouchScreen._setInteractiveDistance);
      AssignMethod(delegates, "TouchScreen_GetScreenRotate", ref TouchScreen._getScreenRotate);
      AssignMethod(delegates, "TouchScreen_CompareWithBlockAndSurface", ref TouchScreen._compareWithBlockAndSurface);
      AssignMethod(delegates, "TouchScreen_Dispose", ref TouchScreen._dispose);
      AssignMethod(delegates, "ClickHandler_New", ref ClickHandler._new);
      AssignMethod(delegates, "ClickHandler_GetHitArea", ref ClickHandler._getHitArea);
      AssignMethod(delegates, "ClickHandler_SetHitArea", ref ClickHandler._setHitArea);
      AssignMethod(delegates, "ClickHandler_IsMouseReleased", ref ClickHandler._isMouseReleased);
      AssignMethod(delegates, "ClickHandler_IsMouseOver", ref ClickHandler._isMouseOver);
      AssignMethod(delegates, "ClickHandler_IsMousePressed", ref ClickHandler._isMousePressed);
      AssignMethod(delegates, "ClickHandler_JustReleased", ref ClickHandler._justReleased);
      AssignMethod(delegates, "ClickHandler_JustPressed", ref ClickHandler._justPressed);
      AssignMethod(delegates, "ClickHandler_UpdateStatus", ref ClickHandler._updateStatus);
      AssignMethod(delegates, "FancyCursor_New", ref FancyCursor._new);
      AssignMethod(delegates, "FancyCursor_GetActive", ref FancyCursor._getActive);
      AssignMethod(delegates, "FancyCursor_SetActive", ref FancyCursor._setActive);
      AssignMethod(delegates, "FancyCursor_GetPosition", ref FancyCursor._getPosition);
      AssignMethod(delegates, "FancyCursor_IsInsideArea", ref FancyCursor._isInsideArea);
      AssignMethod(delegates, "FancyCursor_GetSprites", ref FancyCursor._getSprites);
      AssignMethod(delegates, "FancyCursor_Dispose", ref FancyCursor._dispose);
      AssignMethod(delegates, "FancyTheme_GetColorBg", ref FancyTheme._getColorBg);
      AssignMethod(delegates, "FancyTheme_GetColorWhite", ref FancyTheme._getColorWhite);
      AssignMethod(delegates, "FancyTheme_GetColorMain", ref FancyTheme._getColorMain);
      AssignMethod(delegates, "FancyTheme_GetColorMainDarker", ref FancyTheme._getColorMainDarker);
      AssignMethod(delegates, "FancyTheme_MeasureStringInPixels", ref FancyTheme._measureStringInPixels);
      AssignMethod(delegates, "FancyTheme_GetScale", ref FancyTheme._getScale);
      AssignMethod(delegates, "FancyTheme_SetScale", ref FancyTheme._setScale);
      AssignMethod(delegates, "FancyElementBase_GetPosition", ref FancyElementBase._getPosition);
      AssignMethod(delegates, "FancyElementBase_SetPosition", ref FancyElementBase._setPosition);
      AssignMethod(delegates, "FancyElementBase_GetMargin", ref FancyElementBase._getMargin);
      AssignMethod(delegates, "FancyElementBase_SetMargin", ref FancyElementBase._setMargin);
      AssignMethod(delegates, "FancyElementBase_GetScale", ref FancyElementBase._getScale);
      AssignMethod(delegates, "FancyElementBase_SetScale", ref FancyElementBase._setScale);
      AssignMethod(delegates, "FancyElementBase_GetPixels", ref FancyElementBase._getPixels);
      AssignMethod(delegates, "FancyElementBase_SetPixels", ref FancyElementBase._setPixels);
      AssignMethod(delegates, "FancyElementBase_GetSize", ref FancyElementBase._getSize);
      AssignMethod(delegates, "FancyElementBase_GetViewport", ref FancyElementBase._getViewport);
      AssignMethod(delegates, "FancyElementBase_GetApp", ref FancyElementBase._getApp);
      AssignMethod(delegates, "FancyElementBase_GetParent", ref FancyElementBase._getParent);
      AssignMethod(delegates, "FancyElementBase_GetOffset", ref FancyElementBase._getOffset);
      AssignMethod(delegates, "FancyElementBase_GetSprites", ref FancyElementBase._getSprites);
      AssignMethod(delegates, "FancyElementBase_InitElements", ref FancyElementBase._initElements);
      AssignMethod(delegates, "FancyElementBase_Update", ref FancyElementBase._update);
      AssignMethod(delegates, "FancyElementBase_Dispose", ref FancyElementBase._dispose);
      AssignMethod(delegates, "FancyElementContainerBase_GetChildren", ref FancyElementContainerBase._getChildren);
      AssignMethod(delegates, "FancyElementContainerBase_AddChild", ref FancyElementContainerBase._addChild);
      AssignMethod(delegates, "FancyElementContainerBase_RemoveChild", ref FancyElementContainerBase._removeChild);
      AssignMethod(delegates, "FancyView_NewV", ref FancyView._newV);
      AssignMethod(delegates, "FancyView_GetDirection", ref FancyView._getDirection);
      AssignMethod(delegates, "FancyView_SetDirection", ref FancyView._setDirection);
      AssignMethod(delegates, "FancyApp_New", ref FancyApp._new);
      AssignMethod(delegates, "FancyApp_GetScreen", ref FancyApp._getScreen);
      AssignMethod(delegates, "FancyApp_GetCursor", ref FancyApp._getCursor);
      AssignMethod(delegates, "FancyApp_GetTheme", ref FancyApp._getTheme);
      AssignMethod(delegates, "FancyApp_InitApp", ref FancyApp._initApp);
      AssignMethod(delegates, "FancyButtonBase_GetHandler", ref FancyButtonBase._getHandler);
      AssignMethod(delegates, "FancyButton_New", ref FancyButton._new);
      AssignMethod(delegates, "FancyButton_GetText", ref FancyButton._getText);
      AssignMethod(delegates, "FancyButton_SetText", ref FancyButton._setText);
      AssignMethod(delegates, "FancyButton_SetAction", ref FancyButton._setAction);
      AssignMethod(delegates, "FancyButton_GetAlignment", ref FancyButton._getAlignment);
      AssignMethod(delegates, "FancyButton_SetAlignment", ref FancyButton._setAlignment);
      AssignMethod(delegates, "FancyLabel_New", ref FancyLabel._new);
      AssignMethod(delegates, "FancyLabel_GetText", ref FancyLabel._getText);
      AssignMethod(delegates, "FancyLabel_SetText", ref FancyLabel._setText);
      AssignMethod(delegates, "FancyLabel_SetFontSize", ref FancyLabel._setFontSize);
      AssignMethod(delegates, "FancyLabel_GetAlignment", ref FancyLabel._getAlignment);
      AssignMethod(delegates, "FancyLabel_SetAlignment", ref FancyLabel._setAlignment);
      AssignMethod(delegates, "FancyPanel_New", ref FancyPanel._new);
      AssignMethod(delegates, "FancyProgressBar_New", ref FancyProgressBar._new);
      AssignMethod(delegates, "FancyProgressBar_GetValue", ref FancyProgressBar._getValue);
      AssignMethod(delegates, "FancyProgressBar_SetValue", ref FancyProgressBar._setValue);
      AssignMethod(delegates, "FancySelector_New", ref FancySelector._new);
      AssignMethod(delegates, "FancySelector_SetAction", ref FancySelector._setAction);
      AssignMethod(delegates, "FancySeparator_New", ref FancySeparator._new);
      AssignMethod(delegates, "FancySlider_New", ref FancySlider._new);
      AssignMethod(delegates, "FancySlider_GetRange", ref FancySlider._getRange);
      AssignMethod(delegates, "FancySlider_SetRange", ref FancySlider._setRange);
      AssignMethod(delegates, "FancySlider_GetValue", ref FancySlider._getValue);
      AssignMethod(delegates, "FancySlider_SetValue", ref FancySlider._setValue);
      AssignMethod(delegates, "FancySlider_SetAction", ref FancySlider._setAction);
      AssignMethod(delegates, "FancySlider_GetIsInteger", ref FancySlider._getIsInteger);
      AssignMethod(delegates, "FancySlider_SetIsInteger", ref FancySlider._setIsInteger);
      AssignMethod(delegates, "FancySlider_GetAllowInput", ref FancySlider._getAllowInput);
      AssignMethod(delegates, "FancySlider_SetAllowInput", ref FancySlider._setAllowInput);
      AssignMethod(delegates, "FancySliderRange_NewR", ref FancySliderRange._newR);
      AssignMethod(delegates, "FancySliderRange_GetValueLower", ref FancySliderRange._getValueLower);
      AssignMethod(delegates, "FancySliderRange_SetValueLower", ref FancySliderRange._setValueLower);
      AssignMethod(delegates, "FancySliderRange_SetActionR", ref FancySliderRange._setActionR);
      AssignMethod(delegates, "FancySwitch_New", ref FancySwitch._new);
      AssignMethod(delegates, "FancySwitch_GetTextOn", ref FancySwitch._getTextOn);
      AssignMethod(delegates, "FancySwitch_SetTextOn", ref FancySwitch._setTextOn);
      AssignMethod(delegates, "FancySwitch_GetTextOff", ref FancySwitch._getTextOff);
      AssignMethod(delegates, "FancySwitch_SetTextOff", ref FancySwitch._setTextOff);
      AssignMethod(delegates, "FancySwitch_GetValue", ref FancySwitch._getValue);
      AssignMethod(delegates, "FancySwitch_SetValue", ref FancySwitch._setValue);
      AssignMethod(delegates, "FancySwitch_SetAction", ref FancySwitch._setAction);
      AssignMethod(delegates, "FancyTextField_New", ref FancyTextField._new);
      AssignMethod(delegates, "FancyTextField_GetText", ref FancyTextField._getText);
      AssignMethod(delegates, "FancyTextField_SetText", ref FancyTextField._setText);
      AssignMethod(delegates, "FancyTextField_SetAction", ref FancyTextField._setAction);
      AssignMethod(delegates, "FancyTextField_GetIsNumeric", ref FancyTextField._getIsNumeric);
      AssignMethod(delegates, "FancyTextField_SetIsNumeric", ref FancyTextField._setIsNumeric);
      AssignMethod(delegates, "FancyTextField_GetIsInteger", ref FancyTextField._getIsInteger);
      AssignMethod(delegates, "FancyTextField_SetIsInteger", ref FancyTextField._setIsInteger);
      AssignMethod(delegates, "FancyTextField_GetAllowNegative", ref FancyTextField._getAllowNegative);
      AssignMethod(delegates, "FancyTextField_SetAllowNegative", ref FancyTextField._setAllowNegative);
      AssignMethod(delegates, "FancyTextField_GetAlignment", ref FancyTextField._getAlignment);
      AssignMethod(delegates, "FancyTextField_SetAlignment", ref FancyTextField._setAlignment);
      AssignMethod(delegates, "FancyWindowBar_New", ref FancyWindowBar._new);
      AssignMethod(delegates, "FancyWindowBar_GetText", ref FancyWindowBar._getText);
      AssignMethod(delegates, "FancyWindowBar_SetText", ref FancyWindowBar._setText);
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
  }
  public class TouchScreen
  {
    static public Func<object, IMyCubeBlock> _getBlock;
    static public Func<object, IMyTextSurface> _getSurface;
    static public Func<object, int> _getIndex;
    static public Func<object, bool> _isOnScreen;
    static public Func<object, Vector2> _getCursorPosition;
    static public Func<object, float> _getInteractiveDistance;
    static public Action<object, float> _setInteractiveDistance;
    static public Func<object, int> _getScreenRotate;
    static public Func<object, IMyCubeBlock, IMyTextSurface, bool> _compareWithBlockAndSurface;
    static public Action<object> _dispose;
    internal object _internalObj;
    public TouchScreen(object internalObject) { _internalObj = internalObject; }
    public IMyCubeBlock GetBlock() => _getBlock.Invoke(_internalObj);
    public IMyTextSurface GetSurface() => _getSurface.Invoke(_internalObj);
    public int GetIndex() => _getIndex.Invoke(_internalObj);
    public bool IsOnScreen() => _isOnScreen.Invoke(_internalObj);
    public Vector2 GetCursorPosition() => _getCursorPosition.Invoke(_internalObj);
    public float GetInteractiveDistance() => _getInteractiveDistance.Invoke(_internalObj);
    public void SetInteractiveDistance(float distance) => _setInteractiveDistance.Invoke(_internalObj, distance);
    public int GetScreenRotate() => _getScreenRotate.Invoke(_internalObj);
    public bool CompareWithBlockAndSurface(IMyCubeBlock block, IMyTextSurface surface) => _compareWithBlockAndSurface.Invoke(_internalObj, block, surface);
    public void Dispose() => _dispose.Invoke(_internalObj);
  }
  public class ClickHandler
  {
    static public Func<object> _new;
    static public Func<object, Vector4> _getHitArea;
    static public Action<object, Vector4> _setHitArea;
    static public Func<object, bool> _isMouseReleased;
    static public Func<object, bool> _isMouseOver;
    static public Func<object, bool> _isMousePressed;
    static public Func<object, bool> _justReleased;
    static public Func<object, bool> _justPressed;
    static public Action<object, object> _updateStatus;
    internal object _internalObj;
    public ClickHandler() { _internalObj = _new(); }
    public ClickHandler(object internalObject) { _internalObj = internalObject; }
    public Vector4 GetHitArea() => _getHitArea.Invoke(_internalObj);
    public void SetHitArea(Vector4 hitArea) => _setHitArea.Invoke(_internalObj, hitArea);
    public bool IsMouseReleased() => _isMouseReleased.Invoke(_internalObj);
    public bool IsMouseOver() => _isMouseOver.Invoke(_internalObj);
    public bool IsMousePressed() => _isMousePressed.Invoke(_internalObj);
    public bool JustReleased() => _justReleased.Invoke(_internalObj);
    public bool JustPressed() => _justPressed.Invoke(_internalObj);
    public void UpdateStatus(TouchScreen screen) => _updateStatus.Invoke(_internalObj, screen._internalObj);
  }
  public class FancyCursor
  {
    static public Func<object, object> _new;
    static public Func<object, bool> _getActive;
    static public Action<object, bool> _setActive;
    static public Func<object, Vector2> _getPosition;
    static public Func<object, float, float, float, float, bool> _isInsideArea;
    static public Func<object, List<MySprite>> _getSprites;
    static public Action<object> _dispose;
    internal object _internalObj;
    public FancyCursor(TouchScreen screen) { _internalObj = _new(screen._internalObj); }
    public FancyCursor(object internalObject) { _internalObj = internalObject; }
    public bool GetActive() => _getActive.Invoke(_internalObj);
    public void SetActive(bool active) => _setActive.Invoke(_internalObj, active);
    public Vector2 GetPosition() => _getPosition.Invoke(_internalObj);
    public bool IsInsideArea(float x, float y, float z, float w) => _isInsideArea.Invoke(_internalObj, x, y, z, w);
    public List<MySprite> GetSprites() => _getSprites.Invoke(_internalObj);
    public void Dispose() => _dispose.Invoke(_internalObj);
  }
  public class FancyTheme
  {
    static public Func<object, Color> _getColorBg;
    static public Func<object, Color> _getColorWhite;
    static public Func<object, Color> _getColorMain;
    static public Func<object, int, Color> _getColorMainDarker;
    static public Func<object, String, string, float, Vector2> _measureStringInPixels;
    static public Func<object, float> _getScale;
    static public Action<object, float> _setScale;
    internal object _internalObj;
    public FancyTheme(object internalObject) { _internalObj = internalObject; }
    public Color GetColorBg() => _getColorBg.Invoke(_internalObj);
    public Color GetColorWhite() => _getColorWhite.Invoke(_internalObj);
    public Color GetColorMain() => _getColorMain.Invoke(_internalObj);
    public Color GetColorMainDarker(int value) => _getColorMainDarker.Invoke(_internalObj, value);
    public Vector2 MeasureStringInPixels(String text, string font, float scale) => _measureStringInPixels.Invoke(_internalObj, text, font, scale);
    public float GetScale() => _getScale.Invoke(_internalObj);
    public void SetScale(float scale) => _setScale.Invoke(_internalObj, scale);
  }
  public class FancyElementBase
  {
    static public Func<object, Vector2> _getPosition;
    static public Action<object, Vector2> _setPosition;
    static public Func<object, Vector4> _getMargin;
    static public Action<object, Vector4> _setMargin;
    static public Func<object, Vector2> _getScale;
    static public Action<object, Vector2> _setScale;
    static public Func<object, Vector2> _getPixels;
    static public Action<object, Vector2> _setPixels;
    static public Func<object, Vector2> _getSize;
    static public Func<object, RectangleF> _getViewport;
    static public Func<object, object> _getApp;
    static public Func<object, object> _getParent;
    static public Func<object, Vector2> _getOffset;
    static public Func<object, List<MySprite>> _getSprites;
    static public Action<object> _initElements;
    static public Action<object> _update;
    static public Action<object> _dispose;
    protected FancyApp _app;
    protected FancyElementContainerBase _parent;
    internal object _internalObj;
    public FancyElementBase(object internalObject) { _internalObj = internalObject; }
    public Vector2 GetPosition() => _getPosition.Invoke(_internalObj);
    public void SetPosition(Vector2 position) => _setPosition.Invoke(_internalObj, position);
    public Vector4 GetMargin() => _getMargin.Invoke(_internalObj);
    public void SetMargin(Vector4 margin) => _setMargin.Invoke(_internalObj, margin);
    public Vector2 GetScale() => _getScale.Invoke(_internalObj);
    public void SetScale(Vector2 scale) => _setScale.Invoke(_internalObj, scale);
    public Vector2 GetPixels() => _getPixels.Invoke(_internalObj);
    public void SetPixels(Vector2 pixels) => _setPixels.Invoke(_internalObj, pixels);
    public Vector2 GetSize() => _getSize.Invoke(_internalObj);
    public RectangleF GetViewport() => _getViewport.Invoke(_internalObj);
    public FancyApp GetApp() { if (_app == null) _app = new FancyApp(_getApp.Invoke(_internalObj)); return _app; }
    public FancyElementContainerBase GetParent() { if (_parent == null) _parent = new FancyApp(_getParent.Invoke(_internalObj)); return _parent; }
    public Vector2 GetOffset() => _getOffset.Invoke(_internalObj);
    public List<MySprite> GetSprites() => _getSprites.Invoke(_internalObj);
    public void InitElements() => _initElements.Invoke(_internalObj);
    public void Update() => _update.Invoke(_internalObj);
    public void Dispose() => _dispose.Invoke(_internalObj);
  }
  public class FancyElementContainerBase : FancyElementBase
  {
    static public Func<object, List<object>> _getChildren;
    static public Action<object, object> _addChild;
    static public Action<object, object> _removeChild;
    public FancyElementContainerBase(object internalObject) : base(internalObject) { }
    public List<object> GetChildren() => _getChildren.Invoke(_internalObj);
    public void AddChild(FancyElementBase child) => _addChild.Invoke(_internalObj, child._internalObj);
    public void RemoveChild(FancyElementBase child) => _removeChild.Invoke(_internalObj, child._internalObj);
  }
  public class FancyView : FancyElementContainerBase
  {
    static public Func<int, object> _newV;
    static public Func<object, int> _getDirection;
    static public Action<object, int> _setDirection;
    public enum ViewDirection : int { None = 0, Row = 1, Column = 2 }
    public FancyView(ViewDirection direction = ViewDirection.Column) : base(_newV((int)direction)) { }
    public FancyView(object internalObject) : base(internalObject) { }
    public ViewDirection GetDirection() => (ViewDirection)_getDirection.Invoke(_internalObj);
    public void SetDirection(ViewDirection direction) => _setDirection.Invoke(_internalObj, (int)direction);
  }
  public class FancyApp : FancyView
  {
    static public Func<object> _new;
    static public Func<object, object> _getScreen;
    static public Func<object, object> _getCursor;
    static public Func<object, object> _getTheme;
    static public Action<object, MyCubeBlock, Sandbox.ModAPI.Ingame.IMyTextSurface> _initApp;
    protected TouchScreen _screen;
    protected FancyCursor _cursor;
    protected FancyTheme _theme;
    public FancyApp() : base(_new()) { }
    public FancyApp(object internalObject) : base(internalObject) { }
    public TouchScreen GetScreen() { if (_screen == null) _screen = new TouchScreen(_getScreen.Invoke(_internalObj)); return _screen; }
    public FancyCursor GetCursor() { if (_cursor == null) _cursor = new FancyCursor(_getCursor.Invoke(_internalObj)); return _cursor; }
    public FancyTheme GetTheme() { if (_theme == null) _theme = new FancyTheme(_getTheme.Invoke(_internalObj)); return _theme; }
    public void InitApp(MyCubeBlock block, Sandbox.ModAPI.Ingame.IMyTextSurface surface) => _initApp.Invoke(_internalObj, block, surface);
  }
  public class FancyButtonBase : FancyElementBase
  {
    static public Func<object, object> _getHandler;
    protected ClickHandler _handler;
    public FancyButtonBase(object internalObject) : base(internalObject) { }
    public ClickHandler GetHandler() { if (_handler == null) _handler = new ClickHandler(_getHandler.Invoke(_internalObj)); return _handler; }
  }
  public class FancyButton : FancyButtonBase
  {
    static public Func<string, Action, object> _new;
    static public Func<object, string> _getText;
    static public Action<object, string> _setText;
    static public Action<object, Action> _setAction;
    static public Func<object, TextAlignment> _getAlignment;
    static public Action<object, TextAlignment> _setAlignment;
    public FancyButton(string text, Action action) : base(_new(text, action)) { }
    public FancyButton(object internalObject) : base(internalObject) { }
    public string GetText() => _getText.Invoke(_internalObj);
    public void SetText(string text) => _setText.Invoke(_internalObj, text);
    public void SetAction(Action action) => _setAction.Invoke(_internalObj, action);
    public TextAlignment GetAlignment() => _getAlignment.Invoke(_internalObj);
    public void SetAlignment(TextAlignment alignment) => _setAlignment.Invoke(_internalObj, alignment);
  }
  public class FancyLabel : FancyElementBase
  {
    static public Func<string, float, object> _new;
    static public Func<object, string> _getText;
    static public Action<object, string> _setText;
    static public Action<object, float> _setFontSize;
    static public Func<object, TextAlignment> _getAlignment;
    static public Action<object, TextAlignment> _setAlignment;
    public FancyLabel(string text, float fontSize = 0.5f) : base(_new(text, fontSize)) { }
    public FancyLabel(object internalObject) : base(internalObject) { }
    public string GetText() => _getText.Invoke(_internalObj);
    public void SetText(string text) => _setText.Invoke(_internalObj, text);
    public void SetFontSize(float fontSize) => _setFontSize.Invoke(_internalObj, fontSize);
    public TextAlignment GetAlignment() => _getAlignment.Invoke(_internalObj);
    public void SetAlignment(TextAlignment alignment) => _setAlignment.Invoke(_internalObj, alignment);
  }
  public class FancyPanel : FancyView
  {
    static public Func<object> _new;
    public FancyPanel() : base(_new()) { }
    public FancyPanel(object internalObject) : base(internalObject) { }
  }
  public class FancyProgressBar : FancyElementBase
  {
    static public Func<float, float, bool, object> _new;
    static public Func<object, float> _getValue;
    static public Action<object, float> _setValue;
    public FancyProgressBar(float min, float max, bool bars = true) : base(_new(min, max, bars)) { }
    public FancyProgressBar(object internalObject) : base(internalObject) { }
    public float GetValue() => _getValue.Invoke(_internalObj);
    public void SetValue(float value) => _setValue.Invoke(_internalObj, value);
  }
  public class FancySelector : FancyButtonBase
  {
    static public Func<List<string>, Action<int, string>, bool, object> _new;
    static public Action<object, Action<int, string>> _setAction;
    public FancySelector(List<string> labels, Action<int, string> action, bool loop = true) : base(_new(labels, action, loop)) { }
    public FancySelector(object internalObject) : base(internalObject) { }
    public void SetAction(Action<int, string> action) => _setAction.Invoke(_internalObj, action);
  }
  public class FancySeparator : FancyElementBase
  {
    static public Func<object> _new;
    public FancySeparator() : base(_new()) { }
    public FancySeparator(object internalObject) : base(internalObject) { }
  }
  public class FancySlider : FancyButtonBase
  {
    static public Func<float, float, Action<float>, object> _new;
    static public Func<object, Vector2> _getRange;
    static public Action<object, Vector2> _setRange;
    static public Func<object, float> _getValue;
    static public Action<object, float> _setValue;
    static public Action<object, Action<float>> _setAction;
    static public Func<object, bool> _getIsInteger;
    static public Action<object, bool> _setIsInteger;
    static public Func<object, bool> _getAllowInput;
    static public Action<object, bool> _setAllowInput;
    public FancySlider(float min, float max, Action<float> action) : base(_new(min, max, action)) { }
    public FancySlider(object internalObject) : base(internalObject) { }
    public Vector2 GetRange() => _getRange.Invoke(_internalObj);
    public void SetRange(Vector2 range) => _setRange.Invoke(_internalObj, range);
    public float GetValue() => _getValue.Invoke(_internalObj);
    public void SetValue(float value) => _setValue.Invoke(_internalObj, value);
    public void SetAction(Action<float> action) => _setAction.Invoke(_internalObj, action);
    public bool GetIsInteger() => _getIsInteger.Invoke(_internalObj);
    public void SetIsInteger(bool isInterger) => _setIsInteger.Invoke(_internalObj, isInterger);
    public bool GetAllowInput() => _getAllowInput.Invoke(_internalObj);
    public void SetAllowInput(bool allowInput) => _setAllowInput.Invoke(_internalObj, allowInput);
  }
  public class FancySliderRange : FancySlider
  {
    static public Func<float, float, Action<float, float>, object> _newR;
    static public Func<object, float> _getValueLower;
    static public Action<object, float> _setValueLower;
    static public Action<object, Action<float, float>> _setActionR;
    public FancySliderRange(float min, float max, Action<float, float> action) : base(_newR(min, max, action)) { }
    public FancySliderRange(object internalObject) : base(internalObject) { }
    public float GetValueLower() => _getValueLower.Invoke(_internalObj);
    public void SetValueLower(float value) => _setValueLower.Invoke(_internalObj, value);
    public void SetActionRange(Action<float, float> action) => _setActionR.Invoke(_internalObj, action);
  }
  public class FancySwitch : FancyButtonBase
  {
    static public Func<Action<bool>, string, string, object> _new;
    static public Func<object, string> _getTextOn;
    static public Action<object, string> _setTextOn;
    static public Func<object, string> _getTextOff;
    static public Action<object, string> _setTextOff;
    static public Func<object, bool> _getValue;
    static public Action<object, bool> _setValue;
    static public Action<object, Action<bool>> _setAction;
    public FancySwitch(Action<bool> action, string textOn = "On", string textOff = "Off") : base(_new(action, textOn, textOff)) { }
    public FancySwitch(object internalObject) : base(internalObject) { }
    public string GetTextOn() => _getTextOn.Invoke(_internalObj);
    public void SetTextOn(string text) => _setTextOn.Invoke(_internalObj, text);
    public string GetTextOff() => _getTextOff.Invoke(_internalObj);
    public void SetTextOff(string text) => _setTextOff.Invoke(_internalObj, text);
    public bool GetValue() => _getValue.Invoke(_internalObj);
    public void SetValue(bool value) => _setValue.Invoke(_internalObj, value);
    public void SetAction(Action<bool> action) => _setAction.Invoke(_internalObj, action);
  }
  public class FancyTextField : FancyButtonBase
  {
    static public Func<string, Action<string>, object> _new;
    static public Func<object, string> _getText;
    static public Action<object, string> _setText;
    static public Action<object, Action<string>> _setAction;
    static public Func<object, bool> _getIsNumeric;
    static public Action<object, bool> _setIsNumeric;
    static public Func<object, bool> _getIsInteger;
    static public Action<object, bool> _setIsInteger;
    static public Func<object, bool> _getAllowNegative;
    static public Action<object, bool> _setAllowNegative;
    static public Func<object, TextAlignment> _getAlignment;
    static public Action<object, TextAlignment> _setAlignment;
    public FancyTextField(string text, Action<string> action) : base(_new(text, action)) { }
    public FancyTextField(object internalObject) : base(internalObject) { }
    public string GetText() => _getText.Invoke(_internalObj);
    public void SetText(string text) => _setText.Invoke(_internalObj, text);
    public void SetAction(Action<string> action) => _setAction.Invoke(_internalObj, action);
    public bool GetIsNumeric() => _getIsNumeric.Invoke(_internalObj);
    public void SetIsNumeric(bool isNumeric) => _setIsNumeric.Invoke(_internalObj, isNumeric);
    public bool GetIsInteger() => _getIsInteger.Invoke(_internalObj);
    public void SetIsInteger(bool isInterger) => _setIsInteger.Invoke(_internalObj, isInterger);
    public bool GetAllowNegative() => _getAllowNegative.Invoke(_internalObj);
    public void SetAllowNegative(bool allowNegative) => _setAllowNegative.Invoke(_internalObj, allowNegative);
    public TextAlignment GetAlignment() => _getAlignment.Invoke(_internalObj);
    public void SetAlignment(TextAlignment alignment) => _setAlignment.Invoke(_internalObj, alignment);
  }
  public class FancyWindowBar : FancyElementBase
  {
    static public Func<string, object> _new;
    static public Func<object, string> _getText;
    static public Action<object, string> _setText;
    public FancyWindowBar(string text) : base(_new(text)) { }
    public FancyWindowBar(object internalObject) : base(internalObject) { }
    public string GetText() => _getText.Invoke(_internalObj);
    public void SetText(string text) => _setText.Invoke(_internalObj, text);
  }
}