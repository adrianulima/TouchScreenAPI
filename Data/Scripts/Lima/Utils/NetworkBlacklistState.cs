using ProtoBuf;
using Sandbox.Game;
using Sandbox.ModAPI;
using System.Collections.Generic;
using System;
using VRage.Utils;

namespace Lima
{
  [ProtoContract(UseProtoMembersOnly = true)]
  public class BlacklistState
  {
    [ProtoMember(1)]
    public List<string> controls;
    [ProtoMember(2)]
    public long playerId;
    [ProtoMember(3)]
    public bool enabled;

    public BlacklistState() { }
  }

  public class NetworkBlacklistState
  {
    private ushort _channel;

    public NetworkBlacklistState(ushort channel)
    {
      _channel = channel;
    }

    public void Init()
    {
      if (MyAPIGateway.Multiplayer.MultiplayerActive && MyAPIGateway.Multiplayer.IsServer)
        MyAPIGateway.Multiplayer.RegisterSecureMessageHandler(_channel, HandleMessage);
    }

    public void Dispose()
    {
      MyAPIGateway.Multiplayer.UnregisterSecureMessageHandler(_channel, HandleMessage);
    }

    private void HandleMessage(ushort handler, byte[] rawData, ulong id, bool isFromServer)
    {
      try
      {
        var message = MyAPIGateway.Utilities.SerializeFromBinary<BlacklistState>(rawData);
        if (!isFromServer && MyAPIGateway.Multiplayer.IsServer)
          ApplyBlacklistState(message.controls, message.playerId, message.enabled);
      }
      catch (Exception e)
      {
        MyLog.Default.WriteLineAndConsole($"{e.Message}\n{e.StackTrace}");
      }
    }

    public void SetPlayerInputBlacklistState(List<string> controls, long playerId, bool enabled)
    {
      if (!MyAPIGateway.Multiplayer.MultiplayerActive || MyAPIGateway.Multiplayer.IsServer)
      {
        ApplyBlacklistState(controls, playerId, enabled);
        return;
      }

      var message = new BlacklistState()
      {
        controls = controls,
        playerId = playerId,
        enabled = enabled
      };
      var bytes = MyAPIGateway.Utilities.SerializeToBinary(message);
      MyAPIGateway.Multiplayer.SendMessageToServer(_channel, bytes);
    }

    private void ApplyBlacklistState(List<string> controls, long playerId, bool enabled)
    {
      foreach (string control in controls)
        MyVisualScriptLogicProvider.SetPlayerInputBlacklistState(control, playerId, enabled);
    }
  }
}