using System;
using System.Collections.Generic;


namespace Ex05.GameLogic
{
    public class Player
    {
        private readonly string r_PlayerName;
        private eColors m_PlayerColor;
        private List<Coordinates> m_ValidMoves = new List<Coordinates>();
        private List<Coordinates> m_PlayerCells = new List<Coordinates>();
        private int m_PlayerScore;
        private Random m_Rand;
        private int m_GamesWon;

        public Player(string i_Name, eColors i_PlayerColor, Coordinates i_FirstCoinPosition, Coordinates i_SecondCoinPosition)
        {
            r_PlayerName = i_Name;
            m_PlayerColor = i_PlayerColor;
            m_PlayerCells.Add(i_FirstCoinPosition);
            m_PlayerCells.Add(i_SecondCoinPosition);
            m_PlayerScore = 0;
            m_GamesWon = 0;

            m_Rand = new Random();
        }
        public eColors PlayerColor
        {
            get { return m_PlayerColor; }
            set { m_PlayerColor = value; }
        }
        public string Name
        {
            get { return r_PlayerName; }
        }
        public int Score
        {
            get { return m_PlayerScore; }
            set { m_PlayerScore = value; }
        }
        public Random Rand
        {
            get { return m_Rand; }
            set { m_Rand = value; }
        }
        public List<Coordinates> PlayerCells
        {
            get { return m_PlayerCells; }
            set { m_PlayerCells = value; }
        }

        public List<Coordinates> ValidMoves
        {
            get { return m_ValidMoves; }
            set { m_ValidMoves = value; }
        }
        public int GamesWon 
        {
            get { return m_GamesWon;}
            set { m_GamesWon = value; }
        }

        public bool HasValidMoves()
        {
            bool hasValidMoves = m_ValidMoves.Count != 0;
            
            return hasValidMoves;
        }
        public void Restart(Coordinates i_FirstCoinPositions, Coordinates i_SecondCoinPositions)
        {
            m_ValidMoves.Clear();
            m_PlayerCells.Clear();

            m_PlayerCells.Add(i_FirstCoinPositions);
            m_PlayerCells.Add(i_SecondCoinPositions);
        }

        public Coordinates MakeMove()
        {
            int randomNumber = m_Rand.Next() % m_ValidMoves.Count;

            return m_ValidMoves[randomNumber];
        }
    }
}
