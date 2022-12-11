using System;
using Sandbox.ModAPI;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancySliderRange : FancySlider
  {
    private MySprite _handlerLowerSprite;
    private MySprite _handlerInnerLowerSprite;
    private MySprite _bgLowerSprite;
    public Action<float, float> OnChangeR;

    public float ValueLower = 0;

    public FancySliderRange(float min, float max, Action<float, float> onChange = null) : base(min, max)
    {
      OnChangeR = onChange;

      Scale = new Vector2(1, 0);
      Margin = new Vector4(8, 0, 8, 0);
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
      {
        Value = value;
      }
      else
      {
        ValueLower = value;
      }

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

      _handlerLowerSprite = HandlerSprite;
      _handlerInnerLowerSprite = HandlerInnerSprite;
      _bgLowerSprite = BgSprite;
      if (handler.IsMousePressed || handler.IsMouseOver)
      {
        _bgLowerSprite.Color = App.Theme.Main_20;
      }
      else
      {
        _bgLowerSprite.Color = App.Theme.Main_10;
      }

      var ratio = (ValueLower - Range.X) / (Range.Y - Range.X);
      var prgW = Size.X * ratio;

      var handlerOffset = (Size.Y / 2) * ((ratio * 1.4f) + 0.3f);
      _handlerLowerSprite.Position = Position + new Vector2(prgW - handlerOffset, Size.Y - Size.Y / 2);
      _handlerLowerSprite.Size = new Vector2(Size.Y * 0.8f, Size.Y * 0.8f);

      _handlerInnerLowerSprite.Position = Position + new Vector2(prgW - handlerOffset + Size.Y * 0.15f, Size.Y - Size.Y / 2);
      _handlerInnerLowerSprite.Size = new Vector2(Size.Y * 0.5f, Size.Y * 0.5f);

      _bgLowerSprite.Position = Position + new Vector2(0, Size.Y - Size.Y / 2);
      _bgLowerSprite.Size = new Vector2(prgW, Size.Y / 2);

      var ratioRange = (Value - ValueLower) / (Range.Y - Range.X);
      ProgressSprite.Position = Position + new Vector2(prgW, Size.Y - Size.Y / 2);
      ProgressSprite.Size = new Vector2(Size.X * ratioRange, Size.Y / 2);

      Sprites.Clear();

      Sprites.Add(BgSprite);
      Sprites.Add(_bgLowerSprite);
      Sprites.Add(ProgressSprite);
      Sprites.Add(_handlerLowerSprite);
      Sprites.Add(_handlerInnerLowerSprite);
      Sprites.Add(HandlerSprite);
      Sprites.Add(HandlerInnerSprite);
    }

  }
}