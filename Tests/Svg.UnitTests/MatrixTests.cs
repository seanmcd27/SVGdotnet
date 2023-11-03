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
        protected override string TestResource { get { return GetFullResourceString("matrix.matrix.bmp"); } }

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

            // skewX
            var pl5 = pl.DeepCopy();
            pl5.Transforms = new SvgTransformCollection();
            pl5.Stroke = new SvgColourServer(Color.Purple);

            xlate = new SvgTranslate(75f, 100f);
            pl5.Transforms.Add(xlate);
            var skew = new SvgSkew(45, 0);
            pl5.Transforms.Add(skew);

            doc.Children.Add(pl5);

            // skewY
            var pl6 = pl.DeepCopy();
            pl6.Transforms = new SvgTransformCollection();
            pl6.Stroke = new SvgColourServer(Color.DarkGray);

            xlate = new SvgTranslate(150f, 100f);
            pl6.Transforms.Add(xlate);
            skew = new SvgSkew(0, 45);
            pl6.Transforms.Add(skew);

            doc.Children.Add(pl6);


            // skew X and Y
            var pl7 = pl.DeepCopy();
            pl7.Transforms = new SvgTransformCollection();
            pl7.Stroke = new SvgColourServer(Color.DarkGoldenrod);

            xlate = new SvgTranslate(225f, 100f);
            pl7.Transforms.Add(xlate);
            skew = new SvgSkew(45, 0);
            pl7.Transforms.Add(skew);
            skew = new SvgSkew(0, 45);
            pl7.Transforms.Add(skew);

            doc.Children.Add(pl7);

            // TODO: figure out what to do with the 'shear' transform

            // random other matrix


            var bitmap1 = new Bitmap(width, height);
            base_doc.Draw(bitmap1);

            var stream = GetResourceStream(TestResource);
            var bitmap2 = new Bitmap(stream);

            Assert.IsTrue(ImagesAreEqual(bitmap1, bitmap2));
        }
        // TODO: verify skew(45,45) throws
        [Test]
        public void TestSkew()
        {
            float approx_eps = .001041f;
            var skew = new SvgSkew(44, 46);
            skew.AngleX = 45 - approx_eps;
            Assert.That(() => { skew.AngleY = 45 + approx_eps; },
                Throws.TypeOf<System.ArgumentException>().With.Message.Contains("degenerates"));
            skew.AngleX = 0f;
            skew.AngleY = 45 - approx_eps;
            Assert.That(() => { skew.AngleX = 45 + approx_eps; },
                Throws.TypeOf<System.ArgumentException>().With.Message.Contains("degenerates"));
        }


        // TODO: the same above operations using raw matrix transforms directly
    }
}
