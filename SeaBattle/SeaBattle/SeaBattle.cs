using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SeaBattle
{
    
    enum PlayerTurn
    { 
        Player1,
        Player2,
    }
    public class SeaBattle
    {
        ServerObject server;

        private Player player1;
        private Player player2;
        private Player currentPlayer;

        private PlayerTurn turn = PlayerTurn.Player1;

        private GameType gameType;

        private bool isGameStarted = true;
        private bool isFinishedRound = false;

        private string winner = " ";

        public void Start(bool player1IsAI, bool player2IsAI, GameType type)
        {
            Init(player1IsAI, player2IsAI, type);

            GameCycle();
        }
        private void GameCycle()
        {
            while (isGameStarted || player1.winsCount >= 3 || player2.winsCount >= 3)
            {
                Update();
            }
        }
        private void Update()
        {
            while(!isFinishedRound) DoGameStep();

            WriteWinner();

            ResetLevel();
        }
        private void DoGameStep()
        {
            if (gameType is GameType.Singleplayer) Redraw();
            else if (gameType is GameType.Multiplayer)
            {
                //server.clients[0].clientGame.DrawMyMap();
                //server.clients[1].clientGame.DrawMyMap();
            }

            WriteScore();

            PickPlayer();

            if(gameType is GameType.Singleplayer) DoPlayerTurn();
            else if(gameType is GameType.Multiplayer)
            {
                //server.clients[(int)turn].clientGame.DoHumanPlayerTurn();
            }

            TryFinishGame();

            ChangeTurn();

            if (gameType is GameType.Singleplayer) Redraw();
            else if (gameType is GameType.Multiplayer)
            {
                //server.clients[0].clientGame.DrawMyMap();
                //server.clients[1].clientGame.DrawMyMap();
            }

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
            if(player1.Score >= 20 || player2.Score >= 20)
            {
                EndGame();
            }
        }
        private void EndGame()
        {
            isFinishedRound = true;

            winner = player1.Score > player2.Score ? "Player 1 won!" : "Player 2 won!";
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
        private async void Init(bool p1AI, bool p2AI, GameType type)
        {
            gameType = type;

            player1 = new Player(p1AI, 1);
            player2 = new Player(p2AI, 2);

            player1.SetEnemy(player2);
            player2.SetEnemy(player1);

            SetLevel();

            if (gameType is GameType.Multiplayer)
            {
                server = new ServerObject();
                await server.ListenAsync();
            }
        }
        private void SetLevel()
        {
            player1.Cells.InitMap();
            player2.Cells.InitMap();
            player1.VisibleCells.GenerateBlankField();

            player1.ResetScore();
            player2.ResetScore();
        }
        private void Redraw()
        {
            Console.Clear();

            if(player1.IsAI && player2.IsAI)
            {
                player1.Cells.DrawField();
                player2.Cells.DrawField();
            }
            else if (!player1.IsAI && player2.IsAI)
            {
                player1.Cells.DrawField();
                player1.VisibleCells.DrawField();
            }
        }
    }
}
