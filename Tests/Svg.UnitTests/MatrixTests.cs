//

using NUnit.Framework;
using Svg.Transforms;
using System;
using System.Collections.Generic;
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
            float angleR = angle * (float)Math.PI / 180;
            return new Matrix3x2((float)Math.Cos(angleR), (float)Math.Sin(angleR), -(float)Math.Sin(angleR), (float)Math.Cos(angleR), 0, 0);
        }

        public static Matrix3x2 GetTranslate(SvgPoint p)
        {
            var t = Matrix3x2.Identity;
            if (p.X.Type != SvgUnitType.User || p.Y.Type != SvgUnitType.User)
            {
                throw new ArgumentException("translation point must be expressed in user units");
            }
            t.M31 = p.X.Value;
            t.M32 = p.Y.Value;
            return t;
        }
        protected override string TestResource { get { return GetFullResourceString("matrix.matrix.bmp"); } }

        /// <summary>
        /// We should get a valid dpi (probably 72, 96 or similar).
        /// </summary>
        [Test]
        public void TestTransformOps()
        {
            int width = 800;
            int height = 600;
            var base_doc = EmptySvg(width, height);
            var doc = base_doc.Children[0] as SvgFragment;

            // polygon that is asymmetric in both dimensions
            var pts = new SvgPointCollection();
            pts.Add(new SvgPoint(30f, 10f));
            pts.Add(new SvgPoint(30f, 50f));
            pts.Add(new SvgPoint(40f, 70f));
            pts.Add(new SvgPoint(70f, 50f));
            pts.Add(new SvgPoint(70f, 10f));
            
            // no transform
            var pl = new SvgPolygon()
            {
                Points = pts,
            };
            pl.StrokeWidth = 1f;
            pl.Stroke = new SvgColourServer(Color.Red);
            pl.FillOpacity = 0;
            doc.Children.Add(pl);

            // translated
            var pl2 = pl.DeepCopy();
            pl2.Transforms = new SvgTransformCollection();
            pl2.Stroke = new SvgColourServer(Color.Blue);

            var xlate = new SvgTranslate(50f, 80f);
            pl2.Transforms.Add(xlate);
            doc.Children.Add(pl2);

            // translated and rotated
            var pl3 = pl2.DeepCopy();
            pl3.Transforms = new SvgTransformCollection();
            pl3.Stroke = new SvgColourServer(Color.Green);

            xlate = new SvgTranslate(50f, 80f);
            pl3.Transforms.Add(xlate);

            // both svg and dot net define positive angle to be clockwise 
            float angle = -45;

            var rot = new SvgRotate(angle);
            pl3.Transforms.Add(rot);
            doc.Children.Add(pl3);

            // translated and scaled
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

            // skewX alone
            var pl5 = pl.DeepCopy();
            pl5.Transforms = new SvgTransformCollection();
            pl5.Stroke = new SvgColourServer(Color.Purple);

            //xlate = new SvgTranslate(75f, 100f);
            //pl5.Transforms.Add(xlate);
            var skew = new SvgSkew(45, 0);
            pl5.Transforms.Add(skew);

            doc.Children.Add(pl5);

            // skewY alone
            var pl6 = pl.DeepCopy();
            pl6.Transforms = new SvgTransformCollection();
            pl6.Stroke = new SvgColourServer(Color.DarkGray);

            //xlate = new SvgTranslate(150f, 100f);
            //pl6.Transforms.Add(xlate);
            skew = new SvgSkew(0, 45);
            pl6.Transforms.Add(skew);

            doc.Children.Add(pl6);


            // skew X and Y
            var pl7 = pl.DeepCopy();
            pl7.Transforms = new SvgTransformCollection();
            pl7.Stroke = new SvgColourServer(Color.DarkGoldenrod);

            //xlate = new SvgTranslate(225f, 100f);
            //pl7.Transforms.Add(xlate);
            skew = new SvgSkew(20, 30);
            pl7.Transforms.Add(skew);

            doc.Children.Add(pl7);

            // TODO: figure out what to do with the 'shear' transform


            var bitmap1 = new Bitmap(width, height);
            base_doc.Draw(bitmap1);

            var stream = GetResourceStream(TestResource);
            var bitmap2 = new Bitmap(stream);

            Assert.IsTrue(ImagesAreEqual(bitmap1, bitmap2));

            // TODO: test all the transform ops with a raw matrix and make sure they match

            //**************************************************

            //fresh empty doc
            base_doc = EmptySvg(width, height);
            doc = base_doc.Children[0] as SvgFragment;

            // no transform
            var newpl = pl.DeepCopy();
            doc.Children.Add(newpl);


            // translated
            var newpl2 = newpl.DeepCopy();
            newpl2.Transforms = new SvgTransformCollection();
            newpl2.Stroke = new SvgColourServer(Color.Blue);

            var m_xlate = new SvgMatrix();
            m_xlate.e = 50f;
            m_xlate.f = 80f;
            newpl2.Transforms.Add(m_xlate);
            doc.Children.Add(newpl2);

            // translated and rotated
            var newpl3 = pl.DeepCopy();
            newpl3.Transforms = new SvgTransformCollection();
            newpl3.Stroke = new SvgColourServer(Color.Green);

            var m_xlate3 = m_xlate.Clone() as SvgMatrix;
            newpl3.Transforms.Add(m_xlate3);

            float angleR = angle * (float)Math.PI / 180;
            // test the other ctor
            var l = new List<float>();
            l.Add((float)Math.Cos(angleR));
            l.Add((float)Math.Sin(angleR));
            l.Add(-(float)Math.Sin(angleR));
            l.Add((float)Math.Cos(angleR));
            l.Add(0f);
            l.Add(0f);

            var m_rot = new SvgMatrix(l); // mix named accessor with float list ctor

            newpl3.Transforms.Add(m_rot);
            doc.Children.Add(newpl3);

            // translated and scaled
            var newpl4 = newpl.DeepCopy();
            newpl4.Transforms = new SvgTransformCollection();
            newpl4.Stroke = new SvgColourServer(Color.DeepPink);

            m_xlate = new SvgMatrix(1f, 0f, 0f, 1f, 400f, 300f);
            newpl4.Transforms.Add(m_xlate);
            var m_scale = new SvgMatrix(-2, 0, 0, -3, 0, 0);
            newpl4.Transforms.Add(m_scale);
            m_xlate = new SvgMatrix(1f, 0f, 0f, 1f, 100f, -80f);
            newpl4.Transforms.Add(m_xlate);

            doc.Children.Add(newpl4);

            // translated and skewX alone
            var newpl5 = newpl.DeepCopy();
            newpl5.Transforms = new SvgTransformCollection();
            newpl5.Stroke = new SvgColourServer(Color.Purple);

            //m_xlate = new SvgMatrix(1f, 0f, 0f, 1f, 75f, 100f);
            //newpl5.Transforms.Add(m_xlate);
            var m_skew = new SvgMatrix();
            m_skew.c = (float)Math.Tan(45 * (float)Math.PI / 180);
            newpl5.Transforms.Add(m_skew);

            doc.Children.Add(newpl5);

            // translated and skewY alone
            var newpl6 = newpl.DeepCopy();
            newpl6.Transforms = new SvgTransformCollection();
            newpl6.Stroke = new SvgColourServer(Color.DarkGray);

            //m_xlate = new SvgMatrix(1f, 0f, 0f, 1f, 150f, 100f);
            //newpl6.Transforms.Add(m_xlate);
            m_skew = new SvgMatrix();
            m_skew.b = (float)Math.Tan(45 * (float)Math.PI / 180);
            newpl6.Transforms.Add(m_skew);

            doc.Children.Add(newpl6);

            // translated and skew X and Y
            var newpl7 = newpl.DeepCopy();
            newpl7.Transforms = new SvgTransformCollection();
            newpl7.Stroke = new SvgColourServer(Color.DarkGoldenrod);

            //m_xlate = new SvgMatrix(1f, 0f, 0f, 1f, 150f, 100f);
            //newpl7.Transforms.Add(m_xlate);
            m_skew = new SvgMatrix();
            m_skew.b = (float)Math.Tan(30 * Math.PI / 180);
            m_skew.c = (float)Math.Tan(20 * Math.PI / 180);

            newpl7.Transforms.Add(m_skew);

            doc.Children.Add(newpl7);

            var bitmap3 = new Bitmap(width, height);
            base_doc.Draw(bitmap3);

            Assert.IsTrue(ImagesAreEqual(bitmap3, bitmap2));


        }

        //TODO: test each version of matrix3x2 get/set override

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

#if DISABLED
        [Test]
        public void TransformUnitConversion()
        {
            var u = new SvgUnit(SvgUnitType.Inch, 1.5f);
            var u2 = new SvgUnit(SvgUnitType.Centimeter, 1.5f);
            var t = new Svg.Transforms.SvgTranslate(u, u2);
            Console.WriteLine($"u {u.Value} u2 {u2.Value} t {t.X} {t.Y}");
        }
#endif

    }
}
