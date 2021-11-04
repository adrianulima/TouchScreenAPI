using System;
using System.Collections.Generic;
using Sandbox.Game;
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
  public class FancyButton : FancyButtonBase
  {
    private MySprite bgSprite;
    private MySprite textSprite;

    public string Text;
    public Action _action;

    public FancyButton(string text, Action action)
    {
      Text = text;
      _action = action;
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
        Color = App.Theme.Main_30
      };

      textSprite = new MySprite()
      {
        Type = SpriteType.TEXT,
        Data = Text,
        RotationOrScale = 0.6f * App.Theme.Scale,
        Color = App.Theme.White,
        Alignment = TextAlignment.CENTER,
        FontId = App.Theme.Font
      };

      if (IsMousePressed)
      {
        textSprite.Color = App.Theme.Main_30;
        bgSprite.Color = App.Theme.Main_70;
      }
      else if (IsMouseOver)
      {
        textSprite.Color = App.Theme.White;
        bgSprite.Color = App.Theme.Main_40;
      }
      else
      {
        textSprite.Color = App.Theme.White;
        bgSprite.Color = App.Theme.Main_30;
      }

      if (JustReleased)
      {
        _action();
        // MyAPIGateway.Utilities.ShowNotification($"[ Button callback ]", 2000, MyFontEnum.Red);
      }

      sprites.Clear();

      bgSprite.Position = Position + new Vector2(0, Size.Y / 2);
      bgSprite.Size = Size;

      textSprite.Position = Position + new Vector2(Size.X / 2, Size.Y * 0.5f - (Size.Y / 2.4f));
      // textSprite.Size = Size;

      sprites.Add(bgSprite);
      sprites.Add(textSprite);
    }

  }
}