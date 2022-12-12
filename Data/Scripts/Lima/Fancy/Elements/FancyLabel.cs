using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyLabel : FancyElementBase
  {
    protected MySprite TextSprite;
    public string Text;
    public float FontSize;
    public TextAlignment Alignment = TextAlignment.CENTER;

    public FancyLabel(string text, float fontSize = 0.5f)
    {
      Text = text;
      FontSize = fontSize;

      Scale = new Vector2(1, 0);
      Margin = new Vector4(8, 8, 8, 0);
      Pixels = new Vector2(0, 24);
    }

    public override void Update()
    {
      base.Update();

      TextSprite = new MySprite()
      {
        Type = SpriteType.TEXT,
        Data = Text,
        RotationOrScale = FontSize * App.Theme.Scale,
        Color = App.Theme.White,//Theme.Main,
        Alignment = Alignment,
        FontId = App.Theme.Font
      };

      if (Alignment == TextAlignment.LEFT)
        TextSprite.Position = Position;
      else if (Alignment == TextAlignment.RIGHT)
        TextSprite.Position = Position + new Vector2(Size.X, 0);
      else
        TextSprite.Position = Position + new Vector2(Size.X / 2, 0);

      Sprites.Clear();

      Sprites.Add(TextSprite);
    }

  }
}