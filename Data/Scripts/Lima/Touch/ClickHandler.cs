using Sandbox.ModAPI;
using VRageMath;

namespace Lima.Touch
{
  public class ClickHandler
  {
    private bool _wasPressed = false;
    public Vector4 HitArea = Vector4.Zero;

    private int _status = 0;
    public bool IsMouseReleased { get { return _status == 0; } }
    public bool IsMouseOver { get { return _status == 1 || _status == 2; } }
    public bool IsMousePressed { get { return _status == 2; } }
    public bool WasPressedOutside { get { return _wasPresseOutside; } }

    public bool JustReleased { get; private set; }
    public bool JustPressed { get; private set; }

    public ClickHandler() { }

    private bool _wasPressedInside = false;
    private bool _wasPresseOutside = false;

    public void UpdateStatus(TouchScreen screen)
    {
      if (JustReleased)
        _wasPressed = false;
      JustReleased = false;
      JustPressed = false;
      _status = 0;

      if (HitArea != Vector4.Zero)
      {
        // var mousePressed = MyAPIGateway.Input.IsMousePressed(MyMouseButtonsEnum.Left)
        var mousePressed = !MyAPIGateway.Gui.IsCursorVisible && (MyAPIGateway.Input.IsLeftMousePressed() || MyAPIGateway.Input.IsMiddleMousePressed());
        if (screen.IsInsideArea(
            HitArea.X,
            HitArea.Y,
            HitArea.Z,
            HitArea.W
          ))
        {
          if (mousePressed && !_wasPresseOutside)
          {
            _status = 2;
            if (!_wasPressed)
              JustPressed = true;
            _wasPressed = true;
            _wasPressedInside = true;
          }
          else if (_wasPressed)
          {
            JustReleased = true;
            _wasPressed = false;
            _status = 1;
            _wasPresseOutside = false;
          }
          else
          {
            _status = 1;
            if (!mousePressed)
              _wasPresseOutside = false;
          }
        }
        else
        {
          _wasPresseOutside = !_wasPressedInside && mousePressed;
          if (!mousePressed)
          {
            _wasPressedInside = false;
            _wasPressed = false;
          }
        }
      }
    }

  }
}