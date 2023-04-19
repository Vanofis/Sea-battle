using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SeaBattle
{
    enum PlayerTurn
    { 
        Player1,
        Player2,
    }
    public class SeaBattle
    {
        private Player player1;
        private Player player2;
        private Player currentPlayer;
        private PlayerTurn turn = PlayerTurn.Player1;

        private bool isGameStarted = true;
        private bool isFinishedRound = false;

        private string winner = " ";

        public void Start(bool player1IsAI, bool player2IsAI)
        {
            Init(player1IsAI, player2IsAI);

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

            WriteWinner();

            ResetLevel();
        }
        private void DoGameStep()
        {
            Redraw();

            WriteScore();

            PickPlayer();

            DoPlayerTurn();

            ChangeTurn();

            Redraw();
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

            TryFinishGame();
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
        private void Init(bool p1AI, bool p2AI)
        {
            player1 = new Player(p1AI);
            player2 = new Player(p2AI);

            player1.SetEnemy(player2);
            player2.SetEnemy(player1);

            SetLevel();
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
        private async void CreateServer()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 1234);
            using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);
            socket.Listen(1);

            Console.WriteLine($"Your connection adress is: {socket.LocalEndPoint}");

            using Socket client = await socket.AcceptAsync();
        }
    }
}
