using System;
using System.Net.Sockets;

namespace SeaBattle
{
    class Menu
    {
        private int gamemodeNumber = 0;

        private string multiplayerOption = " ";

        GameHandler gameHandler = new GameHandler(); 

        public void LaunchMenu()
        {
            Console.WriteLine("Welcome to Sea battle!");
            Console.WriteLine("Choose gamemode:");
            Console.WriteLine("Write 1 - Player VS AI");
            Console.WriteLine("Write 2 - Player VS Player");
            Console.WriteLine("Write 3 - AI VS AI (Spectate battle)");

            GetInput();

            HandleInput();
        }
        private void GetInput()
        {
            bool isCorrectInput = false;

            do
            {
                gamemodeNumber = 0;

                isCorrectInput = int.TryParse(Console.ReadLine(), out gamemodeNumber);

                if(gamemodeNumber > 3 || gamemodeNumber < 1) 
                    isCorrectInput = false;

                if (!isCorrectInput) 
                    Console.WriteLine("Wrong input");
            }
            while (!isCorrectInput);
        }
        private void HandleInput()
        {
            switch (gamemodeNumber) 
            {
                case 1:
                    gameHandler.LaunchGame(false, true);
                    break;
                case 2:
                    gameHandler.LaunchGame(false, false);
                    break;
                case 3:
                    gameHandler.LaunchGame(true, true);
                    break;
            }
        }
    }
}
