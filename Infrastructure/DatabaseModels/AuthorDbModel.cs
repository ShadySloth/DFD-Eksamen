using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Database_Benchmarking.Infrastructure.DatabaseModels;

public class AuthorDbModel
{
    [BsonId]
    public ObjectId UserId { get; set; }
    public string Name { get; set; }
}