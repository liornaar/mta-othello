using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace B15_Ex05_Othello
{
    public class GameForm : Form
    {
        private const int k_ButtonSize = 40;
        private const string k_ButtonText = "O";
        private const int k_ButtonSpacing = 5;

        private readonly int r_BoardDimension;
        private GameButton[,] m_ButtonMatrix;
        private GameManager m_GameManager = null; // reference to the GameManager in order to make player moves.

        public GameForm(int i_BoardDimension, GameManager i_GameManager)
        {
            m_GameManager = i_GameManager;
            r_BoardDimension = i_BoardDimension;
            generateButtonMatrix();
            int formSize = ((k_ButtonSize + k_ButtonSpacing) * r_BoardDimension) + k_ButtonSpacing;
            this.ClientSize = new Size(formSize, formSize);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.Text = "Othello";
        }

        public void RestartGame()
        {
            generateButtonMatrix();
        }

        public GameButton this[int i_X, int i_Y]
        {
            get { return m_ButtonMatrix[i_X, i_Y]; }
        }

        private void generateButtonMatrix()
        {
            m_ButtonMatrix = new GameButton[r_BoardDimension, r_BoardDimension];

            for (int y = 0; y < r_BoardDimension; y++)
            {
                for (int x = 0; x < r_BoardDimension; x++)
                {
                    m_ButtonMatrix[y, x] = new GameButton();
                    m_ButtonMatrix[y, x].X = x;
                    m_ButtonMatrix[y, x].Y = y;
                    m_ButtonMatrix[y, x].Width = m_ButtonMatrix[y, x].Height = k_ButtonSize;
                    m_ButtonMatrix[y, x].Location = new System.Drawing.Point((y * (k_ButtonSize + k_ButtonSpacing)) + k_ButtonSpacing, (x * (k_ButtonSpacing + k_ButtonSize)) + k_ButtonSpacing);
                    m_ButtonMatrix[y, x].Click += m_ButtonMatrix_Click;
                    m_ButtonMatrix[y, x].TabIndex = ((y + 1) * r_BoardDimension) + (x + 1);
                    m_ButtonMatrix[y, x].Enabled = false;
                    m_GameManager.GetCellFromEngine(x, y).ChangeColor += gameButton_ChangeColor; 
                    
                    this.Controls.Add(m_ButtonMatrix[y, x]);
                }
            }
        }

        private void gameButton_ChangeColor(Color i_NewColor, int i_X, int i_Y)
        {
            m_ButtonMatrix[i_Y, i_X].BackColor = i_NewColor;

            if (i_NewColor == Color.Gray)
            {
                m_ButtonMatrix[i_Y, i_X].Enabled = true;
            }
            else
            {
                m_ButtonMatrix[i_Y, i_X].Enabled = false;
            }

            if (i_NewColor != Color.Gray && i_NewColor != Color.Empty)
            {
                m_ButtonMatrix[i_Y, i_X].Text = k_ButtonText;
            }
            else
            {
                m_ButtonMatrix[i_Y, i_X].Text = String.Empty;
            }
            
            if (i_NewColor == Color.Black)
            {
                m_ButtonMatrix[i_Y, i_X].ForeColor = Color.White;
            }
        }

        private void m_ButtonMatrix_Click(object sender, EventArgs e)
        {
            GameButton button = sender as GameButton;
            int x = button.X;
            int y = button.Y;
            m_GameManager.DoIteration(y, x);
        }
    }
}
