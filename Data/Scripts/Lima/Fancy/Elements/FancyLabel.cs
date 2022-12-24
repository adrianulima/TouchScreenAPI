using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyLabel : FancyElementBase
  {
    protected MySprite TextSprite;
    public string Text;
    public float FontSize;
    public TextAlignment Alignment;
    public Color? TextColor;

    public FancyLabel(string text, float fontSize = 0.5f, TextAlignment alignment = TextAlignment.CENTER)
    {
      Text = text;
      FontSize = fontSize;
      Alignment = alignment;

      Scale = new Vector2(1, 0);
    }

    public override void Update()
    {
      base.Update();

      var lines = 1;
      for (int i = 0; i < Text.Length; i++)
      {
        if (Text[i] == '\n')
          lines++;
      }
      Pixels.Y = 32 * FontSize * lines;

      TextSprite = new MySprite()
      {
        Type = SpriteType.TEXT,
        Data = Text,
        RotationOrScale = FontSize * ThemeScale,
        Color = TextColor ?? App.Theme.White,//Theme.Main,
        Alignment = Alignment,
        FontId = App.Theme.Font
      };

      if (Alignment == TextAlignment.LEFT)
        TextSprite.Position = Position;
      else if (Alignment == TextAlignment.RIGHT)
        TextSprite.Position = Position + new Vector2(GetSize().X, 0);
      else
        TextSprite.Position = Position + new Vector2(GetSize().X / 2, 0);

      Sprites.Clear();

      Sprites.Add(TextSprite);
    }

  }
}