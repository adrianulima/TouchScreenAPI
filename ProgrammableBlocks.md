# TouchScreenAPI with Programmable Blocks

## Why it is not recommended?

It is possible to use TouchScreenAPI with PB, but it is not recommended because of the way Programmable Blocks works in game. Basically every screen update is sent to the server, since to move the mouse and animate the screen elements this api render the screen very often which would cause a lof of network traffic.

It is more recommended that you use it as a Mod with the TSS feature from the game.

> [!IMPORTANT]
> For both the Mods and PB Scripts to work using the API, the TouchScreenAPI mod must be enabled in the game settings.

## I still wanna try it, how to use it?

If you still want to use it, maybe for a single player play through, you can follow the steps below.
There are two ways to use the API, one for the simple Touch API (No UI Kit, just cursor) and another for the UI Kit (Buttons and UI elements).

### For simple Touch API (No UI Kit, just cursor)

1. Copy the entire TouchScreenAPI class from [TouchScreenAPIPB.cs File](./Data/Scripts/Lima/Api/PB/TouchScreenAPIPB.cs.md) to your script.
2. Create `TouchScreenAPI Api;` variable in your script.
3. Instantiate the api on the Program() method with `Api = new TouchScreenAPI(Me);`
4. You now have access tot the API and all the classes and features.

#### Example code using the API

```csharp
// Programmable Block Script
TouchScreenAPI Api;

IMyTerminalBlock _block;
IMyTextSurface _surface;
TouchScreen _screen;
Cursor _cursor;
ClickHandler _squareHandler;
MySprite _square;

bool _init = false;

public Program()
{
  Api = new TouchScreenAPI(Me);

  // Name of the LCD block in the grid
  _block = GridTerminalSystem.GetBlockWithName("LCD Panel PB") as IMyTerminalBlock;
  if (_block == null)
    return;
  var surfaceProvider = _block as IMyTextSurfaceProvider;
  if (surfaceProvider == null)
    return;

  _surface = surfaceProvider.GetSurface(0);
  if (_surface == null)
    return;

  _screen = new TouchScreen(Api.CreateTouchScreen(_block, _surface));
  if (_screen == null)
    return;
  _cursor = new Cursor(_screen);
  if (_cursor == null)
    return;

  _surface.ScriptBackgroundColor = Color.Black;

  _square = new MySprite()
  {
    Type = SpriteType.TEXTURE,
    Data = "SquareSimple",
    RotationOrScale = 0,
    Color = Color.White,
    Position = new Vector2(_surface.TextureSize.X * 0.25f, _surface.TextureSize.Y * 0.5f),
    Size = _surface.TextureSize * 0.5f
  };
  _squareHandler = new ClickHandler();
  _squareHandler.HitArea = new Vector4(_surface.TextureSize.X * 0.25f, _surface.TextureSize.Y * 0.25f, _surface.TextureSize.X * 0.75f, _surface.TextureSize.Y * 0.75f);

  _init = true;
  Runtime.UpdateFrequency = UpdateFrequency.Update10;
}

public void Main(string argument, UpdateType updateType)
{
  Echo($"Api.IsReady: {Api.IsReady}");
  Echo($"_init: {_init}");

  if (!_init)
    return;

  _squareHandler.Update(_screen);
  if (_squareHandler.Mouse1.IsPressed)
    _square.Color = Color.DarkBlue;
  else if (_squareHandler.Mouse1.IsOver)
    _square.Color = Color.DarkRed;
  else
    _square.Color = Color.DarkGreen;

  using (var frame = _surface.DrawFrame())
  {
    frame.Add(_square);
    frame.AddRange(_cursor.GetSprites());
    frame.Dispose();
  }
}

public class TouchScreenAPI
{
  //... the content copied from TouchScreenAPIPB.cs
}
```

### For UI Kit

1. Copy the entire TouchUiKit class from [TouchUiKitPB.cs File](./Data/Scripts/Lima/Api/PB/TouchUiKitPB.cs.md) to your script.
2. Create `TouchUiKit Api;` variable in your script.
3. Instantiate the api on the Program() method with `Api = new TouchUiKit(Me);`
4. You now have access tot the API and all the classes and features.

#### Example code using the Ui Kit API

```csharp
// Programmable Block Script
TouchUiKit Api;

IMyTerminalBlock _block;
IMyTextSurface _surface;
SampleApp _app;

public Program()
{
  Api = new TouchUiKit(Me);

  // Name of the LCD block in the grid
  _block = GridTerminalSystem.GetBlockWithName("LCD Panel PB") as IMyTerminalBlock;
  var surfaceProvider = _block as IMyTextSurfaceProvider;
  _surface = surfaceProvider.GetSurface(0);

  _surface.ScriptBackgroundColor = Color.Black;

  _app = new SampleApp(_block, _surface);
  _app.Theme.Scale = Math.Min(Math.Max(Math.Min(_surface.SurfaceSize.X, _surface.SurfaceSize.Y) / 512, 0.4f), 2);
  _app.Cursor.Scale = _app.Theme.Scale;

  Runtime.UpdateFrequency = UpdateFrequency.Update10;
}

public void Main(string argument, UpdateType updateType)
{
  Echo($"Api.IsReady: {Api.IsReady}");

  if (_app == null)
    return;

  using (var frame = _surface.DrawFrame())
  {
    _app.ForceUpdate();
    frame.AddRange(_app.GetSprites());
    frame.Dispose();
  }
}

public class SampleApp : TouchApp
{
  public SampleApp(IMyCubeBlock block, IMyTextSurface surface) : base(block, surface)
  {
    DefaultBg = true;

    var window = new View(ViewDirection.Row);
    var windowBar = new WindowBar("Sample App");
    windowBar.BgColor = Color.Blue;
    windowBar.Label.TextColor = Color.Red;

    // window.Border=new Vector4(4);
    // window.Padding=new Vector4(14);

    var col1 = new ScrollView();
    col1.Flex = new Vector2(1, 0.75f);

    col1.Margin = new Vector4(24);
    col1.Border = new Vector4(4);
    col1.BorderColor = Color.Red;
    col1.Padding = new Vector4(4);
    col1.ScrollAlwaysVisible = true;

    var col2 = new View();

    var header1 = new Label("Column 1\nMultiline Test", 0.6f);

    var tabs = new Switch(new string[] { "Tab 1", "Tab 2", "Tab 3", "Tab 4" }, 0, (int v) =>
    {
      // Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessage($"{v}", "SampleApp");
    });

    var labelSlider = new Label("A Touch Slider");
    labelSlider.Margin = Vector4.UnitY * 8;
    var slider = new Slider(-100, 100, (float v) =>
    {
      // Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessage($"{v}", "SampleApp");
    });
    slider.Value = 50f;
    var labelSliderRange = new Label("Range Slider");
    labelSliderRange.Margin = Vector4.UnitY * 8;
    var sliderRange = new SliderRange(-100, 100, (float v, float v2) =>
    {
      // Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessage($"{v}, {v2}", "SampleApp");
    });
    sliderRange.Value = 50f;
    sliderRange.ValueLower = -50f;

    var labelProgressBar = new Label("Progress Bar");
    labelProgressBar.Alignment = VRage.Game.GUI.TextPanel.TextAlignment.LEFT;
    labelProgressBar.Margin = Vector4.UnitY * 8;
    var progressBar = new ProgressBar(0, 100, false, 2);
    progressBar.Value = 50;
    progressBar.Label.Text = "50%";
    progressBar.Label.Alignment = VRage.Game.GUI.TextPanel.TextAlignment.RIGHT;

    var labelSelector = new Label("Color");
    labelSelector.Margin = Vector4.UnitY * 8;
    var list = new List<Color> { Color.MediumSeaGreen, Color.Magenta, Color.Maroon, Color.MediumAquamarine, Color.MediumBlue, Color.MediumOrchid };
    var names = new List<String> { "MediumSeaGreen", "Magenta", "Maroon", "MediumAquamarine", "MediumBlue", "MediumOrchid" };
    var selector = new Selector(names, (int i, string color) =>
    {
      this.Screen.Surface.ScriptForegroundColor = list[i];
    });

    var labelSwitch = new Label("Toggle: Off");
    labelSwitch.Alignment = VRage.Game.GUI.TextPanel.TextAlignment.RIGHT;
    labelSwitch.Margin = Vector4.UnitY * 8;
    var switcher = new Switch(new string[] { "Off", "On" }, 0, (int v) =>
    {
      if (v == 1)
        labelSwitch.Text = "Toggle: On";
      else
        labelSwitch.Text = "Toggle: Off";
    });
    // switcher.Margin=new Vector4(8, 0, 8, 0);

    var button = new Button("A Touch Button", () =>
    {
      // Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessage($"{col1.GetFlexSize().Y}", "SampleApp");
    });

    button.Pixels = new Vector2(0, 42);
    button.Margin = Vector4.UnitY * 22;

    var labelTextField = new Label("Text Field");
    labelTextField.Margin = Vector4.UnitY * 8;
    var textField = new TextField();

    var checkboxView = new View(ViewDirection.Row);
    checkboxView.Alignment = ViewAlignment.Center;
    checkboxView.Flex = new Vector2(1, 0);
    checkboxView.Pixels = new Vector2(0, 24);
    // checkboxView.Margin = new Vector4(4);
    checkboxView.Gap = 4;
    var checkboxLabel = new Label("Checkbox", 0.5f, TextAlignment.RIGHT);
    var checkbox = new Checkbox((bool v) =>
    {
      // Sandbox.Game.MyVisualScriptLogicProvider.SendChatMessage($"{v}", "SampleApp");
    });
    checkboxView.AddChild(checkboxLabel);
    checkboxView.AddChild(checkbox);


    col1.AddChild(header1);
    col1.AddChild(labelSlider);
    col1.AddChild(slider);
    col1.AddChild(labelSliderRange);
    col1.AddChild(sliderRange);
    col1.AddChild(labelProgressBar);
    col1.AddChild(progressBar);
    col1.AddChild(labelSwitch);
    col1.AddChild(switcher);
    col1.AddChild(labelSelector);
    col1.AddChild(selector);
    col2.AddChild(tabs);
    col2.AddChild(button);
    col2.AddChild(labelTextField);
    col2.AddChild(textField);
    col2.AddChild(checkboxView);
    window.AddChild(col1);
    window.AddChild(col2);
    AddChild(windowBar);
    AddChild(window);
  }
}

public class TouchUiKit
{
  // ... the content copied from TouchUiKitPB.cs
}
```

> [!NOTE]
> You can minify the contents of the API to save some lines and characters from the PB script, I kept it clean and commented for better readability.
