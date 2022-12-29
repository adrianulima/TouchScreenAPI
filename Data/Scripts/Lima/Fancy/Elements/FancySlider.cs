using System;
using Sandbox.ModAPI;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancySlider : FancyButtonBase
  {
    protected MySprite BgSprite;
    protected MySprite ProgressSprite;
    protected MySprite HandlerSprite;
    protected MySprite HandlerInnerSprite;

    private FancyTextField _innerTextField;
    public FancyTextField InnerTextField
    {
      get
      {
        if (_innerTextField == null)
          _innerTextField = new FancyTextField(Value + "", OnTextSubmit);

        return _innerTextField;
      }
    }

    public float MinValue;
    public float MaxValue;
    public float Value = 0;
    public Action<float> OnChange;

    public bool IsInteger = false;
    public bool AllowInput = true;
    protected bool InputOpen = false;
    protected bool SkipNext = false;

    public FancySlider(float min, float max, Action<float> onChange = null)
    {
      MinValue = min;
      MaxValue = max;
      Value = max;
      OnChange = onChange;

      Scale = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);
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

    public override void Update()
    {
      if (InputOpen)
      {
        InnerTextField.Update();
        Sprites.Clear();
        Sprites.AddRange(InnerTextField.GetSprites());
        return;
      }

      var size = GetSize();
      Handler.HitArea = new Vector4(Position.X, Position.Y, Position.X + size.X, Position.Y + size.Y);

      base.Update();

      if (SkipNext)
      {
        SkipNext = false;
        return;
      }

      BgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.MainColor_4
      };

      ProgressSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.MainColor_7
      };

      HandlerSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "Circle",
        RotationOrScale = 0,
        Color = App.Theme.WhiteColor
      };

      HandlerInnerSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "Circle",
        RotationOrScale = 0,
        Color = App.Theme.MainColor_7
      };

      if (Handler.IsMousePressed)
      {
        BgSprite.Color = App.Theme.MainColor_3;

        var mouseX = MathHelper.Clamp(-0.04f + 1.08f * ((App.Cursor.Position.X - Handler.HitArea.X) / (Handler.HitArea.Z - Handler.HitArea.X)), 0, 1);
        UpdateValue(MinValue + mouseX * (MaxValue - MinValue));
      }
      else if (Handler.IsMouseOver)
        BgSprite.Color = App.Theme.MainColor_3;
      else
        BgSprite.Color = App.Theme.MainColor_2;

      var ratio = ((Value - MinValue) / (MaxValue - MinValue));
      var prgW = size.X * ratio;

      var handlerOffset = -(size.Y * 0.4f) - (size.Y * 0.4f) * (ratio * 1.8f - 0.9f);
      HandlerSprite.Position = Position + new Vector2(prgW + handlerOffset, size.Y / 2);
      HandlerSprite.Size = new Vector2(size.Y * 0.8f, size.Y * 0.8f);

      HandlerInnerSprite.Position = Position + new Vector2(prgW + handlerOffset + size.Y * 0.15f, size.Y / 2);
      HandlerInnerSprite.Size = new Vector2(size.Y * 0.5f, size.Y * 0.5f);

      ProgressSprite.Position = Position + new Vector2(0, size.Y / 2);
      ProgressSprite.Size = new Vector2(prgW, size.Y / 2);

      BgSprite.Position = Position + new Vector2(prgW, size.Y / 2);
      BgSprite.Size = new Vector2(size.X - prgW, size.Y / 2);

      Sprites.Clear();

      Sprites.Add(BgSprite);
      Sprites.Add(ProgressSprite);
      Sprites.Add(HandlerSprite);
      Sprites.Add(HandlerInnerSprite);
    }

    protected void PresentTextInput(float v)
    {
      InputOpen = true;

      InnerTextField.IsNumeric = true;
      InnerTextField.IsInteger = IsInteger;
      InnerTextField.Parent = Parent;
      InnerTextField.Scale = Scale;
      InnerTextField.Position = Position;
      InnerTextField.Pixels = Pixels;
      InnerTextField.Margin = Margin;
      InnerTextField.Text = $"{v}";
      InnerTextField.ToggleEdit(true, true);
      InnerTextField.OnAddedToApp();
    }

    protected void OnTextSubmit(string textValue)
    {
      float v = MathHelper.Clamp(float.Parse(textValue), MinValue, MaxValue);
      InnerTextField.Dispose();
      UpdateValue(v);
      InputOpen = false;
      SkipNext = true;
    }

  }
}