using System;
using System.Collections.Generic;
using System.Linq;
using Lima.Fancy;
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

namespace Lima.Touch
{
  public class SurfaceCoordsManager
  {
    public static SurfaceCoordsManager Instance;

    public readonly List<string> CoordsList = new List<string>();

    public SurfaceCoordsManager()
    {
      if (Instance != null)
        return;

      Instance = this;
    }

    public void Init()
    {
      MyAPIGateway.Utilities.MessageEntered += MessageHandler;
    }

    public void LoadData()
    {
      // // TODO: Load from SBCs or config file
      var coords = new List<string>(){
        "TOUCH:LargeLCDPanel:0:-1.23463:1.23463:-1.02366:-1.23463:-1.23463:-1.02366:1.23463:-1.23463:-1.02366",
        "TOUCH:LargeTextPanel:0:-1.19463:0.694398:-1.02366:-1.19463:-0.694398:-1.02366:1.19463:-0.694398:-1.02366",
        "TOUCH:LargeLCDPanelWide:0:-2.48463:1.23463:-1.02366:-2.48463:-1.23463:-1.02366:2.48463:-1.23463:-1.02366",
        "TOUCH:TransparentLCDLarge:0:-1.0842:1.08415:-1.1926:-1.0842:-1.08416:-1.20177:1.0842:-1.08415:-1.1926",

        "TOUCH:LargeBlockCockpitIndustrial:0:-0.918698:0.620028:-0.619588:-0.973858:0.433626:-0.73788:-0.630658:0.430848:-0.900063",
        "TOUCH:LargeBlockCockpitIndustrial:1:-0.462543:0.71061:-0.657349:-0.49153:0.543964:-0.770974:-0.210559:0.553486:-0.866144",
        "TOUCH:LargeBlockCockpitIndustrial:2:-0.1541:0.71061:-0.760206:-0.142477:0.543964:-0.876892:0.1541:0.553486:-0.870225",
        "TOUCH:LargeBlockCockpitIndustrial:3:0.181572:0.720131:-0.75252:0.210559:0.553486:-0.866144:0.49153:0.543964:-0.770974",
        "TOUCH:LargeBlockCockpitIndustrial:4:-0.123102:-0.3576:-0.497145:-0.123105:-0.422629:-0.384497:0.123105:-0.422629:-0.384498",

        "TOUCH:LargeBlockCockpitSeat:0:-0.20481:0.054932:-0.436759:-0.204811:-0.20229:-0.35828:0.204732:-0.20229:-0.35828",
        "TOUCH:LargeBlockCockpitSeat:1:-0.56155:0.014181:-0.341422:-0.56155:-0.209932:-0.273044:-0.280005:-0.22683:-0.32843",
        "TOUCH:LargeBlockCockpitSeat:2:0.278656:-0.001624:-0.397258:0.27865:-0.225961:-0.32875:0.56008:-0.209189:-0.273803",
        "TOUCH:LargeBlockCockpitSeat:3:-0.114655:-0.236712:-0.310878:-0.114655:-0.284872:-0.197601:0.114692:-0.284872:-0.197601",
        "TOUCH:LargeBlockCockpitSeat:4:-0.620698:-0.226691:-0.168774:-0.568499:-0.288196:-0.078508:-0.438854:-0.309467:-0.167972",
        "TOUCH:LargeBlockCockpitSeat:5:0.490973:-0.24806:-0.258088:0.438842:-0.309487:-0.167938:0.568511:-0.288212:-0.078457"
      };

      CoordsList.AddRange(coords);
    }

    public void UnloadData()
    {
      MyAPIGateway.Utilities.MessageEntered -= MessageHandler;
    }

    private void MessageHandler(string message, ref bool sendToOthers)
    {
      message = message.ToLower();

      if (message.StartsWith(SurfaceCoords.Prefix.ToLower()))
      {
        AddSurfaceCoords(message);

        sendToOthers = false;
      }
    }

    public void AddSurfaceCoords(string coords)
    {
      var parsedCoord = SurfaceCoords.Parse(coords);
      if (!parsedCoord.IsEmpty())
        CoordsList.Add(parsedCoord.ToString());
    }

  }
}