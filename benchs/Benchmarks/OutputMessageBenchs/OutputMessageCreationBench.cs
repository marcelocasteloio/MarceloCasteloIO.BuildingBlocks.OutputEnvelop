using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using Benchmarks.Interfaces;
using MarceloCasteloIO.BuildingBlocks.OutputEnvelop.Enums;
using MarceloCasteloIO.BuildingBlocks.OutputEnvelop.Models;

namespace Benchmarks.OutputMessageBenchs;

[SimpleJob(RunStrategy.Throughput, launchCount: -1)]
[HardwareCounters(
    HardwareCounter.CacheMisses,
    HardwareCounter.Timer,
    HardwareCounter.TotalCycles,
    HardwareCounter.TotalIssues,
    HardwareCounter.BranchMispredictions,
    HardwareCounter.BranchInstructions
)]
[MemoryDiagnoser]
public class OutputMessageCreationBench
    : IBenchmark
{
    private static string _defaultMessageCode = null!;
    private static string _defaultMessageDescription = null!;

    [Params(1, 5, 10)]
    public int OutputMessageCount { get; set; }

    [GlobalSetup]
    public void InitializeValues()
    {
        _defaultMessageCode = new string('A', 50);
        _defaultMessageDescription = new string('A', 255);
    }

    [Benchmark(Baseline = true)]
    public void Create()
    {
        for (int i = 0; i < OutputMessageCount; i++)
            OutputMessage.Create(OutputMessageType.Information, _defaultMessageCode, _defaultMessageDescription);
    }
    [Benchmark]
    public void CreateInformation()
    {
        for (int i = 0; i < OutputMessageCount; i++)
            OutputMessage.CreateInformation(_defaultMessageCode, _defaultMessageDescription);
    }
    [Benchmark()]
    public void CreateWithoutDescription()
    {
        for (int i = 0; i < OutputMessageCount; i++)
            OutputMessage.Create(OutputMessageType.Information, _defaultMessageCode);
    }
    [Benchmark]
    public void CreateInformationWithoutDescription()
    {
        for (int i = 0; i < OutputMessageCount; i++)
            OutputMessage.CreateInformation(_defaultMessageCode);
    }
}
