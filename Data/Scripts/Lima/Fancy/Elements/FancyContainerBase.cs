using System.Collections.Generic;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public abstract class FancyContainerBase : FancyElementBase
  {
    public readonly List<FancyElementBase> Children = new List<FancyElementBase>();

    public FancyContainerBase() { }

    public virtual Vector2 GetFlexSize()
    {
      return GetSize();
    }

    public override void OnAddedToApp()
    {
      base.OnAddedToApp();
      foreach (var child in Children)
        child.OnAddedToApp();
    }

    public override void Update()
    {
      foreach (var child in Children)
      {
        if (child.Enabled)
          child.Update();
      }
      base.Update();
    }

    public override void Dispose()
    {
      base.Dispose();

      foreach (var child in Children)
        child.Dispose();

      Children.Clear();
    }

    public override List<MySprite> GetSprites()
    {
      base.GetSprites();

      foreach (var child in Children)
      {
        if (ValidateChild((child)))
          Sprites.AddRange(child.GetSprites());
      }

      return Sprites;
    }

    protected virtual bool ValidateChild(FancyElementBase child)
    {
      return child.Enabled;
    }

    public virtual FancyElementBase AddChild(FancyElementBase child)
    {
      return AddChild(child, -1);
    }

    public virtual FancyElementBase AddChild(FancyElementBase child, int index)
    {
      if (child.Parent == null && !Children.Contains(child))
      {
        if (index >= 0)
          Children.Insert(index, child);
        else
          Children.Add(child);

        child.Parent = this;
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

    public virtual FancyElementBase RemoveChild(int index)
    {
      if (Children.Count > index)
      {
        var child = Children[index];
        child.Parent = null;
        Children.RemoveAt(index);
        return child;
      }
      return null;
    }

    public virtual FancyElementBase MoveChild(FancyElementBase child, int index)
    {
      if (child.Parent == this && Children.Contains(child))
      {
        if (index >= 0)
        {
          Children.Remove(child);
          Children.Insert(index, child);
        }
        return child;
      }
      return null;
    }

  }
}
