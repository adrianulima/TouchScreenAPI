using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyWindowBar : FancyView
  {
    public FancyLabel Label;

    public FancyWindowBar(string text)
    {
      Scale = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);

      Direction = ViewDirection.Row;
      Alignment = ViewAlignment.Center;

      Label = new FancyLabel(text, 0.5f, TextAlignment.LEFT);
      Label.Margin = new Vector4(4, 0, 4, 0);
      AddChild(Label);
    }

    public override void Update()
    {
      if (UseThemeColors)
        BgColor = App.Theme.MainColor_2;
      base.Update();
    }

  }
}