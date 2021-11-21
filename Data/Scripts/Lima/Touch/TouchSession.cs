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
    public static bool ModEnabled = true;
    public static string ModVersion = "0.0.1";

    public static TouchSession Instance;

    public SurfaceCoordsManager SurfaceCoordsMan = new SurfaceCoordsManager();
    public TouchManager TouchMan = new TouchManager();

    public override void LoadData()
    {
      ModEnabled = !MyAPIGateway.Utilities.IsDedicated;
      if (!ModEnabled)
        return;

      Instance = this;
      SurfaceCoordsMan.LoadData();
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
      Instance = null;
    }

    public override void BeforeStart()
    {
      if (!ModEnabled)
        return;

      TouchMessages.SendApiToMods();
    }

    public override void UpdateAfterSimulation()
    {
      if (MyAPIGateway.Utilities?.IsDedicated == true)
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