
using System;

namespace SeaBattle
{
    class Map
    {
        private static Random rand = new Random();

        public static void GenerateBlankField(Cell[,] field)
        {
            for (int i = 0; i < Tools.fieldSide; i++)
            {
                for (int j = 0; j < Tools.fieldSide; j++)
                {
                    field[j, i] = new Cell(' ');
                }
            }
        }
        public static Cell[,] GenerateField(Cell[,] field)
        {
            for (int i = 0; i < 20; i++)
            {
                (int, int) randomPoint = new(rand.Next(0, Tools.fieldSide), rand.Next(0, Tools.fieldSide));

                while (field[randomPoint.Item1, randomPoint.Item2].IsShip)
                {
                    randomPoint = new(rand.Next(0, Tools.fieldSide), rand.Next(0, Tools.fieldSide));
                }

                field[randomPoint.Item1, randomPoint.Item2].CreateShip();
            }

            return field;
        }
        public static void DrawField(Cell[,] field)
        {
            Console.WriteLine(" 0123456789");
            for (int i = 0; i < Tools.fieldSide; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(i);

                for (int j = 0; j < Tools.fieldSide; j++)
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

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============");
        }
        public static void Redraw(Player player1, Player player2)
        {
            Console.Clear();

            DrawField(player1.Cells);
            DrawField(player1.VisibleCells);
        }

    }
}
