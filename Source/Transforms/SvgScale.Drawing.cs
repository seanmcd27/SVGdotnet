#if !NO_SDC
using System.Drawing.Drawing2D;

namespace Svg.Transforms
{
    public sealed partial class SvgScale : SvgSpecificTransform
    {
        public override Matrix Matrix
        {
            get
            {
                var matrix = new Matrix();
                matrix.Scale(X, Y);
                return matrix;
            }
        }
    }
}
#endif
