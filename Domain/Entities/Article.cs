using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_Benchmarking.Domain.Entities;

public class Article
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public EntityId Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public int AuthorId { get; set; }

    [Required]
    public string BodyText { get; set; }

    public DateTime? Updated { get; set; }
    public DateTime? Deleted { get; set; }

    public Author Author { get; set; }

    public ICollection<Genre> Genres { get; set; }
}
