using System.Collections.Generic;
// using Lima.Utils;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public abstract class FancyElementBase
  {
    protected readonly List<MySprite> Sprites = new List<MySprite>();

    public Vector2 Offset = Vector2.Zero;

    private Vector2 _position = Vector2.Zero;
    public Vector2 Position
    {
      get
      {
        if (this.Parent != null)
          return this.Parent.Position + new Vector2(Margin.X, Margin.Y) + _position * ThemeScale;
        return (new Vector2(Margin.X, Margin.Y) + _position * ThemeScale);
      }
      set { _position = value; }
    }
    private Vector4 _margin = Vector4.Zero;
    public Vector4 Margin
    {
      get { return _margin * ThemeScale; }
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

    private Vector2 _size = Vector2.Zero;
    public Vector2 Size
    {
      get { return this.Parent != null ? new Vector2(Pixels.X - Margin.X - Margin.Z, Pixels.Y - Margin.Y - Margin.W) * ThemeScale + Parent.Size * Scale : Pixels; }
      protected set { _size = value; }
    }

    private FancyApp _app;
    public FancyApp App
    {
      get { return _app ?? Parent?.App; }
      protected set { _app = value; }
    }

    protected float ThemeScale
    {
      get { return App != null ? App.Theme.Scale : 1f; }
    }

    private FancyElementContainerBase _parent;
    public FancyElementContainerBase Parent
    {
      get { return _parent; }
      set
      {
        if (_parent == null || value == null)
          _parent = value;
      }
    }

    public FancyElementBase() { }

    // public bool DebugDraw = true;
    // public Color _debugDrawColor;
    // public Color DebugDrawColor
    // {
    //   get
    //   {
    //     if (_debugDrawColor == Color.Transparent)
    //       _debugDrawColor = new Color(MathUtils.GetRandomInt(30, 100), MathUtils.GetRandomInt(30, 80), MathUtils.GetRandomInt(20, 60), 200);
    //     return _debugDrawColor;
    //   }
    // }

    public virtual void InitElements() { }

    public virtual void Update() { }

    public virtual void Dispose()
    {
      Sprites.Clear();
    }

    public virtual List<MySprite> GetSprites()
    {
      // if (DebugDraw)
      // {
      //   Sprites.Insert(0, new MySprite()
      //   {
      //     Type = SpriteType.TEXTURE,
      //     Data = "SquareSimple",
      //     RotationOrScale = 0,
      //     Color = DebugDrawColor,
      //     Position = Position + new Vector2(0, Size.Y / 2),
      //     Size = Size
      //   });
      // }

      return Sprites;
    }

  }
}
