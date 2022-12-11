using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyView : FancyElementContainerBase
  {
    public enum ViewDirection : int
    {
      None = 0,
      Row = 1,
      Column = 2
    }

    public ViewDirection Direction = ViewDirection.Column;

    protected MySprite BgSprite;
    public Color? BgColor;

    public FancyView(ViewDirection direction = ViewDirection.Column, Color? bgColor = null)
    {
      Direction = direction;
      BgColor = bgColor;
    }

    public override void Update()
    {
      base.Update();

      if (BgColor != null)
      {
        BgSprite = new MySprite()
        {
          Type = SpriteType.TEXTURE,
          Data = "SquareSimple",
          RotationOrScale = 0,
          Color = BgColor
        };
      }

      Sprites.Clear();

      if (BgColor != null)
      {
        BgSprite.Position = Position + new Vector2(0, Size.Y / 2);
        BgSprite.Size = Size;

        Sprites.Add(BgSprite);
      }

      var len = children.Count;
      for (int i = 0; i < len; i++)
      {
        if (i == 0)
        {
          children[0].Offset = Vector2.Zero;
          continue;
        }
        var lastChild = children[i - 1];
        var oX = Direction == ViewDirection.Row ? -Position.X + lastChild.Position.X + lastChild.Size.X + lastChild.Margin.Z : 0;
        var oY = Direction == ViewDirection.Column ? -Position.Y + lastChild.Position.Y + lastChild.Size.Y + lastChild.Margin.W : 0;
        children[i].Offset = new Vector2(oX, oY);
      }
    }
  }
}