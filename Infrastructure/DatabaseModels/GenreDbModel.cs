using System.ComponentModel.DataAnnotations;
using Database_Benchmarking.Domain.Entities;
using MongoDB.Bson;

namespace Database_Benchmarking.Infrastructure.DatabaseModels;

public class GenreDbModel
{
    public ObjectId Id { get; set; }
    public string Type { get; set; }
    public ICollection<ObjectId> ArticleIds { get; set; }
}