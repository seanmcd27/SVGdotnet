#if !NO_SDC
using System.Drawing.Drawing2D;
using System.Numerics;

namespace Svg.Transforms
{
    public static class MatrixExtensions
    {
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
        // a(m11) b(m12)
        // c(m21) d(m22)
        // e(m31) f(m31)

        public static Matrix3x2 ToMatrix3x2(this Matrix m)
        {
            return new Matrix3x2(m.Elements[0], m.Elements[1], m.Elements[2], m.Elements[3], m.Elements[4], m.Elements[5]);
        }
        public static Matrix ToMatrix(this Matrix3x2 m)
        {
            return new Matrix(m.M11, m.M12, m.M21, m.M22, m.M31, m.M32);
        }
    };
    public abstract partial class SvgTransform
    {
        public abstract Matrix Matrix { get; }

        #region Equals implementation

        public override bool Equals(object obj)
        {
            var other = obj as SvgTransform;
            if (other == null)
                return false;

            return Matrix.Equals(other.Matrix);
        }

        public override int GetHashCode()
        {
            return Matrix.GetHashCode();
        }

        public static bool operator ==(SvgTransform lhs, SvgTransform rhs)
        {
            if (ReferenceEquals(lhs, rhs))
                return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
                return false;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(SvgTransform lhs, SvgTransform rhs)
        {
            return !(lhs == rhs);
        }

        #endregion
    }
}
#endif
