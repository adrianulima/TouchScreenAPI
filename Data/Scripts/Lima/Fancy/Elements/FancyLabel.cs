using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyLabel : FancyElementBase
  {
    protected MySprite textSprite;
    public string Text;
    public float FontSize;

    public FancyLabel(string text, float fontSize = 0.5f)
    {
      Text = text;
      FontSize = fontSize;
    }

    public override void Update()
    {
      base.Update();

      textSprite = new MySprite()
      {
        Type = SpriteType.TEXT,
        Data = Text,
        RotationOrScale = FontSize * App.Theme.Scale,
        Color = App.Theme.White,//Theme.Main,
        Alignment = TextAlignment.CENTER,
        FontId = App.Theme.Font
      };

      textSprite.Position = Position + new Vector2(Size.X / 2, 0);

      sprites.Clear();

      sprites.Add(textSprite);
    }

  }
}