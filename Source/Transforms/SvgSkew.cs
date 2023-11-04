//

using System;
using System.Text;

namespace Svg.Transforms
{
    /// <summary>
    /// The class which applies the specified skew vector to this Matrix.
    /// </summary>
    public sealed partial class SvgSkew : SvgSpecificTransform
    {
        const float _eps = (1f / 960f); // TODO: validate assumption about epsilon. but, an order of magnitude smaller than 1/96 seems reasonable for graphics. also this should probably be in a base class of some sort
        float _x = 0f;
        float _y = 0f;
        // drawing 2D throws E_INVALIDARG with no explanation if both 45
        public float AngleX
        {
            get
            {
                return _x;
            }
            set
            {
                if ((Math.Abs(_y - 45f) < _eps) &&
                     (Math.Abs(value - 45f) < _eps)
                   )
                {
                    throw new ArgumentException("skewing both x and y by 45 degenerates to a line");
                }
                else
                {
                    _x = value;
                }
            }
        }

        public float AngleY
        {
            get
            {
                return _y;
            }
            set
            {
                if ((Math.Abs(_x - 45f) < _eps) &&
                     (Math.Abs(value - 45f) < _eps)
                   )
                {
                    throw new ArgumentException("skewing both x and y by 45 degenerates to a line");
                }
                else
                {
                    _y = value;
                }
            }
        }


        public override string WriteToString()
        {
            StringBuilder s = new StringBuilder("");
            if (AngleX != 0f) {
                s.Append($"skewX({AngleX.ToSvgString()})");
            }
            if (AngleY != 0f)
            {
                if (s.Length > 0)
                {
                    s.Append("");
                }
                s.Append($"skewY({AngleY.ToSvgString()})");
            }
            return s.ToString();
        }

        public SvgSkew(float x, float y)
        {
            AngleX = x;
            AngleY = y;
        }

        public override SvgMatrix SvgMatrix
        {
            get
            {
                var m = new SvgMatrix();
                m.b = (float)Math.Tan(AngleY);
                m.c = (float)Math.Tan(AngleX);
                return m;

            }
            set
            {
                AngleX = (float)Math.Atan(value.c);
                AngleY = (float)Math.Atan(value.b);
            }
        }

        public override object Clone()
        {
            return new SvgSkew(AngleX, AngleY);
        }
    }
}
