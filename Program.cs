using ASNParserApp;

var appIsRunning = true;

var mainProcess = new MainProcess();

Console.WriteLine("To quit, enter q");
Console.WriteLine();

while (appIsRunning)
{
    mainProcess.MonitorFolder();

    var action = Console.ReadLine();

    if (action == "q")
    {
        appIsRunning = false;
        break;
    }
}