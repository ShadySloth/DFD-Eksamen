using Database_Benchmarking.Consoles;
using Database_Benchmarking.Consoles.SharedModels;
using Database_Benchmarking.Infrastructure.Generators;
using Database_Benchmarking.Infrastructure.MockObjectBuilders.Builders;

namespace Database_Benchmarking;


static class Program
{
    
    static void Main(string[] args)
    {
        BenchmarkConsole.Run();
    }
}