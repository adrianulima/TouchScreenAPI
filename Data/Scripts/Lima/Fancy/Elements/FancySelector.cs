using System;
using System.Collections.Generic;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancySelector : FancyButtonBase
  {
    private MySprite bgSprite;
    private MySprite arrowSprite;
    private MySprite arrowBgSprite;
    private MySprite arrow2Sprite;
    private MySprite arrow2BgSprite;
    private MySprite textSprite;

    public int Selected = 0;
    public List<string> Labels;
    protected Action<int, string> _action;
    protected bool _loop;

    public FancySelector(List<string> labels, Action<int, string> action, bool loop = true)
    {
      Labels = labels;
      _action = action;
      _loop = loop;
    }

    public override void Update()
    {
      hitArea = new Vector4(Position.X, Position.Y, Position.X + Size.X, Position.Y + Size.Y);

      base.Update();

      bgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_10
      };

      arrowBgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_30
      };

      arrow2BgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_30
      };

      arrowSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "AH_BoreSight",
        RotationOrScale = MathHelper.Pi,
        Color = App.Theme.White
      };

      arrow2Sprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "AH_BoreSight",
        RotationOrScale = 0,
        Color = App.Theme.White
      };

      textSprite = new MySprite()
      {
        Type = SpriteType.TEXT,
        Data = Labels[Selected],
        RotationOrScale = 0.6f * App.Theme.Scale,
        Color = App.Theme.White,//Theme.Main,
        Alignment = TextAlignment.CENTER,
        FontId = App.Theme.Font
      };

      if (IsMouseOver)
      {
        var mouseX = App.Cursor.Position.X - hitArea.X;
        if (IsMousePressed)
        {
          if (mouseX < Size.Y)
            arrowBgSprite.Color = App.Theme.Main_70;
          else if (mouseX > Size.X - Size.Y)
            arrow2BgSprite.Color = App.Theme.Main_70;
        }
        else
        {
          if (mouseX < Size.Y)
            arrowBgSprite.Color = App.Theme.Main_40;
          else if (mouseX > Size.X - Size.Y)
            arrow2BgSprite.Color = App.Theme.Main_40;
        }
      }
      else
      {

        arrowBgSprite.Color = App.Theme.Main_30;
        arrow2BgSprite.Color = App.Theme.Main_30;
      }

      if (JustReleased)
      {
        var mouseX = App.Cursor.Position.X - hitArea.X;
        var prev = Selected;
        if (mouseX < Size.Y)
          Selected -= 1;
        else if (mouseX > Size.X - Size.Y)
          Selected += 1;

        var count = Labels.Count;
        if (Selected < 0)
          Selected = _loop ? count - 1 : 0;

        if (Selected >= count)
          Selected = _loop ? 0 : count - 1;

        if (prev != Selected)
          _action(Selected, Labels[Selected]);
      }

      sprites.Clear();

      bgSprite.Position = Position + new Vector2(0, Size.Y / 2);
      bgSprite.Size = Size;

      arrowSprite.Position = arrowBgSprite.Position = Position + new Vector2(0, Size.Y / 2);
      arrowSprite.Size = arrowBgSprite.Size = new Vector2(Size.Y, Size.Y);

      arrow2Sprite.Position = arrow2BgSprite.Position = Position + new Vector2(Size.X - Size.Y, Size.Y / 2);
      arrow2Sprite.Size = arrow2BgSprite.Size = new Vector2(Size.Y, Size.Y);

      textSprite.Position = Position + new Vector2(Size.X / 2, Size.Y * 0.5f - Size.Y / 2.4f);
      // textSprite.Size = Size;

      sprites.Add(bgSprite);
      sprites.Add(arrowBgSprite);
      sprites.Add(arrow2BgSprite);
      sprites.Add(arrowSprite);
      sprites.Add(arrow2Sprite);
      sprites.Add(textSprite);
    }

  }
}