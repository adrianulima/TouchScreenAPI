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
        Color = App.Theme.MainColor_8
      };

      Sprites.Clear();

      var size = GetSize();
      _bgSprite.Position = Position + new Vector2(0, size.Y / 2);
      _bgSprite.Size = size;

      Sprites.Add(_bgSprite);
    }

  }
}