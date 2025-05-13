namespace Database_Benchmarking.Domain.Service.Interfaces;

public interface IServiceController
{
    TimeSpan Create(int count);
    TimeSpan GetAll();
    TimeSpan Delete(int count);
    TimeSpan Update(int count);
}