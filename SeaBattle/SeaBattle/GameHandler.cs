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
        private PlayerProfile enemyProfile = null;

        private PlayerTurn lastWinner = PlayerTurn.Draw;

        public void LaunchGame(PlayerProfile profile1, PlayerProfile profile2)
        {
            ManageProfiles(profile1, profile2);

            SaveData(currentPlayerProfile.Name + ".xml", currentPlayerProfile);
            SaveData(enemyProfile.Name + ".xml", enemyProfile);

            while(!SomeoneHas3Wins())
            {
                StartRound(profile1.IsAi, profile2.IsAi);

                lastWinner = game.GetResult();

                AddWin();

                SaveData(currentPlayerProfile.Name + ".xml", currentPlayerProfile);
                SaveData(enemyProfile.Name + ".xml", enemyProfile);
            }

            WriteChampion();

            SaveData(currentPlayerProfile.Name + ".xml", currentPlayerProfile);
            SaveData(enemyProfile.Name + ".xml", enemyProfile);
        }
        private void ManageProfiles(PlayerProfile profile1, PlayerProfile profile2)
        {
            currentPlayerProfile = profile1;
            enemyProfile = profile2;

            enemyProfile.AddGame();
            currentPlayerProfile.AddGame();
        }
        private void SaveData(string profileName, PlayerProfile profile)
        {
            XMLManager.SerializeXML(profile, profileName);
        }
        private void AddWin()
        {
            switch (lastWinner)
            {
                case PlayerTurn.Player1:
                    player1Wins++;
                    currentPlayerProfile.AddWonRound();
                    enemyProfile.AddLostRound();
                    break;
                case PlayerTurn.Player2:
                    currentPlayerProfile.AddLostRound();
                    enemyProfile.AddWonRound();
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
                enemyProfile.AddLostGame();
            }
            else if (player2Wins > player1Wins)
            {
                champion = enemyProfile.Name;

                currentPlayerProfile.AddLostGame();
                enemyProfile.AddWonGames();
            }

            Console.Clear();
            Console.WriteLine($"{champion} is absolute champion!");
        }
        private bool SomeoneHas3Wins()
        {
            return player1Wins >= winsCount || player2Wins >= winsCount;
        }
    }
}
