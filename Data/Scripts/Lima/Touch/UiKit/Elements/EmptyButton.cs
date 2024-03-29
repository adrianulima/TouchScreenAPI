using System;
using VRageMath;

namespace Lima.Touch.UiKit.Elements
{
  public class EmptyButton : View
  {
    public ClickHandler Handler = new ClickHandler();
    public Action OnChange;
    public bool Disabled = false;

    public EmptyButton(Action onChange)
    {
      OnChange = onChange;

      Flex = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);

      Anchor = ViewAnchor.Center;
      Alignment = ViewAlignment.Center;
    }

    public override void Update()
    {
      if (!Disabled)
      {
        var size = GetBoundaries();
        Handler.HitArea = new Vector4(Position.X, Position.Y, Position.X + size.X, Position.Y + size.Y);
        Handler.Update(App.Screen);

        if (Handler.Mouse1.JustReleased && OnChange != null)
          OnChange();
      }

      if (UseThemeColors)
        ApplyThemeStyle();

      base.Update();
    }

    protected virtual void ApplyThemeStyle()
    {
      if (Disabled)
      {
        BgColor = App.Theme.MainColor_3;
        return;
      }

      if (Handler.Mouse1.IsPressed)
        BgColor = App.Theme.MainColor_8;
      else if (Handler.Mouse1.IsOver)
        BgColor = App.Theme.MainColor_5;
      else
        BgColor = App.Theme.MainColor_4;
    }

  }
}