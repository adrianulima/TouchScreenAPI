using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyView : FancyContainerBase
  {
    public enum ViewDirection : byte
    {
      None = 0,
      Row = 1,
      Column = 2
    }
    public enum ViewAlignment : byte
    {
      Start = 0,
      Center = 1,
      End = 2
    }

    public ViewDirection Direction = ViewDirection.Column;
    public ViewAlignment Alignment = ViewAlignment.Start;
    public ViewAlignment Anchor = ViewAlignment.Start;

    protected MySprite BgSprite;
    protected MySprite[] BorderSprites = new MySprite[4];
    protected Vector2 ChildrenPixels = Vector2.Zero;
    protected Vector2 ChildrenScales = Vector2.One;

    public Color? BgColor;
    public Color? BorderColor;
    public Vector4 Border;
    public Vector4 Padding;
    public int Gap = 0;

    public FancyView(ViewDirection direction = ViewDirection.Column, Color? bgColor = null)
    {
      Direction = direction;
      BgColor = bgColor;
    }

    public override Vector2 GetSize()
    {
      return base.GetSize() - GetExtraBounds();
    }

    public override Vector2 GetFlexSize()
    {
      var scales = new Vector2(ChildrenScales.X < 1 ? 1 : ChildrenScales.X, ChildrenScales.Y < 1 ? 1 : ChildrenScales.Y);
      return (base.GetFlexSize() - ChildrenPixels * ThemeScale) * (1 / scales);
    }

    public override Vector2 GetBoundaries()
    {
      return base.GetBoundaries() + GetExtraBounds();
    }

    protected override bool ValidateChild(FancyElementBase child)
    {
      if (child.Absolute)
        return base.ValidateChild(child);

      if (Direction != ViewDirection.None && !child.Absolute)
      {
        var size = GetSize();
        var before = new Vector2(Border.X + Padding.X, Border.Y + Padding.Y) * ThemeScale;
        var childSize = child.GetSize();
        if (Direction == ViewDirection.Row)
        {
          if (child.Position.X + childSize.X > Position.X + size.X + before.X + 1)
            return false;
          if (child.Position.X < Position.X + before.Y - 1)
            return false;
        }
        else if (Direction == ViewDirection.Column)
        {
          if (child.Position.Y + childSize.Y > Position.Y + size.Y + before.Y + 1)
            return false;
          if (child.Position.Y < Position.Y + before.Y - 1)
            return false;
        }
      }

      return base.ValidateChild(child);
    }

    private void UpdateChildrenPixelsAndScales()
    {
      int childrenCount = Children.Count;
      if (Direction != ViewDirection.None && childrenCount > 0)
      {
        ChildrenPixels = Vector2.Zero;
        ChildrenScales = Vector2.Zero;

        foreach (var child in Children)
        {
          if (!child.Enabled || child.Absolute) continue;

          ChildrenPixels += new Vector2(child.Margin.X + child.Margin.Z, child.Margin.Y + child.Margin.W);

          if (Direction == ViewDirection.Row)
          {
            ChildrenPixels.X += child.Pixels.X;
            ChildrenScales.X += child.Scale.X;
          }
          else if (Direction == ViewDirection.Column)
          {
            ChildrenPixels.Y += child.Pixels.Y;
            ChildrenScales.Y += child.Scale.Y;
          }
        }

        if (Direction == ViewDirection.Row)
          ChildrenPixels.X += Gap * (childrenCount - 1);
        else if (Direction == ViewDirection.Column)
          ChildrenPixels.Y += Gap * (childrenCount - 1);
      }
    }

    protected virtual void UpdateChildrenPositions()
    {
      FancyElementBase prevChild = null;
      if (Direction != ViewDirection.None)
      {
        var isRow = Direction == ViewDirection.Row;
        var before = new Vector2(Border.X + Padding.X, Border.Y + Padding.Y) * ThemeScale;

        var anchorStart = Vector2.Zero;
        var remainingFlex = GetFlexSize() * new Vector2(1 - ChildrenScales.X, 1 - ChildrenScales.Y);
        if ((isRow && remainingFlex.X > 0) || (!isRow && remainingFlex.Y > 0))
        {
          if (Anchor == ViewAlignment.Center)
            anchorStart = isRow ? new Vector2(remainingFlex.X * 0.5f, 0) : new Vector2(0, remainingFlex.Y * 0.5f);
          else if (Anchor == ViewAlignment.End)
            anchorStart = isRow ? new Vector2(remainingFlex.X, 0) : new Vector2(0, remainingFlex.Y);
        }

        var size = GetSize();
        foreach (var child in Children)
        {
          if (!child.Enabled || child.Absolute) continue;

          var originPos = before + Position + new Vector2(child.Margin.X, child.Margin.Y) * ThemeScale;

          var align = Vector2.Zero;
          if (child.SelfAlignment == ViewAlignment.Center || (Alignment == ViewAlignment.Center && child.SelfAlignment == null))
            align = (size * 0.5f - child.GetBoundaries() * 0.5f) * (isRow ? Vector2.UnitY : Vector2.UnitX);
          else if (child.SelfAlignment == ViewAlignment.End || (Alignment == ViewAlignment.End && child.SelfAlignment == null))
            align = (size - child.GetBoundaries()) * (isRow ? Vector2.UnitY : Vector2.UnitX);

          if (prevChild == null)
          {
            child.Position = originPos + anchorStart + align;
            prevChild = child;
            continue;
          }

          var prevChildBounds = prevChild.GetBoundaries();
          var oX = isRow ? prevChild.Position.X + prevChildBounds.X - Position.X + (Gap + prevChild.Margin.Z - Border.X - Padding.X) * ThemeScale : 0;
          var oY = !isRow ? prevChild.Position.Y + prevChildBounds.Y - Position.Y + (Gap + prevChild.Margin.W - Border.Y - Padding.Y) * ThemeScale : 0;
          child.Position = originPos + new Vector2(oX, oY) + align;
          prevChild = child;
        }
      }
    }

    public override void Update()
    {
      UpdateChildrenPixelsAndScales();
      UpdateChildrenPositions();

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
        if (Border[s] > 0)
        {
          BorderSprites[s] = new MySprite()
          {
            Type = SpriteType.TEXTURE,
            Data = "SquareSimple",
            RotationOrScale = 0,
            Color = BorderColor ?? App.Theme.MainColor_7
          };
        }
      }

      Sprites.Clear();

      var size = GetSize();
      var extraBounds = GetExtraBounds();

      if (BgColor != null)
      {
        BgSprite.Position = Position + new Vector2(0, (size.Y + extraBounds.Y) / 2);
        BgSprite.Size = size + extraBounds;

        Sprites.Add(BgSprite);
      }

      if (Border.X > 0)
      {
        BorderSprites[0].Position = Position + new Vector2(0, (size.Y + extraBounds.Y) / 2);
        BorderSprites[0].Size = new Vector2(Border.X * ThemeScale, size.Y + extraBounds.Y);

        Sprites.Add(BorderSprites[0]);
      }
      if (Border.Y > 0)
      {
        BorderSprites[1].Position = Position + new Vector2(0, Border.Y / 2) * ThemeScale;
        BorderSprites[1].Size = new Vector2(size.X + extraBounds.X, Border.Y * ThemeScale);

        Sprites.Add(BorderSprites[1]);
      }
      if (Border.Z > 0)
      {
        BorderSprites[2].Position = Position + new Vector2(size.X + (Border.X + Padding.X + Padding.Z) * ThemeScale, (size.Y + extraBounds.Y) / 2);
        BorderSprites[2].Size = new Vector2(Border.Z * ThemeScale, (size.Y + extraBounds.Y));

        Sprites.Add(BorderSprites[2]);
      }
      if (Border.W > 0)
      {
        BorderSprites[3].Position = Position + new Vector2(0, size.Y + (Border.Y + Padding.Y + Padding.W + Border.W / 2) * ThemeScale);
        BorderSprites[3].Size = new Vector2(size.X + extraBounds.X, Border.W * ThemeScale);

        Sprites.Add(BorderSprites[3]);
      }
    }

    protected virtual Vector2 GetExtraBounds()
    {
      var borderSize = new Vector2(Border.X + Border.Z, Border.Y + Border.W) * ThemeScale;
      var paddingSize = new Vector2(Padding.X + Padding.Z, Padding.Y + Padding.W) * ThemeScale;
      return borderSize + paddingSize;
    }
  }
}