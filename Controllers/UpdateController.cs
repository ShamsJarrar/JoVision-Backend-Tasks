using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UpdateController : ControllerBase
{
    private readonly string _filesPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");

    public UpdateController() {
        if(!Directory.Exists(_filesPath)){
            Directory.CreateDirectory(_filesPath);
        }
    }

    [HttpPost]
    public async Task<IActionResult> update([FromForm] IFormFile image, [FromForm] string owner)
    {
        if(image == null || image.ContentType != "image/jpeg" ||
        owner == null || owner == "")
            return BadRequest("Invalid input!");
        
        string imagePath = Path.Combine(_filesPath, image.FileName);
        string metadataPath = imagePath + ".json";

        if(!System.IO.File.Exists(imagePath))
            return BadRequest("The image does not exist");

        try
        {
            var metadata = System.IO.File.ReadAllText(metadataPath);
            Metadata deserializedMetadata = JsonSerializer.Deserialize<Metadata>(metadata);
            
            if(deserializedMetadata.Owner != owner)
                return Forbid();
            
            using (var fs = new FileStream(imagePath, FileMode.Create)) {
                await image.CopyToAsync(fs);
            }

            deserializedMetadata.ModificationTime = DateTime.UtcNow;
            await System.IO.File.WriteAllTextAsync(metadataPath, JsonSerializer.Serialize(deserializedMetadata));
            
            return Ok("File Updated Successfully!");
        }
        catch(Exception e)
        {
            return StatusCode(500, "Internal Server Error: " + e.Message);
        }
    }
}