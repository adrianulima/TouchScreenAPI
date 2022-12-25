using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyScrollView : FancyView
  {
    public int BarWidth = 8;
    public bool ScrollAlwaysVisible = false;

    private float _scroll = 0;
    public float Scroll
    {
      get { return _scroll; }
      set { _scroll = MathHelper.Clamp(value, 0, 1); }
    }

    private ClickHandler _handler = new ClickHandler();

    private MySprite _bgSprite;
    private MySprite _bar;

    private Vector2 _flexSize;

    public FancyScrollView(ViewDirection direction = ViewDirection.Column, Color? bgColor = null) : base(direction, bgColor)
    {
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
      var needsScroll = _flexSize.Y < 0;
      _flexSize = GetFlexSize();
      if (Direction != ViewDirection.Column || (!ScrollAlwaysVisible && !needsScroll))
      {
        base.Update();
        return;
      }

      _handler.UpdateStatus(App.Screen);

      var size = GetSize();

      var width = BarWidth * ThemeScale;
      // Pixels = new Vector2(width, 0);

      // Adds an extra podding to give inside space for the scrollbar
      var prevPad = Padding;
      Padding.Z += width;
      base.Update();
      Padding = prevPad;

      _bgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.MainColor_2
      };

      _bgSprite.Position = Position + new Vector2(size.X + Border.X + Padding.X + Padding.Z - width, Border.Y + Padding.Y + size.Y / 2);
      _bgSprite.Size = new Vector2(width, size.Y + Padding.Y + Padding.W);

      Sprites.Add(_bgSprite);

      if (needsScroll)
      {
        _bar = new MySprite()
        {
          Type = SpriteType.TEXTURE,
          Data = "SquareSimple",
          RotationOrScale = 0,
          Color = App.Theme.MainColor_4
        };

        var bgPos = _bgSprite.Position.GetValueOrDefault();
        var bgSize = _bgSprite.Size.GetValueOrDefault();
        _handler.HitArea = new Vector4(bgPos.X, bgPos.Y - bgSize.Y / 2, bgPos.X + bgSize.X, bgPos.Y + bgSize.Y / 2);

        var barSize = new Vector2(bgSize.X, MathHelper.Clamp(bgSize.Y + _flexSize.Y * ThemeScale, bgSize.Y * 0.5f, bgSize.Y * 0.9f));
        _bar.Size = barSize;

        if (_handler.IsMouseOver)
        {
          _bar.Color = App.Theme.MainColor_5;
          var mouseY = App.Cursor.Position.Y - _handler.HitArea.Y;
          if (_handler.IsMousePressed)
          {
            _bar.Color = App.Theme.MainColor_6;
            var adjust = 0.2f;
            Scroll = -adjust + (1 + 2 * adjust) * (App.Cursor.Position.Y - _handler.HitArea.Y) / ((_handler.HitArea.W * (1 - adjust)) - _handler.HitArea.Y);
          }
        }
        else
        {
          _bar.Color = App.Theme.MainColor_4;
        }

        var diffSize = bgSize.Y - barSize.Y;
        _bar.Position = bgPos + Vector2.UnitY * ((Scroll * diffSize) - (diffSize / 2));

        Sprites.Add(_bar);
      }
    }
  }
}