using Lima.Touch;
using Sandbox.ModAPI;
using VRageMath;

namespace Lima.Fancy
{
  public class ClickHandler
  {
    private bool _wasPressed = false;
    public Vector4 hitArea = Vector4.Zero;

    private int _status = 0;
    public bool IsMouseReleased { get { return _status == 0; } }
    public bool IsMouseOver { get { return _status == 1 || _status == 2; } }
    public bool IsMousePressed { get { return _status == 2; } }

    public bool JustReleased { get; private set; }
    public bool JustPressed { get; private set; }

    public ClickHandler() { }

    public void UpdateStatus(TouchScreen screen)
    {
      if (JustReleased)
        _wasPressed = false;
      JustReleased = false;
      JustPressed = false;
      _status = 0;

      if (hitArea != Vector4.Zero)
      {
        if (screen.IsInsideArea(
            hitArea.X,
            hitArea.Y,
            hitArea.Z,
            hitArea.W
          ))
        {
          // if (MyAPIGateway.Input.IsMousePressed(MyMouseButtonsEnum.Left))
          if (MyAPIGateway.Input.IsAnyMouseOrJoystickPressed())
          {
            _status = 2;
            if (!_wasPressed)
              JustPressed = true;
            _wasPressed = true;
          }
          else if (_wasPressed)
          {
            _status = 1;
            JustReleased = true;
            _wasPressed = false;
          }
          else
          {
            _status = 1;
          }
        }
      }
    }

  }
}