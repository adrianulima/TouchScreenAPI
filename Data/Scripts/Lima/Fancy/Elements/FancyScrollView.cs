using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyScrollView : FancyView
  {
    public bool ScrollAlwaysVisible = false;

    public FancyBarContainer ScrollBar;

    private float _scroll = 0;
    public float Scroll
    {
      get { return _scroll; }
      set { _scroll = MathHelper.Clamp(value, 0, 1); }
    }

    private ClickHandler _handler = new ClickHandler();
    private Vector2 _flexSize;

    public FancyScrollView(ViewDirection direction = ViewDirection.Column, Color? bgColor = null) : base(direction, bgColor)
    {
      ScrollBar = new FancyBarContainer(true);
      ScrollBar.Pixels = new Vector2(12, 0);
      ScrollBar.Scale = Vector2.Zero;
      ScrollBar.Absolute = true;
      AddChild(ScrollBar);
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
      Position.Y += Scroll * _flexSize.Y;
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

      _handler.UpdateStatus(App.Screen);

      var size = GetSize();

      ScrollBar.Enabled = true;
      ScrollBar.Position = Position + new Vector2(size.X + (Border.X + Padding.X + Padding.Z - ScrollBar.Pixels.X) * ThemeScale, Border.Y * ThemeScale);
      ScrollBar.Pixels.Y = (size.Y / ThemeScale) + Padding.Y + Padding.W;
      ScrollBar.Scale = Vector2.Zero;

      if (needsScroll)
      {
        var bgPos = ScrollBar.Position;
        _handler.HitArea = new Vector4(bgPos.X, bgPos.Y, bgPos.X + ScrollBar.Pixels.X * ThemeScale, bgPos.Y + ScrollBar.Pixels.Y * ThemeScale);

        var barSize = 1 - (-_flexSize.Y / (ScrollBar.Pixels.Y * ThemeScale));
        ScrollBar.Ratio = MathHelper.Clamp(barSize, 0.1f, 0.9f);

        if (_handler.IsMouseOver)
        {
          ScrollBar.Bar.BgColor = App.Theme.MainColor_5;
          if (_handler.IsMousePressed)
          {
            ScrollBar.Bar.BgColor = App.Theme.MainColor_6;
            var cursorRatio = (App.Cursor.Position.Y - _handler.HitArea.Y) / (_handler.HitArea.W - _handler.HitArea.Y);
            Scroll = cursorRatio * (1 + 2 * ScrollBar.Ratio) - (ScrollBar.Ratio * 0.5f);
          }
        }
        else
          ScrollBar.Bar.BgColor = App.Theme.MainColor_4;
      }
      else
        ScrollBar.Ratio = 0;

      ScrollBar.Offset = Scroll;

      // Adds an extra podding to give inside space for the scrollbar
      var prevPad = Padding;
      Padding.Z += ScrollBar.Pixels.X;
      base.Update();
      Padding = prevPad;
    }
  }
}