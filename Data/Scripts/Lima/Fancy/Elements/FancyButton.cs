using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyButton : FancyButtonBase
  {
    private MySprite bgSprite;
    private MySprite textSprite;

    public string Text;
    public Action _action;
    public TextAlignment Alignment = TextAlignment.CENTER;

    public FancyButton(string text, Action action)
    {
      Text = text;
      _action = action;

      Scale = new Vector2(1, 0);
      Margin = new Vector4(8, 8, 8, 0);
      Pixels = new Vector2(0, 24);
    }

    public override void Update()
    {
      handler.hitArea = new Vector4(Position.X, Position.Y, Position.X + Size.X, Position.Y + Size.Y);

      base.Update();

      bgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_30
      };

      textSprite = new MySprite()
      {
        Type = SpriteType.TEXT,
        Data = Text,
        RotationOrScale = 0.6f * App.Theme.Scale,
        Color = App.Theme.White,
        Alignment = Alignment,
        FontId = App.Theme.Font
      };

      if (handler.IsMousePressed)
      {
        textSprite.Color = App.Theme.Main_30;
        bgSprite.Color = App.Theme.Main_70;
      }
      else if (handler.IsMouseOver)
      {
        textSprite.Color = App.Theme.White;
        bgSprite.Color = App.Theme.Main_40;
      }
      else
      {
        textSprite.Color = App.Theme.White;
        bgSprite.Color = App.Theme.Main_30;
      }

      if (handler.JustReleased)
      {
        _action();
        // MyAPIGateway.Utilities.ShowNotification($"[ Button callback ]", 2000, MyFontEnum.Red);
      }

      sprites.Clear();

      bgSprite.Position = Position + new Vector2(0, Size.Y / 2);
      bgSprite.Size = Size;

      if (Alignment == TextAlignment.LEFT)
        textSprite.Position = Position + new Vector2(0, Size.Y * 0.5f - (Size.Y / 2.4f));
      else if (Alignment == TextAlignment.RIGHT)
        textSprite.Position = Position + new Vector2(Size.X, Size.Y * 0.5f - (Size.Y / 2.4f));
      else
        textSprite.Position = Position + new Vector2(Size.X / 2, Size.Y * 0.5f - (Size.Y / 2.4f));

      sprites.Add(bgSprite);
      sprites.Add(textSprite);
    }

  }
}