using NUnit.Framework;

namespace Svg.UnitTests
{
    [TestFixture]
    public class SvgPointCollectionTests
    {
        [Test]
        public void ToStringReturnsValidString()
        {
            var collection = new SvgPointCollection
            {
                new SvgPoint(1.6f, 3.2f),
                new SvgPoint(1.2f, 5f)
            };
            Assert.AreEqual("1.6,3.2 1.2,5", collection.ToString());
        }

        [Test]
        public void CloneReturnsValidObjectType()
        {
            var collection = new SvgPointCollection();
            Assert.IsInstanceOf(typeof(SvgPointCollection), collection.Clone());
        }
    }
}
