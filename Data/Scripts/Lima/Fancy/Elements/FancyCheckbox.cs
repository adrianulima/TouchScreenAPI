using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyCheckbox : FancyElementBase
  {
    public ClickHandler Handler = new ClickHandler();

    public bool Value;
    public Action<bool> OnChange;

    public FancyCheckbox(Action<bool> onChange, bool value = false)
    {
      OnChange = onChange;
      Value = value;

      Scale = new Vector2(0, 0);
      Pixels = new Vector2(24, 24);
    }

    public override void Update()
    {
      var size = GetSize();

      Handler.HitArea = new Vector4(Position.X, Position.Y, Position.X + size.X, Position.Y + size.Y);
      Handler.UpdateStatus(App.Screen);

      base.Update();

      var bgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.MainColor_4
      };

      var bgHandler = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.MainColor_2
      };

      if (Handler.IsMousePressed)
      {
        bgSprite.Color = App.Theme.MainColor_8;
        bgHandler.Color = App.Theme.MainColor_6;
      }
      else if (Handler.IsMouseOver)
      {
        bgSprite.Color = App.Theme.MainColor_5;
        bgHandler.Color = App.Theme.MainColor_3;
      }
      else
      {
        bgSprite.Color = App.Theme.MainColor_4;
        bgHandler.Color = App.Theme.MainColor_2;
      }

      if (Handler.JustReleased)
      {
        Value = !Value;
        OnChange(Value);
      }

      Sprites.Clear();

      var gap = 2 * ThemeScale;

      bgSprite.Position = Position + new Vector2(0, size.Y / 2);
      bgSprite.Size = size;

      bgHandler.Position = Position + new Vector2(gap, size.Y / 2);
      bgHandler.Size = bgSprite.Size - Vector2.One * gap * 2;

      Sprites.Add(bgSprite);
      Sprites.Add(bgHandler);

      if (Value)
      {
        var check1 = new MySprite()
        {
          Type = SpriteType.TEXTURE,
          Data = "Triangle",
          RotationOrScale = -MathHelper.PiOver4,
          Color = App.Theme.WhiteColor
        };
        check1.Position = Position + new Vector2(size.X / 2 - size.Y / 4, size.Y / 2 + size.Y / 12 - gap);
        check1.Size = new Vector2(gap * 1.5f, size.Y / 2f);

        var check2 = check1;
        check2.RotationOrScale = MathHelper.PiOver4;
        check2.Position = Position + new Vector2(size.X / 2 + size.X / 5, size.Y / 2 + size.X / 14 - gap);
        check2.Size = new Vector2(gap * 1.8f, size.Y / 1.25f);

        Sprites.Add(check1);
        Sprites.Add(check2);
      }
    }
  }
}