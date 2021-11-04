using System;
using Sandbox.ModAPI;
using VRage.Game.Components;
using VRage.ObjectBuilders;
using VRageMath;

namespace Lima
{
  public class BlockUpdater : MyGameLogicComponent
  {
    public event Action UpdateEvent;
    private ulong _ticks = 0;
    private ulong _skip = 2;
    public ulong Fps { get { return (ulong)(60 / _skip); } set { _skip = (ulong)(60 / MathHelper.Clamp(value, 1, 60)); } }

    public BlockUpdater(ulong fps = 30)
    {
      Fps = fps;
    }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      base.Init(objectBuilder);

      if (MyAPIGateway.Utilities?.IsDedicated == true)
        return;

      NeedsUpdate |= VRage.ModAPI.MyEntityUpdateEnum.EACH_FRAME;
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (_ticks++ % _skip == 0)
        UpdateEvent?.Invoke();
    }

    public override void Close()
    {
      base.Close();
      UpdateEvent = null;
    }
  }
}