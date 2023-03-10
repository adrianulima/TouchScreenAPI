using System;
using VRageMath;

namespace Lima.Touch.UiKit.Elements
{
  public class TouchSwitch : TouchView
  {
    public int Index;
    public TouchButton[] Buttons { get; private set; }
    public Action<int> OnChange;

    public TouchSwitch(string[] labels, int index = 0, Action<int> onChange = null)
    {
      Index = index;
      OnChange = onChange;

      Flex = new Vector2(1, 0);
      Pixels = new Vector2(0, 20);

      Direction = ViewDirection.Row;

      Buttons = new TouchButton[labels.Length];
      for (int i = 0; i < Buttons.Length; i++)
      {
        Buttons[i] = new TouchButton(labels[i], OnClickAction(i));
        Buttons[i].Pixels = Vector2.Zero;
        Buttons[i].Flex = Vector2.One;
        Buttons[i].Label.FontSize = 0.5f;
        Buttons[i].UseThemeColors = false;
        AddChild(Buttons[i]);
      }
    }

    private Action OnClickAction(int i)
    {
      return () =>
      {
        if (OnChange != null && Index != i)
          OnChange(Index = i);
      };
    }

    public override void Update()
    {
      if (UseThemeColors)
        ApplyThemeStyle();

      base.Update();
    }

    public override void Dispose()
    {
      base.Dispose();

      Buttons = null;
    }

    private void ApplyThemeStyle()
    {
      BgColor = App.Theme.MainColor_2;

      for (int i = 0; i < Buttons.Length; i++)
      {
        if (Index == i)
        {
          Buttons[i].Label.TextColor = App.Theme.WhiteColor;
          Buttons[i].BgColor = App.Theme.MainColor_5;
        }
        else
        {
          Buttons[i].Label.TextColor = App.Theme.MainColor_8;
          if (Buttons[i].Handler.IsMousePressed)
            Buttons[i].BgColor = App.Theme.MainColor_4;
          else if (Buttons[i].Handler.IsMouseOver)
            Buttons[i].BgColor = App.Theme.MainColor_5;
          else
            Buttons[i].BgColor = App.Theme.MainColor_2;
        }
      }
    }
  }
}