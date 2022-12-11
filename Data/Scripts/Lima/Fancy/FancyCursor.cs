using System.Collections.Generic;
using Lima.Touch;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy
{
  public class FancyCursor
  {
    protected readonly List<MySprite> sprites = new List<MySprite>();
    private TouchScreen _screen;

    private MySprite cursorSprite;

    public Vector2 Position { get { return _screen.CursorPos; } }
    public bool IsOnScreen { get { return _screen.IsOnScreen; } }

    public bool Active = true;

    public FancyCursor(TouchScreen screen)
    {
      _screen = screen;

      cursorSprite = new MySprite()
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
      sprites.Clear();
    }

    public List<MySprite> GetSprites()
    {
      sprites.Clear();

      if (!Active)
        return sprites;

      if (TouchSession.Instance.TouchMan.CurrentScreen != _screen)
        return sprites;

      cursorSprite.Position = new Vector2(Position.X, Position.Y + 8);
      sprites.Add(cursorSprite);

      return sprites;
    }

    public bool IsInsideArea(float x, float y, float z, float w)
    {
      if (!IsOnScreen || !Active)
        return false;

      return _screen.IsInsideArea(x, y, z, w);
    }
  }
}