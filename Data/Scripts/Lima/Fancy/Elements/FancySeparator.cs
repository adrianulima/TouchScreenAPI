using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancySeparator : FancyElementBase
  {
    private MySprite _bgSprite;

    public FancySeparator()
    {
      Scale = new Vector2(1, 0);
      Margin = new Vector4(8, 2, 8, 0);
      Pixels = new Vector2(0, 2);
    }

    public override void Update()
    {
      base.Update();

      _bgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_70
      };

      sprites.Clear();

      _bgSprite.Position = Position + new Vector2(0, Size.Y / 2);
      _bgSprite.Size = Size;

      sprites.Add(_bgSprite);
    }

  }
}