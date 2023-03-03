using Sandbox.ModAPI;
using VRageMath;

namespace Lima.Utils
{
  internal static class SurfaceUtils
  {
    internal static int GetSurfaceIndex(Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider provider, IMyTextSurface surface)
    {
      var count = provider.SurfaceCount;
      for (int i = 0; i < count; i++)
      {
        if (provider.GetSurface(i) == surface)
          return i;
      }
      return -1;
    }

    internal static Vector2 RotateScreenCoord(Vector2 coord, int rotate)
    {
      if (rotate == 0)
        return coord;
      else if (rotate == 1)
        return new Vector2(coord.Y, 1 - coord.X);
      else if (rotate == 2)
        return new Vector2(1 - coord.X, 1 - coord.Y);
      else
        return new Vector2(1 - coord.Y, coord.X);
    }
  }
}