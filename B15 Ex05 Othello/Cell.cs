using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace B15_Ex05_Othello
{
    // A delegate for cell, announce an event if changed the color.
    public delegate void ButtonEventHandler(Color i_NewColor, int i_X, int i_Y);

    public class Cell
    {
        private readonly int r_X;
        private readonly int r_Y;
        private Color m_Color;

        public event ButtonEventHandler ChangeColor;

        public Cell(Color i_Color, int i_X, int i_Y)
        {
            m_Color = i_Color;
            r_X = i_X;
            r_Y = i_Y;
        }

        public int X
        {
            get { return r_X; }
        }

        public int Y
        {
            get { return r_Y; }
        }

        public Color Color
        {
            get { return m_Color; }
            set 
            {
                ChangeColor.Invoke(value, this.r_X, this.r_Y);
                m_Color = value;
            }
        }
    }
}
