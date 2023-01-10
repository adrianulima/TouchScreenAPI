using Sandbox.Game.Entities;
using System.Collections.Generic;
using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

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
      ApiDelegator = new TouchUiKitDelegator();
    }

    public override bool Load()
    {
      WrapperBase<TouchUiKitDelegator>.SetApi(ApiDelegator as TouchUiKitDelegator);
      return base.Load();
    }

    public override void Unload()
    {
      WrapperBase<TouchUiKitDelegator>.SetApi(null);
      base.Unload();
    }

    protected override void ApiDelegates(IReadOnlyDictionary<string, Delegate> delegates)
    {
      base.ApiDelegates(delegates);
      var apiDel = ApiDelegator as TouchUiKitDelegator;

      AssignMethod(delegates, "TouchTheme_GetBgColor", ref apiDel.TouchTheme_GetBgColor);
      AssignMethod(delegates, "TouchTheme_GetWhiteColor", ref apiDel.TouchTheme_GetWhiteColor);
      AssignMethod(delegates, "TouchTheme_GetMainColor", ref apiDel.TouchTheme_GetMainColor);
      AssignMethod(delegates, "TouchTheme_GetMainColorDarker", ref apiDel.TouchTheme_GetMainColorDarker);
      AssignMethod(delegates, "TouchTheme_MeasureStringInPixels", ref apiDel.TouchTheme_MeasureStringInPixels);
      AssignMethod(delegates, "TouchTheme_GetScale", ref apiDel.TouchTheme_GetScale);
      AssignMethod(delegates, "TouchTheme_SetScale", ref apiDel.TouchTheme_SetScale);
      AssignMethod(delegates, "TouchTheme_GetFont", ref apiDel.TouchTheme_GetFont);
      AssignMethod(delegates, "TouchTheme_SetFont", ref apiDel.TouchTheme_SetFont);
      AssignMethod(delegates, "TouchElementBase_GetEnabled", ref apiDel.TouchElementBase_GetEnabled);
      AssignMethod(delegates, "TouchElementBase_SetEnabled", ref apiDel.TouchElementBase_SetEnabled);
      AssignMethod(delegates, "TouchElementBase_GetAbsolute", ref apiDel.TouchElementBase_GetAbsolute);
      AssignMethod(delegates, "TouchElementBase_SetAbsolute", ref apiDel.TouchElementBase_SetAbsolute);
      AssignMethod(delegates, "TouchElementBase_GetSelfAlignment", ref apiDel.TouchElementBase_GetSelfAlignment);
      AssignMethod(delegates, "TouchElementBase_SetSelfAlignment", ref apiDel.TouchElementBase_SetSelfAlignment);
      AssignMethod(delegates, "TouchElementBase_GetPosition", ref apiDel.TouchElementBase_GetPosition);
      AssignMethod(delegates, "TouchElementBase_SetPosition", ref apiDel.TouchElementBase_SetPosition);
      AssignMethod(delegates, "TouchElementBase_GetMargin", ref apiDel.TouchElementBase_GetMargin);
      AssignMethod(delegates, "TouchElementBase_SetMargin", ref apiDel.TouchElementBase_SetMargin);
      AssignMethod(delegates, "TouchElementBase_GetScale", ref apiDel.TouchElementBase_GetScale);
      AssignMethod(delegates, "TouchElementBase_SetScale", ref apiDel.TouchElementBase_SetScale);
      AssignMethod(delegates, "TouchElementBase_GetPixels", ref apiDel.TouchElementBase_GetPixels);
      AssignMethod(delegates, "TouchElementBase_SetPixels", ref apiDel.TouchElementBase_SetPixels);
      AssignMethod(delegates, "TouchElementBase_GetSize", ref apiDel.TouchElementBase_GetSize);
      AssignMethod(delegates, "TouchElementBase_GetBoundaries", ref apiDel.TouchElementBase_GetBoundaries);
      AssignMethod(delegates, "TouchElementBase_GetApp", ref apiDel.TouchElementBase_GetApp);
      AssignMethod(delegates, "TouchElementBase_GetParent", ref apiDel.TouchElementBase_GetParent);
      AssignMethod(delegates, "TouchElementBase_GetSprites", ref apiDel.TouchElementBase_GetSprites);
      AssignMethod(delegates, "TouchElementBase_ForceUpdate", ref apiDel.TouchElementBase_ForceUpdate);
      AssignMethod(delegates, "TouchElementBase_ForceDispose", ref apiDel.TouchElementBase_ForceDispose);
      AssignMethod(delegates, "TouchElementBase_RegisterUpdate", ref apiDel.TouchElementBase_RegisterUpdate);
      AssignMethod(delegates, "TouchElementBase_UnregisterUpdate", ref apiDel.TouchElementBase_UnregisterUpdate);
      AssignMethod(delegates, "TouchContainerBase_GetChildren", ref apiDel.TouchContainerBase_GetChildren);
      AssignMethod(delegates, "TouchContainerBase_GetFlexSize", ref apiDel.TouchContainerBase_GetFlexSize);
      AssignMethod(delegates, "TouchContainerBase_AddChild", ref apiDel.TouchContainerBase_AddChild);
      AssignMethod(delegates, "TouchContainerBase_AddChildAt", ref apiDel.TouchContainerBase_AddChildAt);
      AssignMethod(delegates, "TouchContainerBase_RemoveChild", ref apiDel.TouchContainerBase_RemoveChild);
      AssignMethod(delegates, "TouchContainerBase_RemoveChildAt", ref apiDel.TouchContainerBase_RemoveChildAt);
      AssignMethod(delegates, "TouchContainerBase_MoveChild", ref apiDel.TouchContainerBase_MoveChild);
      AssignMethod(delegates, "TouchView_New", ref apiDel.TouchView_New);
      AssignMethod(delegates, "TouchView_GetOverflow", ref apiDel.TouchView_GetOverflow);
      AssignMethod(delegates, "TouchView_SetOverflow", ref apiDel.TouchView_SetOverflow);
      AssignMethod(delegates, "TouchView_GetDirection", ref apiDel.TouchView_GetDirection);
      AssignMethod(delegates, "TouchView_SetDirection", ref apiDel.TouchView_SetDirection);
      AssignMethod(delegates, "TouchView_GetAlignment", ref apiDel.TouchView_GetAlignment);
      AssignMethod(delegates, "TouchView_SetAlignment", ref apiDel.TouchView_SetAlignment);
      AssignMethod(delegates, "TouchView_GetAnchor", ref apiDel.TouchView_GetAnchor);
      AssignMethod(delegates, "TouchView_SetAnchor", ref apiDel.TouchView_SetAnchor);
      AssignMethod(delegates, "TouchView_GetUseThemeColors", ref apiDel.TouchView_GetUseThemeColors);
      AssignMethod(delegates, "TouchView_SetUseThemeColors", ref apiDel.TouchView_SetUseThemeColors);
      AssignMethod(delegates, "TouchView_GetBgColor", ref apiDel.TouchView_GetBgColor);
      AssignMethod(delegates, "TouchView_SetBgColor", ref apiDel.TouchView_SetBgColor);
      AssignMethod(delegates, "TouchView_GetBorderColor", ref apiDel.TouchView_GetBorderColor);
      AssignMethod(delegates, "TouchView_SetBorderColor", ref apiDel.TouchView_SetBorderColor);
      AssignMethod(delegates, "TouchView_GetBorder", ref apiDel.TouchView_GetBorder);
      AssignMethod(delegates, "TouchView_SetBorder", ref apiDel.TouchView_SetBorder);
      AssignMethod(delegates, "TouchView_GetPadding", ref apiDel.TouchView_GetPadding);
      AssignMethod(delegates, "TouchView_SetPadding", ref apiDel.TouchView_SetPadding);
      AssignMethod(delegates, "TouchView_GetGap", ref apiDel.TouchView_GetGap);
      AssignMethod(delegates, "TouchView_SetGap", ref apiDel.TouchView_SetGap);
      AssignMethod(delegates, "TouchScrollView_New", ref apiDel.TouchScrollView_New);
      AssignMethod(delegates, "TouchScrollView_GetScroll", ref apiDel.TouchScrollView_GetScroll);
      AssignMethod(delegates, "TouchScrollView_SetScroll", ref apiDel.TouchScrollView_SetScroll);
      AssignMethod(delegates, "TouchScrollView_GetScrollAlwaysVisible", ref apiDel.TouchScrollView_GetScrollAlwaysVisible);
      AssignMethod(delegates, "TouchScrollView_SetScrollAlwaysVisible", ref apiDel.TouchScrollView_SetScrollAlwaysVisible);
      AssignMethod(delegates, "TouchScrollView_GetScrollBar", ref apiDel.TouchScrollView_GetScrollBar);
      AssignMethod(delegates, "TouchApp_New", ref apiDel.TouchApp_New);
      AssignMethod(delegates, "TouchApp_GetScreen", ref apiDel.TouchApp_GetScreen);
      AssignMethod(delegates, "TouchApp_GetViewport", ref apiDel.TouchApp_GetViewport);
      AssignMethod(delegates, "TouchApp_GetCursor", ref apiDel.TouchApp_GetCursor);
      AssignMethod(delegates, "TouchApp_GetTheme", ref apiDel.TouchApp_GetTheme);
      AssignMethod(delegates, "TouchApp_GetDefaultBg", ref apiDel.TouchApp_GetDefaultBg);
      AssignMethod(delegates, "TouchApp_SetDefaultBg", ref apiDel.TouchApp_SetDefaultBg);
      AssignMethod(delegates, "TouchApp_InitApp", ref apiDel.TouchApp_InitApp);
      AssignMethod(delegates, "TouchEmptyButton_New", ref apiDel.TouchEmptyButton_New);
      AssignMethod(delegates, "TouchEmptyButton_GetHandler", ref apiDel.TouchEmptyButton_GetHandler);
      AssignMethod(delegates, "TouchEmptyButton_SetOnChange", ref apiDel.TouchEmptyButton_SetOnChange);
      AssignMethod(delegates, "TouchButton_New", ref apiDel.TouchButton_New);
      AssignMethod(delegates, "TouchButton_GetLabel", ref apiDel.TouchButton_GetLabel);
      AssignMethod(delegates, "TouchCheckbox_New", ref apiDel.TouchCheckbox_New);
      AssignMethod(delegates, "TouchCheckbox_GetValue", ref apiDel.TouchCheckbox_GetValue);
      AssignMethod(delegates, "TouchCheckbox_SetValue", ref apiDel.TouchCheckbox_SetValue);
      AssignMethod(delegates, "TouchCheckbox_SetOnChange", ref apiDel.TouchCheckbox_SetOnChange);
      AssignMethod(delegates, "TouchCheckbox_GetCheckMark", ref apiDel.TouchCheckbox_GetCheckMark);
      AssignMethod(delegates, "TouchLabel_New", ref apiDel.TouchLabel_New);
      AssignMethod(delegates, "TouchLabel_GetAutoBreakLine", ref apiDel.TouchLabel_GetAutoBreakLine);
      AssignMethod(delegates, "TouchLabel_SetAutoBreakLine", ref apiDel.TouchLabel_SetAutoBreakLine);
      AssignMethod(delegates, "TouchLabel_GetOverflow", ref apiDel.TouchLabel_GetOverflow);
      AssignMethod(delegates, "TouchLabel_SetOverflow", ref apiDel.TouchLabel_SetOverflow);
      AssignMethod(delegates, "TouchLabel_GetIsShortened", ref apiDel.TouchLabel_GetIsShortened);
      AssignMethod(delegates, "TouchLabel_GetText", ref apiDel.TouchLabel_GetText);
      AssignMethod(delegates, "TouchLabel_SetText", ref apiDel.TouchLabel_SetText);
      AssignMethod(delegates, "TouchLabel_GetTextColor", ref apiDel.TouchLabel_GetTextColor);
      AssignMethod(delegates, "TouchLabel_SetTextColor", ref apiDel.TouchLabel_SetTextColor);
      AssignMethod(delegates, "TouchLabel_GetFontSize", ref apiDel.TouchLabel_GetFontSize);
      AssignMethod(delegates, "TouchLabel_SetFontSize", ref apiDel.TouchLabel_SetFontSize);
      AssignMethod(delegates, "TouchLabel_GetAlignment", ref apiDel.TouchLabel_GetAlignment);
      AssignMethod(delegates, "TouchLabel_SetAlignment", ref apiDel.TouchLabel_SetAlignment);
      AssignMethod(delegates, "TouchBarContainer_New", ref apiDel.TouchBarContainer_New);
      AssignMethod(delegates, "TouchBarContainer_GetIsVertical", ref apiDel.TouchBarContainer_GetIsVertical);
      AssignMethod(delegates, "TouchBarContainer_SetIsVertical", ref apiDel.TouchBarContainer_SetIsVertical);
      AssignMethod(delegates, "TouchBarContainer_GetRatio", ref apiDel.TouchBarContainer_GetRatio);
      AssignMethod(delegates, "TouchBarContainer_SetRatio", ref apiDel.TouchBarContainer_SetRatio);
      AssignMethod(delegates, "TouchBarContainer_GetOffset", ref apiDel.TouchBarContainer_GetOffset);
      AssignMethod(delegates, "TouchBarContainer_SetOffset", ref apiDel.TouchBarContainer_SetOffset);
      AssignMethod(delegates, "TouchBarContainer_GetBar", ref apiDel.TouchBarContainer_GetBar);
      AssignMethod(delegates, "TouchProgressBar_New", ref apiDel.TouchProgressBar_New);
      AssignMethod(delegates, "TouchProgressBar_GetValue", ref apiDel.TouchProgressBar_GetValue);
      AssignMethod(delegates, "TouchProgressBar_SetValue", ref apiDel.TouchProgressBar_SetValue);
      AssignMethod(delegates, "TouchProgressBar_GetMaxValue", ref apiDel.TouchProgressBar_GetMaxValue);
      AssignMethod(delegates, "TouchProgressBar_SetMaxValue", ref apiDel.TouchProgressBar_SetMaxValue);
      AssignMethod(delegates, "TouchProgressBar_GetMinValue", ref apiDel.TouchProgressBar_GetMinValue);
      AssignMethod(delegates, "TouchProgressBar_SetMinValue", ref apiDel.TouchProgressBar_SetMinValue);
      AssignMethod(delegates, "TouchProgressBar_GetBarsGap", ref apiDel.TouchProgressBar_GetBarsGap);
      AssignMethod(delegates, "TouchProgressBar_SetBarsGap", ref apiDel.TouchProgressBar_SetBarsGap);
      AssignMethod(delegates, "TouchProgressBar_GetLabel", ref apiDel.TouchProgressBar_GetLabel);
      AssignMethod(delegates, "TouchSelector_New", ref apiDel.TouchSelector_New);
      AssignMethod(delegates, "TouchSelector_GetLoop", ref apiDel.TouchSelector_GetLoop);
      AssignMethod(delegates, "TouchSelector_SetLoop", ref apiDel.TouchSelector_SetLoop);
      AssignMethod(delegates, "TouchSelector_GetSelected", ref apiDel.TouchSelector_GetSelected);
      AssignMethod(delegates, "TouchSelector_SetSelected", ref apiDel.TouchSelector_SetSelected);
      AssignMethod(delegates, "TouchSelector_SetOnChange", ref apiDel.TouchSelector_SetOnChange);
      AssignMethod(delegates, "TouchSlider_New", ref apiDel.TouchSlider_New);
      AssignMethod(delegates, "TouchSlider_GetMaxValue", ref apiDel.TouchSlider_GetMaxValue);
      AssignMethod(delegates, "TouchSlider_SetMaxValue", ref apiDel.TouchSlider_SetMaxValue);
      AssignMethod(delegates, "TouchSlider_GetValue", ref apiDel.TouchSlider_GetValue);
      AssignMethod(delegates, "TouchSlider_SetValue", ref apiDel.TouchSlider_SetValue);
      AssignMethod(delegates, "TouchSlider_SetOnChange", ref apiDel.TouchSlider_SetOnChange);
      AssignMethod(delegates, "TouchSlider_GetIsInteger", ref apiDel.TouchSlider_GetIsInteger);
      AssignMethod(delegates, "TouchSlider_SetIsInteger", ref apiDel.TouchSlider_SetIsInteger);
      AssignMethod(delegates, "TouchSlider_GetAllowInput", ref apiDel.TouchSlider_GetAllowInput);
      AssignMethod(delegates, "TouchSlider_SetAllowInput", ref apiDel.TouchSlider_SetAllowInput);
      AssignMethod(delegates, "TouchSlider_GetBar", ref apiDel.TouchSlider_GetBar);
      AssignMethod(delegates, "TouchSlider_GetThumb", ref apiDel.TouchSlider_GetThumb);
      AssignMethod(delegates, "TouchSlider_GetTextInput", ref apiDel.TouchSlider_GetTextInput);
      AssignMethod(delegates, "TouchSliderRange_NewR", ref apiDel.TouchSliderRange_NewR);
      AssignMethod(delegates, "TouchSliderRange_GetValueLower", ref apiDel.TouchSliderRange_GetValueLower);
      AssignMethod(delegates, "TouchSliderRange_SetValueLower", ref apiDel.TouchSliderRange_SetValueLower);
      AssignMethod(delegates, "TouchSliderRange_SetOnChangeR", ref apiDel.TouchSliderRange_SetOnChangeR);
      AssignMethod(delegates, "TouchSliderRange_GetThumbLower", ref apiDel.TouchSliderRange_GetThumbLower);
      AssignMethod(delegates, "TouchSwitch_New", ref apiDel.TouchSwitch_New);
      AssignMethod(delegates, "TouchSwitch_GetIndex", ref apiDel.TouchSwitch_GetIndex);
      AssignMethod(delegates, "TouchSwitch_SetIndex", ref apiDel.TouchSwitch_SetIndex);
      AssignMethod(delegates, "TouchSwitch_GetButtons", ref apiDel.TouchSwitch_GetButtons);
      AssignMethod(delegates, "TouchSwitch_SetOnChange", ref apiDel.TouchSwitch_SetOnChange);
      AssignMethod(delegates, "TouchTextField_New", ref apiDel.TouchTextField_New);
      AssignMethod(delegates, "TouchTextField_GetIsEditing", ref apiDel.TouchTextField_GetIsEditing);
      AssignMethod(delegates, "TouchTextField_GetText", ref apiDel.TouchTextField_GetText);
      AssignMethod(delegates, "TouchTextField_SetText", ref apiDel.TouchTextField_SetText);
      AssignMethod(delegates, "TouchTextField_SetOnChange", ref apiDel.TouchTextField_SetOnChange);
      AssignMethod(delegates, "TouchTextField_GetIsNumeric", ref apiDel.TouchTextField_GetIsNumeric);
      AssignMethod(delegates, "TouchTextField_SetIsNumeric", ref apiDel.TouchTextField_SetIsNumeric);
      AssignMethod(delegates, "TouchTextField_GetIsInteger", ref apiDel.TouchTextField_GetIsInteger);
      AssignMethod(delegates, "TouchTextField_SetIsInteger", ref apiDel.TouchTextField_SetIsInteger);
      AssignMethod(delegates, "TouchTextField_GetAllowNegative", ref apiDel.TouchTextField_GetAllowNegative);
      AssignMethod(delegates, "TouchTextField_SetAllowNegative", ref apiDel.TouchTextField_SetAllowNegative);
      AssignMethod(delegates, "TouchTextField_GetLabel", ref apiDel.TouchTextField_GetLabel);
      AssignMethod(delegates, "TouchWindowBar_New", ref apiDel.TouchWindowBar_New);
      AssignMethod(delegates, "TouchWindowBar_GetLabel", ref apiDel.TouchWindowBar_GetLabel);
      AssignMethod(delegates, "TouchChart_New", ref apiDel.TouchChart_New);
      AssignMethod(delegates, "TouchChart_GetDataSets", ref apiDel.TouchChart_GetDataSets);
      AssignMethod(delegates, "TouchChart_GetDataColors", ref apiDel.TouchChart_GetDataColors);
      AssignMethod(delegates, "TouchChart_GetGridHorizontalLines", ref apiDel.TouchChart_GetGridHorizontalLines);
      AssignMethod(delegates, "TouchChart_SetGridHorizontalLines", ref apiDel.TouchChart_SetGridHorizontalLines);
      AssignMethod(delegates, "TouchChart_GetGridVerticalLines", ref apiDel.TouchChart_GetGridVerticalLines);
      AssignMethod(delegates, "TouchChart_SetGridVerticalLines", ref apiDel.TouchChart_SetGridVerticalLines);
      AssignMethod(delegates, "TouchChart_GetMaxValue", ref apiDel.TouchChart_GetMaxValue);
      AssignMethod(delegates, "TouchChart_GetMinValue", ref apiDel.TouchChart_GetMinValue);
      AssignMethod(delegates, "TouchChart_GetGridColor", ref apiDel.TouchChart_GetGridColor);
      AssignMethod(delegates, "TouchChart_SetGridColor", ref apiDel.TouchChart_SetGridColor);
      AssignMethod(delegates, "TouchEmptyElement_New", ref apiDel.TouchEmptyElement_New);
    }
  }

  /// <summary>
  /// Holds delegates for all Touch Ui Kit features. Populated by when <see cref="TouchUiKit.Load"/> is called.
  /// </summary>
  public class TouchUiKitDelegator : TouchApiDelegator
  {
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

    public Func<int, Color?, object> TouchView_New;
    public Func<object, bool> TouchView_GetOverflow;
    public Action<object, bool> TouchView_SetOverflow;
    public Func<object, int> TouchView_GetDirection;
    public Action<object, int> TouchView_SetDirection;
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
    public Action<object, MyCubeBlock, Sandbox.ModAPI.Ingame.IMyTextSurface> TouchApp_InitApp;

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
    public Func<object, bool> TouchLabel_GetOverflow;
    public Action<object, bool> TouchLabel_SetOverflow;
    public Func<object, bool> TouchLabel_GetIsShortened;
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
    /// If false, the element won't be draw, useful if you want to hide and show but not destroy. Better than removing it.
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
    public TouchView(ViewDirection direction = ViewDirection.Column, Color? bgColor = null) : base(Api.TouchView_New((int)direction, bgColor)) { }
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
    public virtual void InitApp(MyCubeBlock block, Sandbox.ModAPI.Ingame.IMyTextSurface surface) => Api.TouchApp_InitApp.Invoke(InternalObj, block, surface);
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
    public bool Overflow { get { return Api.TouchLabel_GetOverflow.Invoke(InternalObj); } set { Api.TouchLabel_SetOverflow.Invoke(InternalObj, value); } }
    /// <summary>
    /// If <see cref="Overflow" /> is false and the text was shortened to fit the size.
    /// </summary>
    public bool IsShortened { get { return Api.TouchLabel_GetIsShortened.Invoke(InternalObj); } }
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