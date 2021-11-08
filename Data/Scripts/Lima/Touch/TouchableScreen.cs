using System;
using Lima.Fancy;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Lima.Touch
{
  public class TouchableScreen
  {
    public string SubtypeId { get { return Block.BlockDefinition.SubtypeId; } }

    public IMyCubeBlock Block { get; private set; }
    public int Index { get; private set; }
    public SurfaceCoords Coords { get; private set; }
    public RectangleF Viewport { get; private set; }
    public bool IsOnScreen { get; private set; }
    public Vector2 CursorPos { get; private set; }
    public Vector3 Intersection { get; private set; }

    private int _rotate = -1;
    public int ScreenRotate
    {
      get
      {
        // TODO: Make this call happen again with some refresh command
        if (_rotate < 0)
        {
          var builder = (Block.GetObjectBuilderCubeBlock() as Sandbox.Common.ObjectBuilders.MyObjectBuilder_TextPanel);
          if (builder != null)
            _rotate = (int)builder.SelectedRotationIndex;
          else
            _rotate = 0;
        }
        return _rotate;
      }
    }

    public TouchableScreen(IMyCubeBlock block, IMyTextSurface surface)
    {
      Block = block;
      Index = FancyUtils.GetSurfaceIndex(block as IMyTextSurfaceProvider, surface);

      SurfaceCoords coords = SurfaceCoords.Zero;
      SurfaceCoordsManager.Instance.CoordsList.TryGetValue($"{SubtypeId}:{Index}", out coords);

      // MyAPIGateway.Utilities.ShowNotification($"{SubtypeId}:{Index}", 5000, "Green");

      if (coords.IsEmpty())
        throw new Exception($"Can't find coords for {SubtypeId}:{Index}");

      Coords = coords;
      Viewport = new RectangleF(
        (surface.TextureSize - surface.SurfaceSize) * 0.5f,
        surface.SurfaceSize
      );
    }

    private MyStringId Material = MyStringId.GetOrCompute("Square");
    void DrawPoint(Vector3D position)
    {
      Color color = Color.Red;
      MyTransparentGeometry.AddPointBillboard(Material, color, position, 0.05f, 0f);
    }

    void DrawLine(Vector3D position, Vector3D direction, float length, float lineThickness = 0.05f)
    {
      Color color = Color.Blue;
      MyTransparentGeometry.AddLineBillboard(Material, color, position, direction, length, lineThickness);
    }

    public Vector3D CalculateIntersection(MatrixD cameraMatrix)
    {
      IsOnScreen = false;

      var camPosition = cameraMatrix.Translation;
      var camDirection = cameraMatrix.Forward;

      var screenPosTL = FancyUtils.LocalToGlobal(Coords.TopLeft, Block.WorldMatrix);
      var screenNormal = Coords.CalculateNormal(Block.WorldMatrix);

      // MyAPIGateway.Utilities.ShowNotification($"{headPos} {camPosition}", 1, "Green");
      Intersection = FancyUtils.GetLinePlaneIntersection(screenPosTL, screenNormal, camPosition, camDirection);


      var screenPosBL = FancyUtils.LocalToGlobal(Coords.BottomLeft, Block.WorldMatrix);
      var screenPosBR = FancyUtils.LocalToGlobal(Coords.BottomRight, Block.WorldMatrix);
      // DrawPoint(Intersection);
      DrawPoint(screenPosTL);
      DrawPoint(screenPosBL);
      DrawPoint(screenPosBR);

      return Intersection;
    }


    public Vector2 UpdateScreenCoord()
    {
      var screenPosTL = FancyUtils.LocalToGlobal(Coords.TopLeft, Block.WorldMatrix);
      var screenPosBL = FancyUtils.LocalToGlobal(Coords.BottomLeft, Block.WorldMatrix);
      var screenPosBR = FancyUtils.LocalToGlobal(Coords.BottomRight, Block.WorldMatrix);
      var screenUp = Vector3.Normalize(screenPosTL - screenPosBL);
      var screenRight = Vector3.Normalize(screenPosBL - screenPosBR);

      Vector2 screenCoord = FancyUtils.GetPointOnPlane(Intersection, screenPosTL, screenUp, screenRight);
      var width = Vector3.Distance(screenPosBL, screenPosBR);
      var height = Vector3.Distance(screenPosBL, screenPosTL);

      if (screenCoord.X >= 0 && screenCoord.X <= width && screenCoord.Y >= 0 && screenCoord.Y <= height)
      {
        var rotatedCoord = FancyUtils.RotateScreenCoord(new Vector2(((screenCoord.X / width)), ((screenCoord.Y / height))), ScreenRotate);
        var pX = rotatedCoord.X * Viewport.Width + Viewport.X;
        var pY = rotatedCoord.Y * Viewport.Height + Viewport.Y;

        IsOnScreen = true;
        CursorPos = new Vector2(pX, pY);
        return CursorPos;
      }

      return Vector2.Zero;
    }
  }
}