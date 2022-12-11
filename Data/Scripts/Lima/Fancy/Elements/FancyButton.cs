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
      Margin = new Vector4(8, 8, 8, 0);
      Pixels = new Vector2(0, 24);
    }

    public override void Update()
    {
      handler.HitArea = new Vector4(Position.X, Position.Y, Position.X + Size.X, Position.Y + Size.Y);

      base.Update();

      _bgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_30
      };

      _textSprite = new MySprite()
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
        _textSprite.Color = App.Theme.Main_30;
        _bgSprite.Color = App.Theme.Main_70;
      }
      else if (handler.IsMouseOver)
      {
        _textSprite.Color = App.Theme.White;
        _bgSprite.Color = App.Theme.Main_40;
      }
      else
      {
        _textSprite.Color = App.Theme.White;
        _bgSprite.Color = App.Theme.Main_30;
      }

      if (handler.JustReleased)
      {
        OnChange();
        // MyAPIGateway.Utilities.ShowNotification($"[ Button callback ]", 2000, MyFontEnum.Red);
        Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessage(App.Theme.Scale.ToString(), "SampleApp");
      }

      sprites.Clear();

      _bgSprite.Position = Position + new Vector2(0, Size.Y / 2);
      _bgSprite.Size = Size;

      if (Alignment == TextAlignment.LEFT)
        _textSprite.Position = Position + new Vector2(0, Size.Y * 0.5f - (_textSprite.RotationOrScale * 16.6f));
      else if (Alignment == TextAlignment.RIGHT)
        _textSprite.Position = Position + new Vector2(Size.X, Size.Y * 0.5f - (_textSprite.RotationOrScale * 16.6f));
      else
        _textSprite.Position = Position + new Vector2(Size.X / 2, Size.Y * 0.5f - (_textSprite.RotationOrScale * 16.6f));

      sprites.Add(_bgSprite);
      sprites.Add(_textSprite);
    }

  }
}