using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_Benchmarking.Domain.Entities;

public class GenreOnArticle
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public EntityId ArticleId { get; set; }
    public Article Article { get; set; }

    public int GenreId { get; set; }
    public Genre Genre { get; set; }
}