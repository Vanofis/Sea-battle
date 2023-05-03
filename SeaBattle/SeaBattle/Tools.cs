
namespace SeaBattle
{
    public static class Tools
    {
        public const int fieldSide = 10;
        public const int shipCount = 20;

        public static bool IsValidCordinate(int cord)
        {
            return cord >= 0 && cord < fieldSide;
        }
        public static bool IsValidPoint((int, int) point)
        {
            bool xIsValid = point.Item1 >= 0 && point.Item1 < fieldSide;
            bool yIsValid = point.Item2 >= 0 && point.Item2 < fieldSide;

            return xIsValid && yIsValid;
        }
    }
}