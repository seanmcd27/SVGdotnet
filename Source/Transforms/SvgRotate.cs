//
using System;

namespace Svg.Transforms
{
    public sealed partial class SvgRotate : SvgSpecificTransform
    {

        // both svg and dot net define positive angle to be clockwise 
        public float Angle { get; set; }

        public float CenterX { get; set; }

        public float CenterY { get; set; }

        public override string WriteToString()
        {
            return $"rotate({Angle.ToSvgString()}, {CenterX.ToSvgString()}, {CenterY.ToSvgString()})";
        }

        public SvgRotate(float angle)
        {
            Angle = angle;
        }

        public SvgRotate(float angle, float centerX, float centerY)
            : this(angle)
        {
            CenterX = centerX;
            CenterY = centerY;
        }

        public override SvgMatrix SvgMatrix {
            get {
                var m = new SvgMatrix();
                m.a = (float)Math.Cos(Angle);
                m.b = (float)Math.Sin(Angle);
                m.c = (float)-Math.Sin(Angle);
                m.d = (float)Math.Cos(Angle);
                m.e = CenterX;
                m.f = CenterY;
                return m;

           } set {
                Angle = (float)Math.Acos(value.a);
                CenterX = value.e;
                CenterY = value.f;
            }
        }


        public override object Clone()
        {
            return new SvgRotate(Angle, CenterX, CenterY);
        }
    }
}
