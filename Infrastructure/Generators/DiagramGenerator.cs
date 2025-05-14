using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Database_Benchmarking.Consoles.SharedModels;
using ScottPlot;
using System.Drawing;

namespace Database_Benchmarking.Infrastructure.Generators
{
    public class DiagramGenerator
    {
        public static void GenerateDiagram(List<ResultSet> resultSets, string outputPath)
        {
            var plt = new ScottPlot.Plot(800, 600);

            // Sørg for at output-mappen findes
            var directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            // Filtrer batch groups for de normale målinger og gennemsnit
            var batchGroups = resultSets
                .GroupBy(r => r.BatchSize)
                .OrderBy(g => g.Key)
                .ToList();

            // Hvis batchGroups er tom, returner tidligt
            if (!batchGroups.Any())
            {
                Console.WriteLine("Ingen batch grupper til at generere diagram.");
                return;
            }

            string[] batchSizeLabels = batchGroups.Select(g => g.Key.ToString()).ToArray();
            double[] positions = Enumerable.Range(0, batchGroups.Count).Select(i => (double)i).ToArray();

            // For hver batch størrelse, lav separate målinger og gennemsnit
            List<double> efCore = new List<double>();
            List<double> npgSql = new List<double>();
            List<double> mongoDb = new List<double>();
            List<double> efCoreAvg = new List<double>();
            List<double> npgSqlAvg = new List<double>();
            List<double> mongoDbAvg = new List<double>();

            foreach (var group in batchGroups)
            {
                var groupData = group.ToList();

                // Filtrer de normale målinger
                var normalData = groupData.Where(r => !r.IsAverage).ToList();
                // Find gennemsnittet for hver batch
                var averageData = groupData.FirstOrDefault(r => r.IsAverage);

                // Tilføj data til målingerne
                efCore.AddRange(normalData.Select(r => r.EFCorePG / 1000.0));
                npgSql.AddRange(normalData.Select(r => r.NpgSql / 1000.0));
                mongoDb.AddRange(normalData.Select(r => r.MongoDb / 1000.0));

                // Hvis gennemsnittet findes, tilføj det som en ekstra søjle
                if (averageData != null)
                {
                    efCoreAvg.Add(averageData.EFCorePG / 1000.0);
                    npgSqlAvg.Add(averageData.NpgSql / 1000.0);
                    mongoDbAvg.Add(averageData.MongoDb / 1000.0);
                }
                else
                {
                    // Hvis ingen gennemsnit, tilføj tomme data for gennemsnittene
                    efCoreAvg.Add(0);
                    npgSqlAvg.Add(0);
                    mongoDbAvg.Add(0);
                }
            }

            // Kontroller om alle lister har samme længde
            int dataCount = efCore.Count;
            if (dataCount != npgSql.Count || dataCount != mongoDb.Count || dataCount != efCoreAvg.Count || dataCount != npgSqlAvg.Count || dataCount != mongoDbAvg.Count)
            {
                Console.WriteLine("Fejl: Mængden af data for målinger og gennemsnit stemmer ikke overens.");
                return;
            }

            // Definér barWidth for at gøre søjlerne smallere
            double barWidth = 0.1; // Smallere søjler
            double spacing = 0.15; // Afstand mellem grupper af søjler

            // Tilføj bar plots med justerede positioner for at skabe smalere søjler
            var efCorePlot = plt.AddBar(efCore.ToArray(),
                positions.Select(p => p - 1.5 * (barWidth + spacing)).ToArray());
            efCorePlot.Label = "EFCore";
            efCorePlot.FillColor = Color.FromArgb(255, 99, 132);
            efCorePlot.BarWidth = barWidth;

            var npgSqlPlot = plt.AddBar(npgSql.ToArray(),
                positions.Select(p => p - 0.5 * (barWidth + spacing)).ToArray());
            npgSqlPlot.Label = "NpgSql";
            npgSqlPlot.FillColor = Color.FromArgb(54, 162, 235);
            npgSqlPlot.BarWidth = barWidth;

            var mongoDbPlot = plt.AddBar(mongoDb.ToArray(),
                positions.Select(p => p + 0.5 * (barWidth + spacing)).ToArray());
            mongoDbPlot.Label = "MongoDb";
            mongoDbPlot.FillColor = Color.FromArgb(75, 192, 192);
            mongoDbPlot.BarWidth = barWidth;

            // Tilføj gennemsnitsmålinger som en separat søjle
            var efCoreAvgPlot = plt.AddBar(efCoreAvg.ToArray(),
                positions.Select(p => p - 1.5 * (barWidth + spacing) + 0.25).ToArray());
            efCoreAvgPlot.Label = "EFCore Gennemsnit";
            efCoreAvgPlot.FillColor = Color.FromArgb(255, 159, 64);
            efCoreAvgPlot.BarWidth = barWidth;

            var npgSqlAvgPlot = plt.AddBar(npgSqlAvg.ToArray(),
                positions.Select(p => p - 0.5 * (barWidth + spacing) + 0.25).ToArray());
            npgSqlAvgPlot.Label = "NpgSql Gennemsnit";
            npgSqlAvgPlot.FillColor = Color.FromArgb(255, 159, 64);
            npgSqlAvgPlot.BarWidth = barWidth;

            var mongoDbAvgPlot = plt.AddBar(mongoDbAvg.ToArray(),
                positions.Select(p => p + 0.5 * (barWidth + spacing) + 0.25).ToArray());
            mongoDbAvgPlot.Label = "MongoDb Gennemsnit";
            mongoDbAvgPlot.FillColor = Color.FromArgb(255, 159, 64);
            mongoDbAvgPlot.BarWidth = barWidth;

            // Akse- og plot-indstillinger
            plt.XTicks(positions, batchSizeLabels);
            plt.XLabel("Batch Size");
            plt.YLabel("Tid (sekunder)");
            plt.Title(resultSets[0].TestType);
            plt.Legend(location: Alignment.UpperRight);
            plt.SetAxisLimits(yMin: 0);

            // Gem diagrammet
            plt.SaveFig(outputPath);
            Console.WriteLine($"Diagram gemt til: {outputPath}");
        }
    }
}
