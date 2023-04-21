using System;
using System.Net.Sockets;

namespace SeaBattle
{
    public enum GameType
    { 
        Singleplayer,
        Multiplayer,
    }
    class Menu
    {
        private int gamemodeNumber = 0;

        private string multiplayerOption = " ";

        private SeaBattle game = new SeaBattle();

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

                if(gamemodeNumber > 3 || gamemodeNumber < 1) isCorrectInput = false;

                if (!isCorrectInput) Console.WriteLine("Wrong input");
            }
            while (!isCorrectInput);
        }
        private void HandleInput()
        {
            switch (gamemodeNumber) 
            {
                case 1:
                    game.Start(false, true, GameType.Singleplayer);
                    break;
                case 2:
                    ShowMultiplayerOptions();
                    break;
                case 3:
                    game.Start(true, true, GameType.Singleplayer);
                    break;
            }
        }
        private void ShowMultiplayerOptions()
        {
            Console.Clear();

            Console.WriteLine("Choose to player mode");
            Console.WriteLine("Write Host - to create server");
            Console.WriteLine("Write Join - to join opened game");

            GetMultiplayerOptionsInput();
        }
        private void GetMultiplayerOptionsInput()
        {
            do
            {
                multiplayerOption = " ";

                multiplayerOption = Console.ReadLine();

                if (multiplayerOption is "Host" || multiplayerOption is "Join") break;
            }
            while (true);

            switch(multiplayerOption)
            {
                case "Host":
                    game.Start(false, false, GameType.Multiplayer);
                    break;
                case "Join":
                    break;
            }
        }
    }
}
