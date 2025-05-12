using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_Benchmarking.Domain.Entities;

public class Author
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public EntityId UserId { get; set; }

    [Required]
    public string Name { get; set; }
}
