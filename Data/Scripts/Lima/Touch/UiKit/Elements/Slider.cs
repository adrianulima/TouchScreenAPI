using Sandbox.ModAPI;
using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Touch.UiKit.Elements
{
  public class Slider : View
  {
    public ClickHandler Handler = new ClickHandler();

    public float MinValue;
    public float MaxValue;
    public float Value = 0;
    public Action<float> OnChange;

    public bool IsInteger = false;
    public bool AllowInput = true;
    protected bool InputOpen = false;

    public BarContainer Bar;
    public EmptyElement Thumb;
    public TextField InnerTextField;

    public Slider(float min, float max, Action<float> onChange = null)
    {
      MinValue = min;
      MaxValue = max;
      Value = max;
      OnChange = onChange;

      Flex = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);

      InnerTextField = new TextField();
      InnerTextField.Text = $"{Value}";
      InnerTextField.OnSubmit = OnTextSubmit;
      InnerTextField.Enabled = false;
      AddChild(InnerTextField);

      Bar = new BarContainer();
      Bar.Pixels = new Vector2(0, 12);
      Bar.Flex = Vector2.UnitX;
      Bar.Margin = new Vector4(0, 6, 0, 6);
      AddChild(Bar);

      Thumb = new EmptyElement();
      Thumb.Flex = Vector2.Zero;
      Thumb.Absolute = true;
      Bar.AddChild(Thumb);
    }

    public override void Update()
    {
      if (InputOpen)
      {
        if (!InnerTextField.IsEditing)
          InnerTextField.ToggleEdit(true, true);

        base.Update();
        return;
      }

      Handler.Update(App.Screen);

      var size = GetSize();
      Handler.HitArea = new Vector4(Position.X, Position.Y, Position.X + size.X, Position.Y + size.Y);

      if (Handler.Mouse1.IsPressed)
      {
        var offset = size.Y / size.X;
        var cursorRatio = (App.Cursor.Position.X - Handler.HitArea.X) / (Handler.HitArea.Z - Handler.HitArea.X);
        var offsetRatio = MathHelper.Clamp(cursorRatio * (1 + offset) - (offset * 0.5f), 0, 1);
        UpdateValue(MinValue + offsetRatio * (MaxValue - MinValue));
      }

      if (UseThemeColors)
        ApplyThemeStyle();

      UpdateBar();

      base.Update();

      var ratio = ((Value - MinValue) / (MaxValue - MinValue));

      var thumbSize = Bar.Pixels.Y + 8;
      Thumb.Pixels = new Vector2(thumbSize, thumbSize);

      var barSize = Bar.GetSize();

      var scaledPixelsY = Bar.Pixels.Y * ThemeScale;
      thumbSize *= ThemeScale;

      var offsetWidth = thumbSize / 2;
      var handlerOffset = ratio * -(1 + offsetWidth) + (offsetWidth * 0.5f);
      Thumb.Position = Bar.Position + new Vector2(barSize.X * ratio + handlerOffset - thumbSize / 2, scaledPixelsY / 2 - thumbSize / 2);

      var handlerSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "Circle",
        RotationOrScale = 0,
        Color = App.Theme.WhiteColor
      };

      var handlerInnerSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "Circle",
        RotationOrScale = 0,
        Color = Bar.Bar.BgColor
      };

      handlerSprite.Position = Thumb.Position + new Vector2(0, thumbSize / 2);
      handlerSprite.Size = new Vector2(thumbSize, thumbSize);

      handlerInnerSprite.Position = Thumb.Position + new Vector2(thumbSize / 2 - scaledPixelsY / 2, thumbSize / 2);
      handlerInnerSprite.Size = new Vector2(scaledPixelsY, scaledPixelsY);

      Thumb.GetSprites().Clear();

      Thumb.GetSprites().Add(handlerSprite);
      Thumb.GetSprites().Add(handlerInnerSprite);
    }

    private void ApplyThemeStyle()
    {
      Bar.Bar.BgColor = App.Theme.MainColor_5;

      if (Handler.Mouse1.IsPressed)
        Bar.BgColor = App.Theme.MainColor_3;
      else if (Handler.Mouse1.IsOver)
        Bar.BgColor = App.Theme.MainColor_3;
      else
        Bar.BgColor = App.Theme.MainColor_2;
    }

    protected virtual void UpdateValue(float value)
    {
      if (!InputOpen && AllowInput && MyAPIGateway.Input.IsAnyCtrlKeyPressed())
      {
        PresentTextInput(Value);
        return;
      }
      Value = value;

      if (IsInteger)
        Value = (float)Math.Round(Value);

      if (OnChange != null)
        OnChange(Value);
    }

    protected virtual void UpdateBar()
    {
      var ratio = ((Value - MinValue) / (MaxValue - MinValue));
      Bar.Ratio = ratio;
    }

    protected void PresentTextInput(float v)
    {
      InputOpen = true;

      Bar.Enabled = false;
      InnerTextField.Enabled = true;
      InnerTextField.IsNumeric = true;
      InnerTextField.IsInteger = IsInteger;
      InnerTextField.AllowNegative = MinValue < 0;
      InnerTextField.Flex = Flex;
      InnerTextField.Position = Position;
      InnerTextField.Pixels = Pixels;
      InnerTextField.Margin = Margin;
      InnerTextField.Text = $"{v}";
    }

    protected void OnTextSubmit(string textValue)
    {
      UpdateValue(MathHelper.Clamp(float.Parse(textValue), MinValue, MaxValue));
      InputOpen = false;
      InnerTextField.Enabled = false;
      Bar.Enabled = true;
    }

  }
}