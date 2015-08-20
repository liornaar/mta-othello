using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace B15_Ex05_Othello
{
    public class FormGameSettings : Form
    {
        public const int k_MaxBoardSize = 12;
        public const int k_MinBoardSize = 6;

        private bool m_GameAgainstComputer = false;
        private int m_BoardSize = k_MinBoardSize;

        // add members for computer and board size buttons settings
        private Button m_ButtonBoardSize = new Button();
        private Button m_ButtonComputer = new Button();
        private Button m_ButtonPlayer = new Button();

        public int BoardSize
        {
            get { return m_BoardSize; }
        }

        public FormGameSettings()
        {
            this.Size = new Size(360, 200);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Othello - Game Settings";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            initControls();
        }

        /// <summary>
        /// Layouting the controls (textboxes, lables, buttons) on the form
        /// </summary>
        private void initControls()
        {
            m_ButtonBoardSize.Text = "Board Size: 6x6 (click to increase)";
            m_ButtonBoardSize.Size = new Size(this.ClientSize.Width - 40, 50);
            m_ButtonBoardSize.Location = new Point(((this.ClientSize.Width) / 2) - (m_ButtonBoardSize.Size.Width / 2), 20);

            m_ButtonComputer.Text = "Play against the Computer";
            m_ButtonComputer.Size = new Size((m_ButtonBoardSize.Size.Width / 2) - 20, 40);
            int topLeft = m_ButtonBoardSize.Top + (this.ClientSize.Height / 2);
            m_ButtonComputer.Location = new Point(m_ButtonBoardSize.Location.X, topLeft);

            m_ButtonPlayer.Text = "Play against your friend";
            m_ButtonPlayer.Size = m_ButtonComputer.Size;
            m_ButtonPlayer.Location = new Point(m_ButtonComputer.Width + 60, topLeft);

            this.Controls.AddRange(new Control[] { m_ButtonBoardSize, m_ButtonComputer, m_ButtonPlayer });

            this.m_ButtonBoardSize.Click += new EventHandler(m_ButtonBoarSize_Click);
            this.m_ButtonPlayer.Click += new EventHandler(m_ButtonPlayer_Click);
            this.m_ButtonComputer.Click += new EventHandler(m_ButtonComputer_Click);
        }

        void m_ButtonBoarSize_Click(object i_Sender, EventArgs i_EventArgs)
        {
            m_BoardSize += 2;

            if (m_BoardSize > k_MaxBoardSize)
            {
                m_BoardSize = k_MinBoardSize;
            }

            string BoardSizeButtonString = String.Format("Board Size: {0}x{1} (click to increase)", m_BoardSize, m_BoardSize);
            this.m_ButtonBoardSize.Text = BoardSizeButtonString;
        }

        void m_ButtonComputer_Click(object i_Sender, EventArgs i_EventArgs)
        {
            m_GameAgainstComputer = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        void m_ButtonPlayer_Click(object i_Sender, EventArgs i_EventArgs)
        {
            m_GameAgainstComputer = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public bool IsAgainstComputer
        {
            get { return m_GameAgainstComputer; }
        }
    }
}
