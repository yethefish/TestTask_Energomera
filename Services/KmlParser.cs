using System.Xml.Linq;
using Interfaces;
using Models;

namespace Services;

public class KmlParser : IKmlParser
{
    public IEnumerable<Field> ParseFields()
    {
        XDocument fieldsDoc = XDocument.Load("source/fields.kml");
        XDocument centroidsDoc = XDocument.Load("source/centroids.kml");
        XNamespace kmlNs = "http://www.opengis.net/kml/2.2";

        var centroidPlacemarks = centroidsDoc.Descendants(kmlNs + "Placemark")
            .ToDictionary(
                p => p.Descendants(kmlNs + "SimpleData")
                      .FirstOrDefault(sd => sd.Attribute("name")?.Value == "fid")?.Value ?? "",
                p => p.Descendants(kmlNs + "coordinates").First().Value.Trim()
            );

        return fieldsDoc.Descendants(kmlNs + "Placemark").Select(placemark =>
        {
            var id = placemark.Descendants(kmlNs + "SimpleData")
                              .FirstOrDefault(sd => sd.Attribute("name")?.Value == "fid")?.Value ?? "";
            var field = new Field
            {
                Id = id,
                Name = placemark.Element(kmlNs + "name")?.Value ?? ""
            };

            var polygonCoordsStr = placemark.Descendants(kmlNs + "coordinates").First().Value.Trim();
            field.Polygon = polygonCoordsStr.Split(' ')
                .Select(p => p.Split(','))
                .Select(c => new Point(double.Parse(c[1]), double.Parse(c[0])))
                .ToList();

            if (centroidPlacemarks.TryGetValue(id, out var centroidCoordsStr))
            {
                var centerPoints = centroidCoordsStr.Split(',');
                field.Center = new Point(double.Parse(centerPoints[1]), double.Parse(centerPoints[0]));
            }
            
            return field;
        }).ToList();
    }
}