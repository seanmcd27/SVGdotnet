using NUnit.Framework;
using System;
using System.Drawing;
using System.IO;
using System.Xml;

namespace Svg.UnitTests
{
    [TestFixture]
    public class SvgTextTests : SvgTestHelper
    {
        [Test]
        public void TextPropertyAffectsSvgOutput()
        {
            var document = new SvgDocument();
            document.Children.Add(new SvgText { Text = "test1" });
            using (var stream = new MemoryStream())
            {
                document.Write(stream);
                stream.Position = 0;

                var xmlDoc = new XmlDocument();
                xmlDoc.XmlResolver = new SvgDtdResolver();
                xmlDoc.Load(stream);
                Assert.AreEqual("test1", xmlDoc.DocumentElement.FirstChild.InnerText);
            }
        }

        /// <summary>
        /// Test related to bug #473.
        /// We check if changing a coordinate invalidate the cache.
        /// We doing this indirectly by checking the Bound property, which uses the cache.
        /// The Bound coordinates must be updated after adding a X and a Y
        /// </summary>
        [Test]
        public void ChangingCoordinatesInvalidatePathCache()
        {
            SvgText text = new SvgText();
            text.Text = "Test invalidate cache";
            float origX = text.Bounds.X;
            float origY = text.Bounds.Y;
            text.X.Add(100);
            text.Y.Add(100);

            Assert.AreNotEqual(origX, text.Bounds.X);
            Assert.AreNotEqual(origY, text.Bounds.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestWritesCoordinatesForCollectionSet()
        {
            SvgText text = new SvgText();
            text.Text = "Test coordinates";
            text.X = new SvgUnitCollection { 20 };
            text.Y = new SvgUnitCollection { 30 };
            text.Dx = new SvgUnitCollection { 40 };
            text.Dy = new SvgUnitCollection { 50 };

            var xml = text.GetXML();
            Assert.IsTrue(xml.Contains("x=\"20\""));
            Assert.IsTrue(xml.Contains("y=\"30\""));
            Assert.IsTrue(xml.Contains("dx=\"40\""));
            Assert.IsTrue(xml.Contains("dy=\"50\""));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestWritesCoordinatesForCollectionChange()
        {
            SvgText text = new SvgText();
            text.Text = "Test coordinates";
            text.X.Add(20);
            text.Y.Add(30);
            text.Dx.Add(40);
            text.Dy.Add(50);

            var xml = text.GetXML();
            Assert.IsTrue(xml.Contains("x=\"20\""));
            Assert.IsTrue(xml.Contains("y=\"30\""));
            Assert.IsTrue(xml.Contains("dx=\"40\""));
            Assert.IsTrue(xml.Contains("dy=\"50\""));
        }

        [Test]
        public void TestUpdateCoordinatesForCollectionChange()
        {
            SvgText text = new SvgText()
            {
                Text = "Test coordinates",
                X = { 10 },
                Y = { 10 },
                Dx = { 10 },
                Dy = { 10 },
            };

            text.X = new SvgUnitCollection() { 20 };
            text.Y = new SvgUnitCollection() { 30 };
            text.Dx = new SvgUnitCollection() { 40 };
            text.Dy = new SvgUnitCollection() { 50 };

            var xml = text.GetXML();
            Assert.IsTrue(xml.Contains("x=\"20\""));
            Assert.IsTrue(xml.Contains("y=\"30\""));
            Assert.IsTrue(xml.Contains("dx=\"40\""));
            Assert.IsTrue(xml.Contains("dy=\"50\""));
        }
        private bool TestConvertTextPathToSvgPathHelper(string fontFamily)
        {
            string testStr = "ABCDEFGhijklmnop123.,";
            SvgText text = new SvgText()
            {
                Text = testStr,
            };
            text.FontFamily = fontFamily;
            int fntSize = 32; // 24pt @ 96dpi
            text.FontSize = new SvgUnit(SvgUnitType.Pixel, fntSize);
            int width = (int)(text.Bounds.Width + 1);
            int height = (int)(text.Bounds.Height + 1);
            //var tform = text.Transforms ?? new Transforms.SvgTransformCollection();
            //tform.Add(new Svg.Transforms.SvgTranslate(0, height));  // text renders assuming positive Y is up
            //text.Transforms = tform;
            var y = text.Y ?? new SvgUnitCollection();
            y.Add(new SvgUnit(SvgUnitType.Pixel, height)); // text renders assuming positive Y is up
            text.Y = y;

            var svgDoc = new SvgDocument();
            svgDoc.Children.Add(text);

            var bitmap1 = new Bitmap(width, height);
            svgDoc.Draw(bitmap1);
            svgDoc = new SvgDocument();
            var path = text.ToSvgPath();
            svgDoc.Children.Add(path);
            var bitmap2 = new Bitmap(width, height);
            svgDoc.Draw(bitmap2);
            float pct = 0.0f;
            Bitmap imgDiff;
            return ImagesAreEqual(bitmap1, bitmap2, out pct, out imgDiff);
        }
        [Test]
        public void TestConvertTextPathToSvgPath()
        {
            // a couple of random fairly complex fonts that are built into windows
            Assert.IsTrue(TestConvertTextPathToSvgPathHelper("Algerian"));  
            Assert.IsTrue(TestConvertTextPathToSvgPathHelper("WingDings")); 
        }
    }
}
