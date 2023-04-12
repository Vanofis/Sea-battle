using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace SeaBattle
{
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
            Redraw();

            do
            {
                CordsInput();

                PlayerShoot();

                Redraw();

                Thread.Sleep(1000);
            }
            while (isPlayerStreak);

            do
            {
                EnemyShoot();

                Redraw();

                Thread.Sleep(1000);
            }
            while(isEnemyStreak);

            enemyMemberPosition = (null, null);
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

            if (IsValidCordinate(number - 1)) return number - 1;

            ThrowWrongInputException();

            return null;
        }
        static void ThrowWrongInputException()
        {
            Console.WriteLine("Wrong input you dumb fuck");
        }
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

                    ConsoleColor color = field[i, j].Character switch
                    {
                        Cell.destroyedChar => ConsoleColor.Red,
                        Cell.destroyedShipChar => ConsoleColor.Blue,
                        Cell.shipChar => ConsoleColor.DarkYellow,
                        _ => ConsoleColor.White,
                    };

                    Console.ForegroundColor = color;

                    Console.Write(field[i, j].Character);
                }

                Console.WriteLine();
            }

            Console.ForegroundColor= ConsoleColor.White;
            Console.WriteLine("===============");
        }
        static void Redraw()
        {
            Console.Clear();

            DrawField(playerCells);
            DrawField(playerVisibleCells);
        }
        #endregion

        #region AI
        static void EnemyShoot()
        {
            if (!isEnemyStreak) ShootRandomPoint();
            else GetNearPoints();
        }
        static void ShootRandomPoint()
        {
            int x = rand.Next(0, fieldSide);
            int y = rand.Next(0, fieldSide);

            while (playerCells[x, y].IsBombed)
            {
                x = rand.Next(0, fieldSide);
                y = rand.Next(0, fieldSide);
            }

            CheckIfPlayerShip(x, y); 

            playerCells[x, y].DestroyCell();
        }
        static void GetNearPoints()
        {
            (int, int)[] points = GetAvailablePoints();

            List<(int, int)> validPoints = ValidatePoints(points);

            if (validPoints.Count is 0) ShootRandomPoint();
            else ShootNearPoints(validPoints);
        }
        static void ShootNearPoints(List<(int, int)> validPoints)
        {
            int randomNearX = validPoints[rand.Next(0, validPoints.Count)].Item1;
            int randomNearY = validPoints[rand.Next(0, validPoints.Count)].Item2;

            CheckIfPlayerShip(randomNearX, randomNearY);

            playerCells[randomNearX, randomNearY].DestroyCell();


        }
        static List<(int, int)> ValidatePoints((int, int)[] points)
        {
            List<(int, int)> validPoints = new List<(int, int)>();

            foreach (var point in points)
            {
                if (IsValidPoint(point) && !IsBombedPosition(point)) validPoints.Add(point);
            }

            return validPoints;
        }
        static void CheckIfPlayerShip(int x, int y)
        {
            if (playerCells[x, y].IsShip) MemberPosition(x, y);
            else isEnemyStreak = false;
        }
        static bool IsBombedPosition((int, int) point)
        {
            if(!IsValidPoint(point)) return true;

            return playerCells[point.Item1, point.Item2].IsBombed;
        }
        static void MemberPosition(int x, int y)
        {
            isEnemyStreak = true;

            enemyMemberPosition = (x, y);
        }
        static (int, int)[] GetAvailablePoints()
        {
            int startX = (int)enemyMemberPosition.Item1;
            int startY = (int)enemyMemberPosition.Item2;

            (int, int)[] points = new (int, int)[]
            {
                (startX, startY + 1),
                (startX, startY - 1),
                (startX + 1, startY),
                (startX - 1, startY),
            };

            return points;
        }
        #endregion
        static bool IsValidCordinate(int cord)
        {
            return cord >= 0 && cord < fieldSide;
        }
        static bool IsValidPoint((int, int) point)
        {
            bool xIsValid = point.Item1 >= 0 && point.Item1 < fieldSide;
            bool yIsValid = point.Item2 >= 0 && point.Item2 < fieldSide;

            return xIsValid && yIsValid;
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
