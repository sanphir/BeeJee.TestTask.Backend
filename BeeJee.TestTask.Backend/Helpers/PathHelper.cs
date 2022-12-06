using System.Diagnostics;

namespace BeeJee.TestTask.Backend.Helpers
{
    public class PathHelper
    {
        private static string GetBasePath()
        {
            using (var processModule = Process.GetCurrentProcess().MainModule)
            {
                return Path.GetDirectoryName(processModule?.FileName ?? "");
            }
        }
        public static string GetConfigPath()
        {
            const string configFileName = "appsettings.json";

            var configPath = Path.Combine(GetBasePath(), configFileName);
            if (File.Exists(configPath))
            {
                return configPath;
            }

            var basePath = Directory.GetParent(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).ToString();
            configPath = Path.Combine(basePath, configFileName);
            if (File.Exists(configPath))
            {
                return configPath;
            }

            basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();
            return Path.Combine(basePath, configFileName);
        }
    }
}
