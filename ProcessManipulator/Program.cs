using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace ProcessManipulator
{
    internal class Program
    {
        static void ListAllRunningProcesses()
        {
            // Получить все процессы на локальной машине, упорядоченные по PID
            var runningProcs = from proc in Process.GetProcesses() orderby proc.Id select proc;

            // Вывести для каждого процесса идентификатор PID и имя
            foreach(var p in runningProcs)
            {
                string info = $"-> PID: {p.Id}\tName: {p.ProcessName}";
                Console.WriteLine(info);
            }
            Console.WriteLine("*************************************************************************************\n");
        }

        // Если процесс с PID, равным 30592, не существует, то сгенерируется исключение во время выполнения
        static void GetSpecificProcess()
        {
            Process proc = null;
            try
            {
                proc = Process.GetProcessById(30592);
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }  
        }

        static void EnumThreadsForPid(int pID)
        {
            Process theProc = null;
            try
            {
                theProc = Process.GetProcessById(pID);
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            // Вывести статистические сведения по каждому потоку в указанном процессе
            Console.WriteLine("Here are the threads used by: {0}", theProc.ProcessName);
            ProcessThreadCollection theThreads = theProc.Threads;
            foreach(ProcessThread pt in theThreads)
            {
                string info = $"-> Thread ID: {pt.Id}\tStart Time: {pt.StartTime.ToShortTimeString()}\tPriority: {pt.PriorityLevel}";
                Console.WriteLine(info);
            }
            Console.WriteLine("*************************************************************************************\n");
        }

        static void EnumModsForPid(int pID)
        {
            Process theProc = null;
            try
            {
                theProc = Process.GetProcessById(pID);
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            Console.WriteLine("Here are the loaded modules for: {0}", theProc.ProcessName);
            ProcessModuleCollection theMods = theProc.Modules;
            foreach(ProcessModule pm in theMods)
            {
                string info = $"-> Mod Name: {pm.ModuleName}";
                Console.WriteLine(info);
            }
        }

        static void StartAndKillProcess()
        {
            Process proc = null;

            // Запустить GOG
            try
            {
                proc = Process.Start(@"D:\GOG Galaxy\GalaxyClient.exe");
            }
            catch(InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Уничтожить процесс по нажатию <Enter>
            Console.Write("--> Hit enter to kill {0}...", proc.ProcessName);
            Console.ReadLine();

            // Уничтожить все процессы GalaxyClient.exe
            try
            {
                foreach(var p in Process.GetProcessesByName("GalaxyClient"))
                {
                    p.Kill(true);
                }
            }
            catch(InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void UseApplicationVerbs()
        {
            int i = 0;
            ProcessStartInfo si = new ProcessStartInfo(@"D:\Бэкап\Документы\Резюме ОВБ 2018.docx");
            foreach(var verb in si.Verbs)
            {
                Console.WriteLine($" {i++}. {verb}");
            }
            si.WindowStyle = ProcessWindowStyle.Maximized;
            si.Verb = "OpenAsReadOnly";
            si.UseShellExecute = true;
            Process.Start(si);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("***** Fun with Processes *****\n");
            ListAllRunningProcesses();
            GetSpecificProcess();

            // Запросить у пользователя PID и вывести набор активных потоков
            /*Console.WriteLine("***** Enter PID of process to investigate *****");
            Console.Write("PID: ");
            string? pID = Console.ReadLine();
            int theProcID = int.Parse(pID);

            EnumThreadsForPid(theProcID);
            EnumModsForPid(theProcID);*/

            //StartAndKillProcess();
            UseApplicationVerbs();
            Console.ReadLine();
        }
    }
}