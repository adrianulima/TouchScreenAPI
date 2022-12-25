using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyCheckbox : FancyButtonBase
  {
    private MySprite _bgSprite;
    private MySprite _bgHandler;

    private MySprite _check1;
    private MySprite _check2;

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

      base.Update();

      _bgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_40
      };

      _bgHandler = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_20
      };

      if (Handler.IsMousePressed)
      {
        _bgSprite.Color = App.Theme.Main_80;
        _bgHandler.Color = App.Theme.Main_60;
      }
      else if (Handler.IsMouseOver)
      {
        _bgSprite.Color = App.Theme.Main_50;
        _bgHandler.Color = App.Theme.Main_30;
      }
      else
      {
        _bgSprite.Color = App.Theme.Main_40;
        _bgHandler.Color = App.Theme.Main_20;
      }

      if (Handler.JustReleased)
      {
        Value = !Value;
        OnChange(Value);
      }

      Sprites.Clear();

      var gap = 2 * ThemeScale;

      _bgSprite.Position = Position + new Vector2(0, size.Y / 2);
      _bgSprite.Size = size;

      _bgHandler.Position = Position + new Vector2(gap, size.Y / 2);
      _bgHandler.Size = _bgSprite.Size - Vector2.One * gap * 2;

      Sprites.Add(_bgSprite);
      Sprites.Add(_bgHandler);

      if (Value)
      {
        _check1 = new MySprite()
        {
          Type = SpriteType.TEXTURE,
          Data = "Triangle",//"SquareSimple",
          RotationOrScale = -MathHelper.PiOver4,
          Color = App.Theme.White
        };
        _check1.Position = Position + new Vector2(size.X / 2 - size.Y / 4, size.Y / 2 + size.Y / 12 - gap);
        _check1.Size = new Vector2(gap * 1.5f, size.Y / 2f);

        _check2 = _check1;
        _check2.RotationOrScale = MathHelper.PiOver4;
        _check2.Position = Position + new Vector2(size.X / 2 + size.X / 5, size.Y / 2 + size.X / 14 - gap);
        _check2.Size = new Vector2(gap * 1.8f, size.Y / 1.25f);

        Sprites.Add(_check1);
        Sprites.Add(_check2);
      }
    }
  }
}