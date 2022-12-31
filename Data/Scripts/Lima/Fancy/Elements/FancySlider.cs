using System;
using Sandbox.ModAPI;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancySlider : FancyView
  {
    public ClickHandler Handler = new ClickHandler();

    public float MinValue;
    public float MaxValue;
    public float Value = 0;
    public Action<float> OnChange;

    public bool IsInteger = false;
    public bool AllowInput = true;
    protected bool InputOpen = false;

    public FancyBarContainer Bar;
    public FancyEmptyElement Thumb;
    public FancyTextField InnerTextField;

    public FancySlider(float min, float max, Action<float> onChange = null)
    {
      MinValue = min;
      MaxValue = max;
      Value = max;
      OnChange = onChange;

      Scale = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);

      InnerTextField = new FancyTextField($"{Value}", OnTextSubmit);
      InnerTextField.Enabled = false;
      AddChild(InnerTextField);

      Bar = new FancyBarContainer();
      Bar.Pixels = new Vector2(0, 12);
      Bar.Scale = Vector2.UnitX;
      Bar.Margin = new Vector4(0, 6, 0, 6);
      AddChild(Bar);

      Thumb = new FancyEmptyElement();
      Thumb.Scale = Vector2.Zero;
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

      Handler.UpdateStatus(App.Screen);

      var size = GetSize();
      Handler.HitArea = new Vector4(Position.X, Position.Y, Position.X + size.X, Position.Y + size.Y);

      Bar.Bar.BgColor = App.Theme.MainColor_7;

      if (Handler.IsMousePressed)
      {
        Bar.BgColor = App.Theme.MainColor_3;

        var offset = size.Y / size.X;
        var cursorRatio = (App.Cursor.Position.X - Handler.HitArea.X) / (Handler.HitArea.Z - Handler.HitArea.X);
        var offsetRatio = MathHelper.Clamp(cursorRatio * (1 + offset) - (offset * 0.5f), 0, 1);
        UpdateValue(MinValue + offsetRatio * (MaxValue - MinValue));
      }
      else if (Handler.IsMouseOver)
        Bar.BgColor = App.Theme.MainColor_3;
      else
        Bar.BgColor = App.Theme.MainColor_2;

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
        Color = App.Theme.MainColor_7
      };

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

      handlerSprite.Position = Thumb.Position + new Vector2(0, thumbSize / 2);
      handlerSprite.Size = new Vector2(thumbSize, thumbSize);

      handlerInnerSprite.Position = Thumb.Position + new Vector2(thumbSize / 2 - scaledPixelsY / 2, thumbSize / 2);
      handlerInnerSprite.Size = new Vector2(scaledPixelsY, scaledPixelsY);

      Thumb.GetSprites().Clear();

      Thumb.GetSprites().Add(handlerSprite);
      Thumb.GetSprites().Add(handlerInnerSprite);
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
      InnerTextField.Scale = Scale;
      InnerTextField.Position = Position;
      InnerTextField.Pixels = Pixels;
      InnerTextField.Margin = Margin;
      InnerTextField.Text = $"{v}";
    }

    protected void OnTextSubmit(string textValue)
    {
      float v = MathHelper.Clamp(float.Parse(textValue), MinValue, MaxValue);
      UpdateValue(v);
      InputOpen = false;
      InnerTextField.Enabled = false;
      Bar.Enabled = true;
    }

  }
}