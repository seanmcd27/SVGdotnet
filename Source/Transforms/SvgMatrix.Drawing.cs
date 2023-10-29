#if !NO_SDC
using System.Drawing.Drawing2D;

namespace Svg.Transforms
{
    public sealed partial class SvgMatrix : SvgTransform
    {
        public override Matrix Matrix
        {
            get
            {
                return new Matrix(
                    Coordinates[0],
                    Coordinates[1],
                    Coordinates[2],
                    Coordinates[3],
                    Coordinates[4],
                    Coordinates[5]
                );
            }
        }
    }
}
#endif
