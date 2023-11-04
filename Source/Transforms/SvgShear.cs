//
using System;

namespace Svg.Transforms
{
    /// <summary>
    /// The class which applies the specified shear vector to this Matrix.
    /// </summary>
    ///
    // TODO: figure out why this is here.  no svg spec that i can find mentions a shear transform and none of the browsers appear to support such a thing
    public sealed partial class SvgShear : SvgSpecificTransform
    {
        public float X { get; set; }

        public float Y { get; set; }

        public override string WriteToString()
        {
            return $"shear({X.ToSvgString()}, {Y.ToSvgString()})";
        }

        public SvgShear(float x)
            : this(x, x)
        {
        }

        public SvgShear(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override SvgMatrix SvgMatrix
        {
            get
            {
                var m = new SvgMatrix();
                m.b = (float)Math.Tan(Y);
                m.c = (float)Math.Tan(X);
                return m;

            }
            set
            {
                X = (float)Math.Atan(value.c);
                Y = (float)Math.Atan(value.b);
            }
        }

        public override object Clone()
        {
            return new SvgShear(X, Y);
        }


    }
}
