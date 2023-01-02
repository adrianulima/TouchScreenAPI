using System;
using System.Collections.Generic;
using Lima.Utils;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyChart : FancyElementBase
  {
    public readonly List<float[]> DataSets = new List<float[]>();
    public readonly List<Color> DataColors = new List<Color>();

    private int _intervals;
    private Vector2 _cacheSize;

    private float _max;
    private float _min;

    public float MaxValue { get { return _max; } }
    public float MinValue { get { return _min; } }

    public int GridHorizontalLines = 2;
    public int GridVerticalLines = 2;

    public FancyChart(int intervals)
    {
      _intervals = intervals;
    }

    public override void Update()
    {
      base.Update();

      Sprites.Clear();
      _cacheSize = GetSize();

      if (GridHorizontalLines > 0) DrawHorizontalGridLines();
      if (GridVerticalLines > 0) DrawVerticalGridLines();

      UpdateLimitValues();
      DrawDataSets();
    }

    public override void Dispose()
    {
      base.Dispose();

      DataSets.Clear();
      DataColors.Clear();
    }

    private void UpdateLimitValues()
    {
      _max = float.MinValue;
      _min = float.MaxValue;
      for (int i = 0; i < DataSets.Count; i++)
      {
        if (DataSets[i] == null)
          continue;
        var intervalOffset = _intervals - DataSets[i].Length;
        var init = Math.Max(0, -intervalOffset);
        for (int p = init; p < DataSets[i].Length; p++)
        {
          _max = MathHelper.Max(_max, DataSets[i][p]);
          _min = MathHelper.Min(_min, DataSets[i][p]);
        }
      }
    }

    private void DrawDataSets()
    {
      for (int i = 0; i < DataSets.Count; i++)
      {
        if (DataSets[i] == null || DataSets[i].Length == 0)
          continue;
        var intervalOffset = _intervals - DataSets[i].Length;
        var init = Math.Max(0, -intervalOffset);
        var prevPoint = GetValuePosition(init + intervalOffset, DataSets[i][init]);
        for (int p = init + 1; p < DataSets[i].Length; p++)
        {
          var pos = Vector2.Zero;
          var size = Vector2.Zero;
          var angle = 0f;
          prevPoint = CalculateLineSprite(p + intervalOffset, DataSets[i][p], out pos, out size, out angle, prevPoint);

          var sprite = new MySprite()
          {
            Type = SpriteType.TEXTURE,
            Data = "SquareSimple",
            RotationOrScale = angle,
            Color = GetDataColor(i),
            Position = Position + pos + new Vector2(0, i * 0.5f),
            Size = size,
          };
          Sprites.Add(sprite);
        }
      }
    }

    private Vector2 CalculateLineSprite(int index, float value, out Vector2 pos, out Vector2 size, out float angle, Vector2 previous)
    {
      var p0 = previous;
      var p1 = GetValuePosition(index, value);

      var length = Vector2.Distance(p0, p1);

      pos = (p0 + p1) / 2;
      size = new Vector2(1, length);
      angle = (float)Math.Atan2(p1.Y - p0.Y, p1.X - p0.X) + MathHelper.PiOver2;

      return p1;
    }

    private Vector2 GetValuePosition(int index, float value)
    {
      var stepX = (_cacheSize.X - 1) / (_intervals - 1);
      return new Vector2(stepX * index, _cacheSize.Y - _cacheSize.Y * ((value - _min) / (_max - _min)));
    }

    private void DrawHorizontalGridLines()
    {
      var stepY = _cacheSize.Y / (GridHorizontalLines - 1);
      for (int i = 0; i < GridHorizontalLines; i++)
      {
        Sprites.Add(new MySprite()
        {
          Type = SpriteType.TEXTURE,
          Data = "SquareSimple",
          RotationOrScale = 0,
          Color = App.Theme.MainColor_2,
          Position = Position + new Vector2(0, stepY * i),
          Size = new Vector2(_cacheSize.X, 1)
        });
      }
    }

    private void DrawVerticalGridLines()
    {
      var stepX = (_cacheSize.X - 1) / (GridVerticalLines + 1);
      for (int i = 0; i < GridVerticalLines; i++)
      {
        Sprites.Add(new MySprite()
        {
          Type = SpriteType.TEXTURE,
          Data = "SquareSimple",
          RotationOrScale = 0,
          Color = App.Theme.MainColor_2,
          Position = Position + new Vector2(stepX * (i + 1), _cacheSize.Y / 2),
          Size = new Vector2(1, _cacheSize.Y)
        });
      }
    }

    private Color GetDataColor(int index)
    {
      while (DataColors.Count <= index)
      {
        if (index == 0)
        {
          DataColors.Add(App.Theme.MainColor);
          continue;
        }

        var mainR = App.Theme.MainColor.R;
        var mainG = App.Theme.MainColor.G;
        var mainB = App.Theme.MainColor.B;
        var r = MathHelper.Clamp(MathUtils.GetRandomInt(mainR - 50, mainR + 50), 0, 255);
        var g = MathHelper.Clamp(MathUtils.GetRandomInt(mainG - 50, mainG + 50), 0, 255);
        var b = MathHelper.Clamp(MathUtils.GetRandomInt(mainB - 50, mainB + 50), 0, 255);
        DataColors.Add(new Color(r, g, b, 255));
      }

      return DataColors[index];
    }
  }
}