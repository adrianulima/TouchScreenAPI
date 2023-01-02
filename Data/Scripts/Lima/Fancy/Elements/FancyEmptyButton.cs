using System;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyEmptyButton : FancyView
  {
    public ClickHandler Handler = new ClickHandler();

    public Action OnChange;

    public FancyEmptyButton(Action onChange)
    {
      OnChange = onChange;

      Scale = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);

      Anchor = ViewAnchor.Center;
      Alignment = ViewAlignment.Center;
    }

    public override void Update()
    {
      var size = GetBoundaries();

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
        BgColor = App.Theme.MainColor_8;
      else if (Handler.IsMouseOver)
        BgColor = App.Theme.MainColor_5;
      else
        BgColor = App.Theme.MainColor_4;
    }

  }
}