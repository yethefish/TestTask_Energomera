using Models;

namespace Interfaces;

public interface IFieldRepository
{
    IEnumerable<Field> GetAll();
    Field? GetById(string id);
}