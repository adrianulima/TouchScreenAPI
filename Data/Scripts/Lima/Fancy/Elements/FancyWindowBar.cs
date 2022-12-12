using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyWindowBar : FancyElementBase
  {
    private MySprite _bgSprite;
    private MySprite _textSprite;

    public string Text;

    public FancyWindowBar(string text)
    {
      Text = text;

      Scale = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);
    }

    public override void Update()
    {
      base.Update();

      _bgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_10
      };

      _textSprite = new MySprite()
      {
        Type = SpriteType.TEXT,
        Data = Text,
        RotationOrScale = 0.5f * App.Theme.Scale,
        Color = App.Theme.White,//Theme.Main,
        Alignment = TextAlignment.LEFT,
        FontId = App.Theme.Font
      };

      Sprites.Clear();

      var size = GetSize();

      _bgSprite.Position = Position + new Vector2(0, size.Y / 2);
      _bgSprite.Size = size;

      _textSprite.Position = Position + new Vector2(10, size.Y * 0.5f - size.Y / 3);

      Sprites.Add(_bgSprite);
      Sprites.Add(_textSprite);
    }

  }
}