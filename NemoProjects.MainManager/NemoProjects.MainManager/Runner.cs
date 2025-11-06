using System;
using System.Diagnostics;

namespace NemoProjects.MainManager
{
    public class Runner
    {
        /// <summary>
        /// metodo che si occupa di eseguire un applicazione 
        /// </summary>
        public void Run(object appInfo)
        {
            dynamic app = appInfo;

            try
            {
                Console.Clear();
                Console.WriteLine($"Avvio di '{app.Name}' in una nuova finestra...");
                Console.WriteLine();

                var startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/K {app.Command}", // /K = lascia la finestra aperta dopo l'esecuzione
                    UseShellExecute = true,          // necessario per aprire una nuova finestra
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Normal
                };

                var process = Process.Start(startInfo);
                if (process == null)
                {
                    Menu.PressAnyKey($"Impossibile avviare '{app.Name}'.");
                    return;
                }

                Console.WriteLine($"'{app.Name}' avviato (PID: {process.Id}).");
                Console.WriteLine("Puoi chiudere l'app in qualunque momento; il launcher resterà attivo.");
                Console.WriteLine();

                // opzionale: attendi la chiusura del processo, se vuoi bloccare fino a termine
                process.WaitForExit();

                Menu.PressAnyKey($"'{app.Name}' terminato con codice {process.ExitCode}");
            }
            catch (Exception ex)
            {
                Menu.PressAnyKey($"Errore durante l'avvio di '{app.Name}': {ex.Message}");
            }
        }
    }
}
