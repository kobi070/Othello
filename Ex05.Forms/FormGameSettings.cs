using System;
using System.Windows.Forms;
using System.Drawing;

namespace Ex05.Forms
{
    internal class FormGameSettings : Form
    {
        // Conts for the Form and Form logic
        private const int k_MaxBoardSize = 12;
        private const int k_IncrementByTwo = 2;
        private const string k_BoardSizeMessage = "Board size: {0} x {0} (click to increase)";
        private const string k_PlayFriendMessage = "Play against your friend";
        private const string k_PlayComputerMessage = "Play against the computer";
        private const string k_FormGameSettingsName = "Othello - Game Settings";

        private const int k_WindowHeight = 150;
        private const int k_WindowWidth = 300;
        private const int k_WindowMargins = 10;
        private const int k_WindowTopBottomMargins = 20;
        private const int k_ButtonHeight = (k_WindowHeight - (3 * k_WindowTopBottomMargins)) / 2;


        private Button m_ButtonBoardSize = new Button();
        private Button m_ButtonPlayComputer = new Button();
        private Button m_ButtonPlayFriend = new Button();

        private int m_BoardSize = 6;

        public FormGameSettings()
        {
            ClientSize = new Size(k_WindowWidth, k_WindowHeight);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            StartPosition = FormStartPosition.CenterScreen;
            Text = k_FormGameSettingsName;
            MaximizeBox = false;
            MinimizeBox = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Initialize();
        }

        private void Initialize()
        {
            // Initialize The top button on the form which add's +2 by the user req
            m_ButtonBoardSize.Text = String.Format(k_BoardSizeMessage, m_BoardSize);
            m_ButtonBoardSize.Location = new Point(k_WindowMargins, k_WindowTopBottomMargins);
            m_ButtonBoardSize.Width = k_WindowWidth - (2 * k_WindowMargins);
            m_ButtonBoardSize.Height = k_ButtonHeight;

            // Init the play agains computer button
            m_ButtonPlayComputer.Text = k_PlayComputerMessage;
            m_ButtonPlayComputer.Location = new Point(k_WindowMargins, m_ButtonBoardSize.Location.Y + k_ButtonHeight + k_WindowTopBottomMargins);
            m_ButtonPlayComputer.Width = (k_WindowWidth - (3 * k_WindowMargins)) / 2;
            m_ButtonPlayComputer.Height = k_ButtonHeight;


            // Init button to start a game against another player
            m_ButtonPlayFriend.Text = k_PlayFriendMessage;
            m_ButtonPlayFriend.Location = new Point(
                m_ButtonPlayComputer.Location.X + m_ButtonPlayComputer.Width + k_WindowMargins,
                m_ButtonBoardSize.Location.Y + k_ButtonHeight + k_WindowTopBottomMargins);
            m_ButtonPlayFriend.Width = (k_WindowWidth - (3 * k_WindowMargins)) / 2;
            m_ButtonPlayFriend.Height = k_ButtonHeight;

            // Add the buttons to the form controls
            Controls.AddRange(new Control[] { m_ButtonBoardSize, m_ButtonPlayComputer, m_ButtonPlayFriend });

            // Add the button listeners
            m_ButtonBoardSize.Click += m_ButtonBoardSize_Click;
            m_ButtonPlayComputer.Click += m_ButtonPlayComputer_Click;
            m_ButtonPlayFriend.Click += m_ButtonPlayFriend_Click;

        }

        /// <summary>
        ///  Called every time a board is been incremented 
        ///  When button m_ButtonBoardSize is clicked we come here and increment the board size by 2
        ///  possible values are : 6x6, 8x8, 10x10, 12x12
        ///  Then we update the Board size by the button choice (up to the max possible value)
        /// </summary>
        /// <param name="i_Sender"></param>
        /// <param name="i_Args"></param>
        private void m_ButtonBoardSize_Click(object i_Sender, EventArgs i_Args)
        {
            if (m_BoardSize < k_MaxBoardSize)
            {
                m_BoardSize += 2;
            }

            m_ButtonBoardSize.Text = String.Format(k_BoardSizeMessage, m_BoardSize);

        }

        /// <summary>
        /// Click listeners for the game start buttons - Either against a computer,
        /// or against a friendThey set the Form DialogResult as Yes (to play vs computer)
        /// or No (to not play against computer)and close this form.
        /// The Program (The class that created this form) will receive this DialogResultand start FormGame accordingly
        /// => - vs Computer or Player respectively.
        /// </summary>
        private void m_ButtonPlayComputer_Click(object i_Sender, EventArgs i_Args)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void m_ButtonPlayFriend_Click(object i_Sender, EventArgs i_Args)
        {
            DialogResult= DialogResult.No;
            Close();
        }

        public int BoardSize
        {
            get { return m_BoardSize; }
            set { m_BoardSize = value; }
        }
    }
}
