using System;
using Sandbox.ModAPI;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancySliderRange : FancySlider
  {
    public Action<float, float> OnChangeR;

    public float ValueLower = 0;

    public FancySliderRange(float min, float max, Action<float, float> onChange = null) : base(min, max)
    {
      OnChangeR = onChange;

      Scale = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);
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

    public override void Update()
    {
      var skip = SkipNext;
      var input = InputOpen;

      base.Update();

      if (input || skip)
        return;

      var handlerLowerSprite = HandlerSprite;
      var handlerInnerLowerSprite = HandlerInnerSprite;
      var bgLowerSprite = BgSprite;
      if (Handler.IsMousePressed || Handler.IsMouseOver)
        bgLowerSprite.Color = App.Theme.MainColor_3;
      else
        bgLowerSprite.Color = App.Theme.MainColor_2;

      var size = GetSize();
      var ratio = (ValueLower - MinValue) / (MaxValue - MinValue);
      var prgW = size.X * ratio;

      var handlerOffset = -(size.Y * 0.4f) - (size.Y * 0.4f) * (ratio * 1.8f - 0.9f);
      handlerLowerSprite.Position = Position + new Vector2(prgW + handlerOffset, size.Y - size.Y / 2);
      handlerLowerSprite.Size = new Vector2(size.Y * 0.8f, size.Y * 0.8f);

      handlerInnerLowerSprite.Position = Position + new Vector2(prgW + handlerOffset + size.Y * 0.15f, size.Y - size.Y / 2);
      handlerInnerLowerSprite.Size = new Vector2(size.Y * 0.5f, size.Y * 0.5f);

      bgLowerSprite.Position = Position + new Vector2(0, size.Y - size.Y / 2);
      bgLowerSprite.Size = new Vector2(prgW, size.Y / 2);

      var ratioRange = (Value - ValueLower) / (MaxValue - MinValue);
      ProgressSprite.Position = Position + new Vector2(prgW, size.Y - size.Y / 2);
      ProgressSprite.Size = new Vector2(size.X * ratioRange, size.Y / 2);

      Sprites.Clear();

      Sprites.Add(BgSprite);
      Sprites.Add(bgLowerSprite);
      Sprites.Add(ProgressSprite);
      Sprites.Add(handlerLowerSprite);
      Sprites.Add(handlerInnerLowerSprite);
      Sprites.Add(HandlerSprite);
      Sprites.Add(HandlerInnerSprite);
    }

  }
}