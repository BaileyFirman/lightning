using Lightning.Scanning;
using Lightning.Utils;

var file = Benchmark.Run(
    "File Retrived In:",
    () => args.Length == 1 ? File.ReadAllText(args[0]) : string.Empty
);

var tokens = Benchmark.Run(
    "Scanned In:",
    () =>
    {
        var scanner = new LightningScanner();
        return scanner.ScanTokens(file);
    }
);

foreach (var token in tokens)
{
    Console.WriteLine(token);
}
