using System;
using VRageMath;

namespace Lima.Utils
{
  internal static class MathUtils
  {
    public static Vector3D GetLinePlaneIntersection(Vector3D planePoint, Vector3D planeNormal, Vector3D linePoint, Vector3D lineDirection, bool ignoreDirection = false)
    {
      var planeDotLine = planeNormal.Dot(Vector3D.Normalize(lineDirection));
      if (planeDotLine == 0 || (!ignoreDirection && planeDotLine >= 0))
      {
        return Vector3D.Zero;
      }

      double t = (planeNormal.Dot(planePoint) - planeNormal.Dot(linePoint)) / planeDotLine;
      if (!ignoreDirection && t <= 0)
      {
        return Vector3D.Zero;
      }
      return linePoint + (Vector3D.Normalize(lineDirection) * t);
    }

    public static Vector2 GetPointOnPlane(Vector3D point, Vector3D planePoint, Vector3D planeUp, Vector3D planeRight)
    {
      var x = planeRight.Dot(planePoint - point);
      var y = planeUp.Dot(planePoint - point);
      return new Vector2((float)x, (float)y);
    }

    public static Vector3 LocalToGlobal(Vector3 localPos, MatrixD worldMatrix)
    {
      return Vector3D.Transform(localPos, worldMatrix);
    }

    public static Vector3 GlobalToLocal(Vector3D globalPos, MatrixD worldMatrix)
    {
      return Vector3D.TransformNormal(globalPos - worldMatrix.Translation, MatrixD.Transpose(worldMatrix));
    }

    public static Vector3 GlobalToLocalSlowerAlternative(Vector3D globalPos, MatrixD worldMatrixNormalizedInv)
    {
      return Vector3D.Transform(globalPos, worldMatrixNormalizedInv);
    }

    private static Random random = new Random();
    public static int GetRandomInt(int min, int max)
    {
      return random.Next(min, max);
    }
  }
}