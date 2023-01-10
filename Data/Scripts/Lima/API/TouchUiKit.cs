using System;
using System.Collections.Generic;
using Sandbox.Game.Entities;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.API
{
  public class TouchUiKit : TouchScreenAPI
  {
    protected override string GetRequestString() { return "ApiRequestTouchAndUi"; }

    public override bool Load()
    {
      WrapperBase<TouchUiKit>.SetApi(this);
      return base.Load();
    }

    public override void Unload()
    {
      WrapperBase<TouchUiKit>.SetApi(null);
      base.Unload();
    }

    public override void ApiDelegates(IReadOnlyDictionary<string, Delegate> delegates)
    {
      base.ApiDelegates(delegates);

      AssignMethod(delegates, "TouchTheme_GetBgColor", ref TouchTheme_GetBgColor);
      AssignMethod(delegates, "TouchTheme_GetWhiteColor", ref TouchTheme_GetWhiteColor);
      AssignMethod(delegates, "TouchTheme_GetMainColor", ref TouchTheme_GetMainColor);
      AssignMethod(delegates, "TouchTheme_GetMainColorDarker", ref TouchTheme_GetMainColorDarker);
      AssignMethod(delegates, "TouchTheme_MeasureStringInPixels", ref TouchTheme_MeasureStringInPixels);
      AssignMethod(delegates, "TouchTheme_GetScale", ref TouchTheme_GetScale);
      AssignMethod(delegates, "TouchTheme_SetScale", ref TouchTheme_SetScale);
      AssignMethod(delegates, "TouchTheme_GetFont", ref TouchTheme_GetFont);
      AssignMethod(delegates, "TouchTheme_SetFont", ref TouchTheme_SetFont);
      AssignMethod(delegates, "TouchElementBase_GetEnabled", ref TouchElementBase_GetEnabled);
      AssignMethod(delegates, "TouchElementBase_SetEnabled", ref TouchElementBase_SetEnabled);
      AssignMethod(delegates, "TouchElementBase_GetAbsolute", ref TouchElementBase_GetAbsolute);
      AssignMethod(delegates, "TouchElementBase_SetAbsolute", ref TouchElementBase_SetAbsolute);
      AssignMethod(delegates, "TouchElementBase_GetSelfAlignment", ref TouchElementBase_GetSelfAlignment);
      AssignMethod(delegates, "TouchElementBase_SetSelfAlignment", ref TouchElementBase_SetSelfAlignment);
      AssignMethod(delegates, "TouchElementBase_GetPosition", ref TouchElementBase_GetPosition);
      AssignMethod(delegates, "TouchElementBase_SetPosition", ref TouchElementBase_SetPosition);
      AssignMethod(delegates, "TouchElementBase_GetMargin", ref TouchElementBase_GetMargin);
      AssignMethod(delegates, "TouchElementBase_SetMargin", ref TouchElementBase_SetMargin);
      AssignMethod(delegates, "TouchElementBase_GetScale", ref TouchElementBase_GetScale);
      AssignMethod(delegates, "TouchElementBase_SetScale", ref TouchElementBase_SetScale);
      AssignMethod(delegates, "TouchElementBase_GetPixels", ref TouchElementBase_GetPixels);
      AssignMethod(delegates, "TouchElementBase_SetPixels", ref TouchElementBase_SetPixels);
      AssignMethod(delegates, "TouchElementBase_GetSize", ref TouchElementBase_GetSize);
      AssignMethod(delegates, "TouchElementBase_GetBoundaries", ref TouchElementBase_GetBoundaries);
      AssignMethod(delegates, "TouchElementBase_GetApp", ref TouchElementBase_GetApp);
      AssignMethod(delegates, "TouchElementBase_GetParent", ref TouchElementBase_GetParent);
      AssignMethod(delegates, "TouchElementBase_GetSprites", ref TouchElementBase_GetSprites);
      AssignMethod(delegates, "TouchElementBase_ForceUpdate", ref TouchElementBase_ForceUpdate);
      AssignMethod(delegates, "TouchElementBase_ForceDispose", ref TouchElementBase_ForceDispose);
      AssignMethod(delegates, "TouchElementBase_RegisterUpdate", ref TouchElementBase_RegisterUpdate);
      AssignMethod(delegates, "TouchElementBase_UnregisterUpdate", ref TouchElementBase_UnregisterUpdate);
      AssignMethod(delegates, "TouchContainerBase_GetChildren", ref TouchContainerBase_GetChildren);
      AssignMethod(delegates, "TouchContainerBase_GetFlexSize", ref TouchContainerBase_GetFlexSize);
      AssignMethod(delegates, "TouchContainerBase_AddChild", ref TouchContainerBase_AddChild);
      AssignMethod(delegates, "TouchContainerBase_AddChildAt", ref TouchContainerBase_AddChildAt);
      AssignMethod(delegates, "TouchContainerBase_RemoveChild", ref TouchContainerBase_RemoveChild);
      AssignMethod(delegates, "TouchContainerBase_RemoveChildAt", ref TouchContainerBase_RemoveChildAt);
      AssignMethod(delegates, "TouchContainerBase_MoveChild", ref TouchContainerBase_MoveChild);
      AssignMethod(delegates, "TouchView_New", ref TouchView_New);
      AssignMethod(delegates, "TouchView_GetOverflow", ref TouchView_GetOverflow);
      AssignMethod(delegates, "TouchView_SetOverflow", ref TouchView_SetOverflow);
      AssignMethod(delegates, "TouchView_GetDirection", ref TouchView_GetDirection);
      AssignMethod(delegates, "TouchView_SetDirection", ref TouchView_SetDirection);
      AssignMethod(delegates, "TouchView_GetAlignment", ref TouchView_GetAlignment);
      AssignMethod(delegates, "TouchView_SetAlignment", ref TouchView_SetAlignment);
      AssignMethod(delegates, "TouchView_GetAnchor", ref TouchView_GetAnchor);
      AssignMethod(delegates, "TouchView_SetAnchor", ref TouchView_SetAnchor);
      AssignMethod(delegates, "TouchView_GetUseThemeColors", ref TouchView_GetUseThemeColors);
      AssignMethod(delegates, "TouchView_SetUseThemeColors", ref TouchView_SetUseThemeColors);
      AssignMethod(delegates, "TouchView_GetBgColor", ref TouchView_GetBgColor);
      AssignMethod(delegates, "TouchView_SetBgColor", ref TouchView_SetBgColor);
      AssignMethod(delegates, "TouchView_GetBorderColor", ref TouchView_GetBorderColor);
      AssignMethod(delegates, "TouchView_SetBorderColor", ref TouchView_SetBorderColor);
      AssignMethod(delegates, "TouchView_GetBorder", ref TouchView_GetBorder);
      AssignMethod(delegates, "TouchView_SetBorder", ref TouchView_SetBorder);
      AssignMethod(delegates, "TouchView_GetPadding", ref TouchView_GetPadding);
      AssignMethod(delegates, "TouchView_SetPadding", ref TouchView_SetPadding);
      AssignMethod(delegates, "TouchView_GetGap", ref TouchView_GetGap);
      AssignMethod(delegates, "TouchView_SetGap", ref TouchView_SetGap);
      AssignMethod(delegates, "TouchScrollView_New", ref TouchScrollView_New);
      AssignMethod(delegates, "TouchScrollView_GetScroll", ref TouchScrollView_GetScroll);
      AssignMethod(delegates, "TouchScrollView_SetScroll", ref TouchScrollView_SetScroll);
      AssignMethod(delegates, "TouchScrollView_GetScrollAlwaysVisible", ref TouchScrollView_GetScrollAlwaysVisible);
      AssignMethod(delegates, "TouchScrollView_SetScrollAlwaysVisible", ref TouchScrollView_SetScrollAlwaysVisible);
      AssignMethod(delegates, "TouchScrollView_GetScrollBar", ref TouchScrollView_GetScrollBar);
      AssignMethod(delegates, "TouchApp_New", ref TouchApp_New);
      AssignMethod(delegates, "TouchApp_GetScreen", ref TouchApp_GetScreen);
      AssignMethod(delegates, "TouchApp_GetViewport", ref TouchApp_GetViewport);
      AssignMethod(delegates, "TouchApp_GetCursor", ref TouchApp_GetCursor);
      AssignMethod(delegates, "TouchApp_GetTheme", ref TouchApp_GetTheme);
      AssignMethod(delegates, "TouchApp_GetDefaultBg", ref TouchApp_GetDefaultBg);
      AssignMethod(delegates, "TouchApp_SetDefaultBg", ref TouchApp_SetDefaultBg);
      AssignMethod(delegates, "TouchApp_InitApp", ref TouchApp_InitApp);
      AssignMethod(delegates, "TouchEmptyButton_New", ref TouchEmptyButton_New);
      AssignMethod(delegates, "TouchEmptyButton_GetHandler", ref TouchEmptyButton_GetHandler);
      AssignMethod(delegates, "TouchEmptyButton_SetOnChange", ref TouchEmptyButton_SetOnChange);
      AssignMethod(delegates, "TouchButton_New", ref TouchButton_New);
      AssignMethod(delegates, "TouchButton_GetLabel", ref TouchButton_GetLabel);
      AssignMethod(delegates, "TouchCheckbox_New", ref TouchCheckbox_New);
      AssignMethod(delegates, "TouchCheckbox_GetValue", ref TouchCheckbox_GetValue);
      AssignMethod(delegates, "TouchCheckbox_SetValue", ref TouchCheckbox_SetValue);
      AssignMethod(delegates, "TouchCheckbox_SetOnChange", ref TouchCheckbox_SetOnChange);
      AssignMethod(delegates, "TouchCheckbox_GetCheckMark", ref TouchCheckbox_GetCheckMark);
      AssignMethod(delegates, "TouchLabel_New", ref TouchLabel_New);
      AssignMethod(delegates, "TouchLabel_GetAutoBreakLine", ref TouchLabel_GetAutoBreakLine);
      AssignMethod(delegates, "TouchLabel_SetAutoBreakLine", ref TouchLabel_SetAutoBreakLine);
      AssignMethod(delegates, "TouchLabel_GetOverflow", ref TouchLabel_GetOverflow);
      AssignMethod(delegates, "TouchLabel_SetOverflow", ref TouchLabel_SetOverflow);
      AssignMethod(delegates, "TouchLabel_GetIsShortened", ref TouchLabel_GetIsShortened);
      AssignMethod(delegates, "TouchLabel_GetText", ref TouchLabel_GetText);
      AssignMethod(delegates, "TouchLabel_SetText", ref TouchLabel_SetText);
      AssignMethod(delegates, "TouchLabel_GetTextColor", ref TouchLabel_GetTextColor);
      AssignMethod(delegates, "TouchLabel_SetTextColor", ref TouchLabel_SetTextColor);
      AssignMethod(delegates, "TouchLabel_GetFontSize", ref TouchLabel_GetFontSize);
      AssignMethod(delegates, "TouchLabel_SetFontSize", ref TouchLabel_SetFontSize);
      AssignMethod(delegates, "TouchLabel_GetAlignment", ref TouchLabel_GetAlignment);
      AssignMethod(delegates, "TouchLabel_SetAlignment", ref TouchLabel_SetAlignment);
      AssignMethod(delegates, "TouchBarContainer_New", ref TouchBarContainer_New);
      AssignMethod(delegates, "TouchBarContainer_GetIsVertical", ref TouchBarContainer_GetIsVertical);
      AssignMethod(delegates, "TouchBarContainer_SetIsVertical", ref TouchBarContainer_SetIsVertical);
      AssignMethod(delegates, "TouchBarContainer_GetRatio", ref TouchBarContainer_GetRatio);
      AssignMethod(delegates, "TouchBarContainer_SetRatio", ref TouchBarContainer_SetRatio);
      AssignMethod(delegates, "TouchBarContainer_GetOffset", ref TouchBarContainer_GetOffset);
      AssignMethod(delegates, "TouchBarContainer_SetOffset", ref TouchBarContainer_SetOffset);
      AssignMethod(delegates, "TouchBarContainer_GetBar", ref TouchBarContainer_GetBar);
      AssignMethod(delegates, "TouchProgressBar_New", ref TouchProgressBar_New);
      AssignMethod(delegates, "TouchProgressBar_GetValue", ref TouchProgressBar_GetValue);
      AssignMethod(delegates, "TouchProgressBar_SetValue", ref TouchProgressBar_SetValue);
      AssignMethod(delegates, "TouchProgressBar_GetMaxValue", ref TouchProgressBar_GetMaxValue);
      AssignMethod(delegates, "TouchProgressBar_SetMaxValue", ref TouchProgressBar_SetMaxValue);
      AssignMethod(delegates, "TouchProgressBar_GetMinValue", ref TouchProgressBar_GetMinValue);
      AssignMethod(delegates, "TouchProgressBar_SetMinValue", ref TouchProgressBar_SetMinValue);
      AssignMethod(delegates, "TouchProgressBar_GetBarsGap", ref TouchProgressBar_GetBarsGap);
      AssignMethod(delegates, "TouchProgressBar_SetBarsGap", ref TouchProgressBar_SetBarsGap);
      AssignMethod(delegates, "TouchProgressBar_GetLabel", ref TouchProgressBar_GetLabel);
      AssignMethod(delegates, "TouchSelector_New", ref TouchSelector_New);
      AssignMethod(delegates, "TouchSelector_GetLoop", ref TouchSelector_GetLoop);
      AssignMethod(delegates, "TouchSelector_SetLoop", ref TouchSelector_SetLoop);
      AssignMethod(delegates, "TouchSelector_GetSelected", ref TouchSelector_GetSelected);
      AssignMethod(delegates, "TouchSelector_SetSelected", ref TouchSelector_SetSelected);
      AssignMethod(delegates, "TouchSelector_SetOnChange", ref TouchSelector_SetOnChange);
      AssignMethod(delegates, "TouchSlider_New", ref TouchSlider_New);
      AssignMethod(delegates, "TouchSlider_GetMaxValue", ref TouchSlider_GetMaxValue);
      AssignMethod(delegates, "TouchSlider_SetMaxValue", ref TouchSlider_SetMaxValue);
      AssignMethod(delegates, "TouchSlider_GetValue", ref TouchSlider_GetValue);
      AssignMethod(delegates, "TouchSlider_SetValue", ref TouchSlider_SetValue);
      AssignMethod(delegates, "TouchSlider_SetOnChange", ref TouchSlider_SetOnChange);
      AssignMethod(delegates, "TouchSlider_GetIsInteger", ref TouchSlider_GetIsInteger);
      AssignMethod(delegates, "TouchSlider_SetIsInteger", ref TouchSlider_SetIsInteger);
      AssignMethod(delegates, "TouchSlider_GetAllowInput", ref TouchSlider_GetAllowInput);
      AssignMethod(delegates, "TouchSlider_SetAllowInput", ref TouchSlider_SetAllowInput);
      AssignMethod(delegates, "TouchSlider_GetBar", ref TouchSlider_GetBar);
      AssignMethod(delegates, "TouchSlider_GetThumb", ref TouchSlider_GetThumb);
      AssignMethod(delegates, "TouchSlider_GetTextInput", ref TouchSlider_GetTextInput);
      AssignMethod(delegates, "TouchSliderRange_NewR", ref TouchSliderRange_NewR);
      AssignMethod(delegates, "TouchSliderRange_GetValueLower", ref TouchSliderRange_GetValueLower);
      AssignMethod(delegates, "TouchSliderRange_SetValueLower", ref TouchSliderRange_SetValueLower);
      AssignMethod(delegates, "TouchSliderRange_SetOnChangeR", ref TouchSliderRange_SetOnChangeR);
      AssignMethod(delegates, "TouchSliderRange_GetThumbLower", ref TouchSliderRange_GetThumbLower);
      AssignMethod(delegates, "TouchSwitch_New", ref TouchSwitch_New);
      AssignMethod(delegates, "TouchSwitch_GetIndex", ref TouchSwitch_GetIndex);
      AssignMethod(delegates, "TouchSwitch_SetIndex", ref TouchSwitch_SetIndex);
      AssignMethod(delegates, "TouchSwitch_GetButtons", ref TouchSwitch_GetButtons);
      AssignMethod(delegates, "TouchSwitch_SetOnChange", ref TouchSwitch_SetOnChange);
      AssignMethod(delegates, "TouchTextField_New", ref TouchTextField_New);
      AssignMethod(delegates, "TouchTextField_GetIsEditing", ref TouchTextField_GetIsEditing);
      AssignMethod(delegates, "TouchTextField_GetText", ref TouchTextField_GetText);
      AssignMethod(delegates, "TouchTextField_SetText", ref TouchTextField_SetText);
      AssignMethod(delegates, "TouchTextField_SetOnChange", ref TouchTextField_SetOnChange);
      AssignMethod(delegates, "TouchTextField_GetIsNumeric", ref TouchTextField_GetIsNumeric);
      AssignMethod(delegates, "TouchTextField_SetIsNumeric", ref TouchTextField_SetIsNumeric);
      AssignMethod(delegates, "TouchTextField_GetIsInteger", ref TouchTextField_GetIsInteger);
      AssignMethod(delegates, "TouchTextField_SetIsInteger", ref TouchTextField_SetIsInteger);
      AssignMethod(delegates, "TouchTextField_GetAllowNegative", ref TouchTextField_GetAllowNegative);
      AssignMethod(delegates, "TouchTextField_SetAllowNegative", ref TouchTextField_SetAllowNegative);
      AssignMethod(delegates, "TouchTextField_GetLabel", ref TouchTextField_GetLabel);
      AssignMethod(delegates, "TouchWindowBar_New", ref TouchWindowBar_New);
      AssignMethod(delegates, "TouchWindowBar_GetLabel", ref TouchWindowBar_GetLabel);
      AssignMethod(delegates, "TouchChart_New", ref TouchChart_New);
      AssignMethod(delegates, "TouchChart_GetDataSets", ref TouchChart_GetDataSets);
      AssignMethod(delegates, "TouchChart_GetDataColors", ref TouchChart_GetDataColors);
      AssignMethod(delegates, "TouchChart_GetGridHorizontalLines", ref TouchChart_GetGridHorizontalLines);
      AssignMethod(delegates, "TouchChart_SetGridHorizontalLines", ref TouchChart_SetGridHorizontalLines);
      AssignMethod(delegates, "TouchChart_GetGridVerticalLines", ref TouchChart_GetGridVerticalLines);
      AssignMethod(delegates, "TouchChart_SetGridVerticalLines", ref TouchChart_SetGridVerticalLines);
      AssignMethod(delegates, "TouchChart_GetMaxValue", ref TouchChart_GetMaxValue);
      AssignMethod(delegates, "TouchChart_GetMinValue", ref TouchChart_GetMinValue);
      AssignMethod(delegates, "TouchChart_GetGridColor", ref TouchChart_GetGridColor);
      AssignMethod(delegates, "TouchChart_SetGridColor", ref TouchChart_SetGridColor);
      AssignMethod(delegates, "TouchEmptyElement_New", ref TouchEmptyElement_New);
    }

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

  public class TouchTheme : WrapperBase<TouchUiKit>
  {
    public TouchTheme(object internalObject) : base(internalObject) { }
    public Color BgColor { get { return Api.TouchTheme_GetBgColor.Invoke(InternalObj); } }
    public Color WhiteColor { get { return Api.TouchTheme_GetWhiteColor.Invoke(InternalObj); } }
    public Color MainColor { get { return Api.TouchTheme_GetMainColor.Invoke(InternalObj); } }
    public Color GetMainColorDarker(int value) => Api.TouchTheme_GetMainColorDarker.Invoke(InternalObj, value);
    public float Scale { get { return Api.TouchTheme_GetScale.Invoke(InternalObj); } set { Api.TouchTheme_SetScale.Invoke(InternalObj, value); } }
    public string Font { get { return Api.TouchTheme_GetFont.Invoke(InternalObj); } set { Api.TouchTheme_SetFont.Invoke(InternalObj, value); } }
    public Vector2 MeasureStringInPixels(string text, string font, float scale) => Api.TouchTheme_MeasureStringInPixels.Invoke(InternalObj, text, font, scale);
  }
  public abstract class TouchElementBase : WrapperBase<TouchUiKit>
  {
    private TouchApp _app;
    private TouchView _parent;
    public TouchElementBase(object internalObject) : base(internalObject) { }
    public bool Enabled { get { return Api.TouchElementBase_GetEnabled.Invoke(InternalObj); } set { Api.TouchElementBase_SetEnabled.Invoke(InternalObj, value); } }
    public bool Absolute { get { return Api.TouchElementBase_GetAbsolute.Invoke(InternalObj); } set { Api.TouchElementBase_SetAbsolute.Invoke(InternalObj, value); } }
    public ViewAlignment SelfAlignment { get { return (ViewAlignment)Api.TouchElementBase_GetSelfAlignment.Invoke(InternalObj); } set { Api.TouchElementBase_SetSelfAlignment.Invoke(InternalObj, (byte)value); } }
    public Vector2 Position { get { return Api.TouchElementBase_GetPosition.Invoke(InternalObj); } set { Api.TouchElementBase_SetPosition.Invoke(InternalObj, value); } }
    public Vector4 Margin { get { return Api.TouchElementBase_GetMargin.Invoke(InternalObj); } set { Api.TouchElementBase_SetMargin.Invoke(InternalObj, value); } }
    public Vector2 Scale { get { return Api.TouchElementBase_GetScale.Invoke(InternalObj); } set { Api.TouchElementBase_SetScale.Invoke(InternalObj, value); } }
    public Vector2 Pixels { get { return Api.TouchElementBase_GetPixels.Invoke(InternalObj); } set { Api.TouchElementBase_SetPixels.Invoke(InternalObj, value); } }
    public TouchApp App { get { return _app ?? (_app = Wrap<TouchApp>(Api.TouchElementBase_GetApp.Invoke(InternalObj), (obj) => new TouchApp(obj))); } }
    public TouchView Parent { get { return _parent ?? (_parent = Wrap<TouchView>(Api.TouchElementBase_GetParent.Invoke(InternalObj), (obj) => new TouchView(obj))); } }
    public List<MySprite> GetSprites() => Api.TouchElementBase_GetSprites.Invoke(InternalObj);
    public Vector2 GetSize() => Api.TouchElementBase_GetSize.Invoke(InternalObj);
    public Vector2 GetBoundaries() => Api.TouchElementBase_GetBoundaries.Invoke(InternalObj);
    public void ForceUpdate() => Api.TouchElementBase_ForceUpdate.Invoke(InternalObj);
    public void ForceDispose() => Api.TouchElementBase_ForceDispose.Invoke(InternalObj);
    public void RegisterUpdate(Action update) => Api.TouchElementBase_RegisterUpdate.Invoke(InternalObj, update);
    public void UnregisterUpdate(Action update) => Api.TouchElementBase_UnregisterUpdate.Invoke(InternalObj, update);
  }
  public abstract class TouchContainerBase : TouchElementBase
  {
    public TouchContainerBase(object internalObject) : base(internalObject) { }
    public List<object> Children { get { return Api.TouchContainerBase_GetChildren.Invoke(InternalObj); } }
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
  public class TouchView : TouchContainerBase
  {
    public TouchView(ViewDirection direction = ViewDirection.Column, Color? bgColor = null) : base(Api.TouchView_New((int)direction, bgColor)) { }
    public TouchView(object internalObject) : base(internalObject) { }
    public bool Overflow { get { return Api.TouchView_GetOverflow.Invoke(InternalObj); } set { Api.TouchView_SetOverflow.Invoke(InternalObj, value); } }
    public ViewDirection Direction { get { return (ViewDirection)Api.TouchView_GetDirection.Invoke(InternalObj); } set { Api.TouchView_SetDirection.Invoke(InternalObj, (byte)value); } }
    public ViewAlignment Alignment { get { return (ViewAlignment)Api.TouchView_GetAlignment.Invoke(InternalObj); } set { Api.TouchView_SetAlignment.Invoke(InternalObj, (byte)value); } }
    public ViewAnchor Anchor { get { return (ViewAnchor)Api.TouchView_GetAnchor.Invoke(InternalObj); } set { Api.TouchView_SetAnchor.Invoke(InternalObj, (byte)value); } }
    public bool UseThemeColors { get { return Api.TouchView_GetUseThemeColors.Invoke(InternalObj); } set { Api.TouchView_SetUseThemeColors.Invoke(InternalObj, value); } }
    public Color BgColor { get { return Api.TouchView_GetBgColor.Invoke(InternalObj); } set { Api.TouchView_SetBgColor.Invoke(InternalObj, value); } }
    public Color BorderColor { get { return Api.TouchView_GetBorderColor.Invoke(InternalObj); } set { Api.TouchView_SetBorderColor.Invoke(InternalObj, value); } }
    public Vector4 Border { get { return Api.TouchView_GetBorder.Invoke(InternalObj); } set { Api.TouchView_SetBorder.Invoke(InternalObj, value); } }
    public Vector4 Padding { get { return Api.TouchView_GetPadding.Invoke(InternalObj); } set { Api.TouchView_SetPadding.Invoke(InternalObj, value); } }
    public int Gap { get { return Api.TouchView_GetGap.Invoke(InternalObj); } set { Api.TouchView_SetGap.Invoke(InternalObj, value); } }
  }
  public class TouchScrollView : TouchView
  {
    private TouchBarContainer _scrollBar;
    public TouchScrollView(ViewDirection direction = ViewDirection.Column, Color? bgColor = null) : base(Api.TouchScrollView_New((int)direction, bgColor)) { }
    public float Scroll { get { return Api.TouchScrollView_GetScroll.Invoke(InternalObj); } set { Api.TouchScrollView_SetScroll.Invoke(InternalObj, value); } }
    public bool ScrollAlwaysVisible { get { return Api.TouchScrollView_GetScrollAlwaysVisible.Invoke(InternalObj); } set { Api.TouchScrollView_SetScrollAlwaysVisible.Invoke(InternalObj, value); } }
    public TouchBarContainer ScrollBar { get { return _scrollBar ?? (_scrollBar = Wrap<TouchBarContainer>(Api.TouchScrollView_GetScrollBar.Invoke(InternalObj), (obj) => new TouchBarContainer(obj))); } }
  }
  public class TouchApp : TouchView
  {
    private TouchScreen _screen;
    private TouchCursor _cursor;
    private TouchTheme _theme;
    public TouchApp() : base(Api.TouchApp_New()) { }
    public TouchApp(object internalObject) : base(internalObject) { }
    public TouchScreen Screen { get { return _screen ?? (_screen = Wrap<TouchScreen>(Api.TouchApp_GetScreen.Invoke(InternalObj), (obj) => new TouchScreen(obj))); } }
    public RectangleF Viewport { get { return Api.TouchApp_GetViewport.Invoke(InternalObj); } }
    public TouchCursor Cursor { get { return _cursor ?? (_cursor = Wrap<TouchCursor>(Api.TouchApp_GetCursor.Invoke(InternalObj), (obj) => new TouchCursor(obj))); } }
    public TouchTheme Theme { get { return _theme ?? (_theme = Wrap<TouchTheme>(Api.TouchApp_GetTheme.Invoke(InternalObj), (obj) => new TouchTheme(obj))); } }
    public bool DefaultBg { get { return Api.TouchApp_GetDefaultBg.Invoke(InternalObj); } set { Api.TouchApp_SetDefaultBg.Invoke(InternalObj, value); } }
    public virtual void InitApp(MyCubeBlock block, Sandbox.ModAPI.Ingame.IMyTextSurface surface) => Api.TouchApp_InitApp.Invoke(InternalObj, block, surface);
  }
  public class TouchEmptyButton : TouchView
  {
    private ClickHandler _handler;
    public TouchEmptyButton(Action onChange) : base(Api.TouchEmptyButton_New(onChange)) { }
    public TouchEmptyButton(object internalObject) : base(internalObject) { }
    public ClickHandler Handler { get { return _handler ?? (_handler = Wrap<ClickHandler>(Api.TouchEmptyButton_GetHandler.Invoke(InternalObj), (obj) => new ClickHandler(obj))); } }
    public Action OnChange { set { Api.TouchEmptyButton_SetOnChange.Invoke(InternalObj, value); } }
  }
  public class TouchButton : TouchEmptyButton
  {
    private TouchLabel _label;
    public TouchButton(string text, Action onChange) : base(Api.TouchButton_New(text, onChange)) { }
    public TouchButton(object internalObject) : base(internalObject) { }
    public TouchLabel Label { get { return _label ?? (_label = Wrap<TouchLabel>(Api.TouchButton_GetLabel.Invoke(InternalObj), (obj) => new TouchLabel(obj))); } }
  }
  public class TouchCheckbox : TouchView
  {
    private TouchEmptyElement _checkMark;
    public TouchCheckbox(Action<bool> onChange, bool value = false) : base(Api.TouchCheckbox_New(onChange, value)) { }
    public TouchCheckbox(object internalObject) : base(internalObject) { }
    public bool Value { get { return Api.TouchCheckbox_GetValue.Invoke(InternalObj); } set { Api.TouchCheckbox_SetValue.Invoke(InternalObj, value); } }
    public Action<bool> OnChange { set { Api.TouchCheckbox_SetOnChange.Invoke(InternalObj, value); } }
    public TouchEmptyElement CheckMark { get { return _checkMark ?? (_checkMark = Wrap<TouchEmptyElement>(Api.TouchCheckbox_GetCheckMark.Invoke(InternalObj), (obj) => new TouchEmptyElement(obj))); } }
  }
  public class TouchLabel : TouchElementBase
  {
    public TouchLabel(string text, float fontSize = 0.5f, TextAlignment alignment = TextAlignment.CENTER) : base(Api.TouchLabel_New(text, fontSize, alignment)) { }
    public TouchLabel(object internalObject) : base(internalObject) { }
    public bool AutoBreakLine { get { return Api.TouchLabel_GetAutoBreakLine.Invoke(InternalObj); } set { Api.TouchLabel_SetAutoBreakLine.Invoke(InternalObj, value); } }
    public bool Overflow { get { return Api.TouchLabel_GetOverflow.Invoke(InternalObj); } set { Api.TouchLabel_SetOverflow.Invoke(InternalObj, value); } }
    public bool IsShortened { get { return Api.TouchLabel_GetIsShortened.Invoke(InternalObj); } }
    public string Text { get { return Api.TouchLabel_GetText.Invoke(InternalObj); } set { Api.TouchLabel_SetText.Invoke(InternalObj, value); } }
    public Color? TextColor { get { return Api.TouchLabel_GetTextColor.Invoke(InternalObj); } set { Api.TouchLabel_SetTextColor.Invoke(InternalObj, (Color)value); } }
    public float FontSize { get { return Api.TouchLabel_GetFontSize.Invoke(InternalObj); } set { Api.TouchLabel_SetFontSize.Invoke(InternalObj, value); } }
    public TextAlignment Alignment { get { return Api.TouchLabel_GetAlignment.Invoke(InternalObj); } set { Api.TouchLabel_SetAlignment.Invoke(InternalObj, value); } }
  }
  public class TouchBarContainer : TouchView
  {
    private TouchView _bar;
    public TouchBarContainer(bool vertical = false) : base(Api.TouchBarContainer_New(vertical)) { }
    public TouchBarContainer(object internalObject) : base(internalObject) { }
    public bool IsVertical { get { return Api.TouchBarContainer_GetIsVertical.Invoke(InternalObj); } set { Api.TouchBarContainer_SetIsVertical.Invoke(InternalObj, value); } }
    public float Ratio { get { return Api.TouchBarContainer_GetRatio.Invoke(InternalObj); } set { Api.TouchBarContainer_SetRatio.Invoke(InternalObj, value); } }
    public float Offset { get { return Api.TouchBarContainer_GetOffset.Invoke(InternalObj); } set { Api.TouchBarContainer_SetOffset.Invoke(InternalObj, value); } }
    public TouchView Bar { get { return _bar ?? (_bar = Wrap<TouchView>(Api.TouchBarContainer_GetBar.Invoke(InternalObj), (obj) => new TouchView(obj))); } }
  }
  public class TouchProgressBar : TouchBarContainer
  {
    private TouchLabel _label;
    public TouchProgressBar(float min, float max, bool vertical = false, float barsGap = 0) : base(Api.TouchProgressBar_New(min, max, vertical, barsGap)) { }
    public TouchProgressBar(object internalObject) : base(internalObject) { }
    public float Value { get { return Api.TouchProgressBar_GetValue.Invoke(InternalObj); } set { Api.TouchProgressBar_SetValue.Invoke(InternalObj, value); } }
    public float MaxValue { get { return Api.TouchProgressBar_GetMaxValue.Invoke(InternalObj); } set { Api.TouchProgressBar_SetMaxValue.Invoke(InternalObj, value); } }
    public float MinValue { get { return Api.TouchProgressBar_GetMinValue.Invoke(InternalObj); } set { Api.TouchProgressBar_SetMinValue.Invoke(InternalObj, value); } }
    public float BarsGap { get { return Api.TouchProgressBar_GetBarsGap.Invoke(InternalObj); } set { Api.TouchProgressBar_SetBarsGap.Invoke(InternalObj, value); } }
    public TouchLabel Label { get { return _label ?? (_label = Wrap<TouchLabel>(Api.TouchProgressBar_GetLabel.Invoke(InternalObj), (obj) => new TouchLabel(obj))); } }
  }
  public class TouchSelector : TouchView
  {
    public TouchSelector(List<string> labels, Action<int, string> onChange, bool loop = true) : base(Api.TouchSelector_New(labels, onChange, loop)) { }
    public TouchSelector(object internalObject) : base(internalObject) { }
    public bool Loop { get { return Api.TouchSelector_GetLoop.Invoke(InternalObj); } set { Api.TouchSelector_SetLoop.Invoke(InternalObj, value); } }
    public int Selected { get { return Api.TouchSelector_GetSelected.Invoke(InternalObj); } set { Api.TouchSelector_SetSelected.Invoke(InternalObj, value); } }
    public Action<int, string> OnChange { set { Api.TouchSelector_SetOnChange.Invoke(InternalObj, value); } }
  }
  public class TouchSlider : TouchView
  {
    private TouchBarContainer _bar;
    private TouchEmptyElement _thumb;
    private TouchTextField _textInput;
    public TouchSlider(float min, float max, Action<float> onChange) : base(Api.TouchSlider_New(min, max, onChange)) { }
    public TouchSlider(object internalObject) : base(internalObject) { }
    public float MaxValue { get { return Api.TouchSlider_GetMaxValue.Invoke(InternalObj); } set { Api.TouchSlider_SetMaxValue.Invoke(InternalObj, value); } }
    public float MinValue { get { return Api.TouchSlider_GetMinValue.Invoke(InternalObj); } set { Api.TouchSlider_SetMinValue.Invoke(InternalObj, value); } }
    public float Value { get { return Api.TouchSlider_GetValue.Invoke(InternalObj); } set { Api.TouchSlider_SetValue.Invoke(InternalObj, value); } }
    public bool IsInteger { get { return Api.TouchSlider_GetIsInteger.Invoke(InternalObj); } set { Api.TouchSlider_SetIsInteger.Invoke(InternalObj, value); } }
    public bool AllowInput { get { return Api.TouchSlider_GetAllowInput.Invoke(InternalObj); } set { Api.TouchSlider_SetAllowInput.Invoke(InternalObj, value); } }
    public Action<float> OnChange { set { Api.TouchSlider_SetOnChange.Invoke(InternalObj, value); } }
    public TouchBarContainer Bar { get { return _bar ?? (_bar = Wrap<TouchBarContainer>(Api.TouchSlider_GetBar.Invoke(InternalObj), (obj) => new TouchBarContainer(obj))); } }
    public TouchEmptyElement Thumb { get { return _thumb ?? (_thumb = Wrap<TouchEmptyElement>(Api.TouchSlider_GetThumb.Invoke(InternalObj), (obj) => new TouchEmptyElement(obj))); } }
    public TouchTextField TextInput { get { return _textInput ?? (_textInput = Wrap<TouchTextField>(Api.TouchSlider_GetTextInput.Invoke(InternalObj), (obj) => new TouchTextField(obj))); } }
  }
  public class TouchSliderRange : TouchSlider
  {
    private TouchEmptyElement _thumbLower;
    public TouchSliderRange(float min, float max, Action<float, float> onChange) : base(Api.TouchSliderRange_NewR(min, max, onChange)) { }
    public TouchSliderRange(object internalObject) : base(internalObject) { }
    public float ValueLower { get { return Api.TouchSliderRange_GetValueLower.Invoke(InternalObj); } set { Api.TouchSliderRange_SetValueLower.Invoke(InternalObj, value); } }
    public Action<float, float> OnChangeRange { set { Api.TouchSliderRange_SetOnChangeR.Invoke(InternalObj, value); } }
    public TouchEmptyElement ThumbLower { get { return _thumbLower ?? (_thumbLower = Wrap<TouchEmptyElement>(Api.TouchSliderRange_GetThumbLower.Invoke(InternalObj), (obj) => new TouchEmptyElement(obj))); } }
  }
  public class TouchSwitch : TouchView
  {
    private TouchButton[] _buttons;
    public TouchSwitch(string[] labels, int index = 0, Action<int> onChange = null) : base(Api.TouchSwitch_New(labels, index, onChange)) { }
    public TouchSwitch(object internalObject) : base(internalObject) { }
    public int Index { get { return Api.TouchSwitch_GetIndex.Invoke(InternalObj); } set { Api.TouchSwitch_SetIndex.Invoke(InternalObj, value); } }
    public TouchButton[] Buttons { get { return _buttons ?? (_buttons = WrapArray<TouchButton>(Api.TouchSwitch_GetButtons.Invoke(InternalObj), (obj) => new TouchButton(obj))); } }
    public Action<int> OnChange { set { Api.TouchSwitch_SetOnChange.Invoke(InternalObj, value); } }
  }
  public class TouchTextField : TouchView
  {
    private TouchLabel _label;
    public TouchTextField(string text, Action<string, bool> onChange) : base(Api.TouchTextField_New(text, onChange)) { }
    public TouchTextField(object internalObject) : base(internalObject) { }
    public bool IsEditing { get { return Api.TouchTextField_GetIsEditing.Invoke(InternalObj); } }
    public string Text { get { return Api.TouchTextField_GetText.Invoke(InternalObj); } set { Api.TouchTextField_SetText.Invoke(InternalObj, value); } }
    public bool IsNumeric { get { return Api.TouchTextField_GetIsNumeric.Invoke(InternalObj); } set { Api.TouchTextField_SetIsNumeric.Invoke(InternalObj, value); } }
    public bool IsInteger { get { return Api.TouchTextField_GetIsInteger.Invoke(InternalObj); } set { Api.TouchTextField_SetIsInteger.Invoke(InternalObj, value); } }
    public bool AllowNegative { get { return Api.TouchTextField_GetAllowNegative.Invoke(InternalObj); } set { Api.TouchTextField_SetAllowNegative.Invoke(InternalObj, value); } }
    public TouchLabel Label { get { return _label ?? (_label = Wrap<TouchLabel>(Api.TouchTextField_GetLabel.Invoke(InternalObj), (obj) => new TouchLabel(obj))); } }
    public Action<string, bool> OnChange { set { Api.TouchTextField_SetOnChange.Invoke(InternalObj, value); } }
  }
  public class TouchWindowBar : TouchView
  {
    private TouchLabel _label;
    public TouchWindowBar(string text) : base(Api.TouchWindowBar_New(text)) { }
    public TouchWindowBar(object internalObject) : base(internalObject) { }
    public TouchLabel Label { get { return _label ?? (_label = Wrap<TouchLabel>(Api.TouchWindowBar_GetLabel.Invoke(InternalObj), (obj) => new TouchLabel(obj))); } }
  }
  public class TouchChart : TouchElementBase
  {
    public TouchChart(int intervals) : base(Api.TouchChart_New(intervals)) { }
    public TouchChart(object internalObject) : base(internalObject) { }
    public List<float[]> DataSets { get { return Api.TouchChart_GetDataSets.Invoke(InternalObj); } }
    public List<Color> DataColors { get { return Api.TouchChart_GetDataColors.Invoke(InternalObj); } }
    public int GridHorizontalLines { get { return Api.TouchChart_GetGridHorizontalLines.Invoke(InternalObj); } set { Api.TouchChart_SetGridHorizontalLines.Invoke(InternalObj, value); } }
    public int GridVerticalLines { get { return Api.TouchChart_GetGridVerticalLines.Invoke(InternalObj); } set { Api.TouchChart_SetGridVerticalLines.Invoke(InternalObj, value); } }
    public float MaxValue { get { return Api.TouchChart_GetMaxValue.Invoke(InternalObj); } }
    public float MinValue { get { return Api.TouchChart_GetMinValue.Invoke(InternalObj); } }
    public Color? GridColor { get { return Api.TouchChart_GetGridColor.Invoke(InternalObj); } set { Api.TouchChart_SetGridColor.Invoke(InternalObj, (Color)value); } }
  }
  public class TouchEmptyElement : TouchElementBase
  {
    public TouchEmptyElement() : base(Api.TouchEmptyElement_New()) { }
    public TouchEmptyElement(object internalObject) : base(internalObject) { }
  }
}