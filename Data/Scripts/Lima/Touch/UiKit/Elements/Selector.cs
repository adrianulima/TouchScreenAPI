using System.Collections.Generic;
using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Touch.UiKit.Elements
{
  public class Selector : View
  {
    public int Selected = 0;
    public readonly List<string> Labels;
    public Action<int, string> OnChange;
    public bool Loop;

    public EmptyButton LeftButton;
    public EmptyButton RightButton;
    public EmptyElement LeftArrow;
    public EmptyElement RightArrow;
    public Label Label;

    public Selector(List<string> labels, Action<int, string> onChange, bool loop = true)
    {
      Labels = labels;
      OnChange = onChange;
      Loop = loop;

      Direction = ViewDirection.Row;
      Anchor = ViewAnchor.Center;
      Alignment = ViewAlignment.Center;

      Flex = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);

      LeftButton = new EmptyButton(Prev);
      LeftButton.Pixels = new Vector2(24, 0);
      LeftButton.Flex = new Vector2(0, 1);
      AddChild(LeftButton);
      LeftArrow = new EmptyElement();
      LeftButton.AddChild(LeftArrow);
      Label = new Label(Labels[Selected], 0.6f, TextAlignment.CENTER);
      AddChild(Label);
      RightButton = new EmptyButton(Next);
      RightButton.Pixels = new Vector2(24, 0);
      RightButton.Flex = new Vector2(0, 1);
      AddChild(RightButton);
      RightArrow = new EmptyElement();
      RightButton.AddChild(RightArrow);
    }

    private void Next()
    {
      UpdateSelected(1);
    }

    private void Prev()
    {
      UpdateSelected(-1);
    }

    private void UpdateSelected(int diff)
    {
      var prev = Selected;
      Selected += diff;

      var count = Labels.Count;
      if (Selected < 0)
        Selected = Loop ? count - 1 : 0;

      if (Selected >= count)
        Selected = Loop ? 0 : count - 1;

      if (prev != Selected)
      {
        OnChange(Selected, Labels[Selected]);
        Label.Text = Labels[Selected];
      }
    }

    public override void Update()
    {
      if (UseThemeColors)
        BgColor = App.Theme.MainColor_2;

      base.Update();

      var arrowSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "AH_BoreSight",
        RotationOrScale = 0,
        Color = App.Theme.WhiteColor
      };

      var leftSize = LeftArrow.GetSize();
      arrowSprite.RotationOrScale = MathHelper.Pi;
      arrowSprite.Position = LeftArrow.Position + Vector2.UnitY * leftSize.Y * 0.5f;
      arrowSprite.Size = leftSize;
      LeftArrow.GetSprites().Clear();
      LeftArrow.GetSprites().Add(arrowSprite);

      var rightSize = RightArrow.GetSize();
      arrowSprite.RotationOrScale = 0;
      arrowSprite.Position = RightArrow.Position + Vector2.UnitY * rightSize.Y * 0.5f;
      arrowSprite.Size = rightSize;
      RightArrow.GetSprites().Clear();
      RightArrow.GetSprites().Add(arrowSprite);
    }

  }
}