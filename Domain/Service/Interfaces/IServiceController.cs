namespace Database_Benchmarking.Domain.Service;

public interface IServiceController
{
    TimeSpan CreateArticles(int count);
    TimeSpan GetAllArticles();
    TimeSpan DeleteArticles(int count);
    TimeSpan UpdateArticles(int count);
}