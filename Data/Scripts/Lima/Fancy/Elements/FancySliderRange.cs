using System;
using System.Collections.Generic;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancySliderRange : FancySlider
  {
    private MySprite handlerLowerSprite;
    private MySprite handlerInnerLowerSprite;
    private MySprite bgLowerSprite;
    public Action<float, float> _actionR;

    public float ValueLower = 0;

    public FancySliderRange(float min, float max, Action<float, float> action = null) : base(min, max)
    {
      _actionR = action;
    }

    protected override void UpdateValue(float value)
    {
      float diff = Math.Abs(value - Value);
      float diffLower = Math.Abs(value - ValueLower);

      if (!_inputOpen && AllowInput && MyAPIGateway.Input.IsAnyCtrlKeyPressed())
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

      if (_actionR != null)
        _actionR(ValueLower, Value);
    }

    public override void Update()
    {
      var skip = _skipNext;
      var input = _inputOpen;

      base.Update();

      if (input || skip)
        return;

      handlerLowerSprite = handlerSprite;
      handlerInnerLowerSprite = handlerInnerSprite;
      bgLowerSprite = bgSprite;
      if (IsMousePressed || IsMouseOver)
      {
        bgLowerSprite.Color = App.Theme.Main_20;
      }
      else
      {
        bgLowerSprite.Color = App.Theme.Main_10;
      }

      var ratio = (ValueLower - Range.X) / (Range.Y - Range.X);
      var prgW = Size.X * ratio;

      var handlerOffset = (Size.Y / 2) * ((ratio * 1.4f) + 0.3f);
      handlerLowerSprite.Position = Position + new Vector2(prgW - handlerOffset, Size.Y - Size.Y / 2);
      handlerLowerSprite.Size = new Vector2(Size.Y * 0.8f, Size.Y * 0.8f);

      handlerInnerLowerSprite.Position = Position + new Vector2(prgW - handlerOffset + Size.Y * 0.15f, Size.Y - Size.Y / 2);
      handlerInnerLowerSprite.Size = new Vector2(Size.Y * 0.5f, Size.Y * 0.5f);

      bgLowerSprite.Position = Position + new Vector2(0, Size.Y - Size.Y / 2);
      bgLowerSprite.Size = new Vector2(prgW, Size.Y / 2);

      var ratioRange = (Value - ValueLower) / (Range.Y - Range.X);
      progressSprite.Position = Position + new Vector2(prgW, Size.Y - Size.Y / 2);
      progressSprite.Size = new Vector2(Size.X * ratioRange, Size.Y / 2);

      sprites.Clear();

      sprites.Add(bgSprite);
      sprites.Add(bgLowerSprite);
      sprites.Add(progressSprite);
      sprites.Add(handlerLowerSprite);
      sprites.Add(handlerInnerLowerSprite);
      sprites.Add(handlerSprite);
      sprites.Add(handlerInnerSprite);
    }

  }
}