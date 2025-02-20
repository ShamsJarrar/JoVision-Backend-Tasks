using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class TransferController : ControllerBase
{
    private readonly string _filesPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");

    public TransferController()
    {
        if(!Directory.Exists(_filesPath)) {
            Directory.CreateDirectory(_filesPath);
        }
    }

    [HttpGet]
    public IActionResult transfer([FromQuery] string OldOwner, [FromQuery] string NewOwner)
    {
        if (OldOwner == "" || NewOwner == "")
            return BadRequest("Invalid Input!");

        try
        {
            var files = Directory.GetFiles(_filesPath, "*.json")
                .Where(file => {
                    var metadata = JsonSerializer.Deserialize<Metadata>(System.IO.File.ReadAllText(file));
                    return metadata.Owner == OldOwner;
                }).ToList();

            foreach(var path in files)
            {
                Metadata metadata = JsonSerializer.Deserialize<Metadata>(System.IO.File.ReadAllText(path));
                metadata.Owner = NewOwner;
                System.IO.File.WriteAllText(path, JsonSerializer.Serialize(metadata));
            }

            var metadaFiles = Directory.GetFiles(_filesPath, "*.json")
                        .Select(file => JsonSerializer.Deserialize<Metadata>(System.IO.File.ReadAllText(file)))
                        .ToList();

            List<byte[]> images = metadaFiles.Where(f => f.Owner == NewOwner)
                                  .Select(f => System.IO.File.ReadAllBytes(Path.Combine(_filesPath, f.FileName)))
                                  .ToList();

            return Ok(images);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error: " + e.Message);
        }


    }
}