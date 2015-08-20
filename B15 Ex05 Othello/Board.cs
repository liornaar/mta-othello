using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace B15_Ex05_Othello
{
    // board class holds the matrix to show in the game
   public class Board
    {
       private readonly int r_BoardSize;
       private Cell[,] m_BoardMatrixx = null;

       public int Size
       {
           get { return r_BoardSize; }
       }

       public Board(int i_BoardSize)
       {
           r_BoardSize = i_BoardSize;
           m_BoardMatrixx = new Cell[i_BoardSize, i_BoardSize];
           initBoard();
       }

       // indexer for char at cell
       public Cell this[int i_XIndex, int i_Yindex]
       {
           get { return m_BoardMatrixx[i_XIndex, i_Yindex]; }
           set { m_BoardMatrixx[i_XIndex, i_Yindex] = value; }
       }
       
       // for the initialization of the board , we need to put the first coins
       private void initBoard()
       {
           for (int i = 0; i < r_BoardSize; i++)
           {
               for (int j = 0; j < r_BoardSize; j++)
               {
                   m_BoardMatrixx[j, i] = new Cell(Color.Empty, i, j);
               }
           }
       }

       public void Restart()
       {
           for (int i = 0; i < r_BoardSize; i++)
           {
               for (int j = 0; j < r_BoardSize; j++)
               {
                   m_BoardMatrixx[j, i].Color = Color.Empty;
               }
           }
       }

       public void InitNewOthelloGame()
       {
           int center = r_BoardSize / 2;
           UpdateCell(center - 1, center - 1, Color.White);
           UpdateCell(center, center, Color.White);
           UpdateCell(center - 1, center, Color.Black);
           UpdateCell(center, center - 1, Color.Black);
       }

       // updates the cell in the board with the requested color
       public void UpdateCell(int i_X, int i_Y, Color i_Color)
       {
           m_BoardMatrixx[i_X, i_Y].Color = i_Color;
       }

       public bool IsCoordinatesInBounds(int i_X, int i_Y)
       {
           bool isInBounds = true;
           if (i_X >= r_BoardSize || i_X < 0 || i_Y >= r_BoardSize || i_Y < 0)
           {
               isInBounds = false;
           }

           return isInBounds;
       }
    }
}
