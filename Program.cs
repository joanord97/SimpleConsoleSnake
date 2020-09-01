using System;
using GameLogic;
using System.Collections.Generic;


namespace main
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(10,20);
            game.render();

            game.run();

            
            
        }
    }
}
