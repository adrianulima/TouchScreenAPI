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
    private TouchUiKitDelegator _apiDel;
    public TouchUiKit(Sandbox.ModAPI.Ingame.IMyTerminalBlock pb)
    {
      var delegates = pb.GetProperty("2668820525")?.As<IReadOnlyDictionary<string, Delegate>>().GetValue(pb);
      if (delegates != null)
      {
        _apiDel = new TouchUiKitDelegator();
        WrapperBase<TouchUiKitDelegator>.SetApi(_apiDel);
        AssignMethod(out _createTouchScreen, delegates["CreateTouchScreen"]);
        AssignMethod(out _removeTouchScreen, delegates["RemoveTouchScreen"]);
        AssignMethod(out _addSurfaceCoords, delegates["AddSurfaceCoords"]);
        AssignMethod(out _removeSurfaceCoords, delegates["RemoveSurfaceCoords"]);
        AssignMethod(out _apiDel.TouchScreen_GetBlock, delegates["TouchScreen_GetBlock"]);
        AssignMethod(out _apiDel.TouchScreen_GetSurface, delegates["TouchScreen_GetSurface"]);
        AssignMethod(out _apiDel.TouchScreen_GetIndex, delegates["TouchScreen_GetIndex"]);
        AssignMethod(out _apiDel.TouchScreen_IsOnScreen, delegates["TouchScreen_IsOnScreen"]);
        AssignMethod(out _apiDel.TouchScreen_GetCursorPosition, delegates["TouchScreen_GetCursorPosition"]);
        AssignMethod(out _apiDel.TouchScreen_GetInteractiveDistance, delegates["TouchScreen_GetInteractiveDistance"]);
        AssignMethod(out _apiDel.TouchScreen_SetInteractiveDistance, delegates["TouchScreen_SetInteractiveDistance"]);
        AssignMethod(out _apiDel.TouchScreen_GetRotation, delegates["TouchScreen_GetRotation"]);
        AssignMethod(out _apiDel.TouchScreen_CompareWithBlockAndSurface, delegates["TouchScreen_CompareWithBlockAndSurface"]);
        AssignMethod(out _apiDel.TouchScreen_ForceDispose, delegates["TouchScreen_ForceDispose"]);
        AssignMethod(out _apiDel.TouchCursor_New, delegates["TouchCursor_New"]);
        AssignMethod(out _apiDel.TouchCursor_GetActive, delegates["TouchCursor_GetActive"]);
        AssignMethod(out _apiDel.TouchCursor_SetActive, delegates["TouchCursor_SetActive"]);
        AssignMethod(out _apiDel.TouchCursor_GetScale, delegates["TouchCursor_GetScale"]);
        AssignMethod(out _apiDel.TouchCursor_SetScale, delegates["TouchCursor_SetScale"]);
        AssignMethod(out _apiDel.TouchCursor_GetPosition, delegates["TouchCursor_GetPosition"]);
        AssignMethod(out _apiDel.TouchCursor_IsInsideArea, delegates["TouchCursor_IsInsideArea"]);
        AssignMethod(out _apiDel.TouchCursor_GetSprites, delegates["TouchCursor_GetSprites"]);
        AssignMethod(out _apiDel.TouchCursor_ForceDispose, delegates["TouchCursor_ForceDispose"]);
        AssignMethod(out _apiDel.ClickHandler_New, delegates["ClickHandler_New"]);
        AssignMethod(out _apiDel.ClickHandler_GetHitArea, delegates["ClickHandler_GetHitArea"]);
        AssignMethod(out _apiDel.ClickHandler_SetHitArea, delegates["ClickHandler_SetHitArea"]);
        AssignMethod(out _apiDel.ClickHandler_IsMouseReleased, delegates["ClickHandler_IsMouseReleased"]);
        AssignMethod(out _apiDel.ClickHandler_IsMouseOver, delegates["ClickHandler_IsMouseOver"]);
        AssignMethod(out _apiDel.ClickHandler_IsMousePressed, delegates["ClickHandler_IsMousePressed"]);
        AssignMethod(out _apiDel.ClickHandler_JustReleased, delegates["ClickHandler_JustReleased"]);
        AssignMethod(out _apiDel.ClickHandler_JustPressed, delegates["ClickHandler_JustPressed"]);
        AssignMethod(out _apiDel.ClickHandler_UpdateStatus, delegates["ClickHandler_UpdateStatus"]);
        AssignMethod(out _apiDel.TouchTheme_GetBgColor, delegates["TouchTheme_GetBgColor"]);
        AssignMethod(out _apiDel.TouchTheme_GetWhiteColor, delegates["TouchTheme_GetWhiteColor"]);
        AssignMethod(out _apiDel.TouchTheme_GetMainColor, delegates["TouchTheme_GetMainColor"]);
        AssignMethod(out _apiDel.TouchTheme_GetMainColorDarker, delegates["TouchTheme_GetMainColorDarker"]);
        AssignMethod(out _apiDel.TouchTheme_MeasureStringInPixels, delegates["TouchTheme_MeasureStringInPixels"]);
        AssignMethod(out _apiDel.TouchTheme_GetScale, delegates["TouchTheme_GetScale"]);
        AssignMethod(out _apiDel.TouchTheme_SetScale, delegates["TouchTheme_SetScale"]);
        AssignMethod(out _apiDel.TouchTheme_GetFont, delegates["TouchTheme_GetFont"]);
        AssignMethod(out _apiDel.TouchTheme_SetFont, delegates["TouchTheme_SetFont"]);
        AssignMethod(out _apiDel.TouchElementBase_GetEnabled, delegates["TouchElementBase_GetEnabled"]);
        AssignMethod(out _apiDel.TouchElementBase_SetEnabled, delegates["TouchElementBase_SetEnabled"]);
        AssignMethod(out _apiDel.TouchElementBase_GetAbsolute, delegates["TouchElementBase_GetAbsolute"]);
        AssignMethod(out _apiDel.TouchElementBase_SetAbsolute, delegates["TouchElementBase_SetAbsolute"]);
        AssignMethod(out _apiDel.TouchElementBase_GetSelfAlignment, delegates["TouchElementBase_GetSelfAlignment"]);
        AssignMethod(out _apiDel.TouchElementBase_SetSelfAlignment, delegates["TouchElementBase_SetSelfAlignment"]);
        AssignMethod(out _apiDel.TouchElementBase_GetPosition, delegates["TouchElementBase_GetPosition"]);
        AssignMethod(out _apiDel.TouchElementBase_SetPosition, delegates["TouchElementBase_SetPosition"]);
        AssignMethod(out _apiDel.TouchElementBase_GetMargin, delegates["TouchElementBase_GetMargin"]);
        AssignMethod(out _apiDel.TouchElementBase_SetMargin, delegates["TouchElementBase_SetMargin"]);
        AssignMethod(out _apiDel.TouchElementBase_GetScale, delegates["TouchElementBase_GetScale"]);
        AssignMethod(out _apiDel.TouchElementBase_SetScale, delegates["TouchElementBase_SetScale"]);
        AssignMethod(out _apiDel.TouchElementBase_GetPixels, delegates["TouchElementBase_GetPixels"]);
        AssignMethod(out _apiDel.TouchElementBase_SetPixels, delegates["TouchElementBase_SetPixels"]);
        AssignMethod(out _apiDel.TouchElementBase_GetSize, delegates["TouchElementBase_GetSize"]);
        AssignMethod(out _apiDel.TouchElementBase_GetBoundaries, delegates["TouchElementBase_GetBoundaries"]);
        AssignMethod(out _apiDel.TouchElementBase_GetApp, delegates["TouchElementBase_GetApp"]);
        AssignMethod(out _apiDel.TouchElementBase_GetParent, delegates["TouchElementBase_GetParent"]);
        AssignMethod(out _apiDel.TouchElementBase_GetSprites, delegates["TouchElementBase_GetSprites"]);
        AssignMethod(out _apiDel.TouchElementBase_ForceUpdate, delegates["TouchElementBase_ForceUpdate"]);
        AssignMethod(out _apiDel.TouchElementBase_ForceDispose, delegates["TouchElementBase_ForceDispose"]);
        AssignMethod(out _apiDel.TouchElementBase_RegisterUpdate, delegates["TouchElementBase_RegisterUpdate"]);
        AssignMethod(out _apiDel.TouchElementBase_UnregisterUpdate, delegates["TouchElementBase_UnregisterUpdate"]);
        AssignMethod(out _apiDel.TouchContainerBase_GetChildren, delegates["TouchContainerBase_GetChildren"]);
        AssignMethod(out _apiDel.TouchContainerBase_GetFlexSize, delegates["TouchContainerBase_GetFlexSize"]);
        AssignMethod(out _apiDel.TouchContainerBase_AddChild, delegates["TouchContainerBase_AddChild"]);
        AssignMethod(out _apiDel.TouchContainerBase_AddChildAt, delegates["TouchContainerBase_AddChildAt"]);
        AssignMethod(out _apiDel.TouchContainerBase_RemoveChild, delegates["TouchContainerBase_RemoveChild"]);
        AssignMethod(out _apiDel.TouchContainerBase_RemoveChildAt, delegates["TouchContainerBase_RemoveChildAt"]);
        AssignMethod(out _apiDel.TouchContainerBase_MoveChild, delegates["TouchContainerBase_MoveChild"]);
        AssignMethod(out _apiDel.TouchView_New, delegates["TouchView_New"]);
        AssignMethod(out _apiDel.TouchView_GetOverflow, delegates["TouchView_GetOverflow"]);
        AssignMethod(out _apiDel.TouchView_SetOverflow, delegates["TouchView_SetOverflow"]);
        AssignMethod(out _apiDel.TouchView_GetDirection, delegates["TouchView_GetDirection"]);
        AssignMethod(out _apiDel.TouchView_SetDirection, delegates["TouchView_SetDirection"]);
        AssignMethod(out _apiDel.TouchView_GetAlignment, delegates["TouchView_GetAlignment"]);
        AssignMethod(out _apiDel.TouchView_SetAlignment, delegates["TouchView_SetAlignment"]);
        AssignMethod(out _apiDel.TouchView_GetAnchor, delegates["TouchView_GetAnchor"]);
        AssignMethod(out _apiDel.TouchView_SetAnchor, delegates["TouchView_SetAnchor"]);
        AssignMethod(out _apiDel.TouchView_GetUseThemeColors, delegates["TouchView_GetUseThemeColors"]);
        AssignMethod(out _apiDel.TouchView_SetUseThemeColors, delegates["TouchView_SetUseThemeColors"]);
        AssignMethod(out _apiDel.TouchView_GetBgColor, delegates["TouchView_GetBgColor"]);
        AssignMethod(out _apiDel.TouchView_SetBgColor, delegates["TouchView_SetBgColor"]);
        AssignMethod(out _apiDel.TouchView_GetBorderColor, delegates["TouchView_GetBorderColor"]);
        AssignMethod(out _apiDel.TouchView_SetBorderColor, delegates["TouchView_SetBorderColor"]);
        AssignMethod(out _apiDel.TouchView_GetBorder, delegates["TouchView_GetBorder"]);
        AssignMethod(out _apiDel.TouchView_SetBorder, delegates["TouchView_SetBorder"]);
        AssignMethod(out _apiDel.TouchView_GetPadding, delegates["TouchView_GetPadding"]);
        AssignMethod(out _apiDel.TouchView_SetPadding, delegates["TouchView_SetPadding"]);
        AssignMethod(out _apiDel.TouchView_GetGap, delegates["TouchView_GetGap"]);
        AssignMethod(out _apiDel.TouchView_SetGap, delegates["TouchView_SetGap"]);
        AssignMethod(out _apiDel.TouchScrollView_New, delegates["TouchScrollView_New"]);
        AssignMethod(out _apiDel.TouchScrollView_GetScroll, delegates["TouchScrollView_GetScroll"]);
        AssignMethod(out _apiDel.TouchScrollView_SetScroll, delegates["TouchScrollView_SetScroll"]);
        AssignMethod(out _apiDel.TouchScrollView_GetScrollAlwaysVisible, delegates["TouchScrollView_GetScrollAlwaysVisible"]);
        AssignMethod(out _apiDel.TouchScrollView_SetScrollAlwaysVisible, delegates["TouchScrollView_SetScrollAlwaysVisible"]);
        AssignMethod(out _apiDel.TouchScrollView_GetScrollBar, delegates["TouchScrollView_GetScrollBar"]);
        AssignMethod(out _apiDel.TouchApp_New, delegates["TouchApp_New"]);
        AssignMethod(out _apiDel.TouchApp_GetScreen, delegates["TouchApp_GetScreen"]);
        AssignMethod(out _apiDel.TouchApp_GetViewport, delegates["TouchApp_GetViewport"]);
        AssignMethod(out _apiDel.TouchApp_GetCursor, delegates["TouchApp_GetCursor"]);
        AssignMethod(out _apiDel.TouchApp_GetTheme, delegates["TouchApp_GetTheme"]);
        AssignMethod(out _apiDel.TouchApp_GetDefaultBg, delegates["TouchApp_GetDefaultBg"]);
        AssignMethod(out _apiDel.TouchApp_SetDefaultBg, delegates["TouchApp_SetDefaultBg"]);
        AssignMethod(out _apiDel.TouchApp_InitApp, delegates["TouchApp_InitApp"]);
        AssignMethod(out _apiDel.TouchEmptyButton_New, delegates["TouchEmptyButton_New"]);
        AssignMethod(out _apiDel.TouchEmptyButton_GetHandler, delegates["TouchEmptyButton_GetHandler"]);
        AssignMethod(out _apiDel.TouchEmptyButton_SetOnChange, delegates["TouchEmptyButton_SetOnChange"]);
        AssignMethod(out _apiDel.TouchButton_New, delegates["TouchButton_New"]);
        AssignMethod(out _apiDel.TouchButton_GetLabel, delegates["TouchButton_GetLabel"]);
        AssignMethod(out _apiDel.TouchCheckbox_New, delegates["TouchCheckbox_New"]);
        AssignMethod(out _apiDel.TouchCheckbox_GetValue, delegates["TouchCheckbox_GetValue"]);
        AssignMethod(out _apiDel.TouchCheckbox_SetValue, delegates["TouchCheckbox_SetValue"]);
        AssignMethod(out _apiDel.TouchCheckbox_SetOnChange, delegates["TouchCheckbox_SetOnChange"]);
        AssignMethod(out _apiDel.TouchCheckbox_GetCheckMark, delegates["TouchCheckbox_GetCheckMark"]);
        AssignMethod(out _apiDel.TouchLabel_New, delegates["TouchLabel_New"]);
        AssignMethod(out _apiDel.TouchLabel_GetAutoBreakLine, delegates["TouchLabel_GetAutoBreakLine"]);
        AssignMethod(out _apiDel.TouchLabel_SetAutoBreakLine, delegates["TouchLabel_SetAutoBreakLine"]);
        AssignMethod(out _apiDel.TouchLabel_GetAutoEllipsis, delegates["TouchLabel_GetAutoEllipsis"]);
        AssignMethod(out _apiDel.TouchLabel_SetAutoEllipsis, delegates["TouchLabel_SetAutoEllipsis"]);
        AssignMethod(out _apiDel.TouchLabel_GetHasEllipsis, delegates["TouchLabel_GetHasEllipsis"]);
        AssignMethod(out _apiDel.TouchLabel_GetText, delegates["TouchLabel_GetText"]);
        AssignMethod(out _apiDel.TouchLabel_SetText, delegates["TouchLabel_SetText"]);
        AssignMethod(out _apiDel.TouchLabel_GetTextColor, delegates["TouchLabel_GetTextColor"]);
        AssignMethod(out _apiDel.TouchLabel_SetTextColor, delegates["TouchLabel_SetTextColor"]);
        AssignMethod(out _apiDel.TouchLabel_GetFontSize, delegates["TouchLabel_GetFontSize"]);
        AssignMethod(out _apiDel.TouchLabel_SetFontSize, delegates["TouchLabel_SetFontSize"]);
        AssignMethod(out _apiDel.TouchLabel_GetAlignment, delegates["TouchLabel_GetAlignment"]);
        AssignMethod(out _apiDel.TouchLabel_SetAlignment, delegates["TouchLabel_SetAlignment"]);
        AssignMethod(out _apiDel.TouchBarContainer_New, delegates["TouchBarContainer_New"]);
        AssignMethod(out _apiDel.TouchBarContainer_GetIsVertical, delegates["TouchBarContainer_GetIsVertical"]);
        AssignMethod(out _apiDel.TouchBarContainer_SetIsVertical, delegates["TouchBarContainer_SetIsVertical"]);
        AssignMethod(out _apiDel.TouchBarContainer_GetRatio, delegates["TouchBarContainer_GetRatio"]);
        AssignMethod(out _apiDel.TouchBarContainer_SetRatio, delegates["TouchBarContainer_SetRatio"]);
        AssignMethod(out _apiDel.TouchBarContainer_GetOffset, delegates["TouchBarContainer_GetOffset"]);
        AssignMethod(out _apiDel.TouchBarContainer_SetOffset, delegates["TouchBarContainer_SetOffset"]);
        AssignMethod(out _apiDel.TouchBarContainer_GetBar, delegates["TouchBarContainer_GetBar"]);
        AssignMethod(out _apiDel.TouchProgressBar_New, delegates["TouchProgressBar_New"]);
        AssignMethod(out _apiDel.TouchProgressBar_GetValue, delegates["TouchProgressBar_GetValue"]);
        AssignMethod(out _apiDel.TouchProgressBar_SetValue, delegates["TouchProgressBar_SetValue"]);
        AssignMethod(out _apiDel.TouchProgressBar_GetMaxValue, delegates["TouchProgressBar_GetMaxValue"]);
        AssignMethod(out _apiDel.TouchProgressBar_SetMaxValue, delegates["TouchProgressBar_SetMaxValue"]);
        AssignMethod(out _apiDel.TouchProgressBar_GetMinValue, delegates["TouchProgressBar_GetMinValue"]);
        AssignMethod(out _apiDel.TouchProgressBar_SetMinValue, delegates["TouchProgressBar_SetMinValue"]);
        AssignMethod(out _apiDel.TouchProgressBar_GetBarsGap, delegates["TouchProgressBar_GetBarsGap"]);
        AssignMethod(out _apiDel.TouchProgressBar_SetBarsGap, delegates["TouchProgressBar_SetBarsGap"]);
        AssignMethod(out _apiDel.TouchProgressBar_GetLabel, delegates["TouchProgressBar_GetLabel"]);
        AssignMethod(out _apiDel.TouchSelector_New, delegates["TouchSelector_New"]);
        AssignMethod(out _apiDel.TouchSelector_GetLoop, delegates["TouchSelector_GetLoop"]);
        AssignMethod(out _apiDel.TouchSelector_SetLoop, delegates["TouchSelector_SetLoop"]);
        AssignMethod(out _apiDel.TouchSelector_GetSelected, delegates["TouchSelector_GetSelected"]);
        AssignMethod(out _apiDel.TouchSelector_SetSelected, delegates["TouchSelector_SetSelected"]);
        AssignMethod(out _apiDel.TouchSelector_SetOnChange, delegates["TouchSelector_SetOnChange"]);
        AssignMethod(out _apiDel.TouchSlider_New, delegates["TouchSlider_New"]);
        AssignMethod(out _apiDel.TouchSlider_GetMaxValue, delegates["TouchSlider_GetMaxValue"]);
        AssignMethod(out _apiDel.TouchSlider_SetMaxValue, delegates["TouchSlider_SetMaxValue"]);
        AssignMethod(out _apiDel.TouchSlider_GetValue, delegates["TouchSlider_GetValue"]);
        AssignMethod(out _apiDel.TouchSlider_SetValue, delegates["TouchSlider_SetValue"]);
        AssignMethod(out _apiDel.TouchSlider_SetOnChange, delegates["TouchSlider_SetOnChange"]);
        AssignMethod(out _apiDel.TouchSlider_GetIsInteger, delegates["TouchSlider_GetIsInteger"]);
        AssignMethod(out _apiDel.TouchSlider_SetIsInteger, delegates["TouchSlider_SetIsInteger"]);
        AssignMethod(out _apiDel.TouchSlider_GetAllowInput, delegates["TouchSlider_GetAllowInput"]);
        AssignMethod(out _apiDel.TouchSlider_SetAllowInput, delegates["TouchSlider_SetAllowInput"]);
        AssignMethod(out _apiDel.TouchSlider_GetBar, delegates["TouchSlider_GetBar"]);
        AssignMethod(out _apiDel.TouchSlider_GetThumb, delegates["TouchSlider_GetThumb"]);
        AssignMethod(out _apiDel.TouchSlider_GetTextInput, delegates["TouchSlider_GetTextInput"]);
        AssignMethod(out _apiDel.TouchSliderRange_NewR, delegates["TouchSliderRange_NewR"]);
        AssignMethod(out _apiDel.TouchSliderRange_GetValueLower, delegates["TouchSliderRange_GetValueLower"]);
        AssignMethod(out _apiDel.TouchSliderRange_SetValueLower, delegates["TouchSliderRange_SetValueLower"]);
        AssignMethod(out _apiDel.TouchSliderRange_SetOnChangeR, delegates["TouchSliderRange_SetOnChangeR"]);
        AssignMethod(out _apiDel.TouchSliderRange_GetThumbLower, delegates["TouchSliderRange_GetThumbLower"]);
        AssignMethod(out _apiDel.TouchSwitch_New, delegates["TouchSwitch_New"]);
        AssignMethod(out _apiDel.TouchSwitch_GetIndex, delegates["TouchSwitch_GetIndex"]);
        AssignMethod(out _apiDel.TouchSwitch_SetIndex, delegates["TouchSwitch_SetIndex"]);
        AssignMethod(out _apiDel.TouchSwitch_GetButtons, delegates["TouchSwitch_GetButtons"]);
        AssignMethod(out _apiDel.TouchSwitch_SetOnChange, delegates["TouchSwitch_SetOnChange"]);
        AssignMethod(out _apiDel.TouchTextField_New, delegates["TouchTextField_New"]);
        AssignMethod(out _apiDel.TouchTextField_GetIsEditing, delegates["TouchTextField_GetIsEditing"]);
        AssignMethod(out _apiDel.TouchTextField_GetText, delegates["TouchTextField_GetText"]);
        AssignMethod(out _apiDel.TouchTextField_SetText, delegates["TouchTextField_SetText"]);
        AssignMethod(out _apiDel.TouchTextField_SetOnChange, delegates["TouchTextField_SetOnChange"]);
        AssignMethod(out _apiDel.TouchTextField_GetIsNumeric, delegates["TouchTextField_GetIsNumeric"]);
        AssignMethod(out _apiDel.TouchTextField_SetIsNumeric, delegates["TouchTextField_SetIsNumeric"]);
        AssignMethod(out _apiDel.TouchTextField_GetIsInteger, delegates["TouchTextField_GetIsInteger"]);
        AssignMethod(out _apiDel.TouchTextField_SetIsInteger, delegates["TouchTextField_SetIsInteger"]);
        AssignMethod(out _apiDel.TouchTextField_GetAllowNegative, delegates["TouchTextField_GetAllowNegative"]);
        AssignMethod(out _apiDel.TouchTextField_SetAllowNegative, delegates["TouchTextField_SetAllowNegative"]);
        AssignMethod(out _apiDel.TouchTextField_GetLabel, delegates["TouchTextField_GetLabel"]);
        AssignMethod(out _apiDel.TouchWindowBar_New, delegates["TouchWindowBar_New"]);
        AssignMethod(out _apiDel.TouchWindowBar_GetLabel, delegates["TouchWindowBar_GetLabel"]);
        AssignMethod(out _apiDel.TouchChart_New, delegates["TouchChart_New"]);
        AssignMethod(out _apiDel.TouchChart_GetDataSets, delegates["TouchChart_GetDataSets"]);
        AssignMethod(out _apiDel.TouchChart_GetDataColors, delegates["TouchChart_GetDataColors"]);
        AssignMethod(out _apiDel.TouchChart_GetGridHorizontalLines, delegates["TouchChart_GetGridHorizontalLines"]);
        AssignMethod(out _apiDel.TouchChart_SetGridHorizontalLines, delegates["TouchChart_SetGridHorizontalLines"]);
        AssignMethod(out _apiDel.TouchChart_GetGridVerticalLines, delegates["TouchChart_GetGridVerticalLines"]);
        AssignMethod(out _apiDel.TouchChart_SetGridVerticalLines, delegates["TouchChart_SetGridVerticalLines"]);
        AssignMethod(out _apiDel.TouchChart_GetMaxValue, delegates["TouchChart_GetMaxValue"]);
        AssignMethod(out _apiDel.TouchChart_GetMinValue, delegates["TouchChart_GetMinValue"]);
        AssignMethod(out _apiDel.TouchChart_GetGridColor, delegates["TouchChart_GetGridColor"]);
        AssignMethod(out _apiDel.TouchChart_SetGridColor, delegates["TouchChart_SetGridColor"]);
        AssignMethod(out _apiDel.TouchEmptyElement_New, delegates["TouchEmptyElement_New"]);
        IsReady = true;
      }
    }
    private void AssignMethod<T>(out T field, object method) => field = (T)method;
  }
  public class TouchUiKitDelegator
  {
    public Func<object, IngameIMyCubeBlock> TouchScreen_GetBlock;
    public Func<object, IngameIMyTextSurface> TouchScreen_GetSurface;
    public Func<object, int> TouchScreen_GetIndex;
    public Func<object, bool> TouchScreen_IsOnScreen;
    public Func<object, Vector2> TouchScreen_GetCursorPosition;
    public Func<object, float> TouchScreen_GetInteractiveDistance;
    public Action<object, float> TouchScreen_SetInteractiveDistance;
    public Func<object, int> TouchScreen_GetRotation;
    public Func<object, IngameIMyCubeBlock, IngameIMyTextSurface, bool> TouchScreen_CompareWithBlockAndSurface;
    public Action<object> TouchScreen_ForceDispose;
    public Func<object, object> TouchCursor_New;
    public Func<object, bool> TouchCursor_GetActive;
    public Action<object, bool> TouchCursor_SetActive;
    public Func<object, float> TouchCursor_GetScale;
    public Action<object, float> TouchCursor_SetScale;
    public Func<object, Vector2> TouchCursor_GetPosition;
    public Func<object, float, float, float, float, bool> TouchCursor_IsInsideArea;
    public Func<object, List<MySprite>> TouchCursor_GetSprites;
    public Action<object> TouchCursor_ForceDispose;
    public Func<object> ClickHandler_New;
    public Func<object, Vector4> ClickHandler_GetHitArea;
    public Action<object, Vector4> ClickHandler_SetHitArea;
    public Func<object, bool> ClickHandler_IsMouseReleased;
    public Func<object, bool> ClickHandler_IsMouseOver;
    public Func<object, bool> ClickHandler_IsMousePressed;
    public Func<object, bool> ClickHandler_JustReleased;
    public Func<object, bool> ClickHandler_JustPressed;
    public Action<object, object> ClickHandler_UpdateStatus;
    public Func<object, Color> TouchTheme_GetBgColor;
    public Func<object, Color> TouchTheme_GetWhiteColor;
    public Func<object, Color> TouchTheme_GetMainColor;
    public Func<object, int, Color> TouchTheme_GetMainColorDarker;
    public Func<object, string, string, float, Vector2> TouchTheme_MeasureStringInPixels;
    public Func<object, float> TouchTheme_GetScale;
    public Action<object, float> TouchTheme_SetScale;
    public Func<object, string> TouchTheme_GetFont;
    public Action<object, string> TouchTheme_SetFont;
    public Func<object, bool> TouchElementBase_GetEnabled;
    public Action<object, bool> TouchElementBase_SetEnabled;
    public Func<object, bool> TouchElementBase_GetAbsolute;
    public Action<object, bool> TouchElementBase_SetAbsolute;
    public Func<object, byte> TouchElementBase_GetSelfAlignment;
    public Action<object, byte> TouchElementBase_SetSelfAlignment;
    public Func<object, Vector2> TouchElementBase_GetPosition;
    public Action<object, Vector2> TouchElementBase_SetPosition;
    public Func<object, Vector4> TouchElementBase_GetMargin;
    public Action<object, Vector4> TouchElementBase_SetMargin;
    public Func<object, Vector2> TouchElementBase_GetScale;
    public Action<object, Vector2> TouchElementBase_SetScale;
    public Func<object, Vector2> TouchElementBase_GetPixels;
    public Action<object, Vector2> TouchElementBase_SetPixels;
    public Func<object, Vector2> TouchElementBase_GetSize;
    public Func<object, Vector2> TouchElementBase_GetBoundaries;
    public Func<object, object> TouchElementBase_GetApp;
    public Func<object, object> TouchElementBase_GetParent;
    public Func<object, List<MySprite>> TouchElementBase_GetSprites;
    public Action<object> TouchElementBase_ForceUpdate;
    public Action<object> TouchElementBase_ForceDispose;
    public Action<object, Action> TouchElementBase_RegisterUpdate;
    public Action<object, Action> TouchElementBase_UnregisterUpdate;
    public Func<object, List<object>> TouchContainerBase_GetChildren;
    public Func<object, Vector2> TouchContainerBase_GetFlexSize;
    public Action<object, object> TouchContainerBase_AddChild;
    public Action<object, object, int> TouchContainerBase_AddChildAt;
    public Action<object, object> TouchContainerBase_RemoveChild;
    public Action<object, int> TouchContainerBase_RemoveChildAt;
    public Action<object, object, int> TouchContainerBase_MoveChild;
    public Func<byte, Color?, object> TouchView_New;
    public Func<object, bool> TouchView_GetOverflow;
    public Action<object, bool> TouchView_SetOverflow;
    public Func<object, byte> TouchView_GetDirection;
    public Action<object, byte> TouchView_SetDirection;
    public Func<object, byte> TouchView_GetAlignment;
    public Action<object, byte> TouchView_SetAlignment;
    public Func<object, byte> TouchView_GetAnchor;
    public Action<object, byte> TouchView_SetAnchor;
    public Func<object, bool> TouchView_GetUseThemeColors;
    public Action<object, bool> TouchView_SetUseThemeColors;
    public Func<object, Color> TouchView_GetBgColor;
    public Action<object, Color> TouchView_SetBgColor;
    public Func<object, Color> TouchView_GetBorderColor;
    public Action<object, Color> TouchView_SetBorderColor;
    public Func<object, Vector4> TouchView_GetBorder;
    public Action<object, Vector4> TouchView_SetBorder;
    public Func<object, Vector4> TouchView_GetPadding;
    public Action<object, Vector4> TouchView_SetPadding;
    public Func<object, int> TouchView_GetGap;
    public Action<object, int> TouchView_SetGap;
    public Func<int, Color?, object> TouchScrollView_New;
    public Func<object, float> TouchScrollView_GetScroll;
    public Action<object, float> TouchScrollView_SetScroll;
    public Func<object, bool> TouchScrollView_GetScrollAlwaysVisible;
    public Action<object, bool> TouchScrollView_SetScrollAlwaysVisible;
    public Func<object, object> TouchScrollView_GetScrollBar;
    public Func<object> TouchApp_New;
    public Func<object, object> TouchApp_GetScreen;
    public Func<object, RectangleF> TouchApp_GetViewport;
    public Func<object, object> TouchApp_GetCursor;
    public Func<object, object> TouchApp_GetTheme;
    public Func<object, bool> TouchApp_GetDefaultBg;
    public Action<object, bool> TouchApp_SetDefaultBg;
    public Action<object, IngameIMyCubeBlock, IngameIMyTextSurface> TouchApp_InitApp;
    public Func<Action, object> TouchEmptyButton_New;
    public Func<object, object> TouchEmptyButton_GetHandler;
    public Action<object, Action> TouchEmptyButton_SetOnChange;
    public Func<string, Action, object> TouchButton_New;
    public Func<object, object> TouchButton_GetLabel;
    public Func<Action<bool>, bool, object> TouchCheckbox_New;
    public Func<object, bool> TouchCheckbox_GetValue;
    public Action<object, bool> TouchCheckbox_SetValue;
    public Action<object, Action<bool>> TouchCheckbox_SetOnChange;
    public Func<object, object> TouchCheckbox_GetCheckMark;
    public Func<string, float, TextAlignment, object> TouchLabel_New;
    public Func<object, bool> TouchLabel_GetAutoBreakLine;
    public Action<object, bool> TouchLabel_SetAutoBreakLine;
    public Func<object, byte> TouchLabel_GetAutoEllipsis;
    public Action<object, byte> TouchLabel_SetAutoEllipsis;
    public Func<object, bool> TouchLabel_GetHasEllipsis;
    public Func<object, string> TouchLabel_GetText;
    public Action<object, string> TouchLabel_SetText;
    public Func<object, Color?> TouchLabel_GetTextColor;
    public Action<object, Color> TouchLabel_SetTextColor;
    public Func<object, float> TouchLabel_GetFontSize;
    public Action<object, float> TouchLabel_SetFontSize;
    public Func<object, TextAlignment> TouchLabel_GetAlignment;
    public Action<object, TextAlignment> TouchLabel_SetAlignment;
    public Func<bool, object> TouchBarContainer_New;
    public Func<object, bool> TouchBarContainer_GetIsVertical;
    public Action<object, bool> TouchBarContainer_SetIsVertical;
    public Func<object, float> TouchBarContainer_GetRatio;
    public Action<object, float> TouchBarContainer_SetRatio;
    public Func<object, float> TouchBarContainer_GetOffset;
    public Action<object, float> TouchBarContainer_SetOffset;
    public Func<object, object> TouchBarContainer_GetBar;
    public Func<float, float, bool, float, object> TouchProgressBar_New;
    public Func<object, float> TouchProgressBar_GetValue;
    public Action<object, float> TouchProgressBar_SetValue;
    public Func<object, float> TouchProgressBar_GetMaxValue;
    public Action<object, float> TouchProgressBar_SetMaxValue;
    public Func<object, float> TouchProgressBar_GetMinValue;
    public Action<object, float> TouchProgressBar_SetMinValue;
    public Func<object, float> TouchProgressBar_GetBarsGap;
    public Action<object, float> TouchProgressBar_SetBarsGap;
    public Func<object, object> TouchProgressBar_GetLabel;
    public Func<List<string>, Action<int, string>, bool, object> TouchSelector_New;
    public Func<object, bool> TouchSelector_GetLoop;
    public Action<object, bool> TouchSelector_SetLoop;
    public Func<object, int> TouchSelector_GetSelected;
    public Action<object, int> TouchSelector_SetSelected;
    public Action<object, Action<int, string>> TouchSelector_SetOnChange;
    public Func<float, float, Action<float>, object> TouchSlider_New;
    public Func<object, float> TouchSlider_GetMaxValue;
    public Action<object, float> TouchSlider_SetMaxValue;
    public Func<object, float> TouchSlider_GetMinValue;
    public Action<object, float> TouchSlider_SetMinValue;
    public Func<object, float> TouchSlider_GetValue;
    public Action<object, float> TouchSlider_SetValue;
    public Action<object, Action<float>> TouchSlider_SetOnChange;
    public Func<object, bool> TouchSlider_GetIsInteger;
    public Action<object, bool> TouchSlider_SetIsInteger;
    public Func<object, bool> TouchSlider_GetAllowInput;
    public Action<object, bool> TouchSlider_SetAllowInput;
    public Func<object, object> TouchSlider_GetBar;
    public Func<object, object> TouchSlider_GetThumb;
    public Func<object, object> TouchSlider_GetTextInput;
    public Func<float, float, Action<float, float>, object> TouchSliderRange_NewR;
    public Func<object, float> TouchSliderRange_GetValueLower;
    public Action<object, float> TouchSliderRange_SetValueLower;
    public Action<object, Action<float, float>> TouchSliderRange_SetOnChangeR;
    public Func<object, object> TouchSliderRange_GetThumbLower;
    public Func<string[], int, Action<int>, object> TouchSwitch_New;
    public Func<object, int> TouchSwitch_GetIndex;
    public Action<object, int> TouchSwitch_SetIndex;
    public Func<object, object[]> TouchSwitch_GetButtons;
    public Action<object, Action<int>> TouchSwitch_SetOnChange;
    public Func<string, Action<string, bool>, object> TouchTextField_New;
    public Func<object, bool> TouchTextField_GetIsEditing;
    public Func<object, string> TouchTextField_GetText;
    public Action<object, string> TouchTextField_SetText;
    public Action<object, Action<string, bool>> TouchTextField_SetOnChange;
    public Func<object, bool> TouchTextField_GetIsNumeric;
    public Action<object, bool> TouchTextField_SetIsNumeric;
    public Func<object, bool> TouchTextField_GetIsInteger;
    public Action<object, bool> TouchTextField_SetIsInteger;
    public Func<object, bool> TouchTextField_GetAllowNegative;
    public Action<object, bool> TouchTextField_SetAllowNegative;
    public Func<object, object> TouchTextField_GetLabel;
    public Func<string, object> TouchWindowBar_New;
    public Func<object, object> TouchWindowBar_GetLabel;
    public Func<int, object> TouchChart_New;
    public Func<object, List<float[]>> TouchChart_GetDataSets;
    public Func<object, List<Color>> TouchChart_GetDataColors;
    public Func<object, int> TouchChart_GetGridHorizontalLines;
    public Action<object, int> TouchChart_SetGridHorizontalLines;
    public Func<object, int> TouchChart_GetGridVerticalLines;
    public Action<object, int> TouchChart_SetGridVerticalLines;
    public Func<object, float> TouchChart_GetMaxValue;
    public Func<object, float> TouchChart_GetMinValue;
    public Func<object, Color?> TouchChart_GetGridColor;
    public Action<object, Color> TouchChart_SetGridColor;
    public Func<object> TouchEmptyElement_New;
  }
  public abstract class WrapperBase<TT> where TT : TouchUiKitDelegator
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
  public class TouchScreen : WrapperBase<TouchUiKitDelegator>
  {
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchScreen(object internalObject) : base(internalObject) { }
    public IngameIMyCubeBlock Block { get { return Api.TouchScreen_GetBlock.Invoke(InternalObj); } }
    public IngameIMyTextSurface Surface { get { return Api.TouchScreen_GetSurface.Invoke(InternalObj); } }
    public int Index { get { return Api.TouchScreen_GetIndex.Invoke(InternalObj); } }
    public bool IsOnScreen { get { return Api.TouchScreen_IsOnScreen.Invoke(InternalObj); } }
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
  public class TouchCursor : WrapperBase<TouchUiKitDelegator>
  {
    public TouchCursor(TouchScreen screen) : base(Api.TouchCursor_New(screen.InternalObj)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchCursor(object internalObject) : base(internalObject) { }
    public bool Active { get { return Api.TouchCursor_GetActive.Invoke(InternalObj); } set { Api.TouchCursor_SetActive.Invoke(InternalObj, value); } }
    public float Scale { get { return Api.TouchCursor_GetScale.Invoke(InternalObj); } set { Api.TouchCursor_SetScale.Invoke(InternalObj, value); } }
    public Vector2 Position { get { return Api.TouchCursor_GetPosition.Invoke(InternalObj); } }
    public bool IsInsideArea(float x, float y, float z, float w) => Api.TouchCursor_IsInsideArea.Invoke(InternalObj, x, y, z, w);
    public List<MySprite> GetSprites() => Api.TouchCursor_GetSprites.Invoke(InternalObj);
    public void ForceDispose() => Api.TouchCursor_ForceDispose.Invoke(InternalObj);
  }
  public class ClickHandler : WrapperBase<TouchUiKitDelegator>
  {
    public ClickHandler() : base(Api.ClickHandler_New()) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public ClickHandler(object internalObject) : base(internalObject) { }
    /// <summary>
    /// A Vector4 representing the area on screen that should check for cursor position.
    /// </summary>
    public Vector4 HitArea { get { return Api.ClickHandler_GetHitArea.Invoke(InternalObj); } set { Api.ClickHandler_SetHitArea.Invoke(InternalObj, value); } }
    public bool IsMouseReleased { get { return Api.ClickHandler_IsMouseReleased.Invoke(InternalObj); } }
    public bool IsMouseOver { get { return Api.ClickHandler_IsMouseOver.Invoke(InternalObj); } }
    public bool IsMousePressed { get { return Api.ClickHandler_IsMousePressed.Invoke(InternalObj); } }
    public bool JustReleased { get { return Api.ClickHandler_JustReleased.Invoke(InternalObj); } }
    public bool JustPressed { get { return Api.ClickHandler_JustPressed.Invoke(InternalObj); } }
    /// <summary>
    /// This is already called internally by the Touch Manager, only call this if you wanna override the handler status.
    /// </summary>
    public void UpdateStatus(TouchScreen screen) => Api.ClickHandler_UpdateStatus.Invoke(InternalObj, screen.InternalObj);
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/TouchTheme.cs"/>
  /// </summary>
  public class TouchTheme : WrapperBase<TouchUiKitDelegator>
  {
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchTheme(object internalObject) : base(internalObject) { }
    public Color BgColor { get { return Api.TouchTheme_GetBgColor.Invoke(InternalObj); } }
    /// <summary>
    /// This is a high contras color related to main color, it is not exactly a white.
    /// Also can be blacksh if the background color is too light.
    /// </summary>
    public Color WhiteColor { get { return Api.TouchTheme_GetWhiteColor.Invoke(InternalObj); } }
    public Color MainColor { get { return Api.TouchTheme_GetMainColor.Invoke(InternalObj); } }
    /// <summary>
    /// This gets a darker version of the main color pre calculated on the Theme. Lower numbers are darker.
    /// </summary>
    /// <param name="value">One of the options: 1, 2, 3 , 4, 5, 6, 7, 8, 9</param>
    /// <returns>The calculated color</returns>
    public Color GetMainColorDarker(int value) => Api.TouchTheme_GetMainColorDarker.Invoke(InternalObj, value);
    /// <summary>
    /// Scales the entiner App and all its elements, useful for small screens. Can be called at any time.
    /// </summary>
    public float Scale { get { return Api.TouchTheme_GetScale.Invoke(InternalObj); } set { Api.TouchTheme_SetScale.Invoke(InternalObj, value); } }
    public string Font { get { return Api.TouchTheme_GetFont.Invoke(InternalObj); } set { Api.TouchTheme_SetFont.Invoke(InternalObj, value); } }
    /// <summary>
    /// Helper to calculate the width of a text on screen.
    /// </summary>
    /// <returns>A Vector2 with width and height.</returns>
    public Vector2 MeasureStringInPixels(string text, string font, float scale) => Api.TouchTheme_MeasureStringInPixels.Invoke(InternalObj, text, font, scale);
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TouchElementBase.cs"/>
  /// </summary>
  public abstract class TouchElementBase : WrapperBase<TouchUiKitDelegator>
  {
    private TouchApp _app;
    private TouchView _parent;
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchElementBase(object internalObject) : base(internalObject) { }
    /// <summary>
    /// If false, the element will not be drawn, useful if you want to hide and show but not destroy. Better than removing it.
    /// </summary>
    public bool Enabled { get { return Api.TouchElementBase_GetEnabled.Invoke(InternalObj); } set { Api.TouchElementBase_SetEnabled.Invoke(InternalObj, value); } }
    /// <summary>
    /// If true, the element will not align and anchor with the parent. Its position will be related to the screen and size not counted for parent inner size.
    /// </summary>
    public bool Absolute { get { return Api.TouchElementBase_GetAbsolute.Invoke(InternalObj); } set { Api.TouchElementBase_SetAbsolute.Invoke(InternalObj, value); } }
    /// <summary>
    /// Controls the aligment of the element on the crossed axis of parent <see cref="TouchView.Direction" />. Useful for overriding parent's Aligment.
    /// </summary>
    public ViewAlignment SelfAlignment { get { return (ViewAlignment)Api.TouchElementBase_GetSelfAlignment.Invoke(InternalObj); } set { Api.TouchElementBase_SetSelfAlignment.Invoke(InternalObj, (byte)value); } }
    /// <summary>
    /// Position of the element related to screen. This is overriden by the parent if the element's <see cref="Absolute" /> is not true.
    /// </summary>
    public Vector2 Position { get { return Api.TouchElementBase_GetPosition.Invoke(InternalObj); } set { Api.TouchElementBase_SetPosition.Invoke(InternalObj, value); } }
    /// <summary>
    /// Margin values for four sides. Starting from Left, Top, Right and Bottom.
    /// </summary>
    public Vector4 Margin { get { return Api.TouchElementBase_GetMargin.Invoke(InternalObj); } set { Api.TouchElementBase_SetMargin.Invoke(InternalObj, value); } }
    /// <summary>
    /// The ratio of the parent that this element should fill. 1 means 100%. If the parent has more children, the proportional % will be applied.
    /// This is stackable with <see cref="Pixels" />. So set as 0 the axis if you just want a fixed pixels size.
    /// </summary>
    public Vector2 Scale { get { return Api.TouchElementBase_GetScale.Invoke(InternalObj); } set { Api.TouchElementBase_SetScale.Invoke(InternalObj, value); } }
    /// <summary>
    /// Fixed size in pixels, not related to parent.
    /// This is stackable with <see cref="Scale" />. So set as 0 the axis if you just want the size only related to parent.
    /// </summary>
    public Vector2 Pixels { get { return Api.TouchElementBase_GetPixels.Invoke(InternalObj); } set { Api.TouchElementBase_SetPixels.Invoke(InternalObj, value); } }
    /// <summary>
    /// The <see cref="App" /> that this element was added. Be careful, this is null until the element is properly added.
    /// </summary>
    public TouchApp App { get { return _app ?? (_app = Wrap<TouchApp>(Api.TouchElementBase_GetApp.Invoke(InternalObj), (obj) => new TouchApp(obj))); } }
    /// <summary>
    /// The immediate parent of this element.
    /// </summary>
    public TouchView Parent { get { return _parent ?? (_parent = Wrap<TouchView>(Api.TouchElementBase_GetParent.Invoke(InternalObj), (obj) => new TouchView(obj))); } }
    /// <returns>Reference to thes Sprites of this element, if it is a container it also has the children Sprites</returns>
    public List<MySprite> GetSprites() => Api.TouchElementBase_GetSprites.Invoke(InternalObj);
    /// <returns>The calculated final size of the element in pixels. Usefull to position Absolute children.</returns>
    public Vector2 GetSize() => Api.TouchElementBase_GetSize.Invoke(InternalObj);
    /// <returns>The calculated final size and if it is a container also the border and padding.</returns>
    public Vector2 GetBoundaries() => Api.TouchElementBase_GetBoundaries.Invoke(InternalObj);
    /// <summary>
    /// Forces a call to Update method for the internal object. The method is already called from Touch Manager. Only call this if you want to force another call.
    /// </summary>
    public void ForceUpdate() => Api.TouchElementBase_ForceUpdate.Invoke(InternalObj);
    /// <summary>
    /// Forces a call to Dispose method for the internal object. The method is already called from Touch Manager when the App is Disposed.
    /// Only call this for the App instance, or if you want to force another call.
    /// </summary>
    public void ForceDispose() => Api.TouchElementBase_ForceDispose.Invoke(InternalObj);
    /// <summary>
    /// Register a delegate to be called when the internal object Update event is called.
    /// </summary>
    public void RegisterUpdate(Action update) => Api.TouchElementBase_RegisterUpdate.Invoke(InternalObj, update);
    /// <summary>
    /// Unregister a delegate. Recommended to be called on object dispose.
    /// </summary>
    public void UnregisterUpdate(Action update) => Api.TouchElementBase_UnregisterUpdate.Invoke(InternalObj, update);
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TouchContainerBase.cs"/>
  /// </summary>
  public abstract class TouchContainerBase : TouchElementBase
  {
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchContainerBase(object internalObject) : base(internalObject) { }
    public List<object> Children { get { return Api.TouchContainerBase_GetChildren.Invoke(InternalObj); } }
    /// <returns>The calculated remaining size inside the container. Negative when the children sizes are bigger.</returns>
    public Vector2 GetFlexSize() => Api.TouchContainerBase_GetFlexSize.Invoke(InternalObj);
    public void AddChild(TouchElementBase child) => Api.TouchContainerBase_AddChild.Invoke(InternalObj, child.InternalObj);
    public void AddChild(TouchElementBase child, int index) => Api.TouchContainerBase_AddChildAt.Invoke(InternalObj, child.InternalObj, index);
    public void RemoveChild(TouchElementBase child) => Api.TouchContainerBase_RemoveChild.Invoke(InternalObj, child.InternalObj);
    public void RemoveChild(object child) => Api.TouchContainerBase_RemoveChild.Invoke(InternalObj, child);
    public void RemoveChild(int index) => Api.TouchContainerBase_RemoveChildAt.Invoke(InternalObj, index);
    public void MoveChild(TouchElementBase child, int index) => Api.TouchContainerBase_MoveChild.Invoke(InternalObj, child.InternalObj, index);
  }
  public enum ViewDirection : byte { None = 0, Row = 1, Column = 2, RowReverse = 3, ColumnReverse = 4 }
  public enum ViewAlignment : byte { Start = 0, Center = 1, End = 2 }
  public enum ViewAnchor : byte { Start = 0, Center = 1, End = 2, SpaceBetween = 3, SpaceAround = 4 }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TouchView.cs"/>
  /// </summary>
  public class TouchView : TouchContainerBase
  {
    public TouchView(ViewDirection direction = ViewDirection.Column, Color? bgColor = null) : base(Api.TouchView_New((byte)direction, bgColor)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchView(object internalObject) : base(internalObject) { }
    /// <summary>
    /// If false, children outside inner size will be hidden.
    /// </summary>
    public bool Overflow { get { return Api.TouchView_GetOverflow.Invoke(InternalObj); } set { Api.TouchView_SetOverflow.Invoke(InternalObj, value); } }
    public ViewDirection Direction { get { return (ViewDirection)Api.TouchView_GetDirection.Invoke(InternalObj); } set { Api.TouchView_SetDirection.Invoke(InternalObj, (byte)value); } }
    /// <summary>
    /// The aligment of children on the crossed axis of the <see cref="Direction" />.
    /// </summary>
    public ViewAlignment Alignment { get { return (ViewAlignment)Api.TouchView_GetAlignment.Invoke(InternalObj); } set { Api.TouchView_SetAlignment.Invoke(InternalObj, (byte)value); } }
    /// <summary>
    /// The anchor position of the children on the same axis of the <see cref="Direction" />.
    /// </summary>
    public ViewAnchor Anchor { get { return (ViewAnchor)Api.TouchView_GetAnchor.Invoke(InternalObj); } set { Api.TouchView_SetAnchor.Invoke(InternalObj, (byte)value); } }
    /// <summary>
    /// If false, the element will not update colors with the App.Theme. Useful for overriding element themes.
    /// </summary>
    public bool UseThemeColors { get { return Api.TouchView_GetUseThemeColors.Invoke(InternalObj); } set { Api.TouchView_SetUseThemeColors.Invoke(InternalObj, value); } }
    public Color BgColor { get { return Api.TouchView_GetBgColor.Invoke(InternalObj); } set { Api.TouchView_SetBgColor.Invoke(InternalObj, value); } }
    public Color BorderColor { get { return Api.TouchView_GetBorderColor.Invoke(InternalObj); } set { Api.TouchView_SetBorderColor.Invoke(InternalObj, value); } }
    /// <summary>
    /// Border values for four sides. Starting from Left, Top, Right and Bottom.
    /// </summary>
    public Vector4 Border { get { return Api.TouchView_GetBorder.Invoke(InternalObj); } set { Api.TouchView_SetBorder.Invoke(InternalObj, value); } }
    /// <summary>
    /// Padding values for four sides. Starting from Left, Top, Right and Bottom.
    /// </summary>
    public Vector4 Padding { get { return Api.TouchView_GetPadding.Invoke(InternalObj); } set { Api.TouchView_SetPadding.Invoke(InternalObj, value); } }
    /// <summary>
    /// Adds a spacing between children. Better than adding margin to each child, if the same spacing is needed.
    /// </summary>
    public int Gap { get { return Api.TouchView_GetGap.Invoke(InternalObj); } set { Api.TouchView_SetGap.Invoke(InternalObj, value); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TouchScrollView.cs"/>
  /// </summary>
  public class TouchScrollView : TouchView
  {
    private TouchBarContainer _scrollBar;
    public TouchScrollView(ViewDirection direction = ViewDirection.Column, Color? bgColor = null) : base(Api.TouchScrollView_New((int)direction, bgColor)) { }
    /// <summary>
    /// Ratio from 0 to 1.
    /// </summary>
    public float Scroll { get { return Api.TouchScrollView_GetScroll.Invoke(InternalObj); } set { Api.TouchScrollView_SetScroll.Invoke(InternalObj, value); } }
    public bool ScrollAlwaysVisible { get { return Api.TouchScrollView_GetScrollAlwaysVisible.Invoke(InternalObj); } set { Api.TouchScrollView_SetScrollAlwaysVisible.Invoke(InternalObj, value); } }
    public TouchBarContainer ScrollBar { get { return _scrollBar ?? (_scrollBar = Wrap<TouchBarContainer>(Api.TouchScrollView_GetScrollBar.Invoke(InternalObj), (obj) => new TouchBarContainer(obj))); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TouchApp.cs"/>
  /// </summary>
  public class TouchApp : TouchView
  {
    private TouchScreen _screen;
    private TouchCursor _cursor;
    private TouchTheme _theme;
    public TouchApp() : base(Api.TouchApp_New()) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchApp(object internalObject) : base(internalObject) { }
    public TouchScreen Screen { get { return _screen ?? (_screen = Wrap<TouchScreen>(Api.TouchApp_GetScreen.Invoke(InternalObj), (obj) => new TouchScreen(obj))); } }
    public RectangleF Viewport { get { return Api.TouchApp_GetViewport.Invoke(InternalObj); } }
    public TouchCursor Cursor { get { return _cursor ?? (_cursor = Wrap<TouchCursor>(Api.TouchApp_GetCursor.Invoke(InternalObj), (obj) => new TouchCursor(obj))); } }
    public TouchTheme Theme { get { return _theme ?? (_theme = Wrap<TouchTheme>(Api.TouchApp_GetTheme.Invoke(InternalObj), (obj) => new TouchTheme(obj))); } }
    /// <summary>
    /// If true, the app will present a nice background image.
    /// </summary>
    public bool DefaultBg { get { return Api.TouchApp_GetDefaultBg.Invoke(InternalObj); } set { Api.TouchApp_SetDefaultBg.Invoke(InternalObj, value); } }
    /// <summary>
    /// Initiates the app, recommended to be called after a few seconds when used on a TSS.
    /// This method can fail if the block and surface are not ready for TouchScreen, catch any exceptions.
    /// </summary>
    public virtual void InitApp(IngameIMyCubeBlock block, IngameIMyTextSurface surface) => Api.TouchApp_InitApp.Invoke(InternalObj, block, surface);
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TouchEmptyButton.cs"/>
  /// </summary>
  public class TouchEmptyButton : TouchView
  {
    private ClickHandler _handler;
    public TouchEmptyButton(Action onChange) : base(Api.TouchEmptyButton_New(onChange)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchEmptyButton(object internalObject) : base(internalObject) { }
    public ClickHandler Handler { get { return _handler ?? (_handler = Wrap<ClickHandler>(Api.TouchEmptyButton_GetHandler.Invoke(InternalObj), (obj) => new ClickHandler(obj))); } }
    public Action OnChange { set { Api.TouchEmptyButton_SetOnChange.Invoke(InternalObj, value); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TouchButton.cs"/>
  /// </summary>
  public class TouchButton : TouchEmptyButton
  {
    private TouchLabel _label;
    public TouchButton(string text, Action onChange) : base(Api.TouchButton_New(text, onChange)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchButton(object internalObject) : base(internalObject) { }
    public TouchLabel Label { get { return _label ?? (_label = Wrap<TouchLabel>(Api.TouchButton_GetLabel.Invoke(InternalObj), (obj) => new TouchLabel(obj))); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TouchCheckbox.cs"/>
  /// </summary>
  public class TouchCheckbox : TouchView
  {
    private TouchEmptyElement _checkMark;
    public TouchCheckbox(Action<bool> onChange, bool value = false) : base(Api.TouchCheckbox_New(onChange, value)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchCheckbox(object internalObject) : base(internalObject) { }
    public bool Value { get { return Api.TouchCheckbox_GetValue.Invoke(InternalObj); } set { Api.TouchCheckbox_SetValue.Invoke(InternalObj, value); } }
    public Action<bool> OnChange { set { Api.TouchCheckbox_SetOnChange.Invoke(InternalObj, value); } }
    public TouchEmptyElement CheckMark { get { return _checkMark ?? (_checkMark = Wrap<TouchEmptyElement>(Api.TouchCheckbox_GetCheckMark.Invoke(InternalObj), (obj) => new TouchEmptyElement(obj))); } }
  }
  public enum LabelEllipsis : byte { None = 0, Left = 1, Right = 2 }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TouchLabel.cs"/>
  /// </summary>
  public class TouchLabel : TouchElementBase
  {
    public TouchLabel(string text, float fontSize = 0.5f, TextAlignment alignment = TextAlignment.CENTER) : base(Api.TouchLabel_New(text, fontSize, alignment)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchLabel(object internalObject) : base(internalObject) { }
    public bool AutoBreakLine { get { return Api.TouchLabel_GetAutoBreakLine.Invoke(InternalObj); } set { Api.TouchLabel_SetAutoBreakLine.Invoke(InternalObj, value); } }
    /// <summary>
    /// If true, text will not be shortened if bigger than size.
    /// </summary>
    public LabelEllipsis AutoEllipsis { get { return (LabelEllipsis)Api.TouchLabel_GetAutoEllipsis.Invoke(InternalObj); } set { Api.TouchLabel_SetAutoEllipsis.Invoke(InternalObj, (byte)value); } }
    /// <summary>
    /// If <see cref="AutoEllipsis" /> is false and the text was limited to fit the size and added an ellipsis.
    /// </summary>
    public bool HasEllipsis { get { return Api.TouchLabel_GetHasEllipsis.Invoke(InternalObj); } }
    public string Text { get { return Api.TouchLabel_GetText.Invoke(InternalObj); } set { Api.TouchLabel_SetText.Invoke(InternalObj, value); } }
    public Color? TextColor { get { return Api.TouchLabel_GetTextColor.Invoke(InternalObj); } set { Api.TouchLabel_SetTextColor.Invoke(InternalObj, (Color)value); } }
    public float FontSize { get { return Api.TouchLabel_GetFontSize.Invoke(InternalObj); } set { Api.TouchLabel_SetFontSize.Invoke(InternalObj, value); } }
    public TextAlignment Alignment { get { return Api.TouchLabel_GetAlignment.Invoke(InternalObj); } set { Api.TouchLabel_SetAlignment.Invoke(InternalObj, value); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TouchBarContainer.cs"/>
  /// </summary>
  public class TouchBarContainer : TouchView
  {
    private TouchView _bar;
    public TouchBarContainer(bool vertical = false) : base(Api.TouchBarContainer_New(vertical)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchBarContainer(object internalObject) : base(internalObject) { }
    public bool IsVertical { get { return Api.TouchBarContainer_GetIsVertical.Invoke(InternalObj); } set { Api.TouchBarContainer_SetIsVertical.Invoke(InternalObj, value); } }
    /// <summary>
    /// Ratio from 0 to 1. Relative size of the inner bar.
    /// </summary>
    public float Ratio { get { return Api.TouchBarContainer_GetRatio.Invoke(InternalObj); } set { Api.TouchBarContainer_SetRatio.Invoke(InternalObj, value); } }
    /// <summary>
    /// Ratio from 0 to 1. Relative position of the inner bar. Limited by the remaining space of the container.
    /// </summary>
    public float Offset { get { return Api.TouchBarContainer_GetOffset.Invoke(InternalObj); } set { Api.TouchBarContainer_SetOffset.Invoke(InternalObj, value); } }
    public TouchView Bar { get { return _bar ?? (_bar = Wrap<TouchView>(Api.TouchBarContainer_GetBar.Invoke(InternalObj), (obj) => new TouchView(obj))); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TouchProgressBar.cs"/>
  /// </summary>
  public class TouchProgressBar : TouchBarContainer
  {
    private TouchLabel _label;
    public TouchProgressBar(float min, float max, bool vertical = false, float barsGap = 0) : base(Api.TouchProgressBar_New(min, max, vertical, barsGap)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchProgressBar(object internalObject) : base(internalObject) { }
    public float Value { get { return Api.TouchProgressBar_GetValue.Invoke(InternalObj); } set { Api.TouchProgressBar_SetValue.Invoke(InternalObj, value); } }
    public float MaxValue { get { return Api.TouchProgressBar_GetMaxValue.Invoke(InternalObj); } set { Api.TouchProgressBar_SetMaxValue.Invoke(InternalObj, value); } }
    public float MinValue { get { return Api.TouchProgressBar_GetMinValue.Invoke(InternalObj); } set { Api.TouchProgressBar_SetMinValue.Invoke(InternalObj, value); } }
    public float BarsGap { get { return Api.TouchProgressBar_GetBarsGap.Invoke(InternalObj); } set { Api.TouchProgressBar_SetBarsGap.Invoke(InternalObj, value); } }
    public TouchLabel Label { get { return _label ?? (_label = Wrap<TouchLabel>(Api.TouchProgressBar_GetLabel.Invoke(InternalObj), (obj) => new TouchLabel(obj))); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TouchSelector.cs"/>
  /// </summary>
  public class TouchSelector : TouchView
  {
    public TouchSelector(List<string> labels, Action<int, string> onChange, bool loop = true) : base(Api.TouchSelector_New(labels, onChange, loop)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchSelector(object internalObject) : base(internalObject) { }
    public bool Loop { get { return Api.TouchSelector_GetLoop.Invoke(InternalObj); } set { Api.TouchSelector_SetLoop.Invoke(InternalObj, value); } }
    public int Selected { get { return Api.TouchSelector_GetSelected.Invoke(InternalObj); } set { Api.TouchSelector_SetSelected.Invoke(InternalObj, value); } }
    public Action<int, string> OnChange { set { Api.TouchSelector_SetOnChange.Invoke(InternalObj, value); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TouchSlider.cs"/>
  /// </summary>
  public class TouchSlider : TouchView
  {
    private TouchBarContainer _bar;
    private TouchEmptyElement _thumb;
    private TouchTextField _textInput;
    public TouchSlider(float min, float max, Action<float> onChange) : base(Api.TouchSlider_New(min, max, onChange)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchSlider(object internalObject) : base(internalObject) { }
    public float MaxValue { get { return Api.TouchSlider_GetMaxValue.Invoke(InternalObj); } set { Api.TouchSlider_SetMaxValue.Invoke(InternalObj, value); } }
    public float MinValue { get { return Api.TouchSlider_GetMinValue.Invoke(InternalObj); } set { Api.TouchSlider_SetMinValue.Invoke(InternalObj, value); } }
    public float Value { get { return Api.TouchSlider_GetValue.Invoke(InternalObj); } set { Api.TouchSlider_SetValue.Invoke(InternalObj, value); } }
    public bool IsInteger { get { return Api.TouchSlider_GetIsInteger.Invoke(InternalObj); } set { Api.TouchSlider_SetIsInteger.Invoke(InternalObj, value); } }
    /// <summary>
    /// If true, user can Hold Ctrl and click to manually type a number.
    /// </summary>
    public bool AllowInput { get { return Api.TouchSlider_GetAllowInput.Invoke(InternalObj); } set { Api.TouchSlider_SetAllowInput.Invoke(InternalObj, value); } }
    public Action<float> OnChange { set { Api.TouchSlider_SetOnChange.Invoke(InternalObj, value); } }
    public TouchBarContainer Bar { get { return _bar ?? (_bar = Wrap<TouchBarContainer>(Api.TouchSlider_GetBar.Invoke(InternalObj), (obj) => new TouchBarContainer(obj))); } }
    public TouchEmptyElement Thumb { get { return _thumb ?? (_thumb = Wrap<TouchEmptyElement>(Api.TouchSlider_GetThumb.Invoke(InternalObj), (obj) => new TouchEmptyElement(obj))); } }
    public TouchTextField TextInput { get { return _textInput ?? (_textInput = Wrap<TouchTextField>(Api.TouchSlider_GetTextInput.Invoke(InternalObj), (obj) => new TouchTextField(obj))); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TouchSliderRange.cs"/>
  /// </summary>
  public class TouchSliderRange : TouchSlider
  {
    private TouchEmptyElement _thumbLower;
    public TouchSliderRange(float min, float max, Action<float, float> onChange) : base(Api.TouchSliderRange_NewR(min, max, onChange)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchSliderRange(object internalObject) : base(internalObject) { }
    public float ValueLower { get { return Api.TouchSliderRange_GetValueLower.Invoke(InternalObj); } set { Api.TouchSliderRange_SetValueLower.Invoke(InternalObj, value); } }
    public Action<float, float> OnChangeRange { set { Api.TouchSliderRange_SetOnChangeR.Invoke(InternalObj, value); } }
    public TouchEmptyElement ThumbLower { get { return _thumbLower ?? (_thumbLower = Wrap<TouchEmptyElement>(Api.TouchSliderRange_GetThumbLower.Invoke(InternalObj), (obj) => new TouchEmptyElement(obj))); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TouchSwitch.cs"/>
  /// </summary>
  public class TouchSwitch : TouchView
  {
    private TouchButton[] _buttons;
    public TouchSwitch(string[] labels, int index = 0, Action<int> onChange = null) : base(Api.TouchSwitch_New(labels, index, onChange)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchSwitch(object internalObject) : base(internalObject) { }
    public int Index { get { return Api.TouchSwitch_GetIndex.Invoke(InternalObj); } set { Api.TouchSwitch_SetIndex.Invoke(InternalObj, value); } }
    public TouchButton[] Buttons { get { return _buttons ?? (_buttons = WrapArray<TouchButton>(Api.TouchSwitch_GetButtons.Invoke(InternalObj), (obj) => new TouchButton(obj))); } }
    public Action<int> OnChange { set { Api.TouchSwitch_SetOnChange.Invoke(InternalObj, value); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TouchTextField.cs"/>
  /// </summary>
  public class TouchTextField : TouchView
  {
    private TouchLabel _label;
    public TouchTextField(string text, Action<string, bool> onChange) : base(Api.TouchTextField_New(text, onChange)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchTextField(object internalObject) : base(internalObject) { }
    public bool IsEditing { get { return Api.TouchTextField_GetIsEditing.Invoke(InternalObj); } }
    public string Text { get { return Api.TouchTextField_GetText.Invoke(InternalObj); } set { Api.TouchTextField_SetText.Invoke(InternalObj, value); } }
    public bool IsNumeric { get { return Api.TouchTextField_GetIsNumeric.Invoke(InternalObj); } set { Api.TouchTextField_SetIsNumeric.Invoke(InternalObj, value); } }
    public bool IsInteger { get { return Api.TouchTextField_GetIsInteger.Invoke(InternalObj); } set { Api.TouchTextField_SetIsInteger.Invoke(InternalObj, value); } }
    public bool AllowNegative { get { return Api.TouchTextField_GetAllowNegative.Invoke(InternalObj); } set { Api.TouchTextField_SetAllowNegative.Invoke(InternalObj, value); } }
    public TouchLabel Label { get { return _label ?? (_label = Wrap<TouchLabel>(Api.TouchTextField_GetLabel.Invoke(InternalObj), (obj) => new TouchLabel(obj))); } }
    public Action<string, bool> OnChange { set { Api.TouchTextField_SetOnChange.Invoke(InternalObj, value); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TouchWindowBar.cs"/>
  /// </summary>
  public class TouchWindowBar : TouchView
  {
    private TouchLabel _label;
    public TouchWindowBar(string text) : base(Api.TouchWindowBar_New(text)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchWindowBar(object internalObject) : base(internalObject) { }
    public TouchLabel Label { get { return _label ?? (_label = Wrap<TouchLabel>(Api.TouchWindowBar_GetLabel.Invoke(InternalObj), (obj) => new TouchLabel(obj))); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TouchChart.cs"/>
  /// </summary>
  public class TouchChart : TouchElementBase
  {
    public TouchChart(int intervals) : base(Api.TouchChart_New(intervals)) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchChart(object internalObject) : base(internalObject) { }
    public List<float[]> DataSets { get { return Api.TouchChart_GetDataSets.Invoke(InternalObj); } }
    public List<Color> DataColors { get { return Api.TouchChart_GetDataColors.Invoke(InternalObj); } }
    public int GridHorizontalLines { get { return Api.TouchChart_GetGridHorizontalLines.Invoke(InternalObj); } set { Api.TouchChart_SetGridHorizontalLines.Invoke(InternalObj, value); } }
    public int GridVerticalLines { get { return Api.TouchChart_GetGridVerticalLines.Invoke(InternalObj); } set { Api.TouchChart_SetGridVerticalLines.Invoke(InternalObj, value); } }
    public float MaxValue { get { return Api.TouchChart_GetMaxValue.Invoke(InternalObj); } }
    public float MinValue { get { return Api.TouchChart_GetMinValue.Invoke(InternalObj); } }
    public Color? GridColor { get { return Api.TouchChart_GetGridColor.Invoke(InternalObj); } set { Api.TouchChart_SetGridColor.Invoke(InternalObj, (Color)value); } }
  }
  /// <summary>
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/TouchEmptyElement.cs"/>
  /// </summary>
  public class TouchEmptyElement : TouchElementBase
  {
    public TouchEmptyElement() : base(Api.TouchEmptyElement_New()) { }
    /// <summary>
    /// Do not call this ctor directly, unless you have the reference of the original object from the API.
    /// </summary>
    public TouchEmptyElement(object internalObject) : base(internalObject) { }
  }
}
