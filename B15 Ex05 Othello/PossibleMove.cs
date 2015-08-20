using System;
using System.Collections.Generic;
using System.Text;

namespace B15_Ex05_Othello
{   
    public struct PossibleMove
    {
        private int m_X;
        private int m_Y;
        private eDirection m_Direction;

        public PossibleMove(int i_X, int i_Y, eDirection i_Direction)
        {
            m_X = i_X;
            m_Y = i_Y;
            m_Direction = eDirection.None;
            m_Direction = i_Direction;
        }

        public int X
        {
            get { return m_X; }
        }

        public int Y
        {
            get { return m_Y; }
        }
      
        public eDirection Direction
        {
            get { return m_Direction; }
            set { m_Direction = value; }
        }

        public bool isEqualToPoisition(int i_X, int i_Y)
        {
            bool equalToPosition = false;

            if (m_X == i_X && m_Y == i_Y)
            {
                equalToPosition = true;
            }

            return equalToPosition;
        }

        // this method adds another direction to the possible move
        public eDirection addDirection(ref eDirection i_Direction)
        {
            eDirection newDirection = new eDirection();

            newDirection = m_Direction | i_Direction;
            return newDirection;
        }
    }
}
