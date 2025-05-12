using Database_Benchmarking.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Database_Benchmarking.Infrastructure.DatabaseModels;

public class ArticleDbModel
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Title { get; set; }
    public ObjectId AuthorId { get; set; }
    public string BodyText { get; set; }

    public DateTime? Updated { get; set; }
    public DateTime? Deleted { get; set; }

    public ICollection<ObjectId> GenreIds { get; set; }
}