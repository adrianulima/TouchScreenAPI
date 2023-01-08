using System;
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

    public bool AutoBreakLine = false;
    public bool Overflow = false;
    public bool IsShortened { get; private set; } = false;

    public float FontSize
    {
      get { return _fontSize; }
      set
      {
        if (_fontSize != value)
        {
          _fontSize = value;
          UpdateHeight(Text);
        }
      }
    }

    public FancyLabel(string text, float fontSize = 0.5f, TextAlignment alignment = TextAlignment.CENTER)
    {
      Text = text;
      FontSize = fontSize;
      Alignment = alignment;

      Scale = new Vector2(1, 0);
      UpdateHeight(Text);
    }

    private void UpdateHeight(string text)
    {
      var lines = 1;
      for (int i = 0; i < text.Length; i++)
      {
        if (text[i] == '\n')
          lines++;
      }
      Pixels.Y = 32 * FontSize * lines;
    }

    private string BreakLine(string text, float width, MySprite textSprite)
    {
      var saveText = text;
      var lastSpaceIndex = text.LastIndexOf(" ");
      while (lastSpaceIndex > 0 && text.Length > 0 && width < App.Theme.MeasureStringInPixels(text, textSprite.FontId, textSprite.RotationOrScale).X)
      {
        text = text.Substring(0, lastSpaceIndex - 1);
        lastSpaceIndex = text.LastIndexOf(" ");
      }

      if (saveText == text)
        return text;

      saveText = saveText.Remove(text.Length + 1, 1).Insert(text.Length + 1, "\n");
      return BreakLine(saveText, width, textSprite);
    }

    public override void Update()
    {
      var textSprite = new MySprite()
      {
        Type = SpriteType.TEXT,
        Data = Text,
        RotationOrScale = FontSize * ThemeScale,
        Color = TextColor ?? App.Theme.WhiteColor,
        Alignment = Alignment,
        FontId = App.Theme.Font
      };

      var size = GetSize();

      if (AutoBreakLine && Text.Length > 0)
      {
        textSprite.Data = BreakLine(Text, size.X, textSprite);
      }
      else if (!Overflow && Text.Length > 0)
      {
        var text = Text;
        while (text.Length > 0 && size.X < App.Theme.MeasureStringInPixels(text, textSprite.FontId, textSprite.RotationOrScale).X)
          text = text.Substring(0, text.Length - 1).TrimEnd();

        IsShortened = text != Text;
        if (IsShortened)
          textSprite.Data = $"{text.Substring(0, Math.Max(0, text.Length - 2)).TrimEnd()}...";
      }

      UpdateHeight(textSprite.Data);

      if (Alignment == TextAlignment.LEFT)
        textSprite.Position = Position;
      else if (Alignment == TextAlignment.RIGHT)
        textSprite.Position = Position + new Vector2(size.X, 0);
      else
        textSprite.Position = Position + new Vector2(size.X / 2, 0);

      Sprites.Clear();

      base.Update();

      Sprites.Add(textSprite);
    }

  }
}