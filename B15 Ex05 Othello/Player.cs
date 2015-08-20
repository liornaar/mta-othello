using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace B15_Ex05_Othello
{
   public class Player
    {
       private readonly Color r_PlayerColor;
       private string m_Name;
       private int m_Score = 0;
       private bool m_IsComputer = false;

       public Player(Color i_Color)
       {
           r_PlayerColor = i_Color;
       }

       public Color Color
       {
           get { return r_PlayerColor; }
       }

       public string Name
       {
           get { return m_Name; }
           set { m_Name = value; }
       }

       public int Score
       {
           get { return m_Score; }
           set { m_Score = value; }
       }

       public Color GetOpponentSign()
       {
           Color opponentColor = Color.White;
           if (this.Color == Color.White)
           {
               opponentColor = Color.Black;
           }

           return opponentColor;
       }

       public override string ToString()
       {
           return r_PlayerColor.Name;
       }

       public bool IsComputer
       {
           get { return m_IsComputer; }
           set { m_IsComputer = value; }
       }
    }
}
