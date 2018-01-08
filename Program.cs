using System;

namespace Uno
{
    class Program
    {
        static void Main(string[] args)
        {
            Deck deck1 = new Deck();
            System.Console.WriteLine("Enter your name:");
            System.Console.Out.Flush();
            string input = System.Console.ReadLine();
            while(input.Length < 1)
            {
                System.Console.WriteLine("Name is too short.");
                input = System.Console.ReadLine();
            }
            Player player1 = new Player(input);
            Game game = new Game(deck1, player1);
            game.Run();
        }
    }
}
