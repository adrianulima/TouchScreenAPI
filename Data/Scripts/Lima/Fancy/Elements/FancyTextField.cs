using System;
using Lima.Utils;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyTextField : FancyButtonBase
  {
    private MySprite bgSprite;
    private MySprite textSprite;

    public string Text;
    public Action<string> _action;

    private TextInputHandler _inputHandler;
    private string _maxText;
    private bool _blink = false;
    private bool _blinkCaret = false;
    private bool _edit = false;

    public bool IsNumeric = false;
    public bool IsInteger = false;
    public bool AllowNegative = true;
    public TextAlignment Alignment = TextAlignment.CENTER;

    public FancyTextField(string text, Action<string> action)
    {
      Text = text;
      _action = action;
      _inputHandler = new TextInputHandler(AddChar, RemoveLastChar, OnInput);

      Scale = new Vector2(1, 0);
      Margin = new Vector4(8, 0, 8, 0);
      Pixels = new Vector2(0, 24);
    }

    public override void InitElements()
    {
      base.InitElements();
      App.UpdateEvent += UpdateFast;
    }

    public override void Dispose()
    {
      base.Dispose();
      App.UpdateEvent -= UpdateFast;
    }

    private void AddChar(char ch)
    {
      Text = Text + ch;
    }

    private void RemoveLastChar()
    {
      var len = Text.Length;
      if (len > 0)
        Text = Text.Substring(0, len - 1);
    }

    private bool OnInput(char ch)
    {
      if (ch == '\n' || ch == '\r' || ch == '\t')
      {
        ToggleEdit(true, false);
        return false;
      }

      if (IsNumeric)
        return InputUtils.CheckNumericInput(Text, ch, AllowNegative, IsInteger);

      return ch >= ' ';
    }

    public void ToggleEdit(bool force = false, bool value = false)
    {
      if (force)
        _edit = value;
      else
        _edit = !_edit;

      if (!_edit)
        _action(Text);

      InputUtils.SetPlayerKeyboardBlacklistState(_edit);
    }

    public void UpdateFast()
    {
      if (_edit)
        _inputHandler.Update();
    }

    public override void Update()
    {
      handler.HitArea = new Vector4(Position.X, Position.Y, Position.X + Size.X, Position.Y + Size.Y);

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
        RotationOrScale = 0.6f * App.Theme.Scale,
        Color = App.Theme.White,//Theme.Main,
        Alignment = Alignment,
        FontId = App.Theme.Font
      };

      if (_edit)
      {
        Blink();
        bgSprite.Color = _blink ? App.Theme.Main_20 : App.Theme.Main_10;
      }
      else if (handler.IsMousePressed || handler.IsMouseOver)
      {
        bgSprite.Color = App.Theme.Main_20;
      }
      else
      {
        bgSprite.Color = App.Theme.Main_10;
      }

      if (handler.JustReleased)
      {
        ToggleEdit();
      }
      else if (_edit && !handler.IsMouseOver)
      {
        ToggleEdit(true, false);
      }

      sprites.Clear();

      bgSprite.Position = Position + new Vector2(0, Size.Y / 2);
      bgSprite.Size = Size;

      if (_maxText != Text)
      {
        var tx = App.Theme.MeasureStringInPixels(Text, textSprite.FontId, textSprite.RotationOrScale).X;
        if (tx <= Size.X)
          _maxText = Text.Substring(0, Math.Max(0, Text.Length - 3)) + "...";
        else
        {
          textSprite.Data = _maxText;
          bgSprite.Color = _blink ? App.Theme.Main_20 : Color.Red;
        }
      }

      var caretX = 1.5f * App.Theme.Scale;
      if (_edit && _blinkCaret && textSprite.Data != _maxText)
      {
        textSprite.Data = textSprite.Data + "|";
        caretX = 0;
      }


      if (Alignment == TextAlignment.LEFT)
        textSprite.Position = Position + new Vector2(0, Size.Y * 0.5f - (Size.Y / 2.4f));
      else if (Alignment == TextAlignment.RIGHT)
        textSprite.Position = Position + new Vector2(Size.X - caretX * 2, Size.Y * 0.5f - (Size.Y / 2.4f));
      else
        textSprite.Position = Position + new Vector2(Size.X / 2 - caretX, Size.Y * 0.5f - (Size.Y / 2.4f));


      sprites.Add(bgSprite);
      sprites.Add(textSprite);
    }

    private void Blink()
    {
      if (_blink)
        _blinkCaret = !_blinkCaret;
      _blink = !_blink;
    }

  }
}