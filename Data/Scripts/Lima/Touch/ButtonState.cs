namespace Lima.Touch
{
  public class ButtonState
  {
    private int _status = 0;
    public bool IsReleased { get { return _status == 0; } }
    public bool IsOver { get { return _status == 1 || _status == 2; } }
    public bool IsPressed { get { return _status == 2; } }
    public bool WasPressedOutside { get { return _wasPresseOutside; } }
    public bool JustReleased { get; private set; }
    public bool JustPressed { get; private set; }

    public ButtonState() { }

    private bool _wasPressed = false;
    private bool _wasPressedInside = false;
    private bool _wasPresseOutside = false;

    public void Update(bool isPressed, bool isInsideArea)
    {
      if (JustReleased)
        _wasPressed = false;
      JustReleased = false;
      JustPressed = false;
      _status = 0;

      if (isInsideArea)
      {
        if (isPressed && !_wasPresseOutside)
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
          if (!isPressed)
            _wasPresseOutside = false;
        }
      }
      else
      {
        _wasPresseOutside = !_wasPressedInside && isPressed;
        if (!isPressed)
        {
          _wasPressedInside = false;
          _wasPressed = false;
        }
      }
    }
  }
}