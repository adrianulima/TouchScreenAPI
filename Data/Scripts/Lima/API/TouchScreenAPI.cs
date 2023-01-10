using Sandbox.ModAPI;
using System.Collections.Generic;
using System;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Lima.API
{
  /// <summary>
  /// The client API for Touch Screen feature.
  /// This only handle communication with the MOD that have all the features.
  /// Copy this file to your mod, and add TouchScreenAPI mod as dependency.
  /// <see href="https://steamcommunity.com/sharedfiles/filedetails/?id=2668820525"/>
  /// </summary>
  public class TouchScreenAPI
  {
    private const long _channel = 2668820525; // Related to TouchScreenAPI MOD, do not change or reuse.
    private bool _isInitiated;
    private bool _isRegistered;

    private Func<IMyCubeBlock, IMyTextSurface, object> _createTouchScreen;
    private Action<IMyCubeBlock, IMyTextSurface> _removeTouchScreen;
    private Func<float> _getMaxInteractiveDistance;
    private Action<float> _setMaxInteractiveDistance;
    private Action<string> _addSurfaceCoords;
    private Action<string> _removeSurfaceCoords;

    /// <summary>
    /// Wheter the API is ready to be used. True after <see cref="Load"/> is called.
    /// </summary>
    public bool IsReady { get; protected set; }

    /// <summary>
    /// Creates an instance of TouchScreen add adds it to the Touch Manager.
    /// TouchScreen is responsible for checking player direction and distance to screen.
    /// Use only one TouchScreen for each surface on the block that will need it.
    /// Needs to be removed with <see cref="RemoveTouchScreen"/> when the App screen is not using the Api anymore.
    /// </summary>
    /// <param name="block">The block the touch point will be calculated.</param>
    /// <param name="surface">The surface the user will handle touch.</param>
    /// <returns></returns>
    public object CreateTouchScreen(IMyCubeBlock block, IMyTextSurface surface) => _createTouchScreen?.Invoke(block, surface);
    /// <summary>
    /// Dispose the instance of TouchScreen related to given block and surface. And also removes from Touch Manager.
    /// </summary>
    /// <param name="block">The related block.</param>
    /// <param name="surface">The related surface.</param>
    public void RemoveTouchScreen(IMyCubeBlock block, IMyTextSurface surface) => _removeTouchScreen?.Invoke(block, surface);
    /// <summary>
    /// This add surface coordinates to Touch Manager, it is a string similar to how GPS strings work.
    /// This same feature is also available using chat message commands with prefix /touch [string].
    /// </summary>
    /// <example>
    /// The string format is:
    /// TOUCH:<block_name>:<surface_number>:topleftX:topleftY:topleftZ:bottomleftX:bottomleftY:bottomleftZ:bottomRightX:bottomRightY:bottomRightZ
    /// Where the X, Y and Z are 3D model local positions, related to model origin.
    /// <code>
    /// AddSurfaceCoords("TOUCH:LargeLCDPanel:0:-1.23463:1.23463:-1.02366:-1.23463:-1.23463:-1.02366:1.23463:-1.23463:-1.02366");
    /// </code>
    /// </example>
    /// <param name="coords">Formatted coords string.</param>
    public void AddSurfaceCoords(string coords) => _addSurfaceCoords?.Invoke(coords);
    /// <summary>
    /// Removes surface coordinates from Touch Manager. Call it if you need to replace for a specific surface.
    /// </summary>
    /// <param name="coords"></param>
    public void RemoveSurfaceCoords(string coords) => _removeSurfaceCoords?.Invoke(coords);
    /// <returns>Current Touch Manager max interactive distance from player to screen.</returns>
    public float GetMaxInteractiveDistance() => _getMaxInteractiveDistance?.Invoke() ?? -1f;
    /// <summary>
    /// Sets Touch Manager max interactive distance from player to screen.
    /// </summary>
    /// <param name="distance">Distance in game meters.</param>
    public void SetMaxInteractiveDistance(float distance) => _setMaxInteractiveDistance?.Invoke(distance);

    protected virtual string GetRequestString() { return "ApiRequestTouch"; }

    protected TouchApiDelegator ApiDelegator;

    /// <summary>
    /// Create a TouchScreenAPI Instance. Should be only one per mod. 
    /// </summary>
    public TouchScreenAPI()
    {
      ApiDelegator = new TouchApiDelegator();
    }

    /// <summary>
    /// Register the client to receive API endpoints and request it.
    /// Can be called on Session LoadData().
    /// </summary>
    /// <returns>Updated IsReady value.</returns>
    public virtual bool Load()
    {
      if (!_isRegistered)
      {
        _isRegistered = true;
        MyAPIGateway.Utilities.RegisterMessageHandler(_channel, HandleMessage);
      }
      if (!IsReady && !_isInitiated)
        MyAPIGateway.Utilities.SendModMessage(_channel, GetRequestString());

      WrapperBase<TouchApiDelegator>.SetApi(ApiDelegator);
      return IsReady;
    }

    /// <summary>
    /// Unloads API endpoints from clients and reset values.
    /// Can be called on Session UnloadData().
    /// </summary>
    public virtual void Unload()
    {
      if (_isRegistered)
        MyAPIGateway.Utilities.UnregisterMessageHandler(_channel, HandleMessage);
      ApiDelegates(null);
      _isRegistered = false;
      _isInitiated = false;
      IsReady = false;
      WrapperBase<TouchApiDelegator>.SetApi(null);
      ApiDelegator = null;
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
    protected virtual void ApiDelegates(IReadOnlyDictionary<string, Delegate> delegates)
    {
      _isInitiated = delegates != null;
      AssignMethod(delegates, "CreateTouchScreen", ref _createTouchScreen);
      AssignMethod(delegates, "RemoveTouchScreen", ref _removeTouchScreen);
      AssignMethod(delegates, "AddSurfaceCoords", ref _addSurfaceCoords);
      AssignMethod(delegates, "RemoveSurfaceCoords", ref _removeSurfaceCoords);
      AssignMethod(delegates, "GetMaxInteractiveDistance", ref _getMaxInteractiveDistance);
      AssignMethod(delegates, "SetMaxInteractiveDistance", ref _setMaxInteractiveDistance);
      AssignMethod(delegates, "TouchScreen_GetBlock", ref ApiDelegator.TouchScreen_GetBlock);
      AssignMethod(delegates, "TouchScreen_GetSurface", ref ApiDelegator.TouchScreen_GetSurface);
      AssignMethod(delegates, "TouchScreen_GetIndex", ref ApiDelegator.TouchScreen_GetIndex);
      AssignMethod(delegates, "TouchScreen_IsOnScreen", ref ApiDelegator.TouchScreen_IsOnScreen);
      AssignMethod(delegates, "TouchScreen_GetCursorPosition", ref ApiDelegator.TouchScreen_GetCursorPosition);
      AssignMethod(delegates, "TouchScreen_GetInteractiveDistance", ref ApiDelegator.TouchScreen_GetInteractiveDistance);
      AssignMethod(delegates, "TouchScreen_SetInteractiveDistance", ref ApiDelegator.TouchScreen_SetInteractiveDistance);
      AssignMethod(delegates, "TouchScreen_GetRotation", ref ApiDelegator.TouchScreen_GetRotation);
      AssignMethod(delegates, "TouchScreen_CompareWithBlockAndSurface", ref ApiDelegator.TouchScreen_CompareWithBlockAndSurface);
      AssignMethod(delegates, "TouchScreen_ForceDispose", ref ApiDelegator.TouchScreen_ForceDispose);
      AssignMethod(delegates, "TouchCursor_New", ref ApiDelegator.TouchCursor_New);
      AssignMethod(delegates, "TouchCursor_GetActive", ref ApiDelegator.TouchCursor_GetActive);
      AssignMethod(delegates, "TouchCursor_SetActive", ref ApiDelegator.TouchCursor_SetActive);
      AssignMethod(delegates, "TouchCursor_GetScale", ref ApiDelegator.TouchCursor_GetScale);
      AssignMethod(delegates, "TouchCursor_SetScale", ref ApiDelegator.TouchCursor_SetScale);
      AssignMethod(delegates, "TouchCursor_GetPosition", ref ApiDelegator.TouchCursor_GetPosition);
      AssignMethod(delegates, "TouchCursor_IsInsideArea", ref ApiDelegator.TouchCursor_IsInsideArea);
      AssignMethod(delegates, "TouchCursor_GetSprites", ref ApiDelegator.TouchCursor_GetSprites);
      AssignMethod(delegates, "TouchCursor_ForceDispose", ref ApiDelegator.TouchCursor_ForceDispose);
      AssignMethod(delegates, "ClickHandler_New", ref ApiDelegator.ClickHandler_New);
      AssignMethod(delegates, "ClickHandler_GetHitArea", ref ApiDelegator.ClickHandler_GetHitArea);
      AssignMethod(delegates, "ClickHandler_SetHitArea", ref ApiDelegator.ClickHandler_SetHitArea);
      AssignMethod(delegates, "ClickHandler_IsMouseReleased", ref ApiDelegator.ClickHandler_IsMouseReleased);
      AssignMethod(delegates, "ClickHandler_IsMouseOver", ref ApiDelegator.ClickHandler_IsMouseOver);
      AssignMethod(delegates, "ClickHandler_IsMousePressed", ref ApiDelegator.ClickHandler_IsMousePressed);
      AssignMethod(delegates, "ClickHandler_JustReleased", ref ApiDelegator.ClickHandler_JustReleased);
      AssignMethod(delegates, "ClickHandler_JustPressed", ref ApiDelegator.ClickHandler_JustPressed);
      AssignMethod(delegates, "ClickHandler_UpdateStatus", ref ApiDelegator.ClickHandler_UpdateStatus);
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
  }

  /// <summary>
  /// Holds delegates for all Touch features. Populated by when <see cref="TouchScreenAPI.Load"/> is called.
  /// </summary>
  public class TouchApiDelegator
  {
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

  /// <summary>
  /// Wrapper class used to hold Api reference and make the link with Delegates.
  /// </summary>
  /// <typeparam name="TT">TouchApiDelegator or a class that inherits from it.</typeparam>
  public abstract class WrapperBase<TT> where TT : TouchApiDelegator
  {
    static protected TT Api;
    internal static void SetApi(TT api) => Api = api;
    static protected T Wrap<T>(object obj, Func<object, T> ctor) { return (obj == null) ? default(T) : ctor(obj); }
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
  /// <summary>
  /// TouchScreen is responsible for calculating screen direction and position from player.
  /// Each <see cref="IMyTextSurface"/> on a <see cref="IMyCubeBlock"/> may have it.
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/TouchScreen.cs"/>
  /// </summary>
  public class TouchScreen : WrapperBase<TouchApiDelegator>
  {
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
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
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/TouchCursor.cs"/>
  /// </summary>
  public class TouchCursor : WrapperBase<TouchApiDelegator>
  {
    public TouchCursor(TouchScreen screen) : base(Api.TouchCursor_New(screen.InternalObj)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchCursor(object internalObject) : base(internalObject) { }
    public bool Active { get { return Api.TouchCursor_GetActive.Invoke(InternalObj); } set { Api.TouchCursor_SetActive.Invoke(InternalObj, value); } }
    public float Scale { get { return Api.TouchCursor_GetScale.Invoke(InternalObj); } set { Api.TouchCursor_SetScale.Invoke(InternalObj, value); } }
    public Vector2 Position { get { return Api.TouchCursor_GetPosition.Invoke(InternalObj); } }
    public bool IsInsideArea(float x, float y, float z, float w) => Api.TouchCursor_IsInsideArea.Invoke(InternalObj, x, y, z, w);
    public List<MySprite> GetSprites() => Api.TouchCursor_GetSprites.Invoke(InternalObj);
    /// <summary>
    /// Force a call to Cursor Dispose, that clears sprites.
    /// </summary>
    public void ForceDispose() => Api.TouchCursor_ForceDispose.Invoke(InternalObj);
  }
  /// <summary>
  /// Responsible for handling and updating cursor states related to a area on a screen.
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/ClickHandler.cs"/>
  /// </summary>
  public class ClickHandler : WrapperBase<TouchApiDelegator>
  {
    public ClickHandler() : base(Api.ClickHandler_New()) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public ClickHandler(object internalObject) : base(internalObject) { }
    /// <summary>
    /// A Vector4 representing the area on screen that should check for cursor position.
    /// </summary>
    public Vector4 HitArea { get { return Api.ClickHandler_GetHitArea.Invoke(InternalObj); } set { Api.ClickHandler_SetHitArea.Invoke(InternalObj, value); } }
    public bool IsMouseReleased { get { return Api.ClickHandler_IsMouseReleased.Invoke(InternalObj); } }
    public bool IsMouseOver { get { return Api.ClickHandler_IsMouseOver.Invoke(InternalObj); } }
    public bool IsMousePressed { get { return Api.ClickHandler_IsMousePressed.Invoke(InternalObj); } }
    public bool JustReleased { get { return Api.ClickHandler_JustReleased.Invoke(InternalObj); } }
    public bool JustPressed { get { return Api.ClickHandler_JustPressed.Invoke(InternalObj); } }
    /// <summary>
    /// This is already called internally by the Touch Manager, only call this if you wanna override the handler status.
    /// </summary>
    public void UpdateStatus(TouchScreen screen) => Api.ClickHandler_UpdateStatus.Invoke(InternalObj, screen.InternalObj);
  }
}