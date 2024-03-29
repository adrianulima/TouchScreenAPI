using Sandbox.ModAPI;
using System.Collections.Generic;

namespace Lima.Touch
{
  public class SurfaceCoordsFile
  {
    public List<string> Coords = new List<string>();
    public SurfaceCoordsFile(List<string> coords) { Coords.AddRange(coords); }
    public SurfaceCoordsFile() { }
  }

  public class SurfaceCoordsManager
  {
    private readonly List<string> _coordsList = new List<string>();
    private readonly List<string> _customCoordsList = new List<string>();

    private readonly FileStorage<SurfaceCoordsFile> _fileHandler = new FileStorage<SurfaceCoordsFile>("SurfaceCoords.xml");

    public void Init()
    {
      MyAPIGateway.Utilities.MessageEntered += MessageHandler;
    }

    public void LoadData()
    {
      var coords = new List<string>()
      {
        "TOUCH:LargeLCDPanel:0:-1.23463:1.23463:-1.02366:-1.23463:-1.23463:-1.02366:1.23463:-1.23463:-1.02366",
        "TOUCH:LargeTextPanel:0:-1.19463:0.694398:-1.02366:-1.19463:-0.694398:-1.02366:1.19463:-0.694398:-1.02366",
        "TOUCH:LargeLCDPanelWide:0:-2.48463:1.23463:-1.02366:-2.48463:-1.23463:-1.02366:2.48463:-1.23463:-1.02366",
        "TOUCH:TransparentLCDLarge:0:-1.0842:1.08415:-1.1926:-1.0842:-1.08416:-1.20177:1.0842:-1.08415:-1.1926",
        "TOUCH:LargeLCDPanel5x5:0:-6.23:6.23:-1.02037:-6.23:-6.23:-1.02037:6.23:-6.23:-1.02037",
        "TOUCH:LargeLCDPanel5x3:0:-6.23:3.73:-1.02037:-6.23:-3.73:-1.02037:6.23:-3.73:-1.02037",
        "TOUCH:LargeLCDPanel3x3:0:-3.73:3.73:-1.02037:-3.73:-3.73:-1.02037:3.73:-3.73:-1.02037",
        "TOUCH:LargeBlockCorner_LCD_1:0:0.957584:-0.992113:1.20251:0.957584:-1.20251:0.992113:-0.957585:-1.20251:0.992113",
        "TOUCH:LargeBlockCorner_LCD_2:0:-0.957585:-0.992113:1.02049:-0.957585:-1.20251:1.23089:0.957585:-1.20251:1.23089",
        "TOUCH:LargeBlockCorner_LCD_Flat_1:0:-0.957585:-1.17074:-1.21998:-0.957585:-1.17074:-0.923483:0.957585:-1.17074:-0.923484",
        "TOUCH:LargeBlockCorner_LCD_Flat_2:0:-0.957585:-1.17123:0.91751:-0.957585:-1.17123:1.21506:0.957586:-1.17123:1.21506",
        "TOUCH:LargeProgrammableBlock:0:-0.435951:0.48753:0.993358:-0.435951:-0.044765:0.997674:0.436368:-0.044765:0.997674",
        "TOUCH:LargeProgrammableBlock:1:-0.206985:-0.116908:1.0524:-0.206985:-0.23073:1.18466:0.235275:-0.23073:1.18466",
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
        // "TOUCH:LargeBlockConsole:0:-0.991465:-0.38838:-0.991465:-0.991465:-0.38838:0.991465:0.991465:-0.38838:-0.991465",
        "TOUCH:SmallLCDPanel:0:-0.741776:0.741776:-0.110717:-0.741776:-0.741776:-0.110717:0.741776:-0.741776:-0.110717",
        "TOUCH:SmallLCDPanelWide:0:-1.49217:0.742556:-0.110685:-1.49217:-0.742556:-0.110685:1.49245:-0.742556:-0.110685",
        "TOUCH:TransparentLCDSmall:0:-0.21684:0.215277:-0.24354:-0.21684:-0.218385:-0.24354:0.21684:-0.218385:-0.24354",
        "TOUCH:SmallTextPanel:0:-0.215663:0.21561:-0.160736:-0.215663:-0.21561:-0.160736:0.215663:-0.21561:-0.160736",
        "TOUCH:SmallBlockCorner_LCD_1:0:0.201149:-0.138066:0.229387:0.201149:-0.229387:0.138066:-0.201149:-0.229387:0.138066",
        "TOUCH:SmallBlockCorner_LCD_2:0:-0.201149:-0.138066:0.150701:-0.201149:-0.229387:0.242023:0.201149:-0.229387:0.242023",
        "TOUCH:SmallBlockCorner_LCD_Flat_1:0:-0.197906:-0.210322:-0.221506:-0.197906:-0.210322:-0.060492:0.197905:-0.210322:-0.060492",
        "TOUCH:SmallBlockCorner_LCD_Flat_2:0:0.197707:-0.210349:-0.060558:0.197708:-0.210349:-0.221411:-0.197709:-0.210349:-0.221412",
        "TOUCH:SmallProgrammableBlock:0:-0.192459:0.227048:-0.025001:-0.192459:-0.125537:0.037169:0.192471:-0.125537:0.037169",
        "TOUCH:SmallProgrammableBlock:1:-0.188676:-0.17532:0.077997:-0.18931:-0.209441:0.20358:0.189101:-0.209442:0.203582",
        "TOUCH:SmallBlockCockpit:0:-0.114976:0.036534:-0.275231:-0.114976:-0.166655:-0.213239:0.114768:-0.166655:-0.213239",
        "TOUCH:SmallBlockCockpit:1:-0.330622:-0.007521:-0.215116:-0.330621:-0.123458:-0.179744:-0.174889:-0.132765:-0.210248",
        "TOUCH:SmallBlockCockpit:2:0.174837:-0.016915:-0.245605:0.174838:-0.132867:-0.210228:0.330761:-0.123549:-0.179687",
        "TOUCH:SmallBlockCockpit:3:-0.11472:-0.202878:-0.17911:-0.114723:-0.251076:-0.065746:0.114751:-0.251076:-0.065746",
        "TOUCH:SmallBlockCockpitIndustrial:0:-0.462543:0.74513:-0.596508:-0.500172:0.568964:-0.712422:-0.210559:0.568964:-0.809882",
        "TOUCH:SmallBlockCockpitIndustrial:1:-0.1541:0.74513:-0.694986:-0.1541:0.568964:-0.81834:0.1541:0.568964:-0.818339",
        "TOUCH:SmallBlockCockpitIndustrial:2:0.17293:0.74513:-0.693967:0.210559:0.568964:-0.809881:0.500172:0.568964:-0.712421",
        "TOUCH:SmallBlockCockpitIndustrial:3:-0.123102:-0.116188:-0.282139:-0.123105:-0.181217:-0.169491:0.123105:-0.181217:-0.169491",
        "TOUCH:DBSmallBlockFighterCockpit:0:-0.286403:0.058772:-0.746375:-0.286403:-0.249135:-0.661003:0.285588:-0.249135:-0.661003",
        "TOUCH:DBSmallBlockFighterCockpit:1:-0.211372:-0.267189:-0.410418:-0.21138:-0.362092:-0.332993:-0.023276:-0.362089:-0.332974",
        "TOUCH:DBSmallBlockFighterCockpit:2:0.022894:-0.267189:-0.410418:0.022886:-0.362092:-0.332993:0.21099:-0.362089:-0.332974",
        "TOUCH:DBSmallBlockFighterCockpit:3:-0.212735:-0.416471:-0.222682:-0.212735:-0.462862:-0.040841:0.212735:-0.462862:-0.040841",
        "TOUCH:DBSmallBlockFighterCockpit:4:-0.058204:-0.474206:-0.010117:-0.044999:-0.547803:0.142677:0.044999:-0.547803:0.142677",
        "TOUCH:DBSmallBlockFighterCockpit:5:0.425303:-0.361203:0.080077:0.311416:-0.44669:0.081541:0.318325:-0.453803:0.186467",
        "TOUCH:OpenCockpitSmall:0:-0.180041:0.146844:-0.053429:-0.180049:0.02569:0.108006:0.181396:0.025686:0.108012",
        "TOUCH:RoverCockpit:0:-0.392778:-0.046541:-0.293668:-0.392662:-0.189838:-0.248914:-0.155318:-0.189838:-0.309159",
        "TOUCH:RoverCockpit:1:-0.136667:-0.046311:-0.362766:-0.136667:-0.159087:-0.324238:0.136667:-0.159087:-0.324238",
        "TOUCH:RoverCockpit:2:0.155318:-0.045813:-0.364009:0.155318:-0.189838:-0.309159:0.392662:-0.189838:-0.248914",
        "TOUCH:RoverCockpit:3:-0.052947:-0.08278:-0.135536:-0.052947:-0.138511:-0.104184:0.052947:-0.138511:-0.104184",
        "TOUCH:RoverCockpit:4:0.20219:-0.228944:-0.228622:0.195939:-0.279159:-0.211448:0.267393:-0.279159:-0.185441",
        "TOUCH:BuggyCockpit:0:-0.14081:0.18805:-0.369332:-0.14434:0.034122:-0.310549:0.14647:0.035292:-0.310549",
        "TOUCH:BuggyCockpit:1:-0.396802:0.164742:-0.21128:-0.369163:0.03563:-0.169657:-0.183721:0.03563:-0.292798",
        "TOUCH:BuggyCockpit:2:0.21136:0.164742:-0.334421:0.183721:0.03563:-0.292798:0.369163:0.03563:-0.169657",
        "TOUCH:LargeTurretControlBlock:0:-0.593234:0.900054:0.114358:-0.593234:0.325494:-0.072266:0.385896:0.325494:-0.072266",
        "TOUCH:LargeTurretControlBlock:1:0.483036:0.900054:0.114358:0.483036:0.576696:-0.036392:0.704897:0.576696:-0.036392",
        "TOUCH:LargeTurretControlBlock:2:-0.520536:-0.199998:0.073605:-0.520536:-0.405104:0.278711:-0.34654:-0.405104:0.278711",
        "TOUCH:LargeTurretControlBlock:3:0.34654:-0.199998:0.073605:0.34654:-0.405104:0.278711:0.520536:-0.405104:0.278711",
        "TOUCH:SmallTurretControlBlock:0:-0.143935:0.235472:-0.306565:-0.143935:0.235472:-0.091221:0.144789:0.235472:-0.091221",
        "TOUCH:LargeBlockStandingCockpit:0:-0.372228:-0.08327:-1.12346:-0.372228:-0.156863:-1.06368:-0.111456:-0.156863:-1.06368",
        "TOUCH:LargeBlockStandingCockpit:1:-0.434899:-0.205596:-0.989841:-0.434899:-0.251315:-0.819213:-0.014451:-0.251315:-0.819213",
        "TOUCH:EmotionControllerLarge:0:1.22559:1.13086:1.13086:1.22559:-1.13086:1.13086:1.22559:-1.13086:-1.13086",
        "TOUCH:EmotionControllerLarge:1:-0.09949:0.52588:0.72754:-0.09949:0.00616:0.8667:0.4939:0.00616:0.8667",
        "TOUCH:EmotionControllerLarge:2:-0.08331:-0.12225:1:-0.08331:-0.26782:1.14453:0.47827:-0.26782:1.14453",
        "TOUCH:LargeProgrammableBlockReskin:0:0.29712:0.86475:-0.55811:0.29712:0.4021:-0.55859:0.55859:0.4021:-0.29712",
        "TOUCH:LargeProgrammableBlockReskin:1:0.25024:-0.05444:-0.44922:0.17676:-0.17944:-0.37573:0.37378:-0.18054:-0.17761",
        "TOUCH:LargeFullBlockLCDPanel:0:1.25:1.19434:1.25:1.25:-1.19434:1.25:1.25:-1.19434:-1.25",
        "TOUCH:LargeDiagonalLCDPanel:0:1.25:1.19434:1.25:1.25:-1.19434:1.25:-1.25:-1.19434:-1.25",
        "TOUCH:LargeCurvedLCDPanel:0:1.22852:1.19434:0.92383:1.22852:-1.19434:0.92383:-0.92383:-1.19434:-1.22852",
        "TOUCH:LargeBlockInsetEntertainmentCorner:0:0.84619:0.72314:-0.99854:0.84619:-0.25879:-0.99854:-0.84277:-0.25879:-0.99854",
        "TOUCH:LargeBlockInsetButtonPanel:0:1.08106:0.56592:0.69238:1.08106:-0.23901:0.69238:1.08106:-0.23901:-0.69238",
        "TOUCH:LargeBlockInsetButtonPanel:1:0.77637:0.33179:-0.93213:0.77637:-0.10138:-0.93213:0.03107:-0.10138:-0.93213",
        "TOUCH:LargeBlockInsetButtonPanel:2:-0.03345:0.33179:-0.93213:-0.03345:-0.10138:-0.93213:-0.77881:-0.10138:-0.93213",
        "TOUCH:LargeMedicalRoomReskin:0:1.25195:0.44702:-1.4082:1.21484:0.23706:-1.4082:1.21484:0.23706:-1.00977",
        "TOUCH:SmallBlockCapCockpit:0:-0.51563:0.68359:-0.427:-0.51563:0.6333:-0.45605:-0.30518:0.6333:-0.45605",
        "TOUCH:SmallBlockCapCockpit:1:-0.46753:0.23303:-0.3999:-0.46704:0.04352:-0.34863:-0.20093:0.04227:-0.4375",
        "TOUCH:SmallBlockCapCockpit:2:-0.15991:0.22644:-0.47192:-0.16016:0.08759:-0.43677:0.16016:0.08759:-0.43677",
        "TOUCH:SmallBlockCapCockpit:3:0.20105:0.2323:-0.48755:0.20093:0.04227:-0.4375:0.46704:0.04352:-0.34863",
        "TOUCH:HoloLCDLarge:0:-1.25:1.25:-0.82324:-1.25:-1.25:-0.82324:1.25:-1.25:-0.82324"
      };
      _coordsList.AddRange(coords);

      var current = _fileHandler.Load();
      if (current != null)
      {
        foreach (var coord in current.Coords)
          AddSurfaceCoords(coord);
      }
    }

    public void SaveSurfaceCoordsFile()
    {
      _fileHandler.Save(new SurfaceCoordsFile(_customCoordsList));
    }

    public void Dispose()
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

    public void AddSurfaceCoords(string coordsString)
    {
      var parsedCoord = SurfaceCoords.Parse(coordsString);
      if (!parsedCoord.IsEmpty())
      {
        RemoveSurfaceCoords(coordsString, true);
        _coordsList.Add(parsedCoord.ToString());
        _customCoordsList.Add(parsedCoord.ToString());
      }
    }

    public string GetSurfaceCoords(string subtypeId, int surfaceIndex)
    {
      return _coordsList.Find(x => x.StartsWith($"{SurfaceCoords.Prefix}:{subtypeId}:{surfaceIndex}")) ?? "";
    }

    public void RemoveSurfaceCoordsByTypeAndIndex(string subtypeId, int surfaceIndex)
    {
      RemoveSurfaceCoords(GetSurfaceCoords(subtypeId, surfaceIndex), false);
    }

    public void RemoveSurfaceCoords(string coordsString, bool ignoreVertices)
    {
      if (ignoreVertices)
      {
        var parsedCoord = SurfaceCoords.Parse(coordsString);
        if (!parsedCoord.IsEmpty())
          RemoveSurfaceCoordsByTypeAndIndex(parsedCoord.BuilderTypeString, parsedCoord.Index);
      }
      else
      {
        var index = _coordsList.IndexOf(coordsString);
        if (index >= 0)
          _coordsList.RemoveAt(index);

        var indexCustom = _customCoordsList.IndexOf(coordsString);
        if (indexCustom >= 0)
          _customCoordsList.RemoveAt(indexCustom);
      }
    }

  }
}