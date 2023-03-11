using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Touch.UiKit.Elements
{
  public class Button : EmptyButton
  {
    public Label Label;

    public Button(string text, Action onChange) : base(onChange)
    {
      Flex = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);

      Anchor = ViewAnchor.Center;
      Alignment = ViewAlignment.Center;

      Label = new Label(text, 0.6f, TextAlignment.CENTER);
      AddChild(Label);
    }

    protected override void ApplyThemeStyle()
    {
      base.ApplyThemeStyle();

      if (Disabled)
      {
        Label.TextColor = App.Theme.MainColor_4;
        return;
      }

      if (Handler.Mouse1.IsPressed)
        Label.TextColor = App.Theme.MainColor_4;
      else if (Handler.Mouse1.IsOver)
        Label.TextColor = App.Theme.WhiteColor;
      else
        Label.TextColor = App.Theme.WhiteColor;
    }

  }
}