using System.Collections.Generic;
using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;
using IngameIMyTextSurface = Sandbox.ModAPI.Ingame.IMyTextSurface;
using IngameIMyCubeBlock = VRage.Game.ModAPI.Ingame.IMyCubeBlock;

namespace Lima.API
{
  /// <summary>
  /// The client API for Touch Screen and Touch Ui Kit feature.
  /// This only handle communication with the MOD that have all the features.
  /// Copy this file to your mod, and add TouchScreenAPI mod as dependency.
  /// <see href="https://steamcommunity.com/sharedfiles/filedetails/?id=2668820525"/>
  /// </summary>
  public class TouchUiKit : TouchScreenAPI
  {
    protected override string GetRequestString() { return "ApiRequestTouchAndUi"; }

    /// <summary>
    /// If you want Ui Kit features, only instantiate this class, it calls TouchScreenAPI automatically.
    /// </summary>
    public TouchUiKit()
    {
      ApiDelegator = new UiKitDelegator();
    }

    public override bool Load()
    {
      WrapperBase<UiKitDelegator>.SetApi(ApiDelegator as UiKitDelegator);
      return base.Load();
    }

    public override void Unload()
    {
      WrapperBase<UiKitDelegator>.SetApi(null);
      base.Unload();
    }

    protected override void ApiDelegates(IReadOnlyDictionary<string, Delegate> delegates)
    {
      base.ApiDelegates(delegates);
      var apiDel = ApiDelegator as UiKitDelegator;

      AssignMethod(delegates, "Theme_GetBgColor", ref apiDel.Theme_GetBgColor);
      AssignMethod(delegates, "Theme_GetWhiteColor", ref apiDel.Theme_GetWhiteColor);
      AssignMethod(delegates, "Theme_GetMainColor", ref apiDel.Theme_GetMainColor);
      AssignMethod(delegates, "Theme_GetMainColorDarker", ref apiDel.Theme_GetMainColorDarker);
      AssignMethod(delegates, "Theme_MeasureStringInPixels", ref apiDel.Theme_MeasureStringInPixels);
      AssignMethod(delegates, "Theme_GetScale", ref apiDel.Theme_GetScale);
      AssignMethod(delegates, "Theme_SetScale", ref apiDel.Theme_SetScale);
      AssignMethod(delegates, "Theme_GetFont", ref apiDel.Theme_GetFont);
      AssignMethod(delegates, "Theme_SetFont", ref apiDel.Theme_SetFont);
      AssignMethod(delegates, "ElementBase_GetEnabled", ref apiDel.ElementBase_GetEnabled);
      AssignMethod(delegates, "ElementBase_SetEnabled", ref apiDel.ElementBase_SetEnabled);
      AssignMethod(delegates, "ElementBase_GetAbsolute", ref apiDel.ElementBase_GetAbsolute);
      AssignMethod(delegates, "ElementBase_SetAbsolute", ref apiDel.ElementBase_SetAbsolute);
      AssignMethod(delegates, "ElementBase_GetSelfAlignment", ref apiDel.ElementBase_GetSelfAlignment);
      AssignMethod(delegates, "ElementBase_SetSelfAlignment", ref apiDel.ElementBase_SetSelfAlignment);
      AssignMethod(delegates, "ElementBase_GetPosition", ref apiDel.ElementBase_GetPosition);
      AssignMethod(delegates, "ElementBase_SetPosition", ref apiDel.ElementBase_SetPosition);
      AssignMethod(delegates, "ElementBase_GetMargin", ref apiDel.ElementBase_GetMargin);
      AssignMethod(delegates, "ElementBase_SetMargin", ref apiDel.ElementBase_SetMargin);
      AssignMethod(delegates, "ElementBase_GetFlex", ref apiDel.ElementBase_GetFlex);
      AssignMethod(delegates, "ElementBase_SetFlex", ref apiDel.ElementBase_SetFlex);
      AssignMethod(delegates, "ElementBase_GetPixels", ref apiDel.ElementBase_GetPixels);
      AssignMethod(delegates, "ElementBase_SetPixels", ref apiDel.ElementBase_SetPixels);
      AssignMethod(delegates, "ElementBase_GetSize", ref apiDel.ElementBase_GetSize);
      AssignMethod(delegates, "ElementBase_GetBoundaries", ref apiDel.ElementBase_GetBoundaries);
      AssignMethod(delegates, "ElementBase_GetApp", ref apiDel.ElementBase_GetApp);
      AssignMethod(delegates, "ElementBase_GetParent", ref apiDel.ElementBase_GetParent);
      AssignMethod(delegates, "ElementBase_GetSprites", ref apiDel.ElementBase_GetSprites);
      AssignMethod(delegates, "ElementBase_ForceUpdate", ref apiDel.ElementBase_ForceUpdate);
      AssignMethod(delegates, "ElementBase_ForceDispose", ref apiDel.ElementBase_ForceDispose);
      AssignMethod(delegates, "ElementBase_RegisterUpdate", ref apiDel.ElementBase_RegisterUpdate);
      AssignMethod(delegates, "ElementBase_UnregisterUpdate", ref apiDel.ElementBase_UnregisterUpdate);
      AssignMethod(delegates, "ContainerBase_GetChildren", ref apiDel.ContainerBase_GetChildren);
      AssignMethod(delegates, "ContainerBase_GetFlexSize", ref apiDel.ContainerBase_GetFlexSize);
      AssignMethod(delegates, "ContainerBase_AddChild", ref apiDel.ContainerBase_AddChild);
      AssignMethod(delegates, "ContainerBase_AddChildAt", ref apiDel.ContainerBase_AddChildAt);
      AssignMethod(delegates, "ContainerBase_RemoveChild", ref apiDel.ContainerBase_RemoveChild);
      AssignMethod(delegates, "ContainerBase_RemoveChildAt", ref apiDel.ContainerBase_RemoveChildAt);
      AssignMethod(delegates, "ContainerBase_MoveChild", ref apiDel.ContainerBase_MoveChild);
      AssignMethod(delegates, "View_New", ref apiDel.View_New);
      AssignMethod(delegates, "View_GetOverflow", ref apiDel.View_GetOverflow);
      AssignMethod(delegates, "View_SetOverflow", ref apiDel.View_SetOverflow);
      AssignMethod(delegates, "View_GetDirection", ref apiDel.View_GetDirection);
      AssignMethod(delegates, "View_SetDirection", ref apiDel.View_SetDirection);
      AssignMethod(delegates, "View_GetAlignment", ref apiDel.View_GetAlignment);
      AssignMethod(delegates, "View_SetAlignment", ref apiDel.View_SetAlignment);
      AssignMethod(delegates, "View_GetAnchor", ref apiDel.View_GetAnchor);
      AssignMethod(delegates, "View_SetAnchor", ref apiDel.View_SetAnchor);
      AssignMethod(delegates, "View_GetUseThemeColors", ref apiDel.View_GetUseThemeColors);
      AssignMethod(delegates, "View_SetUseThemeColors", ref apiDel.View_SetUseThemeColors);
      AssignMethod(delegates, "View_GetBgColor", ref apiDel.View_GetBgColor);
      AssignMethod(delegates, "View_SetBgColor", ref apiDel.View_SetBgColor);
      AssignMethod(delegates, "View_GetBorderColor", ref apiDel.View_GetBorderColor);
      AssignMethod(delegates, "View_SetBorderColor", ref apiDel.View_SetBorderColor);
      AssignMethod(delegates, "View_GetBorder", ref apiDel.View_GetBorder);
      AssignMethod(delegates, "View_SetBorder", ref apiDel.View_SetBorder);
      AssignMethod(delegates, "View_GetPadding", ref apiDel.View_GetPadding);
      AssignMethod(delegates, "View_SetPadding", ref apiDel.View_SetPadding);
      AssignMethod(delegates, "View_GetGap", ref apiDel.View_GetGap);
      AssignMethod(delegates, "View_SetGap", ref apiDel.View_SetGap);
      AssignMethod(delegates, "ScrollView_New", ref apiDel.ScrollView_New);
      AssignMethod(delegates, "ScrollView_GetScroll", ref apiDel.ScrollView_GetScroll);
      AssignMethod(delegates, "ScrollView_SetScroll", ref apiDel.ScrollView_SetScroll);
      AssignMethod(delegates, "ScrollView_GetScrollAlwaysVisible", ref apiDel.ScrollView_GetScrollAlwaysVisible);
      AssignMethod(delegates, "ScrollView_SetScrollAlwaysVisible", ref apiDel.ScrollView_SetScrollAlwaysVisible);
      AssignMethod(delegates, "ScrollView_GetScrollWheelEnabled", ref apiDel.ScrollView_GetScrollWheelEnabled);
      AssignMethod(delegates, "ScrollView_SetScrollWheelEnabled", ref apiDel.ScrollView_SetScrollWheelEnabled);
      AssignMethod(delegates, "ScrollView_GetScrollWheelStep", ref apiDel.ScrollView_GetScrollWheelStep);
      AssignMethod(delegates, "ScrollView_SetScrollWheelStep", ref apiDel.ScrollView_SetScrollWheelStep);
      AssignMethod(delegates, "ScrollView_GetScrollBar", ref apiDel.ScrollView_GetScrollBar);
      AssignMethod(delegates, "TouchApp_New", ref apiDel.TouchApp_New);
      AssignMethod(delegates, "TouchApp_GetScreen", ref apiDel.TouchApp_GetScreen);
      AssignMethod(delegates, "TouchApp_GetViewport", ref apiDel.TouchApp_GetViewport);
      AssignMethod(delegates, "TouchApp_GetCursor", ref apiDel.TouchApp_GetCursor);
      AssignMethod(delegates, "TouchApp_GetTheme", ref apiDel.TouchApp_GetTheme);
      AssignMethod(delegates, "TouchApp_GetDefaultBg", ref apiDel.TouchApp_GetDefaultBg);
      AssignMethod(delegates, "TouchApp_SetDefaultBg", ref apiDel.TouchApp_SetDefaultBg);
      AssignMethod(delegates, "EmptyButton_New", ref apiDel.EmptyButton_New);
      AssignMethod(delegates, "EmptyButton_GetHandler", ref apiDel.EmptyButton_GetHandler);
      AssignMethod(delegates, "EmptyButton_SetOnChange", ref apiDel.EmptyButton_SetOnChange);
      AssignMethod(delegates, "EmptyButton_GetDisabled", ref apiDel.EmptyButton_GetDisabled);
      AssignMethod(delegates, "EmptyButton_SetDisabled", ref apiDel.EmptyButton_SetDisabled);
      AssignMethod(delegates, "Button_New", ref apiDel.Button_New);
      AssignMethod(delegates, "Button_GetLabel", ref apiDel.Button_GetLabel);
      AssignMethod(delegates, "Checkbox_New", ref apiDel.Checkbox_New);
      AssignMethod(delegates, "Checkbox_GetValue", ref apiDel.Checkbox_GetValue);
      AssignMethod(delegates, "Checkbox_SetValue", ref apiDel.Checkbox_SetValue);
      AssignMethod(delegates, "Checkbox_SetOnChange", ref apiDel.Checkbox_SetOnChange);
      AssignMethod(delegates, "Checkbox_GetCheckMark", ref apiDel.Checkbox_GetCheckMark);
      AssignMethod(delegates, "Label_New", ref apiDel.Label_New);
      AssignMethod(delegates, "Label_GetAutoBreakLine", ref apiDel.Label_GetAutoBreakLine);
      AssignMethod(delegates, "Label_SetAutoBreakLine", ref apiDel.Label_SetAutoBreakLine);
      AssignMethod(delegates, "Label_GetAutoEllipsis", ref apiDel.Label_GetAutoEllipsis);
      AssignMethod(delegates, "Label_SetAutoEllipsis", ref apiDel.Label_SetAutoEllipsis);
      AssignMethod(delegates, "Label_GetHasEllipsis", ref apiDel.Label_GetHasEllipsis);
      AssignMethod(delegates, "Label_GetText", ref apiDel.Label_GetText);
      AssignMethod(delegates, "Label_SetText", ref apiDel.Label_SetText);
      AssignMethod(delegates, "Label_GetTextColor", ref apiDel.Label_GetTextColor);
      AssignMethod(delegates, "Label_SetTextColor", ref apiDel.Label_SetTextColor);
      AssignMethod(delegates, "Label_GetFontSize", ref apiDel.Label_GetFontSize);
      AssignMethod(delegates, "Label_SetFontSize", ref apiDel.Label_SetFontSize);
      AssignMethod(delegates, "Label_GetAlignment", ref apiDel.Label_GetAlignment);
      AssignMethod(delegates, "Label_SetAlignment", ref apiDel.Label_SetAlignment);
      AssignMethod(delegates, "Label_GetLines", ref apiDel.Label_GetLines);
      AssignMethod(delegates, "Label_GetMaxLines", ref apiDel.Label_GetMaxLines);
      AssignMethod(delegates, "Label_SetMaxLines", ref apiDel.Label_SetMaxLines);
      AssignMethod(delegates, "BarContainer_New", ref apiDel.BarContainer_New);
      AssignMethod(delegates, "BarContainer_GetIsVertical", ref apiDel.BarContainer_GetIsVertical);
      AssignMethod(delegates, "BarContainer_SetIsVertical", ref apiDel.BarContainer_SetIsVertical);
      AssignMethod(delegates, "BarContainer_GetRatio", ref apiDel.BarContainer_GetRatio);
      AssignMethod(delegates, "BarContainer_SetRatio", ref apiDel.BarContainer_SetRatio);
      AssignMethod(delegates, "BarContainer_GetOffset", ref apiDel.BarContainer_GetOffset);
      AssignMethod(delegates, "BarContainer_SetOffset", ref apiDel.BarContainer_SetOffset);
      AssignMethod(delegates, "BarContainer_GetBar", ref apiDel.BarContainer_GetBar);
      AssignMethod(delegates, "ProgressBar_New", ref apiDel.ProgressBar_New);
      AssignMethod(delegates, "ProgressBar_GetValue", ref apiDel.ProgressBar_GetValue);
      AssignMethod(delegates, "ProgressBar_SetValue", ref apiDel.ProgressBar_SetValue);
      AssignMethod(delegates, "ProgressBar_GetMaxValue", ref apiDel.ProgressBar_GetMaxValue);
      AssignMethod(delegates, "ProgressBar_SetMaxValue", ref apiDel.ProgressBar_SetMaxValue);
      AssignMethod(delegates, "ProgressBar_GetMinValue", ref apiDel.ProgressBar_GetMinValue);
      AssignMethod(delegates, "ProgressBar_SetMinValue", ref apiDel.ProgressBar_SetMinValue);
      AssignMethod(delegates, "ProgressBar_GetBarsGap", ref apiDel.ProgressBar_GetBarsGap);
      AssignMethod(delegates, "ProgressBar_SetBarsGap", ref apiDel.ProgressBar_SetBarsGap);
      AssignMethod(delegates, "ProgressBar_GetLabel", ref apiDel.ProgressBar_GetLabel);
      AssignMethod(delegates, "Selector_New", ref apiDel.Selector_New);
      AssignMethod(delegates, "Selector_GetLoop", ref apiDel.Selector_GetLoop);
      AssignMethod(delegates, "Selector_SetLoop", ref apiDel.Selector_SetLoop);
      AssignMethod(delegates, "Selector_GetSelected", ref apiDel.Selector_GetSelected);
      AssignMethod(delegates, "Selector_SetSelected", ref apiDel.Selector_SetSelected);
      AssignMethod(delegates, "Selector_SetOnChange", ref apiDel.Selector_SetOnChange);
      AssignMethod(delegates, "Slider_New", ref apiDel.Slider_New);
      AssignMethod(delegates, "Slider_GetMaxValue", ref apiDel.Slider_GetMaxValue);
      AssignMethod(delegates, "Slider_SetMaxValue", ref apiDel.Slider_SetMaxValue);
      AssignMethod(delegates, "Slider_GetValue", ref apiDel.Slider_GetValue);
      AssignMethod(delegates, "Slider_SetValue", ref apiDel.Slider_SetValue);
      AssignMethod(delegates, "Slider_SetOnChange", ref apiDel.Slider_SetOnChange);
      AssignMethod(delegates, "Slider_GetIsInteger", ref apiDel.Slider_GetIsInteger);
      AssignMethod(delegates, "Slider_SetIsInteger", ref apiDel.Slider_SetIsInteger);
      AssignMethod(delegates, "Slider_GetAllowInput", ref apiDel.Slider_GetAllowInput);
      AssignMethod(delegates, "Slider_SetAllowInput", ref apiDel.Slider_SetAllowInput);
      AssignMethod(delegates, "Slider_GetBar", ref apiDel.Slider_GetBar);
      AssignMethod(delegates, "Slider_GetThumb", ref apiDel.Slider_GetThumb);
      AssignMethod(delegates, "Slider_GetTextInput", ref apiDel.Slider_GetTextInput);
      AssignMethod(delegates, "SliderRange_NewR", ref apiDel.SliderRange_NewR);
      AssignMethod(delegates, "SliderRange_GetValueLower", ref apiDel.SliderRange_GetValueLower);
      AssignMethod(delegates, "SliderRange_SetValueLower", ref apiDel.SliderRange_SetValueLower);
      AssignMethod(delegates, "SliderRange_SetOnChangeR", ref apiDel.SliderRange_SetOnChangeR);
      AssignMethod(delegates, "SliderRange_GetThumbLower", ref apiDel.SliderRange_GetThumbLower);
      AssignMethod(delegates, "Switch_New", ref apiDel.Switch_New);
      AssignMethod(delegates, "Switch_GetIndex", ref apiDel.Switch_GetIndex);
      AssignMethod(delegates, "Switch_SetIndex", ref apiDel.Switch_SetIndex);
      AssignMethod(delegates, "Switch_GetButtons", ref apiDel.Switch_GetButtons);
      AssignMethod(delegates, "Switch_SetOnChange", ref apiDel.Switch_SetOnChange);
      AssignMethod(delegates, "TextField_New", ref apiDel.TextField_New);
      AssignMethod(delegates, "TextField_GetIsEditing", ref apiDel.TextField_GetIsEditing);
      AssignMethod(delegates, "TextField_GetText", ref apiDel.TextField_GetText);
      AssignMethod(delegates, "TextField_SetText", ref apiDel.TextField_SetText);
      AssignMethod(delegates, "TextField_SetOnSubmit", ref apiDel.TextField_SetOnSubmit);
      AssignMethod(delegates, "TextField_SetOnBlur", ref apiDel.TextField_SetOnBlur);
      AssignMethod(delegates, "TextField_GetRevertOnBlur", ref apiDel.TextField_GetRevertOnBlur);
      AssignMethod(delegates, "TextField_SetRevertOnBlur", ref apiDel.TextField_SetRevertOnBlur);
      AssignMethod(delegates, "TextField_GetSubmitOnBlur", ref apiDel.TextField_GetSubmitOnBlur);
      AssignMethod(delegates, "TextField_SetSubmitOnBlur", ref apiDel.TextField_SetSubmitOnBlur);
      AssignMethod(delegates, "TextField_GetIsNumeric", ref apiDel.TextField_GetIsNumeric);
      AssignMethod(delegates, "TextField_SetIsNumeric", ref apiDel.TextField_SetIsNumeric);
      AssignMethod(delegates, "TextField_GetIsInteger", ref apiDel.TextField_GetIsInteger);
      AssignMethod(delegates, "TextField_SetIsInteger", ref apiDel.TextField_SetIsInteger);
      AssignMethod(delegates, "TextField_GetAllowNegative", ref apiDel.TextField_GetAllowNegative);
      AssignMethod(delegates, "TextField_SetAllowNegative", ref apiDel.TextField_SetAllowNegative);
      AssignMethod(delegates, "TextField_GetLabel", ref apiDel.TextField_GetLabel);
      AssignMethod(delegates, "TextField_Blur", ref apiDel.TextField_Blur);
      AssignMethod(delegates, "TextField_Focus", ref apiDel.TextField_Focus);
      AssignMethod(delegates, "WindowBar_New", ref apiDel.WindowBar_New);
      AssignMethod(delegates, "WindowBar_GetLabel", ref apiDel.WindowBar_GetLabel);
      AssignMethod(delegates, "Chart_New", ref apiDel.Chart_New);
      AssignMethod(delegates, "Chart_GetDataSets", ref apiDel.Chart_GetDataSets);
      AssignMethod(delegates, "Chart_GetDataColors", ref apiDel.Chart_GetDataColors);
      AssignMethod(delegates, "Chart_GetGridHorizontalLines", ref apiDel.Chart_GetGridHorizontalLines);
      AssignMethod(delegates, "Chart_SetGridHorizontalLines", ref apiDel.Chart_SetGridHorizontalLines);
      AssignMethod(delegates, "Chart_GetGridVerticalLines", ref apiDel.Chart_GetGridVerticalLines);
      AssignMethod(delegates, "Chart_SetGridVerticalLines", ref apiDel.Chart_SetGridVerticalLines);
      AssignMethod(delegates, "Chart_GetMaxValue", ref apiDel.Chart_GetMaxValue);
      AssignMethod(delegates, "Chart_GetMinValue", ref apiDel.Chart_GetMinValue);
      AssignMethod(delegates, "Chart_GetGridColor", ref apiDel.Chart_GetGridColor);
      AssignMethod(delegates, "Chart_SetGridColor", ref apiDel.Chart_SetGridColor);
      AssignMethod(delegates, "EmptyElement_New", ref apiDel.EmptyElement_New);
    }
  }

  /// <summary>
  /// Holds delegates for all Touch Ui Kit features. Populated by when <see cref="TouchUiKit.Load"/> is called.
  /// </summary>
  public class UiKitDelegator : TouchApiDelegator
  {
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
  /// <see href="https://github.com/adrianulima/TouchScreenAPI/blob/main/Data/Scripts/Lima/Touch/UiKit/Elements/App.cs"/>
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
    public Action<string> OnBlur { set { Api.TextField_SetOnBlur.Invoke(InternalObj, value); } }
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