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
    protected MySprite[] BorderSprites = new MySprite[4];

    public Color? BgColor;
    public Color? BorderColor;
    public Vector4 BorderWidth;

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


      for (int s = 0; s < BorderSprites.Length; s++)
      {
        // if (BorderWidth[s] > 0)
        // {
        BorderSprites[s] = new MySprite()
        {
          Type = SpriteType.TEXTURE,
          Data = "SquareSimple",
          RotationOrScale = 0,
          Color = BorderColor ?? App.Theme.Main_60
        };
        // }
      }

      Sprites.Clear();

      if (BgColor != null)
      {
        BgSprite.Position = Position + new Vector2(0, Size.Y / 2);
        BgSprite.Size = Size;

        Sprites.Add(BgSprite);
      }

      var sizeY = Size.Y - (Position.Y - App.Viewport.Y);
      if (BorderWidth.X > 0)
      {
        BorderSprites[0].Position = Position + new Vector2(0, sizeY / 2);
        BorderSprites[0].Size = new Vector2(BorderWidth.X, sizeY);

        Sprites.Add(BorderSprites[0]);
      }
      if (BorderWidth.Y > 0)
      {
        BorderSprites[1].Position = Position + new Vector2(0, BorderWidth.Y / 2);
        BorderSprites[1].Size = new Vector2(Size.X, BorderWidth.Y);

        Sprites.Add(BorderSprites[1]);
      }
      if (BorderWidth.Z > 0)
      {
        BorderSprites[2].Position = Position + new Vector2(Size.X - BorderWidth.Z, sizeY / 2);
        BorderSprites[2].Size = new Vector2(BorderWidth.Z, sizeY);

        Sprites.Add(BorderSprites[2]);
      }
      if (BorderWidth.W > 0)
      {
        BorderSprites[3].Position = Position + new Vector2(0, sizeY - BorderWidth.W / 2);
        BorderSprites[3].Size = new Vector2(Size.X, BorderWidth.W);

        Sprites.Add(BorderSprites[3]);
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