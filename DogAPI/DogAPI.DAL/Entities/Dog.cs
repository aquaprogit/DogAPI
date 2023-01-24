using System.ComponentModel.DataAnnotations;

namespace DogAPI.DAL.Entities;
public class Dog
{
    [Key]
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int TailLength { get; set; }
    public int Weight { get; set; }
}
