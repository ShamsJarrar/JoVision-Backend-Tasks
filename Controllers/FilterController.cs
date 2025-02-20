using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

public enum FilterType
{
    ByModificationDate = 0,
    ByCreationDateDescending = 1,
    ByCreationDateAscending = 2,
    ByOwner = 3
}

[Route("api/[controller]")]
[ApiController]
public class FilterController : ControllerBase
{
    private readonly string _filesPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");

    public FilterController()
    {
        if(!Directory.Exists(_filesPath)) {
            Directory.CreateDirectory(_filesPath);
        }
    }

    [HttpPost]
    public IActionResult filter([FromForm] string icreationDate, [FromForm] string imodificationDate,
    [FromForm] string owner, [FromForm] string ifilterType) 
    {    
        DateTime creationDate, modificationDate;
        if (!DateTime.TryParse(icreationDate, out creationDate) ||
            !DateTime.TryParse(imodificationDate, out modificationDate))
        {
            return BadRequest("Invalid date format!");
        }

        if (!Enum.TryParse<FilterType>(ifilterType, true, out var filterType))
            return BadRequest("Invalid filter type!");

        var files = Directory.GetFiles(_filesPath, "*.json")
            .Select(file => JsonSerializer.Deserialize<Metadata>(System.IO.File.ReadAllText(file)))
            .ToList();

        List<byte[]> images = [];

        try
        {

            switch(filterType)
            {
                case FilterType.ByModificationDate:
                    images = files.Where(f => f.ModificationTime < modificationDate)
                                  .Select(f => System.IO.File.ReadAllBytes(Path.Combine(_filesPath, f.FileName)))
                                  .ToList();
                    break;
                
                case FilterType.ByCreationDateDescending:
                    images = files.Where(f => f.CreationTime > creationDate)
                                  .OrderByDescending(f => f.CreationTime)
                                  .Select(f => System.IO.File.ReadAllBytes(Path.Combine(_filesPath, f.FileName)))
                                  .ToList();
                    break;
                
                case FilterType.ByCreationDateAscending:
                    images = files.Where(f => f.CreationTime > creationDate)
                                  .OrderBy(f => f.CreationTime)
                                  .Select(f => System.IO.File.ReadAllBytes(Path.Combine(_filesPath, f.FileName)))
                                  .ToList();
                    break;
                
                case FilterType.ByOwner:
                    images = files.Where(f => f.Owner == owner)
                                  .Select(f => System.IO.File.ReadAllBytes(Path.Combine(_filesPath, f.FileName)))
                                  .ToList();
                    break;
                
                default:
                    return BadRequest("Invalid Input!");
            }

            return Ok(images);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error: " + e.Message);
        }

    }
}