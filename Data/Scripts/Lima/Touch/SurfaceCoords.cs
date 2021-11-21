using System;
using Lima.Utils;
using VRageMath;

namespace Lima.Touch
{
  public struct SurfaceCoords : IEquatable<SurfaceCoords>
  {
    // Builder TypeId
    public string BuilderTypeString;

    // SurfaceProvider surface index
    public int Index;

    // Top left screen corner
    public Vector3 TopLeft;

    // Top right screen corner
    public Vector3 BottomLeft;

    // Bottom left 
    public Vector3 BottomRight;

    public SurfaceCoords(string builderTypeString, int index, Vector3 topLeft, Vector3 bottomLeft, Vector3 bottomRight)
    {
      this.BuilderTypeString = builderTypeString;
      this.Index = index;
      this.TopLeft = topLeft;
      this.BottomLeft = bottomLeft;
      this.BottomRight = bottomRight;
    }

    public bool IsEmpty()
    {
      return this.Equals(Zero);
    }

    public Vector3D CalculateNormal(MatrixD blockWorldMatrix)
    {
      var A = MathUtils.LocalToGlobal(TopLeft, blockWorldMatrix);
      var B = MathUtils.LocalToGlobal(BottomLeft, blockWorldMatrix);
      var C = MathUtils.LocalToGlobal(BottomRight, blockWorldMatrix);
      return Vector3.Normalize(Vector3.Cross(B - A, C - A));
    }

    public override string ToString() => $"{Prefix}:{BuilderTypeString}:{Index}:{TopLeft.X}:{TopLeft.Y}:{TopLeft.Z}:{BottomLeft.X}:{BottomLeft.Y}:{BottomLeft.Z}:{BottomRight.X}:{BottomRight.Y}:{BottomRight.Z}";

    public override bool Equals(object obj)
    {
      if (!(obj is SurfaceCoords))
        return false;

      return Equals(this, (SurfaceCoords)obj);
    }

    public bool Equals(SurfaceCoords other)
    {
      return this.Index == other.Index && this.BuilderTypeString == other.BuilderTypeString && this.TopLeft == other.TopLeft && this.BottomLeft == other.BottomLeft && this.BottomRight == other.BottomRight;
    }

    public override int GetHashCode()
    {
      return Index.GetHashCode() ^ BuilderTypeString.GetHashCode() ^ TopLeft.GetHashCode() ^ BottomLeft.GetHashCode() ^ BottomRight.GetHashCode();
    }

    public static bool operator ==(SurfaceCoords value1, SurfaceCoords value2)
    {
      return value1.Equals(value2);
    }

    public static bool operator !=(SurfaceCoords value1, SurfaceCoords value2)
    {
      return !value1.Equals(value2);
    }

    public static string Prefix = "TOUCH";

    public static SurfaceCoords Zero = new SurfaceCoords(null, 0, Vector3.Zero, Vector3.Zero, Vector3.Zero);

    public static SurfaceCoords Parse(string strigifiedCoords)
    {
      var args = strigifiedCoords.Split(':');
      if (args.Length < 12)
        return Zero;

      return new SurfaceCoords(
        args[1],
        int.Parse(args[2]),
        new Vector3(float.Parse(args[3]), float.Parse(args[4]), float.Parse(args[5])),
        new Vector3(float.Parse(args[6]), float.Parse(args[7]), float.Parse(args[8])),
        new Vector3(float.Parse(args[9]), float.Parse(args[10]), float.Parse(args[11]))
      );
    }
  }
}