using System.Collections.Generic;
// using Lima.Utils;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public abstract class FancyElementBase
  {
    protected readonly List<MySprite> Sprites = new List<MySprite>();

    public bool Enabled = true;

    public Vector2 Position = Vector2.Zero;
    public Vector2 Pixels = Vector2.Zero;
    public Vector4 Margin = Vector4.Zero;

    private Vector2 _scale = Vector2.One;
    public Vector2 Scale
    {
      get { return _scale; }
      set { _scale = new Vector2(MathHelper.Clamp(value.X, 0, 1), MathHelper.Clamp(value.Y, 0, 1)); }
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

    public virtual Vector2 GetSize()
    {
      return this.Parent != null ? Pixels * ThemeScale + Parent.GetSize() * Scale : Pixels;
    }

    public virtual Vector2 GetBoundaries()
    {
      return GetSize();
    }

    public virtual void InitElements() { }

    public virtual void Update() { }

    public virtual void Dispose()
    {
      Sprites.Clear();
    }

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
      //     Position = Position + new Vector2(0, GetBoundaries().Y / 2),
      //     Size = GetBoundaries()
      //   });
      // }

      return Sprites;
    }

  }
}
