using System;
using System.Collections.Generic;
using Database_Benchmarking.Consoles;
using Database_Benchmarking.Consoles.SharedModels;
using Database_Benchmarking.Infrastructure.Generators;

namespace Database_Benchmarking
{
    static class Program
    {
        static void Main(string[] args)
        {
            // Eksempel på at oprette en liste af ResultSet objekter
            List<ResultSet> resultSets = new List<ResultSet>
            {
                new ResultSet { BatchSize = 1000, EFCorePG = TimeSpan.FromSeconds(1.5), NpgSql = TimeSpan.FromSeconds(1.3), MongoDb = TimeSpan.FromSeconds(1.6) },
                new ResultSet { BatchSize = 2000, EFCorePG = TimeSpan.FromSeconds(2.0), NpgSql = TimeSpan.FromSeconds(1.8), MongoDb = TimeSpan.FromSeconds(2.1) },
                new ResultSet { BatchSize = 3000, EFCorePG = TimeSpan.FromSeconds(2.5), NpgSql = TimeSpan.FromSeconds(2.3), MongoDb = TimeSpan.FromSeconds(2.6) }
            };

            // Angiv stien til output PNG-fil
            string outputPath = "C:\\path\\to\\output\\diagram.png";

            // Kald DiagramGenerator's GenerateDiagram metode
            DiagramGenerator.GenerateDiagram(resultSets, outputPath);

            Console.WriteLine("Diagram genereret og gemt som: " + outputPath);

            // Kald BenchmarkConsole Run-metoden
            //BenchmarkConsole.Run();
        }
    }
}