using System;
using System.Collections.Generic;
using System.Linq;
using Lima.Utils;
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
  public abstract class FancyElementBase
  {
    protected readonly List<MySprite> sprites = new List<MySprite>();

    private Vector2 _position = Vector2.Zero;
    public Vector2 Position
    {
      get
      {
        if (this.Parent != null)
          return new Vector2(Margin.X + Offset.X, Margin.Y + Offset.Y) + this.Parent.Position + _position * ThemeScale;
        // TODO: + Margin + Offset ?
        return _position * _size;
      }
      set { _position = value; }
    }
    private Vector4 _margin = Vector4.Zero;
    public Vector4 Margin
    {
      get { return this.Parent != null ? this.Parent.Margin + _margin * ThemeScale : _margin * ThemeScale; }
      set { _margin = value; }
    }
    private Vector2 _scale = Vector2.One;
    public Vector2 Scale
    {
      get { return _scale; }
      set { _scale = new Vector2(MathHelper.Clamp(value.X, 0, 1), MathHelper.Clamp(value.Y, 0, 1)); }
    }

    private Vector2 _pixels = Vector2.Zero;
    public Vector2 Pixels
    {
      get { return _pixels; }
      set { _pixels = value; }
    }

    protected Vector2 _size = Vector2.Zero;
    public Vector2 Size
    {
      get { return this.Parent != null ? new Vector2(-Margin.X - Margin.Z + Pixels.X, Pixels.Y) + Parent.Size * Scale : _size; }
    }

    protected RectangleF _viewport;
    public RectangleF Viewport
    {
      get { return _viewport != null ? _viewport : Parent.Viewport; }
    }

    protected FancyApp _app;
    public FancyApp App
    {
      get { return _app ?? Parent?.App; }
    }

    protected float ThemeScale
    {
      get { return App != null ? App.Theme.Scale : 1f; }
    }

    public FancyElementContainerBase _parent;
    public FancyElementContainerBase Parent
    {
      get { return _parent; }
      set
      {
        if (_parent == null || value == null)
          _parent = value;
      }
    }

    public Vector2 Offset = Vector2.Zero;

    public FancyElementBase() { }

    public bool DebugDraw = false;
    public Color _debugDrawColor;
    public Color DebugDrawColor
    {
      get
      {
        if (_debugDrawColor == Color.Transparent)
          _debugDrawColor = new Color(MathUtils.GetRandomInt(30, 100), MathUtils.GetRandomInt(30, 80), MathUtils.GetRandomInt(20, 60), 200);
        return _debugDrawColor;
      }
    }

    public virtual void InitElements() { }

    public virtual void Update() { }

    public virtual void Dispose()
    {
      sprites.Clear();
    }

    public virtual List<MySprite> GetSprites()
    {
      if (DebugDraw)
      {
        sprites.Insert(0, new MySprite()
        {
          Type = SpriteType.TEXTURE,
          Data = "SquareSimple",
          RotationOrScale = 0,
          Color = DebugDrawColor,
          Position = Position + new Vector2(0, Size.Y / 2),
          Size = Size
        });
      }

      return sprites;
    }

  }
}
