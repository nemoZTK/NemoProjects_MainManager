using System;
using System.Collections.Generic;
using System.IO;

namespace NemoProjects.MainManager
{
    public class Menu(string configPath )
    {
        private readonly string _configPath= configPath;
        private readonly Runner _runner =new();



        // ================================================ M E T O D I  P R I V A T I =================================================

        /// <summary>
        /// menu di selezione dell' applicazione da eseguire 
        /// </summary>
        private void RunSelectionLoop(List<AppInfo> apps)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Seleziona un'applicazione ===");
                Console.WriteLine();

                for (int i = 0; i < apps.Count; i++)
                    Console.WriteLine($"{i + 1}. {apps[i].Name}");

                Console.WriteLine("0. Torna indietro");
                Console.WriteLine();
                Console.Write("Scelta: ");

                if (!int.TryParse(Console.ReadLine(), out int choice) ||
                    choice < 0 || choice > apps.Count)
                {
                    PressAnyKey("Scelta non valida.");
                    continue;
                }

                if (choice == 0)
                    break;

                _runner.Run(apps[choice - 1]);
            }
        }

        /// <summary>
        /// metodo per caricare le applicazioni da file di configurazione
        /// </summary>
        /// <returns>una lista di AppInfo</returns>
        private List<AppInfo> LoadApplications()
        {
            var apps = new List<AppInfo>();

            if (!File.Exists(_configPath))
                return apps;

            foreach (var line in File.ReadAllLines(_configPath))
            {
                var trimmed = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#"))
                    continue;

                var parts = trimmed.Split('|', 2);
                if (parts.Length != 2)
                    continue;

                apps.Add(new AppInfo
                {
                    Name = parts[0].Trim(),
                    Command = parts[1].Trim()
                });
            }

            return apps;
        }

        /// <summary>
        /// metodo per scansionare una cartella e trovare eseguibili
        /// </summary>
        /// <returns>una lista di AppInfo</returns>
        private static List<AppInfo> ScanDirectory(string? directoryPath)
        {
            var apps = new List<AppInfo>();

            if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
                return apps;

            var files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);
            foreach (var path in files)
            {
                var ext = Path.GetExtension(path).ToLowerInvariant();
                if (ext is not (".exe" or ".jar" or ".py" or ".dll"))
                    continue;

                var name = Path.GetFileName(path);
                apps.Add(new AppInfo
                {
                    Name = name,
                    Command = GetExecutionCommand(path)
                });
            }

            return apps;
        }

        /// <summary>
        /// genera il comando di esecuzione in base all' estensione del file
        /// </summary>
        /// <returns></returns>
        private static string GetExecutionCommand(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return ext switch
            {
                ".jar" => $"java -jar \"{path}\"",
                ".py" => $"python \"{path}\"",
                ".dll" => $"dotnet \"{path}\"",
                _ => $"\"{path}\""
            };
        }

        private  void LaunchPythonScript()
        {

            string basePath = AppContext.BaseDirectory;
            // Percorso completo allo script Python (assumendo che sia nella root del progetto)
            string scriptPath = Path.Combine(basePath, "NemoProjects_template.py");

            if (!File.Exists(scriptPath))
            {
                Console.WriteLine($"Script non trovato in: {scriptPath}");
                PressAnyKey("Assicurati che il file esista nella root del progetto.");
                return;
            }

            // Comando per aprire una nuova finestra di cmd e avviare lo script
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/k \"cd /d \"{basePath}\" && python \"{scriptPath}\"\"",
                UseShellExecute = true,   // Necessario per aprire una nuova shell
                WorkingDirectory = basePath
            };

            try
            {
                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore nell'esecuzione dello script: {ex.Message}");
                PressAnyKey("Controlla che Python sia installato e nel PATH.");
            }
        }



        /// <summary>
        /// Mostra il menu iniziale con banner e selezione
        /// </summary>
        /// <returns>rirorna la stringa scritta dall' utente</returns>
        private string StartMenu()
        {
            string basePath = AppContext.BaseDirectory;
            Console.WriteLine(File.ReadAllText(Path.Combine(basePath, "banner.txt")));
            Console.WriteLine("1. usa lista eseguibili da apps.txt");
            Console.WriteLine("2. scansiona una cartella");
            Console.WriteLine("3. esegui script di creazione NemoProjects_<Lang>");
            Console.WriteLine("0. Esci");
            Console.WriteLine();
            Console.Write("Seleziona un'opzione: ");
            var res= Console.ReadLine();
            return res??"";
        }




        // ================================================ M E T O D I  P U B B L I C I =================================================

        /// <summary>
        /// fa premere un tasto per continuare e stampa un messaggio 
        /// </summary>
        public static void PressAnyKey(string message)
        {
            Console.WriteLine();
            Console.WriteLine(message);
            Console.WriteLine("Premi un tasto per continuare...");
            Console.ReadKey(true);
        }
        /// <summary>
        /// Loop principale del menu, gestisce la scelta da lista o da cartella
        /// </summary>
        public void Start()
        {
            while (true)
            {
                Console.Clear();
                
                var choice =StartMenu();

                List<AppInfo> apps;


                switch (choice)
                {
                    case "1":
                        apps = LoadApplications();
                        break;
                    case "2":
                        Console.Write("Inserisci il percorso della cartella: ");
                        var dir = Console.ReadLine();
                        apps = ScanDirectory(dir);
                        break;
                    case "3":
                        LaunchPythonScript();
                        PressAnyKey("Script Python lanciato in una nuova shell.");
                        continue;
                    case "0":
                        return;
                    default:
                        PressAnyKey("Scelta non valida.");
                        continue;
                }

                if (apps.Count == 0)
                {
                    PressAnyKey("Nessuna applicazione trovata.");
                    continue;
                }

                RunSelectionLoop(apps);
            }
        }
   }

}
