/***************************************************************
 * File: Side.cs
 * Created By: Syed Ghulam Akbar		Date: 28 June, 2005
 * Description: This the main chess game class. It contains a chess board and two players. 
 * Also initialize and maintains the status of the game.
 * 
 * Modification History
 * 11/03/2010 11:20 pm v1.3 - Added Support for the Save/Load Options
 ***************************************************************/

using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace ChessLibrary
{
	/// <summary>
	/// This the main chess game class. It contains a chess board and two players. Also initialize and maintains the status of the game.
	/// </summary>
    [Serializable]
	public class Game
	{
		// Define delegates used to communicate the chess events to the UI
		public delegate void ChessComputerThinking(int depth, int currentMove, int TotalMoves, int TotalAnalzyed , Move BestMove);

		public event ChessComputerThinking ComputerThinking;	// Event used to fire computer thinking status

		public Board Board;		            // expose the game board to outside world
        public Side.SideType GameTurn;		    // Current game turn i.e. White or Black

        private Stack m_MovesHistory;		// Contains all moves made by the user		
        private Stack m_RedoMovesHistory;	// Contains all the Redo moves made by the user
		private Rules m_Rules;			    // Contains all the chess rules
		private Player m_WhitePlayer;	    // White Player objectg
		private Player m_BlackPlayer;	    // Black player object

		public bool DoNullMovePruning;		// True when compute should do null move pruning to speed up search
		public bool DoPrincipleVariation;	// True when computer should use principle variation to optimize search
		public bool DoQuiescentSearch;		// Return true when computer should do Queiscent search

		public Game()
		{
			Board = new Board();

			m_Rules = new Rules(Board, this);	
			m_MovesHistory = new Stack();
			m_RedoMovesHistory = new Stack();
            m_WhitePlayer = new Player(new Side(Side.SideType.White), Player.Type.Human, m_Rules);	// For the start both player are human
            m_BlackPlayer = new Player(new Side(Side.SideType.Black), Player.Type.Human, m_Rules);	// For the start both player are human
		}

		// Fire the computer thinking events to all the subscribers
		public void NotifyComputerThinking(int depth, int currentMove, int TotalMoves, int TotalAnalzyed, Move BestMove)
		{
			if (ComputerThinking!=null)	// There are some subscribers
				ComputerThinking(depth, currentMove, TotalMoves, TotalAnalzyed, BestMove);
		}

		// get the new item by rew and column
		public Cell this[int row, int col]
		{
			get
			{
				return Board[row, col];
			}
		}

		// get the new item by string location
		public Cell this[string strloc]
		{
			get
			{
				return Board[strloc];	
			}
		}

		// Return true, when it's a computer vs. computer game
		public bool CompVsCompGame()
		{
			return (m_WhitePlayer.PlayerType == m_BlackPlayer.PlayerType);
		}

        /// <summary>
        /// Save the current game state to the given file path
        /// </summary>
        /// <param name="filePath"></param>
        public void SaveGame(string filePath)
        {
            try
            {
                // Create the Game Xml 
                XmlDocument gameXmlDocument = new XmlDocument();
                XmlNode gameXml = XmlSerialize(gameXmlDocument);

                gameXmlDocument.AppendChild(gameXmlDocument.CreateXmlDeclaration("1.0", "utf-8", null));
                gameXmlDocument.AppendChild(gameXml);

                // Build the text writer and serlization the file
                gameXmlDocument.Save(filePath);
                return;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Load the current game state from the given file path
        /// </summary>
        /// <param name="filePath"></param>
        public void LoadGame(string filePath)
        {
            try
            {
                // Create the Game Xml 
                XmlDocument gameXmlDocument = new XmlDocument();
                gameXmlDocument.Load(filePath);

                XmlNode gameNode = gameXmlDocument.FirstChild;
                if (gameNode.NodeType == XmlNodeType.XmlDeclaration)
                    gameNode = gameNode.NextSibling;

                // De-serialize the Game state from the XML
                XmlDeserialize(gameNode);
            }
            catch (Exception) { }
        }

        // Computer the checksum for the XML content
        private string GetChecksum(string content)
        {
            SHA256Managed sha = new SHA256Managed();
            byte[] checksum = sha.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(content));
            return BitConverter.ToString(checksum).Replace("-", String.Empty);
        }

        /// <summary>
        /// Serialize the Game object as XML String
        /// </summary>
        /// <returns>XML containing the Game object state XML</returns>
        public XmlNode XmlSerialize(XmlDocument xmlDoc)
        {
            XmlElement xmlGame = xmlDoc.CreateElement("Game");

            // Append game state attributes
            xmlGame.AppendChild(XMLHelper.CreateNodeWithValue(xmlDoc, "DoNullMovePruning", DoNullMovePruning.ToString()));
            xmlGame.AppendChild(XMLHelper.CreateNodeWithValue(xmlDoc, "DoPrincipleVariation", DoPrincipleVariation.ToString()));
            xmlGame.AppendChild(XMLHelper.CreateNodeWithValue(xmlDoc, "DoQuiescentSearch", DoQuiescentSearch.ToString()));

            // Append the Game turn info
            xmlGame.AppendChild(XMLHelper.CreateNodeWithValue(xmlDoc, "GameTurn", GameTurn.ToString()));

            // Append the Board State
            xmlGame.AppendChild(Board.XmlSerialize(xmlDoc));

            // Append the Player Info
            xmlGame.AppendChild(XMLHelper.CreateNodeWithXmlValue(xmlDoc, "WhitePlayer", XMLHelper.XmlSerialize(typeof(Player), m_WhitePlayer)));
            xmlGame.AppendChild(XMLHelper.CreateNodeWithXmlValue(xmlDoc, "BlackPlayer", XMLHelper.XmlSerialize(typeof(Player), m_BlackPlayer)));

            object[] moves = m_MovesHistory.ToArray();

            // Store all the moves from the move history
            string xml = "";
            for (int i = moves.Length - 1; i >= 0; i-- )
            {
                Move move = (Move)moves[i];
                xml += XMLHelper.XmlSerialize(typeof(Move), move);
            }
            xmlGame.AppendChild(XMLHelper.CreateNodeWithXmlValue(xmlDoc, "MovesHistory", xml));

            // Create the Checksome to avoid user temporing of the file
            string checksum = GetChecksum(xmlGame.InnerXml);
            (xmlGame as XmlElement).SetAttribute("Checksum", checksum);
            (xmlGame as XmlElement).SetAttribute("Version", "1.2");

            // Return this as String
            return xmlGame;
        }

        /// <summary>
        /// DeSerialize the Game object from XML String
        /// </summary>
        /// <returns>XML containing the Game object state XML</returns>
        public void XmlDeserialize(XmlNode xmlGame)
        {
            // If this source file doesn't contain the check sum attribut, return back
            if (xmlGame.Attributes["Checksum"] == null)
                return;

            // Read game state attributes
            DoNullMovePruning = (XMLHelper.GetNodeText(xmlGame, "DoNullMovePruning") == "True");
            DoPrincipleVariation = (XMLHelper.GetNodeText(xmlGame, "DoPrincipleVariation") == "True");
            DoQuiescentSearch = (XMLHelper.GetNodeText(xmlGame, "DoQuiescentSearch") == "True");

            // Restore the Game turn info
            GameTurn = (XMLHelper.GetNodeText(xmlGame, "DoQuiescentSearch") == "Black") ? Side.SideType.Black : Side.SideType.White;

            // Restore the Board State
            XmlNode xmlBoard = XMLHelper.GetFirstNodeByName(xmlGame, "Board");
            Board.XmlDeserialize(xmlBoard);

            // Restore the Player info
            XmlNode xmlPlayer = XMLHelper.GetFirstNodeByName(xmlGame, "WhitePlayer");
            m_WhitePlayer = (Player)XMLHelper.XmlDeserialize(typeof(Player), xmlPlayer.InnerXml);
            m_WhitePlayer.GameRules = m_Rules;

            xmlPlayer = XMLHelper.GetFirstNodeByName(xmlGame, "BlackPlayer");
            m_BlackPlayer = (Player)XMLHelper.XmlDeserialize(typeof(Player), xmlPlayer.InnerXml);
            m_BlackPlayer.GameRules = m_Rules;

            // Restore all the moves for the move history
            XmlNode xmlMoves = XMLHelper.GetFirstNodeByName(xmlGame, "MovesHistory");
            foreach (XmlNode xmlMove in xmlMoves.ChildNodes)
            {
                Move move = (Move)XMLHelper.XmlDeserialize(typeof(Move), xmlMove.OuterXml);
                m_MovesHistory.Push(move);
            }
        }

		// Reset the game board and all player status
		public void Reset()
		{
			m_MovesHistory.Clear();
			m_RedoMovesHistory.Clear();

			// Reset player timers
			m_WhitePlayer.ResetTime();
			m_BlackPlayer.ResetTime();

            GameTurn = Side.SideType.White;	// In chess first turn is always of white
			m_WhitePlayer.TimeStart();	// Player time starts
			Board.Init();	// Initialize the board object
		}

		// Return back the white player reference
		public Player WhitePlayer
		{
			get
			{
				return m_WhitePlayer;
			}
		}

		// Return back the black player reference
		public Player BlackPlayer
		{
			get
			{
				return m_BlackPlayer;
			}
		}

		// Return the active player who has the turn to play
		public Player ActivePlay
		{
			get
			{
				if (BlackTurn())
					return m_BlackPlayer;
				else
					return m_WhitePlayer;
			}
		}

		// Return the enemy player for the given player
		public Player EnemyPlayer(Side Player)
		{
			if (Player.isBlack())
				return m_WhitePlayer;
			else
				return m_BlackPlayer;
		}

		// Return back the given side type
        public Player GetPlayerBySide(Side.SideType type)
		{
            if (type == Side.SideType.Black)
				return m_BlackPlayer;
			else
				return m_WhitePlayer;
		}

		// Re-calculate the total thinking time of the player
		public void UpdateTime()
		{
			if (BlackTurn())	// Black player turn
				m_BlackPlayer.UpdateTime();
			else
				m_WhitePlayer.UpdateTime();
		}

		// Return true if it's black turn to move
		public bool BlackTurn()
		{
            return (GameTurn == Side.SideType.Black);
		}

		// Return true if it's white turn to move
		public bool WhiteTurn()
		{
            return (GameTurn == Side.SideType.White);
		}

		// Set game turn for the next player
		public void NextPlayerTurn()
		{
            if (GameTurn == Side.SideType.White)
			{
				m_WhitePlayer.TimeEnd();		
				m_BlackPlayer.TimeStart();		// Start player timer
                GameTurn = Side.SideType.Black;		// Set black's turn
			}
			else
			{
				m_BlackPlayer.TimeEnd();
				m_WhitePlayer.TimeStart();		// Start player timer
                GameTurn = Side.SideType.White;		// Set white's turn
			}
		}

		// Returns all the legal moves for the given cell
		public ArrayList GetLegalMoves(Cell source)
		{
			return m_Rules.GetLegalMoves(source);
		}

		// Creat the move object and execute it
		public int DoMove(string source, string dest)
		{
			int MoveResult;

			// check if it's user turn to play
            if (this.Board[source].piece != null && this.Board[source].piece.Type != Piece.PieceType.Empty && this.Board[source].piece.Side.type == GameTurn)
			{
				Move UserMove = new Move(this.Board[source], this.Board[dest]);	// create the move object
				MoveResult=m_Rules.DoMove(UserMove);

				// If the move was successfully executed
				if (MoveResult==0)
				{
					m_MovesHistory.Push(UserMove);
					NextPlayerTurn();
				}
			}
			else
				MoveResult=-1;
			return MoveResult;	// Executed
		}

		// Undo one move from the moves history
		public bool UnDoMove()
		{
			// Check if there are Undo Moves available
			if (m_MovesHistory.Count>0)
			{
				Move UserMove = (Move)m_MovesHistory.Pop();	// Ge the user move from his moves history stack
				m_RedoMovesHistory.Push(UserMove);			// Add this move in user Redo moves stack
				m_Rules.UndoMove(UserMove);					// Undo the user move
				NextPlayerTurn();							// Switch the user turn
				return true;
			}
			else
				return false;
		}

		// Redo one move from the ReDo moves history
		public bool ReDoMove()
		{
			// Check if there are Redo Moves
			if (m_RedoMovesHistory.Count>0)
			{
				Move UserMove = (Move)m_RedoMovesHistory.Pop();	// Ge the user move from his moves history stack
				m_MovesHistory.Push(UserMove);				// Add to the user undo move list
				m_Rules.DoMove(UserMove);					// Undo the user move
				NextPlayerTurn();							// Switch the user turn
				return true;
			}
			else
				return false;
		}

        /// <summary>
        /// Get the move history object
        /// </summary>
        public Stack MoveHistory
        {
            get { return m_MovesHistory; }
        }

		// Return true if the given side is checkmate
        public bool IsCheckMate(Side.SideType PlayerSide)
		{
			return m_Rules.IsCheckMate(PlayerSide);
		}

		// Return true if the given side is stalemate
        public bool IsStaleMate(Side.SideType PlayerSide)
		{
			return m_Rules.IsStaleMate(PlayerSide);
		}

		// Return true if the current player is under check
		public bool IsUnderCheck()
		{
			return m_Rules.IsUnderCheck(GameTurn);
		}
		 

		// Return the last executed move
		public Move GetLastMove()
		{
			// Check if there are Undo Moves available
			if (m_MovesHistory.Count>0)
			{
				return (Move)m_MovesHistory.Peek();	// Ge the user move from his moves history stack
			}
			return null;
		}

		// Set the promo item for the last move
		public void SetPromoPiece(Piece PromoPiece)
		{
			// Check if there are Undo Moves available
			if (m_MovesHistory.Count>0)
			{
				Move move=(Move)m_MovesHistory.Peek();	// Ge the user move from his moves history
				move.EndCell.piece = PromoPiece;	// Set the promo piece
				move.PromoPiece = PromoPiece;		// Update the promo piece variable
			}
		}
	}
}
