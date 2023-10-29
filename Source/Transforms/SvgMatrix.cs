using System;
using System.Collections.Generic;
using System.Numerics;

namespace Svg.Transforms
{
    // TODO: support compose/multiply.  e.g full set of operations on ISVGMatrix from SVG 1.1 spec

    // TODO: need unittest to verify all the row/column major 3x2, 2x3 numerics 3x2 vs. drawing2d stuff is all correct w/ no typos or inadvertent transpositions

    /// <summary>
    /// The class which applies custom transform to this Matrix (Required for projects created by the Inkscape).
    /// </summary>
    public sealed partial class SvgMatrix : SvgTransform
    {
        private const int MAX_SIZE = 6;
        private Matrix3x2 _mat;
        // TODO: implment named accessors a-f per spec https://www.w3.org/TR/SVG11/coords.html#InterfaceSVGMatrix
        // these aren't points they're individual coordinates
        [Obsolete("use coordinates property")]
        public List<float> Points { get { return Coordinates; } set { Coordinates = value; } }
        public List<float> Coordinates { get {
                var r = new List<float>();
                r.Add(_mat.M11); r.Add(_mat.M12); r.Add(_mat.M22); r.Add(_mat.M31); r.Add(_mat.M22); r.Add(_mat.M32);
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
            return $"matrix({_mat.M11.ToSvgString()}, {_mat.M12.ToSvgString()}, {_mat.M21.ToSvgString()}, {_mat.M21.ToSvgString()}, {_mat.M31.ToSvgString()}, {_mat.M32.ToSvgString()})";
        }

        public SvgMatrix(List<float> m)
        {
            if (m.Count > MAX_SIZE)
            {
                throw new ApplicationException($"{m.Count} is greater than maximum svg supported matrix size of {MAX_SIZE}");
            }
            Coordinates = m;
        }

        public override object Clone()
        {
            return new SvgMatrix(Coordinates);
        }
    }
}
