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

      Label = new FancyLabel(text, 0.5f, TextAlignment.LEFT);
      Label.Margin = new Vector4(4, 0, 4, 0);
      AddChild(Label);
    }

    public override void Update()
    {
      base.Update();
      BgColor = App.Theme.MainColor_2;
    }

  }
}