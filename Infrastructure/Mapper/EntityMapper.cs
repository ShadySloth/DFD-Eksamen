using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.DatabaseModels;
using MongoDB.Bson;

namespace Database_Benchmarking.Infrastructure.Mapper;

public static class EntityMapper
{
    public static ArticleDbModel ToDbModel(Article article)
    {
        return new ArticleDbModel
        {
            Id = new ObjectId(article.Id.Value),
            Title = article.Title,
            AuthorId = new ObjectId(article.AuthorId.Value),
            BodyText = article.BodyText,
            Updated = article.Updated,
            Deleted = article.Deleted,
            GenreIds = article.Genres.Select(genre => new ObjectId(genre.Id.Value)).ToList()
        };
    }
    
    public static Article ToDomainEntity(ArticleDbModel articleDbModel)
    {
        return new Article
        {
            Id = new EntityId(articleDbModel.Id.ToString()),
            Title = articleDbModel.Title,
            AuthorId = new EntityId(articleDbModel.AuthorId.ToString()),
            BodyText = articleDbModel.BodyText,
            Updated = articleDbModel.Updated,
            Deleted = articleDbModel.Deleted
        };
    }
    
    public static GenreDbModel ToDbModel(Genre genre)
    {
        return new GenreDbModel
        {
            Id = new ObjectId(genre.Id.Value),
            Type = genre.Type,
            ArticleIds = genre.Articles.Select(article => new ObjectId(article.Id.Value)).ToList()
        };
    }
    
    public static Genre ToDomainEntity(GenreDbModel genreDbModel)
    {
        return new Genre
        {
            Id = new EntityId(genreDbModel.Id.ToString()),
            Type = genreDbModel.Type
        };
    }
    
    public static AuthorDbModel ToDbModel(Author author)
    {
        return new AuthorDbModel
        {
            UserId = new ObjectId(author.UserId.Value),
            Name = author.Name,
        };
    }
    
    public static Author ToDomainEntity(AuthorDbModel authorDbModel)
    {
        return new Author
        {
            UserId = new EntityId(authorDbModel.UserId.ToString()),
            Name = authorDbModel.Name
        };
    }
}