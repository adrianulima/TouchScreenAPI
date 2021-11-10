using System;
using System.Collections.Generic;
using Lima.Fancy;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

//Change namespace to your mod's namespace
namespace Lima.Touch
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
    private Func<IMyCubeBlock, IMyTextSurface, ITouchScreen> _createTouchScreen;
    private Action<IMyCubeBlock, IMyTextSurface> _removeTouchScreen;
    private Action<string> _addSurfaceCoords;
    private Action<string> _removeSurfaceCoords;

    public float GetMaxInteractiveDistance() => _getMaxInteractiveDistance?.Invoke() ?? -1f;
    public void SetMaxInteractiveDistance(float distance) => _setMaxInteractiveDistance?.Invoke(distance);
    public ITouchScreen CreateTouchScreen(IMyCubeBlock block, IMyTextSurface surface) => _createTouchScreen?.Invoke(block, surface);
    public void RemoveTouchScreen(IMyCubeBlock block, IMyTextSurface surface) => _removeTouchScreen?.Invoke(block, surface);
    public void AddSurfaceCoords(string coords) => _addSurfaceCoords?.Invoke(coords);
    public void RemoveSurfaceCoords(string coords) => _removeSurfaceCoords?.Invoke(coords);

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
      AssignMethod(delegates, "CreateTouchScreen", ref _createTouchScreen);
      AssignMethod(delegates, "RemoveTouchScreen", ref _removeTouchScreen);
      AssignMethod(delegates, "AddSurfaceCoords", ref _addSurfaceCoords);
      AssignMethod(delegates, "RemoveSurfaceCoords", ref _removeSurfaceCoords);
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

  public interface ITouchScreen
  {
    bool Active { get; set; }
    int Index { get; }
    RectangleF Viewport { get; }
    bool IsOnScreen { get; }
    Vector2 CursorPos { get; }
    Vector3 Intersection { get; }
    float InteractiveDistance { get; set; }
    IMyCubeBlock Block { get; }
    IMyTextSurface Surface { get; }
    void ForceRotationUpdate();
    // string SubtypeId { get; }
    // ISurfaceCoords Coords { get; }
    // int ScreenRotate { get; }

    // Vector3D CalculateIntersection(MatrixD cameraMatrix);
    // Vector2 UpdateScreenCoord();
  }

  // public interface ISurfaceCoords
  // {
  //   Vector3 TopLeft { get; }
  //   Vector3 BottomLeft { get; }
  //   Vector3 BottomRight { get; }
  // }
}