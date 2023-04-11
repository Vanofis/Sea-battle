using System;

namespace SeaBattle
{
    public enum Direction
    { 
        Horizontal,
        Vertical,
    }

    public class SeaBattle
    {
        private static Random rand = new Random();

        private const int fieldSide = 10;

        private static Cell[,] playerCells = new Cell[fieldSide, fieldSide];
        private static Cell[,] playerVisibleCells = new Cell[fieldSide, fieldSide];
        
        private static Cell[,] enemyCells = new Cell[fieldSide, fieldSide];

        private static (int?, int?) lastShootCords;
        private static (int?, int?) enemyMemberPosition = (null, null);

        private static bool isPlayerStreak = false;
        private static bool isEnemyStreak = false;

        static void Main(string[] args)
        {
            Init();

            while (true) 
            {
                DoGameStep();
            }
        }
        static void DoGameStep()
        {
            DrawField(playerCells);
            DrawField(playerVisibleCells);

            do
            {
                CordsInput();

                PlayerShoot();
            }
            while (isPlayerStreak);

            do
            {
                EnemyShoot();
            }
            while(isEnemyStreak);

            enemyMemberPosition = (null, null);

            Console.Clear();
        }
        #region Input
        static void CordsInput()
        {
            Console.WriteLine("Write your X shoot coordinates");
            lastShootCords.Item2 = GetInput();

            Console.WriteLine("Write your Y shoot coordinates");

            lastShootCords.Item1 = GetInput();
        }
        static int? GetInput()
        {
            string input = Console.ReadLine();

            int number;

            int.TryParse(input, out number);

            if (IsValidPoint(number - 1)) return number - 1;

            ThrowWrongInputException();

            return null;
        }
        static void ThrowWrongInputException()
        {
            Console.WriteLine("Wrong input you dumb fuck");
        }
        #endregion

        #region Map
        static void GenerateBlankField(Cell[,] field)
        {
            for (int i = 0; i < fieldSide; i++)
            {
                for (int j = 0; j < fieldSide; j++)
                {
                    field[j, i] = new Cell(' ');
                }
            }
        }
        static Cell[,] GenerateField(Cell[,] field)
        {
            for (int i = 0; i < 20; i++)
            {
                (int, int) randomPoint = new(rand.Next(0, fieldSide), rand.Next(0, fieldSide));

                while (field[randomPoint.Item1, randomPoint.Item2].IsShip)
                {
                    randomPoint = new(rand.Next(0, fieldSide), rand.Next(0, fieldSide));
                }

                field[randomPoint.Item1, randomPoint.Item2].CreateShip();
            }

            return field;
        }
        static void DrawField(Cell[,] field)
        {
            for (int i = 0; i < fieldSide; i++)
            {
                for (int j = 0; j < fieldSide; j++)
                {
                    Console.Write(field[i, j].Character);
                }

                Console.WriteLine();
            }

            Console.WriteLine("===============");
        }
        #endregion

        #region AI

        #endregion
        static void PlayerShoot()
        {
            if (lastShootCords.Item1 is null || lastShootCords.Item2 is null) return;

            int x = (int)lastShootCords.Item1;
            int y = (int)lastShootCords.Item2;

            if (enemyCells[x, y].IsBombed) return;

            if (enemyCells[x, y].IsShip) playerVisibleCells[x, y].RevealShip();

            isPlayerStreak = enemyCells[x, y].IsShip;

            enemyCells[x, y].DestroyCell();
            playerVisibleCells[x, y].DestroyCell();
        }
        static void EnemyShoot()
        {
            if(!isEnemyStreak) ShootRandomPoint();
        }
        static void ShootRandomPoint()
        {
            int x = rand.Next(0, fieldSide);
            int y = rand.Next(0, fieldSide);

            if (playerCells[x, y].IsShip) MemberPosition(x, y);

            playerCells[x, y].DestroyCell();
        }
        static void ShootNearPoints()
        {

            
        }
        static void MemberPosition(int x, int y)
        {
            isEnemyStreak = true;

            enemyMemberPosition = (x, y);
        }
        static bool IsValidPoint(int cord)
        {
            return cord >= 0 && cord < fieldSide;
        }
        static void Init()
        {
            GenerateBlankField(enemyCells);
            GenerateBlankField(playerCells);
            GenerateBlankField(playerVisibleCells);

            GenerateField(enemyCells);
            GenerateField(playerCells);
        }
    }
}
