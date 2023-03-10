using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Touch.UiKit.Elements
{
  public class TouchCheckbox : TouchView
  {
    public ClickHandler Handler = new ClickHandler();

    public bool Value;
    public Action<bool> OnChange;

    public TouchEmptyElement CheckMark;

    public TouchCheckbox(Action<bool> onChange, bool value = false)
    {
      OnChange = onChange;
      Value = value;

      Flex = new Vector2(0, 0);
      Pixels = new Vector2(20, 20);
      Border = new Vector4(2);

      Anchor = ViewAnchor.Center;
      Alignment = ViewAlignment.Center;

      CheckMark = new TouchEmptyElement();
      AddChild(CheckMark);
    }

    public override void Update()
    {
      var size = GetBoundaries();

      Handler.HitArea = new Vector4(Position.X, Position.Y, Position.X + size.X, Position.Y + size.Y);
      Handler.UpdateStatus(App.Screen);

      if (UseThemeColors)
        ApplyThemeStyle();


      if (Handler.JustReleased)
      {
        Value = !Value;
        OnChange(Value);
      }

      base.Update();

      CheckMark.GetSprites().Clear();

      var checkSize = CheckMark.GetSize();
      if (Value)
      {
        var check1 = new MySprite()
        {
          Type = SpriteType.TEXTURE,
          Data = "Triangle",
          RotationOrScale = -MathHelper.PiOver4,
          Color = App.Theme.WhiteColor
        };

        check1.Position = CheckMark.Position + new Vector2(checkSize.X / 2 - checkSize.X / 3, checkSize.Y / 2 + checkSize.Y / 12);
        check1.Size = new Vector2(3 * ThemeScale, checkSize.Y / 1.8f);

        var check2 = check1;
        check2.RotationOrScale = MathHelper.PiOver4;
        check2.Position = CheckMark.Position + new Vector2(checkSize.X / 2 + checkSize.X / 8, checkSize.Y / 2);
        check2.Size = new Vector2(3.6f * ThemeScale, checkSize.Y);

        CheckMark.GetSprites().Add(check1);
        CheckMark.GetSprites().Add(check2);
      }
    }

    private void ApplyThemeStyle()
    {
      if (Handler.IsMousePressed)
      {
        BorderColor = App.Theme.MainColor_8;
        BgColor = App.Theme.MainColor_6;
      }
      else if (Handler.IsMouseOver)
      {
        BorderColor = App.Theme.MainColor_5;
        BgColor = App.Theme.MainColor_3;
      }
      else
      {
        BorderColor = App.Theme.MainColor_4;
        BgColor = App.Theme.MainColor_2;
      }
    }
  }
}