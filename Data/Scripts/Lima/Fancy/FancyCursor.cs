using System;
using System.Collections.Generic;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Lima.Fancy
{
  public class FancyCursor
  {
    protected readonly List<MySprite> sprites = new List<MySprite>();
    private MyCubeBlock _block;
    private RectangleF _viewport;
    private FancyTheme Theme;

    private MySprite cursorSprite;

    private const int PlayersUpdateInterval = 60 * 15; // 15 seconds
    private int PlayersUpdateTick = PlayersUpdateInterval + 1;
    public readonly List<IMyPlayer> Players = new List<IMyPlayer>();

    public Vector2 Position { get; private set; }
    public bool IsOnScreen { get; private set; }

    private int _rotate = -1;
    public int ScreenRotate
    {
      get
      {
        // TODO: Make this call happen again with some refresh command
        if (_rotate < 0)
        {
          var builder = (_block.GetObjectBuilderCubeBlock() as Sandbox.Common.ObjectBuilders.MyObjectBuilder_TextPanel);
          if (builder != null)
            _rotate = (int)builder.SelectedRotationIndex;
          else
            _rotate = 0;
        }
        return _rotate;
      }
    }

    public FancyCursor(MyCubeBlock block, RectangleF viewport, FancyTheme theme)
    {
      _block = block;
      _viewport = viewport;
      Theme = theme;

      Position = Vector2.Zero;
      cursorSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "Textures\\FactionLogo\\Builders\\BuilderIcon_6.dds",
        Position = Vector2.Zero,
        RotationOrScale = -0.55f,
        Color = Color.White,
        Size = new Vector2(16, 26)
      };

      LoadData();
    }

    public void Dispose()
    {
      sprites.Clear();
      UnloadData();
    }

    protected void LoadData()
    {
      MyVisualScriptLogicProvider.PlayerConnected += PlayersChanged;
      MyVisualScriptLogicProvider.PlayerDisconnected += PlayersChanged;
    }

    protected void UnloadData()
    {
      MyVisualScriptLogicProvider.PlayerConnected -= PlayersChanged;
      MyVisualScriptLogicProvider.PlayerDisconnected -= PlayersChanged;
    }

    void PlayersChanged(long playerId)
    {
      PlayersUpdateTick = PlayersUpdateInterval;
    }

    public void Update()
    {
      PlayersUpdateTick += 10; // ScriptUpdate.Update10
      if (PlayersUpdateTick > PlayersUpdateInterval)
      {
        PlayersUpdateTick = 0;
        Players.Clear();
        MyAPIGateway.Players.GetPlayers(Players);
      }
    }

    public List<MySprite> GetSprites()
    {
      sprites.Clear();

      Update();
      IsOnScreen = false;

      // Hide LCD cursor if Game cursor is visible
      if (MyAPIGateway.Gui.IsCursorVisible)
        return sprites;

      var blockNormal = _block.WorldMatrix.Backward;
      var blockPos = _block.WorldMatrix.Translation - _block.WorldMatrix.Backward;

      foreach (IMyPlayer player in Players)
      {
        var interactiveDist = 15;//(player.Character as VRage.Game.Entity.UseObject.IMyUseObject)?.InteractiveDistance ?? 5;

        if (player.Controller.ControlledEntity == null)
          continue;

        var head = player.Controller.ControlledEntity.GetHeadMatrix(true);

        var headDir = head.Forward;
        var headPos = head.Translation;

        var headDist = Vector3.Distance(blockPos, headPos);
        if (headDist >= interactiveDist)
          continue;

        Vector3D intersection = FancyUtils.GetLinePlaneIntersection(blockPos, blockNormal, headPos, headDir);
        if (intersection == Vector3D.Zero)
          continue;

        var blockBox = _block.GetGeometryLocalBox();
        Vector2 screenCoord = FancyUtils.GetPointOnPlane(intersection, blockPos, _block.WorldMatrix.Up, _block.WorldMatrix.Left);

        if ((screenCoord.X < blockBox.Max.X && screenCoord.X > blockBox.Min.X) &&
            (screenCoord.Y < blockBox.Max.Y && screenCoord.Y > blockBox.Min.Y))
        {
          var rotatedCoord = RotateScreenCoord(new Vector2(((screenCoord.X / blockBox.Max.X) + 1) * 0.5f, ((screenCoord.Y / blockBox.Max.Y) + 1) * 0.5f), ScreenRotate);
          var pX = rotatedCoord.X * _viewport.Width + _viewport.X;
          var pY = rotatedCoord.Y * _viewport.Height + _viewport.Y;
          IsOnScreen = true;

          Position = new Vector2(pX, pY);
          cursorSprite.Position = new Vector2(pX, pY + 8);
          sprites.Add(cursorSprite);

          // FancyUtils.Log($">> {MyAPIGateway.Input?.TextInput.Count}");
        }
      }

      return sprites;
    }

    private Vector2 RotateScreenCoord(Vector2 coord, int rotate)
    {
      if (rotate == 0)
      {
        return coord;
      }
      else if (rotate == 1)
      {
        return new Vector2(coord.Y, 1 - coord.X);
      }
      else if (rotate == 2)
      {
        return new Vector2(1 - coord.X, 1 - coord.Y);
      }
      else
      {
        return new Vector2(1 - coord.Y, coord.X);
      }
    }

    public bool IsInsideArea(float x, float y, float z, float w)
    {
      if (!IsOnScreen)
        return false;

      return Position.X >= x && Position.Y >= y && Position.X <= z && Position.Y <= w;
    }
  }
}