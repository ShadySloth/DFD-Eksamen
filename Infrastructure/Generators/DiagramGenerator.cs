using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Database_Benchmarking.Consoles.SharedModels;
using ScottPlot;

namespace Database_Benchmarking.Infrastructure.Generators
{
    public class DiagramGenerator
    {
        public static void GenerateDiagram(List<ResultSet> resultSets, string outputPath)
        {
            var plt = new ScottPlot.Plot(1200, 600);

            EnsureOutputDirectoryExists(outputPath);

            var batchGroups = resultSets
                .GroupBy(r => r.BatchSize)
                .OrderBy(g => g.Key)
                .ToList();

            double barWidth = 0.012;
            double innerSpacing = 0.01;
            double groupSpacing = 0.02;

            var series = new List<SeriesData>
            {
                new("EFCore", r => r.EFCorePG, Color.FromArgb(255, 99, 132), Color.FromArgb(160, 255, 99, 132)),
                new("MongoDb", r => r.MongoDb, Color.FromArgb(75, 192, 192), Color.FromArgb(160, 75, 192, 192)),
                new("NpgSql", r => r.NpgSql, Color.FromArgb(54, 162, 235), Color.FromArgb(160, 54, 162, 235))
            };

            double currentX = 0;
            var xTicks = new List<(double, string)>();

            foreach (var group in batchGroups)
            {
                int countBefore = series.Sum(s => s.Positions.Count + s.AvgPositions.Count);

                foreach (var s in series)
                {
                    // Regular entries
                    foreach (var r in group.Where(r => !r.IsAverage && s.Selector(r) > 0))
                    {
                        s.Values.Add(s.Selector(r) / 1000.0);
                        s.Positions.Add(currentX);
                        currentX += barWidth + innerSpacing;
                    }

                    // Average entry
                    var avg = group.FirstOrDefault(r => r.IsAverage && s.Selector(r) > 0);
                    if (avg != null)
                    {
                        s.AvgValues.Add(s.Selector(avg) / 1000.0);
                        s.AvgPositions.Add(currentX);
                        currentX += barWidth + innerSpacing;
                    }
                }

                int countAfter = series.Sum(s => s.Positions.Count + s.AvgPositions.Count);

                double groupMid = series
                    .SelectMany(s => s.Positions.Concat(s.AvgPositions))
                    .Skip(countBefore)
                    .Take(countAfter - countBefore)
                    .DefaultIfEmpty(currentX)
                    .Average();

                xTicks.Add((groupMid, group.Key.ToString()));
                currentX += groupSpacing;
            }

            // Legg til plott
            foreach (var s in series)
            {
                var p1 = plt.AddBar(s.Values.ToArray(), s.Positions.ToArray());
                p1.Label = s.Name;
                p1.FillColor = s.Color;
                p1.BarWidth = barWidth;

                var p2 = plt.AddBar(s.AvgValues.ToArray(), s.AvgPositions.ToArray());
                p2.Label = $"{s.Name} (avg)";
                p2.FillColor = s.AvgColor;
                p2.BarWidth = barWidth;
            }

            plt.XTicks(xTicks.Select(x => x.Item1).ToArray(), xTicks.Select(x => x.Item2).ToArray());
            plt.XLabel("Batch Size");
            plt.YLabel("Tid (sekunder)");
            plt.Title(resultSets.First().TestType);
            plt.Legend(location: Alignment.UpperRight);
            plt.SetAxisLimits(yMin: 0);

            // Legg til annotasjoner for avg / 100
            foreach (var group in batchGroups)
            {
                foreach (var s in series)
                {
                    var avg = group.FirstOrDefault(r => r.IsAverage && s.Selector(r) > 0);
                    if (avg != null)
                    {
                        double msPer100 = (s.Selector(avg) / (double)group.Key) * 100;
                        int idx = s.AvgValues.IndexOf(s.Selector(avg) / 1000.0);
                        if (idx >= 0)
                        {
                            double xpos = s.AvgPositions[idx];
                            plt.AddText($"{msPer100:F1} ms / 100", xpos, -0.02, 10, s.Color);
                        }
                    }
                }
            }

            plt.SaveFig(outputPath);
            Console.WriteLine($"Diagram gemt til: {outputPath}");
        }

        private static void EnsureOutputDirectoryExists(string outputPath)
        {
            var directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        private class SeriesData
        {
            public string Name { get; }
            public Func<ResultSet, double> Selector { get; }
            public Color Color { get; }
            public Color AvgColor { get; }
            public List<double> Values { get; } = new();
            public List<double> Positions { get; } = new();
            public List<double> AvgValues { get; } = new();
            public List<double> AvgPositions { get; } = new();

            public SeriesData(string name, Func<ResultSet, double> selector, Color color, Color avgColor)
            {
                Name = name;
                Selector = selector;
                Color = color;
                AvgColor = avgColor;
            }
        }
    }
}
