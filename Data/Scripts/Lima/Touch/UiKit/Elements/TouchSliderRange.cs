using System;
using Sandbox.ModAPI;
using VRageMath;

namespace Lima.Touch.UiKit.Elements
{
  public class TouchSliderRange : TouchSlider
  {
    public Action<float, float> OnChangeR;

    public float ValueLower = 0;

    public TouchEmptyElement ThumbLower;

    public TouchSliderRange(float min, float max, Action<float, float> onChange = null) : base(min, max)
    {
      OnChangeR = onChange;

      Scale = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);

      ThumbLower = new TouchEmptyElement();
      ThumbLower.Scale = Vector2.Zero;
      ThumbLower.Absolute = true;
      Bar.AddChild(ThumbLower);
    }

    public override void Update()
    {
      if (InputOpen)
      {
        base.Update();
        return;
      }

      base.Update();

      var ratio = (ValueLower - MinValue) / (MaxValue - MinValue);

      ThumbLower.Pixels = Thumb.Pixels;

      var thumbSize = Bar.Pixels.Y + 8;
      var barSize = Bar.GetSize();
      var scaledPixelsY = Bar.Pixels.Y * ThemeScale;
      thumbSize *= ThemeScale;

      var offsetWidth = thumbSize / 2;
      var handlerOffset = ratio * -(1 + offsetWidth) + (offsetWidth * 0.5f);
      ThumbLower.Position = Bar.Position + new Vector2(barSize.X * ratio + handlerOffset - thumbSize / 2, scaledPixelsY / 2 - thumbSize / 2);

      var sprites = Thumb.GetSprites();
      var handlerSprite = sprites[0];
      var handlerInnerSprite = sprites[1];
      handlerSprite.Position = ThumbLower.Position + new Vector2(0, thumbSize / 2);
      handlerInnerSprite.Position = ThumbLower.Position + new Vector2(thumbSize / 2 - scaledPixelsY / 2, thumbSize / 2);
      ThumbLower.GetSprites().Clear();
      ThumbLower.GetSprites().Add(handlerSprite);
      ThumbLower.GetSprites().Add(handlerInnerSprite);
    }

    protected override void UpdateValue(float value)
    {
      float diff = Math.Abs(value - Value);
      float diffLower = Math.Abs(value - ValueLower);

      if (!InputOpen && AllowInput && MyAPIGateway.Input.IsAnyCtrlKeyPressed())
      {
        PresentTextInput(diff < diffLower ? Value : ValueLower);
        return;
      }

      if (diff < diffLower)
        Value = value;
      else
        ValueLower = value;

      if (ValueLower > Value)
      {
        var v = Value;
        Value = ValueLower;
        ValueLower = v;
      }

      if (IsInteger)
      {
        Value = (float)Math.Round(Value);
        ValueLower = (float)Math.Round(ValueLower);
      }

      if (OnChangeR != null)
        OnChangeR(ValueLower, Value);
    }

    protected override void UpdateBar()
    {
      var ratio = (Value - ValueLower) / (MaxValue - MinValue);
      var offset = (ValueLower - MinValue) / ((ValueLower - MinValue) + (MaxValue - Value));
      Bar.Ratio = ratio;
      Bar.Offset = float.IsNaN(offset) ? 0 : offset;
    }

  }
}