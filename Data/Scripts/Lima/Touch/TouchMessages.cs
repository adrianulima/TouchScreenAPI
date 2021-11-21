using System;
using System.Collections.Generic;
using System.Linq;
using Lima.Fancy;
using Lima.Fancy.Elements;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Lima.Touch
{
  public static class TouchMessages
  {
    // TODO: Replace with proper TouchAPI mod id
    private const long _channel = 123;

    public static void SendApiToMods()
    {
      //Create a dictionary of delegates that point to methods in the Touch API code
      //Send the dictionary to other mods that registered to this ID
      MyAPIGateway.Utilities.SendModMessage(_channel, GetApiDictionary());
    }

    // TODO: Register this method
    private static void HandleMessage(object msg)
    {
      if ((msg as string) == "TouchApiEndpointRequest")
        MyAPIGateway.Utilities.SendModMessage(_channel, GetApiDictionary());
    }

    public static Dictionary<string, Delegate> GetApiDictionary()
    {
      var dict = new Dictionary<string, Delegate>();

      dict.Add("GetMaxInteractiveDistance", new Func<float>(GetMaxInteractiveDistance));
      dict.Add("SetMaxInteractiveDistance", new Action<float>(SetMaxInteractiveDistance));
      dict.Add("CreateTouchScreen", new Func<IMyCubeBlock, IMyTextSurface, TouchScreen>(CreateTouchScreen));
      dict.Add("RemoveTouchScreen", new Action<IMyCubeBlock, IMyTextSurface>(RemoveTouchScreen));
      dict.Add("AddSurfaceCoords", new Action<string>(AddSurfaceCoords));
      dict.Add("RemoveSurfaceCoords", new Action<string>(RemoveSurfaceCoords));

      dict.Add("TouchScreen_GetBlock", new Func<object, IMyCubeBlock>(TouchScreen_GetBlock));
      dict.Add("TouchScreen_GetSurface", new Func<object, IMyTextSurface>(TouchScreen_GetSurface));
      dict.Add("TouchScreen_GetIndex", new Func<object, int>(TouchScreen_GetIndex));
      dict.Add("TouchScreen_IsOnScreen", new Func<object, bool>(TouchScreen_IsOnScreen));
      dict.Add("TouchScreen_GetCursorPosition", new Func<object, Vector2>(TouchScreen_GetCursorPosition));
      dict.Add("TouchScreen_GetInteractiveDistance", new Func<object, float>(TouchScreen_GetInteractiveDistance));
      dict.Add("TouchScreen_SetInteractiveDistance", new Action<object, float>(TouchScreen_SetInteractiveDistance));
      dict.Add("TouchScreen_GetScreenRotate", new Func<object, int>(TouchScreen_GetScreenRotate));
      dict.Add("TouchScreen_CompareWithBlockAndSurface", new Func<object, IMyCubeBlock, IMyTextSurface, bool>(TouchScreen_CompareWithBlockAndSurface));
      dict.Add("TouchScreen_Dispose", new Action<object>(TouchScreen_Dispose));

      dict.Add("FancyElementBase_GetPosition", new Func<object, Vector2>(FancyElementBase_GetPosition));
      dict.Add("FancyElementBase_SetPosition", new Action<object, Vector2>(FancyElementBase_SetPosition));
      dict.Add("FancyElementBase_GetMargin", new Func<object, Vector4>(FancyElementBase_GetMargin));
      dict.Add("FancyElementBase_SetMargin", new Action<object, Vector4>(FancyElementBase_SetMargin));
      dict.Add("FancyElementBase_GetScale", new Func<object, Vector2>(FancyElementBase_GetScale));
      dict.Add("FancyElementBase_SetScale", new Action<object, Vector2>(FancyElementBase_SetScale));
      dict.Add("FancyElementBase_GetPixels", new Func<object, Vector2>(FancyElementBase_GetPixels));
      dict.Add("FancyElementBase_SetPixels", new Action<object, Vector2>(FancyElementBase_SetPixels));
      dict.Add("FancyElementBase_GetSize", new Func<object, Vector2>(FancyElementBase_GetSize));
      dict.Add("FancyElementBase_GetViewport", new Func<object, RectangleF>(FancyElementBase_GetViewport));
      dict.Add("FancyElementBase_GetApp", new Func<object, FancyApp>(FancyElementBase_GetApp));
      dict.Add("FancyElementBase_GetParent", new Func<object, FancyElementContainerBase>(FancyElementBase_GetParent));
      dict.Add("FancyElementBase_GetOffset", new Func<object, Vector2>(FancyElementBase_GetOffset));
      dict.Add("FancyElementBase_GetSprites", new Func<object, List<MySprite>>(FancyElementBase_GetSprites));
      dict.Add("FancyElementBase_InitElements", new Action<object>(FancyElementBase_InitElements));
      dict.Add("FancyElementBase_Update", new Action<object>(FancyElementBase_Update));
      dict.Add("FancyElementBase_Dispose", new Action<object>(FancyElementBase_Dispose));

      dict.Add("FancyElementContainerBase_GetChildren", new Func<object, List<object>>(FancyElementContainerBase_GetChildren));
      dict.Add("FancyElementContainerBase_AddChild", new Action<object, object>(FancyElementContainerBase_AddChild));
      dict.Add("FancyElementContainerBase_RemoveChild", new Action<object, object>(FancyElementContainerBase_RemoveChild));

      dict.Add("FancyView_GetDirection", new Func<object, int>(FancyView_GetDirection));
      dict.Add("FancyView_SetDirection", new Action<object, int>(FancyView_SetDirection));

      dict.Add("FancyApp_New", new Func<FancyApp>(FancyApp_New));
      dict.Add("FancyApp_GetScreen", new Func<object, TouchScreen>(FancyApp_GetScreen));
      dict.Add("FancyApp_GetCursor", new Func<object, FancyCursor>(FancyApp_GetCursor));
      dict.Add("FancyApp_GetTheme", new Func<object, FancyTheme>(FancyApp_GetTheme));
      dict.Add("FancyApp_InitApp", new Action<object, MyCubeBlock, Sandbox.ModAPI.Ingame.IMyTextSurface>(FancyApp_InitApp));

      dict.Add("FancyCursor_GetActive", new Func<object, bool>(FancyCursor_GetActive));
      dict.Add("FancyCursor_SetActive", new Action<object, bool>(FancyCursor_SetActive));
      dict.Add("FancyCursor_GetPosition", new Func<object, Vector2>(FancyCursor_GetPosition));
      dict.Add("FancyCursor_IsInsideArea", new Func<object, float, float, float, float, bool>(FancyCursor_IsInsideArea));

      dict.Add("FancyTheme_GetColorBg", new Func<object, Color>(FancyTheme_GetColorBg));
      dict.Add("FancyTheme_GetColorWhite", new Func<object, Color>(FancyTheme_GetColorWhite));
      dict.Add("FancyTheme_GetColorMain", new Func<object, Color>(FancyTheme_GetColorMain));
      dict.Add("FancyTheme_GetColorMainDarker", new Func<object, int, Color>(FancyTheme_GetColorMainDarker));
      dict.Add("FancyTheme_MeasureStringInPixels", new Func<object, String, string, float, Vector2>(FancyTheme_MeasureStringInPixels));

      return dict;
    }

    public static TouchScreen CreateTouchScreen(IMyCubeBlock block, IMyTextSurface surface)
    {
      var screen = new TouchScreen(block, surface);
      TouchManager.Instance.Screens.Add(screen);
      return screen;
    }

    public static void RemoveTouchScreen(IMyCubeBlock block, IMyTextSurface surface) => TouchManager.Instance.RemoveScreen(block, surface);
    public static List<TouchScreen> GetTouchScreensList() => TouchManager.Instance.Screens;
    public static TouchScreen GetTargetTouchScreen() => TouchManager.Instance.CurrentScreen;
    public static float GetMaxInteractiveDistance() => TouchManager.Instance.MaxInteractiveDistance;
    public static void SetMaxInteractiveDistance(float distance) => TouchManager.Instance.MaxInteractiveDistance = distance;
    public static void AddSurfaceCoords(string coords) => SurfaceCoordsManager.Instance.AddSurfaceCoords(coords);
    public static void RemoveSurfaceCoords(string coords)
    {
      var index = SurfaceCoordsManager.Instance.CoordsList.IndexOf(coords);
      if (index >= 0)
        SurfaceCoordsManager.Instance.CoordsList.RemoveAt(index);
    }

    static public IMyCubeBlock TouchScreen_GetBlock(object obj) => (obj as TouchScreen).Block;
    static public IMyTextSurface TouchScreen_GetSurface(object obj) => (obj as TouchScreen).Surface;
    static public int TouchScreen_GetIndex(object obj) => (obj as TouchScreen).Index;
    static public bool TouchScreen_IsOnScreen(object obj) => (obj as TouchScreen).IsOnScreen;
    static public Vector2 TouchScreen_GetCursorPosition(object obj) => (obj as TouchScreen).CursorPos;
    static public float TouchScreen_GetInteractiveDistance(object obj) => (obj as TouchScreen).InteractiveDistance;
    static public void TouchScreen_SetInteractiveDistance(object obj, float distance) => (obj as TouchScreen).InteractiveDistance = distance;
    static public int TouchScreen_GetScreenRotate(object obj) => (obj as TouchScreen).ScreenRotate;
    static public bool TouchScreen_CompareWithBlockAndSurface(object obj, IMyCubeBlock block, IMyTextSurface surface) => (obj as TouchScreen).CompareWithBlockAndSurface(block, surface);
    static public void TouchScreen_Dispose(object obj) => (obj as TouchScreen).Dispose();

    static public Vector2 FancyElementBase_GetPosition(object obj) => (obj as FancyElementBase).Position;
    static public void FancyElementBase_SetPosition(object obj, Vector2 position) => (obj as FancyElementBase).Position = position;
    static public Vector4 FancyElementBase_GetMargin(object obj) => (obj as FancyElementBase).Margin;
    static public void FancyElementBase_SetMargin(object obj, Vector4 margin) => (obj as FancyElementBase).Margin = margin;
    static public Vector2 FancyElementBase_GetScale(object obj) => (obj as FancyElementBase).Scale;
    static public void FancyElementBase_SetScale(object obj, Vector2 scale) => (obj as FancyElementBase).Scale = scale;
    static public Vector2 FancyElementBase_GetPixels(object obj) => (obj as FancyElementBase).Pixels;
    static public void FancyElementBase_SetPixels(object obj, Vector2 pixels) => (obj as FancyElementBase).Pixels = pixels;
    static public Vector2 FancyElementBase_GetSize(object obj) => (obj as FancyElementBase).Size;
    static public RectangleF FancyElementBase_GetViewport(object obj) => (obj as FancyElementBase).Viewport;
    static public FancyApp FancyElementBase_GetApp(object obj) => (obj as FancyElementBase).App;
    static public FancyElementContainerBase FancyElementBase_GetParent(object obj) => (obj as FancyElementBase).Parent;
    static public Vector2 FancyElementBase_GetOffset(object obj) => (obj as FancyElementBase).Offset;
    static public List<MySprite> FancyElementBase_GetSprites(object obj) => (obj as FancyElementBase).GetSprites();
    static public void FancyElementBase_InitElements(object obj) => (obj as FancyElementBase).InitElements();
    static public void FancyElementBase_Update(object obj) => (obj as FancyElementBase).Update();
    static public void FancyElementBase_Dispose(object obj) => (obj as FancyElementBase).Dispose();

    static public List<object> FancyElementContainerBase_GetChildren(object obj) => (obj as FancyElementContainerBase).children.Cast<object>().ToList();
    static public void FancyElementContainerBase_AddChild(object obj, object child) => (obj as FancyElementContainerBase).AddChild((FancyElementBase)child);
    static public void FancyElementContainerBase_RemoveChild(object obj, object child) => (obj as FancyElementContainerBase).RemoveChild((FancyElementBase)child);

    static public int FancyView_GetDirection(object obj) => (int)(obj as FancyView).Direction;
    static public void FancyView_SetDirection(object obj, int direction) => (obj as FancyView).Direction = (FancyView.ViewDirection)direction;

    static public FancyApp FancyApp_New() => new FancyApp();
    static public TouchScreen FancyApp_GetScreen(object obj) => (obj as FancyApp).Screen;
    static public FancyCursor FancyApp_GetCursor(object obj) => (obj as FancyApp).Cursor;
    static public FancyTheme FancyApp_GetTheme(object obj) => (obj as FancyApp).Theme;
    static public void FancyApp_InitApp(object obj, MyCubeBlock block, Sandbox.ModAPI.Ingame.IMyTextSurface surface) => (obj as FancyApp).InitApp(block, surface);

    static public bool FancyCursor_GetActive(object obj) => (obj as FancyCursor).Active;
    static public void FancyCursor_SetActive(object obj, bool active) => (obj as FancyCursor).Active = active;
    static public Vector2 FancyCursor_GetPosition(object obj) => (obj as FancyCursor).Position;
    static public bool FancyCursor_IsInsideArea(object obj, float x, float y, float z, float w) => (obj as FancyCursor).IsInsideArea(x, y, z, w);

    static public Color FancyTheme_GetColorBg(object obj) => (obj as FancyTheme).Bg;
    static public Color FancyTheme_GetColorWhite(object obj) => (obj as FancyTheme).White;
    static public Color FancyTheme_GetColorMain(object obj) => (obj as FancyTheme).Main;
    static public Color FancyTheme_GetColorMainDarker(object obj, int value)
    {
      var theme = (obj as FancyTheme);
      if (value <= 1) return theme.Main_10;
      else if (value <= 2) return theme.Main_20;
      else if (value <= 3) return theme.Main_30;
      else if (value <= 4) return theme.Main_40;
      else if (value <= 5) return theme.Main_50;
      else if (value <= 6) return theme.Main_60;
      else if (value <= 7) return theme.Main_70;
      else if (value <= 8) return theme.Main_80;
      return theme.Main_90;
    }
    static public Vector2 FancyTheme_MeasureStringInPixels(object obj, String text, string font, float scale) => (obj as FancyTheme).MeasureStringInPixels(text, font, scale);
  }
}