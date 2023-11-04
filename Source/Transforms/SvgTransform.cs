using System;

namespace Svg.Transforms
{

    public sealed partial class SvgMatrix { };


    public abstract partial class SvgTransform : ICloneable
    {
        public abstract string WriteToString();

        public abstract object Clone();

        public override string ToString()
        {
            return WriteToString();
        }
    }
    public abstract partial class SvgSpecificTransform : SvgTransform
    {
        public abstract SvgMatrix SvgMatrix { get; set; }

    }
}
