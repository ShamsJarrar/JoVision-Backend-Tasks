using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CreateController : ControllerBase
{
    private readonly string _filesPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");

    public CreateController() 
    {
        if(!Directory.Exists(_filesPath)){
            Directory.CreateDirectory(_filesPath);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] IFormFile image, [FromForm] string owner) 
    {
        if(image == null || image.ContentType != "image/jpeg" ||
        owner == null || owner == "")
            return BadRequest("Invalid input!");

        string imagePath = Path.Combine(_filesPath, image.FileName);
        string metadataPath = imagePath + ".json";

        if(System.IO.File.Exists(imagePath))
            return BadRequest("Image already exists!");
        
        try 
        {
            using (var fs = new FileStream(imagePath, FileMode.Create)) {
                await image.CopyToAsync(fs);
            }

            Metadata metadata = new Metadata(owner);

            await System.IO.File.WriteAllTextAsync(metadataPath, JsonSerializer.Serialize(metadata));
            return Created($"/api/create/{image.FileName}", "File created succefully!");
        }
        catch (Exception e) 
        {
            return StatusCode(500, "Internal Server Error: " + e.Message);
        }
    }
}