namespace myPoint
{
    public class MyPoint
    {
        public double x = 0;
        public double y = 0;

        public MyPoint()
        {

        }
        public MyPoint(MyPoint p)
        {
            this.x = p.x;
            this.y = p.y;
        }
        public MyPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static MyPoint? GetDelta(MyPoint? from, MyPoint? to)
        {
            MyPoint delta = new MyPoint(0,0);
            if ((from != null) && (to != null))
            {
                delta = new MyPoint(to);
                delta.x -= from.x;
                delta.y -= from.y;
            }
            return delta;
        }

        public static MyPoint Invert(MyPoint p_in)
        {
            MyPoint inverted_point = new MyPoint(p_in);
            inverted_point.x *= -1.0;
            inverted_point.y *= -1.0;
            return inverted_point;
        }

        public static MyPoint Move(MyPoint p_in, MyPoint origin)
        {
            MyPoint moved_point = new MyPoint(p_in);
            moved_point.x += origin.x;
            moved_point.y += origin.y;
            return moved_point;
        }

        public static MyPoint Rotate(MyPoint p_in, double rotate)
        {
            return Rotate(p_in, rotate, new MyPoint(0.0, 0.0));
        }

        public static MyPoint Rotate(MyPoint p_in, double rotate, MyPoint p_origin)
        {
            MyPoint rotated_point = new MyPoint(0.0, 0.0);
            MyPoint temp = new MyPoint(p_in);

            temp = Move(temp, Invert(p_origin));

            rotated_point.x = (temp.x * Math.Cos(rotate)) - (temp.y * Math.Sin(rotate));
            rotated_point.y = (temp.y * Math.Cos(rotate)) + (temp.x * Math.Sin(rotate));

            rotated_point = Move(rotated_point, p_origin);

            return rotated_point;
        }

        public static double GetLength(MyPoint delta)
        {
            double length = Math.Sqrt((delta.x * delta.x) + (delta.y * delta.y));
            return length;
        }

        public static double GetRotation(MyPoint delta)
        {
            double rotate = 0.0;
            if ((delta.x != 0.0) && (delta.y != 0.0))
            {
                rotate = Math.Atan(Math.Abs(delta.y / delta.x));
                if (delta.y > 0)
                {
                    if (delta.x > 0)
                    {
                        rotate *= 1.0;
                    }
                    else
                    {
                        rotate = (Math.PI) - rotate;
                    }
                }
                else
                {
                    if (delta.x > 0)
                    {
                        rotate = 4.0 * (Math.PI / 2.0) - rotate;
                    }
                    else
                    {
                        rotate = ((Math.PI) + rotate);
                    }
                }
            }
            else if ((delta.x == 0.0) && (delta.y == 0.0))
            {
                rotate = 0.0;
            }
            else
            {
                if (delta.x == 0.0)
                {
                    if (delta.y > 0)
                    {
                        rotate = 1.0 * (Math.PI / 2.0);
                    }
                    else if (delta.y < 0)
                    {
                        rotate = 3.0 * (Math.PI / 2.0);
                    }
                }
                if (delta.y == 0.0)
                {
                    if (delta.x > 0)
                    {
                        rotate = 0.0 * (Math.PI / 2.0);
                    }
                    else if (delta.x < 0)
                    {
                        rotate = 2.0 * (Math.PI / 2.0);
                    }
                }
            }
            return rotate;
        }

    }

}
