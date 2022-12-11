using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancySwitch : FancyButtonBase
  {
    private MySprite _bgSprite;
    private MySprite _handlerSprite;
    private MySprite _selectedSprite;
    private MySprite[] _textSprites;

    public int Index;
    public readonly string[] TabNames;
    public Action<int> OnChange;

    public FancySwitch(string[] tabNames, int index = 0, Action<int> onChange = null)
    {
      TabNames = tabNames;
      Index = index;

      _textSprites = new MySprite[TabNames.Length];

      Scale = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);
    }

    public override void Update()
    {
      handler.HitArea = new Vector4(Position.X, Position.Y, Position.X + Size.X, Position.Y + Size.Y);

      var width = Size.X / _textSprites.Length;
      var halfWidth = width / 2f;

      base.Update();

      _bgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_10
      };

      _handlerSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_20
      };

      _selectedSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_60
      };

      for (int i = 0; i < _textSprites.Length; i++)
      {
        _textSprites[i] = new MySprite()
        {
          Type = SpriteType.TEXT,
          Data = TabNames[i],
          RotationOrScale = 0.5f * App.Theme.Scale,
          Color = App.Theme.White,//Theme.Main,
          Alignment = TextAlignment.CENTER,
          FontId = App.Theme.Font
        };
      }

      if (handler.JustReleased)
      {
        var mouseX = App.Cursor.Position.X - handler.HitArea.X;
        var prev = Index;
        Index = (int)Math.Floor(mouseX / width);

        if (OnChange != null && prev != Index)
          OnChange(Index);
      }

      sprites.Clear();

      _bgSprite.Position = Position + new Vector2(0, Size.Y / 2);
      _bgSprite.Size = Size;

      sprites.Add(_bgSprite);

      if (handler.IsMousePressed || handler.IsMouseOver)
      {
        var mouseX = App.Cursor.Position.X - handler.HitArea.X;
        var p = (int)Math.Floor(mouseX / width);

        if (p != Index)
        {
          _handlerSprite.Position = Position + new Vector2(width * p, Size.Y / 2);
          _handlerSprite.Size = new Vector2(width, Size.Y);

          sprites.Add(_handlerSprite);
        }
      }

      _selectedSprite.Position = Position + new Vector2(width * Index, Size.Y / 2);
      _selectedSprite.Size = new Vector2(width, Size.Y);

      sprites.Add(_selectedSprite);

      for (int j = 0; j < _textSprites.Length; j++)
      {
        _textSprites[j].Position = Position + new Vector2(j * width + halfWidth, Size.Y * 0.5f - (_textSprites[j].RotationOrScale * 16.6f));
        _textSprites[j].Color = j == Index ? App.Theme.White : App.Theme.Main_40;
        sprites.Add(_textSprites[j]);
      }
    }
  }
}