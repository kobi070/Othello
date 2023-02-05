using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex05.GameLogic
{
    public class GameLogic
    {
        private GameStatus m_CurrentGameStatus;
        private Coordinates m_UpLeft = new Coordinates(-1, -1);
        private Coordinates m_Up = new Coordinates(0, -1);
        private Coordinates m_UpRight = new Coordinates(1, -1);
        private Coordinates m_Left = new Coordinates(-1, 0);
        private Coordinates m_Right = new Coordinates(1, 0);
        private Coordinates m_DownLeft = new Coordinates(-1, 1);
        private Coordinates m_Down = new Coordinates(0, 1);
        private Coordinates m_DownRight = new Coordinates(1, 1);

        private Coordinates[] m_Directions;

        public GameLogic(GameStatus i_GameStatus)
        {
            m_CurrentGameStatus = i_GameStatus;


            m_Directions = new Coordinates[8] { m_UpLeft, m_Up, m_UpRight, m_Left, m_Right, m_DownLeft, m_Down, m_DownRight };
        }

        public void UpdateGame(Coordinates i_Move)
        {
            // Sets the cells by the rigth colors
            m_CurrentGameStatus.CurrentBoard.GameBoard[i_Move.X, i_Move.Y] = (eCell)m_CurrentGameStatus.CurrentPlayer.PlayerColor;
            m_CurrentGameStatus.CurrentPlayer.PlayerCells.Add(i_Move);

            // Check every cell in evrey direction
            foreach (Coordinates direction in m_Directions)
            {
                // Add possible directions to user choice of move
                Coordinates moveToDirection = i_Move + direction;

                // Check our possible moves inside the board 
                if (isInBoardLimits(moveToDirection, m_CurrentGameStatus.BoardSize))
                {
                    // Get the cell we want to move to
                    eCell cellToMove = m_CurrentGameStatus.CurrentBoard.GameBoard[moveToDirection.X, moveToDirection.Y];

                    // Validate if the cell is not empty and the current cell is not the same color as our current player color
                    // Therfore the move is valid.
                    if (!cellToMove.Equals(eCell.Empty) && !cellToMove.Equals((eCell)m_CurrentGameStatus.CurrentPlayer.PlayerColor))
                    {
                        // Check if we could flip directions
                        Coordinates? coordinatesToCheck = CheckIfPossibleToFlip(moveToDirection, direction, (eCell)m_CurrentGameStatus.CurrentPlayer.PlayerColor);

                        // If coordinatesToCheck == null => There are no coordinates we can flip
                        if (coordinatesToCheck != null)
                        {
                            // Flip the coin
                            Flip(moveToDirection, direction, cellToMove);
                        }
                    }
                }
            }

            m_CurrentGameStatus.LastMove = i_Move;
            m_CurrentGameStatus.NameOfPlayerMadeLastMove = m_CurrentGameStatus.CurrentPlayer.Name;
        }
        
        public void Flip(Coordinates i_Move, Coordinates i_Dirction, eCell i_CellType)
        {
            eCell currentCell = m_CurrentGameStatus.CurrentBoard.GameBoard[i_Move.X, i_Move.Y];
            Coordinates coordinatesToMove = i_Move + i_Dirction;
            eCell possibleNewCurrentCell = m_CurrentGameStatus.CurrentBoard.GameBoard[coordinatesToMove.X, coordinatesToMove.Y];

            // if our possible new cell equals our current cell =>  send the Flip function recursivley with the next cell possible
            // else flip all the given coins
            if (possibleNewCurrentCell.Equals(currentCell))
            {
                Flip(coordinatesToMove, i_Dirction, possibleNewCurrentCell);
            }

            // Updateing the players coins
            if (currentCell.Equals(eCell.Black))
            {
                m_CurrentGameStatus.CurrentBoard.GameBoard[i_Move.X, i_Move.Y] = eCell.White;
            }
            else
            {
                m_CurrentGameStatus.CurrentBoard.GameBoard[i_Move.X, i_Move.Y] = eCell.Black;
            }
            
            if (m_CurrentGameStatus.CurrentPlayer.Equals(m_CurrentGameStatus.FisrtPlayer))
            {
                m_CurrentGameStatus.FisrtPlayer.ValidMoves.Remove(i_Move);
            }
            else
            {
                m_CurrentGameStatus.SecondPlayer.ValidMoves.Remove(i_Move);
            }

            m_CurrentGameStatus.CurrentPlayer.ValidMoves.Add(i_Move);
        }

        public void CalcValidMoves(Player i_Player)
        {
            // clearing all the moves of player 
            i_Player.ValidMoves.Clear();

            Coordinates? tempCoordToCheckForValidMovePossibility = null;

            // runs on all the cells occupied by player and cheks to see if from them we can find a cell to 
            // put a coin on, that'll flip all the coins on the way
            foreach (Coordinates cellOccupied in i_Player.PlayerCells)
            {
                eCell currentBoardCell = m_CurrentGameStatus.CurrentBoard.GameBoard[cellOccupied.X, cellOccupied.Y];

                // for each cell occupied, checks all 8 directions to find places to put coins
                foreach (Coordinates direction in m_Directions)
                {
                    Coordinates adjacentCoordinate = cellOccupied + direction;

                    // checking that new coordinate is within the boinds of the board
                    if (isInBoardLimits(adjacentCoordinate, m_CurrentGameStatus.BoardSize))
                    {
                        eCell adjacentCell = m_CurrentGameStatus.CurrentBoard.GameBoard[adjacentCoordinate.X, adjacentCoordinate.Y];

                        // checking if there is a valid move at the end of this vector, starting from current coord going in direction
                        if (!currentBoardCell.Equals(adjacentCell) && !adjacentCell.Equals(eCell.Empty))
                        {
                            tempCoordToCheckForValidMovePossibility = CheckIfPossibleToFlip(adjacentCoordinate, direction, eCell.Empty);
                        }

                        // the matrix coord isnt null - we found a valid move. adding the cooridante of that valid move to our valid moves list
                        if (tempCoordToCheckForValidMovePossibility != null)
                        {
                            if (!i_Player.ValidMoves.Contains((Coordinates)tempCoordToCheckForValidMovePossibility))
                            {
                                i_Player.ValidMoves.Add((Coordinates)tempCoordToCheckForValidMovePossibility);
                            }
                        }
                    }

                }
            }
        }
        
        public void CalcScore()
        {
            m_CurrentGameStatus.FisrtPlayer.Score = 0;
            m_CurrentGameStatus.SecondPlayer.Score = 0;

            foreach (eCell cell in m_CurrentGameStatus.CurrentBoard.GameBoard)
            {
                if (cell.Equals(eCell.White))
                {
                    m_CurrentGameStatus.SecondPlayer.Score++;
                }
                else if (cell.Equals(eCell.Black))
                {
                    m_CurrentGameStatus.FisrtPlayer.Score++;
                }
            }
        }
        
        private bool isInBoardLimits(Coordinates i_MoveToDirection, int i_BoardSize)
        {
            bool isInBoardLimit = true;

            if (i_MoveToDirection.X >= i_BoardSize || i_MoveToDirection.Y < 0 || i_MoveToDirection.Y >= i_BoardSize || i_MoveToDirection.X < 0)
            {
                isInBoardLimit = false;
            }

            return isInBoardLimit;
        }
        
        public Coordinates? CheckIfPossibleToFlip(Coordinates i_MoveToDirection, Coordinates i_Direcions, eCell i_CellType)
        {
            // Check new direction
            Coordinates? flipCoinDirection = null;
            Coordinates joinedCoordinates =  i_MoveToDirection + i_Direcions;

            if (isInBoardLimits(joinedCoordinates, m_CurrentGameStatus.BoardSize))
            {
                eCell cellToUse = m_CurrentGameStatus.CurrentBoard.GameBoard[joinedCoordinates.X, joinedCoordinates.Y];

                // If cellToUse is the same type as i_CellType => return the joinedCordinates
                if (cellToUse.Equals(i_CellType))
                {
                    flipCoinDirection = joinedCoordinates;
                }
                // In case its not equal return null
                else if (!cellToUse.Equals(m_CurrentGameStatus.CurrentBoard.GameBoard[joinedCoordinates.X, joinedCoordinates.Y]))
                {
                    flipCoinDirection = null;
                }
                // Recursivley call the function to check other options
                else
                {
                    CheckIfPossibleToFlip(joinedCoordinates, i_Direcions, i_CellType);
                }
            }

            return flipCoinDirection;
        }
    }
}
