using Amazon.CDK;

namespace Cdk2;

internal sealed class Program
{
    public static void Main(string[] args)
    {
        var app = new App();
        _ = new Cdk2Stack(app, "Cdk2Stack", new StackProps());
        app.Synth();
    }
}
