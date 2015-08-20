using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace B15_Ex05_Othello
{
    public enum eSystemReply
    {
        None,
        ValidMove,
        ErrorBadInput,
        ErrorBadLogicInput,
        PlayerHasNoMoves,
        PlayerCanMove,
        PlayerWantsAnotherGame,
        PlayerWantsToQuit,
        GameOver
    }

    public class GameManager
    {
        private int m_BlackGamesScore;
        private int m_WhiteGamesScore;
        private GameEngine m_CurrentGame = null;
        private GameForm m_GameUI = null;
        private Player m_BlackPlayer = null;
        private Player m_WhitePlayer = null;
        private Player m_CurrentPlayer = null;

        // initializes the board and players.
        public void InitGame()
        {
            m_BlackPlayer = new Player(Color.Black);
            m_WhitePlayer = new Player(Color.White);
            m_CurrentPlayer = m_BlackPlayer;
            m_BlackGamesScore = m_WhiteGamesScore = 0;

            FormGameSettings formSettings = new FormGameSettings();
            formSettings.ShowDialog();
            if (formSettings.DialogResult == DialogResult.OK)
            {
                if (formSettings.IsAgainstComputer)
                {
                    m_WhitePlayer.IsComputer = true;
                }

                m_CurrentGame = new GameEngine(formSettings.BoardSize);
                m_GameUI = new GameForm(formSettings.BoardSize, this);
                m_CurrentGame.InitNewGame();

                startGame();
            }
        }

        private void startGame()
        {
            List<PossibleMove?> possibleMoves = m_CurrentGame.GetPossibleMoves(m_CurrentPlayer);
            m_CurrentGame.UpdateCellsForPossibleMoves(possibleMoves);
            m_GameUI.ShowDialog();
        }

        public Cell GetCellFromEngine(int i_X, int i_Y)
        {
            return m_CurrentGame.Board[i_X, i_Y];
        }

        private void switchPlayer(Player i_Player)
        {
            if (i_Player == m_BlackPlayer)
            {
                m_CurrentPlayer = m_WhitePlayer;
            }
            else
            {
                m_CurrentPlayer = m_BlackPlayer;
            }
        }

        public void DoIteration(int i_X, int i_Y)
        {
            bool playerCanMove = false;

            if (!m_CurrentPlayer.IsComputer)
            {
                m_CurrentGame.Move(i_X, i_Y, m_CurrentPlayer);

                switchPlayer(m_CurrentPlayer);
                playerCanMove = updatePossibleMoves();
            }

            if (m_CurrentPlayer.IsComputer && playerCanMove)
            {
                m_CurrentGame.DoComputerMove(m_CurrentPlayer);

                // changing the current player to user-player
                switchPlayer(m_CurrentPlayer);
                playerCanMove = updatePossibleMoves();
            }

            if (!playerCanMove)
            {
                switchPlayer(m_CurrentPlayer);
                playerCanMove = updatePossibleMoves();

                if (!playerCanMove)
                {
                    // There are no more possible moves, so the program announce the winner
                    showEndOfGameMessage();
                }
            }
        }

        private bool updatePossibleMoves()
        {
            // the method will return false if there are no more possible moves for the current player
            bool updated = false;

            List<PossibleMove?> possibleMoves = m_CurrentGame.GetPossibleMoves(m_CurrentPlayer);
            if (possibleMoves.Count != 0)
            {
                updated = true;
                m_CurrentGame.UpdateCellsForPossibleMoves(possibleMoves);
            }

            return updated;
        }

        private Player getWinner(out string o_Score)
        {
            Player winner = null; // if its a tie, we return null
            m_CurrentGame.UpdatePlayerScore(m_BlackPlayer);
            m_CurrentGame.UpdatePlayerScore(m_WhitePlayer);
            o_Score = String.Format("{0}/{1}", m_BlackPlayer.Score, m_WhitePlayer.Score);

            if (m_BlackPlayer.Score > m_WhitePlayer.Score)
            {
                winner = m_BlackPlayer;
                m_BlackGamesScore++;
            }
            else if (m_WhitePlayer.Score > m_BlackPlayer.Score)
            {
                winner = m_WhitePlayer;
                o_Score = String.Format("{0}/{1}", m_WhitePlayer.Score, m_BlackPlayer.Score);
                m_WhiteGamesScore++;
            }

            return winner;
        }

        private void showEndOfGameMessage()
        {
            string score, endOfGameAnnounce;
            Player winner = getWinner(out score);

            if (winner != null)
            {
                // One of the player won a round.
                // Update the round counting and build an announcment string
                endOfGameAnnounce = String.Format(
@"{0} Won!! ({1}) ({2}/{3})
Whould you like another round?",
                winner.ToString(), score, m_WhiteGamesScore, m_BlackGamesScore);
            }
            else
            {
                endOfGameAnnounce = String.Format(
@"Its a tie ({0}) ({1}/{2})
Whould you like another round?",
                score, m_WhiteGamesScore, m_BlackGamesScore);
            }

            DialogResult dialogResult = MessageBox.Show(endOfGameAnnounce, "Othello", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                anotherRound();
            }
            else
            {
                m_GameUI.Close();
            }
        }

        private void anotherRound()
        {
            m_CurrentGame.RestartGame();
            m_CurrentPlayer = m_BlackPlayer;
            List<PossibleMove?> possibleMoves = m_CurrentGame.GetPossibleMoves(m_CurrentPlayer);
            m_CurrentGame.UpdateCellsForPossibleMoves(possibleMoves);
        }
    }
}
