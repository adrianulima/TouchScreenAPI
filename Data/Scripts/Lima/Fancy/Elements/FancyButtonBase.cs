using Sandbox.ModAPI;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyButtonBase : FancyElementBase
  {
    public ClickHandler handler = new ClickHandler();

    public FancyButtonBase() { }

    public override void Update()
    {
      base.Update();

      handler.UpdateStatus(App.Screen);
    }

  }
}