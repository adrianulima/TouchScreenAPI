using Sandbox.ModAPI;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyButtonBase : FancyElementBase
  {
    public ClickHandler Handler = new ClickHandler();

    public FancyButtonBase() { }

    public override void Update()
    {
      base.Update();

      Handler.UpdateStatus(App.Screen);
    }

  }
}