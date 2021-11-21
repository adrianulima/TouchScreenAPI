using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyWindowBar : FancyElementBase
  {
    private MySprite bgSprite;
    private MySprite textSprite;

    public string Text;

    public FancyWindowBar(string text)
    {
      Text = text;
    }

    public override void Update()
    {
      base.Update();

      bgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_10
      };

      textSprite = new MySprite()
      {
        Type = SpriteType.TEXT,
        Data = Text,
        RotationOrScale = 0.5f * App.Theme.Scale,
        Color = App.Theme.White,//Theme.Main,
        Alignment = TextAlignment.LEFT,
        FontId = App.Theme.Font
      };

      sprites.Clear();

      bgSprite.Position = Position + new Vector2(0, Size.Y / 2);
      bgSprite.Size = Size;

      textSprite.Position = Position + new Vector2(10, Size.Y * 0.5f - Size.Y / 3);

      sprites.Add(bgSprite);
      sprites.Add(textSprite);
    }

  }
}