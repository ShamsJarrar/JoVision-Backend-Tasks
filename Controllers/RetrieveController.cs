using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class RetrieveController : ControllerBase
{
    private readonly string _filesPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");

    public RetrieveController() {
        if(!Directory.Exists(_filesPath)){
            Directory.CreateDirectory(_filesPath);
        }
    }

    [HttpGet]
    public IActionResult retrieve([FromQuery] string image, [FromQuery] string owner)
    {
        if(image == null || image == "" ||
        owner == null || owner == null) 
            return BadRequest("Invalid Input!");
        
        string imagePath = Path.Combine(_filesPath, image);
        string metadataPath = imagePath + ".json";

        if(!System.IO.File.Exists(imagePath))
            return NotFound();
        
        try
        {
            var metadata = System.IO.File.ReadAllText(metadataPath);
            Metadata deserializedMetadata = JsonSerializer.Deserialize<Metadata>(metadata);
            
            if(deserializedMetadata.Owner != owner)
                return Forbid();
            
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            return File(imageBytes, "image/jpeg", image);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error: " + e.Message);
        }
    }
}