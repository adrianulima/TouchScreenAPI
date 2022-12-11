using System;
using System.Collections.Generic;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancySelector : FancyButtonBase
  {
    private MySprite _bgSprite;
    private MySprite _arrowSprite;
    private MySprite _arrowBgSprite;
    private MySprite _arrow2Sprite;
    private MySprite _arrow2BgSprite;
    private MySprite _textSprite;

    public int Selected = 0;
    public List<string> Labels;
    public Action<int, string> OnChange;
    protected bool _loop;

    public FancySelector(List<string> labels, Action<int, string> onChange, bool loop = true)
    {
      Labels = labels;
      OnChange = onChange;
      _loop = loop;

      Scale = new Vector2(1, 0);
      Margin = new Vector4(8, 0, 8, 0);
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
        Color = App.Theme.Main_10
      };

      _arrowBgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_30
      };

      _arrow2BgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_30
      };

      _arrowSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "AH_BoreSight",
        RotationOrScale = MathHelper.Pi,
        Color = App.Theme.White
      };

      _arrow2Sprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "AH_BoreSight",
        RotationOrScale = 0,
        Color = App.Theme.White
      };

      _textSprite = new MySprite()
      {
        Type = SpriteType.TEXT,
        Data = Labels[Selected],
        RotationOrScale = 0.6f * App.Theme.Scale,
        Color = App.Theme.White,//Theme.Main,
        Alignment = TextAlignment.CENTER,
        FontId = App.Theme.Font
      };

      if (handler.IsMouseOver)
      {
        var mouseX = App.Cursor.Position.X - handler.HitArea.X;
        if (handler.IsMousePressed)
        {
          if (mouseX < Size.Y)
            _arrowBgSprite.Color = App.Theme.Main_70;
          else if (mouseX > Size.X - Size.Y)
            _arrow2BgSprite.Color = App.Theme.Main_70;
        }
        else
        {
          if (mouseX < Size.Y)
            _arrowBgSprite.Color = App.Theme.Main_40;
          else if (mouseX > Size.X - Size.Y)
            _arrow2BgSprite.Color = App.Theme.Main_40;
        }
      }
      else
      {
        _arrowBgSprite.Color = App.Theme.Main_30;
        _arrow2BgSprite.Color = App.Theme.Main_30;
      }

      if (handler.JustReleased)
      {
        var mouseX = App.Cursor.Position.X - handler.HitArea.X;
        var prev = Selected;
        if (mouseX < Size.Y)
          Selected -= 1;
        else if (mouseX > Size.X - Size.Y)
          Selected += 1;

        var count = Labels.Count;
        if (Selected < 0)
          Selected = _loop ? count - 1 : 0;

        if (Selected >= count)
          Selected = _loop ? 0 : count - 1;

        if (prev != Selected)
          OnChange(Selected, Labels[Selected]);
      }

      sprites.Clear();

      _bgSprite.Position = Position + new Vector2(0, Size.Y / 2);
      _bgSprite.Size = Size;

      _arrowSprite.Position = _arrowBgSprite.Position = Position + new Vector2(0, Size.Y / 2);
      _arrowSprite.Size = _arrowBgSprite.Size = new Vector2(Size.Y, Size.Y);

      _arrow2Sprite.Position = _arrow2BgSprite.Position = Position + new Vector2(Size.X - Size.Y, Size.Y / 2);
      _arrow2Sprite.Size = _arrow2BgSprite.Size = new Vector2(Size.Y, Size.Y);

      _textSprite.Position = Position + new Vector2(Size.X / 2, Size.Y * 0.5f - (_textSprite.RotationOrScale * 16.6f));
      // textSprite.Size = Size;

      sprites.Add(_bgSprite);
      sprites.Add(_arrowBgSprite);
      sprites.Add(_arrow2BgSprite);
      sprites.Add(_arrowSprite);
      sprites.Add(_arrow2Sprite);
      sprites.Add(_textSprite);
    }

  }
}