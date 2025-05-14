namespace Database_Benchmarking.Domain.Service.Interfaces;

public interface IServiceController
{
    TimeSpan CreateArticles(int count);
    TimeSpan GetAllArticles(int count);
    TimeSpan GetByIdArticles(int count, int indexToGet);
    TimeSpan DeleteArticles(int count);
    TimeSpan UpdateArticles(int count);
    
    
    TimeSpan CreateAuthors(int count);
    TimeSpan GetAllAuthors(int count);
    TimeSpan GetByIdAuthors(int count, int indexToGet);
    TimeSpan DeleteAuthors(int count);
    TimeSpan UpdateAuthors(int count);
}