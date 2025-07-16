using Interfaces;
using Models;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class FieldsController : ControllerBase
{
    private readonly IFieldRepository _fieldRepository;

    public FieldsController(IFieldRepository fieldRepository)
    {
        _fieldRepository = fieldRepository;
    }

    [HttpGet]
    public IActionResult GetAllFields()
    {
        var fields = _fieldRepository.GetAll().Select(f => new
        {
            f.Id,
            f.Name,
            Size = f.GetArea(),
            Locations = new
            {
                Center = new[] { f.Center.Latitude, f.Center.Longitude },
                Polygon = f.Polygon.Select(p => new[] { p.Latitude, p.Longitude })
            }
        });
        return Ok(fields);
    }

    [HttpGet("{id}/size")]
    public IActionResult GetFieldSize(string id)
    {
        var field = _fieldRepository.GetById(id);
        if (field == null) return NotFound();
        return Ok(field.GetArea());
    }

    [HttpGet("{id}/distance")]
    public IActionResult GetDistanceToCenter(string id, [FromQuery] double lat, [FromQuery] double lon)
    {
        var field = _fieldRepository.GetById(id);
        if (field == null) return NotFound();
        
        var point = new Point(lat, lon);
        return Ok(field.GetDistanceTo(point));
    }

    [HttpGet("contains")]
    public IActionResult GetFieldContainingPoint([FromQuery] double lat, [FromQuery] double lon)
    {
        var point = new Point(lat, lon);
        var field = _fieldRepository.GetAll().FirstOrDefault(f => f.Contains(point));

        if (field != null)
        {
            return Ok(new { field.Id, field.Name });
        }
        return Ok(false);
    }
}