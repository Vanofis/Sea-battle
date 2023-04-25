using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle
{
    public class GameHandler
    {
        private int player1Wins = 0;
        private int player2Wins = 0;

        private const int winsCount = 3;

        private SeaBattle game = new SeaBattle();

        public void LaunchGame(bool player1IsAI, bool player2IsAI)
        {
            while(!SomeoneHas3Wins())
            {
                StartRound(player1IsAI, player2IsAI);
            }

            WriteChampion();
        }
        public void AddWin(int playerIndex)
        {
            switch (playerIndex) 
            {
                case 1:
                    player1Wins++;
                    break;
                case 2:
                    player2Wins++;
                    break;
            }
        }
        private void StartRound(bool player1IsAI, bool player2IsAI)
        {
            game.Start(player1IsAI, player2IsAI, this);
        }
        private void WriteChampion()
        {
            string champion = player1Wins > player2Wins ? "Player 1" : "Player2";

            Console.WriteLine($"{champion} is absolute champion!");
        }
        private bool SomeoneHas3Wins()
        {
            return player1Wins >= winsCount || player2Wins >= winsCount;
        }
    }
}
