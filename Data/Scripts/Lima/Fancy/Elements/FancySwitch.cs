using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancySwitch : FancyButtonBase
  {
    private MySprite bgSprite;
    private MySprite handlerSprite;
    private MySprite textOnSprite;
    private MySprite textOffSprite;

    public string TextOn;
    public string TextOff;
    public bool Value = false;
    public Action<bool> _action;

    public FancySwitch(Action<bool> action, string textOn = "On", string textOff = "Off")
    {
      _action = action;
      TextOn = textOn;
      TextOff = textOff;

      Scale = new Vector2(1, 0);
      Margin = new Vector4(8, 0, 8, 0);
      Pixels = new Vector2(0, 24);
    }

    public override void Update()
    {
      handler.HitArea = new Vector4(Position.X, Position.Y, Position.X + Size.X, Position.Y + Size.Y);

      base.Update();

      bgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_10
      };

      handlerSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main
      };

      textOnSprite = new MySprite()
      {
        Type = SpriteType.TEXT,
        Data = TextOn,
        RotationOrScale = 0.6f * App.Theme.Scale,
        Color = App.Theme.White,//Theme.Main,
        Alignment = TextAlignment.CENTER,
        FontId = App.Theme.Font
      };

      textOffSprite = new MySprite()
      {
        Type = SpriteType.TEXT,
        Data = TextOff,
        RotationOrScale = 0.6f * App.Theme.Scale,
        Color = App.Theme.White,//Theme.Main,
        Alignment = TextAlignment.CENTER,
        FontId = App.Theme.Font
      };

      if (handler.IsMousePressed)
      {
        bgSprite.Color = App.Theme.Main_20;
      }
      else if (handler.IsMouseOver)
      {
        bgSprite.Color = App.Theme.Main_20;
      }
      else
      {
        bgSprite.Color = App.Theme.Main_10;
      }

      if (handler.JustReleased)
      {
        Value = !Value;
        _action(Value);
      }

      sprites.Clear();

      bgSprite.Position = Position + new Vector2(0, Size.Y / 2);
      bgSprite.Size = Size;

      var gap = 2;
      var dir = (Value ? 1 : 0);

      handlerSprite.Position = Position + new Vector2(gap * (1 - dir) + (Size.X / 2) * dir, Size.Y / 2);
      handlerSprite.Size = new Vector2(Size.X / 2 - gap, Size.Y - gap * 2);

      textOnSprite.Position = Position + new Vector2(Size.X / 2f + Size.X / 4f, Size.Y * 0.5f - Size.Y / 2.4f);
      textOffSprite.Position = Position + new Vector2(Size.X / 4f, Size.Y * 0.5f - Size.Y / 2.4f);

      textOnSprite.Color = Value ? App.Theme.White : App.Theme.Main_40;
      textOffSprite.Color = Value ? App.Theme.Main_40 : App.Theme.White;

      sprites.Add(bgSprite);
      sprites.Add(handlerSprite);
      sprites.Add(textOnSprite);
      sprites.Add(textOffSprite);
    }

  }
}