/***************************************************************
 * File: Move.cs
 * Created By: Syed Ghulam Akbar		Date: 02 July, 2005
 * Description: This class stores info about a single chess game move.
 * It contains source and target chess squars/cells and also the type
 * of move and related info.
 ***************************************************************/

using System;

namespace ChessLibrary
{
	/// <summary>
	/// This class stores info about a single chess game move.
	/// It contains source and target chess squars/cells and also the type
	/// of move and related info.
	/// </summary>
    [Serializable]
	public class Move
	{
		public enum MoveType {NormalMove, CaputreMove, TowerMove, PromotionMove, EnPassant};	// Type of the move

		private Cell m_StartCell;	// start cell
		private Cell m_EndCell;		// end cell
		private Piece m_Piece;			// Piece being moved
		private Piece m_CapturedPiece;	// Piece captured by this mov
		private Piece m_PromoPiece;		// Piece selected after pawn promotion
		private Piece m_EnPassantPiece;	// Piece captured during enpassant move
		private MoveType m_Type;		// Type of the move
		private bool m_CauseCheck;		// if cause or leave the user under check
		private int	m_Score;			// Score of the move from the board analyze routine

        // Emptry internal constructor for XML Serialization support
        internal Move()
        {
            m_Score = 0;
        }

		public Move(Cell begin, Cell end)
		{
			m_StartCell=begin;
			m_EndCell=end;
			m_Piece=begin.piece;
			m_CapturedPiece=end.piece;
			m_Score=0;
		}

		// Returns the move start cell
		public Cell StartCell
		{
			get
			{
				return m_StartCell;
			}
            set
            {
                m_StartCell = value;
            }
		}

		// Returns the move end cell
		public Cell EndCell
		{
			get
			{
				return m_EndCell;
			}
            set
            {
                m_EndCell = value;
            }
		}

		// Returns the piece which was moved
		public Piece Piece
		{
			get
			{
				return m_Piece;
			}
            set
            {
                m_Piece = value;
            }
		}

		// Returns the captured piece 
		public Piece CapturedPiece
		{
			get
			{
				return m_CapturedPiece;
			}
            set
            {
                m_CapturedPiece = value;
            }
		}

		// Get and Set the move type property
		public MoveType Type
		{
			get
			{
				return m_Type;
			}
			set
			{
				m_Type=value;
			}
		}

		// Return true if the move when executed, place or leave user under check
		public bool CauseCheck
		{
			get
			{
				return m_CauseCheck;
			}
			set
			{
				m_CauseCheck=value;
			}
		}

		// Set and get the promo piece
		public Piece PromoPiece
		{
			get
			{
				return m_PromoPiece;
			}
			set
			{
				m_PromoPiece=value;
			}
		}

		// Set and get the EnPassant piece
		public Piece EnPassantPiece
		{
			get
			{
				return m_EnPassantPiece;
			}
			set
			{
				m_EnPassantPiece=value;
			}
		}

		// Set and get the move Score
		public int Score
		{
			get
			{
				return m_Score;
			}
			set
			{
				m_Score=value;
			}
		}

		// Return true if the move was promo move
		public bool IsPromoMove()
		{
			return m_Type==MoveType.PromotionMove;
		}

		// Return true if the move was capture move
		public bool IsCaptureMove()
		{
			return m_Type==MoveType.CaputreMove;
		}

		//Return a descriptive move text
		public override string ToString()
		{
			if (m_Type == Move.MoveType.CaputreMove)	// It's a capture move
				return m_Piece + " " + m_StartCell.ToString2() + "x" + m_EndCell.ToString2();
			else
				return m_Piece + " " + m_StartCell.ToString2() + "-" + m_EndCell.ToString2();
		}
	}

	// This class is used to compare two Move type objects
	public class MoveCompare : System.Collections.IComparer
	{
		// Empty constructore
		public MoveCompare()
		{
		}

		public int Compare(Object firstObj, Object SecondObj)
		{
			Move firstMove = (Move)firstObj;
			Move secondMove = (Move)SecondObj;

			return -firstMove.Score.CompareTo(secondMove.Score);
		}
	}
}
