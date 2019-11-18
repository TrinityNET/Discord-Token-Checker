using System.Collections.Generic;

namespace TrinityCord
{
    class Variables
    {
        public static List<string> Tokens = new List<string>();
        public static List<string> Proxies = new List<string>();

        public static List<string> WorkingTokens = new List<string>();
        public static List<string> WorkingProxies = new List<string>();

        public static List<string> NonWorkingTokens = new List<string>();
        public static int NonWorkingProxies = 0;

        public static int CheckedProxies = 0;

        public static bool ProxiesChecked = false;
    }
}
