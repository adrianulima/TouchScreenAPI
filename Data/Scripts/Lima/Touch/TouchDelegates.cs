using Lima.Touch.UiKit.Elements;
using Lima.Touch.UiKit;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System;
using VRage.Game.Entity;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRageMath;
using IngameIMyTextSurface = Sandbox.ModAPI.Ingame.IMyTextSurface;
using IngameIMyCubeBlock = VRage.Game.ModAPI.Ingame.IMyCubeBlock;
using IngameIMyBlockGroup = Sandbox.ModAPI.Ingame.IMyBlockGroup;


namespace Lima.Touch
{
  public class TouchDelegates
  {
    private const long _channel = 2668820525;
    private bool _isRegistered;
    public bool IsReady { get; private set; }

    public void Load()
    {
      if (!_isRegistered)
      {
        _isRegistered = true;
        MyAPIGateway.Utilities.RegisterMessageHandler(_channel, HandleMessage);
        MyEntities.OnEntityCreate += EntityCreated;
      }
      IsReady = true;
      MyAPIGateway.Utilities.SendModMessage(_channel, GetTouchAndUiApiDictionary());
    }

    public void Unload()
    {
      if (_isRegistered)
      {
        _isRegistered = false;
        MyAPIGateway.Utilities.UnregisterMessageHandler(_channel, HandleMessage);
        MyEntities.OnEntityCreate -= EntityCreated;
      }
      IsReady = false;
      MyAPIGateway.Utilities.SendModMessage(_channel, new Dictionary<string, Delegate>());
    }

    private void HandleMessage(object msg)
    {
      if ((msg as string) == "ApiRequestTouch")
        MyAPIGateway.Utilities.SendModMessage(_channel, GetTouchApiDictionary());
      else if ((msg as string) == "ApiRequestTouchAndUi")
        MyAPIGateway.Utilities.SendModMessage(_channel, GetTouchAndUiApiDictionary());
    }

    private void EntityCreated(MyEntity ent)
    {
      IMyProgrammableBlock pb = ent as IMyProgrammableBlock;
      if (pb != null)
      {
        MyEntities.OnEntityCreate -= EntityCreated;

        var dict = GetTouchAndUiApiDictionary();
        var builder = ImmutableDictionary.CreateBuilder<string, Delegate>();
        foreach (var del in dict)
          builder.Add(del.Key, del.Value);

        var immutableDict = builder.ToImmutable();
        var p = Sandbox.ModAPI.MyAPIGateway.TerminalControls.CreateProperty<IReadOnlyDictionary<string, Delegate>, IMyProgrammableBlock>($"{_channel}");
        p.Getter = b => immutableDict;
        p.Setter = (b, v) => { };
        Sandbox.ModAPI.MyAPIGateway.TerminalControls.AddControl<IMyProgrammableBlock>(p);
      }
    }

    private Dictionary<string, Delegate> GetTouchAndUiApiDictionary()
    {
      var dict = new Dictionary<string, Delegate>
      {
        { "Theme_GetBgColor", new Func<object, Color>(Theme_GetBgColor) },
        { "Theme_GetWhiteColor", new Func<object, Color>(Theme_GetWhiteColor) },
        { "Theme_GetMainColor", new Func<object, Color>(Theme_GetMainColor) },
        { "Theme_GetMainColorDarker", new Func<object, int, Color>(Theme_GetMainColorDarker) },
        { "Theme_MeasureStringInPixels", new Func<object, string, string, float, Vector2>(Theme_MeasureStringInPixels) },
        { "Theme_GetScale", new Func<object, float>(Theme_GetScale) },
        { "Theme_SetScale", new Action<object, float>(Theme_SetScale) },
        { "Theme_GetFont", new Func<object, string>(Theme_GetFont) },
        { "Theme_SetFont", new Action<object, string>(Theme_SetFont) },

        { "ElementBase_GetEnabled", new Func<object, bool>(ElementBase_GetEnabled) },
        { "ElementBase_SetEnabled", new Action<object, bool>(ElementBase_SetEnabled) },
        { "ElementBase_GetAbsolute", new Func<object, bool>(ElementBase_GetAbsolute) },
        { "ElementBase_SetAbsolute", new Action<object, bool>(ElementBase_SetAbsolute) },
        { "ElementBase_GetSelfAlignment", new Func<object, byte>(ElementBase_GetSelfAlignment) },
        { "ElementBase_SetSelfAlignment", new Action<object, byte>(ElementBase_SetSelfAlignment) },
        { "ElementBase_GetPosition", new Func<object, Vector2>(ElementBase_GetPosition) },
        { "ElementBase_SetPosition", new Action<object, Vector2>(ElementBase_SetPosition) },
        { "ElementBase_GetMargin", new Func<object, Vector4>(ElementBase_GetMargin) },
        { "ElementBase_SetMargin", new Action<object, Vector4>(ElementBase_SetMargin) },
        { "ElementBase_GetFlex", new Func<object, Vector2>(ElementBase_GetFlex) },
        { "ElementBase_SetFlex", new Action<object, Vector2>(ElementBase_SetFlex) },
        { "ElementBase_GetPixels", new Func<object, Vector2>(ElementBase_GetPixels) },
        { "ElementBase_SetPixels", new Action<object, Vector2>(ElementBase_SetPixels) },
        { "ElementBase_GetSize", new Func<object, Vector2>(ElementBase_GetSize) },
        { "ElementBase_GetBoundaries", new Func<object, Vector2>(ElementBase_GetBoundaries) },
        { "ElementBase_GetApp", new Func<object, TouchApp>(ElementBase_GetApp) },
        { "ElementBase_GetParent", new Func<object, ContainerBase>(ElementBase_GetParent) },
        { "ElementBase_GetSprites", new Func<object, List<MySprite>>(ElementBase_GetSprites) },
        { "ElementBase_ForceUpdate", new Action<object>(ElementBase_ForceUpdate) },
        { "ElementBase_ForceDispose", new Action<object>(ElementBase_ForceDispose) },
        { "ElementBase_RegisterUpdate", new Action<object, Action>(ElementBase_RegisterUpdate) },
        { "ElementBase_UnregisterUpdate", new Action<object, Action>(ElementBase_UnregisterUpdate) },

        { "ContainerBase_GetChildren", new Func<object, List<object>>(ContainerBase_GetChildren) },
        { "ContainerBase_GetFlexSize", new Func<object, Vector2>(ContainerBase_GetFlexSize) },
        { "ContainerBase_AddChild", new Action<object, object>(ContainerBase_AddChild) },
        { "ContainerBase_AddChildAt", new Action<object, object, int>(ContainerBase_AddChildAt) },
        { "ContainerBase_RemoveChild", new Action<object, object>(ContainerBase_RemoveChild) },
        { "ContainerBase_RemoveChildAt", new Action<object, int>(ContainerBase_RemoveChildAt) },
        { "ContainerBase_MoveChild", new Action<object, object, int>(ContainerBase_MoveChild) },

        { "View_New", new Func<byte, Color?, View>(View_New) },
        { "View_GetOverflow", new Func<object, bool>(View_GetOverflow) },
        { "View_SetOverflow", new Action<object, bool>(View_SetOverflow) },
        { "View_GetDirection", new Func<object, byte>(View_GetDirection) },
        { "View_SetDirection", new Action<object, byte>(View_SetDirection) },
        { "View_GetAlignment", new Func<object, byte>(View_GetAlignment) },
        { "View_SetAlignment", new Action<object, byte>(View_SetAlignment) },
        { "View_GetAnchor", new Func<object, byte>(View_GetAnchor) },
        { "View_SetAnchor", new Action<object, byte>(View_SetAnchor) },
        { "View_GetUseThemeColors", new Func<object, bool>(View_GetUseThemeColors) },
        { "View_SetUseThemeColors", new Action<object, bool>(View_SetUseThemeColors) },
        { "View_GetBgColor", new Func<object, Color>(View_GetBgColor) },
        { "View_SetBgColor", new Action<object, Color>(View_SetBgColor) },
        { "View_GetBorderColor", new Func<object, Color>(View_GetBorderColor) },
        { "View_SetBorderColor", new Action<object, Color>(View_SetBorderColor) },
        { "View_GetBorder", new Func<object, Vector4>(View_GetBorder) },
        { "View_SetBorder", new Action<object, Vector4>(View_SetBorder) },
        { "View_GetPadding", new Func<object, Vector4>(View_GetPadding) },
        { "View_SetPadding", new Action<object, Vector4>(View_SetPadding) },
        { "View_GetGap", new Func<object, int>(View_GetGap) },
        { "View_SetGap", new Action<object, int>(View_SetGap) },

        { "ScrollView_New", new Func<int, Color?, ScrollView>(ScrollView_New) },
        { "ScrollView_GetScroll", new Func<object, float>(ScrollView_GetScroll) },
        { "ScrollView_SetScroll", new Action<object, float>(ScrollView_SetScroll) },
        { "ScrollView_GetScrollAlwaysVisible", new Func<object, bool>(ScrollView_GetScrollAlwaysVisible) },
        { "ScrollView_SetScrollAlwaysVisible", new Action<object, bool>(ScrollView_SetScrollAlwaysVisible) },
        { "ScrollView_GetScrollWheelEnabled", new Func<object, bool>(ScrollView_GetScrollWheelEnabled) },
        { "ScrollView_SetScrollWheelEnabled", new Action<object, bool>(ScrollView_SetScrollWheelEnabled) },
        { "ScrollView_GetScrollWheelStep", new Func<object, float>(ScrollView_GetScrollWheelStep) },
        { "ScrollView_SetScrollWheelStep", new Action<object, float>(ScrollView_SetScrollWheelStep) },
        { "ScrollView_GetScrollBar", new Func<object, object>(ScrollView_GetScrollBar) },

        { "TouchApp_New", new Func<IngameIMyCubeBlock, IngameIMyTextSurface, TouchApp>(TouchApp_New) },
        { "TouchApp_GetScreen", new Func<object, TouchScreen>(TouchApp_GetScreen) },
        { "TouchApp_GetViewport", new Func<object, RectangleF>(TouchApp_GetViewport) },
        { "TouchApp_GetCursor", new Func<object, Cursor>(TouchApp_GetCursor) },
        { "TouchApp_GetTheme", new Func<object, Theme>(TouchApp_GetTheme) },
        { "TouchApp_GetDefaultBg", new Func<object, bool>(TouchApp_GetDefaultBg) },
        { "TouchApp_SetDefaultBg", new Action<object, bool>(TouchApp_SetDefaultBg) },

        { "EmptyButton_New", new Func<Action, object>(EmptyButton_New) },
        { "EmptyButton_GetHandler", new Func<object, object>(EmptyButton_GetHandler) },
        { "EmptyButton_SetOnChange", new Action<object, Action>(EmptyButton_SetOnChange) },
        { "EmptyButton_GetDisabled", new Func<object, bool>(EmptyButton_GetDisabled) },
        { "EmptyButton_SetDisabled", new Action<object, bool>(EmptyButton_SetDisabled) },

        { "Button_New", new Func<string, Action, object>(Button_New) },
        { "Button_GetLabel", new Func<object, object>(Button_GetLabel) },

        { "Checkbox_New", new Func<Action<bool>, bool, object>(Checkbox_New) },
        { "Checkbox_GetValue", new Func<object, bool>(Checkbox_GetValue) },
        { "Checkbox_SetValue", new Action<object, bool>(Checkbox_SetValue) },
        { "Checkbox_SetOnChange", new Action<object, Action<bool>>(Checkbox_SetOnChange) },
        { "Checkbox_GetCheckMark", new Func<object, object>(Checkbox_GetCheckMark) },

        { "Label_New", new Func<string, float, TextAlignment, object>(Label_New) },
        { "Label_GetAutoBreakLine", new Func<object, bool>(Label_GetAutoBreakLine) },
        { "Label_SetAutoBreakLine", new Action<object, bool>(Label_SetAutoBreakLine) },
        { "Label_GetAutoEllipsis", new Func<object, byte>(Label_GetAutoEllipsis) },
        { "Label_SetAutoEllipsis", new Action<object, byte>(Label_SetAutoEllipsis) },
        { "Label_GetHasEllipsis", new Func<object, bool>(Label_GetHasEllipsis) },
        { "Label_GetText", new Func<object, string>(Label_GetText) },
        { "Label_SetText", new Action<object, string>(Label_SetText) },
        { "Label_GetTextColor", new Func<object, Color?>(Label_GetTextColor) },
        { "Label_SetTextColor", new Action<object, Color>(Label_SetTextColor) },
        { "Label_GetFontSize", new Func<object, float>(Label_GetFontSize) },
        { "Label_SetFontSize", new Action<object, float>(Label_SetFontSize) },
        { "Label_GetAlignment", new Func<object, TextAlignment>(Label_GetAlignment) },
        { "Label_SetAlignment", new Action<object, TextAlignment>(Label_SetAlignment) },
        { "Label_GetLines", new Func<object, int>(Label_GetLines) },
        { "Label_GetMaxLines", new Func<object, int>(Label_GetMaxLines) },
        { "Label_SetMaxLines", new Action<object, int>(Label_SetMaxLines) },

        { "BarContainer_New", new Func<bool, object>(BarContainer_New) },
        { "BarContainer_GetIsVertical", new Func<object, bool>(BarContainer_GetIsVertical) },
        { "BarContainer_SetIsVertical", new Action<object, bool>(BarContainer_SetIsVertical) },
        { "BarContainer_GetRatio", new Func<object, float>(BarContainer_GetRatio) },
        { "BarContainer_SetRatio", new Action<object, float>(BarContainer_SetRatio) },
        { "BarContainer_GetOffset", new Func<object, float>(BarContainer_GetOffset) },
        { "BarContainer_SetOffset", new Action<object, float>(BarContainer_SetOffset) },
        { "BarContainer_GetBar", new Func<object, object>(BarContainer_GetBar) },

        { "ProgressBar_New", new Func<float, float, bool, float, object>(ProgressBar_New) },
        { "ProgressBar_GetValue", new Func<object, float>(ProgressBar_GetValue) },
        { "ProgressBar_SetValue", new Action<object, float>(ProgressBar_SetValue) },
        { "ProgressBar_GetMaxValue", new Func<object, float>(ProgressBar_GetMaxValue) },
        { "ProgressBar_SetMaxValue", new Action<object, float>(ProgressBar_SetMaxValue) },
        { "ProgressBar_GetMinValue", new Func<object, float>(ProgressBar_GetMinValue) },
        { "ProgressBar_SetMinValue", new Action<object, float>(ProgressBar_SetMinValue) },
        { "ProgressBar_GetBarsGap", new Func<object, float>(ProgressBar_GetBarsGap) },
        { "ProgressBar_SetBarsGap", new Action<object, float>(ProgressBar_SetBarsGap) },
        { "ProgressBar_GetLabel", new Func<object, object>(ProgressBar_GetLabel) },

        { "Selector_New", new Func<List<string>, Action<int, string>, bool, object>(Selector_New) },
        { "Selector_GetLoop", new Func<object, bool>(Selector_GetLoop) },
        { "Selector_SetLoop", new Action<object, bool>(Selector_SetLoop) },
        { "Selector_GetSelected", new Func<object, int>(Selector_GetSelected) },
        { "Selector_SetSelected", new Action<object, int>(Selector_SetSelected) },
        { "Selector_SetOnChange", new Action<object, Action<int, string>>(Selector_SetOnChange) },

        { "Slider_New", new Func<float, float, Action<float>, object>(Slider_New) },
        { "Slider_GetMaxValue", new Func<object, float>(Slider_GetMaxValue) },
        { "Slider_SetMaxValue", new Action<object, float>(Slider_SetMaxValue) },
        { "Slider_GetMinValue", new Func<object, float>(Slider_GetMinValue) },
        { "Slider_SetMinValue", new Action<object, float>(Slider_SetMinValue) },
        { "Slider_GetValue", new Func<object, float>(Slider_GetValue) },
        { "Slider_SetValue", new Action<object, float>(Slider_SetValue) },
        { "Slider_SetOnChange", new Action<object, Action<float>>(Slider_SetOnChange) },
        { "Slider_GetIsInteger", new Func<object, bool>(Slider_GetIsInteger) },
        { "Slider_SetIsInteger", new Action<object, bool>(Slider_SetIsInteger) },
        { "Slider_GetAllowInput", new Func<object, bool>(Slider_GetAllowInput) },
        { "Slider_SetAllowInput", new Action<object, bool>(Slider_SetAllowInput) },
        { "Slider_GetBar", new Func<object, object>(Slider_GetBar) },
        { "Slider_GetThumb", new Func<object, object>(Slider_GetThumb) },
        { "Slider_GetTextInput", new Func<object, object>(Slider_GetTextInput) },

        { "SliderRange_NewR", new Func<float, float, Action<float, float>, object>(SliderRange_NewR) },
        { "SliderRange_GetValueLower", new Func<object, float>(SliderRange_GetValueLower) },
        { "SliderRange_SetValueLower", new Action<object, float>(SliderRange_SetValueLower) },
        { "SliderRange_SetOnChangeR", new Action<object, Action<float, float>>(SliderRange_SetOnChangeR) },
        { "SliderRange_GetThumbLower", new Func<object, object>(SliderRange_GetThumbLower) },

        { "Switch_New", new Func<string[], int, Action<int>, object>(Switch_New) },
        { "Switch_GetIndex", new Func<object, int>(Switch_GetIndex) },
        { "Switch_SetIndex", new Action<object, int>(Switch_SetIndex) },
        { "Switch_GetButtons", new Func<object, Button[]>(Switch_GetButtons) },
        { "Switch_SetOnChange", new Action<object, Action<int>>(Switch_SetOnChange) },

        { "TextField_New", new Func<object>(TextField_New) },
        { "TextField_GetIsEditing", new Func<object, bool>(TextField_GetIsEditing) },
        { "TextField_GetText", new Func<object, string>(TextField_GetText) },
        { "TextField_SetText", new Action<object, string>(TextField_SetText) },
        { "TextField_SetOnSubmit", new Action<object, Action<string>>(TextField_SetOnSubmit) },
        { "TextField_SetOnBlur", new Action<object, Action<string>>(TextField_SetOnBlur) },
        { "TextField_GetRevertOnBlur", new Func<object, bool>(TextField_GetRevertOnBlur) },
        { "TextField_SetRevertOnBlur", new Action<object, bool>(TextField_SetRevertOnBlur) },
        { "TextField_GetSubmitOnBlur", new Func<object, bool>(TextField_GetSubmitOnBlur) },
        { "TextField_SetSubmitOnBlur", new Action<object, bool>(TextField_SetSubmitOnBlur) },
        { "TextField_GetIsNumeric", new Func<object, bool>(TextField_GetIsNumeric) },
        { "TextField_SetIsNumeric", new Action<object, bool>(TextField_SetIsNumeric) },
        { "TextField_GetIsInteger", new Func<object, bool>(TextField_GetIsInteger) },
        { "TextField_SetIsInteger", new Action<object, bool>(TextField_SetIsInteger) },
        { "TextField_GetAllowNegative", new Func<object, bool>(TextField_GetAllowNegative) },
        { "TextField_SetAllowNegative", new Action<object, bool>(TextField_SetAllowNegative) },
        { "TextField_GetLabel", new Func<object, object>(TextField_GetLabel) },
        { "TextField_Blur", new Action<object>(TextField_Blur) },
        { "TextField_Focus", new Action<object>(TextField_Focus) },

        { "WindowBar_New", new Func<string, object>(WindowBar_New) },
        { "WindowBar_GetLabel", new Func<object, object>(WindowBar_GetLabel) },

        { "Chart_New", new Func<int, object>(Chart_New) },
        { "Chart_GetDataSets", new Func<object, List<float[]>>(Chart_GetDataSets) },
        { "Chart_GetDataColors", new Func<object, List<Color>>(Chart_GetDataColors) },
        { "Chart_GetGridHorizontalLines", new Func<object, int>(Chart_GetGridHorizontalLines) },
        { "Chart_SetGridHorizontalLines", new Action<object, int>(Chart_SetGridHorizontalLines) },
        { "Chart_GetGridVerticalLines", new Func<object, int>(Chart_GetGridVerticalLines) },
        { "Chart_SetGridVerticalLines", new Action<object, int>(Chart_SetGridVerticalLines) },
        { "Chart_GetMaxValue", new Func<object, float>(Chart_GetMaxValue) },
        { "Chart_GetMinValue", new Func<object, float>(Chart_GetMinValue) },
        { "Chart_GetGridColor", new Func<object, Color?>(Chart_GetGridColor) },
        { "Chart_SetGridColor", new Action<object, Color>(Chart_SetGridColor) },

        { "EmptyElement_New", new Func<object>(EmptyElement_New) }
      };

      return GetTouchApiDictionary().Concat(dict).ToLookup(x => x.Key, x => x.Value).ToDictionary(x => x.Key, g => g.First());
    }

    private Dictionary<string, Delegate> GetTouchApiDictionary()
    {
      var dict = new Dictionary<string, Delegate>
      {
        { "CreateTouchScreen", new Func<IngameIMyCubeBlock, IngameIMyTextSurface, object>(CreateTouchScreen) },
        { "RemoveTouchScreen", new Action<IngameIMyCubeBlock, IngameIMyTextSurface>(RemoveTouchScreen) },
        { "AddSurfaceCoords", new Action<string>(AddSurfaceCoords) },
        { "RemoveSurfaceCoords", new Action<string>(RemoveSurfaceCoords) },
        { "GetBlockIconSprite", new Func<IngameIMyCubeBlock, string>(GetBlockIconSprite) },
        { "GetBlockGroupIconSprite", new Func<IngameIMyBlockGroup, string>(GetBlockGroupIconSprite) },

        { "TouchScreen_GetBlock", new Func<object, IngameIMyCubeBlock>(TouchScreen_GetBlock) },
        { "TouchScreen_GetSurface", new Func<object, IngameIMyTextSurface>(TouchScreen_GetSurface) },
        { "TouchScreen_GetIndex", new Func<object, int>(TouchScreen_GetIndex) },
        { "TouchScreen_IsOnScreen", new Func<object, bool>(TouchScreen_IsOnScreen) },
        { "TouchScreen_GetMouse1", new Func<object, object>(TouchScreen_GetMouse1) },
        { "TouchScreen_GetMouse2", new Func<object, object>(TouchScreen_GetMouse2) },
        { "TouchScreen_GetCursorPosition", new Func<object, Vector2>(TouchScreen_GetCursorPosition) },
        { "TouchScreen_GetInteractiveDistance", new Func<object, float>(TouchScreen_GetInteractiveDistance) },
        { "TouchScreen_SetInteractiveDistance", new Action<object, float>(TouchScreen_SetInteractiveDistance) },
        { "TouchScreen_GetRotation", new Func<object, int>(TouchScreen_GetRotation) },
        { "TouchScreen_CompareWithBlockAndSurface", new Func<object, IngameIMyCubeBlock, IngameIMyTextSurface, bool>(TouchScreen_CompareWithBlockAndSurface) },
        { "TouchScreen_ForceDispose", new Action<object>(TouchScreen_ForceDispose) },

        { "Cursor_New", new Func<object, Cursor>(Cursor_New) },
        { "Cursor_GetEnabled", new Func<object, bool>(Cursor_GetEnabled) },
        { "Cursor_SetEnabled", new Action<object, bool>(Cursor_SetEnabled) },
        { "Cursor_GetScale", new Func<object, float>(Cursor_GetScale) },
        { "Cursor_SetScale", new Action<object, float>(Cursor_SetScale) },
        { "Cursor_GetPosition", new Func<object, Vector2>(Cursor_GetPosition) },
        { "Cursor_IsInsideArea", new Func<object, float, float, float, float, bool>(Cursor_IsInsideArea) },
        { "Cursor_GetSprites", new Func<object, List<MySprite>>(Cursor_GetSprites) },
        { "Cursor_ForceDispose", new Action<object>(Cursor_ForceDispose) },

        { "ClickHandler_New", new Func<object>(ClickHandler_New) },
        { "ClickHandler_GetHitArea", new Func<object, Vector4>(ClickHandler_GetHitArea) },
        { "ClickHandler_SetHitArea", new Action<object, Vector4>(ClickHandler_SetHitArea) },
        { "ClickHandler_Update", new Action<object, object>(ClickHandler_Update) },
        { "ClickHandler_GetMouse1", new Func<object, object>(ClickHandler_GetMouse1) },
        { "ClickHandler_GetMouse2", new Func<object, object>(ClickHandler_GetMouse2) },

        { "ButtonState_New", new Func<object>(ButtonState_New) },
        { "ButtonState_IsReleased", new Func<object, bool>(ButtonState_IsReleased) },
        { "ButtonState_IsOver", new Func<object, bool>(ButtonState_IsOver) },
        { "ButtonState_IsPressed", new Func<object, bool>(ButtonState_IsPressed) },
        { "ButtonState_JustReleased", new Func<object, bool>(ButtonState_JustReleased) },
        { "ButtonState_JustPressed", new Func<object, bool>(ButtonState_JustPressed) },
        { "ButtonState_Update", new Action<object, bool, bool>(ButtonState_Update) },
      };

      return dict;
    }

    private object CreateTouchScreen(IngameIMyCubeBlock block, IngameIMyTextSurface surface)
    {
      var castBlock = block as IMyCubeBlock;
      var castSurface = surface as IMyTextSurface;
      if (castBlock == null || castSurface == null)
        return null;

      RemoveTouchScreen(castBlock, castSurface);
      var screen = new TouchScreen(castBlock, castSurface);
      TouchSession.Instance.TouchMan.Screens.Add(screen);
      return screen;
    }
    private void RemoveTouchScreen(IngameIMyCubeBlock block, IngameIMyTextSurface surface)
    {
      var castBlock = block as IMyCubeBlock;
      var castSurface = surface as IMyTextSurface;
      if (castBlock == null || castSurface == null)
        return;
      TouchSession.Instance.TouchMan.RemoveScreen(castBlock, castSurface);
    }
    // private List<TouchScreen> GetTouchScreensList() => TouchSession.Instance.TouchMan.Screens;
    // private TouchScreen GetTargetTouchScreen() => TouchSession.Instance.TouchMan.CurrentScreen;
    private void AddSurfaceCoords(string coords) => TouchSession.Instance.SurfaceCoordsMan.AddSurfaceCoords(coords);
    private void RemoveSurfaceCoords(string coords)
    {
      TouchSession.Instance.SurfaceCoordsMan.RemoveSurfaceCoords(coords, true);
    }
    private string GetBlockIconSprite(IngameIMyCubeBlock block)
    {
      return TouchSession.Instance.IconHandler.GetBlockTexture(block);
    }
    private string GetBlockGroupIconSprite(IngameIMyBlockGroup blockGroup)
    {
      return TouchSession.Instance.IconHandler.GetBlockGroupTexture(blockGroup);
    }

    private IngameIMyCubeBlock TouchScreen_GetBlock(object obj) => (obj as TouchScreen).Block;
    private IngameIMyTextSurface TouchScreen_GetSurface(object obj) => (obj as TouchScreen).Surface;
    private int TouchScreen_GetIndex(object obj) => (obj as TouchScreen).Index;
    private bool TouchScreen_IsOnScreen(object obj) => (obj as TouchScreen).IsOnScreen;
    private ButtonState TouchScreen_GetMouse1(object obj) => (obj as TouchScreen).Mouse1;
    private ButtonState TouchScreen_GetMouse2(object obj) => (obj as TouchScreen).Mouse2;
    private Vector2 TouchScreen_GetCursorPosition(object obj) => (obj as TouchScreen).CursorPosition;
    private float TouchScreen_GetInteractiveDistance(object obj) => (obj as TouchScreen).InteractiveDistance;
    private void TouchScreen_SetInteractiveDistance(object obj, float distance) => (obj as TouchScreen).InteractiveDistance = distance;
    private int TouchScreen_GetRotation(object obj) => (obj as TouchScreen).Rotation;
    private bool TouchScreen_CompareWithBlockAndSurface(object obj, IngameIMyCubeBlock block, IngameIMyTextSurface surface)
    {
      var castBlock = block as IMyCubeBlock;
      var castSurface = surface as IMyTextSurface;
      if (castBlock == null || castSurface == null)
        return false;
      return (obj as TouchScreen).CompareWithBlockAndSurface(castBlock, castSurface);
    }
    private void TouchScreen_ForceDispose(object obj) => (obj as TouchScreen).Dispose();

    private bool ElementBase_GetEnabled(object obj) => (obj as ElementBase).Enabled;
    private void ElementBase_SetEnabled(object obj, bool enabled) => (obj as ElementBase).Enabled = enabled;
    private bool ElementBase_GetAbsolute(object obj) => (obj as ElementBase).Absolute;
    private void ElementBase_SetAbsolute(object obj, bool absolute) => (obj as ElementBase).Absolute = absolute;
    private byte ElementBase_GetSelfAlignment(object obj) => (byte)(obj as ElementBase).SelfAlignment;
    private void ElementBase_SetSelfAlignment(object obj, byte alignment) => (obj as ElementBase).SelfAlignment = (ViewAlignment)alignment;
    private Vector2 ElementBase_GetPosition(object obj) => (obj as ElementBase).Position;
    private void ElementBase_SetPosition(object obj, Vector2 position) => (obj as ElementBase).Position = position;
    private Vector4 ElementBase_GetMargin(object obj) => (obj as ElementBase).Margin;
    private void ElementBase_SetMargin(object obj, Vector4 margin) => (obj as ElementBase).Margin = margin;
    private Vector2 ElementBase_GetFlex(object obj) => (obj as ElementBase).Flex;
    private void ElementBase_SetFlex(object obj, Vector2 flex) => (obj as ElementBase).Flex = flex;
    private Vector2 ElementBase_GetPixels(object obj) => (obj as ElementBase).Pixels;
    private void ElementBase_SetPixels(object obj, Vector2 pixels) => (obj as ElementBase).Pixels = pixels;
    private Vector2 ElementBase_GetSize(object obj) => (obj as ElementBase).GetSize();
    private Vector2 ElementBase_GetBoundaries(object obj) => (obj as ElementBase).GetBoundaries();
    private TouchApp ElementBase_GetApp(object obj) => (obj as ElementBase).App;
    private ContainerBase ElementBase_GetParent(object obj) => (obj as ElementBase).Parent;
    private List<MySprite> ElementBase_GetSprites(object obj) => (obj as ElementBase).GetSprites();
    private void ElementBase_ForceUpdate(object obj) => (obj as ElementBase).Update();
    private void ElementBase_ForceDispose(object obj) => (obj as ElementBase).Dispose();
    private void ElementBase_RegisterUpdate(object obj, Action update) => (obj as ElementBase).UpdateEvent += update;
    private void ElementBase_UnregisterUpdate(object obj, Action update) => (obj as ElementBase).UpdateEvent -= update;

    private List<object> ContainerBase_GetChildren(object obj) => (obj as ContainerBase).Children.Cast<object>().ToList();
    private Vector2 ContainerBase_GetFlexSize(object obj) => (obj as ContainerBase).GetFlexSize();
    private void ContainerBase_AddChild(object obj, object child) => (obj as ContainerBase).AddChild((ElementBase)child);
    private void ContainerBase_AddChildAt(object obj, object child, int index) => (obj as ContainerBase).AddChild((ElementBase)child, index);
    private void ContainerBase_RemoveChild(object obj, object child) => (obj as ContainerBase).RemoveChild((ElementBase)child);
    private void ContainerBase_RemoveChildAt(object obj, int index) => (obj as ContainerBase).RemoveChild(index);
    private void ContainerBase_MoveChild(object obj, object child, int index) => (obj as ContainerBase).MoveChild((ElementBase)child, index);

    private View View_New(byte direction, Color? bgColor = null) => new View((ViewDirection)direction, bgColor);
    private bool View_GetOverflow(object obj) => (obj as View).Overflow;
    private void View_SetOverflow(object obj, bool overflow) => (obj as View).Overflow = overflow;
    private byte View_GetDirection(object obj) => (byte)(obj as View).Direction;
    private void View_SetDirection(object obj, byte direction) => (obj as View).Direction = (ViewDirection)direction;
    private byte View_GetAlignment(object obj) => (byte)(obj as View).Alignment;
    private void View_SetAlignment(object obj, byte alignment) => (obj as View).Alignment = (ViewAlignment)alignment;
    private byte View_GetAnchor(object obj) => (byte)(obj as View).Anchor;
    private void View_SetAnchor(object obj, byte anchor) => (obj as View).Anchor = (ViewAnchor)anchor;
    private bool View_GetUseThemeColors(object obj) => (obj as View).UseThemeColors;
    private void View_SetUseThemeColors(object obj, bool useTheme) => (obj as View).UseThemeColors = useTheme;
    private Color View_GetBgColor(object obj) => (Color)(obj as View).BgColor;
    private void View_SetBgColor(object obj, Color bgColor) => (obj as View).BgColor = bgColor;
    private Color View_GetBorderColor(object obj) => (Color)(obj as View).BorderColor;
    private void View_SetBorderColor(object obj, Color borderColor) => (obj as View).BorderColor = borderColor;
    private Vector4 View_GetBorder(object obj) => (Vector4)(obj as View).Border;
    private void View_SetBorder(object obj, Vector4 border) => (obj as View).Border = border;
    private Vector4 View_GetPadding(object obj) => (Vector4)(obj as View).Padding;
    private void View_SetPadding(object obj, Vector4 padding) => (obj as View).Padding = padding;
    private int View_GetGap(object obj) => (int)(obj as View).Gap;
    private void View_SetGap(object obj, int gap) => (obj as View).Gap = gap;

    private ScrollView ScrollView_New(int direction, Color? bgColor = null) => new ScrollView((ViewDirection)direction, bgColor);
    private float ScrollView_GetScroll(object obj) => (obj as ScrollView).Scroll;
    private void ScrollView_SetScroll(object obj, float scroll) => (obj as ScrollView).Scroll = scroll;
    private bool ScrollView_GetScrollAlwaysVisible(object obj) => (obj as ScrollView).ScrollAlwaysVisible;
    private void ScrollView_SetScrollAlwaysVisible(object obj, bool visible) => (obj as ScrollView).ScrollAlwaysVisible = visible;
    private bool ScrollView_GetScrollWheelEnabled(object obj) => (obj as ScrollView).ScrollWheelEnabled;
    private void ScrollView_SetScrollWheelEnabled(object obj, bool wheel) => (obj as ScrollView).ScrollWheelEnabled = wheel;
    private float ScrollView_GetScrollWheelStep(object obj) => (obj as ScrollView).ScrollWheelStep;
    private void ScrollView_SetScrollWheelStep(object obj, float step) => (obj as ScrollView).ScrollWheelStep = step;
    private BarContainer ScrollView_GetScrollBar(object obj) => (obj as ScrollView).ScrollBar;

    private TouchApp TouchApp_New(IngameIMyCubeBlock block, IngameIMyTextSurface surface)
    {
      var castBlock = block as IMyCubeBlock;
      var castSurface = surface as IMyTextSurface;
      if (castBlock == null || castSurface == null)
        return null;
      return new TouchApp(castBlock, castSurface);
    }
    private TouchScreen TouchApp_GetScreen(object obj) => (obj as TouchApp).Screen;
    private RectangleF TouchApp_GetViewport(object obj) => (obj as TouchApp).Viewport;
    private Cursor TouchApp_GetCursor(object obj) => (obj as TouchApp).Cursor;
    private Theme TouchApp_GetTheme(object obj) => (obj as TouchApp).Theme;
    private bool TouchApp_GetDefaultBg(object obj) => (obj as TouchApp).DefaultBg;
    private void TouchApp_SetDefaultBg(object obj, bool defaultBg) => (obj as TouchApp).DefaultBg = defaultBg;

    private Cursor Cursor_New(object screen) => new Cursor(screen as TouchScreen);
    private bool Cursor_GetEnabled(object obj) => (obj as Cursor).Enabled;
    private void Cursor_SetEnabled(object obj, bool enabled) => (obj as Cursor).Enabled = enabled;
    private float Cursor_GetScale(object obj) => (obj as Cursor).Scale;
    private void Cursor_SetScale(object obj, float scale) => (obj as Cursor).Scale = scale;
    private Vector2 Cursor_GetPosition(object obj) => (obj as Cursor).Position;
    private bool Cursor_IsInsideArea(object obj, float x, float y, float z, float w) => (obj as Cursor).IsInsideArea(x, y, z, w);
    private List<MySprite> Cursor_GetSprites(object obj) => (obj as Cursor).GetSprites();
    private void Cursor_ForceDispose(object obj) => (obj as Cursor).Dispose();

    private Color Theme_GetBgColor(object obj) => (obj as Theme).BgColor;
    private Color Theme_GetWhiteColor(object obj) => (obj as Theme).WhiteColor;
    private Color Theme_GetMainColor(object obj) => (obj as Theme).MainColor;
    private Color Theme_GetMainColorDarker(object obj, int value)
    {
      var theme = (obj as Theme);
      if (value <= 1) return theme.MainColor_1;
      else if (value <= 2) return theme.MainColor_2;
      else if (value <= 3) return theme.MainColor_3;
      else if (value <= 4) return theme.MainColor_4;
      else if (value <= 5) return theme.MainColor_5;
      else if (value <= 6) return theme.MainColor_6;
      else if (value <= 7) return theme.MainColor_7;
      else if (value <= 8) return theme.MainColor_8;
      return theme.MainColor_9;
    }
    private Vector2 Theme_MeasureStringInPixels(object obj, string text, string font, float scale) => (obj as Theme).MeasureStringInPixels(text, font, scale);
    private float Theme_GetScale(object obj) => (obj as Theme).Scale;
    private void Theme_SetScale(object obj, float scale) => (obj as Theme).Scale = scale;
    private string Theme_GetFont(object obj) => (obj as Theme).Font;
    private void Theme_SetFont(object obj, string font) => (obj as Theme).Font = font;

    private ClickHandler ClickHandler_New() => new ClickHandler();
    private Vector4 ClickHandler_GetHitArea(object obj) => (obj as ClickHandler).HitArea;
    private void ClickHandler_SetHitArea(object obj, Vector4 hitArea) => (obj as ClickHandler).HitArea = hitArea;
    private void ClickHandler_Update(object obj, object screen) => (obj as ClickHandler).Update(screen as TouchScreen);
    private ButtonState ClickHandler_GetMouse1(object obj) => (obj as ClickHandler).Mouse1;
    private ButtonState ClickHandler_GetMouse2(object obj) => (obj as ClickHandler).Mouse2;

    private ButtonState ButtonState_New() => new ButtonState();
    private bool ButtonState_IsReleased(object obj) => (obj as ButtonState).IsReleased;
    private bool ButtonState_IsOver(object obj) => (obj as ButtonState).IsOver;
    private bool ButtonState_IsPressed(object obj) => (obj as ButtonState).IsPressed;
    private bool ButtonState_JustReleased(object obj) => (obj as ButtonState).JustReleased;
    private bool ButtonState_JustPressed(object obj) => (obj as ButtonState).JustPressed;
    private void ButtonState_Update(object obj, bool isPressed, bool isInsideArea) => (obj as ButtonState).Update(isPressed, isInsideArea);

    private EmptyButton EmptyButton_New(Action onChange) => new EmptyButton(onChange);
    private ClickHandler EmptyButton_GetHandler(object obj) => (obj as EmptyButton).Handler;
    private void EmptyButton_SetOnChange(object obj, Action onChange) => (obj as EmptyButton).OnChange = onChange;
    private bool EmptyButton_GetDisabled(object obj) => (obj as EmptyButton).Disabled;
    private void EmptyButton_SetDisabled(object obj, bool disabled) => (obj as EmptyButton).Disabled = disabled;

    private Button Button_New(string text, Action onChange) => new Button(text, onChange);
    private Label Button_GetLabel(object obj) => (obj as Button).Label;

    private Checkbox Checkbox_New(Action<bool> onChange, bool value = false) => new Checkbox(onChange, value);
    private bool Checkbox_GetValue(object obj) => (obj as Checkbox).Value;
    private void Checkbox_SetValue(object obj, bool value) => (obj as Checkbox).Value = value;
    private void Checkbox_SetOnChange(object obj, Action<bool> onChange) => (obj as Checkbox).OnChange = onChange;
    private EmptyElement Checkbox_GetCheckMark(object obj) => (obj as Checkbox).CheckMark;

    private Label Label_New(string text, float fontSize = 0.5f, TextAlignment alignment = TextAlignment.CENTER) => new Label(text, fontSize, alignment);
    private bool Label_GetAutoBreakLine(object obj) => (obj as Label).AutoBreakLine;
    private void Label_SetAutoBreakLine(object obj, bool breakLine) => (obj as Label).AutoBreakLine = breakLine;
    private byte Label_GetAutoEllipsis(object obj) => (byte)(obj as Label).AutoEllipsis;
    private void Label_SetAutoEllipsis(object obj, byte overflow) => (obj as Label).AutoEllipsis = (LabelEllipsis)overflow;
    private bool Label_GetHasEllipsis(object obj) => (obj as Label).HasEllipsis;
    private string Label_GetText(object obj) => (obj as Label).Text;
    private void Label_SetText(object obj, string text) => (obj as Label).Text = text;
    private Color? Label_GetTextColor(object obj) => (obj as Label).TextColor;
    private void Label_SetTextColor(object obj, Color color) => (obj as Label).TextColor = color;
    private float Label_GetFontSize(object obj) => (obj as Label).FontSize;
    private void Label_SetFontSize(object obj, float fontSize) => (obj as Label).FontSize = fontSize;
    private TextAlignment Label_GetAlignment(object obj) => (obj as Label).Alignment;
    private void Label_SetAlignment(object obj, TextAlignment alignment) => (obj as Label).Alignment = alignment;
    private int Label_GetLines(object obj) => (obj as Label).Lines;
    private int Label_GetMaxLines(object obj) => (obj as Label).MaxLines;
    private void Label_SetMaxLines(object obj, int max) => (obj as Label).MaxLines = max;

    private BarContainer BarContainer_New(bool vertical = false) => new BarContainer(vertical);
    private bool BarContainer_GetIsVertical(object obj) => (obj as BarContainer).IsVertical;
    private void BarContainer_SetIsVertical(object obj, bool vertical) => (obj as BarContainer).IsVertical = vertical;
    private float BarContainer_GetRatio(object obj) => (obj as BarContainer).Ratio;
    private void BarContainer_SetRatio(object obj, float ratio) => (obj as BarContainer).Ratio = ratio;
    private float BarContainer_GetOffset(object obj) => (obj as BarContainer).Offset;
    private void BarContainer_SetOffset(object obj, float offset) => (obj as BarContainer).Offset = offset;
    private View BarContainer_GetBar(object obj) => (obj as BarContainer).Bar;

    private ProgressBar ProgressBar_New(float min, float max, bool vertical = false, float barsGap = 0) => new ProgressBar(min, max, vertical, barsGap);
    private float ProgressBar_GetValue(object obj) => (obj as ProgressBar).Value;
    private void ProgressBar_SetValue(object obj, float value) => (obj as ProgressBar).Value = value;
    private float ProgressBar_GetMaxValue(object obj) => (obj as ProgressBar).MaxValue;
    private void ProgressBar_SetMaxValue(object obj, float max) => (obj as ProgressBar).MaxValue = max;
    private float ProgressBar_GetMinValue(object obj) => (obj as ProgressBar).MinValue;
    private void ProgressBar_SetMinValue(object obj, float min) => (obj as ProgressBar).MinValue = min;
    private float ProgressBar_GetBarsGap(object obj) => (obj as ProgressBar).BarsGap;
    private void ProgressBar_SetBarsGap(object obj, float gap) => (obj as ProgressBar).BarsGap = gap;
    private Label ProgressBar_GetLabel(object obj) => (obj as ProgressBar).Label;

    private Selector Selector_New(List<string> labels, Action<int, string> onChange, bool loop = true) => new Selector(labels, onChange, loop);
    private bool Selector_GetLoop(object obj) => (obj as Selector).Loop;
    private void Selector_SetLoop(object obj, bool loop) => (obj as Selector).Loop = loop;
    private int Selector_GetSelected(object obj) => (obj as Selector).Selected;
    private void Selector_SetSelected(object obj, int selected) => (obj as Selector).Selected = selected;
    private void Selector_SetOnChange(object obj, Action<int, string> onChange) => (obj as Selector).OnChange = onChange;

    private Slider Slider_New(float min, float max, Action<float> onChange) => new Slider(min, max, onChange);
    private float Slider_GetMaxValue(object obj) => (obj as Slider).MaxValue;
    private void Slider_SetMaxValue(object obj, float max) => (obj as Slider).MaxValue = max;
    private float Slider_GetMinValue(object obj) => (obj as Slider).MinValue;
    private void Slider_SetMinValue(object obj, float min) => (obj as Slider).MinValue = min;
    private float Slider_GetValue(object obj) => (obj as Slider).Value;
    private void Slider_SetValue(object obj, float value) => (obj as Slider).Value = value;
    private void Slider_SetOnChange(object obj, Action<float> onChange) => (obj as Slider).OnChange = onChange;
    private bool Slider_GetIsInteger(object obj) => (obj as Slider).IsInteger;
    private void Slider_SetIsInteger(object obj, bool interger) => (obj as Slider).IsInteger = interger;
    private bool Slider_GetAllowInput(object obj) => (obj as Slider).AllowInput;
    private void Slider_SetAllowInput(object obj, bool allowInput) => (obj as Slider).AllowInput = allowInput;
    private BarContainer Slider_GetBar(object obj) => (obj as Slider).Bar;
    private EmptyElement Slider_GetThumb(object obj) => (obj as Slider).Thumb;
    private TextField Slider_GetTextInput(object obj) => (obj as Slider).InnerTextField;

    private SliderRange SliderRange_NewR(float min, float max, Action<float, float> onChange) => new SliderRange(min, max, onChange);
    private float SliderRange_GetValueLower(object obj) => (obj as SliderRange).ValueLower;
    private void SliderRange_SetValueLower(object obj, float value) => (obj as SliderRange).ValueLower = value;
    private void SliderRange_SetOnChangeR(object obj, Action<float, float> onChange) => (obj as SliderRange).OnChangeR = onChange;
    private EmptyElement SliderRange_GetThumbLower(object obj) => (obj as SliderRange).ThumbLower;

    private Switch Switch_New(string[] labels, int index = 0, Action<int> onChange = null) => new Switch(labels, index, onChange);
    private int Switch_GetIndex(object obj) => (obj as Switch).Index;
    private void Switch_SetIndex(object obj, int index) => (obj as Switch).Index = index;
    private Button[] Switch_GetButtons(object obj) => (obj as Switch).Buttons;
    private void Switch_SetOnChange(object obj, Action<int> onChange) => (obj as Switch).OnChange = onChange;

    private TextField TextField_New() => new TextField();
    private bool TextField_GetIsEditing(object obj) => (obj as TextField).IsEditing;
    private string TextField_GetText(object obj) => (obj as TextField).Text;
    private void TextField_SetText(object obj, string text) => (obj as TextField).Text = text;
    private void TextField_SetOnSubmit(object obj, Action<string> onSubmit) => (obj as TextField).OnSubmit = onSubmit;
    private void TextField_SetOnBlur(object obj, Action<string> onBlur) => (obj as TextField).OnBlur = onBlur;
    private bool TextField_GetRevertOnBlur(object obj) => (obj as TextField).RevertOnBlur;
    private void TextField_SetRevertOnBlur(object obj, bool revert) => (obj as TextField).RevertOnBlur = revert;
    private bool TextField_GetSubmitOnBlur(object obj) => (obj as TextField).SubmitOnBlur;
    private void TextField_SetSubmitOnBlur(object obj, bool submit) => (obj as TextField).SubmitOnBlur = submit;
    private bool TextField_GetIsNumeric(object obj) => (obj as TextField).IsNumeric;
    private void TextField_SetIsNumeric(object obj, bool isNumeric) => (obj as TextField).IsNumeric = isNumeric;
    private bool TextField_GetIsInteger(object obj) => (obj as TextField).IsInteger;
    private void TextField_SetIsInteger(object obj, bool isInterger) => (obj as TextField).IsInteger = isInterger;
    private bool TextField_GetAllowNegative(object obj) => (obj as TextField).AllowNegative;
    private void TextField_SetAllowNegative(object obj, bool allowNegative) => (obj as TextField).AllowNegative = allowNegative;
    private Label TextField_GetLabel(object obj) => (obj as TextField).Label;
    private void TextField_Blur(object obj) => (obj as TextField).Blur();
    private void TextField_Focus(object obj) => (obj as TextField).Focus();

    private WindowBar WindowBar_New(string text) => new WindowBar(text);
    private Label WindowBar_GetLabel(object obj) => (obj as WindowBar).Label;

    private Chart Chart_New(int intervals) => new Chart(intervals);
    private List<float[]> Chart_GetDataSets(object obj) => (obj as Chart).DataSets;
    private List<Color> Chart_GetDataColors(object obj) => (obj as Chart).DataColors;
    private int Chart_GetGridHorizontalLines(object obj) => (obj as Chart).GridHorizontalLines;
    private void Chart_SetGridHorizontalLines(object obj, int lines) => (obj as Chart).GridHorizontalLines = lines;
    private int Chart_GetGridVerticalLines(object obj) => (obj as Chart).GridVerticalLines;
    private void Chart_SetGridVerticalLines(object obj, int lines) => (obj as Chart).GridVerticalLines = lines;
    private float Chart_GetMaxValue(object obj) => (obj as Chart).MaxValue;
    private float Chart_GetMinValue(object obj) => (obj as Chart).MinValue;
    private Color? Chart_GetGridColor(object obj) => (obj as Chart).GridColor;
    private void Chart_SetGridColor(object obj, Color color) => (obj as Chart).GridColor = color;

    private EmptyElement EmptyElement_New() => new EmptyElement();
  }
}