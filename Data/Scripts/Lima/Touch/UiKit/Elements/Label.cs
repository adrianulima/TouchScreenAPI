using System;
using System.Text;
using Sandbox.ModAPI;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Touch.UiKit.Elements
{
  public enum LabelEllipsis : byte
  {
    None = 0, Left = 1, Right = 2
  }
  public class Label : ElementBase
  {
    public string Text;
    private float _fontSize;
    public TextAlignment Alignment;
    public Color? TextColor;

    public bool AutoBreakLine = false;
    public LabelEllipsis AutoEllipsis = LabelEllipsis.Right;
    public bool HasEllipsis { get; private set; } = false;

    public int Lines { get; private set; } = 0;
    public int MaxLines = 9999;

    private string _data;
    private string _prevText;
    private float _prevWidth;
    private float _prevScale;
    private float _prevfontSize;

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

    public Label(string text, float fontSize = 0.5f, TextAlignment alignment = TextAlignment.CENTER)
    {
      Text = text;
      FontSize = fontSize;
      Alignment = alignment;

      Flex = new Vector2(1, 0);
      UpdateHeight(Text);
    }

    private int UpdateHeight(string text)
    {
      var lines = 1;
      for (int i = 0; i < text.Length; i++)
      {
        if (text[i] == '\n')
          lines++;
      }

      Pixels.Y = MathHelper.CeilToInt(28.8f * FontSize * MathHelper.Min(lines, MaxLines));
      Lines = lines;
      return lines;
    }

    private string BreakLine(string text, float width, MySprite textSprite)
    {
      if (text.Length > 0 && width < App.Theme.MeasureStringInPixels(text, textSprite.FontId, textSprite.RotationOrScale).X)
      {
        float curLineLength = 0;
        var strBuilder = new StringBuilder();
        var words = text.Split(' ');

        var spaceWid = App.Theme.MeasureStringInPixels(" ", textSprite.FontId, textSprite.RotationOrScale).X;

        for (int i = 0; i < words.Length; i += 1)
        {
          var word = words[i];
          var wordWid = App.Theme.MeasureStringInPixels(word, textSprite.FontId, textSprite.RotationOrScale).X;

          if (curLineLength + wordWid > width)
          {
            if (curLineLength > 0)
            {
              strBuilder.Append("\n");
              curLineLength = 0;
            }

            if (wordWid > width)
            {
              while (word.Length > 0 && width < App.Theme.MeasureStringInPixels(word, textSprite.FontId, textSprite.RotationOrScale).X)
                word = word.Substring(Math.Min(0, word.Length), word.Length - 1).TrimEnd();

              word = $"{word.Substring(0, Math.Max(0, word.Length - 3)).TrimEnd()}...";
            }
          }

          strBuilder.Append(word);
          curLineLength += wordWid;
          if (i < words.Length - 1)
          {
            strBuilder.Append(" ");
            curLineLength += spaceWid;
          }

        }
        return strBuilder.ToString();
      }

      return text;
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
        if (_prevText == Text && _prevWidth == size.X && _prevScale == ThemeScale && _prevfontSize == _fontSize)
          textSprite.Data = _data;
        else
          textSprite.Data = BreakLine(Text, size.X, textSprite);
      }
      else if (AutoEllipsis != LabelEllipsis.None && Text.Length > 0)
      {
        var text = Text;
        var start = AutoEllipsis == LabelEllipsis.Left ? 1 : 0;

        if (_prevText == Text && _prevWidth == size.X && _prevScale == ThemeScale && _prevfontSize == _fontSize)
        {
          textSprite.Data = _data;
        }
        else
        {
          while (text.Length > 0 && size.X < App.Theme.MeasureStringInPixels(text, textSprite.FontId, textSprite.RotationOrScale).X)
            text = text.Substring(Math.Min(start, text.Length), text.Length - 1).TrimEnd();

          HasEllipsis = text != Text;
          if (HasEllipsis)
            textSprite.Data = AutoEllipsis == LabelEllipsis.Left
            ? $"...{Text.Substring(Math.Min(Text.Length - (text.Length - 3), Text.Length), Math.Max(0, text.Length - 3)).TrimStart()}"
            : $"{text.Substring(0, Math.Max(0, text.Length - 3)).TrimEnd()}...";
        }
      }

      if (UpdateHeight(textSprite.Data) > MaxLines)
      {
        var lines = textSprite.Data.Split('\n');
        var text = "";
        for (int i = 0; i < MaxLines; i++)
        {
          text = $"{text}{lines[i]}";
          if (i < MaxLines - 1)
            text = $"{text}\n";
        }
        textSprite.Data = text;
      }

      _data = textSprite.Data;
      _prevText = Text;
      _prevWidth = size.X;
      _prevScale = ThemeScale;
      _prevfontSize = _fontSize;

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