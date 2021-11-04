
using Sandbox.ModAPI;
using VRage.Collections;
using System;

namespace Lima
{
  public class TextInputHandler
  {
    public Func<char, bool> OnInputAction;
    private readonly Action<char> OnAppendAction;
    private readonly Action OnBackspaceAction;

    public TextInputHandler(Action<char> onAppendAction, Action onBackspaceAction, Func<char, bool> onInputAction = null)
    {
      OnAppendAction = onAppendAction;
      OnBackspaceAction = onBackspaceAction;
      OnInputAction = onInputAction;
    }

    public void Update()
    {
      ListReader<char> input = MyAPIGateway.Input.TextInput;

      for (int i = 0; i < input.Count; i++)
      {
        if (input[i] == '\b')
          OnBackspaceAction?.Invoke();
        else if (OnInputAction == null || OnInputAction(input[i]))
          OnAppendAction?.Invoke(input[i]);
      }
    }
  }
}