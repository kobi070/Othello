﻿using System;
using System.Windows.Forms;

namespace Ex05.Forms
{
    public class Program
    {
        public static void Main()
        {
            Run();
        }

        public static void Run()
        {
            FormGameSettings formGameSettings = new FormGameSettings();
            FormGame formGame;
            bool? isGameAgainstComputer = null;

            DialogResult result = formGameSettings.ShowDialog();
            if (result == DialogResult.Yes)
            {
                isGameAgainstComputer = true;
            }
            else if (result == DialogResult.No)
            {
                isGameAgainstComputer = false;
            }

            if (isGameAgainstComputer != null)
            {
                formGame = new FormGame(formGameSettings.BoardSize, (bool)isGameAgainstComputer);

                formGame.ShowDialog();
            }
        }
    }
}
