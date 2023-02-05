using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex05.GameLogic
{
    public class GameStatus
    {
        // This class should hold our date of a game => Game board, Players, and any data associted with the game
        // Or during a game (live)

        private Player m_CurrentPlayer; // keeps a pointer to the current player => would like to use a diffrent method like a "listner" and "norifyier"
        private Player m_FirstPlayer;
        private Player m_SecondPlayer;
        private Board m_GameBoard;
        private bool m_AgainstComputer;
        private Coordinates? m_LastMove;
        private string m_NameOfPlayerMadeLastMove;

        public GameStatus(int i_BoardSize, bool i_AgainsComputer)
        {
            // Creation of two player's Black and White
            m_FirstPlayer = new Player("Black", eColors.Black, new Coordinates((i_BoardSize / 2) - 1, i_BoardSize / 2), new Coordinates(i_BoardSize / 2, (i_BoardSize / 2) - 1));
            m_SecondPlayer = new Player("White", eColors.White, new Coordinates((i_BoardSize / 2) - 1, (i_BoardSize / 2) - 1), new Coordinates(i_BoardSize / 2, i_BoardSize / 2)); ;

            // Current Randomley choosing the first to play
            m_CurrentPlayer = m_SecondPlayer;

            // User choice to play against an actuall player or the computer
            m_AgainstComputer = i_AgainsComputer;

            // Using Board class we creaed to init the new Board game
            m_GameBoard = new Board(i_BoardSize);
        }
        private Player getRandomPlayer()
        {
            Player randomPlayer = null;
            Random random = new Random();
            int randomValue = random.Next() % 2;

            if (randomValue == (int)eColors.Black)
            {
                randomPlayer = m_FirstPlayer;
            }
            else if (randomValue == (int)eColors.White)
            {
                randomPlayer = m_SecondPlayer;
            }
            
            return randomPlayer;
        }

        public int BoardSize
        {
            get
            {
                return (int)Math.Sqrt(CurrentBoard.GameBoard.Length);
            }
        }
        public Board CurrentBoard
        {
            get { return m_GameBoard; }
        }

        public Coordinates? LastMove
        {
            get { return m_LastMove; }
            set { m_LastMove = value; }
        }

        public string NameOfPlayerMadeLastMove
        {
            get { return m_NameOfPlayerMadeLastMove;}
            set { m_NameOfPlayerMadeLastMove = value; }
        }

        public bool AgainstComputer
        {
            get { return m_AgainstComputer;}
        }

        public Player CurrentPlayer
        {
            get { return m_CurrentPlayer;}
            set { m_CurrentPlayer = value; }
        }

        public Player FisrtPlayer
        {
            get { return m_FirstPlayer; }
            set { m_FirstPlayer = value; }
        }

        public Player SecondPlayer
        {
            get { return m_SecondPlayer;}
            set { m_SecondPlayer = value; }
        }

        public bool GameOver()
        {
            bool isGameOver = true;

            if (m_FirstPlayer.HasValidMoves() || m_SecondPlayer.HasValidMoves())
            {
                isGameOver = false;
            }
            
            return isGameOver;
        }

        public void SwitchTurns()
        {
            if (m_CurrentPlayer.Name == m_FirstPlayer.Name)
            {
                m_CurrentPlayer = m_SecondPlayer;
            }
            else
            {
                m_CurrentPlayer = m_FirstPlayer;
            }
        }

        public Player GetLeadingPlayer()
        {
            Player leadingPlayer = null;
            
            if (m_FirstPlayer.Score > m_SecondPlayer.Score)
            {
                leadingPlayer = m_FirstPlayer;
            }
            else
            {
                leadingPlayer = m_SecondPlayer;
            }

            return leadingPlayer;
        }

        public void RestartGame()
        {
            int sizeOfBoard = BoardSize;
            m_GameBoard = new Board(sizeOfBoard);

            m_FirstPlayer.Restart(new Coordinates((sizeOfBoard / 2) - 1, (sizeOfBoard / 2) - 1),
                new Coordinates((sizeOfBoard / 2), (sizeOfBoard / 2)));
            m_SecondPlayer.Restart(new Coordinates((sizeOfBoard / 2) - 1, (sizeOfBoard / 2)),
                new Coordinates((sizeOfBoard / 2), (sizeOfBoard / 2) - 1));

            m_CurrentPlayer = getRandomPlayer();

            m_LastMove = null;
        }


    }
}
