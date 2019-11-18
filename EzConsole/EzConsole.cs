using System;

namespace EzConsoleClass
{
    class EzConsole
    {
        public static void WriteLine(object value, ConsoleColor Color, bool read)
        {
            Console.ForegroundColor = Color;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.White;
            if (read)
                Console.ReadLine();
        }

        public static void Write(object value, ConsoleColor Color, bool read)
        {
            Console.ForegroundColor = Color;
            Console.Write(value);
            Console.ForegroundColor = ConsoleColor.White;
            if (read)
                Console.ReadLine();
        }
    }
}
