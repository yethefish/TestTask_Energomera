using Models;

namespace Interfaces;

public interface IKmlParser
{
    IEnumerable<Field> ParseFields();
}