using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace B15_Ex05_Othello
{
    public class GameButton : Button
    {
        private int m_ButtonX;
        private int m_ButtonY;

        public int X
        {
            get { return m_ButtonX; }
            set { m_ButtonX = value; }
        }

        public int Y
        {
            get { return m_ButtonY; }
            set { m_ButtonY = value; }
        }
    }
}
