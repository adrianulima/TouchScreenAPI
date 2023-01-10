
using Sandbox.ModAPI;
using VRage.Collections;
using System;

namespace Lima.Touch.UiKit
{
  public class TextInputHandler
  {
    public Func<char, bool> OnInputAction;
    private readonly Action<char> _onAppendAction;
    private readonly Action _onBackspaceAction;

    public TextInputHandler(Action<char> onAppendAction, Action onBackspaceAction, Func<char, bool> onInputAction = null)
    {
      _onAppendAction = onAppendAction;
      _onBackspaceAction = onBackspaceAction;
      OnInputAction = onInputAction;
    }

    public void Update()
    {
      ListReader<char> input = MyAPIGateway.Input.TextInput;

      for (int i = 0; i < input.Count; i++)
      {
        if (input[i] == '\b')
          _onBackspaceAction?.Invoke();
        else if (OnInputAction == null || OnInputAction(input[i]))
          _onAppendAction?.Invoke(input[i]);
      }
    }
  }
}