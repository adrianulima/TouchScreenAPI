using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyButton : FancyEmptyButton
  {
    public FancyLabel Label;

    public FancyButton(string text, Action onChange) : base(onChange)
    {
      Scale = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);

      Anchor = ViewAnchor.Center;
      Alignment = ViewAlignment.Center;

      Label = new FancyLabel(text, 0.6f, TextAlignment.CENTER);
      AddChild(Label);
    }

    protected override void ApplyThemeStyle()
    {
      base.ApplyThemeStyle();

      if (Handler.IsMousePressed)
        Label.TextColor = App.Theme.MainColor_4;
      else if (Handler.IsMouseOver)
        Label.TextColor = App.Theme.WhiteColor;
      else
        Label.TextColor = App.Theme.WhiteColor;
    }

  }
}