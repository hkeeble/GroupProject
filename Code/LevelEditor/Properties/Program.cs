using System;

namespace LevelEditor
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Source game = new Source())
            {
                game.Run();
            }
        }
    }
#endif
}

