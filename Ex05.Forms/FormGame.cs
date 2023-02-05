using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Ex05.GameLogic;

namespace Ex05.Forms
{
    internal class FormGame : Form
    {
        private const string k_Title = "Othello - {0}'s turn";
        private const string k_Othello = "Othello";
        private const string k_ComputerMove = "Computer played {0},{1}";
        private const string k_NoPossibleMoves = "{0} has no moves!";
        private const string k_WinMessage =
@"{0} Won!! ({1}/{2}) ({3}/{4})
Would you like another round?";
        private const string k_TieMessage =
@"It's a tie!! ({0}/{0})
Would you like another round?";
        private const string k_ButtonText = "O";

        private const int k_ButtonSize = 50;
        private const int k_ButtonMargin = 4;
        private const int k_EdgeMargin = 10;

        private readonly int m_BoardSize;
        private ButtonGame[,] m_BoardCells;
        private GameStatus m_CurrentGameState;
        private GameLogic.GameLogic m_GameOperator;

        private int m_NumOfGamesPlayed;

        public FormGame(int i_BoardSize, bool i_AgainstComputer)
        {
            // Dynamically calculate the form size, according to the set button sizes (k_ButtonSize),
            // the margins between the buttons (k_ButtonMargin) and the edge margins (k_EdgeMargin) and
            // of course the size of the board (i_BoardSize)
            int clintSize = ((2 * k_EdgeMargin) + (i_BoardSize * k_ButtonSize) + ((i_BoardSize - 1) * k_ButtonMargin));
            ClientSize = new Size(clintSize, clintSize);

            // Set the form and style => Form.Center, Style.3D
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Fixed3D;


            m_BoardSize = i_BoardSize;
            m_BoardCells = new ButtonGame[m_BoardSize, m_BoardSize];

            // Intiate a GameLogic and a GameStatus objects to run the game logic
            m_CurrentGameState = new GameStatus(i_BoardSize, i_AgainstComputer);
            m_GameOperator = new GameLogic.GameLogic(m_CurrentGameState);

            // Add all the board pieces to the board
            addButtons();

            // Start the game
            startNextGame();

        }

        // this method calculates the possible moves of both player and then starts the
        // actual game loop - runGame. we use it to avoid multiple calculation in cases a player has no moves
        private void startNextGame()
        {
            // Calculate each player's current moves at the begining of every turn
            m_GameOperator.CalcValidMoves(m_CurrentGameState.FisrtPlayer);
            m_GameOperator.CalcValidMoves(m_CurrentGameState.SecondPlayer);

            // Run the game
            runGame();


        }

        // this is the method the runs the "game loop" - according to the current game status decides if the
        // game is over, who's turn is it, and if that player has any moves, and continues the game accordingly
        private void runGame()
        {
            // Update the board evrey turn.
            // happens before any validations occurs
            updateBoard();

            // Check if one of the players has a valid move
            // Yes => Continue game
            // No => Game is over
            if ( m_CurrentGameState.FisrtPlayer.HasValidMoves() || m_CurrentGameState.SecondPlayer.HasValidMoves())
            {
                // In case one of the players has valid moves 
                // We will switch turns
                m_CurrentGameState.SwitchTurns();

                // Check if the current player has valid moves
                // if he does have then chose the move
                // if the computer is playing chose randomly (or base minimax algo)
                if (m_CurrentGameState.CurrentPlayer.HasValidMoves())
                {
                    /// check if the current player now is the oppiste player which allows us to know sevrel things
                    /// if the current player is the second player and also the computer we want the computer to make a random choice
                    /// we will set a new Coordinates and use MakeMove() to detirmine randomly the next move
                    if ((m_CurrentGameState.CurrentPlayer == m_CurrentGameState.SecondPlayer) && m_CurrentGameState.AgainstComputer)
                    {
                        // randomley 
                        Coordinates move = m_CurrentGameState.CurrentPlayer.MakeMove();
                        Console.WriteLine(move);
                        m_GameOperator.UpdateGame(move);

                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        MessageBox.Show(string.Format(k_ComputerMove, move.X + 1, move.Y + 1), k_Othello, buttons);

                        // a move was made, so there is a need to calculate the possible moves of both players
                        startNextGame();
                    }
                    else
                    {
                        // Show the possible moves to current player
                        setPossibleMoves();
                    }
                }
                else
                {
                    MessageBoxButtons buttons = MessageBoxButtons.OK;

                    // if the current player does not have a valid move
                    MessageBox.Show(String.Format(k_NoPossibleMoves, m_CurrentGameState.CurrentPlayer.Name), k_Othello, buttons);
                    runGame();
                }
            }
            else
            {
                // Game is over
                gameOver();
            }
        }

        /// <summary>
        /// Initiale creation of all the board buttons 
        /// adding them to the board matrix and Controls
        /// </summary>
        private void addButtons()
        {

            int rowOffset = k_EdgeMargin;
            int lineOffset;
            for (int row = 0; row < m_BoardSize; row++)
            {
                lineOffset = k_EdgeMargin;
                for (int line = 0; line < m_BoardSize; line++)
                {
                    // Create a new button and set it's coordinates on the board
                    ButtonGame button = new ButtonGame(row, line);

                    // Set it's size according to the desired button size (k_ButtonSize)
                    button.Size = new Size(k_ButtonSize, k_ButtonSize);

                    // Dynamically calculate it's position on the form
                    int rowMargin = k_ButtonMargin * row;
                    int lineMargin = k_ButtonMargin * line;

                    // Set it's location, set its Enabled status to false and add it to the form Controls
                    button.Location = new Point(rowOffset + rowMargin, lineOffset + lineMargin);
                    button.Enabled = false;
                    Controls.Add(button);

                    // Update the line offset for the next button
                    lineOffset += k_ButtonSize;

                    // Finally, add it to the board matrix in the proper position
                    m_BoardCells[row, line] = button;
                }

                // Update the row offset for the next line of buttons
                rowOffset += k_ButtonSize;
            }
        }

        private void updateBoard()
        {
            ButtonGame currButton;

            // goes on the entire game board and set the cells to the relevant color
            for (int i = 0; i < m_BoardSize; i++)
            {
                for (int j = 0; j < m_BoardSize; j++)
                {
                    currButton = m_BoardCells[i, j];

                    // removes the button from the event listener's list as the possible moves were changed
                    currButton.Click -= buttonToChoose_Click;

                    switch (m_CurrentGameState.CurrentBoard.GameBoard[i, j])
                    {
                        case eCell.Black:
                            currButton.BackColor = Color.Black;
                            currButton.Text = k_ButtonText;
                            break;
                        case eCell.White:
                            currButton.BackColor = Color.White;
                            currButton.Text = k_ButtonText;
                            break;
                        case eCell.Empty:
                            currButton.BackColor = default(Color);
                            currButton.Text = string.Empty;
                            break;
                    }

                    currButton.Enabled = false;
                }
            }
        }

        // Method called when there are no possible moves to any player
        // Therfore it handles the MessageBox for asking the user/s if the want another game
        // Also to present the score
        private void gameOver()
        {
            // Calc the scores of the players
            m_GameOperator.CalcScore();

            Player winner = null;
            Player loser = null;

            bool tie = false;
            string messageBoxMessage;
            MessageBoxButtons messageButtons;
            DialogResult dialogResult;

            // setting the winner by score's
            if (m_CurrentGameState.FisrtPlayer.Score > m_CurrentGameState.SecondPlayer.Score)
            {
                winner = m_CurrentGameState.FisrtPlayer;
                loser = m_CurrentGameState.SecondPlayer;
            }
            else if (m_CurrentGameState.FisrtPlayer.Score < m_CurrentGameState.SecondPlayer.Score)
            {
                winner = m_CurrentGameState.SecondPlayer;
                loser = m_CurrentGameState.FisrtPlayer;
            }
            else
            {
                tie = true;
            }

            if (!tie)
            {
                winner.GamesWon++;
                messageBoxMessage = string.Format(k_WinMessage, winner.Name, winner.Score, loser.Score, winner.GamesWon, m_NumOfGamesPlayed);
            }
            else
            {
                messageBoxMessage = string.Format(k_TieMessage, m_CurrentGameState.FisrtPlayer.Score);
            }


            messageButtons = MessageBoxButtons.YesNo;
            dialogResult = MessageBox.Show(messageBoxMessage, k_Othello, messageButtons);

            // according to the users choice, restarts the game or closes the application
            if (dialogResult == DialogResult.Yes)
            {
                m_CurrentGameState.RestartGame();
                startNextGame();
            }
            else
            {
                Close();
            }
        }

        private void setPossibleMoves()
        {
            string currentPlayerName = m_CurrentGameState.CurrentPlayer.Name;
            List<Coordinates> possibleMoves = m_CurrentGameState.CurrentPlayer.ValidMoves;

            // sets the title of the form to the name of the current player
            Text = string.Format(k_Title, currentPlayerName);

            // for each possible move, sets the relevent button on the board to green and adds
            // it to the event listener's list
            foreach (Coordinates coord in possibleMoves)
            {
                ButtonGame buttonToChoose = m_BoardCells[coord.X, coord.Y];
                buttonToChoose.BackColor = Color.LightGreen;
                buttonToChoose.Enabled = true;
                buttonToChoose.Click += buttonToChoose_Click;
            }
        }

        private void buttonToChoose_Click(object i_Sender, EventArgs i_Args)
        {
            // Chose the i_Sender as ButtonGame or PictureBox
            ButtonGame button = i_Sender as ButtonGame;

            // Update the game state acordingly
            m_GameOperator.UpdateGame(new Coordinates(button.X, button.Y));

            // Start a new game
            startNextGame();
        }
    }
}
