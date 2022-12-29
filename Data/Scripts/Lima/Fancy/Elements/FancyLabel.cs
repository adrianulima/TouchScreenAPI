using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyLabel : FancyElementBase
  {
    public string Text;
    private float _fontSize;
    public TextAlignment Alignment;
    public Color? TextColor;

    public float FontSize
    {
      get { return _fontSize; }
      set
      {
        if (_fontSize != value)
        {
          _fontSize = value;
          UpdateHeight();
        }
      }
    }

    public FancyLabel(string text, float fontSize = 0.5f, TextAlignment alignment = TextAlignment.CENTER)
    {
      Text = text;
      FontSize = fontSize;
      Alignment = alignment;

      Scale = new Vector2(1, 0);
      UpdateHeight();
    }

    private void UpdateHeight()
    {
      var lines = 1;
      for (int i = 0; i < Text.Length; i++)
      {
        if (Text[i] == '\n')
          lines++;
      }
      Pixels.Y = 32 * FontSize * lines;
    }

    public override void Update()
    {
      UpdateHeight();

      var textSprite = new MySprite()
      {
        Type = SpriteType.TEXT,
        Data = Text,
        RotationOrScale = FontSize * ThemeScale,
        Color = TextColor ?? App.Theme.WhiteColor,
        Alignment = Alignment,
        FontId = App.Theme.Font
      };

      if (Alignment == TextAlignment.LEFT)
        textSprite.Position = Position;
      else if (Alignment == TextAlignment.RIGHT)
        textSprite.Position = Position + new Vector2(GetSize().X, 0);
      else
        textSprite.Position = Position + new Vector2(GetSize().X / 2, 0);

      Sprites.Clear();

      base.Update();

      Sprites.Add(textSprite);
    }

  }
}