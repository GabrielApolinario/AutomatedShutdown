using System.ComponentModel;
using System.Diagnostics;
using System.Timers;

public class Program
{
    const int DEFAULT_MINUTES_UNTIL_SHUTDOWN = 60;

    private static string? userInput;
    private static bool parsedMinutesSuccessfully;
    private static int minutesUntilShutdown;

    private static int SecondsUntilShutdown => minutesUntilShutdown * DEFAULT_MINUTES_UNTIL_SHUTDOWN;
    private static void Main(string[] args)
    {
        ShowMenu();
        GetUserInput();
        DefineMinutesUntilShutdown();
        ScheduleShutdown();
        ShowShutdownMessage();
    }

    private static void ScheduleShutdown()
    {
        Process.Start("shutdown", $@"/s /f /t {SecondsUntilShutdown}");
    }

    private static void ShowShutdownMessage()
    {
        Console.Clear();

        TimeSpan timeSpan = TimeSpan.FromMinutes(minutesUntilShutdown);
        string minutesText = timeSpan.TotalMinutes > 1 ? "minutos" : "minuto";
        Console.WriteLine($@"O computador será desligado em {timeSpan.TotalMinutes} {minutesText}. See ya!");
    }

    private static void GetUserInput(bool retryAtempt = false)
    {        
        if (retryAtempt)
        {
            Console.Clear();
            Console.WriteLine("A opção escolhida é inválida, tente novamente.");
            ShowMenu();
        }
        
        userInput = Console.ReadLine();
    }

    private static void ShowMenu()
    {
        Console.WriteLine(
                "╔═════════════════════════════════════════════════════════════════════════╗" +
            "\r\n║                                  MENU                                   ║" +
            "\r\n╠═════════════════════════════════════════════════════════════════════════╣");

        Console.WriteLine(@"║ 1. Aperte ""Enter"" para desligar o camputador em 1 hora                  ║");
        Console.WriteLine("║ 2. Digite o tempo em minutos para desligar o computador automaticamente ║");
        //Console.WriteLine(@"║ 3. Cancele um agendamento de desligamento automático digitando ""CANCEL"" ");

        Console.WriteLine("╚═════════════════════════════════════════════════════════════════════════╝");
    }

    private static void DefineMinutesUntilShutdown()
    {
        minutesUntilShutdown = DEFAULT_MINUTES_UNTIL_SHUTDOWN;

        if (!string.IsNullOrEmpty(userInput))
        {
            parsedMinutesSuccessfully = int.TryParse(userInput, out minutesUntilShutdown);

            while (!parsedMinutesSuccessfully)
            {
                GetUserInput(true);
                DefineMinutesUntilShutdown();
            }
        }
    }
}