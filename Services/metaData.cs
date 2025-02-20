using System;
public class Metadata
{
    public string Owner {get; set;}
    public DateTime CreationTime {get; set;}
    public DateTime ModificationTime {get; set;}
    public string FileName {get; set;}

    public Metadata(string owner, string fileName)
    {
        this.Owner = owner;
        this.CreationTime = DateTime.UtcNow;
        this.ModificationTime = DateTime.UtcNow;
        this.FileName = fileName;
    }
}