using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancySwitch : FancyButtonBase
  {
    private MySprite bgSprite;
    private MySprite handlerSprite;
    private MySprite selectedSprite;
    private MySprite[] textSprites;

    public int Index;
    public readonly string[] TabNames;
    public Action<int> OnChange;

    public FancySwitch(string[] tabNames, int index = 0, Action<int> onChange = null)
    {
      TabNames = tabNames;
      Index = index;

      textSprites = new MySprite[TabNames.Length];

      Scale = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);
    }

    public override void Update()
    {
      handler.HitArea = new Vector4(Position.X, Position.Y, Position.X + Size.X, Position.Y + Size.Y);

      var width = Size.X / textSprites.Length;
      var halfWidth = width / 2f;

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
        Color = App.Theme.Main_20
      };

      selectedSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_60
      };

      for (int i = 0; i < textSprites.Length; i++)
      {
        textSprites[i] = new MySprite()
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

      bgSprite.Position = Position + new Vector2(0, Size.Y / 2);
      bgSprite.Size = Size;

      sprites.Add(bgSprite);

      if (handler.IsMousePressed || handler.IsMouseOver)
      {
        var mouseX = App.Cursor.Position.X - handler.HitArea.X;
        var p = (int)Math.Floor(mouseX / width);

        if (p != Index)
        {
          handlerSprite.Position = Position + new Vector2(width * p, Size.Y / 2);
          handlerSprite.Size = new Vector2(width, Size.Y);

          sprites.Add(handlerSprite);
        }
      }

      selectedSprite.Position = Position + new Vector2(width * Index, Size.Y / 2);
      selectedSprite.Size = new Vector2(width, Size.Y);

      sprites.Add(selectedSprite);

      for (int j = 0; j < textSprites.Length; j++)
      {
        textSprites[j].Position = Position + new Vector2(j * width + halfWidth, Size.Y * 0.5f - (textSprites[j].RotationOrScale * 16.6f));
        textSprites[j].Color = j == Index ? App.Theme.White : App.Theme.Main_40;
        sprites.Add(textSprites[j]);
      }
    }
  }
}