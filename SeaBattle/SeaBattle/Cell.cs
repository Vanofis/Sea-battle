
namespace SeaBattle
{
    public class Cell
    {
        public char Character { get; private set; } = ' ';

        public bool IsShip { get; private set; } = false;

        public bool IsBombed { get; private set; } = false;

        public const char shipChar = '$';
        public const char destroyedChar = 'o';
        public const char destroyedShipChar = 'X';

        public Cell(char character)
        {
            Character = character;
        }
        public void CreateShip()
        {
            IsShip = true;

            Character = shipChar;
        }
        public void DestroyCell()
        {
            if (IsShip)
            {
                Character = destroyedShipChar;
                
                IsShip = false;
            }
            else
            {
                Character = destroyedChar;
            }

            IsBombed = true;
        }
        public void RevealShip() // This method is for player visible cells ONLY
        {
            IsShip = true;
        }
    }
}
