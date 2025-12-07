using myPoint;
using System.Collections.Generic;
using System;
using netDxf.Tables;
using netDxf;
using System.Data;
using netDxf.Entities;

namespace myGenerator
{
    public class GeneratorLine
    {
        public Polyline2D? poly = null;
        private MyPoint? from = null;
        private MyPoint? transformed_from = null;
        private MyPoint? transformed_to = null;
        private MyPoint? offset = null;
        private double rotate = 0.0;
        private double material_thickness = 0.0;
        private bool notFirstLine = false;
        private Boolean dog_bone_mode = false;
        private double milling_tool_diameter = 0.0;
        private Layer? layerBlack = null;
        private DxfDocument? doc = null;

        public GeneratorLine(MyPoint? start, MyPoint? offset, double rotate, double material_thickness, Boolean dog_bone_mode, double milling_tool_diameter, DxfDocument? doc, Layer? layerBlack)
        {
            this.from = start;
            this.rotate = rotate;
            this.offset = offset;
            this.dog_bone_mode = dog_bone_mode;
            this.milling_tool_diameter = milling_tool_diameter;
            this.material_thickness = material_thickness;
            this.doc = doc;
            this.layerBlack = layerBlack;

            transformed_from = MyPoint.Move(MyPoint.Rotate(this.from, this.rotate), this.offset);
            this.poly = new Polyline2D();
            poly.IsClosed = true;
        }

        public void Add(MyPoint to)
        {
            this.transformed_from = MyPoint.Move(MyPoint.Rotate(this.from, rotate), this.offset);
            this.transformed_to = MyPoint.Move(MyPoint.Rotate(to, rotate), this.offset);
            DrawSpecialLine(poly, this.transformed_from, this.transformed_to, this.material_thickness, notFirstLine, this.milling_tool_diameter);
            notFirstLine = true;
            from = new MyPoint(to);
        }
        public void AddWithFillet(MyPoint to, double radius)
        {
            this.transformed_from = MyPoint.Move(MyPoint.Rotate(this.from, rotate), this.offset);
            this.transformed_to = MyPoint.Move(MyPoint.Rotate(to, rotate), this.offset);
            DrawSpecialLineFillet(poly, this.transformed_from, this.transformed_to, this.material_thickness, notFirstLine, this.milling_tool_diameter, radius);
            notFirstLine = true;
            from = new MyPoint(to);
        }
        public void AddCircle(MyPoint to)
        {
            this.transformed_from = MyPoint.Move(MyPoint.Rotate(this.from, rotate), this.offset);
            this.transformed_to = MyPoint.Move(MyPoint.Rotate(to, rotate), this.offset);
            poly.Vertexes.AddRange(DrawSpecialCircle(this.transformed_from, this.transformed_to, this.material_thickness, notFirstLine, this.milling_tool_diameter));
            notFirstLine = true;
            from = new MyPoint(to);
        }
        public void Finished()
        {
            poly.Color = AciColor.Cyan;
            poly.Layer = layerBlack;
            doc.Entities.Add(poly);
            this.poly = new Polyline2D();
        }
        public List<Polyline2DVertex> DrawSpecialCircle(MyPoint from, MyPoint to, double material_thickness, bool continued_line, double milling_tool_diameter)
        {
            List<Polyline2DVertex> vr = new List<Polyline2DVertex>();
            MyPoint delta = MyPoint.GetDelta(from, to);
            if ((delta.x == 0) && (delta.y == 0))
            {
                // do nothing
            }
            else
            {
                MyPoint center = new MyPoint((from.x + to.x) / 2.0, (from.y + to.y) / 2.0 );
                double a1 = Math.Atan2(from.y - center.y, from.x - center.x);
                double a2 = Math.Atan2(to.y - center.y, to.x - center.x);

                double arcAngle = a2 - a1;

                // normalize to -PI..PI
                if (arcAngle > Math.PI) arcAngle -= 2 * Math.PI;
                if (arcAngle < -Math.PI) arcAngle += 2 * Math.PI;

                double bulge = Math.Tan(arcAngle / 4);

                if (continued_line == false)
                {
                    vr.Add(new Polyline2DVertex(new Vector2(from.x, from.y), -bulge));
                    vr.Add(new Polyline2DVertex(new Vector2(to.x, to.y)));
                } else
                {
                    vr.Add(new Polyline2DVertex(new Vector2(from.x, from.y), bulge));
                    vr.Add(new Polyline2DVertex(new Vector2(to.x, to.y)));
                }
            }
            return vr;
        }
        public void DrawSpecialLine(Polyline2D ?poly, MyPoint from, MyPoint to, double material_thickness, bool continued_line, double milling_tool_diameter)
        {
            MyPoint delta = MyPoint.GetDelta(from, to);
            if ((delta.x == 0) && (delta.y == 0))
            {
                // do nothing
            }
            else
            {
                if (continued_line == false)
                {
                    poly.Vertexes.Add(new Polyline2DVertex(new Vector2(from.x, from.y)));
                }
                poly.Vertexes.Add(new Polyline2DVertex(new Vector2(to.x, to.y)));
            }
        }

        public void DrawSpecialLineFillet(Polyline2D? poly, MyPoint from, MyPoint to, double material_thickness, bool continued_line, double milling_tool_diameter, double radius)
        {
            MyPoint delta = MyPoint.GetDelta(from, to);
            if ((delta.x == 0) && (delta.y == 0))
            {
                // do nothing
            }
            else
            {
                if (continued_line == false)
                {
                    Polyline2DVertex pl = new Polyline2DVertex(new Vector2(from.x, from.y));
                    poly.Vertexes.Add(pl);
                }
                Polyline2DVertex pl2 = new Polyline2DVertex(new Vector2(to.x, to.y));
                poly.Vertexes.Add(pl2);
            }
        }

    }
    public static class FilletHelper
    {
        public static (Vector2 T1, Vector2 T2, Vector2 Center) Fillet(
            Vector2 P1, Vector2 P2, Vector2 P3, double R)
        {
            Vector2 v1 = Vector2.Normalize(P1 - P2);
            Vector2 v2 = Vector2.Normalize(P3 - P2);

            double dot = Vector2.DotProduct(v1, v2);
            double angle = Math.Acos(dot);

            double d = R / Math.Tan(angle / 2.0);

            Vector2 T1 = P2 + v1 * (float)d;
            Vector2 T2 = P2 + v2 * (float)d;

            Vector2 n = Vector2.Normalize(v1 + v2);
            double h = R / Math.Sin(angle / 2.0);

            Vector2 C = P2 + n * (float)h;

            return (T1, T2, C);
        }
        public static void AddFilletToPolyline2D(Polyline2D poly, int cornerIndex, double R)
        {
            int count = poly.Vertexes.Count;

            int i1 = (cornerIndex - 1 + count) % count;
            int i2 = cornerIndex;
            int i3 = (cornerIndex + 1) % count;

            var vPrev = poly.Vertexes[i1];
            var vCorner = poly.Vertexes[i2];
            var vNext = poly.Vertexes[i3];

            Vector2 P1 = vPrev.Position;
            Vector2 P2 = vCorner.Position;
            Vector2 P3 = vNext.Position;

            // Calculate fillet geometry
            var (T1, T2, C) = FilletHelper.Fillet(P1, P2, P3, R);

            // Remove old corner vertex
            poly.Vertexes.RemoveAt(i2);


            // Now compute bulge for the arc between T1 and T2
            double angle1 = Math.Atan2(T1.Y - C.Y, T1.X - C.X);
            double angle2 = Math.Atan2(T2.Y - C.Y, T2.X - C.X);

            double included = angle2 - angle1;

            // Normalize to (-PI, PI)
            while (included > Math.PI) included -= 2 * Math.PI;
            while (included < -Math.PI) included += 2 * Math.PI;

            double bulge = Math.Tan(included / 4);

            poly.Vertexes.Insert(i2,     new Polyline2DVertex(T1, bulge));
            poly.Vertexes.Insert(i2 + 1, new Polyline2DVertex(T2));
        }
    }
}
