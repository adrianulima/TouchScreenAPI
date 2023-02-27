using System.Collections.Generic;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Touch.UiKit.Elements
{
  public abstract class TouchContainerBase : TouchElementBase
  {
    public readonly List<TouchElementBase> Children = new List<TouchElementBase>();

    public TouchContainerBase() { }

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
        if (child.IsValidated = ValidateChild((child)))
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
        if (child.IsValidated)
          Sprites.AddRange(child.GetSprites());
      }

      return Sprites;
    }

    protected virtual bool ValidateChild(TouchElementBase child)
    {
      return child.Enabled;
    }

    public virtual TouchElementBase AddChild(TouchElementBase child)
    {
      return AddChild(child, -1);
    }

    public virtual TouchElementBase AddChild(TouchElementBase child, int index)
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

    public virtual TouchElementBase RemoveChild(TouchElementBase child)
    {
      if (Children.Contains(child))
      {
        child.Parent = null;
        Children.Remove(child);
        return child;
      }
      return null;
    }

    public virtual TouchElementBase RemoveChild(int index)
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

    public virtual TouchElementBase MoveChild(TouchElementBase child, int index)
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
