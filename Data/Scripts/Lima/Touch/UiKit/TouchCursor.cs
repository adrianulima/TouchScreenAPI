using System.Collections.Generic;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Touch.UiKit
{
  public class TouchCursor
  {
    protected readonly List<MySprite> Sprites = new List<MySprite>();
    private TouchScreen _screen;

    private MySprite _cursorSprite;

    public Vector2 Position { get { return _screen.CursorPosition; } }
    public bool IsOnScreen { get { return _screen.IsOnScreen; } }

    public bool Active = true;
    public float Scale = 1f;

    public TouchCursor(TouchScreen screen)
    {
      _screen = screen;

      _cursorSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "Textures\\FactionLogo\\Builders\\BuilderIcon_6.dds",
        Position = Vector2.Zero,
        RotationOrScale = -0.55f,
        Color = Color.White,
        Size = new Vector2(16, 26)
      };
    }

    public void Dispose()
    {
      Sprites.Clear();
    }

    public List<MySprite> GetSprites()
    {
      Sprites.Clear();

      if (!Active)
        return Sprites;

      if (TouchSession.Instance.TouchMan.CurrentScreen != _screen)
        return Sprites;

      _cursorSprite.Size = new Vector2(16, 26) * Scale;
      _cursorSprite.Position = new Vector2(Position.X - 3 * Scale, Position.Y + 8 * Scale);
      Sprites.Add(_cursorSprite);

      return Sprites;
    }

    public bool IsInsideArea(float x, float y, float z, float w)
    {
      if (!IsOnScreen || !Active)
        return false;

      return _screen.IsInsideArea(x, y, z, w);
    }
  }
}