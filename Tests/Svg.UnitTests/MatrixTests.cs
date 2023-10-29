//

using NUnit.Framework;
using Svg.Transforms;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;

namespace Svg.UnitTests
{
    /// <summary>
    /// Test that basic matrix operations work. 
    /// this confirms that basic interop between system numerics matrix3x2 and drawing2d matrix works right
    /// Currently only tested on Windows
    /// </summary>
    [TestFixture]
    public class MatrixTest : SvgTestHelper
    {
        public static Matrix3x2 GetRotate(float angle)
        {
            return new Matrix3x2((float)Math.Cos(angle), (float)Math.Sin(angle), -(float)Math.Sin(angle), (float)Math.Cos(angle), 0, 0);
        }

        public static Matrix3x2 GetTranslate(SvgPoint p)
        {
            var t = Matrix3x2.Identity;
            t.M31 = p.X;
            t.M32 = p.Y;
            return t;
        }
        /// <summary>
        /// We should get a valid dpi (probably 72, 96 or similar).
        /// </summary>
        [Test]
        public void TestMatrixOps()
        {
            int width = 800;
            int height = 600;
            var base_doc = EmptySvg(width, height);
            var doc = base_doc.Children[0] as SvgFragment;

            var pts = new SvgPointCollection();
            pts.Add(new SvgPoint(30f, 10f));
            pts.Add(new SvgPoint(30f, 50f));
            pts.Add(new SvgPoint(40f, 70f));
            pts.Add(new SvgPoint(70f, 50f));
            pts.Add(new SvgPoint(70f, 10f));

            var pl = new SvgPolygon()
            {
                Points = pts,
            };
            pl.StrokeWidth = 1f;
            pl.Stroke = new SvgColourServer(Color.Red);
            pl.FillOpacity = 0;
            doc.Children.Add(pl);

            var pl2 = pl.DeepCopy();
            pl2.Transforms = new SvgTransformCollection();
            pl2.Stroke = new SvgColourServer(Color.Blue);

            var xlate = new SvgTranslate(50f, 80f);
            pl2.Transforms.Add(xlate);
            doc.Children.Add(pl2);

            var pl3 = pl2.DeepCopy();
            pl3.Transforms = new SvgTransformCollection();
            pl3.Stroke = new SvgColourServer(Color.Green);

            xlate = new SvgTranslate(50f, 80f);
            pl3.Transforms.Add(xlate);

            var rot = new SvgRotate(-45);
            pl3.Transforms.Add(rot);
            doc.Children.Add(pl3);

            var pl4 = pl.DeepCopy();
            pl4.Transforms = new SvgTransformCollection();
            pl4.Stroke = new SvgColourServer(Color.DeepPink);

            xlate = new SvgTranslate(400f, 300f);
            pl4.Transforms.Add(xlate);
            var scale = new SvgScale(-2, -3);
            pl4.Transforms.Add(scale);
            xlate = new SvgTranslate(100f, -80f);
            pl4.Transforms.Add(xlate);

            doc.Children.Add(pl4);

            var bitmap1 = new Bitmap(width, height);
            base_doc.Draw(bitmap1);


        }
        // TODO: the same above operations using named transforms directly
    }
}
