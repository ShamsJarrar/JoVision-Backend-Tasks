using System;
public class Metadata
{
    public string Owner {get; set;}
    public DateTime CreationTime {get; set;}
    public DateTime ModificationTime {get; set;}

    public Metadata(string owner)
    {
        this.Owner = owner;
        this.CreationTime = DateTime.UtcNow;
        this.ModificationTime = DateTime.UtcNow;
    }
}