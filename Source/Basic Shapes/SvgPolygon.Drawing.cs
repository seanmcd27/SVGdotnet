﻿#if !NO_SDC
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace Svg
{
    public partial class SvgPolygon : SvgMarkerElement
    {
        private GraphicsPath _path;

        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if (this._path == null || this.IsPathDirty)
            {
                this._path = new GraphicsPath();
                this._path.StartFigure();

                try
                {
                    var points = this.Points;
                    for (int i = 0; i < points.Count; ++i)
                    {
                        var endPoint = SvgUnit.GetDevicePoint(points[i].X, points[i].Y, renderer, this);

                        // If it is to render, don't need to consider stroke width.
                        // i.e stroke width only to be considered when calculating boundary
                        if (renderer == null)
                        {
                            var radius = base.StrokeWidth.ToDeviceValue(renderer, UnitRenderingType.Other, this) * 2;
                            _path.AddEllipse(endPoint.X - radius, endPoint.Y - radius, 2 * radius, 2 * radius);
                            continue;
                        }

                        if (i == 0)
                            continue;
                        // first line
                        else if (_path.PointCount == 0)
                        {
                            _path.AddLine(SvgUnit.GetDevicePoint(points[i - 1].X, points[i - 1].Y, renderer, this), endPoint);
                        }
                        else
                        {
                            _path.AddLine(_path.GetLastPoint(), endPoint);
                        }
                    }
                }
                catch
                {
                    Trace.TraceError("Error parsing points");
                }

                this._path.CloseFigure();
                if (renderer != null)
                    this.IsPathDirty = false;
            }
            return this._path;
        }
    }
}
#endif
