using System.Collections.Generic;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public abstract class FancyElementContainerBase : FancyElementBase
  {
    public readonly List<FancyElementBase> Children = new List<FancyElementBase>();

    public FancyElementContainerBase() { }

    public override Vector2 GetSize()
    {
      return base.GetSize() - new Vector2(Margin.X + Margin.Z, Margin.Y + Margin.W);
    }

    public override void InitElements()
    {
      base.InitElements();
      foreach (var child in Children)
        child.InitElements();
    }

    public override void Update()
    {
      base.Update();
      foreach (var child in Children)
      {
        if (child.Enabled)
          child.Update();
      }
    }

    public override void Dispose()
    {
      base.Dispose();

      foreach (var child in Children)
        child.Dispose();
    }

    public override List<MySprite> GetSprites()
    {
      base.GetSprites();

      foreach (FancyElementBase child in Children)
      {
        if (child.Enabled)
          Sprites.AddRange(child.GetSprites());
      }

      return Sprites;
    }

    public virtual FancyElementBase AddChild(FancyElementBase child)
    {
      if (child.Parent == null && !Children.Contains(child))
      {
        child.Parent = this;
        Children.Add(child);
        return child;
      }

      return null;
    }

    public virtual FancyElementBase RemoveChild(FancyElementBase child)
    {
      if (Children.Contains(child))
      {
        child.Parent = null;
        Children.Remove(child);
        return child;
      }

      return null;
    }

  }
}
