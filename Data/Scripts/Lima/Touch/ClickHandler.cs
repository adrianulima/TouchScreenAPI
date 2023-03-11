using VRageMath;

namespace Lima.Touch
{
  public class ClickHandler
  {
    public Vector4 HitArea = Vector4.Zero;

    public ButtonState Mouse1 = new ButtonState();
    public ButtonState Mouse2 = new ButtonState();

    public ClickHandler() { }

    public void Update(TouchScreen screen)
    {
      if (HitArea != Vector4.Zero)
      {
        var mousePressed = screen.Mouse1.IsPressed;
        var mouse2Pressed = screen.Mouse2.IsPressed;
        var isInsideArea = screen.IsInsideArea(HitArea.X, HitArea.Y, HitArea.Z, HitArea.W);

        Mouse1.Update(mousePressed, isInsideArea);
        Mouse2.Update(mouse2Pressed, isInsideArea);
      }
    }

  }
}