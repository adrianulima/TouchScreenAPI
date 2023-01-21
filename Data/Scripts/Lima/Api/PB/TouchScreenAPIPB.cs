using Sandbox.ModAPI.Interfaces;
using System.Collections.Generic;
using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;
using IngameIMyTextSurface = Sandbox.ModAPI.Ingame.IMyTextSurface;
using IngameIMyCubeBlock = VRage.Game.ModAPI.Ingame.IMyCubeBlock;

namespace Lima.API.PB.UI
{
  // Copy these classes to inside your PB Program Class
  // This has only the TouchScreen feature, no UI kit
  public class TouchScreenAPI
  {
    public bool IsReady { get; protected set; }
    private Func<IngameIMyCubeBlock, IngameIMyTextSurface, object> _createTouchScreen;
    private Action<IngameIMyCubeBlock, IngameIMyTextSurface> _removeTouchScreen;
    private Action<string> _addSurfaceCoords;
    private Action<string> _removeSurfaceCoords;
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
        AssignMethod(out ApiDelegator.TouchScreen_GetBlock, delegates["TouchScreen_GetBlock"]);
        AssignMethod(out ApiDelegator.TouchScreen_GetSurface, delegates["TouchScreen_GetSurface"]);
        AssignMethod(out ApiDelegator.TouchScreen_GetIndex, delegates["TouchScreen_GetIndex"]);
        AssignMethod(out ApiDelegator.TouchScreen_IsOnScreen, delegates["TouchScreen_IsOnScreen"]);
        AssignMethod(out ApiDelegator.TouchScreen_GetCursorPosition, delegates["TouchScreen_GetCursorPosition"]);
        AssignMethod(out ApiDelegator.TouchScreen_GetInteractiveDistance, delegates["TouchScreen_GetInteractiveDistance"]);
        AssignMethod(out ApiDelegator.TouchScreen_SetInteractiveDistance, delegates["TouchScreen_SetInteractiveDistance"]);
        AssignMethod(out ApiDelegator.TouchScreen_GetRotation, delegates["TouchScreen_GetRotation"]);
        AssignMethod(out ApiDelegator.TouchScreen_CompareWithBlockAndSurface, delegates["TouchScreen_CompareWithBlockAndSurface"]);
        AssignMethod(out ApiDelegator.TouchScreen_ForceDispose, delegates["TouchScreen_ForceDispose"]);
        AssignMethod(out ApiDelegator.TouchCursor_New, delegates["TouchCursor_New"]);
        AssignMethod(out ApiDelegator.TouchCursor_GetActive, delegates["TouchCursor_GetActive"]);
        AssignMethod(out ApiDelegator.TouchCursor_SetActive, delegates["TouchCursor_SetActive"]);
        AssignMethod(out ApiDelegator.TouchCursor_GetScale, delegates["TouchCursor_GetScale"]);
        AssignMethod(out ApiDelegator.TouchCursor_SetScale, delegates["TouchCursor_SetScale"]);
        AssignMethod(out ApiDelegator.TouchCursor_GetPosition, delegates["TouchCursor_GetPosition"]);
        AssignMethod(out ApiDelegator.TouchCursor_IsInsideArea, delegates["TouchCursor_IsInsideArea"]);
        AssignMethod(out ApiDelegator.TouchCursor_GetSprites, delegates["TouchCursor_GetSprites"]);
        AssignMethod(out ApiDelegator.TouchCursor_ForceDispose, delegates["TouchCursor_ForceDispose"]);
        AssignMethod(out ApiDelegator.ClickHandler_New, delegates["ClickHandler_New"]);
        AssignMethod(out ApiDelegator.ClickHandler_GetHitArea, delegates["ClickHandler_GetHitArea"]);
        AssignMethod(out ApiDelegator.ClickHandler_SetHitArea, delegates["ClickHandler_SetHitArea"]);
        AssignMethod(out ApiDelegator.ClickHandler_IsMouseReleased, delegates["ClickHandler_IsMouseReleased"]);
        AssignMethod(out ApiDelegator.ClickHandler_IsMouseOver, delegates["ClickHandler_IsMouseOver"]);
        AssignMethod(out ApiDelegator.ClickHandler_IsMousePressed, delegates["ClickHandler_IsMousePressed"]);
        AssignMethod(out ApiDelegator.ClickHandler_JustReleased, delegates["ClickHandler_JustReleased"]);
        AssignMethod(out ApiDelegator.ClickHandler_JustPressed, delegates["ClickHandler_JustPressed"]);
        AssignMethod(out ApiDelegator.ClickHandler_UpdateStatus, delegates["ClickHandler_UpdateStatus"]);
        IsReady = true;
      }
    }
    private void AssignMethod<T>(out T field, object method) => field = (T)method;
  }
  public class TouchApiDelegator
  {
    public Func<object, IngameIMyCubeBlock> TouchScreen_GetBlock;
    public Func<object, IngameIMyTextSurface> TouchScreen_GetSurface;
    public Func<object, int> TouchScreen_GetIndex;
    public Func<object, bool> TouchScreen_IsOnScreen;
    public Func<object, Vector2> TouchScreen_GetCursorPosition;
    public Func<object, float> TouchScreen_GetInteractiveDistance;
    public Action<object, float> TouchScreen_SetInteractiveDistance;
    public Func<object, int> TouchScreen_GetRotation;
    public Func<object, IngameIMyCubeBlock, IngameIMyTextSurface, bool> TouchScreen_CompareWithBlockAndSurface;
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
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchScreen(object internalObject) : base(internalObject) { }
    public IngameIMyCubeBlock Block { get { return Api.TouchScreen_GetBlock.Invoke(InternalObj); } }
    public IngameIMyTextSurface Surface { get { return Api.TouchScreen_GetSurface.Invoke(InternalObj); } }
    public int Index { get { return Api.TouchScreen_GetIndex.Invoke(InternalObj); } }
    public bool IsOnScreen { get { return Api.TouchScreen_IsOnScreen.Invoke(InternalObj); } }
    public Vector2 CursorPosition { get { return Api.TouchScreen_GetCursorPosition.Invoke(InternalObj); } }
    public float InteractiveDistance { get { return Api.TouchScreen_GetInteractiveDistance.Invoke(InternalObj); } }
    public void SetInteractiveDistance(float distance) => Api.TouchScreen_SetInteractiveDistance.Invoke(InternalObj, distance);
    public int Rotation { get { return Api.TouchScreen_GetRotation.Invoke(InternalObj); } }
    public bool CompareWithBlockAndSurface(IngameIMyCubeBlock block, IngameIMyTextSurface surface) => Api.TouchScreen_CompareWithBlockAndSurface.Invoke(InternalObj, block, surface);
    public void ForceDispose() => Api.TouchScreen_ForceDispose.Invoke(InternalObj);
  }
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
