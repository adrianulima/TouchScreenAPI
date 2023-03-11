using Sandbox.ModAPI.Interfaces;
using System.Collections.Generic;
using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;
using IngameIMyTextSurface = Sandbox.ModAPI.Ingame.IMyTextSurface;
using IngameIMyCubeBlock = VRage.Game.ModAPI.Ingame.IMyCubeBlock;

namespace Lima.API.PB
{
  // Copy these classes to inside your PB Program Class
  // This has both the TouchScreen feature and the UI kit
  public class TouchUiKit
  {
    public bool IsReady { get; protected set; }
    private Func<IngameIMyCubeBlock, IngameIMyTextSurface, object> _createTouchScreen;
    private Action<IngameIMyCubeBlock, IngameIMyTextSurface> _removeTouchScreen;
    private Action<string> _addSurfaceCoords;
    private Action<string> _removeSurfaceCoords;
    /// <summary>
    /// Creates an instance of TouchScreen add adds it to the Touch Manager.
    /// TouchScreen is responsible for checking player direction and distance to screen.
    /// Use only one TouchScreen for each surface on the block that will need it.
    /// Needs to be removed with <see cref="RemoveTouchScreen"/> when the App screen is not using the Api anymore.
    /// </summary>
    /// <param name="block">The block the touch point will be calculated.</param>
    /// <param name="surface">The surface the user will handle touch.</param>
    /// <returns></returns>
    public object CreateTouchScreen(IngameIMyCubeBlock block, IngameIMyTextSurface surface) => _createTouchScreen?.Invoke(block, surface);
    /// <summary>
    /// Dispose the instance of TouchScreen related to given block and surface. And also removes from Touch Manager.
    /// </summary>
    /// <param name="block">The related block.</param>
    /// <param name="surface">The related surface.</param>
    public void RemoveTouchScreen(IngameIMyCubeBlock block, IngameIMyTextSurface surface) => _removeTouchScreen?.Invoke(block, surface);
    /// <summary>
    /// This add surface coordinates to Touch Manager, it is a string similar to how GPS strings work.
    /// This same feature is also available using chat message commands with prefix /touch [string].
    /// </summary>
    /// <example>
    /// The string format is:
    /// TOUCH:<block_name>:<surface_number>:topleftX:topleftY:topleftZ:bottomleftX:bottomleftY:bottomleftZ:bottomRightX:bottomRightY:bottomRightZ
    /// Where the X, Y and Z are 3D model local positions, related to model origin.
    /// <code>
    /// AddSurfaceCoords("TOUCH:LargeLCDPanel:0:-1.23463:1.23463:-1.02366:-1.23463:-1.23463:-1.02366:1.23463:-1.23463:-1.02366");
    /// </code>
    /// </example>
    /// <param name="coords">Formatted coords string.</param>
    public void AddSurfaceCoords(string coords) => _addSurfaceCoords?.Invoke(coords);
    /// <summary>
    /// Removes surface coordinates from Touch Manager. Call it if you need to replace for a specific surface.
    /// </summary>
    /// <param name="coords"></param>
    public void RemoveSurfaceCoords(string coords) => _removeSurfaceCoords?.Invoke(coords);
    private UiKitDelegator _apiDel;
    public TouchUiKit(Sandbox.ModAPI.Ingame.IMyTerminalBlock pb)
    {
      var delegates = pb.GetProperty("2668820525")?.As<IReadOnlyDictionary<string, Delegate>>().GetValue(pb);
      if (delegates != null)
      {
        _apiDel = new UiKitDelegator();
        WrapperBase<UiKitDelegator>.SetApi(_apiDel);
        AssignMethod(out _createTouchScreen, delegates["CreateTouchScreen"]);
        AssignMethod(out _removeTouchScreen, delegates["RemoveTouchScreen"]);
        AssignMethod(out _addSurfaceCoords, delegates["AddSurfaceCoords"]);
        AssignMethod(out _removeSurfaceCoords, delegates["RemoveSurfaceCoords"]);
        AssignMethod(out _apiDel.TouchScreen_GetBlock, delegates["TouchScreen_GetBlock"]);
        AssignMethod(out _apiDel.TouchScreen_GetSurface, delegates["TouchScreen_GetSurface"]);
        AssignMethod(out _apiDel.TouchScreen_GetIndex, delegates["TouchScreen_GetIndex"]);
        AssignMethod(out _apiDel.TouchScreen_IsOnScreen, delegates["TouchScreen_IsOnScreen"]);
        AssignMethod(out _apiDel.TouchScreen_GetMouse1, delegates["TouchScreen_GetMouse1"]);
        AssignMethod(out _apiDel.TouchScreen_GetMouse2, delegates["TouchScreen_GetMouse2"]);
        AssignMethod(out _apiDel.TouchScreen_GetCursorPosition, delegates["TouchScreen_GetCursorPosition"]);
        AssignMethod(out _apiDel.TouchScreen_GetInteractiveDistance, delegates["TouchScreen_GetInteractiveDistance"]);
        AssignMethod(out _apiDel.TouchScreen_SetInteractiveDistance, delegates["TouchScreen_SetInteractiveDistance"]);
        AssignMethod(out _apiDel.TouchScreen_GetRotation, delegates["TouchScreen_GetRotation"]);
        AssignMethod(out _apiDel.TouchScreen_CompareWithBlockAndSurface, delegates["TouchScreen_CompareWithBlockAndSurface"]);
        AssignMethod(out _apiDel.TouchScreen_ForceDispose, delegates["TouchScreen_ForceDispose"]);
        AssignMethod(out _apiDel.Cursor_New, delegates["Cursor_New"]);
        AssignMethod(out _apiDel.Cursor_GetEnabled, delegates["Cursor_GetEnabled"]);
        AssignMethod(out _apiDel.Cursor_SetEnabled, delegates["Cursor_SetEnabled"]);
        AssignMethod(out _apiDel.Cursor_GetScale, delegates["Cursor_GetScale"]);
        AssignMethod(out _apiDel.Cursor_SetScale, delegates["Cursor_SetScale"]);
        AssignMethod(out _apiDel.Cursor_GetPosition, delegates["Cursor_GetPosition"]);
        AssignMethod(out _apiDel.Cursor_IsInsideArea, delegates["Cursor_IsInsideArea"]);
        AssignMethod(out _apiDel.Cursor_GetSprites, delegates["Cursor_GetSprites"]);
        AssignMethod(out _apiDel.Cursor_ForceDispose, delegates["Cursor_ForceDispose"]);
        AssignMethod(out _apiDel.ClickHandler_New, delegates["ClickHandler_New"]);
        AssignMethod(out _apiDel.ClickHandler_GetHitArea, delegates["ClickHandler_GetHitArea"]);
        AssignMethod(out _apiDel.ClickHandler_SetHitArea, delegates["ClickHandler_SetHitArea"]);
        AssignMethod(out _apiDel.ClickHandler_Update, delegates["ClickHandler_Update"]);
        AssignMethod(out _apiDel.ClickHandler_GetMouse1, delegates["ClickHandler_GetMouse1"]);
        AssignMethod(out _apiDel.ClickHandler_GetMouse2, delegates["ClickHandler_GetMouse2"]);
        AssignMethod(out _apiDel.ButtonState_New, delegates["ButtonState_New"]);
        AssignMethod(out _apiDel.ButtonState_IsReleased, delegates["ButtonState_IsReleased"]);
        AssignMethod(out _apiDel.ButtonState_IsOver, delegates["ButtonState_IsOver"]);
        AssignMethod(out _apiDel.ButtonState_IsPressed, delegates["ButtonState_IsPressed"]);
        AssignMethod(out _apiDel.ButtonState_JustReleased, delegates["ButtonState_JustReleased"]);
        AssignMethod(out _apiDel.ButtonState_JustPressed, delegates["ButtonState_JustPressed"]);
        AssignMethod(out _apiDel.ButtonState_Update, delegates["ButtonState_Update"]);
        AssignMethod(out _apiDel.Theme_GetBgColor, delegates["Theme_GetBgColor"]);
        AssignMethod(out _apiDel.Theme_GetWhiteColor, delegates["Theme_GetWhiteColor"]);
        AssignMethod(out _apiDel.Theme_GetMainColor, delegates["Theme_GetMainColor"]);
        AssignMethod(out _apiDel.Theme_GetMainColorDarker, delegates["Theme_GetMainColorDarker"]);
        AssignMethod(out _apiDel.Theme_MeasureStringInPixels, delegates["Theme_MeasureStringInPixels"]);
        AssignMethod(out _apiDel.Theme_GetScale, delegates["Theme_GetScale"]);
        AssignMethod(out _apiDel.Theme_SetScale, delegates["Theme_SetScale"]);
        AssignMethod(out _apiDel.Theme_GetFont, delegates["Theme_GetFont"]);
        AssignMethod(out _apiDel.Theme_SetFont, delegates["Theme_SetFont"]);
        AssignMethod(out _apiDel.ElementBase_GetEnabled, delegates["ElementBase_GetEnabled"]);
        AssignMethod(out _apiDel.ElementBase_SetEnabled, delegates["ElementBase_SetEnabled"]);
        AssignMethod(out _apiDel.ElementBase_GetAbsolute, delegates["ElementBase_GetAbsolute"]);
        AssignMethod(out _apiDel.ElementBase_SetAbsolute, delegates["ElementBase_SetAbsolute"]);
        AssignMethod(out _apiDel.ElementBase_GetSelfAlignment, delegates["ElementBase_GetSelfAlignment"]);
        AssignMethod(out _apiDel.ElementBase_SetSelfAlignment, delegates["ElementBase_SetSelfAlignment"]);
        AssignMethod(out _apiDel.ElementBase_GetPosition, delegates["ElementBase_GetPosition"]);
        AssignMethod(out _apiDel.ElementBase_SetPosition, delegates["ElementBase_SetPosition"]);
        AssignMethod(out _apiDel.ElementBase_GetMargin, delegates["ElementBase_GetMargin"]);
        AssignMethod(out _apiDel.ElementBase_SetMargin, delegates["ElementBase_SetMargin"]);
        AssignMethod(out _apiDel.ElementBase_GetFlex, delegates["ElementBase_GetFlex"]);
        AssignMethod(out _apiDel.ElementBase_SetFlex, delegates["ElementBase_SetFlex"]);
        AssignMethod(out _apiDel.ElementBase_GetPixels, delegates["ElementBase_GetPixels"]);
        AssignMethod(out _apiDel.ElementBase_SetPixels, delegates["ElementBase_SetPixels"]);
        AssignMethod(out _apiDel.ElementBase_GetSize, delegates["ElementBase_GetSize"]);
        AssignMethod(out _apiDel.ElementBase_GetBoundaries, delegates["ElementBase_GetBoundaries"]);
        AssignMethod(out _apiDel.ElementBase_GetApp, delegates["ElementBase_GetApp"]);
        AssignMethod(out _apiDel.ElementBase_GetParent, delegates["ElementBase_GetParent"]);
        AssignMethod(out _apiDel.ElementBase_GetSprites, delegates["ElementBase_GetSprites"]);
        AssignMethod(out _apiDel.ElementBase_ForceUpdate, delegates["ElementBase_ForceUpdate"]);
        AssignMethod(out _apiDel.ElementBase_ForceDispose, delegates["ElementBase_ForceDispose"]);
        AssignMethod(out _apiDel.ElementBase_RegisterUpdate, delegates["ElementBase_RegisterUpdate"]);
        AssignMethod(out _apiDel.ElementBase_UnregisterUpdate, delegates["ElementBase_UnregisterUpdate"]);
        AssignMethod(out _apiDel.ContainerBase_GetChildren, delegates["ContainerBase_GetChildren"]);
        AssignMethod(out _apiDel.ContainerBase_GetFlexSize, delegates["ContainerBase_GetFlexSize"]);
        AssignMethod(out _apiDel.ContainerBase_AddChild, delegates["ContainerBase_AddChild"]);
        AssignMethod(out _apiDel.ContainerBase_AddChildAt, delegates["ContainerBase_AddChildAt"]);
        AssignMethod(out _apiDel.ContainerBase_RemoveChild, delegates["ContainerBase_RemoveChild"]);
        AssignMethod(out _apiDel.ContainerBase_RemoveChildAt, delegates["ContainerBase_RemoveChildAt"]);
        AssignMethod(out _apiDel.ContainerBase_MoveChild, delegates["ContainerBase_MoveChild"]);
        AssignMethod(out _apiDel.View_New, delegates["View_New"]);
        AssignMethod(out _apiDel.View_GetOverflow, delegates["View_GetOverflow"]);
        AssignMethod(out _apiDel.View_SetOverflow, delegates["View_SetOverflow"]);
        AssignMethod(out _apiDel.View_GetDirection, delegates["View_GetDirection"]);
        AssignMethod(out _apiDel.View_SetDirection, delegates["View_SetDirection"]);
        AssignMethod(out _apiDel.View_GetAlignment, delegates["View_GetAlignment"]);
        AssignMethod(out _apiDel.View_SetAlignment, delegates["View_SetAlignment"]);
        AssignMethod(out _apiDel.View_GetAnchor, delegates["View_GetAnchor"]);
        AssignMethod(out _apiDel.View_SetAnchor, delegates["View_SetAnchor"]);
        AssignMethod(out _apiDel.View_GetUseThemeColors, delegates["View_GetUseThemeColors"]);
        AssignMethod(out _apiDel.View_SetUseThemeColors, delegates["View_SetUseThemeColors"]);
        AssignMethod(out _apiDel.View_GetBgColor, delegates["View_GetBgColor"]);
        AssignMethod(out _apiDel.View_SetBgColor, delegates["View_SetBgColor"]);
        AssignMethod(out _apiDel.View_GetBorderColor, delegates["View_GetBorderColor"]);
        AssignMethod(out _apiDel.View_SetBorderColor, delegates["View_SetBorderColor"]);
        AssignMethod(out _apiDel.View_GetBorder, delegates["View_GetBorder"]);
        AssignMethod(out _apiDel.View_SetBorder, delegates["View_SetBorder"]);
        AssignMethod(out _apiDel.View_GetPadding, delegates["View_GetPadding"]);
        AssignMethod(out _apiDel.View_SetPadding, delegates["View_SetPadding"]);
        AssignMethod(out _apiDel.View_GetGap, delegates["View_GetGap"]);
        AssignMethod(out _apiDel.View_SetGap, delegates["View_SetGap"]);
        AssignMethod(out _apiDel.ScrollView_New, delegates["ScrollView_New"]);
        AssignMethod(out _apiDel.ScrollView_GetScroll, delegates["ScrollView_GetScroll"]);
        AssignMethod(out _apiDel.ScrollView_SetScroll, delegates["ScrollView_SetScroll"]);
        AssignMethod(out _apiDel.ScrollView_GetScrollAlwaysVisible, delegates["ScrollView_GetScrollAlwaysVisible"]);
        AssignMethod(out _apiDel.ScrollView_SetScrollAlwaysVisible, delegates["ScrollView_SetScrollAlwaysVisible"]);
        AssignMethod(out _apiDel.ScrollView_GetScrollWheelEnabled, delegates["ScrollView_GetScrollWheelEnabled"]);
        AssignMethod(out _apiDel.ScrollView_SetScrollWheelEnabled, delegates["ScrollView_SetScrollWheelEnabled"]);
        AssignMethod(out _apiDel.ScrollView_GetScrollWheelStep, delegates["ScrollView_GetScrollWheelStep"]);
        AssignMethod(out _apiDel.ScrollView_SetScrollWheelStep, delegates["ScrollView_SetScrollWheelStep"]);
        AssignMethod(out _apiDel.ScrollView_GetScrollBar, delegates["ScrollView_GetScrollBar"]);
        AssignMethod(out _apiDel.TouchApp_New, delegates["TouchApp_New"]);
        AssignMethod(out _apiDel.TouchApp_GetScreen, delegates["TouchApp_GetScreen"]);
        AssignMethod(out _apiDel.TouchApp_GetViewport, delegates["TouchApp_GetViewport"]);
        AssignMethod(out _apiDel.TouchApp_GetCursor, delegates["TouchApp_GetCursor"]);
        AssignMethod(out _apiDel.TouchApp_GetTheme, delegates["TouchApp_GetTheme"]);
        AssignMethod(out _apiDel.TouchApp_GetDefaultBg, delegates["TouchApp_GetDefaultBg"]);
        AssignMethod(out _apiDel.TouchApp_SetDefaultBg, delegates["TouchApp_SetDefaultBg"]);
        AssignMethod(out _apiDel.EmptyButton_New, delegates["EmptyButton_New"]);
        AssignMethod(out _apiDel.EmptyButton_GetHandler, delegates["EmptyButton_GetHandler"]);
        AssignMethod(out _apiDel.EmptyButton_SetOnChange, delegates["EmptyButton_SetOnChange"]);
        AssignMethod(out _apiDel.EmptyButton_GetDisabled, delegates["EmptyButton_GetDisabled"]);
        AssignMethod(out _apiDel.EmptyButton_SetDisabled, delegates["EmptyButton_SetDisabled"]);
        AssignMethod(out _apiDel.Button_New, delegates["Button_New"]);
        AssignMethod(out _apiDel.Button_GetLabel, delegates["Button_GetLabel"]);
        AssignMethod(out _apiDel.Checkbox_New, delegates["Checkbox_New"]);
        AssignMethod(out _apiDel.Checkbox_GetValue, delegates["Checkbox_GetValue"]);
        AssignMethod(out _apiDel.Checkbox_SetValue, delegates["Checkbox_SetValue"]);
        AssignMethod(out _apiDel.Checkbox_SetOnChange, delegates["Checkbox_SetOnChange"]);
        AssignMethod(out _apiDel.Checkbox_GetCheckMark, delegates["Checkbox_GetCheckMark"]);
        AssignMethod(out _apiDel.Label_New, delegates["Label_New"]);
        AssignMethod(out _apiDel.Label_GetAutoBreakLine, delegates["Label_GetAutoBreakLine"]);
        AssignMethod(out _apiDel.Label_SetAutoBreakLine, delegates["Label_SetAutoBreakLine"]);
        AssignMethod(out _apiDel.Label_GetAutoEllipsis, delegates["Label_GetAutoEllipsis"]);
        AssignMethod(out _apiDel.Label_SetAutoEllipsis, delegates["Label_SetAutoEllipsis"]);
        AssignMethod(out _apiDel.Label_GetHasEllipsis, delegates["Label_GetHasEllipsis"]);
        AssignMethod(out _apiDel.Label_GetText, delegates["Label_GetText"]);
        AssignMethod(out _apiDel.Label_SetText, delegates["Label_SetText"]);
        AssignMethod(out _apiDel.Label_GetTextColor, delegates["Label_GetTextColor"]);
        AssignMethod(out _apiDel.Label_SetTextColor, delegates["Label_SetTextColor"]);
        AssignMethod(out _apiDel.Label_GetFontSize, delegates["Label_GetFontSize"]);
        AssignMethod(out _apiDel.Label_SetFontSize, delegates["Label_SetFontSize"]);
        AssignMethod(out _apiDel.Label_GetAlignment, delegates["Label_GetAlignment"]);
        AssignMethod(out _apiDel.Label_SetAlignment, delegates["Label_SetAlignment"]);
        AssignMethod(out _apiDel.Label_GetLines, delegates["Label_GetLines"]);
        AssignMethod(out _apiDel.Label_GetMaxLines, delegates["Label_GetMaxLines"]);
        AssignMethod(out _apiDel.Label_SetMaxLines, delegates["Label_SetMaxLines"]);
        AssignMethod(out _apiDel.BarContainer_New, delegates["BarContainer_New"]);
        AssignMethod(out _apiDel.BarContainer_GetIsVertical, delegates["BarContainer_GetIsVertical"]);
        AssignMethod(out _apiDel.BarContainer_SetIsVertical, delegates["BarContainer_SetIsVertical"]);
        AssignMethod(out _apiDel.BarContainer_GetRatio, delegates["BarContainer_GetRatio"]);
        AssignMethod(out _apiDel.BarContainer_SetRatio, delegates["BarContainer_SetRatio"]);
        AssignMethod(out _apiDel.BarContainer_GetOffset, delegates["BarContainer_GetOffset"]);
        AssignMethod(out _apiDel.BarContainer_SetOffset, delegates["BarContainer_SetOffset"]);
        AssignMethod(out _apiDel.BarContainer_GetBar, delegates["BarContainer_GetBar"]);
        AssignMethod(out _apiDel.ProgressBar_New, delegates["ProgressBar_New"]);
        AssignMethod(out _apiDel.ProgressBar_GetValue, delegates["ProgressBar_GetValue"]);
        AssignMethod(out _apiDel.ProgressBar_SetValue, delegates["ProgressBar_SetValue"]);
        AssignMethod(out _apiDel.ProgressBar_GetMaxValue, delegates["ProgressBar_GetMaxValue"]);
        AssignMethod(out _apiDel.ProgressBar_SetMaxValue, delegates["ProgressBar_SetMaxValue"]);
        AssignMethod(out _apiDel.ProgressBar_GetMinValue, delegates["ProgressBar_GetMinValue"]);
        AssignMethod(out _apiDel.ProgressBar_SetMinValue, delegates["ProgressBar_SetMinValue"]);
        AssignMethod(out _apiDel.ProgressBar_GetBarsGap, delegates["ProgressBar_GetBarsGap"]);
        AssignMethod(out _apiDel.ProgressBar_SetBarsGap, delegates["ProgressBar_SetBarsGap"]);
        AssignMethod(out _apiDel.ProgressBar_GetLabel, delegates["ProgressBar_GetLabel"]);
        AssignMethod(out _apiDel.Selector_New, delegates["Selector_New"]);
        AssignMethod(out _apiDel.Selector_GetLoop, delegates["Selector_GetLoop"]);
        AssignMethod(out _apiDel.Selector_SetLoop, delegates["Selector_SetLoop"]);
        AssignMethod(out _apiDel.Selector_GetSelected, delegates["Selector_GetSelected"]);
        AssignMethod(out _apiDel.Selector_SetSelected, delegates["Selector_SetSelected"]);
        AssignMethod(out _apiDel.Selector_SetOnChange, delegates["Selector_SetOnChange"]);
        AssignMethod(out _apiDel.Slider_New, delegates["Slider_New"]);
        AssignMethod(out _apiDel.Slider_GetMaxValue, delegates["Slider_GetMaxValue"]);
        AssignMethod(out _apiDel.Slider_SetMaxValue, delegates["Slider_SetMaxValue"]);
        AssignMethod(out _apiDel.Slider_GetValue, delegates["Slider_GetValue"]);
        AssignMethod(out _apiDel.Slider_SetValue, delegates["Slider_SetValue"]);
        AssignMethod(out _apiDel.Slider_SetOnChange, delegates["Slider_SetOnChange"]);
        AssignMethod(out _apiDel.Slider_GetIsInteger, delegates["Slider_GetIsInteger"]);
        AssignMethod(out _apiDel.Slider_SetIsInteger, delegates["Slider_SetIsInteger"]);
        AssignMethod(out _apiDel.Slider_GetAllowInput, delegates["Slider_GetAllowInput"]);
        AssignMethod(out _apiDel.Slider_SetAllowInput, delegates["Slider_SetAllowInput"]);
        AssignMethod(out _apiDel.Slider_GetBar, delegates["Slider_GetBar"]);
        AssignMethod(out _apiDel.Slider_GetThumb, delegates["Slider_GetThumb"]);
        AssignMethod(out _apiDel.Slider_GetTextInput, delegates["Slider_GetTextInput"]);
        AssignMethod(out _apiDel.SliderRange_NewR, delegates["SliderRange_NewR"]);
        AssignMethod(out _apiDel.SliderRange_GetValueLower, delegates["SliderRange_GetValueLower"]);
        AssignMethod(out _apiDel.SliderRange_SetValueLower, delegates["SliderRange_SetValueLower"]);
        AssignMethod(out _apiDel.SliderRange_SetOnChangeR, delegates["SliderRange_SetOnChangeR"]);
        AssignMethod(out _apiDel.SliderRange_GetThumbLower, delegates["SliderRange_GetThumbLower"]);
        AssignMethod(out _apiDel.Switch_New, delegates["Switch_New"]);
        AssignMethod(out _apiDel.Switch_GetIndex, delegates["Switch_GetIndex"]);
        AssignMethod(out _apiDel.Switch_SetIndex, delegates["Switch_SetIndex"]);
        AssignMethod(out _apiDel.Switch_GetButtons, delegates["Switch_GetButtons"]);
        AssignMethod(out _apiDel.Switch_SetOnChange, delegates["Switch_SetOnChange"]);
        AssignMethod(out _apiDel.TextField_New, delegates["TextField_New"]);
        AssignMethod(out _apiDel.TextField_GetIsEditing, delegates["TextField_GetIsEditing"]);
        AssignMethod(out _apiDel.TextField_GetText, delegates["TextField_GetText"]);
        AssignMethod(out _apiDel.TextField_SetText, delegates["TextField_SetText"]);
        AssignMethod(out _apiDel.TextField_SetOnSubmit, delegates["TextField_SetOnSubmit"]);
        AssignMethod(out _apiDel.TextField_SetOnBlur, delegates["TextField_SetOnBlur"]);
        AssignMethod(out _apiDel.TextField_GetRevertOnBlur, delegates["TextField_GetRevertOnBlur"]);
        AssignMethod(out _apiDel.TextField_SetRevertOnBlur, delegates["TextField_SetRevertOnBlur"]);
        AssignMethod(out _apiDel.TextField_GetSubmitOnBlur, delegates["TextField_GetSubmitOnBlur"]);
        AssignMethod(out _apiDel.TextField_SetSubmitOnBlur, delegates["TextField_SetSubmitOnBlur"]);
        AssignMethod(out _apiDel.TextField_GetIsNumeric, delegates["TextField_GetIsNumeric"]);
        AssignMethod(out _apiDel.TextField_SetIsNumeric, delegates["TextField_SetIsNumeric"]);
        AssignMethod(out _apiDel.TextField_GetIsInteger, delegates["TextField_GetIsInteger"]);
        AssignMethod(out _apiDel.TextField_SetIsInteger, delegates["TextField_SetIsInteger"]);
        AssignMethod(out _apiDel.TextField_GetAllowNegative, delegates["TextField_GetAllowNegative"]);
        AssignMethod(out _apiDel.TextField_SetAllowNegative, delegates["TextField_SetAllowNegative"]);
        AssignMethod(out _apiDel.TextField_GetLabel, delegates["TextField_GetLabel"]);
        AssignMethod(out _apiDel.TextField_Blur, delegates["TextField_Blur"]);
        AssignMethod(out _apiDel.TextField_Focus, delegates["TextField_Focus"]);
        AssignMethod(out _apiDel.WindowBar_New, delegates["WindowBar_New"]);
        AssignMethod(out _apiDel.WindowBar_GetLabel, delegates["WindowBar_GetLabel"]);
        AssignMethod(out _apiDel.Chart_New, delegates["Chart_New"]);
        AssignMethod(out _apiDel.Chart_GetDataSets, delegates["Chart_GetDataSets"]);
        AssignMethod(out _apiDel.Chart_GetDataColors, delegates["Chart_GetDataColors"]);
        AssignMethod(out _apiDel.Chart_GetGridHorizontalLines, delegates["Chart_GetGridHorizontalLines"]);
        AssignMethod(out _apiDel.Chart_SetGridHorizontalLines, delegates["Chart_SetGridHorizontalLines"]);
        AssignMethod(out _apiDel.Chart_GetGridVerticalLines, delegates["Chart_GetGridVerticalLines"]);
        AssignMethod(out _apiDel.Chart_SetGridVerticalLines, delegates["Chart_SetGridVerticalLines"]);
        AssignMethod(out _apiDel.Chart_GetMaxValue, delegates["Chart_GetMaxValue"]);
        AssignMethod(out _apiDel.Chart_GetMinValue, delegates["Chart_GetMinValue"]);
        AssignMethod(out _apiDel.Chart_GetGridColor, delegates["Chart_GetGridColor"]);
        AssignMethod(out _apiDel.Chart_SetGridColor, delegates["Chart_SetGridColor"]);
        AssignMethod(out _apiDel.EmptyElement_New, delegates["EmptyElement_New"]);
        IsReady = true;
      }
    }
    private void AssignMethod<T>(out T field, object method) => field = (T)method;
  }
  public class UiKitDelegator
  {
    public Func<object, IngameIMyCubeBlock> TouchScreen_GetBlock;
    public Func<object, IngameIMyTextSurface> TouchScreen_GetSurface;
    public Func<object, int> TouchScreen_GetIndex;
    public Func<object, bool> TouchScreen_IsOnScreen;
    public Func<object, object> TouchScreen_GetMouse1;
    public Func<object, object> TouchScreen_GetMouse2;
    public Func<object, Vector2> TouchScreen_GetCursorPosition;
    public Func<object, float> TouchScreen_GetInteractiveDistance;
    public Action<object, float> TouchScreen_SetInteractiveDistance;
    public Func<object, int> TouchScreen_GetRotation;
    public Func<object, IngameIMyCubeBlock, IngameIMyTextSurface, bool> TouchScreen_CompareWithBlockAndSurface;
    public Action<object> TouchScreen_ForceDispose;
    public Func<object, object> Cursor_New;
    public Func<object, bool> Cursor_GetEnabled;
    public Action<object, bool> Cursor_SetEnabled;
    public Func<object, float> Cursor_GetScale;
    public Action<object, float> Cursor_SetScale;
    public Func<object, Vector2> Cursor_GetPosition;
    public Func<object, float, float, float, float, bool> Cursor_IsInsideArea;
    public Func<object, List<MySprite>> Cursor_GetSprites;
    public Action<object> Cursor_ForceDispose;
    public Func<object> ClickHandler_New;
    public Func<object, Vector4> ClickHandler_GetHitArea;
    public Action<object, Vector4> ClickHandler_SetHitArea;
    public Action<object, object> ClickHandler_Update;
    public Func<object, object> ClickHandler_GetMouse1;
    public Func<object, object> ClickHandler_GetMouse2;
    public Func<object> ButtonState_New;
    public Func<object, bool> ButtonState_IsReleased;
    public Func<object, bool> ButtonState_IsOver;
    public Func<object, bool> ButtonState_IsPressed;
    public Func<object, bool> ButtonState_JustReleased;
    public Func<object, bool> ButtonState_JustPressed;
    public Action<object, bool, bool> ButtonState_Update;
    public Func<object, Color> Theme_GetBgColor;
    public Func<object, Color> Theme_GetWhiteColor;
    public Func<object, Color> Theme_GetMainColor;
    public Func<object, int, Color> Theme_GetMainColorDarker;
    public Func<object, string, string, float, Vector2> Theme_MeasureStringInPixels;
    public Func<object, float> Theme_GetScale;
    public Action<object, float> Theme_SetScale;
    public Func<object, string> Theme_GetFont;
    public Action<object, string> Theme_SetFont;
    public Func<object, bool> ElementBase_GetEnabled;
    public Action<object, bool> ElementBase_SetEnabled;
    public Func<object, bool> ElementBase_GetAbsolute;
    public Action<object, bool> ElementBase_SetAbsolute;
    public Func<object, byte> ElementBase_GetSelfAlignment;
    public Action<object, byte> ElementBase_SetSelfAlignment;
    public Func<object, Vector2> ElementBase_GetPosition;
    public Action<object, Vector2> ElementBase_SetPosition;
    public Func<object, Vector4> ElementBase_GetMargin;
    public Action<object, Vector4> ElementBase_SetMargin;
    public Func<object, Vector2> ElementBase_GetFlex;
    public Action<object, Vector2> ElementBase_SetFlex;
    public Func<object, Vector2> ElementBase_GetPixels;
    public Action<object, Vector2> ElementBase_SetPixels;
    public Func<object, Vector2> ElementBase_GetSize;
    public Func<object, Vector2> ElementBase_GetBoundaries;
    public Func<object, object> ElementBase_GetApp;
    public Func<object, object> ElementBase_GetParent;
    public Func<object, List<MySprite>> ElementBase_GetSprites;
    public Action<object> ElementBase_ForceUpdate;
    public Action<object> ElementBase_ForceDispose;
    public Action<object, Action> ElementBase_RegisterUpdate;
    public Action<object, Action> ElementBase_UnregisterUpdate;
    public Func<object, List<object>> ContainerBase_GetChildren;
    public Func<object, Vector2> ContainerBase_GetFlexSize;
    public Action<object, object> ContainerBase_AddChild;
    public Action<object, object, int> ContainerBase_AddChildAt;
    public Action<object, object> ContainerBase_RemoveChild;
    public Action<object, int> ContainerBase_RemoveChildAt;
    public Action<object, object, int> ContainerBase_MoveChild;
    public Func<byte, Color?, object> View_New;
    public Func<object, bool> View_GetOverflow;
    public Action<object, bool> View_SetOverflow;
    public Func<object, byte> View_GetDirection;
    public Action<object, byte> View_SetDirection;
    public Func<object, byte> View_GetAlignment;
    public Action<object, byte> View_SetAlignment;
    public Func<object, byte> View_GetAnchor;
    public Action<object, byte> View_SetAnchor;
    public Func<object, bool> View_GetUseThemeColors;
    public Action<object, bool> View_SetUseThemeColors;
    public Func<object, Color> View_GetBgColor;
    public Action<object, Color> View_SetBgColor;
    public Func<object, Color> View_GetBorderColor;
    public Action<object, Color> View_SetBorderColor;
    public Func<object, Vector4> View_GetBorder;
    public Action<object, Vector4> View_SetBorder;
    public Func<object, Vector4> View_GetPadding;
    public Action<object, Vector4> View_SetPadding;
    public Func<object, int> View_GetGap;
    public Action<object, int> View_SetGap;
    public Func<int, Color?, object> ScrollView_New;
    public Func<object, float> ScrollView_GetScroll;
    public Action<object, float> ScrollView_SetScroll;
    public Func<object, bool> ScrollView_GetScrollAlwaysVisible;
    public Action<object, bool> ScrollView_SetScrollAlwaysVisible;
    public Func<object, bool> ScrollView_GetScrollWheelEnabled;
    public Action<object, bool> ScrollView_SetScrollWheelEnabled;
    public Func<object, float> ScrollView_GetScrollWheelStep;
    public Action<object, float> ScrollView_SetScrollWheelStep;
    public Func<object, object> ScrollView_GetScrollBar;
    public Func<IngameIMyCubeBlock, IngameIMyTextSurface, object> TouchApp_New;
    public Func<object, object> TouchApp_GetScreen;
    public Func<object, RectangleF> TouchApp_GetViewport;
    public Func<object, object> TouchApp_GetCursor;
    public Func<object, object> TouchApp_GetTheme;
    public Func<object, bool> TouchApp_GetDefaultBg;
    public Action<object, bool> TouchApp_SetDefaultBg;
    public Func<Action, object> EmptyButton_New;
    public Func<object, object> EmptyButton_GetHandler;
    public Action<object, Action> EmptyButton_SetOnChange;
    public Func<object, bool> EmptyButton_GetDisabled;
    public Action<object, bool> EmptyButton_SetDisabled;
    public Func<string, Action, object> Button_New;
    public Func<object, object> Button_GetLabel;
    public Func<Action<bool>, bool, object> Checkbox_New;
    public Func<object, bool> Checkbox_GetValue;
    public Action<object, bool> Checkbox_SetValue;
    public Action<object, Action<bool>> Checkbox_SetOnChange;
    public Func<object, object> Checkbox_GetCheckMark;
    public Func<string, float, TextAlignment, object> Label_New;
    public Func<object, bool> Label_GetAutoBreakLine;
    public Action<object, bool> Label_SetAutoBreakLine;
    public Func<object, byte> Label_GetAutoEllipsis;
    public Action<object, byte> Label_SetAutoEllipsis;
    public Func<object, bool> Label_GetHasEllipsis;
    public Func<object, string> Label_GetText;
    public Action<object, string> Label_SetText;
    public Func<object, Color?> Label_GetTextColor;
    public Action<object, Color> Label_SetTextColor;
    public Func<object, float> Label_GetFontSize;
    public Action<object, float> Label_SetFontSize;
    public Func<object, TextAlignment> Label_GetAlignment;
    public Action<object, TextAlignment> Label_SetAlignment;
    public Func<object, int> Label_GetLines;
    public Func<object, int> Label_GetMaxLines;
    public Action<object, int> Label_SetMaxLines;
    public Func<bool, object> BarContainer_New;
    public Func<object, bool> BarContainer_GetIsVertical;
    public Action<object, bool> BarContainer_SetIsVertical;
    public Func<object, float> BarContainer_GetRatio;
    public Action<object, float> BarContainer_SetRatio;
    public Func<object, float> BarContainer_GetOffset;
    public Action<object, float> BarContainer_SetOffset;
    public Func<object, object> BarContainer_GetBar;
    public Func<float, float, bool, float, object> ProgressBar_New;
    public Func<object, float> ProgressBar_GetValue;
    public Action<object, float> ProgressBar_SetValue;
    public Func<object, float> ProgressBar_GetMaxValue;
    public Action<object, float> ProgressBar_SetMaxValue;
    public Func<object, float> ProgressBar_GetMinValue;
    public Action<object, float> ProgressBar_SetMinValue;
    public Func<object, float> ProgressBar_GetBarsGap;
    public Action<object, float> ProgressBar_SetBarsGap;
    public Func<object, object> ProgressBar_GetLabel;
    public Func<List<string>, Action<int, string>, bool, object> Selector_New;
    public Func<object, bool> Selector_GetLoop;
    public Action<object, bool> Selector_SetLoop;
    public Func<object, int> Selector_GetSelected;
    public Action<object, int> Selector_SetSelected;
    public Action<object, Action<int, string>> Selector_SetOnChange;
    public Func<float, float, Action<float>, object> Slider_New;
    public Func<object, float> Slider_GetMaxValue;
    public Action<object, float> Slider_SetMaxValue;
    public Func<object, float> Slider_GetMinValue;
    public Action<object, float> Slider_SetMinValue;
    public Func<object, float> Slider_GetValue;
    public Action<object, float> Slider_SetValue;
    public Action<object, Action<float>> Slider_SetOnChange;
    public Func<object, bool> Slider_GetIsInteger;
    public Action<object, bool> Slider_SetIsInteger;
    public Func<object, bool> Slider_GetAllowInput;
    public Action<object, bool> Slider_SetAllowInput;
    public Func<object, object> Slider_GetBar;
    public Func<object, object> Slider_GetThumb;
    public Func<object, object> Slider_GetTextInput;
    public Func<float, float, Action<float, float>, object> SliderRange_NewR;
    public Func<object, float> SliderRange_GetValueLower;
    public Action<object, float> SliderRange_SetValueLower;
    public Action<object, Action<float, float>> SliderRange_SetOnChangeR;
    public Func<object, object> SliderRange_GetThumbLower;
    public Func<string[], int, Action<int>, object> Switch_New;
    public Func<object, int> Switch_GetIndex;
    public Action<object, int> Switch_SetIndex;
    public Func<object, object[]> Switch_GetButtons;
    public Action<object, Action<int>> Switch_SetOnChange;
    public Func<object> TextField_New;
    public Func<object, bool> TextField_GetIsEditing;
    public Func<object, string> TextField_GetText;
    public Action<object, string> TextField_SetText;
    public Action<object, Action<string>> TextField_SetOnSubmit;
    public Action<object, Action<string>> TextField_SetOnBlur;
    public Func<object, bool> TextField_GetRevertOnBlur;
    public Action<object, bool> TextField_SetRevertOnBlur;
    public Func<object, bool> TextField_GetSubmitOnBlur;
    public Action<object, bool> TextField_SetSubmitOnBlur;
    public Func<object, bool> TextField_GetIsNumeric;
    public Action<object, bool> TextField_SetIsNumeric;
    public Func<object, bool> TextField_GetIsInteger;
    public Action<object, bool> TextField_SetIsInteger;
    public Func<object, bool> TextField_GetAllowNegative;
    public Action<object, bool> TextField_SetAllowNegative;
    public Func<object, object> TextField_GetLabel;
    public Action<object> TextField_Blur;
    public Action<object> TextField_Focus;
    public Func<string, object> WindowBar_New;
    public Func<object, object> WindowBar_GetLabel;
    public Func<int, object> Chart_New;
    public Func<object, List<float[]>> Chart_GetDataSets;
    public Func<object, List<Color>> Chart_GetDataColors;
    public Func<object, int> Chart_GetGridHorizontalLines;
    public Action<object, int> Chart_SetGridHorizontalLines;
    public Func<object, int> Chart_GetGridVerticalLines;
    public Action<object, int> Chart_SetGridVerticalLines;
    public Func<object, float> Chart_GetMaxValue;
    public Func<object, float> Chart_GetMinValue;
    public Func<object, Color?> Chart_GetGridColor;
    public Action<object, Color> Chart_SetGridColor;
    public Func<object> EmptyElement_New;
  }
  public abstract class WrapperBase<TT> where TT : UiKitDelegator
  {
    static protected TT Api;
    internal static void SetApi(TT api) => Api = api;
    static protected T Wrap<T>(object obj, Func<object, T> ctor) { return (obj == null) ? default(T) : ctor(obj); }
    static protected T[] WrapArray<T>(object[] objArray, Func<object, T> ctor)
    {
      var newArray = new T[objArray.Length];
      for (int i = 0; i < objArray.Length; i++)
        newArray[i] = Wrap<T>(objArray[i], ctor);
      return newArray;
    }
    internal object InternalObj { get; private set; }
    public WrapperBase(object internalObject) { InternalObj = internalObject; }
  }
  public class TouchScreen : WrapperBase<UiKitDelegator>
  {
    private ButtonState _mouse1;
    private ButtonState _mouse2;
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchScreen(object internalObject) : base(internalObject) { }
    public IngameIMyCubeBlock Block { get { return Api.TouchScreen_GetBlock.Invoke(InternalObj); } }
    public IngameIMyTextSurface Surface { get { return Api.TouchScreen_GetSurface.Invoke(InternalObj); } }
    public int Index { get { return Api.TouchScreen_GetIndex.Invoke(InternalObj); } }
    public bool IsOnScreen { get { return Api.TouchScreen_IsOnScreen.Invoke(InternalObj); } }
    public ButtonState Mouse1 { get { return _mouse1 ?? (_mouse1 = Wrap<ButtonState>(Api.TouchScreen_GetMouse1.Invoke(InternalObj), (obj) => new ButtonState(obj))); } }
    public ButtonState Mouse2 { get { return _mouse2 ?? (_mouse2 = Wrap<ButtonState>(Api.TouchScreen_GetMouse2.Invoke(InternalObj), (obj) => new ButtonState(obj))); } }
    public Vector2 CursorPosition { get { return Api.TouchScreen_GetCursorPosition.Invoke(InternalObj); } }
    public float InteractiveDistance { get { return Api.TouchScreen_GetInteractiveDistance.Invoke(InternalObj); } }
    public void SetInteractiveDistance(float distance) => Api.TouchScreen_SetInteractiveDistance.Invoke(InternalObj, distance);
    public int Rotation { get { return Api.TouchScreen_GetRotation.Invoke(InternalObj); } }
    public bool CompareWithBlockAndSurface(IngameIMyCubeBlock block, IngameIMyTextSurface surface) => Api.TouchScreen_CompareWithBlockAndSurface.Invoke(InternalObj, block, surface);
    /// <summary>
    /// Force a call to Cursor Dispose, that clears sprites.
    /// </summary>
    public void ForceDispose() => Api.TouchScreen_ForceDispose.Invoke(InternalObj);
  }
  public class Cursor : WrapperBase<UiKitDelegator>
  {
    public Cursor(TouchScreen screen) : base(Api.Cursor_New(screen.InternalObj)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public Cursor(object internalObject) : base(internalObject) { }
    public bool Enabled { get { return Api.Cursor_GetEnabled.Invoke(InternalObj); } set { Api.Cursor_SetEnabled.Invoke(InternalObj, value); } }
    public float Scale { get { return Api.Cursor_GetScale.Invoke(InternalObj); } set { Api.Cursor_SetScale.Invoke(InternalObj, value); } }
    public Vector2 Position { get { return Api.Cursor_GetPosition.Invoke(InternalObj); } }
    public bool IsInsideArea(float x, float y, float z, float w) => Api.Cursor_IsInsideArea.Invoke(InternalObj, x, y, z, w);
    public List<MySprite> GetSprites() => Api.Cursor_GetSprites.Invoke(InternalObj);
    public void ForceDispose() => Api.Cursor_ForceDispose.Invoke(InternalObj);
  }
  public class ClickHandler : WrapperBase<UiKitDelegator>
  {
    private ButtonState _mouse1;
    private ButtonState _mouse2;
    public ClickHandler() : base(Api.ClickHandler_New()) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public ClickHandler(object internalObject) : base(internalObject) { }
    /// <summary>
    /// A Vector4 representing the area on screen that should check for cursor position.
    /// </summary>
    public Vector4 HitArea { get { return Api.ClickHandler_GetHitArea.Invoke(InternalObj); } set { Api.ClickHandler_SetHitArea.Invoke(InternalObj, value); } }
    /// <summary>
    /// This is already called internally by the Touch Manager, only call this if you wanna override the handler status.
    /// </summary>
    public void Update(TouchScreen screen) => Api.ClickHandler_Update.Invoke(InternalObj, screen.InternalObj);
    public ButtonState Mouse1 { get { return _mouse1 ?? (_mouse1 = Wrap<ButtonState>(Api.ClickHandler_GetMouse1.Invoke(InternalObj), (obj) => new ButtonState(obj))); } }
    public ButtonState Mouse2 { get { return _mouse2 ?? (_mouse2 = Wrap<ButtonState>(Api.ClickHandler_GetMouse2.Invoke(InternalObj), (obj) => new ButtonState(obj))); } }
  }
  public class ButtonState : WrapperBase<UiKitDelegator>
  {
    public ButtonState() : base(Api.ButtonState_New()) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public ButtonState(object internalObject) : base(internalObject) { }
    public bool IsReleased { get { return Api.ButtonState_IsReleased.Invoke(InternalObj); } }
    public bool IsOver { get { return Api.ButtonState_IsOver.Invoke(InternalObj); } }
    public bool IsPressed { get { return Api.ButtonState_IsPressed.Invoke(InternalObj); } }
    public bool JustReleased { get { return Api.ButtonState_JustReleased.Invoke(InternalObj); } }
    public bool JustPressed { get { return Api.ButtonState_JustPressed.Invoke(InternalObj); } }
    public void Update(bool isPressed, bool isInsideArea) => Api.ButtonState_Update.Invoke(InternalObj, isPressed, isInsideArea);
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Theme.cs"/>
  /// </summary>
  public class Theme : WrapperBase<UiKitDelegator>
  {
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public Theme(object internalObject) : base(internalObject) { }
    public Color BgColor { get { return Api.Theme_GetBgColor.Invoke(InternalObj); } }
    /// <summary>
    /// This is a high contras color related to main color, it is not exactly a white.
    /// Also can be blacksh if the background color is too light.
    /// </summary>
    public Color WhiteColor { get { return Api.Theme_GetWhiteColor.Invoke(InternalObj); } }
    public Color MainColor { get { return Api.Theme_GetMainColor.Invoke(InternalObj); } }
    /// <summary>
    /// This gets a darker version of the main color pre calculated on the Theme. Lower numbers are darker.
    /// </summary>
    /// <param name="value">One of the options: 1, 2, 3 , 4, 5, 6, 7, 8, 9</param>
    /// <returns>The calculated color</returns>
    public Color GetMainColorDarker(int value) => Api.Theme_GetMainColorDarker.Invoke(InternalObj, value);
    /// <summary>
    /// Scales the entire App and all its elements, useful for small screens. Can be called at any time.
    /// </summary>
    public float Scale { get { return Api.Theme_GetScale.Invoke(InternalObj); } set { Api.Theme_SetScale.Invoke(InternalObj, value); } }
    public string Font { get { return Api.Theme_GetFont.Invoke(InternalObj); } set { Api.Theme_SetFont.Invoke(InternalObj, value); } }
    /// <summary>
    /// Helper to calculate the width of a text on screen.
    /// </summary>
    /// <returns>A Vector2 with width and height.</returns>
    public Vector2 MeasureStringInPixels(string text, string font, float scale) => Api.Theme_MeasureStringInPixels.Invoke(InternalObj, text, font, scale);
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/ElementBase.cs"/>
  /// </summary>
  public abstract class ElementBase : WrapperBase<UiKitDelegator>
  {
    private TouchApp _app;
    private View _parent;
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public ElementBase(object internalObject) : base(internalObject) { }
    /// <summary>
    /// If false, the element will not be drawn, useful if you want to hide and show but not destroy. Better than removing it.
    /// </summary>
    public bool Enabled { get { return Api.ElementBase_GetEnabled.Invoke(InternalObj); } set { Api.ElementBase_SetEnabled.Invoke(InternalObj, value); } }
    /// <summary>
    /// If true, the element will not align and anchor with the parent. Its position will be related to the screen and size not counted for parent inner size.
    /// </summary>
    public bool Absolute { get { return Api.ElementBase_GetAbsolute.Invoke(InternalObj); } set { Api.ElementBase_SetAbsolute.Invoke(InternalObj, value); } }
    /// <summary>
    /// Controls the aligment of the element on the crossed axis of parent <see cref="View.Direction" />. Useful for overriding parent's Aligment.
    /// </summary>
    public ViewAlignment SelfAlignment { get { return (ViewAlignment)Api.ElementBase_GetSelfAlignment.Invoke(InternalObj); } set { Api.ElementBase_SetSelfAlignment.Invoke(InternalObj, (byte)value); } }
    /// <summary>
    /// Position of the element related to screen. This is overriden by the parent if the element's <see cref="Absolute" /> is not true.
    /// </summary>
    public Vector2 Position { get { return Api.ElementBase_GetPosition.Invoke(InternalObj); } set { Api.ElementBase_SetPosition.Invoke(InternalObj, value); } }
    /// <summary>
    /// Margin values for four sides. Starting from Left, Top, Right and Bottom.
    /// </summary>
    public Vector4 Margin { get { return Api.ElementBase_GetMargin.Invoke(InternalObj); } set { Api.ElementBase_SetMargin.Invoke(InternalObj, value); } }
    /// <summary>
    /// The ratio of the parent that this element should fill. 1 means 100%. If the parent has more children, the proportional % will be applied.
    /// This is stackable with <see cref="Pixels" />. So set as 0 the axis if you just want a fixed pixels size.
    /// </summary>
    public Vector2 Flex { get { return Api.ElementBase_GetFlex.Invoke(InternalObj); } set { Api.ElementBase_SetFlex.Invoke(InternalObj, value); } }
    /// <summary>
    /// Fixed size in pixels, not related to parent.
    /// This is stackable with <see cref="Flex" />. So set as 0 the axis if you just want the size only related to parent.
    /// </summary>
    public Vector2 Pixels { get { return Api.ElementBase_GetPixels.Invoke(InternalObj); } set { Api.ElementBase_SetPixels.Invoke(InternalObj, value); } }
    /// <summary>
    /// The <see cref="App" /> that this element was added. Be careful, this is null until the element is properly added.
    /// </summary>
    public TouchApp App { get { return _app ?? (_app = Wrap<TouchApp>(Api.ElementBase_GetApp.Invoke(InternalObj), (obj) => new TouchApp(obj))); } }
    /// <summary>
    /// The immediate parent of this element.
    /// </summary>
    public View Parent { get { return _parent ?? (_parent = Wrap<View>(Api.ElementBase_GetParent.Invoke(InternalObj), (obj) => new View(obj))); } }
    /// <returns>Reference to thes Sprites of this element, if it is a container it also has the children Sprites</returns>
    public List<MySprite> GetSprites() => Api.ElementBase_GetSprites.Invoke(InternalObj);
    /// <returns>The calculated final size of the element in pixels. Usefull to position Absolute children.</returns>
    public Vector2 GetSize() => Api.ElementBase_GetSize.Invoke(InternalObj);
    /// <returns>The calculated final size and if it is a container also the border and padding.</returns>
    public Vector2 GetBoundaries() => Api.ElementBase_GetBoundaries.Invoke(InternalObj);
    /// <summary>
    /// Forces a call to Update method for the internal object. The method is already called from Touch Manager. Only call this if you want to force another call.
    /// </summary>
    public void ForceUpdate() => Api.ElementBase_ForceUpdate.Invoke(InternalObj);
    /// <summary>
    /// Forces a call to Dispose method for the internal object. The method is already called from Touch Manager when the App is Disposed.
    /// Only call this for the App instance, or if you want to force another call.
    /// </summary>
    public void ForceDispose() => Api.ElementBase_ForceDispose.Invoke(InternalObj);
    /// <summary>
    /// Register a delegate to be called when the internal object Update event is called.
    /// </summary>
    public void RegisterUpdate(Action update) => Api.ElementBase_RegisterUpdate.Invoke(InternalObj, update);
    /// <summary>
    /// Unregister a delegate. Recommended to be called on object dispose.
    /// </summary>
    public void UnregisterUpdate(Action update) => Api.ElementBase_UnregisterUpdate.Invoke(InternalObj, update);
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/ContainerBase.cs"/>
  /// </summary>
  public abstract class ContainerBase : ElementBase
  {
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public ContainerBase(object internalObject) : base(internalObject) { }
    public List<object> Children { get { return Api.ContainerBase_GetChildren.Invoke(InternalObj); } }
    /// <returns>The calculated remaining size inside the container. Negative when the children sizes are bigger.</returns>
    public Vector2 GetFlexSize() => Api.ContainerBase_GetFlexSize.Invoke(InternalObj);
    public void AddChild(ElementBase child) => Api.ContainerBase_AddChild.Invoke(InternalObj, child.InternalObj);
    public void AddChild(ElementBase child, int index) => Api.ContainerBase_AddChildAt.Invoke(InternalObj, child.InternalObj, index);
    public void RemoveChild(ElementBase child) => Api.ContainerBase_RemoveChild.Invoke(InternalObj, child.InternalObj);
    public void RemoveChild(object child) => Api.ContainerBase_RemoveChild.Invoke(InternalObj, child);
    public void RemoveChild(int index) => Api.ContainerBase_RemoveChildAt.Invoke(InternalObj, index);
    public void MoveChild(ElementBase child, int index) => Api.ContainerBase_MoveChild.Invoke(InternalObj, child.InternalObj, index);
  }
  public enum ViewDirection : byte { None = 0, Row = 1, Column = 2, RowReverse = 3, ColumnReverse = 4 }
  public enum ViewAlignment : byte { Start = 0, Center = 1, End = 2 }
  public enum ViewAnchor : byte { Start = 0, Center = 1, End = 2, SpaceBetween = 3, SpaceAround = 4 }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/View.cs"/>
  /// </summary>
  public class View : ContainerBase
  {
    public View(ViewDirection direction = ViewDirection.Column, Color? bgColor = null) : base(Api.View_New((byte)direction, bgColor)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public View(object internalObject) : base(internalObject) { }
    /// <summary>
    /// If false, children outside inner size will be hidden.
    /// </summary>
    public bool Overflow { get { return Api.View_GetOverflow.Invoke(InternalObj); } set { Api.View_SetOverflow.Invoke(InternalObj, value); } }
    public ViewDirection Direction { get { return (ViewDirection)Api.View_GetDirection.Invoke(InternalObj); } set { Api.View_SetDirection.Invoke(InternalObj, (byte)value); } }
    /// <summary>
    /// The aligment of children on the crossed axis of the <see cref="Direction" />.
    /// </summary>
    public ViewAlignment Alignment { get { return (ViewAlignment)Api.View_GetAlignment.Invoke(InternalObj); } set { Api.View_SetAlignment.Invoke(InternalObj, (byte)value); } }
    /// <summary>
    /// The anchor position of the children on the same axis of the <see cref="Direction" />.
    /// </summary>
    public ViewAnchor Anchor { get { return (ViewAnchor)Api.View_GetAnchor.Invoke(InternalObj); } set { Api.View_SetAnchor.Invoke(InternalObj, (byte)value); } }
    /// <summary>
    /// If false, the element will not update colors with the App.Theme. Useful for overriding element themes.
    /// </summary>
    public bool UseThemeColors { get { return Api.View_GetUseThemeColors.Invoke(InternalObj); } set { Api.View_SetUseThemeColors.Invoke(InternalObj, value); } }
    public Color BgColor { get { return Api.View_GetBgColor.Invoke(InternalObj); } set { Api.View_SetBgColor.Invoke(InternalObj, value); } }
    public Color BorderColor { get { return Api.View_GetBorderColor.Invoke(InternalObj); } set { Api.View_SetBorderColor.Invoke(InternalObj, value); } }
    /// <summary>
    /// Border values for four sides. Starting from Left, Top, Right and Bottom.
    /// </summary>
    public Vector4 Border { get { return Api.View_GetBorder.Invoke(InternalObj); } set { Api.View_SetBorder.Invoke(InternalObj, value); } }
    /// <summary>
    /// Padding values for four sides. Starting from Left, Top, Right and Bottom.
    /// </summary>
    public Vector4 Padding { get { return Api.View_GetPadding.Invoke(InternalObj); } set { Api.View_SetPadding.Invoke(InternalObj, value); } }
    /// <summary>
    /// Adds a spacing between children. Better than adding margin to each child, if the same spacing is needed.
    /// </summary>
    public int Gap { get { return Api.View_GetGap.Invoke(InternalObj); } set { Api.View_SetGap.Invoke(InternalObj, value); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/ScrollView.cs"/>
  /// </summary>
  public class ScrollView : View
  {
    private BarContainer _scrollBar;
    public ScrollView(ViewDirection direction = ViewDirection.Column, Color? bgColor = null) : base(Api.ScrollView_New((int)direction, bgColor)) { }
    /// <summary>
    /// Ratio from 0 to 1.
    /// </summary>
    public float Scroll { get { return Api.ScrollView_GetScroll.Invoke(InternalObj); } set { Api.ScrollView_SetScroll.Invoke(InternalObj, value); } }
    public bool ScrollAlwaysVisible { get { return Api.ScrollView_GetScrollAlwaysVisible.Invoke(InternalObj); } set { Api.ScrollView_SetScrollAlwaysVisible.Invoke(InternalObj, value); } }
    public bool ScrollWheelEnabled { get { return Api.ScrollView_GetScrollWheelEnabled.Invoke(InternalObj); } set { Api.ScrollView_SetScrollWheelEnabled.Invoke(InternalObj, value); } }
    public float ScrollWheelStep { get { return Api.ScrollView_GetScrollWheelStep.Invoke(InternalObj); } set { Api.ScrollView_SetScrollWheelStep.Invoke(InternalObj, value); } }
    public BarContainer ScrollBar { get { return _scrollBar ?? (_scrollBar = Wrap<BarContainer>(Api.ScrollView_GetScrollBar.Invoke(InternalObj), (obj) => new BarContainer(obj))); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TouchApp.cs"/>
  /// </summary>
  public class TouchApp : View
  {
    private TouchScreen _screen;
    private Cursor _cursor;
    private Theme _theme;
    /// <summary>
    /// Instantiates the app, recommended to be called after a few seconds when used on a TSS.
    /// Can return null if the block and surface are not ready for TouchScreen, catch any exceptions.
    /// </summary>
    public TouchApp(IngameIMyCubeBlock block, IngameIMyTextSurface surface) : base(Api.TouchApp_New(block, surface)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchApp(object internalObject) : base(internalObject) { }
    public TouchScreen Screen { get { return _screen ?? (_screen = Wrap<TouchScreen>(Api.TouchApp_GetScreen.Invoke(InternalObj), (obj) => new TouchScreen(obj))); } }
    public RectangleF Viewport { get { return Api.TouchApp_GetViewport.Invoke(InternalObj); } }
    public Cursor Cursor { get { return _cursor ?? (_cursor = Wrap<Cursor>(Api.TouchApp_GetCursor.Invoke(InternalObj), (obj) => new Cursor(obj))); } }
    public Theme Theme { get { return _theme ?? (_theme = Wrap<Theme>(Api.TouchApp_GetTheme.Invoke(InternalObj), (obj) => new Theme(obj))); } }
    /// <summary>
    /// If true, the app will present a nice background image.
    /// </summary>
    public bool DefaultBg { get { return Api.TouchApp_GetDefaultBg.Invoke(InternalObj); } set { Api.TouchApp_SetDefaultBg.Invoke(InternalObj, value); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/EmptyButton.cs"/>
  /// </summary>
  public class EmptyButton : View
  {
    private ClickHandler _handler;
    public EmptyButton(Action onChange) : base(Api.EmptyButton_New(onChange)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public EmptyButton(object internalObject) : base(internalObject) { }
    public ClickHandler Handler { get { return _handler ?? (_handler = Wrap<ClickHandler>(Api.EmptyButton_GetHandler.Invoke(InternalObj), (obj) => new ClickHandler(obj))); } }
    public Action OnChange { set { Api.EmptyButton_SetOnChange.Invoke(InternalObj, value); } }
    /// <summary>
    /// If true, the button will not be clickable and will not fire the onChange event.
    /// </summary>
    public bool Disabled { get { return Api.EmptyButton_GetDisabled.Invoke(InternalObj); } set { Api.EmptyButton_SetDisabled.Invoke(InternalObj, value); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/Button.cs"/>
  /// </summary>
  public class Button : EmptyButton
  {
    private Label _label;
    public Button(string text, Action onChange) : base(Api.Button_New(text, onChange)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public Button(object internalObject) : base(internalObject) { }
    public Label Label { get { return _label ?? (_label = Wrap<Label>(Api.Button_GetLabel.Invoke(InternalObj), (obj) => new Label(obj))); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/Checkbox.cs"/>
  /// </summary>
  public class Checkbox : View
  {
    private EmptyElement _checkMark;
    public Checkbox(Action<bool> onChange, bool value = false) : base(Api.Checkbox_New(onChange, value)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public Checkbox(object internalObject) : base(internalObject) { }
    public bool Value { get { return Api.Checkbox_GetValue.Invoke(InternalObj); } set { Api.Checkbox_SetValue.Invoke(InternalObj, value); } }
    public Action<bool> OnChange { set { Api.Checkbox_SetOnChange.Invoke(InternalObj, value); } }
    public EmptyElement CheckMark { get { return _checkMark ?? (_checkMark = Wrap<EmptyElement>(Api.Checkbox_GetCheckMark.Invoke(InternalObj), (obj) => new EmptyElement(obj))); } }
  }
  public enum LabelEllipsis : byte { None = 0, Left = 1, Right = 2 }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/Label.cs"/>
  /// </summary>
  public class Label : ElementBase
  {
    public Label(string text, float fontSize = 0.5f, TextAlignment alignment = TextAlignment.CENTER) : base(Api.Label_New(text, fontSize, alignment)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public Label(object internalObject) : base(internalObject) { }
    public bool AutoBreakLine { get { return Api.Label_GetAutoBreakLine.Invoke(InternalObj); } set { Api.Label_SetAutoBreakLine.Invoke(InternalObj, value); } }
    /// <summary>
    /// If true, text will not be shortened if bigger than size.
    /// </summary>
    public LabelEllipsis AutoEllipsis { get { return (LabelEllipsis)Api.Label_GetAutoEllipsis.Invoke(InternalObj); } set { Api.Label_SetAutoEllipsis.Invoke(InternalObj, (byte)value); } }
    /// <summary>
    /// If <see cref="AutoEllipsis" /> is false and the text was limited to fit the size and added an ellipsis.
    /// </summary>
    public bool HasEllipsis { get { return Api.Label_GetHasEllipsis.Invoke(InternalObj); } }
    public string Text { get { return Api.Label_GetText.Invoke(InternalObj); } set { Api.Label_SetText.Invoke(InternalObj, value); } }
    public Color? TextColor { get { return Api.Label_GetTextColor.Invoke(InternalObj); } set { Api.Label_SetTextColor.Invoke(InternalObj, (Color)value); } }
    public float FontSize { get { return Api.Label_GetFontSize.Invoke(InternalObj); } set { Api.Label_SetFontSize.Invoke(InternalObj, value); } }
    public TextAlignment Alignment { get { return Api.Label_GetAlignment.Invoke(InternalObj); } set { Api.Label_SetAlignment.Invoke(InternalObj, value); } }
    public int Lines { get { return Api.Label_GetLines.Invoke(InternalObj); } }
    public int MaxLines { get { return Api.Label_GetMaxLines.Invoke(InternalObj); } set { Api.Label_SetMaxLines.Invoke(InternalObj, value); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/BarContainer.cs"/>
  /// </summary>
  public class BarContainer : View
  {
    private View _bar;
    public BarContainer(bool vertical = false) : base(Api.BarContainer_New(vertical)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public BarContainer(object internalObject) : base(internalObject) { }
    public bool IsVertical { get { return Api.BarContainer_GetIsVertical.Invoke(InternalObj); } set { Api.BarContainer_SetIsVertical.Invoke(InternalObj, value); } }
    /// <summary>
    /// Ratio from 0 to 1. Relative size of the inner bar.
    /// </summary>
    public float Ratio { get { return Api.BarContainer_GetRatio.Invoke(InternalObj); } set { Api.BarContainer_SetRatio.Invoke(InternalObj, value); } }
    /// <summary>
    /// Ratio from 0 to 1. Relative position of the inner bar. Limited by the remaining space of the container.
    /// </summary>
    public float Offset { get { return Api.BarContainer_GetOffset.Invoke(InternalObj); } set { Api.BarContainer_SetOffset.Invoke(InternalObj, value); } }
    public View Bar { get { return _bar ?? (_bar = Wrap<View>(Api.BarContainer_GetBar.Invoke(InternalObj), (obj) => new View(obj))); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/ProgressBar.cs"/>
  /// </summary>
  public class ProgressBar : BarContainer
  {
    private Label _label;
    public ProgressBar(float min, float max, bool vertical = false, float barsGap = 0) : base(Api.ProgressBar_New(min, max, vertical, barsGap)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public ProgressBar(object internalObject) : base(internalObject) { }
    public float Value { get { return Api.ProgressBar_GetValue.Invoke(InternalObj); } set { Api.ProgressBar_SetValue.Invoke(InternalObj, value); } }
    public float MaxValue { get { return Api.ProgressBar_GetMaxValue.Invoke(InternalObj); } set { Api.ProgressBar_SetMaxValue.Invoke(InternalObj, value); } }
    public float MinValue { get { return Api.ProgressBar_GetMinValue.Invoke(InternalObj); } set { Api.ProgressBar_SetMinValue.Invoke(InternalObj, value); } }
    public float BarsGap { get { return Api.ProgressBar_GetBarsGap.Invoke(InternalObj); } set { Api.ProgressBar_SetBarsGap.Invoke(InternalObj, value); } }
    public Label Label { get { return _label ?? (_label = Wrap<Label>(Api.ProgressBar_GetLabel.Invoke(InternalObj), (obj) => new Label(obj))); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/Selector.cs"/>
  /// </summary>
  public class Selector : View
  {
    public Selector(List<string> labels, Action<int, string> onChange, bool loop = true) : base(Api.Selector_New(labels, onChange, loop)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public Selector(object internalObject) : base(internalObject) { }
    public bool Loop { get { return Api.Selector_GetLoop.Invoke(InternalObj); } set { Api.Selector_SetLoop.Invoke(InternalObj, value); } }
    public int Selected { get { return Api.Selector_GetSelected.Invoke(InternalObj); } set { Api.Selector_SetSelected.Invoke(InternalObj, value); } }
    public Action<int, string> OnChange { set { Api.Selector_SetOnChange.Invoke(InternalObj, value); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/Slider.cs"/>
  /// </summary>
  public class Slider : View
  {
    private BarContainer _bar;
    private EmptyElement _thumb;
    private TextField _textInput;
    public Slider(float min, float max, Action<float> onChange) : base(Api.Slider_New(min, max, onChange)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public Slider(object internalObject) : base(internalObject) { }
    public float MaxValue { get { return Api.Slider_GetMaxValue.Invoke(InternalObj); } set { Api.Slider_SetMaxValue.Invoke(InternalObj, value); } }
    public float MinValue { get { return Api.Slider_GetMinValue.Invoke(InternalObj); } set { Api.Slider_SetMinValue.Invoke(InternalObj, value); } }
    public float Value { get { return Api.Slider_GetValue.Invoke(InternalObj); } set { Api.Slider_SetValue.Invoke(InternalObj, value); } }
    public bool IsInteger { get { return Api.Slider_GetIsInteger.Invoke(InternalObj); } set { Api.Slider_SetIsInteger.Invoke(InternalObj, value); } }
    /// <summary>
    /// If true, user can Hold Ctrl and click to manually type a number.
    /// </summary>
    public bool AllowInput { get { return Api.Slider_GetAllowInput.Invoke(InternalObj); } set { Api.Slider_SetAllowInput.Invoke(InternalObj, value); } }
    public Action<float> OnChange { set { Api.Slider_SetOnChange.Invoke(InternalObj, value); } }
    public BarContainer Bar { get { return _bar ?? (_bar = Wrap<BarContainer>(Api.Slider_GetBar.Invoke(InternalObj), (obj) => new BarContainer(obj))); } }
    public EmptyElement Thumb { get { return _thumb ?? (_thumb = Wrap<EmptyElement>(Api.Slider_GetThumb.Invoke(InternalObj), (obj) => new EmptyElement(obj))); } }
    public TextField TextInput { get { return _textInput ?? (_textInput = Wrap<TextField>(Api.Slider_GetTextInput.Invoke(InternalObj), (obj) => new TextField(obj))); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/SliderRange.cs"/>
  /// </summary>
  public class SliderRange : Slider
  {
    private EmptyElement _thumbLower;
    public SliderRange(float min, float max, Action<float, float> onChange) : base(Api.SliderRange_NewR(min, max, onChange)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public SliderRange(object internalObject) : base(internalObject) { }
    public float ValueLower { get { return Api.SliderRange_GetValueLower.Invoke(InternalObj); } set { Api.SliderRange_SetValueLower.Invoke(InternalObj, value); } }
    public Action<float, float> OnChangeRange { set { Api.SliderRange_SetOnChangeR.Invoke(InternalObj, value); } }
    public EmptyElement ThumbLower { get { return _thumbLower ?? (_thumbLower = Wrap<EmptyElement>(Api.SliderRange_GetThumbLower.Invoke(InternalObj), (obj) => new EmptyElement(obj))); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/Switch.cs"/>
  /// </summary>
  public class Switch : View
  {
    private Button[] _buttons;
    public Switch(string[] labels, int index = 0, Action<int> onChange = null) : base(Api.Switch_New(labels, index, onChange)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public Switch(object internalObject) : base(internalObject) { }
    public int Index { get { return Api.Switch_GetIndex.Invoke(InternalObj); } set { Api.Switch_SetIndex.Invoke(InternalObj, value); } }
    public Button[] Buttons { get { return _buttons ?? (_buttons = WrapArray<Button>(Api.Switch_GetButtons.Invoke(InternalObj), (obj) => new Button(obj))); } }
    public Action<int> OnChange { set { Api.Switch_SetOnChange.Invoke(InternalObj, value); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TextField.cs"/>
  /// </summary>
  public class TextField : View
  {
    private Label _label;
    public TextField() : base(Api.TextField_New()) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TextField(object internalObject) : base(internalObject) { }
    public bool IsEditing { get { return Api.TextField_GetIsEditing.Invoke(InternalObj); } }
    public string Text { get { return Api.TextField_GetText.Invoke(InternalObj); } set { Api.TextField_SetText.Invoke(InternalObj, value); } }
    public bool IsNumeric { get { return Api.TextField_GetIsNumeric.Invoke(InternalObj); } set { Api.TextField_SetIsNumeric.Invoke(InternalObj, value); } }
    public bool IsInteger { get { return Api.TextField_GetIsInteger.Invoke(InternalObj); } set { Api.TextField_SetIsInteger.Invoke(InternalObj, value); } }
    public bool AllowNegative { get { return Api.TextField_GetAllowNegative.Invoke(InternalObj); } set { Api.TextField_SetAllowNegative.Invoke(InternalObj, value); } }
    public Label Label { get { return _label ?? (_label = Wrap<Label>(Api.TextField_GetLabel.Invoke(InternalObj), (obj) => new Label(obj))); } }
    public Action<string> OnSubmit { set { Api.TextField_SetOnSubmit.Invoke(InternalObj, value); } }
    public bool RevertOnBlur { get { return Api.TextField_GetRevertOnBlur.Invoke(InternalObj); } set { Api.TextField_SetRevertOnBlur.Invoke(InternalObj, value); } }
    public bool SubmitOnBlur { get { return Api.TextField_GetSubmitOnBlur.Invoke(InternalObj); } set { Api.TextField_SetSubmitOnBlur.Invoke(InternalObj, value); } }
    public void Blur() => Api.TextField_Blur.Invoke(InternalObj);
    public void Focus() => Api.TextField_Focus.Invoke(InternalObj);
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/WindowBar.cs"/>
  /// </summary>
  public class WindowBar : View
  {
    private Label _label;
    public WindowBar(string text) : base(Api.WindowBar_New(text)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public WindowBar(object internalObject) : base(internalObject) { }
    public Label Label { get { return _label ?? (_label = Wrap<Label>(Api.WindowBar_GetLabel.Invoke(InternalObj), (obj) => new Label(obj))); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/Chart.cs"/>
  /// </summary>
  public class Chart : ElementBase
  {
    public Chart(int intervals) : base(Api.Chart_New(intervals)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public Chart(object internalObject) : base(internalObject) { }
    public List<float[]> DataSets { get { return Api.Chart_GetDataSets.Invoke(InternalObj); } }
    public List<Color> DataColors { get { return Api.Chart_GetDataColors.Invoke(InternalObj); } }
    public int GridHorizontalLines { get { return Api.Chart_GetGridHorizontalLines.Invoke(InternalObj); } set { Api.Chart_SetGridHorizontalLines.Invoke(InternalObj, value); } }
    public int GridVerticalLines { get { return Api.Chart_GetGridVerticalLines.Invoke(InternalObj); } set { Api.Chart_SetGridVerticalLines.Invoke(InternalObj, value); } }
    public float MaxValue { get { return Api.Chart_GetMaxValue.Invoke(InternalObj); } }
    public float MinValue { get { return Api.Chart_GetMinValue.Invoke(InternalObj); } }
    public Color? GridColor { get { return Api.Chart_GetGridColor.Invoke(InternalObj); } set { Api.Chart_SetGridColor.Invoke(InternalObj, (Color)value); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/EmptyElement.cs"/>
  /// </summary>
  public class EmptyElement : ElementBase
  {
    public EmptyElement() : base(Api.EmptyElement_New()) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public EmptyElement(object internalObject) : base(internalObject) { }
  }
}
