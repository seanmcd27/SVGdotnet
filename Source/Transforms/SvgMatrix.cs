using System;
using System.Collections.Generic;
using System.Numerics;

namespace Svg.Transforms
{
    // TODO: support compose/multiply.  e.g full set of operations on ISVGMatrix from SVG 1.1 spec

    /// <summary>
    /// The class which applies custom transform to this Matrix (Required for projects created by the Inkscape).
    /// </summary>
    public sealed partial class SvgMatrix : SvgTransform
    {
        private const int MAX_SIZE = 6;
        // svg matrix(a,b,c,d,e,f)
        // a c e
        // b d f
        // 0 0 1
        //
        // drawing2d matrix (a,b,c,d,e,f)
        // a b 0
        // c d 0
        // e f 1
        //
        // system.numerics matrix3x2 (a,b,c,d,e,f)
        // a(m11) b(m12) 0
        // c(m21) d(m22) 0
        // e(m31) f(m31) 1


        private Matrix3x2 _mat;
        // named accessors a-f per spec https://www.w3.org/TR/SVG11/coords.html#InterfaceSVGMatrix

        public float a { get => _mat.M11; set => _mat.M11 = value; }
        public float b { get => _mat.M12; set => _mat.M12 = value; }
        public float c { get => _mat.M21; set => _mat.M21 = value; }
        public float d { get => _mat.M22; set => _mat.M22 = value; }
        public float e { get => _mat.M31; set => _mat.M31 = value; }
        public float f { get => _mat.M32; set => _mat.M32 = value; }

        // these aren't entire points they're individual coordinates, name should match semantics
        [Obsolete("use coordinates property")]
        public List<float> Points { get { return Coordinates; } set { Coordinates = value; } }
        public List<float> Coordinates { get {
                var r = new List<float>();
                r.Add(_mat.M11); r.Add(_mat.M12); r.Add(_mat.M21); r.Add(_mat.M22); r.Add(_mat.M31); r.Add(_mat.M32);
                return r;
            } set {
                if (value.Count != MAX_SIZE) {
                    // should less than MAX_SIZE get filled with 0 instead of throwing?  probably not, usually that would just hide bugs
                    throw new ApplicationException($"Coordinate count of {value.Count} is not an svg supported 2x3 matrix of {MAX_SIZE} values");
                }
                _mat = new Matrix3x2(value[0], value[1], value[2], value[3], value[4], value[5]);
            }
        }

        public Matrix3x2 Matrix3x2 {get { return _mat;} set {_mat = value;}}


        public override string WriteToString()
        {
            return $"matrix({_mat.M11.ToSvgString()}, {_mat.M12.ToSvgString()}, {_mat.M21.ToSvgString()}, {_mat.M22.ToSvgString()}, {_mat.M31.ToSvgString()}, {_mat.M32.ToSvgString()})";
        }

        public SvgMatrix(List<float> m)
        {
            if (m.Count > MAX_SIZE)
            {
                throw new ApplicationException($"{m.Count} is greater than maximum svg supported matrix size of {MAX_SIZE}");
            }
            Coordinates = m;
        }
        public SvgMatrix(float a1, float b1, float c1, float d1, float e1, float f1)
        {
            a = a1;
            b = b1;
            c = c1;
            d = d1;
            e = e1;
            f = f1;
        }

        public SvgMatrix()
        {
            _mat = Matrix3x2.Identity;
        }
        public SvgMatrix(Matrix3x2 m)
        {
            _mat = m;
        }

        public override object Clone()
        {
            return new SvgMatrix(_mat);
        }
    }
}
