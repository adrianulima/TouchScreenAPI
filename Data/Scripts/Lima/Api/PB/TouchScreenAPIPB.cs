using Sandbox.ModAPI.Interfaces;
using System.Collections.Generic;
using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;
using IMyTextSurface = Sandbox.ModAPI.Ingame.IMyTextSurface;
using IMyCubeBlock = VRage.Game.ModAPI.Ingame.IMyCubeBlock;
using IMyBlockGroup = Sandbox.ModAPI.Ingame.IMyBlockGroup;

namespace Lima.API.PB.UI
{
  // Copy these classes to inside your PB Program Class
  // This has only the TouchScreen feature, no UI kit
  public class TouchScreenAPI
  {
    public bool IsReady { get; protected set; }
    private Func<IMyCubeBlock, IMyTextSurface, object> _createTouchScreen;
    private Action<IMyCubeBlock, IMyTextSurface> _removeTouchScreen;
    private Action<string> _addSurfaceCoords;
    private Action<string> _removeSurfaceCoords;
    private Func<IMyCubeBlock, string> _getBlockIconSprite;
    private Func<IMyBlockGroup, string> _getBlockGroupIconSprite;
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
    /// <summary>
    /// Gets the icon from block definition and set as LCD sprite definition.
    /// </summary>
    /// <param name="block"></param>
    public string GetBlockIconSprite(IMyCubeBlock block) => _getBlockIconSprite?.Invoke(block);
    public string GetBlockGroupIconSprite(IMyBlockGroup blockGroup) => _getBlockGroupIconSprite?.Invoke(blockGroup);
    protected TouchApiDelegator ApiDelegator;
    public TouchScreenAPI(Sandbox.ModAPI.Ingame.IMyTerminalBlock pb)
    {
      var delegates = pb.GetProperty("2668820525")?.As<IReadOnlyDictionary<string, Delegate>>().GetValue(pb);
      if (delegates != null)
      {
        ApiDelegator = new TouchApiDelegator();
        WrapperBase<TouchApiDelegator>.SetApi(ApiDelegator);
        AssignMethod(out _createTouchScreen, delegates["CreateTouchScreen"]);
        AssignMethod(out _removeTouchScreen, delegates["RemoveTouchScreen"]);
        AssignMethod(out _addSurfaceCoords, delegates["AddSurfaceCoords"]);
        AssignMethod(out _removeSurfaceCoords, delegates["RemoveSurfaceCoords"]);
        AssignMethod(out _getBlockIconSprite, delegates["GetBlockIconSprite"]);
        AssignMethod(out _getBlockGroupIconSprite, delegates["GetBlockGroupIconSprite"]);
        AssignMethod(out ApiDelegator.TouchScreen_GetBlock, delegates["TouchScreen_GetBlock"]);
        AssignMethod(out ApiDelegator.TouchScreen_GetSurface, delegates["TouchScreen_GetSurface"]);
        AssignMethod(out ApiDelegator.TouchScreen_GetIndex, delegates["TouchScreen_GetIndex"]);
        AssignMethod(out ApiDelegator.TouchScreen_IsOnScreen, delegates["TouchScreen_IsOnScreen"]);
        AssignMethod(out ApiDelegator.TouchScreen_GetMouse1, delegates["TouchScreen_GetMouse1"]);
        AssignMethod(out ApiDelegator.TouchScreen_GetMouse2, delegates["TouchScreen_GetMouse2"]);
        AssignMethod(out ApiDelegator.TouchScreen_GetMouse3, delegates["TouchScreen_GetMouse3"]);
        AssignMethod(out ApiDelegator.TouchScreen_GetCursorPosition, delegates["TouchScreen_GetCursorPosition"]);
        AssignMethod(out ApiDelegator.TouchScreen_GetInteractiveDistance, delegates["TouchScreen_GetInteractiveDistance"]);
        AssignMethod(out ApiDelegator.TouchScreen_SetInteractiveDistance, delegates["TouchScreen_SetInteractiveDistance"]);
        AssignMethod(out ApiDelegator.TouchScreen_GetRotation, delegates["TouchScreen_GetRotation"]);
        AssignMethod(out ApiDelegator.TouchScreen_CompareWithBlockAndSurface, delegates["TouchScreen_CompareWithBlockAndSurface"]);
        AssignMethod(out ApiDelegator.TouchScreen_ForceDispose, delegates["TouchScreen_ForceDispose"]);
        AssignMethod(out ApiDelegator.Cursor_New, delegates["Cursor_New"]);
        AssignMethod(out ApiDelegator.Cursor_GetEnabled, delegates["Cursor_GetEnabled"]);
        AssignMethod(out ApiDelegator.Cursor_SetEnabled, delegates["Cursor_SetEnabled"]);
        AssignMethod(out ApiDelegator.Cursor_GetScale, delegates["Cursor_GetScale"]);
        AssignMethod(out ApiDelegator.Cursor_SetScale, delegates["Cursor_SetScale"]);
        AssignMethod(out ApiDelegator.Cursor_GetPosition, delegates["Cursor_GetPosition"]);
        AssignMethod(out ApiDelegator.Cursor_IsInsideArea, delegates["Cursor_IsInsideArea"]);
        AssignMethod(out ApiDelegator.Cursor_GetSprites, delegates["Cursor_GetSprites"]);
        AssignMethod(out ApiDelegator.Cursor_ForceDispose, delegates["Cursor_ForceDispose"]);
        AssignMethod(out ApiDelegator.ClickHandler_New, delegates["ClickHandler_New"]);
        AssignMethod(out ApiDelegator.ClickHandler_GetHitArea, delegates["ClickHandler_GetHitArea"]);
        AssignMethod(out ApiDelegator.ClickHandler_SetHitArea, delegates["ClickHandler_SetHitArea"]);
        AssignMethod(out ApiDelegator.ClickHandler_Update, delegates["ClickHandler_Update"]);
        AssignMethod(out ApiDelegator.ClickHandler_GetMouse1, delegates["ClickHandler_GetMouse1"]);
        AssignMethod(out ApiDelegator.ClickHandler_GetMouse2, delegates["ClickHandler_GetMouse2"]);
        AssignMethod(out ApiDelegator.ClickHandler_GetMouse3, delegates["ClickHandler_GetMouse3"]);
        AssignMethod(out ApiDelegator.ButtonState_New, delegates["ButtonState_New"]);
        AssignMethod(out ApiDelegator.ButtonState_IsReleased, delegates["ButtonState_IsReleased"]);
        AssignMethod(out ApiDelegator.ButtonState_IsOver, delegates["ButtonState_IsOver"]);
        AssignMethod(out ApiDelegator.ButtonState_IsPressed, delegates["ButtonState_IsPressed"]);
        AssignMethod(out ApiDelegator.ButtonState_JustReleased, delegates["ButtonState_JustReleased"]);
        AssignMethod(out ApiDelegator.ButtonState_JustPressed, delegates["ButtonState_JustPressed"]);
        AssignMethod(out ApiDelegator.ButtonState_Update, delegates["ButtonState_Update"]);
        IsReady = true;
      }
    }
    private void AssignMethod<T>(out T field, object method) => field = (T)method;
  }
  public class TouchApiDelegator
  {
    public Func<object, IMyCubeBlock> TouchScreen_GetBlock;
    public Func<object, IMyTextSurface> TouchScreen_GetSurface;
    public Func<object, int> TouchScreen_GetIndex;
    public Func<object, bool> TouchScreen_IsOnScreen;
    public Func<object, object> TouchScreen_GetMouse1;
    public Func<object, object> TouchScreen_GetMouse2;
    public Func<object, object> TouchScreen_GetMouse3;
    public Func<object, Vector2> TouchScreen_GetCursorPosition;
    public Func<object, float> TouchScreen_GetInteractiveDistance;
    public Action<object, float> TouchScreen_SetInteractiveDistance;
    public Func<object, int> TouchScreen_GetRotation;
    public Func<object, IMyCubeBlock, IMyTextSurface, bool> TouchScreen_CompareWithBlockAndSurface;
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
  public class TouchScreen : WrapperBase<TouchApiDelegator>
  {
    private ButtonState _mouse1;
    private ButtonState _mouse2;
    private ButtonState _mouse3;
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchScreen(object internalObject) : base(internalObject) { }
    public IMyCubeBlock Block { get { return Api.TouchScreen_GetBlock.Invoke(InternalObj); } }
    public IMyTextSurface Surface { get { return Api.TouchScreen_GetSurface.Invoke(InternalObj); } }
    public int Index { get { return Api.TouchScreen_GetIndex.Invoke(InternalObj); } }
    public bool IsOnScreen { get { return Api.TouchScreen_IsOnScreen.Invoke(InternalObj); } }
    public ButtonState Mouse1 { get { return _mouse1 ?? (_mouse1 = Wrap<ButtonState>(Api.TouchScreen_GetMouse1.Invoke(InternalObj), (obj) => new ButtonState(obj))); } }
    public ButtonState Mouse2 { get { return _mouse2 ?? (_mouse2 = Wrap<ButtonState>(Api.TouchScreen_GetMouse2.Invoke(InternalObj), (obj) => new ButtonState(obj))); } }
    public ButtonState Mouse3 { get { return _mouse3 ?? (_mouse3 = Wrap<ButtonState>(Api.TouchScreen_GetMouse3.Invoke(InternalObj), (obj) => new ButtonState(obj))); } }
    public Vector2 CursorPosition { get { return Api.TouchScreen_GetCursorPosition.Invoke(InternalObj); } }
    public float InteractiveDistance { get { return Api.TouchScreen_GetInteractiveDistance.Invoke(InternalObj); } set { Api.TouchScreen_SetInteractiveDistance.Invoke(InternalObj, value); } }
    public int Rotation { get { return Api.TouchScreen_GetRotation.Invoke(InternalObj); } }
    public bool CompareWithBlockAndSurface(IMyCubeBlock block, IMyTextSurface surface) => Api.TouchScreen_CompareWithBlockAndSurface.Invoke(InternalObj, block, surface);
    public void ForceDispose() => Api.TouchScreen_ForceDispose.Invoke(InternalObj);
  }
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
