using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Drawing;

namespace B15_Ex05_Othello
{
    [Flags]
    public enum eDirection
    {
        None = 0,
        Left = 1,
        Right = 2,
        Up = 4,
        Down = 8,
        LeftUp = 16,
        LeftDown = 32,
        RightUp = 64,
        RightDown = 128
    }

    internal class GameEngine
    {
        internal const int k_XCoord = 0;
        internal const int k_YCoord = 1;
        internal const int k_XYCoord = 2;
        internal const int k_true = 1;

        private static readonly Random m_RandomMoveForPC = new Random();
        private Board m_Board;
        private List<PossibleMove?> m_listOfPossibleMoves;

        public GameEngine(int i_BoardSize)
        {
            m_Board = new Board(i_BoardSize);
        }

        public void InitNewGame()
        {
            m_Board.InitNewOthelloGame();
        }

        // Get all empty cells in the board 
        private List<int[]> getPlayerCellsInBoard(Player i_Player)
        {
            List<int[]> allPlayerCellsInBoard = new List<int[]>();
            int[] cellCoordinates;
            int boardSize = m_Board.Size;

            for (int row = 0; row < boardSize; row++)
            {
                for (int colomn = 0; colomn < boardSize; colomn++)
                {
                    if (m_Board[row, colomn].Color == i_Player.Color)
                    {
                        cellCoordinates = new int[k_XYCoord];
                        cellCoordinates[k_XCoord] = row;
                        cellCoordinates[k_YCoord] = colomn;
                        allPlayerCellsInBoard.Add(cellCoordinates);
                    }
                }
            }

            return allPlayerCellsInBoard;
        }

        public void RestartGame()
        {
            Board.Restart();
            Board.InitNewOthelloGame();
        }

        // Get the current game board
        public Board Board
        {
            get { return m_Board; }
        }

        public void DoComputerMove(Player i_Player)
        {
            int numberOfMoves = m_listOfPossibleMoves.Count;
            int newMove = m_RandomMoveForPC.Next(numberOfMoves);

            drawPossibleMove(m_listOfPossibleMoves[newMove], i_Player);
            UpdatePlayerScore(i_Player);
            clearLastRoundPossibleMove(m_listOfPossibleMoves);
        }

        // Holds the move of the player
        public eSystemReply Move(int i_X, int i_Y, Player i_Player)
        {
            if (i_Player.IsComputer)
            {
                DoComputerMove(i_Player);
            }

            eSystemReply moveReply = eSystemReply.ValidMove;

            moveReply = doPlayerMoveWithXY(i_X, i_Y, i_Player);
            if (moveReply == eSystemReply.ValidMove)
            {
                UpdatePlayerScore(i_Player);
                clearLastRoundPossibleMove(m_listOfPossibleMoves);
            }

            return moveReply;
        }

        public void UpdatePlayerScore(Player i_Player)
        {
            List<int[]> playerCellsInBoard = getPlayerCellsInBoard(i_Player);

            i_Player.Score = playerCellsInBoard.Count;
        }

        private void clearLastRoundPossibleMove(List<PossibleMove?> i_PossibleMoves)
        {
            foreach (PossibleMove move in i_PossibleMoves)
            {
                if(m_Board[move.X, move.Y].Color == Color.Gray)
                {
                m_Board[move.X, move.Y].Color = Color.Empty;
                    }
            }

            m_listOfPossibleMoves.Clear();
        }

        public eSystemReply CheckIfUserCanPlay(Player i_Player)
     {
         eSystemReply canPlay = eSystemReply.PlayerHasNoMoves;
         List<PossibleMove?> listOfPossibleMovesForPlayer = GetPossibleMoves(i_Player);

         // if there are possible moves then the user can play.
         if (listOfPossibleMovesForPlayer.Count > 0)
         {
             canPlay = eSystemReply.PlayerCanMove;
             m_listOfPossibleMoves = listOfPossibleMovesForPlayer;
         }

         return canPlay;
     }

        private eSystemReply doPlayerMoveWithXY(int i_X, int i_Y, Player i_Player)
        {
            eSystemReply checkMoveReply = eSystemReply.ValidMove;
            bool playerCanMove = false;

            // Checks if the input is in the possible move list
            foreach (PossibleMove? move in m_listOfPossibleMoves)
            {
                // If it's in the list, it's a valid move
                if (move.Value.isEqualToPoisition(i_X, i_Y))
                {
                    playerCanMove = true;
                    drawPossibleMove(move, i_Player);
                }
            }

            if (playerCanMove == false)
            {
                checkMoveReply = eSystemReply.ErrorBadLogicInput;
            }

            return checkMoveReply;
        }

        // Draw a move of a player
        private void drawPossibleMove(PossibleMove? i_PossibleMove, Player i_Player)
        {
            if (i_PossibleMove != null)
            {
                // Drawing line for each direction in the possible move and update the board
                string[] directionsStringArray = i_PossibleMove.Value.Direction.ToString().Split(new[] { ", " }, StringSplitOptions.None);
                foreach (string directionStr in directionsStringArray)
                {
                    eDirection direction = (eDirection)System.Enum.Parse(typeof(eDirection), directionStr);
                    drawLineFromCellInDirection(i_PossibleMove.Value.X, i_PossibleMove.Value.Y, direction, i_Player);
                }
            }
        }

        // Get the list of the possible moves in the board for a player
        public List<PossibleMove?> GetPossibleMoves(Player i_Player)
        {
            List<int[]> playerCells = getPlayerCellsInBoard(i_Player);
            List<eDirection> listOfDirectionsForPossibleMoves = new List<eDirection>();
            List<PossibleMove?> listOfPossibleMoves = new List<PossibleMove?>();
            bool searchInDirection = false;

            foreach (int[] xyCoord in playerCells)
            {
                // Now we need to check all the directions with opponent sign in them
                foreach (string str in Enum.GetNames(typeof(eDirection)))
                {
                    eDirection direction = (eDirection)System.Enum.Parse(typeof(eDirection), str);
                    searchInDirection = checkCellInDirection(xyCoord[k_XCoord], xyCoord[k_YCoord], (eDirection)direction, i_Player.GetOpponentSign());
                    if (searchInDirection)
                    {
                        listOfDirectionsForPossibleMoves.Add(direction);
                    }
                }

                // For each direction with opponent we need to check if there is a possible move in the direction
                foreach (eDirection direction in listOfDirectionsForPossibleMoves)
                {
                    PossibleMove? newMove = getPossibleCellFromDirection(xyCoord[k_XCoord], xyCoord[k_YCoord], direction, i_Player);
                    addLocationToCell(newMove, ref listOfPossibleMoves);
                }

                // every new position we check we clear the list
                listOfDirectionsForPossibleMoves.Clear();
            }

            m_listOfPossibleMoves = listOfPossibleMoves;
            return listOfPossibleMoves;
        }

        private void addLocationToCell(PossibleMove? i_Cell, ref List<PossibleMove?> i_ListOfPossibleMoves)
        {
            bool cellFound = false;
            PossibleMove? newMove;

            if (i_Cell != null)
            {
                // Checks if the cell is already in the possible move list
                foreach (PossibleMove? move in i_ListOfPossibleMoves)
                {
                    if (move.Value.isEqualToPoisition(i_Cell.Value.X, i_Cell.Value.Y))
                    {
                        eDirection direction = new eDirection();
                        direction = i_Cell.Value.Direction;
                        direction |= move.Value.Direction;
                        cellFound = true;
                        newMove = new PossibleMove(i_Cell.Value.X, i_Cell.Value.Y, direction);
                        i_ListOfPossibleMoves.Remove(move);
                        i_ListOfPossibleMoves.Add(newMove);
                        break;
                    }
                }
                
            // If the cell is not in the list we add it.
                if (!cellFound)
                {
                    i_ListOfPossibleMoves.Add(i_Cell);
                }
            }
        }

        public void UpdateCellsForPossibleMoves(List<PossibleMove?> i_PossibleMoves)
        {
            foreach (PossibleMove move in i_PossibleMoves)
            {
                m_Board.UpdateCell(move.X, move.Y, Color.Gray);
            }
        }

        // translate the eDirection to row and column direction so will be easier to handle on the board
        private void getDirectionRowAndColumn(ref int o_rowDirection, ref int o_columnDirection, eDirection i_Direction)
        {
            switch (i_Direction)
            {
                case eDirection.Left:
                    o_columnDirection = -1;
                    o_rowDirection = 0;
                    break;

                case eDirection.Right:
                    o_columnDirection = 1;
                    o_rowDirection = 0;
                    break;

                case eDirection.Up:
                    o_rowDirection = -1;
                    o_columnDirection = 0;
                    break;

                case eDirection.Down:
                    o_rowDirection = 1;
                    o_columnDirection = 0;
                    break;

                case eDirection.LeftDown:
                    o_columnDirection = -1;
                    o_rowDirection = 1;
                    break;

                case eDirection.LeftUp:
                    o_columnDirection = -1;
                    o_rowDirection = -1;
                    break;

                case eDirection.RightDown:
                    o_columnDirection = 1;
                    o_rowDirection = 1;
                    break;

                case eDirection.RightUp:
                    o_columnDirection = 1;
                    o_rowDirection = -1;
                    break;

                default:
                    break;
            }
        }

        // check the cells in a certain direction and search for a specific color
        private bool checkCellInDirection(int i_X, int i_Y, eDirection i_Direction, Color i_ColorToSearch)
        {
            bool signExist = false;
            int rowDirection = 0, columnDirection = 0;

            getDirectionRowAndColumn(ref rowDirection, ref columnDirection, i_Direction);
            if (m_Board.IsCoordinatesInBounds(i_X + rowDirection, i_Y + columnDirection))
            {
                if (m_Board[i_X + rowDirection, i_Y + columnDirection].Color == i_ColorToSearch)
                {
                    signExist = true;
                }
            }

            return signExist;
        }

        // Get a possible move from cell to direction
        private PossibleMove? getPossibleCellFromDirection(int i_XCoord, int i_YCoord, eDirection i_Direction, Player i_Player)
        {
            int boardRow = 0, boardCol = 0;
            PossibleMove? possibleMove = null;
            bool foundSpaceToPlaceNewMove = false;
            bool inBoundsOfBoard = true;

            getDirectionRowAndColumn(ref boardRow, ref boardCol, i_Direction);

            // we want to search for an empty spot because we know theres already an opponent sign near us
            while (!foundSpaceToPlaceNewMove && inBoundsOfBoard)
            {
                i_XCoord += boardRow;
                i_YCoord += boardCol;

                // Checks if coordinates still in bound of the board
                inBoundsOfBoard = m_Board.IsCoordinatesInBounds(i_XCoord, i_YCoord);
                if (!inBoundsOfBoard)
                {
                    break;
                } 
                else if (m_Board[i_XCoord, i_YCoord].Color == i_Player.Color)
                {
                    break;
                } 
                else if (m_Board[i_XCoord, i_YCoord].Color == Color.Empty || m_Board[i_XCoord, i_YCoord].Color == Color.Gray)
                {
                    foundSpaceToPlaceNewMove = true;
                }
            }

            // if we found a new place, create a new coordinate array and return it
            if (foundSpaceToPlaceNewMove)
            {
                possibleMove = new PossibleMove(i_XCoord, i_YCoord, getOppositeDirection(i_Direction));
            }

            return possibleMove;
        }

        private eDirection getOppositeDirection(eDirection i_Direction)
        {
            eDirection oppositeDirection = eDirection.None;

            switch (i_Direction)
            {
                    case eDirection.Left:
                    oppositeDirection = eDirection.Right;
                    break;
                case eDirection.Right:
                    oppositeDirection = eDirection.Left;
                    break;
                case eDirection.Up:
                    oppositeDirection = eDirection.Down;
                    break;
                case eDirection.Down:
                    oppositeDirection = eDirection.Up;
                    break;
                case eDirection.LeftDown:
                    oppositeDirection = eDirection.RightUp;
                    break;
                case eDirection.LeftUp:
                    oppositeDirection = eDirection.RightDown;
                    break;
                case eDirection.RightUp:
                    oppositeDirection = eDirection.LeftDown;
                    break;
                case eDirection.RightDown:
                    oppositeDirection = eDirection.LeftUp;
                    break;
            }

            return oppositeDirection;
        }

        private void drawLineFromCellInDirection(int i_X, int i_Y, eDirection i_Direction, Player i_Player)
        {
            int rowDirection = 0, columnDirection = 0;

            m_Board.UpdateCell(i_X, i_Y, i_Player.Color);
            getDirectionRowAndColumn(ref rowDirection, ref columnDirection, i_Direction);
            do
            {
                i_X += rowDirection;
                i_Y += columnDirection;
                if (m_Board.IsCoordinatesInBounds(i_X + rowDirection, i_Y + columnDirection))
                {
                    m_Board.UpdateCell(i_X, i_Y, i_Player.Color);
                }
            } while (m_Board.IsCoordinatesInBounds(i_X + rowDirection, i_Y + columnDirection) && m_Board[i_X + rowDirection, i_Y + columnDirection].Color != i_Player.Color);
        }
    }
}
