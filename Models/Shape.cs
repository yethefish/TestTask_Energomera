using System.Collections.ObjectModel;
namespace Models;
public abstract class Shape
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public abstract double GetArea();
    public abstract bool Contains(Point point);
}