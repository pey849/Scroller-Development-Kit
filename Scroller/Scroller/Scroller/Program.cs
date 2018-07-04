using System;

namespace Scroller
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //http://stackoverflow.com/questions/9679375/run-an-exe-from-c-sharp-code
            using (var Game = new ScrollerGame())
                Game.Run();
        }
    }
}

