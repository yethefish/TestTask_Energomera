using Models;
using Interfaces;

namespace Data;

public class FieldRepository : IFieldRepository
{
    private readonly List<Field> _fields;

    public FieldRepository(IKmlParser kmlParser)
    {
        _fields = kmlParser.ParseFields().ToList();
    }

    public IEnumerable<Field> GetAll() => _fields;

    public Field? GetById(string id) => _fields.FirstOrDefault(f => f.Id == id);
}