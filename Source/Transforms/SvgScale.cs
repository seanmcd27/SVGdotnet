namespace Svg.Transforms
{
    public sealed partial class SvgScale : SvgSpecificTransform
    {
        public float X { get; set; }

        public float Y { get; set; }

        public override string WriteToString()
        {
            if (X == Y)
                return $"scale({X.ToSvgString()})";
            return $"scale({X.ToSvgString()}, {Y.ToSvgString()})";
        }

        public SvgScale(float x)
            : this(x, x)
        {
        }

        public SvgScale(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override SvgMatrix SvgMatrix
        {
            get
            {
                var m = new SvgMatrix();
                m.a = X;
                m.d = Y;
                return m;

            }
            set
            {
                X = value.a;
                Y = value.d;
            }
        }


        public override object Clone()
        {
            return new SvgScale(X, Y);
        }
    }
}
