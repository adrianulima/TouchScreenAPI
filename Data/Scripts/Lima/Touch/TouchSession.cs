using Sandbox.ModAPI;
using System;
using VRage.Game.Components;
using VRage.Game;
using VRage.Utils;

namespace Lima.Touch
{
  [MySessionComponentDescriptor(MyUpdateOrder.Simulation)]
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
      TouchMan.Dispose();
      SurfaceCoordsMan.Dispose();
      _delegator.Unload();
      Instance = null;
      ModEnabled = false;
    }

    public override void Simulate()
    {
      base.Simulate();

      if (!ModEnabled)
        return;

      try
      {
        TouchMan.UpdateAfterSimulation();
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