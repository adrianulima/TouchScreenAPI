using System;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;

namespace Lima.Touch
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  public class TouchSession : MySessionComponentBase
  {
    public static TouchSession Instance;
    public bool ModEnabled = true;

    public SurfaceCoordsManager SurfaceCoordsMan = new SurfaceCoordsManager();
    public TouchManager TouchMan = new TouchManager();

    private TouchDelegates _delegator;

    public override void LoadData()
    {
      ModEnabled = !MyAPIGateway.Utilities.IsDedicated;
      if (!ModEnabled)
        return;

      Instance = this;
      _delegator = new TouchDelegates();
      SurfaceCoordsMan.LoadData();
      _delegator.Load();
    }

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      if (!ModEnabled)
        return;

      SurfaceCoordsMan.Init();
    }

    protected override void UnloadData()
    {
      SurfaceCoordsMan.UnloadData();
      _delegator.Unload();
      Instance = null;
    }

    public override void UpdateAfterSimulation()
    {
      if (!ModEnabled)
        return;

      try
      {
        TouchMan.Update();
      }
      catch (Exception e)
      {
        MyLog.Default.WriteLineAndConsole($"{e.Message}\n{e.StackTrace}");

        if (MyAPIGateway.Session?.Player != null)
          MyAPIGateway.Utilities.ShowNotification($"[ ERROR: {GetType().FullName}: {e.Message} ]", 2000, MyFontEnum.Red);
      }
    }
  }
}