using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI.Ingame;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public abstract class FancyElementContainerBase : FancyElementBase
  {
    public readonly List<FancyElementBase> children = new List<FancyElementBase>();

    public FancyElementContainerBase() { }

    public override void InitElements()
    {
      base.InitElements();
      foreach (var child in children)
        child.InitElements();
    }

    public override void Update()
    {
      base.Update();
      foreach (var child in children)
        child.Update();
    }

    public override void Dispose()
    {
      base.Dispose();

      foreach (var child in children)
        child.Dispose();
    }

    public override List<MySprite> GetSprites()
    {
      base.GetSprites();

      foreach (FancyElementBase child in children)
      {
        sprites.AddRange(child.GetSprites());
      }

      return sprites;
    }

    public virtual FancyElementBase AddChild(FancyElementBase child)
    {
      if (child.Parent == null && !children.Contains(child))
      {
        child.Parent = this;
        children.Add(child);
        return child;
      }

      return null;
    }

    public virtual FancyElementBase RemoveChild(FancyElementBase child)
    {
      if (children.Contains(child))
      {
        child.Parent = null;
        children.Remove(child);
        return child;
      }

      return null;
    }

  }
}
