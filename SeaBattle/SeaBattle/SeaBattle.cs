using System;
using System.Threading;

namespace SeaBattle
{
    
    public enum PlayerTurn
    { 
        Player1,
        Player2,
        Draw,
    }
    public class SeaBattle
    {
        public PlayerTurn winnerPlayer = PlayerTurn.Draw;

        private Player player1;
        private Player player2;
        private Player currentPlayer;

        private PlayerTurn turn = PlayerTurn.Player1;

        private bool isFinishedRound = false;

        private string winner = " ";

        public void Start(bool player1IsAI, bool player2IsAI)
        {
            Init(player1IsAI, player2IsAI);

            StartGameCycle();
        }
        public PlayerTurn GetResult()
        {
            return winnerPlayer;
        }
        private void StartGameCycle()
        {
            while(!isFinishedRound) DoGameStep();

            WriteWinner();

            ResetLevel();
        }
        private void DoGameStep()
        {
            Redraw();

            WriteScore();

            PickPlayer();

            DoPlayerTurn();

            TryFinishGame();

            ChangeTurn();

            Redraw();

            Thread.Sleep(750);
        }
        private void PickPlayer()
        {
            switch(turn)
            { 
                case PlayerTurn.Player1:
                    currentPlayer = player1;
                    break;
                case PlayerTurn.Player2:
                    currentPlayer = player2;
                    break;
            }
        }
        private void DoPlayerTurn()
        {
            currentPlayer.DoTurn();
        }
        private void ChangeTurn()
        {
            if(turn is PlayerTurn.Player1 && !player1.IsStreak)
            {
                turn = PlayerTurn.Player2;
            }
            else if (turn is PlayerTurn.Player2 && !player2.IsStreak)
            {
                turn = PlayerTurn.Player1;
            }
        }
        private void WriteScore()
        {
            Console.WriteLine($"Your score {player1.Score}");
            Console.WriteLine($"Enemy score {player2.Score}");
        }
        private void TryFinishGame()
        {
            if(player1.Score >= Tools.shipCount || player2.Score >= Tools.shipCount)
            {
                EndGame();
            }
        }
        private void EndGame()
        {
            isFinishedRound = true;

            if(player1.Score > player2.Score)
            {
                winner = "Player 1 won!";
                winnerPlayer = PlayerTurn.Player1;
            }
            else
            {
                winner = "Player 2 won!";
                winnerPlayer = PlayerTurn.Player2;
            }
        }
        private void WriteWinner()
        {
            Console.Clear();

            Console.WriteLine(winner);

            Console.ReadKey();
        }
        private void ResetLevel()
        {
            isFinishedRound = false;

            winner = " ";

            SetLevel();
        }
        private void Init(bool p1AI, bool p2AI)
        {
            turn = PlayerTurn.Player1;

            player1 = new Player(p1AI, 1);
            player2 = new Player(p2AI, 2);

            currentPlayer = player1;

            player1.SetEnemy(player2);
            player2.SetEnemy(player1);

            SetLevel();
        }
        private void SetLevel()
        {
            player1.Cells.InitMap();
            player2.Cells.InitMap();

            player1.VisibleCells.GenerateBlankField();
            player2.VisibleCells.GenerateBlankField();

            player1.ResetScore();
            player2.ResetScore();
        }
        private void Redraw()
        {
            Console.Clear();

            if (player1.IsAI && player2.IsAI)
            {
                player1.Cells.DrawField();
                player2.Cells.DrawField();
            }
            else if (!player1.IsAI && player2.IsAI)
            {
                player1.Cells.DrawField();
                player1.VisibleCells.DrawField();
            }
            else if (!player1.IsAI && !player2.IsAI)
            {
                currentPlayer.Cells.DrawField();
                currentPlayer.VisibleCells.DrawField();
            }
        }
    }
}
