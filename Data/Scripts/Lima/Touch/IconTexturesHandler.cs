using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System.Collections.Generic;
using System;
using VRage.Collections;
using IngameIMyCubeBlock = VRage.Game.ModAPI.Ingame.IMyCubeBlock;
using IngameIMyBlockGroup = Sandbox.ModAPI.Ingame.IMyBlockGroup;

namespace Lima
{
  public class IconTexturesHandler
  {
    private ListReader<MyLCDTextureDefinition> _lcdTextureDefinitions;

    private Dictionary<string, int> _dict = new Dictionary<string, int>();
    private int _placeholder = 0;

    public IconTexturesHandler()
    {
      _lcdTextureDefinitions = MyDefinitionManager.Static.GetLCDTexturesDefinitions();
    }

    public void Dispose()
    {
      _lcdTextureDefinitions = null;
      _dict = null;
    }

    public string GetBlockTexture(IngameIMyCubeBlock block)
    {
      var defId = block.BlockDefinition.ToString();
      if (_dict.ContainsKey(defId))
        return $"TouchScreenAPI/Placeholder{_dict[defId]}";

      if (_placeholder < 1000)
      {
        var placeholderSubtype = $"TouchScreenAPI/Placeholder{_placeholder + 1}";
        foreach (var item in _lcdTextureDefinitions)
        {
          if (item.Id.SubtypeName == placeholderSubtype)
          {
            var icons = (block as MyCubeBlock).BlockDefinition.Icons;
            if (icons.Length > 0)
            {
              _dict[defId] = _placeholder + 1;
              item.SpritePath = icons[0];
              _placeholder++;
              return placeholderSubtype;
            }
          }
        }
      }
      return "TouchScreenAPI/Fake";
    }

    public string GetBlockGroupTexture(IngameIMyBlockGroup blockGroup)
    {
      List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
      (blockGroup as IMyBlockGroup).GetBlocks(blocks);

      HashSet<Type> hashSet = new HashSet<Type>();
      foreach (var myTerminalBlock in blocks)
        hashSet.Add(myTerminalBlock.GetType());

      if (hashSet.Count == 1)
        return GetBlockTexture(blocks[0]);
      else
        return "TouchScreenAPI/GroupIcon";
    }
  }
}