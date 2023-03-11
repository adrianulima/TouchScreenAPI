using Lima.Utils;
using Sandbox.ModAPI;
using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Touch.UiKit.Elements
{
  public class TextField : View
  {
    public ClickHandler Handler = new ClickHandler();

    public Label Label;

    public string Text = "";
    public Action<string> OnSubmit;
    public Action<string> OnBlur;

    private TextInputHandler _inputHandler;
    private string _saveText = "";
    private bool _blink = false;
    private bool _blinkCaret = false;

    public bool IsEditing { get; private set; } = false;
    public bool IsNumeric = false;
    public bool IsInteger = false;
    public bool AllowNegative = true;

    public bool RevertOnBlur = false;
    public bool SubmitOnBlur = true;

    public TextField()
    {
      _inputHandler = new TextInputHandler(AddChar, RemoveLastChar, OnInput);

      Flex = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);

      Anchor = ViewAnchor.Center;
      Alignment = ViewAlignment.Center;

      Label = new Label(Text, 0.6f, TextAlignment.CENTER);
      Label.AutoEllipsis = LabelEllipsis.Left;
      AddChild(Label);
    }

    public override void OnAddedToApp()
    {
      base.OnAddedToApp();
      App.UpdateAtSimulationEvent -= UpdateAtSimulation;
      App.UpdateAtSimulationEvent += UpdateAtSimulation;
      MyAPIGateway.Gui.GuiControlCreated -= OnGuiControlCreated;
      MyAPIGateway.Gui.GuiControlCreated += OnGuiControlCreated;
    }

    public override void Dispose()
    {
      base.Dispose();
      App.UpdateAtSimulationEvent -= UpdateAtSimulation;
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
        ToggleEdit(true, false, false);
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

    public void Blur()
    {
      if (IsEditing)
      {
        if (RevertOnBlur)
          Text = _saveText;
        IsEditing = false;
        InputUtils.SetPlayerKeyboardBlacklistState(IsEditing);
      }
    }

    public void Focus()
    {
      if (!IsEditing)
        ToggleEdit(true, true, false);
    }

    public void ToggleEdit(bool force = false, bool value = false, bool blur = false)
    {
      if (force)
        IsEditing = value;
      else
        IsEditing = !IsEditing;

      if (!IsEditing)
      {
        if (blur)
        {
          if (RevertOnBlur)
            Text = _saveText;

          if (SubmitOnBlur && OnSubmit != null)
            OnSubmit(Text);

          if (OnBlur != null)
            OnBlur(Text);
        }
        else if (OnSubmit != null)
          OnSubmit(Text);
      }
      else
        _saveText = Text;

      InputUtils.SetPlayerKeyboardBlacklistState(IsEditing);
    }

    public void UpdateAtSimulation()
    {
      if (IsEditing)
        _inputHandler.Update();
    }

    public override void Update()
    {
      var size = GetBoundaries();

      Handler.HitArea = new Vector4(Position.X, Position.Y, Position.X + size.X, Position.Y + size.Y);
      Handler.Update(App.Screen);

      if (IsEditing)
        Blink();

      if (UseThemeColors)
        ApplyThemeStyle();

      if (Handler.Mouse1.JustPressed)
        ToggleEdit(false, false, false);
      else if (IsEditing && ((!Handler.Mouse1.IsOver && Handler.Mouse1.WasPressedOutside) || !App.Screen.IsOnScreen))
        ToggleEdit(true, false, true);

      Label.Text = Text;
      Label.AutoEllipsis = IsEditing ? LabelEllipsis.Left : LabelEllipsis.Right;

      if (IsEditing && _blinkCaret)
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

    private void ApplyThemeStyle()
    {
      if (IsEditing)
        BgColor = App.Theme.MainColor_3;//_blink ? App.Theme.MainColor_3 : App.Theme.MainColor_2;
      else if (Handler.Mouse1.IsOver)
        BgColor = App.Theme.MainColor_3;
      else
        BgColor = App.Theme.MainColor_2;
    }

    private void Blink()
    {
      if (_blink)
        _blinkCaret = !_blinkCaret;
      _blink = !_blink;
    }

  }
}