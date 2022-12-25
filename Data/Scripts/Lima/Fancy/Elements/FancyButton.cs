using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyButton : FancyButtonBase
  {
    private MySprite _bgSprite;
    private MySprite _textSprite;

    public string Text;
    public Action OnChange;
    public TextAlignment Alignment = TextAlignment.CENTER;

    public FancyButton(string text, Action onChange)
    {
      Text = text;
      OnChange = onChange;

      Scale = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);
    }

    public override void Update()
    {
      var size = GetSize();
      Handler.HitArea = new Vector4(Position.X, Position.Y, Position.X + size.X, Position.Y + size.Y);

      base.Update();

      _bgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.MainColor_4
      };

      _textSprite = new MySprite()
      {
        Type = SpriteType.TEXT,
        Data = Text,
        RotationOrScale = 0.6f * ThemeScale,
        Color = App.Theme.WhiteColor,
        Alignment = Alignment,
        FontId = App.Theme.Font
      };

      if (Handler.IsMousePressed)
      {
        _textSprite.Color = App.Theme.MainColor_4;
        _bgSprite.Color = App.Theme.MainColor_8;
      }
      else if (Handler.IsMouseOver)
      {
        _textSprite.Color = App.Theme.WhiteColor;
        _bgSprite.Color = App.Theme.MainColor_5;
      }
      else
      {
        _textSprite.Color = App.Theme.WhiteColor;
        _bgSprite.Color = App.Theme.MainColor_4;
      }

      if (Handler.JustReleased)
      {
        OnChange();
      }

      Sprites.Clear();

      _bgSprite.Position = Position + new Vector2(0, size.Y / 2);
      _bgSprite.Size = size;

      if (Alignment == TextAlignment.LEFT)
        _textSprite.Position = Position + new Vector2(0, size.Y * 0.5f - (_textSprite.RotationOrScale * 16.6f));
      else if (Alignment == TextAlignment.RIGHT)
        _textSprite.Position = Position + new Vector2(size.X, size.Y * 0.5f - (_textSprite.RotationOrScale * 16.6f));
      else
        _textSprite.Position = Position + new Vector2(size.X / 2, size.Y * 0.5f - (_textSprite.RotationOrScale * 16.6f));

      Sprites.Add(_bgSprite);
      Sprites.Add(_textSprite);
    }

  }
}