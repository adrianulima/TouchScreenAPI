using System;
using Sandbox.ModAPI;
using VRageMath;

namespace Lima.Touch.UiKit.Elements
{
  public class TouchScrollView : TouchView
  {
    public ClickHandler Handler = new ClickHandler();
    public ClickHandler HandlerContent = new ClickHandler();

    public bool ScrollAlwaysVisible = false;
    public bool ScrollWheelEnabled = true;
    public float ScrollWheelStep = 12;

    public TouchBarContainer ScrollBar;

    private float _scroll = 0;
    public float Scroll
    {
      get { return _scroll; }
      set { _scroll = MathHelper.Clamp(value, 0, 1); }
    }
    private Vector2 _flexSize;

    bool _mouseOver = false;
    int _delta = 0;

    public TouchScrollView(ViewDirection direction = ViewDirection.Column, Color? bgColor = null) : base(direction, bgColor)
    {
      Overflow = false;

      ScrollBar = new TouchBarContainer(true);
      ScrollBar.Pixels = new Vector2(12, 0);
      ScrollBar.Scale = Vector2.Zero;
      ScrollBar.Absolute = true;
      ScrollBar.UseThemeColors = false;
      AddChild(ScrollBar);
    }

    public override void OnAddedToApp()
    {
      base.OnAddedToApp();
      App.UpdateAtSimulationEvent -= UpdateAtSimulation;
      App.UpdateAtSimulationEvent += UpdateAtSimulation;
    }

    public override void Dispose()
    {
      base.Dispose();
      App.UpdateAtSimulationEvent -= UpdateAtSimulation;
    }

    public void UpdateAtSimulation()
    {
      if (_mouseOver)
        _delta += Math.Sign(MyAPIGateway.Input.DeltaMouseScrollWheelValue());
      else
        _delta = 0;
    }

    protected override void UpdateChildrenPositions()
    {
      var needsScroll = _flexSize.Y < 0;
      if (Direction != ViewDirection.Column || !needsScroll)
      {
        base.UpdateChildrenPositions();
        return;
      }

      var prevPos = Position;
      Position += Vector2.UnitY * Scroll * _flexSize.Y;
      base.UpdateChildrenPositions();
      Position = prevPos;
    }

    public override void Update()
    {
      _flexSize = GetFlexSize();
      var needsScroll = _flexSize.Y < 0;
      if (Direction != ViewDirection.Column || (!ScrollAlwaysVisible && !needsScroll))
      {
        base.Update();
        ScrollBar.Enabled = false;
        return;
      }

      var size = GetSize();

      ScrollBar.Enabled = true;
      ScrollBar.Position = Position + new Vector2(size.X + (Border.X + Padding.X + Padding.Z - ScrollBar.Pixels.X) * ThemeScale, Border.Y * ThemeScale);
      ScrollBar.Pixels.Y = (size.Y / ThemeScale) + Padding.Y + Padding.W;
      ScrollBar.Scale = Vector2.Zero;

      if (needsScroll)
      {
        var bgPos = ScrollBar.Position;
        Handler.HitArea = new Vector4(bgPos.X, bgPos.Y, bgPos.X + ScrollBar.Pixels.X * ThemeScale, bgPos.Y + ScrollBar.Pixels.Y * ThemeScale);
        Handler.UpdateStatus(App.Screen);

        var barSize = 1 - (-_flexSize.Y / (ScrollBar.Pixels.Y * ThemeScale));
        ScrollBar.Ratio = MathHelper.Clamp(barSize, 0.1f, 0.9f);

        if (Handler.IsMousePressed)
        {
          var cursorRatio = (App.Cursor.Position.Y - Handler.HitArea.Y) / (Handler.HitArea.W - Handler.HitArea.Y);
          Scroll = cursorRatio * (1 + ScrollBar.Ratio) - (ScrollBar.Ratio * 0.5f);
        }
        else
        {
          HandlerContent.HitArea = new Vector4(Position.X, Position.Y, bgPos.X, bgPos.Y + ScrollBar.Pixels.Y * ThemeScale);
          HandlerContent.UpdateStatus(App.Screen);

          _mouseOver = HandlerContent.IsMouseOver;
          if (_mouseOver && _delta != 0)
          {
            var wheel = (ScrollWheelStep / -_flexSize.Y) * ((float)_delta);
            Scroll -= wheel;
            _delta = 0;
          }
        }

      }
      else
        ScrollBar.Ratio = 0;

      ScrollBar.Offset = Scroll;

      if (UseThemeColors)
        ApplyThemeStyle();

      // Adds an extra podding to give inside space for the scrollbar
      var prevPad = Padding;
      Padding.Z += ScrollBar.Pixels.X;
      base.Update();
      Padding = prevPad;
    }

    private void ApplyThemeStyle()
    {
      ScrollBar.BgColor = App.Theme.MainColor_2;
      if (Handler.IsMousePressed)
        ScrollBar.Bar.BgColor = App.Theme.MainColor_5;
      else if (Handler.IsMouseOver)
        ScrollBar.Bar.BgColor = App.Theme.MainColor_5;
      else
        ScrollBar.Bar.BgColor = App.Theme.MainColor_4;
    }
  }
}