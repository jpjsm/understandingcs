// <copyright file="Program.cs" company="OpenTelemetry Authors">
// Copyright The OpenTelemetry Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System.Diagnostics.Metrics;
using OpenTelemetry;
using OpenTelemetry.Metrics;

using System;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace OpenTelemetryMetrics;

public class Program
{
    private static readonly Meter MyMeter = new("ACME Inc.Produce.Fruits", "1.0");
    private static readonly Counter<long> MyFruitCounter = MyMeter.CreateCounter<long>(
        "MyFruitCounter"
    );
    private static readonly Histogram<double> MyTimerHistogram = MyMeter.CreateHistogram<double>(
        "MyTimerMeter",
        unit: "millisecond"
    );
    private static readonly Counter<double> MyTimeAccumulator = MyMeter.CreateCounter<double>(
        "MyTimeAccumulator"
    );
    private static readonly Histogram<double> MyFruitWeightHistogram =
        MyMeter.CreateHistogram<double>("MyFruitWeightMeter", unit: "gram");

    private static readonly Dictionary<string, List<string>> FruitColors = new Dictionary<
        string,
        List<string>
    >
    {
        {
            "apple",
            new List<string> { "green", "white", "yellow", "striped", "red" }
        },
        {
            "lemon",
            new List<string> { "True Lemon", "Citron", "Lemonade", "Lime", "yellow", "green" }
        },
        {
            "peach",
            new List<string>
            {
                "yellow with a slight blush",
                "red-over cream",
                "red-blushed yellow skin",
                "slightly blushed golden orange skin",
                "yellow",
                "red-blushed yellow skin"
            }
        },
    };

    public static void Main()
    {
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddMeter(MyMeter.Name)
            .AddOtlpExporter(
                opt =>
                {
                    opt.Protocol = OtlpExportProtocol.Grpc;
                    opt.Endpoint = new Uri("http://localhost:4317"); // This is the default if not specified, and Protocol == Grpc
                }
            )
            .AddConsoleExporter()
            .Build();

        Random rnd = new Random();
        double totalseconds = 0.0;
        string[] fruits = FruitColors.Keys.ToArray();
        while (!Console.KeyAvailable)
        {
            int elapsedtime = rnd.Next(20, 50);
            totalseconds += elapsedtime / 1000.0;

            Thread.Sleep(elapsedtime);
            string fruit = fruits[rnd.Next(0, fruits.Length)];
            string color = FruitColors[fruit][rnd.Next(0, FruitColors[fruit].Count)];
            int qty = rnd.Next(1, 10);
            MyFruitCounter.Add(qty, new("name", fruit), new("color", color));
            //MyTimeAccumulator
            MyTimeAccumulator.Add(elapsedtime, new("name", fruit), new("color", color));
            MyTimerHistogram.Record(elapsedtime, new("name", fruit), new("color", color));
            MyFruitWeightHistogram.Record(
                500 + 50 * Math.Sin(totalseconds),
                new("name", fruit),
                new("color", color)
            );
        }
    }
}
