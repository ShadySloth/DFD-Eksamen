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
        public static void GenerateDiagram(List<ResultSet> resultSets, string outputPath, string type)
        {
            EnsureOutputDirectoryExists(outputPath);

            ExportUnitsPerSecondTableAsImage(resultSets, outputPath, type);

            var plt = new ScottPlot.Plot(1200, 600);
            
            var batchGroups = resultSets
                .GroupBy(r => r.BatchSize)
                .OrderBy(g => g.Key)
                .ToList();

            double barWidth = 0.055;
            double innerSpacing = 0.01;
            double groupSpacing = 0.2;

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
                double groupX = Math.Log10(group.Key);

                double groupStartX = groupX - ((series.Count * 2 * (barWidth + innerSpacing)) / 2.0);

                int barIndex = 0;

                foreach (var s in series)
                {
                    foreach (var r in group.Where(r => !r.IsAverage && s.Selector(r) > 0))
                    {
                        s.Values.Add(s.Selector(r) / 1000.0);
                        s.Positions.Add(groupStartX + barIndex * (barWidth + innerSpacing));
                        barIndex++;
                    }

                    var avg = group.FirstOrDefault(r => r.IsAverage && s.Selector(r) > 0);
                    if (avg != null)
                    {
                        s.AvgValues.Add(s.Selector(avg) / 1000.0);
                        s.AvgPositions.Add(groupStartX + barIndex * (barWidth + innerSpacing));
                        barIndex++;
                    }
                }

                double groupCenterX = groupStartX + ((barIndex - 1) * (barWidth + innerSpacing)) / 2.0;
                xTicks.Add((groupCenterX, group.Key.ToString()));
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

            plt.SaveFig(outputPath + "/" + type + "_diagram" + ".jpg");
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
        
private static void ExportUnitsPerSecondTableAsImage(List<ResultSet> resultSets, string outputPath, string type)
{
    var batchGroups = resultSets
        .GroupBy(r => r.BatchSize)
        .OrderBy(g => g.Key)
        .ToList();

    var headers = new[] { "Batch Size", "EFCore (units/s)", "MongoDb (units/s)", "NpgSql (units/s)" };
    var rows = new List<string[]>();

    foreach (var group in batchGroups)
    {
        var ef = group.FirstOrDefault(r => r.IsAverage && r.EFCorePG > 0);
        var mongo = group.FirstOrDefault(r => r.IsAverage && r.MongoDb > 0);
        var npg = group.FirstOrDefault(r => r.IsAverage && r.NpgSql > 0);

        double efUnitsPerSec = ef != null ? group.Key / (ef.EFCorePG / 1000.0) : 0;
        double mongoUnitsPerSec = mongo != null ? group.Key / (mongo.MongoDb / 1000.0) : 0;
        double npgUnitsPerSec = npg != null ? group.Key / (npg.NpgSql / 1000.0) : 0;

        rows.Add(new[]
        {
            group.Key.ToString(),
            $"{efUnitsPerSec:F2}",
            $"{mongoUnitsPerSec:F2}",
            $"{npgUnitsPerSec:F2}"
        });
    }

    int cellPadding = 10;
    int cellWidth = 180;
    int cellHeight = 40;
    int rowCount = rows.Count + 1;
    int colCount = headers.Length;

    int width = colCount * cellWidth;
    int height = rowCount * cellHeight;

    using Bitmap bmp = new(width, height);
    using Graphics g = Graphics.FromImage(bmp);
    g.Clear(Color.White);

    using Pen pen = new(Color.Black, 1);
    using Font font = new("Arial", 12);
    using StringFormat sf = new()
    {
        Alignment = StringAlignment.Center,
        LineAlignment = StringAlignment.Center
    };

    // Draw headers
    for (int col = 0; col < colCount; col++)
    {
        Rectangle rect = new(col * cellWidth, 0, cellWidth, cellHeight);
        g.DrawRectangle(pen, rect);
        g.DrawString(headers[col], font, Brushes.Black, rect, sf);
    }

    // Draw rows
    for (int row = 0; row < rows.Count; row++)
    {
        for (int col = 0; col < colCount; col++)
        {
            Rectangle rect = new(col * cellWidth, (row + 1) * cellHeight, cellWidth, cellHeight);
            g.DrawRectangle(pen, rect);
            g.DrawString(rows[row][col], font, Brushes.Black, rect, sf);
        }
    }

    string filePath = Path.Combine(outputPath, type  + "_scheme" + ".png");
    bmp.Save(filePath);
}

    }
}
