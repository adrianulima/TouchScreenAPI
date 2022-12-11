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
    protected MySprite TextSprite;

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

    public Vector2 Range;
    public float Value = 0;
    public Action<float> OnChange;

    public bool IsInteger = false;
    public bool AllowInput = true;
    protected bool InputOpen = false;
    protected bool SkipNext = false;

    public FancySlider(float min, float max, Action<float> onChange = null)
    {
      Range = new Vector2(min, max);
      Value = MathHelper.Clamp(Value, min, max);
      OnChange = onChange;

      Scale = new Vector2(1, 0);
      Margin = new Vector4(8, 0, 8, 0);
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

      handler.HitArea = new Vector4(Position.X, Position.Y, Position.X + Size.X, Position.Y + Size.Y);

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
        Color = App.Theme.Main_30
      };

      ProgressSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_60
      };

      HandlerSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "Circle",
        RotationOrScale = 0,
        Color = App.Theme.White
      };

      HandlerInnerSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "Circle",
        RotationOrScale = 0,
        Color = App.Theme.Main_60
      };

      if (handler.IsMousePressed)
      {
        BgSprite.Color = App.Theme.Main_20;

        var mouseX = MathHelper.Clamp(-0.04f + 1.08f * ((App.Cursor.Position.X - handler.HitArea.X) / (handler.HitArea.Z - handler.HitArea.X)), 0, 1);
        UpdateValue(Range.X + mouseX * (Range.Y - Range.X));
      }
      else if (handler.IsMouseOver)
      {
        BgSprite.Color = App.Theme.Main_20;
      }
      else
      {
        BgSprite.Color = App.Theme.Main_10;
      }

      var ratio = ((Value - Range.X) / (Range.Y - Range.X));
      var prgW = Size.X * ratio;

      var handlerOffset = (Size.Y / 2) * ((ratio * 1.4f) + 0.3f);
      HandlerSprite.Position = Position + new Vector2(prgW - handlerOffset, Size.Y / 2);
      HandlerSprite.Size = new Vector2(Size.Y * 0.8f, Size.Y * 0.8f);

      HandlerInnerSprite.Position = Position + new Vector2(prgW - handlerOffset + Size.Y * 0.15f, Size.Y / 2);
      HandlerInnerSprite.Size = new Vector2(Size.Y * 0.5f, Size.Y * 0.5f);

      ProgressSprite.Position = Position + new Vector2(0, Size.Y / 2);
      ProgressSprite.Size = new Vector2(prgW, Size.Y / 2);

      BgSprite.Position = Position + new Vector2(prgW, Size.Y / 2);
      BgSprite.Size = new Vector2(Size.X - prgW, Size.Y / 2);

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
      InnerTextField.Pixels = Pixels;
      InnerTextField.Offset = Offset;
      InnerTextField.Margin = Margin;
      InnerTextField.Text = $"{v}";
      InnerTextField.ToggleEdit(true, true);
      InnerTextField.InitElements();
    }

    protected void OnTextSubmit(string textValue)
    {
      float v = MathHelper.Clamp(float.Parse(textValue), Range.X, Range.Y);
      InnerTextField.Dispose();
      UpdateValue(v);
      InputOpen = false;
      SkipNext = true;
    }

  }
}