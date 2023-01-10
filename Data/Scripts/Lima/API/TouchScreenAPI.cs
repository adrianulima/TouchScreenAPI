using System;
using System.Collections.Generic;
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
    private bool _isInitiated;
    private bool _isRegistered;
    public bool IsReady { get; protected set; }

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

    protected virtual string GetRequestString() { return "ApiRequestTouch"; }

    public virtual bool Load()
    {
      if (!_isRegistered)
      {
        _isRegistered = true;
        MyAPIGateway.Utilities.RegisterMessageHandler(_channel, HandleMessage);
      }
      if (!IsReady && !_isInitiated)
        MyAPIGateway.Utilities.SendModMessage(_channel, GetRequestString());

      WrapperBase<TouchScreenAPI>.SetApi(this);
      return IsReady;
    }
    public virtual void Unload()
    {
      if (_isRegistered)
        MyAPIGateway.Utilities.UnregisterMessageHandler(_channel, HandleMessage);
      ApiDelegates(null);
      _isRegistered = false;
      _isInitiated = false;
      IsReady = false;
      WrapperBase<TouchScreenAPI>.SetApi(null);
    }
    private void HandleMessage(object msg)
    {
      if (_isInitiated || msg is string)
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
    public virtual void ApiDelegates(IReadOnlyDictionary<string, Delegate> delegates)
    {
      _isInitiated = delegates != null;
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
      AssignMethod(delegates, "TouchCursor_New", ref TouchCursor_New);
      AssignMethod(delegates, "TouchCursor_GetActive", ref TouchCursor_GetActive);
      AssignMethod(delegates, "TouchCursor_SetActive", ref TouchCursor_SetActive);
      AssignMethod(delegates, "TouchCursor_GetScale", ref TouchCursor_GetScale);
      AssignMethod(delegates, "TouchCursor_SetScale", ref TouchCursor_SetScale);
      AssignMethod(delegates, "TouchCursor_GetPosition", ref TouchCursor_GetPosition);
      AssignMethod(delegates, "TouchCursor_IsInsideArea", ref TouchCursor_IsInsideArea);
      AssignMethod(delegates, "TouchCursor_GetSprites", ref TouchCursor_GetSprites);
      AssignMethod(delegates, "TouchCursor_ForceDispose", ref TouchCursor_ForceDispose);
      AssignMethod(delegates, "ClickHandler_New", ref ClickHandler_New);
      AssignMethod(delegates, "ClickHandler_GetHitArea", ref ClickHandler_GetHitArea);
      AssignMethod(delegates, "ClickHandler_SetHitArea", ref ClickHandler_SetHitArea);
      AssignMethod(delegates, "ClickHandler_IsMouseReleased", ref ClickHandler_IsMouseReleased);
      AssignMethod(delegates, "ClickHandler_IsMouseOver", ref ClickHandler_IsMouseOver);
      AssignMethod(delegates, "ClickHandler_IsMousePressed", ref ClickHandler_IsMousePressed);
      AssignMethod(delegates, "ClickHandler_JustReleased", ref ClickHandler_JustReleased);
      AssignMethod(delegates, "ClickHandler_JustPressed", ref ClickHandler_JustPressed);
      AssignMethod(delegates, "ClickHandler_UpdateStatus", ref ClickHandler_UpdateStatus);
    }
    protected void AssignMethod<T>(IReadOnlyDictionary<string, Delegate> delegates, string name, ref T field) where T : class
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

    public Func<object, object> TouchCursor_New;
    public Func<object, bool> TouchCursor_GetActive;
    public Action<object, bool> TouchCursor_SetActive;
    public Func<object, float> TouchCursor_GetScale;
    public Action<object, float> TouchCursor_SetScale;
    public Func<object, Vector2> TouchCursor_GetPosition;
    public Func<object, float, float, float, float, bool> TouchCursor_IsInsideArea;
    public Func<object, List<MySprite>> TouchCursor_GetSprites;
    public Action<object> TouchCursor_ForceDispose;

    public Func<object> ClickHandler_New;
    public Func<object, Vector4> ClickHandler_GetHitArea;
    public Action<object, Vector4> ClickHandler_SetHitArea;
    public Func<object, bool> ClickHandler_IsMouseReleased;
    public Func<object, bool> ClickHandler_IsMouseOver;
    public Func<object, bool> ClickHandler_IsMousePressed;
    public Func<object, bool> ClickHandler_JustReleased;
    public Func<object, bool> ClickHandler_JustPressed;
    public Action<object, object> ClickHandler_UpdateStatus;
  }

  public abstract class WrapperBase<TT> where TT : TouchScreenAPI
  {
    static protected TT Api;
    internal static void SetApi(TT api) => Api = api;

    static protected T Wrap<T>(object obj, Func<object, T> ctor)
    {
      return (obj == null) ? default(T) : ctor(obj);
    }

    static protected T[] WrapArray<T>(object[] objArray, Func<object, T> ctor)
    {
      var newArray = new T[objArray.Length];
      for (int i = 0; i < objArray.Length; i++)
        newArray[i] = Wrap<T>(objArray[i], ctor);
      return newArray;
    }

    internal object InternalObj { get; private set; }
    public WrapperBase(object internalObject) { InternalObj = internalObject; }
  }
  public class TouchScreen : WrapperBase<TouchScreenAPI>
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
  public class TouchCursor : WrapperBase<TouchScreenAPI>
  {
    public TouchCursor(TouchScreen screen) : base(Api.TouchCursor_New(screen.InternalObj)) { }
    public TouchCursor(object internalObject) : base(internalObject) { }
    public bool Active { get { return Api.TouchCursor_GetActive.Invoke(InternalObj); } set { Api.TouchCursor_SetActive.Invoke(InternalObj, value); } }
    public float Scale { get { return Api.TouchCursor_GetScale.Invoke(InternalObj); } set { Api.TouchCursor_SetScale.Invoke(InternalObj, value); } }
    public Vector2 Position { get { return Api.TouchCursor_GetPosition.Invoke(InternalObj); } }
    public bool IsInsideArea(float x, float y, float z, float w) => Api.TouchCursor_IsInsideArea.Invoke(InternalObj, x, y, z, w);
    public List<MySprite> GetSprites() => Api.TouchCursor_GetSprites.Invoke(InternalObj);
    public void ForceDispose() => Api.TouchCursor_ForceDispose.Invoke(InternalObj);
  }
  public class ClickHandler : WrapperBase<TouchScreenAPI>
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
}