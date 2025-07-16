namespace Models;
public class Field : Shape
{
    public Point Center { get; set; }
    public List<Point> Polygon { get; set; } = new List<Point>();

    public override double GetArea()
    {
        double area = 0;
        int j = Polygon.Count - 1;
        for (int i = 0; i < Polygon.Count; i++)
        {
            area += (Polygon[j].Longitude + Polygon[i].Longitude) * (Polygon[j].Latitude - Polygon[i].Latitude);
            j = i;
        }
        return Math.Abs(area / 2) * 111.32 * 111.32 * Math.Cos(Polygon[0].Latitude * Math.PI / 180) * 100;
    }

    public double GetDistanceTo(Point point)
    {
        const double R = 6371e3;
        var phi1 = Center.Latitude * Math.PI / 180;
        var phi2 = point.Latitude * Math.PI / 180;
        var deltaPhi = (point.Latitude - Center.Latitude) * Math.PI / 180;
        var deltaLambda = (point.Longitude - Center.Longitude) * Math.PI / 180;

        var a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                Math.Cos(phi1) * Math.Cos(phi2) *
                Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return R * c;
    }

    public override bool Contains(Point point)
    {
        bool isInside = false;
        int n = Polygon.Count;
        for (int i = 0, j = n - 1; i < n; j = i++)
        {
            var p1 = Polygon[i];
            var p2 = Polygon[j];

            bool intersect = ((p1.Longitude > point.Longitude) != (p2.Longitude > point.Longitude))
                && (point.Latitude < (p2.Latitude - p1.Latitude) * (point.Longitude - p1.Longitude) / (p2.Longitude - p1.Longitude) + p1.Latitude);
            if (intersect)
            {
                isInside = !isInside;
            }
        }
        return isInside;
    }
}