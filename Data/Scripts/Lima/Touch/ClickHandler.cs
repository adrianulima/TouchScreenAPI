using VRageMath;

namespace Lima.Touch
{
  public class ClickHandler
  {
    public Vector4 HitArea = Vector4.Zero;

    public ButtonState Mouse1 = new ButtonState();
    public ButtonState Mouse2 = new ButtonState();
    public ButtonState Mouse3 = new ButtonState();

    public ClickHandler() { }

    public void Update(TouchScreen screen)
    {
      if (HitArea != Vector4.Zero)
      {
        var isInsideArea = screen.IsInsideArea(HitArea.X, HitArea.Y, HitArea.Z, HitArea.W);
        Mouse1.Update(screen.Mouse1.IsPressed, isInsideArea);
        Mouse2.Update(screen.Mouse2.IsPressed, isInsideArea);
        Mouse3.Update(screen.Mouse3.IsPressed, isInsideArea);
      }
    }

  }
}