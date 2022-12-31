using System;
using Lima.Utils;
using Sandbox.ModAPI;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyTextField : FancyView
  {
    public ClickHandler Handler = new ClickHandler();

    public FancyLabel Label;

    public string Text;
    public Action<string, bool> OnChange;

    private TextInputHandler _inputHandler;
    private string _saveText = "";
    private bool _blink = false;
    private bool _blinkCaret = false;

    public bool IsEditing { get; private set; } = false;
    public bool IsNumeric = false;
    public bool IsInteger = false;
    public bool AllowNegative = true;

    public FancyTextField(string text, Action<string, bool> onChange)
    {
      Text = text;
      OnChange = onChange;
      _inputHandler = new TextInputHandler(AddChar, RemoveLastChar, OnInput);

      Scale = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);

      Anchor = ViewAlignment.Center;
      Alignment = ViewAlignment.Center;

      Label = new FancyLabel(text, 0.6f, TextAlignment.CENTER);
      AddChild(Label);
    }

    public override void OnAddedToApp()
    {
      base.OnAddedToApp();
      App.UpdateAfterSimulationEvent -= UpdateAfterSimulation;
      App.UpdateAfterSimulationEvent += UpdateAfterSimulation;
      MyAPIGateway.Gui.GuiControlCreated -= OnGuiControlCreated;
      MyAPIGateway.Gui.GuiControlCreated += OnGuiControlCreated;
    }

    public override void Dispose()
    {
      base.Dispose();
      App.UpdateAfterSimulationEvent -= UpdateAfterSimulation;
      MyAPIGateway.Gui.GuiControlCreated -= OnGuiControlCreated;
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

    private void OnGuiControlCreated(object _)
    {
      if (IsEditing)
        ToggleEdit(true, false, true);
    }

    public void ToggleEdit(bool force = false, bool value = false, bool revert = false)
    {
      if (force)
        IsEditing = value;
      else
        IsEditing = !IsEditing;

      if (!IsEditing)
      {
        if (revert)
          Text = _saveText;

        OnChange(Text, revert);
      }
      else
        _saveText = Text;

      InputUtils.SetPlayerKeyboardBlacklistState(IsEditing);
    }

    public void UpdateAfterSimulation()
    {
      if (IsEditing)
        _inputHandler.Update();
    }

    public override void Update()
    {
      var size = GetSize();

      Handler.HitArea = new Vector4(Position.X, Position.Y, Position.X + size.X, Position.Y + size.Y);
      Handler.UpdateStatus(App.Screen);

      if (IsEditing)
      {
        Blink();
        BgColor = _blink ? App.Theme.MainColor_3 : App.Theme.MainColor_2;
      }
      else if (Handler.IsMousePressed || Handler.IsMouseOver)
        BgColor = App.Theme.MainColor_3;
      else
        BgColor = App.Theme.MainColor_2;

      if (Handler.JustPressed)
        ToggleEdit(false, false, Text == _saveText);
      else if (IsEditing && !Handler.IsMouseOver)
        ToggleEdit(true, false, Text == _saveText);

      Label.Text = Text;

      if (IsEditing && _blinkCaret && !Label.IsShortened)
      {
        Label.Text = Label.Text + "|";
        base.Update();
      }
      else
      {
        base.Update();
        if (Label.GetSprites().Count > 0)
        {
          var textSrite = Label.GetSprites()[0];
          var caretX = 1.5f * ThemeScale;
          if (Label.Alignment == TextAlignment.RIGHT)
            textSrite.Position = textSrite.Position + new Vector2(-caretX * 2, 0);
          else if (Label.Alignment == TextAlignment.CENTER)
            textSrite.Position = textSrite.Position + new Vector2(-caretX, 0);

          Label.GetSprites()[0] = textSrite;
        }
      }
    }

    private void Blink()
    {
      if (_blink)
        _blinkCaret = !_blinkCaret;
      _blink = !_blink;
    }

  }
}