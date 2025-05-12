using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_Benchmarking.Domain.Entities;

public class Genre
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public EntityId Id { get; set; }

    [Required]
    public string Type { get; set; }
}
