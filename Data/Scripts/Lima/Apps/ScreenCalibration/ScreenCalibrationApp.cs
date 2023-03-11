using Lima.Touch.UiKit.Elements;
using Lima.Touch;
using Sandbox.Game;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.Game.Models;
using VRage.Game;
using VRage.Utils;
using VRage;
using VRageMath;
using BlendTypeEnum = VRageRender.MyBillboard.BlendTypeEnum;
using IntersectionFlags = VRage.Game.Components.IntersectionFlags;
using Lima.Utils;

namespace Lima.Apps.ScreenCalibration
{
  public class ScreenCalibrationApp : TouchApp
  {
    private Label _label;
    private View _view;
    private View _overlayView;
    private Button _button;

    private bool _calibrating = true;
    private int _step = 0;
    private bool _wasPressed = false;
    private string _tryAgain = "";
    private string _newCoords = "";
    private bool _crossHair = false;

    private Vector3D _vertex0;
    private Vector3D _vertex1;
    private Vector3D _vertex2;
    // private Vector3D _normal;

    public ScreenCalibrationApp(IMyCubeBlock block, IMyTextSurface surface) : base(block, surface)
    {
      DefaultBg = true;

      Cursor.Enabled = false;

      _overlayView = new View();
      _overlayView.Absolute = true;
      _overlayView.Anchor = ViewAnchor.Center;
      _overlayView.Alignment = ViewAlignment.Center;
      _overlayView.Position = Position;
      AddChild(_overlayView);

      _label = new Label("", 0.7f);
      _overlayView.AddChild(_label);

      _view = new View();
      _view.Alignment = ViewAlignment.Start;
      _view.Anchor = ViewAnchor.Start;
      _view.Padding = new Vector4(15);
      AddChild(_view);

      _button = new Button("Click", () =>
      {
        MyClipboardHelper.SetClipboard(_newCoords);
      });
      _button.Flex = Vector2.Zero;
      _button.Pixels = new Vector2(60, 24);
      _view.AddChild(_button);

      this.UpdateAtSimulationEvent += UpdateCrossHair;
    }

    protected override void CreateAlertPanel()
    {
    }

    private void UpdateCrossHair()
    {
      if (!_crossHair)
        return;
      var camMatrix = MyAPIGateway.Session.Camera.WorldMatrix;
      DrawCrossHair(camMatrix, camMatrix.Translation);
    }

    private MyStringId Material = MyStringId.GetOrCompute("Square");
    private void DrawCrossHair(MatrixD camMatrix, Vector3D camTranslation)
    {
      var currentFOV = MyAPIGateway.Session.Camera.FieldOfViewAngle / 70;
      MyTransparentGeometry.AddBillboardOriented(Material, Color.White, camTranslation
          + camMatrix.Forward * 0.05f, camMatrix.Left, camMatrix.Up, 0.0001f * currentFOV, 0.0006f * currentFOV, Vector2.Zero, BlendTypeEnum.PostPP);
      MyTransparentGeometry.AddBillboardOriented(Material, Color.White, camTranslation
          + camMatrix.Forward * 0.05f, camMatrix.Left, camMatrix.Up, 0.0006f * currentFOV, 0.0001f * currentFOV, Vector2.Zero, BlendTypeEnum.PostPP);
    }

    public override void Dispose()
    {
      base.Dispose();
      this.UpdateAtSimulationEvent -= UpdateCrossHair;
    }

    public override void Update()
    {
      base.Update();

      UpdateRaycast();

      var text = "Calibration failed, reset the App.\n";
      if (_calibrating)
      {
        text = "";
        if (_step == 0)
          text = $"{text}Click the button in the upper left";
        if (_step == 1)
          text = $"{text}Click the button in the lower left";
        if (_step == 2)
          text = $"{text}Click the button in the lower right";
      }
      else if (_step == 3)
      {
        if (MyAPIGateway.Input.IsAnyCtrlKeyPressed())
          text = "Copy calibration to share\nwith others. They can use\n'/touch {coords}' chat command.\n";
        else
          text = "Calibration success. You are done!\n\nHold Ctrl to see extra options.\n";
      }

      if (_step == 0 && Cursor.Position != Vector2.Zero)
        text = $"Only use this App for modded LCDs.\nThis screen is already calibrated!\n Changes will override current.\n\n{text}";

      _button.Enabled = _calibrating || (_step == 3 && MyAPIGateway.Input.IsAnyCtrlKeyPressed());
      _label.Text = text;
    }

    private void UpdateRaycast()
    {
      _crossHair = false;
      if (_calibrating)
      {
        var camMatrix = MyAPIGateway.Session.Camera.WorldMatrix;
        var camPos = camMatrix.Translation;
        var camDirection = camMatrix.Forward;
        var target = camPos + camDirection * 10;

        if (MyAPIGateway.Session.Player?.Controller?.ControlledEntity?.Entity is IMyCockpit && MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.LOOKAROUND))
          _crossHair = true;

        var line = new LineD(camPos, target);
        MyIntersectionResultLineTriangleEx? triangleEx;
        if ((Screen.Block as IMyCubeBlock).GetIntersectionWithLine(ref line, out triangleEx, IntersectionFlags.DIRECT_TRIANGLES))
        {
          InputUtils.SetPlayerUseBlacklistState(true);
          if (!_wasPressed && MyAPIGateway.Input.IsAnyMouseOrJoystickPressed())
          {
            var localPos = triangleEx?.IntersectionPointInObjectSpace ?? default(Vector3);
            var triangle = triangleEx?.Triangle.InputTriangle ?? default(MyTriangle_Vertices);
            var closestVertex = GetClosestVertex(localPos, triangle);

            _step++;
            UpdateStep(closestVertex);

            if (_step == 3)// && _normal == triangleEx?.NormalInObjectSpace)
            {
              _newCoords = new SurfaceCoords(Screen.SubtypeId, Screen.Index, _vertex0, _vertex1, _vertex2).ToString();
              TouchSession.Instance?.SurfaceCoordsMan.AddSurfaceCoords(_newCoords);
              TouchSession.Instance?.SurfaceCoordsMan.SaveSurfaceCoordsFile();
              Screen.RefreshCoords();
              _calibrating = false;

              UpdateButton();
            }
            // else if (_step == 3)
            // {
            //   _tryAgain = "== Try again! ==\n";
            //   _step = 0;
            // }
          }
        }
        else
        {
          InputUtils.SetPlayerUseBlacklistState(false);
          if (_wasPressed)
          {
            _calibrating = false;
            _step = 0;
          }
        }
        _wasPressed = MyAPIGateway.Input.IsAnyMouseOrJoystickPressed();
      }
      else if (!Cursor.Enabled)
        InputUtils.SetPlayerUseBlacklistState(false);
    }

    private void UpdateButton()
    {
      _view.RemoveChild(_button);
      _button.Label.Text = "Copy to clipboard";
      _button.Pixels = new Vector2(200, 24);
      Cursor.Enabled = true;
      _overlayView.AddChild(_button);
    }

    private void UpdateStep(Vector3D pos)
    {
      if (_step == 1)
      {
        _vertex0 = pos;
        _view.Alignment = ViewAlignment.Start;
        _view.Anchor = ViewAnchor.End;
      }
      else if (_step == 2)
      {
        _vertex1 = pos;
        _view.Alignment = ViewAlignment.End;
        _view.Anchor = ViewAnchor.End;
      }
      else
      {
        _vertex2 = pos;
        // _normal = CalculateNormal();
        _view.Alignment = ViewAlignment.Start;
        _view.Anchor = ViewAnchor.Start;
      }
    }

    private Vector3D GetClosestVertex(Vector3D pos, MyTriangle_Vertices triangle)
    {
      var dist0 = Vector3D.Distance(pos, triangle.Vertex0);
      var dist1 = Vector3D.Distance(pos, triangle.Vertex1);
      var dist2 = Vector3D.Distance(pos, triangle.Vertex2);
      var vert = triangle.Vertex0;
      if (dist1 < dist0 && dist1 < dist2)
        vert = triangle.Vertex1;
      if (dist2 < dist1 && dist2 < dist0)
        vert = triangle.Vertex2;
      return vert;
    }

    // private string Format(Vector3D vertex)
    // {
    //   return $"{vertex.X.ToString("0.#####")}:{vertex.Y.ToString("0.#####")}:{vertex.Z.ToString("0.#####")}";
    // }

    // private Vector3D CalculateNormal()
    // {
    //   return Vector3D.Normalize(Vector3D.Cross(_vertex1 - _vertex0, _vertex2 - _vertex0));
    // }
  }
}