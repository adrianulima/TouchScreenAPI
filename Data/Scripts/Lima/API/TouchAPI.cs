using System;
using System.Collections.Generic;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

//Change namespace to your mod's namespace
namespace Lima.API
{
  public class TouchAPI
  {
    // TODO: Replace with proper TouchAPI mod id
    private const long _channel = 123;
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

    public TouchAPI() { }

    public bool Load()
    {
      if (!_isRegistered)
      {
        _isRegistered = true;
        MyAPIGateway.Utilities.RegisterMessageHandler(_channel, HandleMessage);
      }
      if (!IsReady)
        MyAPIGateway.Utilities.SendModMessage(_channel, "ApiEndpointRequest");

      return IsReady;
    }

    public void Unload()
    {
      if (_isRegistered)
      {
        MyAPIGateway.Utilities.UnregisterMessageHandler(_channel, HandleMessage);
      }

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
        MyLog.Default.WriteLineAndConsole("TouchAPI Failed To Load For Client: " + MyAPIGateway.Utilities.GamePaths.ModScopeName);
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

      AssignMethod(delegates, "FancyView_GetDirection", ref FancyView._getDirection);
      AssignMethod(delegates, "FancyView_SetDirection", ref FancyView._setDirection);

      AssignMethod(delegates, "FancyApp_New", ref FancyApp._new);
      AssignMethod(delegates, "FancyApp_GetScreen", ref FancyApp._getScreen);
      AssignMethod(delegates, "FancyApp_GetCursor", ref FancyApp._getCursor);
      AssignMethod(delegates, "FancyApp_GetTheme", ref FancyApp._getTheme);
      AssignMethod(delegates, "FancyApp_InitApp", ref FancyApp._initApp);

      AssignMethod(delegates, "FancyCursor_GetActive", ref FancyCursor._getActive);
      AssignMethod(delegates, "FancyCursor_SetActive", ref FancyCursor._setActive);
      AssignMethod(delegates, "FancyCursor_GetPosition", ref FancyCursor._getPosition);
      AssignMethod(delegates, "FancyCursor_IsInsideArea", ref FancyCursor._isInsideArea);

      AssignMethod(delegates, "FancyTheme_GetColorBg", ref FancyTheme._getColorBg);
      AssignMethod(delegates, "FancyTheme_GetColorWhite", ref FancyTheme._getColorWhite);
      AssignMethod(delegates, "FancyTheme_GetColorMain", ref FancyTheme._getColorMain);
      AssignMethod(delegates, "FancyTheme_GetColorMainDarker", ref FancyTheme._getColorMainDarker);
      AssignMethod(delegates, "FancyTheme_MeasureStringInPixels", ref FancyTheme._measureStringInPixels);
      AssignMethod(delegates, "FancyTheme_GetScale", ref FancyTheme._getScale);
      AssignMethod(delegates, "FancyTheme_SetScale", ref FancyTheme._setScale);

      AssignMethod(delegates, "FancyButtonBase_GetHitArea", ref FancyButtonBase._getHitArea);
      AssignMethod(delegates, "FancyButtonBase_SetHitArea", ref FancyButtonBase._setHitArea);
      AssignMethod(delegates, "FancyButtonBase_IsMouseReleased", ref FancyButtonBase._isMouseReleased);
      AssignMethod(delegates, "FancyButtonBase_IsMouseOver", ref FancyButtonBase._isMouseOver);
      AssignMethod(delegates, "FancyButtonBase_IsMousePressed", ref FancyButtonBase._isMousePressed);
      AssignMethod(delegates, "FancyButtonBase_JustReleased", ref FancyButtonBase._justReleased);
      AssignMethod(delegates, "FancyButtonBase_JustPressed", ref FancyButtonBase._justPressed);

      AssignMethod(delegates, "FancyButton_New", ref FancyButton._new);
      AssignMethod(delegates, "FancyButton_GetText", ref FancyButton._getText);
      AssignMethod(delegates, "FancyButton_SetText", ref FancyButton._setText);
      AssignMethod(delegates, "FancyButton_SetAction", ref FancyButton._setAction);
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

    protected object _internalObj;
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
    static public Func<object, int> _getDirection;
    static public Action<object, int> _setDirection;

    public enum ViewDirection : int { None = 0, Row = 1, Column = 2 }

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

  public class FancyCursor
  {
    static public Func<object, bool> _getActive;
    static public Action<object, bool> _setActive;
    static public Func<object, Vector2> _getPosition;
    static public Func<object, float, float, float, float, bool> _isInsideArea;

    protected object _internalObj;
    public FancyCursor(object internalObject) { _internalObj = internalObject; }

    public bool GetActive() => _getActive.Invoke(_internalObj);
    public void SetActive(bool active) => _setActive.Invoke(_internalObj, active);
    public Vector2 GetPosition() => _getPosition.Invoke(_internalObj);
    public bool IsInsideArea(float x, float y, float z, float w) => _isInsideArea.Invoke(_internalObj, x, y, z, w);
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

    protected object _internalObj;
    public FancyTheme(object internalObject) { _internalObj = internalObject; }

    public Color GetColorBg() => _getColorBg.Invoke(_internalObj);
    public Color GetColorWhite() => _getColorWhite.Invoke(_internalObj);
    public Color GetColorMain() => _getColorMain.Invoke(_internalObj);
    public Color GetColorMainDarker(int value) => _getColorMainDarker.Invoke(_internalObj, value);
    public Vector2 MeasureStringInPixels(String text, string font, float scale) => _measureStringInPixels.Invoke(_internalObj, text, font, scale);
    public float GetScale() => _getScale.Invoke(_internalObj);
    public void SetScale(float scale) => _setScale.Invoke(_internalObj, scale);
  }

  public class FancyButtonBase : FancyElementBase
  {
    static public Func<object, Vector4> _getHitArea;
    static public Action<object, Vector4> _setHitArea;
    static public Func<object, bool> _isMouseReleased;
    static public Func<object, bool> _isMouseOver;
    static public Func<object, bool> _isMousePressed;
    static public Func<object, bool> _justReleased;
    static public Func<object, bool> _justPressed;

    public FancyButtonBase(object internalObject) : base(internalObject) { }

    public Vector4 GetHitArea() => _getHitArea.Invoke(_internalObj);
    public void SetHitArea(Vector4 hitArea) => _setHitArea.Invoke(_internalObj, hitArea);
    public bool IsMouseReleased() => _isMouseReleased.Invoke(_internalObj);
    public bool IsMouseOver() => _isMouseOver.Invoke(_internalObj);
    public bool IsMousePressed() => _isMousePressed.Invoke(_internalObj);
    public bool JustReleased() => _justReleased.Invoke(_internalObj);
    public bool JustPressed() => _justPressed.Invoke(_internalObj);
  }

  public class FancyButton : FancyButtonBase
  {
    static public Func<string, Action, object> _new;
    static public Func<object, string> _getText;
    static public Action<object, string> _setText;
    static public Action<object, Action> _setAction;

    public FancyButton(string text, Action action) : base(_new(text, action)) { }
    public FancyButton(object internalObject) : base(internalObject) { }

    public string GetText() => _getText.Invoke(_internalObj);
    public void SetText(string text) => _setText.Invoke(_internalObj, text);
    public void SetAction(Action action) => _setAction.Invoke(_internalObj, action);
  }
}