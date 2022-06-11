using Lightning.Scanner;
using Lightning.Utils;

Benchmark.Run(
    "Scanned In:",
    () =>
    {
        var scanner = new LightningScanner();
    }
);
