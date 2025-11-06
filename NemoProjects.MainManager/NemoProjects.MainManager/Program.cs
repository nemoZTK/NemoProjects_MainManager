using NemoProjects.MainManager;

namespace NemoProjects.MainManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string basePath = AppContext.BaseDirectory;
            var menu = new Menu(Path.Combine(basePath, "apps.txt"));
            menu.Start();

        }
    }
}