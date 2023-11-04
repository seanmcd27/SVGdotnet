namespace Svg.Transforms
{
    public sealed partial class SvgTranslate : SvgSpecificTransform
    {
        public float X { get; set; }

        public float Y { get; set; }

        public override string WriteToString()
        {
            return $"translate({X.ToSvgString()}, {Y.ToSvgString()})";
        }

        public SvgTranslate(float x, float y)
        {
            X = x;
            Y = y;
        }

        public SvgTranslate(float x)
            : this(x, 0f)
        {
        }
        public override SvgMatrix SvgMatrix
        {
            get
            {
                var m = new SvgMatrix();
                m.e = X;
                m.f = Y;
                return m;

            }
            set
            {
                X = value.e;
                Y = value.f;
            }
        }



        public override object Clone()
        {
            return new SvgTranslate(X, Y);
        }
    }
}
