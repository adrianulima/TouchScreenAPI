using System;
using System.Collections.Generic;
using Lima.Utils;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Touch.UiKit.Elements
{
  public abstract class TouchElementBase
  {
    public event Action UpdateEvent;

    protected readonly List<MySprite> Sprites = new List<MySprite>();

    public bool Enabled = true;
    public bool Absolute = false;

    public ViewAlignment? SelfAlignment;

    public Vector2 Position = Vector2.Zero;
    public Vector2 Pixels = Vector2.Zero;
    public Vector4 Margin = Vector4.Zero;
    public Vector2 Scale = Vector2.One;

    private TouchApp _app;
    public TouchApp App
    {
      get { return _app ?? Parent?.App; }
      protected set { _app = value; }
    }

    protected float ThemeScale
    {
      get { return App != null ? App.Theme.Scale : 1f; }
    }

    private TouchContainerBase _parent;
    public TouchContainerBase Parent
    {
      get { return _parent; }
      set
      {
        if (_parent == null || value == null)
        {
          _parent = value;
          if (_parent?.App != null) OnAddedToApp();
        }
      }
    }

    public TouchElementBase() { }

    public virtual Vector2 GetSize()
    {
      return this.Parent != null ? Pixels * ThemeScale + Parent.GetFlexSize() * Scale : Pixels;
    }

    public virtual Vector2 GetBoundaries()
    {
      return GetSize();
    }

    public virtual void Update()
    {
      UpdateEvent?.Invoke();
    }

    public virtual void Dispose()
    {
      Sprites.Clear();
      UpdateEvent = null;
    }

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

    public virtual List<MySprite> GetSprites()
    {
      if (DebugDraw)
      {
        Sprites.Insert(0, new MySprite()
        {
          Type = SpriteType.TEXTURE,
          Data = "SquareSimple",
          RotationOrScale = 0,
          Color = DebugDrawColor,
          Position = Position + new Vector2(0, GetBoundaries().Y / 2),
          Size = GetBoundaries()
        });
      }

      return Sprites;
    }

    public virtual void OnAddedToApp() { }
  }
}
