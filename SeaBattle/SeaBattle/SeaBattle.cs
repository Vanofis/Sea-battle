using System;
using System.Threading;

namespace SeaBattle
{
    public class SeaBattle
    {
        private Player player1;
        private Player player2;        

        private bool isGameStarted = true;
        private bool isFinishedRound = false;

        public void Start()
        {
            Init();

            GameCycle();
        }
        private void GameCycle()
        {
            while (isGameStarted)
            {
                Update();
            }
        }
        private void Update()
        {
            while(!isFinishedRound) DoGameStep();
        }
        private void DoGameStep()
        {
            Map.Redraw(player1, player2);

            do
            {
                player1.DoTurn();

                Map.Redraw(player1, player2);

                Thread.Sleep(1000);
            }
            while (player1.IsStreak);

            do
            {
                player2.DoTurn();

                Map.Redraw(player1, player2);

                Thread.Sleep(1000);
            }
            while(player2.IsStreak);

            if (player2.IsAI) player2.ResetMemory();
        }

        #region Map
        #endregion

        private void Init()
        {
            player1 = new Player(false);
            player2 = new Player(true);

            player1.SetEnemy(player2);
            player2.SetEnemy(player1);

            Map.GenerateBlankField(player2.Cells);
            Map.GenerateBlankField(player1.Cells);
            Map.GenerateBlankField(player1.VisibleCells);

            Map.GenerateField(player2.Cells);
            Map.GenerateField(player1.Cells);
        }
    }
}
