using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI;
using System.Collections.Generic;
using System;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRageMath;

namespace Lima.PBAPI
{
  public class TouchScreenAPI
  {
    public bool IsReady { get; protected set; }
    private Func<IMyCubeBlock, IMyTextSurface, object> _createTouchScreen;
    private Action<IMyCubeBlock, IMyTextSurface> _removeTouchScreen;
    private Action<string> _addSurfaceCoords;
    private Action<string> _removeSurfaceCoords;
    public object CreateTouchScreen(IMyCubeBlock block, IMyTextSurface surface) => _createTouchScreen?.Invoke(block, surface);
    public void RemoveTouchScreen(IMyCubeBlock block, IMyTextSurface surface) => _removeTouchScreen?.Invoke(block, surface);
    public void AddSurfaceCoords(string coords) => _addSurfaceCoords?.Invoke(coords);
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
  public class TouchCursor : WrapperBase<TouchApiDelegator>
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
  public class ClickHandler : WrapperBase<TouchApiDelegator>
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
