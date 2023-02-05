using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex05.GameLogic
{
    public class Board
    {
        private eCell[,] m_GameBoard;

        public Board(int i_BoardSize)
        {
            m_GameBoard = initBoard(i_BoardSize);
        }

        private eCell[,] initBoard(int i_BoardSize)
        {
            eCell[,] resultBoard = new eCell[i_BoardSize, i_BoardSize];
            
            for (int row = 0; row < i_BoardSize; row++)
            {
                for (int col = 0; col < i_BoardSize; col++)
                {
                    if ((row == (i_BoardSize / 2) - 1 || row == i_BoardSize / 2) && row == col)
                    {
                        resultBoard[row, col] = eCell.White;
                    }
                    else if ((row == (i_BoardSize / 2) - 1 && col == i_BoardSize / 2) || (row == i_BoardSize / 2 && col == (i_BoardSize / 2) - 1))
                    {
                        resultBoard[row, col] = eCell.Black;
                    }
                    else
                    {
                        resultBoard[row, col] = eCell.Empty;
                    }
                }
            }
            return resultBoard;
        }
        public eCell[,] GameBoard 
        {
            get { return m_GameBoard; }
        }
    }
}
