using System;
using System.Collections.Generic;
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
            // Create a new Plot object using the default constructor
            var plt = new Plot();  // Default constructor for ScottPlot

            // Group resultater efter batch size
            var batchSizes = new Dictionary<int, List<ResultSet>>();
            foreach (var result in resultSets)
            {
                if (!batchSizes.ContainsKey(result.BatchSize))
                {
                    batchSizes[result.BatchSize] = new List<ResultSet>();
                }
                batchSizes[result.BatchSize].Add(result);
            }

            // Lists to store the values for each series (EFCore, NpgSql, MongoDb, Average)
            var efCoreTimes = new List<double>();
            var npgSqlTimes = new List<double>();
            var mongoDbTimes = new List<double>();
            var averageTimes = new List<double>();

            // Calculate average times for each batch size
            foreach (var batchSize in batchSizes)
            {
                var averageEFCore = GetAverageTimeSpan(batchSize.Value, r => r.EFCorePG);
                var averageNpgSql = GetAverageTimeSpan(batchSize.Value, r => r.NpgSql);
                var averageMongoDb = GetAverageTimeSpan(batchSize.Value, r => r.MongoDb);

                efCoreTimes.Add(averageEFCore.TotalSeconds);
                npgSqlTimes.Add(averageNpgSql.TotalSeconds);
                mongoDbTimes.Add(averageMongoDb.TotalSeconds);
                averageTimes.Add((averageEFCore.TotalSeconds + averageNpgSql.TotalSeconds + averageMongoDb.TotalSeconds) / 3);
            }

            // Set up positions for each bar group (spacing for grouped bars)
            double barWidth = 0.2; // Width of each bar
            double spacing = 0.3; // Space between groups of bars

            var xPositions = Enumerable.Range(0, batchSizes.Count).Select(x => x * (barWidth + spacing)).ToArray();

            // Create an array with bar widths for each bar
            double[] barWidths = new double[efCoreTimes.Count];
            for (int i = 0; i < barWidths.Length; i++)
            {
                barWidths[i] = barWidth; // All bars have the same width
            }

            // Add bars for each database with the appropriate color
            plt.AddBar(efCoreTimes.ToArray(), xPositions, barWidths, System.Drawing.Color.FromArgb(255, 99, 132)); // EFCore bar series (color: red)
            plt.AddBar(npgSqlTimes.ToArray(), xPositions.Select(x => x + barWidth).ToArray(), barWidths, System.Drawing.Color.FromArgb(54, 162, 235)); // NpgSql bar series (color: blue)
            plt.AddBar(mongoDbTimes.ToArray(), xPositions.Select(x => x + 2 * barWidth).ToArray(), barWidths, System.Drawing.Color.FromArgb(75, 192, 192)); // MongoDb bar series (color: green)
            plt.AddBar(averageTimes.ToArray(), xPositions.Select(x => x + 3 * barWidth).ToArray(), barWidths, System.Drawing.Color.FromArgb(255, 159, 64)); // Average bar series (color: orange)

            // Set X-axis labels as batch sizes
            var batchSizeLabels = batchSizes.Keys.ToArray();
            plt.XTicks(xPositions, batchSizeLabels.Select(x => x.ToString()).ToArray());

            // Set Y-axis with time in seconds
            plt.YLabel("Time (Seconds)");
            plt.Title("Database Benchmark Results");


            // Save the plot to a PNG file
            plt.SaveFig(outputPath);

            Console.WriteLine($"Diagram gemt som {outputPath}");
        }

        private static TimeSpan GetAverageTimeSpan(List<ResultSet> resultSets, Func<ResultSet, TimeSpan> selector)
        {
            // Calculate the average TimeSpan for the relevant measurements
            double averageSeconds = 0;
            foreach (var result in resultSets)
            {
                averageSeconds += selector(result).TotalSeconds;
            }
            averageSeconds /= resultSets.Count;
            return TimeSpan.FromSeconds(averageSeconds);
        }
    }
}
