using Lima.Utils;
using Sandbox.ModAPI;
using System.Linq;
using System;
using VRage.Game.ModAPI;
using VRageMath;

namespace Lima.Touch
{
  public class TouchScreen
  {
    public bool Active { get; set; }

    public event Action UpdateAfterSimulationEvent;

    public string SubtypeId { get { return Block.BlockDefinition.SubtypeId; } }

    public IMyCubeBlock Block { get; private set; }
    public IMyTextSurface Surface { get; private set; }
    public int Index { get; private set; }
    public SurfaceCoords Coords { get; private set; }
    public RectangleF Viewport { get; private set; }
    public bool IsOnScreen { get; private set; }
    public Vector2 CursorPosition { get; private set; }
    public Vector3 Intersection { get; private set; }
    public float _interactiveDistance = TouchSession.Instance.TouchMan.DefaultInteractiveDistance;
    public float InteractiveDistance
    {
      get { return _interactiveDistance; }
      set { _interactiveDistance = MathHelper.Clamp(value, 0, TouchSession.Instance.TouchMan.MaxInteractiveDistance); }
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

      var coordString = TouchSession.Instance.SurfaceCoordsMan.CoordsList.SingleOrDefault(c => c.StartsWith($"{SurfaceCoords.Prefix}:{SubtypeId}:{Index}"));
      if (coordString == null)
        throw new Exception($"Can't find coords for {SubtypeId}:{Index}");

      SurfaceCoords coords = SurfaceCoords.Parse(coordString);
      if (coords.IsEmpty())
        throw new Exception($"Failed to parse coords for {SubtypeId}:{Index}");

      Active = true;
      Coords = coords;
      Viewport = new RectangleF(
        (surface.TextureSize - surface.SurfaceSize) * 0.5f,
        surface.SurfaceSize
      );
    }

    public void ForceRotationUpdate()
    {
      _rotation = -1;
    }

    // private MyStringId Material = MyStringId.GetOrCompute("Square");
    // void DrawPoint(Vector3D position)
    // {
    //   Color color = Color.Red;
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
      // // DrawPoint(Intersection);
      // DrawPoint(screenPosTL);
      // DrawPoint(screenPosBL);
      // DrawPoint(screenPosBR);

      return Intersection;
    }

    public Vector2 UpdateScreenCoord()
    {
      var screenPosTL = MathUtils.LocalToGlobal(Coords.TopLeft, Block.WorldMatrix);
      var screenPosBL = MathUtils.LocalToGlobal(Coords.BottomLeft, Block.WorldMatrix);
      var screenPosBR = MathUtils.LocalToGlobal(Coords.BottomRight, Block.WorldMatrix);
      var screenUp = Vector3.Normalize(screenPosTL - screenPosBL);
      var screenRight = Vector3.Normalize(screenPosBL - screenPosBR);

      Vector2 screenCoord = MathUtils.GetPointOnPlane(Intersection, screenPosTL, screenUp, screenRight);
      var width = Vector3.Distance(screenPosBL, screenPosBR);
      var height = Vector3.Distance(screenPosBL, screenPosTL);

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
      var provider = block as Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider;
      if (provider == null)
        throw new Exception($"Block is not a IMyTextSurfaceProvider {block}");
      var i = SurfaceUtils.GetSurfaceIndex(provider, surface);

      return Block == block && (Surface == surface || Index == i);
    }

    public void UpdateAfterSimulation()
    {
      UpdateAfterSimulationEvent?.Invoke();
    }

    public void Dispose()
    {
      UpdateAfterSimulationEvent = null;
    }

    public bool IsInsideArea(float x, float y, float z, float w)
    {
      if (!IsOnScreen || !Active)
        return false;

      return CursorPosition.X >= x && CursorPosition.Y >= y && CursorPosition.X <= z && CursorPosition.Y <= w;
    }
  }
}