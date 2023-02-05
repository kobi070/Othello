using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex05.GameLogic
{
    public struct Coordinates
    {
        private int m_X;
        private int m_Y;

        public Coordinates(int i_X, int i_Y)
        {
            m_X = i_X;
            m_Y = i_Y;
        }
        public int X { get { return m_X; } }
        public int Y { get { return m_Y; } }

        public static Coordinates operator +(Coordinates i_FirstCoordinate, Coordinates i_SecondCordinate)
        {
            return new Coordinates(i_FirstCoordinate.X + i_SecondCordinate.X, i_FirstCoordinate.Y + i_SecondCordinate.Y);
        }
    }
    public enum eCell
    {
        Black,
        White,
        Empty
    }
    

    public enum eColors
    { 
        Black,
        White
    }
}
