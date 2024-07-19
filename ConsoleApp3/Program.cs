using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

class Program
{
    public static void Main(string[] args)
    {
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());
    }
}
