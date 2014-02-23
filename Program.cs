using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD11
{
    class Program
    {
        static void Main(string[] args)
        {
            using (GameContainer game = new GameContainer()) {
                game.Run();
            }
        }
    }
}
