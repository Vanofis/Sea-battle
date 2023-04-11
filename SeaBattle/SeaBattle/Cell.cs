using System;

namespace SeaBattle
{
    class Cell
    {
        public char Character { get; private set; } = ' ';

        public bool IsShip { get; private set; } = false;

        public bool IsBombed { get; private set; } = false;

        private const char shipChar = '$';
        private const char destroyedChar = 'o';
        private const char destroyedShipChar = 'X';

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
            else Character = destroyedChar;
        }
        public void RevealShip() // This method is for player visible cells ONLY
        {
            IsShip = true;
        }
    }
}
