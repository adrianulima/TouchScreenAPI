using VRageMath;

namespace Lima.Touch.UiKit.Elements
{
  public class AlertPanel : TouchView
  {
    public TouchLabel Label;

    public AlertPanel(string text)
    {
      Absolute = true;
      Alignment = ViewAlignment.Center;
      Anchor = ViewAnchor.Center;
      Padding = new Vector4(4);
      Border = new Vector4(2);
      BorderColor = Color.DarkRed;
      BgColor = Color.Black;
      Flex = Vector2.Zero;

      Label = new TouchLabel(text);
      Label.AutoBreakLine = true;
      AddChild(Label);
    }

    public override void Update()
    {
      base.Update();

      var w = Parent.Pixels.X < 128 ? Parent.Pixels.X : (Parent.Pixels.X / 1.5f);
      Label.FontSize = MathHelper.Max(0.4f, w / 400);

      var h = Label.Pixels.Y + 12;
      Pixels = new Vector2(w, h);
      Position = Parent.Position + new Vector2(Parent.Pixels.X * 0.5f - w / 2, Parent.Pixels.Y * 0.5f - h * 0.5f);
    }

  }
}
