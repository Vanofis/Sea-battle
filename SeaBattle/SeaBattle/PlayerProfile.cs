using System;

namespace SeaBattle
{
    [Serializable]
    public class PlayerProfile
    {
        public string Name = "BasePlayer";

        public bool IsAi = false;

        //Games
        public int Wins = 0;
        public int Loses = 0;
        public int Games = 0;

        //Rounds
        public int LostRounds = 0;
        public int WonRounds = 0;

        public PlayerProfile() { }
        public PlayerProfile(string name, bool isAI) 
        { 
            Name = name;
            IsAi = isAI;
        }

        #region Control Methods
        public void AddWonRound()
        {
            WonRounds++;
        }
        public void AddLostRound()
        {
            LostRounds++;
        }
        public void AddGame()
        {
            Games++;
        }
        public void AddLostGame() 
        { 
            Loses++;
        }
        public void AddWonGames()
        {
            Wins++;
        }
        #endregion
    }
}
