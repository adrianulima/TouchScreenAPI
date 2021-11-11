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
        "TOUCH:LargeBlockCockpitSeat:5:0.490973:-0.24806:-0.258088:0.438842:-0.309487:-0.167938:0.568511:-0.288212:-0.078457",


        "TOUCH:LargeSciFiButtonTerminal:0:0.293092:0.599074:1.11425:0.293092:0.224412:1.11425:-0.293092:0.224412:1.11425",

        "TOUCH:LargeSciFiButtonPanel:0:-0.672101:-0.169771:-1.03718:-0.672101:-0.281539:-0.922805:-0.407485:-0.281539:-0.922806",
        "TOUCH:LargeSciFiButtonPanel:1:-0.312649:-0.169771:-1.03718:-0.312648:-0.281539:-0.922805:-0.048033:-0.281539:-0.922806",
        "TOUCH:LargeSciFiButtonPanel:2:0.047825:-0.169771:-1.03718:0.047826:-0.281539:-0.922805:0.312441:-0.281539:-0.922806",
        "TOUCH:LargeSciFiButtonPanel:3:0.407537:-0.169771:-1.03718:0.407537:-0.281539:-0.922805:0.672153:-0.281539:-0.922806",

        "TOUCH:CockpitOpen:0:-0.70525:0.01592:-0.315901:-0.70525:-0.24127:-0.167418:0.70525:-0.24127:-0.167418",

        "TOUCH:LargeBlockCockpit:0:-0.683253:0.648219:-0.430285:-0.683253:-0.094249:-0.231341:0.683253:-0.094249:-0.231341",

        "TOUCH:OpenCockpitLarge:0:-0.32339:-0.388036:-0.868841:-0.32339:-0.527916:-0.544065:0.317703:-0.527916:-0.544065",
        "TOUCH:OpenCockpitLarge:1:-0.961732:-0.339453:-0.626006:-0.852037:-0.503519:-0.516081:-0.617797:-0.503381:-0.750337",
        "TOUCH:OpenCockpitLarge:2:0.632119:-0.346583:-0.770753:0.632119:-0.449903:-0.770753:0.736728:-0.449903:-0.666222",
        "TOUCH:OpenCockpitLarge:3:-1.01423:-0.352563:0.014583:-0.941375:-0.423826:0.014583:-0.941375:-0.423826:-0.06059",
        "TOUCH:OpenCockpitLarge:4:1.04306:-0.3606:-0.245201:0.922525:-0.481052:-0.245201:0.922525:-0.481052:0.010062",

        "TOUCH:LargeLCDPanel5x5:0:-6.23:6.23:-1.02037:-6.23:-6.23:-1.02037:6.23:-6.23:-1.02037",
        "TOUCH:LargeLCDPanel5x3:0:-6.23:3.73:-1.02037:-6.23:-3.73:-1.02037:6.23:-3.73:-1.02037",
        "TOUCH:LargeLCDPanel3x3:0:-3.73:3.73:-1.02037:-3.73:-3.73:-1.02037:3.73:-3.73:-1.02037",

        "TOUCH:LargeBlockCorner_LCD_1:0:0.957584:-0.992113:1.20251:0.957584:-1.20251:0.992113:-0.957585:-1.20251:0.992113",

        "TOUCH:LargeBlockCorner_LCD_2:0:-0.957585:-0.992113:1.02049:-0.957585:-1.20251:1.23089:0.957585:-1.20251:1.23089",

        "TOUCH:LargeBlockCorner_LCD_Flat_1:0:-0.957585:-1.17074:-1.21998:-0.957585:-1.17074:-0.923483:0.957585:-1.17074:-0.923484",

        "TOUCH:LargeBlockCorner_LCD_Flat_2:0:-0.957585:-1.17123:0.91751:-0.957585:-1.17123:1.21506:0.957586:-1.17123:1.21506",

        "TOUCH:LargeProgrammableBlock:0:-0.435951:0.48753:0.993358:-0.435951:-0.044765:0.997674:0.436368:-0.044765:0.997674",

        "TOUCH:LargeProgrammableBlock:1:-0.206985:-0.116908:1.0524:-0.206985:-0.23073:1.18466:0.235275:-0.23073:1.18466"



        // "TOUCH:LargeBlockConsole:0:-0.991465:-0.38838:-0.991465:-0.991465:-0.38838:0.991465:0.991465:-0.38838:-0.991465"

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