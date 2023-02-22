using Lima.Utils;
using Sandbox.ModAPI;
using System;
using VRage.Game.ModAPI;
// using VRage.Game;
// using VRage.Utils;
using VRageMath;

namespace Lima.Touch
{
  public class TouchScreen
  {
    public bool Active { get; set; }

    public event Action UpdateAtSimulationEvent;

    public string SubtypeId { get { return Block.BlockDefinition.SubtypeId; } }

    public IMyCubeBlock Block { get; private set; }
    public IMyTextSurface Surface { get; private set; }
    public int Index { get; private set; }
    public SurfaceCoords Coords { get; private set; }
    public RectangleF Viewport { get; private set; }
    public bool IsOnScreen { get; private set; }
    public Vector2 CursorPosition { get; private set; }
    public Vector3D Intersection { get; private set; }
    public float _interactiveDistance = 10;
    public float InteractiveDistance
    {
      get { return _interactiveDistance; }
      set { _interactiveDistance = value > 0 ? value : 0; }
    }

    private int _rotation = -1;
    public int Rotation
    {
      get
      {
        if (_rotation < 0)
        {
          _rotation = 0;
          if (Block is IMyTextPanel)
          {
            var builder = (Block.GetObjectBuilderCubeBlock() as Sandbox.Common.ObjectBuilders.MyObjectBuilder_TextPanel);
            if (builder != null)
            {
              _rotation = (int)builder.SelectedRotationIndex;
              return _rotation;
            }
          }
        }
        return _rotation;
      }
    }

    public TouchScreen(IMyCubeBlock block, IMyTextSurface surface)
    {
      Block = block;
      Surface = surface;

      var provider = block as Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider;
      if (provider == null)
        throw new Exception($"Block is not a IMyTextSurfaceProvider {block}");
      Index = SurfaceUtils.GetSurfaceIndex(provider, surface);

      RefreshCoords();
      Active = true;
      Viewport = new RectangleF(
        (surface.TextureSize - surface.SurfaceSize) * 0.5f,
        surface.SurfaceSize
      );
    }

    public void RefreshCoords()
    {
      SurfaceCoords coords = SurfaceCoords.Zero;
      var coordString = TouchSession.Instance.SurfaceCoordsMan.GetSurfaceCoords(SubtypeId, Index);
      if (coordString != "")
      {
        coords = SurfaceCoords.Parse(coordString);
        if (coords.IsEmpty())
          throw new Exception($"Failed to parse coords for {SubtypeId}:{Index}");
      }

      Coords = coords;
    }

    public void ForceRotationUpdate()
    {
      _rotation = -1;
    }

    // private MyStringId Material = MyStringId.GetOrCompute("Square");
    // void DrawPoint(Vector3D position, Color color)
    // {
    //   MyTransparentGeometry.AddPointBillboard(Material, color, position, 0.05f, 0f);
    // }

    // void DrawLine(Vector3D position, Vector3D direction, float length, float lineThickness = 0.05f)
    // {
    //   Color color = Color.Blue;
    //   MyTransparentGeometry.AddLineBillboard(Material, color, position, direction, length, lineThickness);
    // }

    public Vector3D CalculateIntersection(MatrixD cameraMatrix)
    {
      IsOnScreen = false;

      var camPosition = cameraMatrix.Translation;
      var camDirection = cameraMatrix.Forward;

      var screenPosTL = MathUtils.LocalToGlobal(Coords.TopLeft, Block.WorldMatrix);
      var screenNormal = Coords.CalculateNormal(Block.WorldMatrix);

      // MyAPIGateway.Utilities.ShowNotification($"{headPos} {camPosition}", 1, "Green");
      Intersection = MathUtils.GetLinePlaneIntersection(screenPosTL, screenNormal, camPosition, camDirection);

      // var screenPosBL = MathUtils.LocalToGlobal(Coords.BottomLeft, Block.WorldMatrix);
      // var screenPosBR = MathUtils.LocalToGlobal(Coords.BottomRight, Block.WorldMatrix);
      // DrawPoint(Intersection, Color.Yellow);
      // DrawPoint(screenPosTL, Color.Red);
      // DrawPoint(screenPosBL, Color.Green);
      // DrawPoint(screenPosBR, Color.Blue);

      return Intersection;
    }

    public Vector2 UpdateScreenCoord()
    {
      var screenPosTL = MathUtils.LocalToGlobal(Coords.TopLeft, Block.WorldMatrix);
      var screenPosBL = MathUtils.LocalToGlobal(Coords.BottomLeft, Block.WorldMatrix);
      var screenPosBR = MathUtils.LocalToGlobal(Coords.BottomRight, Block.WorldMatrix);
      var screenUp = Vector3D.Normalize(screenPosTL - screenPosBL);
      var screenRight = Vector3D.Normalize(screenPosBL - screenPosBR);

      Vector2 screenCoord = MathUtils.GetPointOnPlane(Intersection, screenPosTL, screenUp, screenRight);
      var width = (float)Vector3D.Distance(screenPosBL, screenPosBR);
      var height = (float)Vector3D.Distance(screenPosBL, screenPosTL);

      if (screenCoord.X >= 0 && screenCoord.X <= width && screenCoord.Y >= 0 && screenCoord.Y <= height)
      {
        var rotatedCoord = SurfaceUtils.RotateScreenCoord(new Vector2(((screenCoord.X / width)), ((screenCoord.Y / height))), Rotation);
        var pX = rotatedCoord.X * Viewport.Width + Viewport.X;
        var pY = rotatedCoord.Y * Viewport.Height + Viewport.Y;

        IsOnScreen = true;
        CursorPosition = new Vector2(pX, pY);
        return CursorPosition;
      }

      return Vector2.Zero;
    }

    public bool CompareWithBlockAndSurface(IMyCubeBlock block, IMyTextSurface surface)
    {
      if (Block != block)
        return false;

      var provider = block as Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider;
      if (provider == null)
        return false;

      return (Block == block) && (Surface == surface || Index == SurfaceUtils.GetSurfaceIndex(provider, surface));
    }

    public void UpdateAtSimulation()
    {
      UpdateAtSimulationEvent?.Invoke();
    }

    public void Dispose()
    {
      UpdateAtSimulationEvent = null;
    }

    public bool IsInsideArea(float x, float y, float z, float w)
    {
      if (!IsOnScreen || !Active)
        return false;

      return CursorPosition.X >= x && CursorPosition.Y >= y && CursorPosition.X <= z && CursorPosition.Y <= w;
    }
  }
}