using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyButton : FancyView
  {
    public ClickHandler Handler = new ClickHandler();

    public FancyLabel Label;
    public Action OnChange;

    public FancyButton(string text, Action onChange)
    {
      OnChange = onChange;

      Scale = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);

      Anchor = ViewAlignment.Center;
      Alignment = ViewAlignment.Center;

      Label = new FancyLabel(text, 0.6f, TextAlignment.CENTER);
      AddChild(Label);
    }

    public override void Update()
    {
      var size = GetSize();

      Handler.HitArea = new Vector4(Position.X, Position.Y, Position.X + size.X, Position.Y + size.Y);
      Handler.UpdateStatus(App.Screen);

      if (Handler.JustReleased)
        OnChange();

      if (UseThemeColors)
        ApplyThemeStyle();

      base.Update();
    }

    private void ApplyThemeStyle()
    {
      if (Handler.IsMousePressed)
      {
        Label.TextColor = App.Theme.MainColor_4;
        BgColor = App.Theme.MainColor_8;
      }
      else if (Handler.IsMouseOver)
      {
        Label.TextColor = App.Theme.WhiteColor;
        BgColor = App.Theme.MainColor_5;
      }
      else
      {
        Label.TextColor = App.Theme.WhiteColor;
        BgColor = App.Theme.MainColor_4;
      }
    }

  }
}