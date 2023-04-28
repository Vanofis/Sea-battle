using System;

namespace SeaBattle
{
    public class GameHandler
    {
        private int player1Wins = 0;
        private int player2Wins = 0;

        private const int winsCount = 3;

        private SeaBattle game = new SeaBattle();

        private PlayerProfile currentPlayerProfile = null;
        private PlayerProfile aiProfile = null;

        private PlayerTurn lastWinner = PlayerTurn.Draw;

        public void LaunchGame(bool player1IsAI, bool player2IsAI, PlayerProfile profile)
        {
            currentPlayerProfile = profile;

            CheckForAIvsAI(player1IsAI, player2IsAI);

            currentPlayerProfile.AddGame();

            SaveData();

            while(!SomeoneHas3Wins())
            {
                StartRound(player1IsAI, player2IsAI);

                lastWinner = game.GetResult();

                AddWin();

                SaveData();
            }

            WriteChampion();

            SaveData();
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
        private void SaveData()
        {
            string name = @"\" + currentPlayerProfile.Name + ".xml";

            XMLManager.SerializeXML(currentPlayerProfile, name);
        }
        private void CheckForAIvsAI(bool player1IsAI, bool player2IsAI)
        {
            if(player1IsAI && player2IsAI)
                currentPlayerProfile = XMLManager.DeserializeXML("Ai.xml");
        }
        private void AddWin()
        {
            switch (lastWinner)
            {
                case PlayerTurn.Player1:
                    player1Wins++;
                    currentPlayerProfile.AddWonRound();
                    break;
                case PlayerTurn.Player2:
                    currentPlayerProfile.AddLostRound();
                    player2Wins++;
                    break;
                case PlayerTurn.Draw:
                    player1Wins++;
                    player2Wins++;
                    break;
            }
        }
        private void StartRound(bool player1IsAI, bool player2IsAI)
        {
            game.Start(player1IsAI, player2IsAI);
        }
        private void WriteChampion()
        {
            string champion = "No one";

            if (player1Wins > player2Wins)
            {
                champion = currentPlayerProfile.Name;

                currentPlayerProfile.AddWonGames();
            }
            else if (player2Wins > player1Wins)
            {
                champion = "Player 2";

                currentPlayerProfile.AddLostGame();
            }

            Console.WriteLine($"{champion} is absolute champion!");
        }
        private bool SomeoneHas3Wins()
        {
            return player1Wins >= winsCount || player2Wins >= winsCount;
        }
    }
}
