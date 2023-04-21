
using System;

namespace SeaBattle
{
    public class ClientGame
    {
        private Player myPlayer;

        public void BindPlayer(Player player)
        {
            myPlayer = player;
        }
        public void DrawMyMap()
        {
            myPlayer.Cells.DrawField();
            myPlayer.VisibleCells.DrawField();
        }
        public void DoHumanPlayerTurn()
        {
            myPlayer.DoTurn();
        }
    }
}
