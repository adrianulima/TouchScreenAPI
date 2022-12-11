using System;
using Lima.Utils;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyTextField : FancyButtonBase
  {
    private MySprite _bgSprite;
    private MySprite _textSprite;

    public string Text;
    public Action<string> OnChange;

    private TextInputHandler _inputHandler;
    private string _maxText;
    private bool _blink = false;
    private bool _blinkCaret = false;
    private bool _edit = false;

    public bool IsNumeric = false;
    public bool IsInteger = false;
    public bool AllowNegative = true;
    public TextAlignment Alignment = TextAlignment.CENTER;

    public FancyTextField(string text, Action<string> onChange)
    {
      Text = text;
      OnChange = onChange;
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
        OnChange(Text);

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
        RotationOrScale = 0.6f * App.Theme.Scale,
        Color = App.Theme.White,//Theme.Main,
        Alignment = Alignment,
        FontId = App.Theme.Font
      };

      if (_edit)
      {
        Blink();
        _bgSprite.Color = _blink ? App.Theme.Main_20 : App.Theme.Main_10;
      }
      else if (handler.IsMousePressed || handler.IsMouseOver)
      {
        _bgSprite.Color = App.Theme.Main_20;
      }
      else
      {
        _bgSprite.Color = App.Theme.Main_10;
      }

      if (handler.JustReleased)
      {
        ToggleEdit();
      }
      else if (_edit && !handler.IsMouseOver)
      {
        ToggleEdit(true, false);
      }

      Sprites.Clear();

      _bgSprite.Position = Position + new Vector2(0, Size.Y / 2);
      _bgSprite.Size = Size;

      if (_maxText != Text)
      {
        var tx = App.Theme.MeasureStringInPixels(Text, _textSprite.FontId, _textSprite.RotationOrScale).X;
        if (tx <= Size.X)
          _maxText = Text.Substring(0, Math.Max(0, Text.Length - 3)) + "...";
        else
        {
          _textSprite.Data = _maxText;
          _bgSprite.Color = _blink ? App.Theme.Main_20 : Color.Red;
        }
      }

      var caretX = 1.5f * App.Theme.Scale;
      if (_edit && _blinkCaret && _textSprite.Data != _maxText)
      {
        _textSprite.Data = _textSprite.Data + "|";
        caretX = 0;
      }


      if (Alignment == TextAlignment.LEFT)
        _textSprite.Position = Position + new Vector2(0, Size.Y * 0.5f - (_textSprite.RotationOrScale * 16.6f));
      else if (Alignment == TextAlignment.RIGHT)
        _textSprite.Position = Position + new Vector2(Size.X - caretX * 2, Size.Y * 0.5f - (_textSprite.RotationOrScale * 16.6f));
      else
        _textSprite.Position = Position + new Vector2(Size.X / 2 - caretX, Size.Y * 0.5f - (_textSprite.RotationOrScale * 16.6f));


      Sprites.Add(_bgSprite);
      Sprites.Add(_textSprite);
    }

    private void Blink()
    {
      if (_blink)
        _blinkCaret = !_blinkCaret;
      _blink = !_blink;
    }

  }
}