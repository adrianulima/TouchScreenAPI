
using System.Collections.Generic;
using System;
using VRage.Game.GUI.TextPanel;
using VRage.Utils;
using VRageMath;
using MyAPIGateway = Sandbox.ModAPI.MyAPIGateway;
using IngameIMyTextSurface = Sandbox.ModAPI.Ingame.IMyTextSurface;
using IngameIMyCubeBlock = VRage.Game.ModAPI.Ingame.IMyCubeBlock;
using IngameIMyBlockGroup = Sandbox.ModAPI.Ingame.IMyBlockGroup;

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

    /// <summary>
    /// Wheter the API is ready to be used. True after <see cref="Load"/> is called.
    /// </summary>
    public bool IsReady { get; protected set; }

    private Func<IngameIMyCubeBlock, IngameIMyTextSurface, object> _createTouchScreen;
    private Action<IngameIMyCubeBlock, IngameIMyTextSurface> _removeTouchScreen;
    private Action<string> _addSurfaceCoords;
    private Action<string> _removeSurfaceCoords;
    private Func<IngameIMyCubeBlock, string> _getBlockIconSprite;
    private Func<IngameIMyBlockGroup, string> _getBlockGroupIconSprite;

    /// <summary>
    /// Creates an instance of TouchScreen add adds it to the Touch Manager.
    /// TouchScreen is responsible for checking player direction and distance to screen.
    /// Use only one TouchScreen for each surface on the block that will need it.
    /// Needs to be removed with <see cref="RemoveTouchScreen"/> when the App screen is not using the Api anymore.
    /// </summary>
    /// <param name="block">The block the touch point will be calculated.</param>
    /// <param name="surface">The surface the user will handle touch.</param>
    /// <returns></returns>
    public object CreateTouchScreen(IngameIMyCubeBlock block, IngameIMyTextSurface surface) => _createTouchScreen?.Invoke(block, surface);
    /// <summary>
    /// Dispose the instance of TouchScreen related to given block and surface. And also removes from Touch Manager.
    /// </summary>
    /// <param name="block">The related block.</param>
    /// <param name="surface">The related surface.</param>
    public void RemoveTouchScreen(IngameIMyCubeBlock block, IngameIMyTextSurface surface) => _removeTouchScreen?.Invoke(block, surface);
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
    /// <summary>
    /// Gets the icon from block definition and set as LCD sprite definition.
    /// </summary>
    /// <param name="block"></param>
    public string GetBlockIconSprite(IngameIMyCubeBlock block) => _getBlockIconSprite?.Invoke(block);
    public string GetBlockGroupIconSprite(IngameIMyBlockGroup blockGroup) => _getBlockGroupIconSprite?.Invoke(blockGroup);

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
      AssignMethod(delegates, "GetBlockIconSprite", ref _getBlockIconSprite);
      AssignMethod(delegates, "GetBlockGroupIconSprite", ref _getBlockGroupIconSprite);
      AssignMethod(delegates, "TouchScreen_GetBlock", ref ApiDelegator.TouchScreen_GetBlock);
      AssignMethod(delegates, "TouchScreen_GetSurface", ref ApiDelegator.TouchScreen_GetSurface);
      AssignMethod(delegates, "TouchScreen_GetIndex", ref ApiDelegator.TouchScreen_GetIndex);
      AssignMethod(delegates, "TouchScreen_IsOnScreen", ref ApiDelegator.TouchScreen_IsOnScreen);
      AssignMethod(delegates, "TouchScreen_GetMouse1", ref ApiDelegator.TouchScreen_GetMouse1);
      AssignMethod(delegates, "TouchScreen_GetMouse2", ref ApiDelegator.TouchScreen_GetMouse2);
      AssignMethod(delegates, "TouchScreen_GetMouse3", ref ApiDelegator.TouchScreen_GetMouse3);
      AssignMethod(delegates, "TouchScreen_GetCursorPosition", ref ApiDelegator.TouchScreen_GetCursorPosition);
      AssignMethod(delegates, "TouchScreen_GetInteractiveDistance", ref ApiDelegator.TouchScreen_GetInteractiveDistance);
      AssignMethod(delegates, "TouchScreen_SetInteractiveDistance", ref ApiDelegator.TouchScreen_SetInteractiveDistance);
      AssignMethod(delegates, "TouchScreen_GetRotation", ref ApiDelegator.TouchScreen_GetRotation);
      AssignMethod(delegates, "TouchScreen_CompareWithBlockAndSurface", ref ApiDelegator.TouchScreen_CompareWithBlockAndSurface);
      AssignMethod(delegates, "TouchScreen_ForceDispose", ref ApiDelegator.TouchScreen_ForceDispose);
      AssignMethod(delegates, "Cursor_New", ref ApiDelegator.Cursor_New);
      AssignMethod(delegates, "Cursor_GetEnabled", ref ApiDelegator.Cursor_GetEnabled);
      AssignMethod(delegates, "Cursor_SetEnabled", ref ApiDelegator.Cursor_SetEnabled);
      AssignMethod(delegates, "Cursor_GetScale", ref ApiDelegator.Cursor_GetScale);
      AssignMethod(delegates, "Cursor_SetScale", ref ApiDelegator.Cursor_SetScale);
      AssignMethod(delegates, "Cursor_GetPosition", ref ApiDelegator.Cursor_GetPosition);
      AssignMethod(delegates, "Cursor_IsInsideArea", ref ApiDelegator.Cursor_IsInsideArea);
      AssignMethod(delegates, "Cursor_GetSprites", ref ApiDelegator.Cursor_GetSprites);
      AssignMethod(delegates, "Cursor_ForceDispose", ref ApiDelegator.Cursor_ForceDispose);
      AssignMethod(delegates, "ClickHandler_New", ref ApiDelegator.ClickHandler_New);
      AssignMethod(delegates, "ClickHandler_GetHitArea", ref ApiDelegator.ClickHandler_GetHitArea);
      AssignMethod(delegates, "ClickHandler_SetHitArea", ref ApiDelegator.ClickHandler_SetHitArea);
      AssignMethod(delegates, "ClickHandler_Update", ref ApiDelegator.ClickHandler_Update);
      AssignMethod(delegates, "ClickHandler_GetMouse1", ref ApiDelegator.ClickHandler_GetMouse1);
      AssignMethod(delegates, "ClickHandler_GetMouse2", ref ApiDelegator.ClickHandler_GetMouse2);
      AssignMethod(delegates, "ClickHandler_GetMouse3", ref ApiDelegator.ClickHandler_GetMouse3);
      AssignMethod(delegates, "ButtonState_New", ref ApiDelegator.ButtonState_New);
      AssignMethod(delegates, "ButtonState_IsReleased", ref ApiDelegator.ButtonState_IsReleased);
      AssignMethod(delegates, "ButtonState_IsOver", ref ApiDelegator.ButtonState_IsOver);
      AssignMethod(delegates, "ButtonState_IsPressed", ref ApiDelegator.ButtonState_IsPressed);
      AssignMethod(delegates, "ButtonState_JustReleased", ref ApiDelegator.ButtonState_JustReleased);
      AssignMethod(delegates, "ButtonState_JustPressed", ref ApiDelegator.ButtonState_JustPressed);
      AssignMethod(delegates, "ButtonState_Update", ref ApiDelegator.ButtonState_Update);
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
    public Func<object, IngameIMyCubeBlock> TouchScreen_GetBlock;
    public Func<object, IngameIMyTextSurface> TouchScreen_GetSurface;
    public Func<object, int> TouchScreen_GetIndex;
    public Func<object, bool> TouchScreen_IsOnScreen;
    public Func<object, object> TouchScreen_GetMouse1;
    public Func<object, object> TouchScreen_GetMouse2;
    public Func<object, object> TouchScreen_GetMouse3;
    public Func<object, Vector2> TouchScreen_GetCursorPosition;
    public Func<object, float> TouchScreen_GetInteractiveDistance;
    public Action<object, float> TouchScreen_SetInteractiveDistance;
    public Func<object, int> TouchScreen_GetRotation;
    public Func<object, IngameIMyCubeBlock, IngameIMyTextSurface, bool> TouchScreen_CompareWithBlockAndSurface;
    public Action<object> TouchScreen_ForceDispose;

    public Func<object, object> Cursor_New;
    public Func<object, bool> Cursor_GetEnabled;
    public Action<object, bool> Cursor_SetEnabled;
    public Func<object, float> Cursor_GetScale;
    public Action<object, float> Cursor_SetScale;
    public Func<object, Vector2> Cursor_GetPosition;
    public Func<object, float, float, float, float, bool> Cursor_IsInsideArea;
    public Func<object, List<MySprite>> Cursor_GetSprites;
    public Action<object> Cursor_ForceDispose;

    public Func<object> ClickHandler_New;
    public Func<object, Vector4> ClickHandler_GetHitArea;
    public Action<object, Vector4> ClickHandler_SetHitArea;
    public Action<object, object> ClickHandler_Update;
    public Func<object, object> ClickHandler_GetMouse1;
    public Func<object, object> ClickHandler_GetMouse2;
    public Func<object, object> ClickHandler_GetMouse3;

    public Func<object> ButtonState_New;
    public Func<object, bool> ButtonState_IsReleased;
    public Func<object, bool> ButtonState_IsOver;
    public Func<object, bool> ButtonState_IsPressed;
    public Func<object, bool> ButtonState_JustReleased;
    public Func<object, bool> ButtonState_JustPressed;
    public Action<object, bool, bool> ButtonState_Update;
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
  /// Each <see cref="IngameIMyTextSurface"/> on a <see cref="IngameIMyTextSurface"/> may have it.
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/TouchScreen.cs"/>
  /// </summary>
  public class TouchScreen : WrapperBase<TouchApiDelegator>
  {
    private ButtonState _mouse1;
    private ButtonState _mouse2;
    private ButtonState _mouse3;
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchScreen(object internalObject) : base(internalObject) { }
    public IngameIMyCubeBlock Block { get { return Api.TouchScreen_GetBlock.Invoke(InternalObj); } }
    public IngameIMyTextSurface Surface { get { return Api.TouchScreen_GetSurface.Invoke(InternalObj); } }
    public int Index { get { return Api.TouchScreen_GetIndex.Invoke(InternalObj); } }
    public bool IsOnScreen { get { return Api.TouchScreen_IsOnScreen.Invoke(InternalObj); } }
    public ButtonState Mouse1 { get { return _mouse1 ?? (_mouse1 = Wrap<ButtonState>(Api.TouchScreen_GetMouse1.Invoke(InternalObj), (obj) => new ButtonState(obj))); } }
    public ButtonState Mouse2 { get { return _mouse2 ?? (_mouse2 = Wrap<ButtonState>(Api.TouchScreen_GetMouse2.Invoke(InternalObj), (obj) => new ButtonState(obj))); } }
    public ButtonState Mouse3 { get { return _mouse3 ?? (_mouse3 = Wrap<ButtonState>(Api.TouchScreen_GetMouse3.Invoke(InternalObj), (obj) => new ButtonState(obj))); } }
    public Vector2 CursorPosition { get { return Api.TouchScreen_GetCursorPosition.Invoke(InternalObj); } }
    public float InteractiveDistance { get { return Api.TouchScreen_GetInteractiveDistance.Invoke(InternalObj); } set { Api.TouchScreen_SetInteractiveDistance.Invoke(InternalObj, value); } }
    public int Rotation { get { return Api.TouchScreen_GetRotation.Invoke(InternalObj); } }
    public bool CompareWithBlockAndSurface(IngameIMyCubeBlock block, IngameIMyTextSurface surface) => Api.TouchScreen_CompareWithBlockAndSurface.Invoke(InternalObj, block, surface);
    public void ForceDispose() => Api.TouchScreen_ForceDispose.Invoke(InternalObj);
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/Cursor.cs"/>
  /// </summary>
  public class Cursor : WrapperBase<TouchApiDelegator>
  {
    public Cursor(TouchScreen screen) : base(Api.Cursor_New(screen.InternalObj)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public Cursor(object internalObject) : base(internalObject) { }
    public bool Enabled { get { return Api.Cursor_GetEnabled.Invoke(InternalObj); } set { Api.Cursor_SetEnabled.Invoke(InternalObj, value); } }
    public float Scale { get { return Api.Cursor_GetScale.Invoke(InternalObj); } set { Api.Cursor_SetScale.Invoke(InternalObj, value); } }
    public Vector2 Position { get { return Api.Cursor_GetPosition.Invoke(InternalObj); } }
    public bool IsInsideArea(float x, float y, float z, float w) => Api.Cursor_IsInsideArea.Invoke(InternalObj, x, y, z, w);
    public List<MySprite> GetSprites() => Api.Cursor_GetSprites.Invoke(InternalObj);
    /// <summary>
    /// Force a call to Cursor Dispose, that clears sprites.
    /// </summary>
    public void ForceDispose() => Api.Cursor_ForceDispose.Invoke(InternalObj);
  }
  /// <summary>
  /// Responsible for handling and updating cursor states related to a area on a screen.
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/ClickHandler.cs"/>
  /// </summary>
  public class ClickHandler : WrapperBase<TouchApiDelegator>
  {
    private ButtonState _mouse1;
    private ButtonState _mouse2;
    private ButtonState _mouse3;
    public ClickHandler() : base(Api.ClickHandler_New()) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public ClickHandler(object internalObject) : base(internalObject) { }
    /// <summary>
    /// A Vector4 representing the area on screen that should check for cursor position.
    /// </summary>
    public Vector4 HitArea { get { return Api.ClickHandler_GetHitArea.Invoke(InternalObj); } set { Api.ClickHandler_SetHitArea.Invoke(InternalObj, value); } }
    /// <summary>
    /// This is already called internally by the Touch Manager, only call this if you wanna override the handler status.
    /// </summary>
    public void Update(TouchScreen screen) => Api.ClickHandler_Update.Invoke(InternalObj, screen.InternalObj);
    public ButtonState Mouse1 { get { return _mouse1 ?? (_mouse1 = Wrap<ButtonState>(Api.ClickHandler_GetMouse1.Invoke(InternalObj), (obj) => new ButtonState(obj))); } }
    public ButtonState Mouse2 { get { return _mouse2 ?? (_mouse2 = Wrap<ButtonState>(Api.ClickHandler_GetMouse2.Invoke(InternalObj), (obj) => new ButtonState(obj))); } }
    public ButtonState Mouse3 { get { return _mouse3 ?? (_mouse3 = Wrap<ButtonState>(Api.ClickHandler_GetMouse3.Invoke(InternalObj), (obj) => new ButtonState(obj))); } }
  }
  /// <summary>
  /// Responsible for holding calculating and holding button state.
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/ButtonState.cs"/>
  /// </summary>
  public class ButtonState : WrapperBase<TouchApiDelegator>
  {
    public ButtonState() : base(Api.ButtonState_New()) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public ButtonState(object internalObject) : base(internalObject) { }
    public bool IsReleased { get { return Api.ButtonState_IsReleased.Invoke(InternalObj); } }
    public bool IsOver { get { return Api.ButtonState_IsOver.Invoke(InternalObj); } }
    public bool IsPressed { get { return Api.ButtonState_IsPressed.Invoke(InternalObj); } }
    public bool JustReleased { get { return Api.ButtonState_JustReleased.Invoke(InternalObj); } }
    public bool JustPressed { get { return Api.ButtonState_JustPressed.Invoke(InternalObj); } }
    public void Update(bool isPressed, bool isInsideArea) => Api.ButtonState_Update.Invoke(InternalObj, isPressed, isInsideArea);
  }
}
