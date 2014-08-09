/***************************************************************
 * File: Piece.cs
 * Created By: Syed Ghulam Akbar		Date: 27 June, 2005
 * Description: This class contains all the chess game pieces movement
 * rules.
 ***************************************************************/
using System;
using System.Collections;

namespace ChessLibrary
{
	/// <summary>
	/// This class contains all the chess game pieces movement
	/// rules.
	/// </summary>
	public class Rules
	{
		private Board m_Board;	// store a reference to the game board
		private Game m_Game;	// store a reference to the current game

		public Rules(Board board, Game game)
		{
			m_Board=board;
			m_Game=game;
		}

		// Return the current game board object
		public Board ChessBoard
		{
			get 
			{
				return m_Board;	
			}
		}

		// Return the current game object
		public Game ChessGame
		{
			get 
			{
				return m_Game;	
			}
		}

		// Return true if the given side is checkmate
        public bool IsCheckMate(Side.SideType PlayerSide)
		{
			// if player is under check and he has no moves
			if ( IsUnderCheck(PlayerSide) && GetCountOfPossibleMoves(PlayerSide) == 0)
				return true;	// player is checkmate
			else
				return false;
		}

		// Return true if the given side is stalemate
        public bool IsStaleMate(Side.SideType PlayerSide)
		{
			// if player is not under check and he has no moves
			if ( !IsUnderCheck(PlayerSide) && GetCountOfPossibleMoves(PlayerSide) == 0)
				return true;	// player is checkmate
			else
				return false;
		}

		// Returns true move command results
		public int DoMove(Move move)
		{
			// first of all check here that if move is legal i.e we can make a move
            ArrayList LegalMoves = GetLegalMoves(m_Board[move.StartCell]);

            if (!LegalMoves.Contains(m_Board[move.EndCell]))	// Not a legal move
				return -2;	// Return illegal move error

			// Now if we can move check and execute the move
			SetMoveType(move);	// calculate and set the move type
			ExecuteMove(move);	// Execute the move

			return 0;	// Executed
		}

		// Actually execute the move
		public void ExecuteMove(Move move)
		{
			// Check and execute the the move
			switch (move.Type)
			{
				case Move.MoveType.CaputreMove:		// Capture move
					DoNormalMove(move);
					break;

				case Move.MoveType.NormalMove:		// Normal move
					DoNormalMove(move);
					break;

				case Move.MoveType.TowerMove:		// Tower move
					DoTowerMove(move);
					break;

				case Move.MoveType.PromotionMove:	// Promotion move
					DoPromoMove(move);
					break;

				case Move.MoveType.EnPassant:		// EnPassant move
					DoEnPassantMove(move);
					break;
			}
		}

		// return type of the move given the move object
		private void SetMoveType(Move move)
		{
			// start with the normal move type
			move.Type = Move.MoveType.NormalMove;

			// check if the move is of capture type
			if (move.EndCell.piece != null && move.EndCell.piece.Type != Piece.PieceType.Empty ) 
				move.Type = Move.MoveType.CaputreMove;

			// check if the move is of tower/castling type
			if (move.StartCell.piece != null && move.StartCell.piece.Type == Piece.PieceType.King)
			{
				if (Math.Abs(move.EndCell.col - move.StartCell.col)>1)	// king can move to other than neighbour cell only in tower move
					move.Type = Move.MoveType.TowerMove;
			}

			// check if the move is a pawn promotion move
			if (move.StartCell.piece != null && move.StartCell.piece.Type == Piece.PieceType.Pawn)
			{
				// Pawn is being promoted
				if (move.EndCell.row == 8 || move.EndCell.row == 1)
					move.Type = Move.MoveType.PromotionMove;
			}		

			// check if the move is a en passant move
			if (move.StartCell.piece != null && move.StartCell.piece.Type == Piece.PieceType.Pawn)
			{
				// Pawn is being being moved in a corner without a piece
				if ((move.EndCell.piece == null || move.EndCell.piece.IsEmpty()) && move.StartCell.col != move.EndCell.col )
					move.Type = Move.MoveType.EnPassant;
			}
		}

		// Do the normal move i.e. desitnation is empty; simply move the source piece
		private void DoNormalMove(Move move)
		{
			m_Board[move.StartCell].piece.Moves++;	// incremenet moves
			m_Board[move.EndCell].piece = m_Board[move.StartCell].piece;		// Move object at the destination
			m_Board[move.StartCell].piece = new Piece(Piece.PieceType.Empty);	// Empty the source location
		}

		// Do the castling/tower move. King interchanges it's position with it's rock
		private void DoTowerMove(Move move)
		{
			DoNormalMove(move);	// move the king to target position

			// Now check the direction of the king movement
			if (move.EndCell.col > move.StartCell.col) // moving right
			{
				Cell rockcell = m_Board.RightCell(move.EndCell);
				Move newmove = new Move(rockcell,m_Board.LeftCell(move.EndCell)); // create the move for rock
				DoNormalMove(newmove); // Move the rock
			}
			else
			{
				// Move to the left side
				Cell rockcell = m_Board.LeftCell(move.EndCell);
                rockcell = m_Board.LeftCell(rockcell);
				Move newmove = new Move(rockcell,m_Board.RightCell(move.EndCell)); // create the move for rock
				DoNormalMove(newmove); // Move the rock
			}
		}

		// Do the pawn promotion move
		private void DoPromoMove(Move move)
		{
			DoNormalMove(move);	// Do the normal move
			// check if promo piece is already selected by the user
			if (move.PromoPiece==null)
				m_Board[move.EndCell].piece = new Piece(Piece.PieceType.Queen, m_Board[move.EndCell].piece.Side);	// Set the end cell to queen
			else
				m_Board[move.EndCell].piece = move.PromoPiece;
		}

		// Do the EnPassant Move
		private void DoEnPassantMove(Move move)
		{
			Cell EnPassantCell;

			if (move.StartCell.piece.Side.isWhite())	// white piece is moving
				EnPassantCell = m_Board.BottomCell(move.EndCell);	// Get the cell under target position
			else
				EnPassantCell = m_Board.TopCell(move.EndCell);	// Get the cell under target position

			move.EnPassantPiece = EnPassantCell.piece;				// Save a reference to the en passant cell
			EnPassantCell.piece = new Piece(Piece.PieceType.Empty);	// Empty the en-passant cell
			DoNormalMove(move);	// Move the pawn to it's target position
		}

		// Undo the user move 
		public void UndoMove(Move move)
		{
			if (move.Type == Move.MoveType.CaputreMove || move.Type == Move.MoveType.NormalMove || move.Type == Move.MoveType.PromotionMove ) 
				UndoNormalMove(move);

			// Undo the tower move
			if (move.Type == Move.MoveType.TowerMove)
			{
				UndoNormalMove(move);	// First move the king to it's orignal position
				if (move.EndCell.col > move.StartCell.col) // moving right
				{
					// Now move the rook back to it's orignal position
					Cell source = m_Board.LeftCell(move.EndCell);	// Get the new position of the rock
					Cell target = m_Board[move.StartCell.row, 8];	// Get the rook orignal position

					m_Board[source].piece.Moves--;	// decrement moves
					m_Board[target].piece = m_Board[source].piece;		// Move object at the destination
					m_Board[source].piece = new Piece(Piece.PieceType.Empty);	// Empty the source location	
				}
				else	// Moving Left
				{
					// Now move the rook back to it's orignal position
					Cell source = m_Board.RightCell(move.EndCell);	// Get the new position of the rock
					Cell target = m_Board[move.StartCell.row, 1];	// Get the rook orignal position

					m_Board[source].piece.Moves--;	// decrement moves
					m_Board[target].piece = m_Board[source].piece;		// Move object at the destination
					m_Board[source].piece = new Piece(Piece.PieceType.Empty);	// Empty the source location
				}
			}

			// Undo the EnPassant move
			if (move.Type == Move.MoveType.EnPassant)
			{
				Cell EnPassantCell;

				UndoNormalMove(move);
				if (move.StartCell.piece.Side.isWhite())	// white piece was moved
					EnPassantCell = m_Board.BottomCell(move.EndCell);	// Get the cell under target position
				else
					EnPassantCell = m_Board.TopCell(move.EndCell);	// Get the cell under target position

				EnPassantCell.piece = move.EnPassantPiece;	// set back the enpassant piece
			}
		}

		// Undo the normal move i.e. desitnation was empty; simply moe the source piece back to it's orignal
		private void UndoNormalMove(Move move)
		{
			m_Board[move.EndCell].piece = move.CapturedPiece;		// Move object at the destination
			m_Board[move.StartCell].piece = move.Piece;	// Empty the source location
			m_Board[move.StartCell].piece.Moves--;	// decrement moves
		}

		// Return true if the given side type is under check state
        public bool IsUnderCheck(Side.SideType PlayerSide)
		{
			Cell OwnerKingCell=null;
			ArrayList OwnerCells = m_Board.GetSideCell(PlayerSide);

			// loop all the owner squars and get his king cell
			foreach (string CellName in OwnerCells)
			{
				if (m_Board[CellName].piece.Type == Piece.PieceType.King )
				{
					OwnerKingCell = m_Board[CellName]; // store the enemy cell position
					break;	// break the loop
				}
			}

			// Loop all the enemy squars and get their possible moves
			ArrayList EnemyCells = m_Board.GetSideCell((new Side(PlayerSide)).Enemy());
			foreach (string CellName in EnemyCells)
			{
				ArrayList moves = GetPossibleMoves(m_Board[CellName]);	// Get the moves for the enemy piece
				// King is directly under attack
				if (moves.Contains(OwnerKingCell))	
					return true;
			}
			return false;
		}

		// Returns a count of all the possilbe moves for given side
        private int GetCountOfPossibleMoves(Side.SideType PlayerSide)
		{
			int TotalMoves=0;
           
			// Loop all the owner squars and get their possible moves
			ArrayList PlayerCells = m_Board.GetSideCell(PlayerSide);
			foreach (string CellName in PlayerCells)
			{
				ArrayList moves = GetLegalMoves(m_Board[CellName]);	// Get all the legal moves for the owner piece
				TotalMoves+=moves.Count;
			}
			return TotalMoves;
		}

		// Returns true if the give move place the user under check
		private bool CauseCheck(Move move)
		{
			bool CauseCheck=false;
            Side.SideType PlayerSide = move.StartCell.piece.Side.type;

			// To check if a move cause check, we actually need to execute and check the result of that move
			ExecuteMove(move);
			CauseCheck=IsUnderCheck(PlayerSide);
			UndoMove(move);	// undo the move

			return CauseCheck;
		}

		// Returns all the legal moves for the given cell
		public ArrayList GetLegalMoves(Cell source)
		{
			ArrayList LegalMoves;

			LegalMoves=GetPossibleMoves(source);	// Get the legal moves
			ArrayList ToRemove = new ArrayList();	// contains a list of all the moves to remove

			// Now check and mark all the moves which moves user under check
			foreach (Cell target in  LegalMoves)
			{
				// if the move place or leave the user under check
				if (CauseCheck(new Move(source, target)))	
					ToRemove.Add(target);
			}

            // When checking the moves for the king, don't allow tower/caslting, if
            // the king is under check
            if (source.piece.Type == Piece.PieceType.King && IsUnderCheck(source.piece.Side.type))
            {
                foreach (Cell target in LegalMoves)
                {
                    // if the move place or leave the user under check
                    if (Math.Abs(target.col - source.col) > 1)
                        ToRemove.Add(target);
                }
            }

			// remove all the illegal moves
			foreach (Cell cell in  ToRemove)
			{
				LegalMoves.Remove(cell);	// remove the illegal move
			}
			return LegalMoves;
		}

		// Generate all the legal moves for the given side and return back moves in the 
		// sorted order
		public ArrayList GenerateAllLegalMoves(Side PlayerSide)
		{
			ArrayList TotalMoves = new ArrayList();
			ArrayList PlayerCells = m_Board.GetSideCell(PlayerSide.type);
			Move move;	// contains the temporary move object

			// Loop all the owner squars and get their possible moves
			foreach (string CellName in PlayerCells)
			{
				ArrayList moves = GetLegalMoves(m_Board[CellName]);	// Get all the legal moves for the owner piece
				
				foreach (Cell dest in moves)
				{
					move = new Move(m_Board[CellName], dest);
					SetMoveType(move);				// Set the move type

					if (move.IsPromoMove())			// Pawn promotion move
						move.Score=1000;
					else if (move.IsCaptureMove())	// Pawn capture move
						move.Score=move.EndCell.piece.GetWeight();	// Sort by item being captured

					TotalMoves.Add(move);	// Add the move to total moves
				}
			}

			// For the best performance of the alpha beta search. It's extemely important
			// that the best sort is tried first. So that cut of is achieved in early moves
			MoveCompare moveCompareObj= new MoveCompare();
			TotalMoves.Sort(moveCompareObj);	// Move the moves with highest score on top of the array

			return TotalMoves;
		}


		// Generate all the good capture moves. These are used for quiescent searching 
		public ArrayList GenerateGoodCaptureMoves(Side PlayerSide)
		{
			ArrayList TotalMoves = new ArrayList();
			ArrayList PlayerCells = m_Board.GetSideCell(PlayerSide.type);
			Move move;	// contains the temporary move object

			// Loop all the owner squars and get their possible moves
			foreach (string CellName in PlayerCells)
			{
				// Currently we are only checking moves of high rank pieces
				if (m_Board[CellName].piece.GetWeight()> 100)
				{
					ArrayList moves = GetLegalMoves(m_Board[CellName]);	// Get all the legal moves for the owner piece
				
					foreach (Cell dest in moves)
					{
						// Check only capture moves
						if ( dest.piece != null && !dest.piece.IsEmpty())
						{
							move = new Move(m_Board[CellName], dest);
							//SetMoveType(move);		// Set the move type
							TotalMoves.Add(move);	// Add the move to total moves
						}
					}
				}
			}
			return TotalMoves;
		}
		
		// Returns all the possible moves wether they are legal or not i.e. some moves may cause or leave check 
		// so for the particular situation they may become illegal 
		public ArrayList GetPossibleMoves(Cell source)
		{
			ArrayList LegalMoves = new ArrayList();

			// Check the legal moves for the object
			switch (source.piece.Type)
			{
				case Piece.PieceType.Empty:	// cell is empty
					break;

				case Piece.PieceType.Pawn:	// Pawn object
					GetPawnMoves(source, LegalMoves);
					break;

				case Piece.PieceType.Knight:	// Knight object
					GetKnightMoves(source, LegalMoves);
					break;

				case Piece.PieceType.Rook:	// Rook piece
					GetRookMoves(source, LegalMoves);
					break;

				case Piece.PieceType.Bishop:	// Bishop piece
					GetBishopMoves(source, LegalMoves);
					break;

				case Piece.PieceType.Queen:	// Queen piece
					GetQueenMoves(source, LegalMoves);
					break;

				case Piece.PieceType.King:	// king piece
					GetKingMoves(source, LegalMoves);
					break;
			}

			return LegalMoves;
		}

		// Returns true if the last move was a pawn begin move. It's used for En Passant move detection
		private Move LastMoveWasPawnBegin()
		{
			// Now get user last move and see if it's a pawn move
			Move lastmove = m_Game.GetLastMove();

			if (lastmove!=null)	// last moe is not available
			{
				if (lastmove.Piece.IsPawn()&& lastmove.Piece.Moves == 1)
				{
						return lastmove;
				}
			}
			return null;
		}

		// calculate the possible moves for the pawn object and insert them into passed array
		private void GetPawnMoves(Cell source, ArrayList moves)
		{
			Cell newcell;

			if (source.piece.Side.isWhite())
			{
				// Calculate moves for the white piece
				newcell = m_Board.TopCell(source);	
				if (newcell!=null && newcell.IsEmpty()) // Top cell is available for the move
					moves.Add(newcell);
				
				// Check the 2nd top element from source
				if (newcell != null && newcell.IsEmpty())
				{
					newcell = m_Board.TopCell(newcell);	
					if (newcell!=null && source.piece.Moves == 0 && newcell.IsEmpty()) // 2nd top cell is available and piece has not yet moved
						moves.Add(newcell);
				}

				// Check top-left cell for enemy piece
				newcell = m_Board.TopLeftCell(source);	
				if (newcell!=null && newcell.IsOwnedByEnemy(source)) // Top cell is available for the move
					moves.Add(newcell);

				// Check top-right cell for enemy piece
				newcell = m_Board.TopRightCell(source);	
				if (newcell!=null && newcell.IsOwnedByEnemy(source)) // Top cell is available for the move
					moves.Add(newcell);

				// Check for possible En Passant Move
				Move LastPawnMove=LastMoveWasPawnBegin();	

				if (LastPawnMove!=null)	// last move was a pawn move
				{
					if (source.row == LastPawnMove.EndCell.row) // Can do En Passant
					{
						if (LastPawnMove.EndCell.col == source.col-1)	// En Passant pawn is on left side
						{
							newcell = m_Board.TopLeftCell(source);	
							if (newcell!=null && newcell.IsEmpty()) // Top cell is available for the move
								moves.Add(newcell);
						}

						if (LastPawnMove.EndCell.col == source.col+1)	// En Passant pawn is on left side
						{
							newcell = m_Board.TopRightCell(source);	
							if (newcell!=null && newcell.IsEmpty()) // Top cell is available for the move
								moves.Add(newcell);
						}
					}
				}
			}
			else
			{
				// Calculate moves for the black piece
				newcell = m_Board.BottomCell(source);	
				if (newcell!=null && newcell.IsEmpty()) // bottom cell is available for the move
					moves.Add(newcell);
				
				// Check the 2nd bottom cell from source
				if (newcell!=null && newcell.IsEmpty())
				{
					newcell = m_Board.BottomCell(newcell);	
					if (newcell!=null && source.piece.Moves == 0 && newcell.IsEmpty()) // 2nd bottom cell is available and piece has not yet moved
						moves.Add(newcell);
				}

				// Check bottom-left cell for enemy piece
				newcell = m_Board.BottomLeftCell(source);	
				if (newcell!=null && newcell.IsOwnedByEnemy(source)) // Bottom cell is available for the move
					moves.Add(newcell);

				// Check bottom-right cell for enemy piece
				newcell = m_Board.BottomRightCell(source);	
				if (newcell!=null && newcell.IsOwnedByEnemy(source)) // Bottom cell is available for the move
					moves.Add(newcell);

				// Check for possible En Passant Move
				Move LastPawnMove=LastMoveWasPawnBegin();	

				if (LastPawnMove!=null)	// last move was a pawn move
				{
					if (source.row == LastPawnMove.EndCell.row) // Can do En Passant
					{
						if (LastPawnMove.EndCell.col == source.col-1)	// En Passant pawn is on left side
						{
							newcell = m_Board.BottomLeftCell(source);	
							if (newcell!=null && newcell.IsEmpty()) // Bottom cell is available for the move
								moves.Add(newcell);
						}

						if (LastPawnMove.EndCell.col == source.col+1)	// En Passant pawn is on left side
						{
							newcell = m_Board.BottomRightCell(source);	
							if (newcell!=null && newcell.IsEmpty()) // Bottom cell is available for the move
								moves.Add(newcell);
						}
					}
				}
			}
		}

		// calculate the possible moves for the knight piece and insert them into passed array
		private void GetKnightMoves(Cell source, ArrayList moves)
		{
			Cell newcell;

			// First check top two left and right moves for knight
			newcell = m_Board.TopCell(source);
			if (newcell!=null)
			{
				newcell = m_Board.TopLeftCell(newcell);
				// target cell is empty or is owned by the enemy piece
				if (newcell!=null && !newcell.IsOwned(source)) 
					moves.Add(newcell);

				newcell = m_Board.TopCell(source);
				newcell = m_Board.TopRightCell(newcell);
				// target cell is empty or is owned by the enemy piece
				if (newcell!=null && !newcell.IsOwned(source) ) 
					moves.Add(newcell);
			}
			// Now check 2nd bottom left and right cells
			newcell = m_Board.BottomCell(source);
			if (newcell!=null)
			{
				newcell = m_Board.BottomLeftCell(newcell);
				// target cell is empty or is owned by the enemy piece
				if (newcell!=null && !newcell.IsOwned(source) ) 
					moves.Add(newcell);

				newcell = m_Board.BottomCell(source);
				newcell = m_Board.BottomRightCell(newcell);
				// target cell is empty or is owned by the enemy piece
				if (newcell!=null && !newcell.IsOwned(source) ) 
					moves.Add(newcell);
			}
			// Now check 2nd Left Top and bottom cells
			newcell = m_Board.LeftCell(source);
			if (newcell!=null)
			{
				newcell = m_Board.TopLeftCell(newcell);
				// target cell is empty or is owned by the enemy piece
				if (newcell!=null && !newcell.IsOwned(source) ) 
					moves.Add(newcell);

				newcell = m_Board.LeftCell(source);
				newcell = m_Board.BottomLeftCell(newcell);
				// target cell is empty or is owned by the enemy piece
				if (newcell!=null && !newcell.IsOwned(source) ) 
					moves.Add(newcell);
			}
			// Now check 2nd Right Top and bottom cells
			newcell = m_Board.RightCell(source);
			if (newcell!=null)
			{
				newcell = m_Board.TopRightCell(newcell);
				// target cell is empty or is owned by the enemy piece
				if (newcell!=null && !newcell.IsOwned(source) ) 
					moves.Add(newcell);

				newcell = m_Board.RightCell(source);
				newcell = m_Board.BottomRightCell(newcell);
				// target cell is empty or is owned by the enemy piece
				if (newcell!=null && !newcell.IsOwned(source) ) 
					moves.Add(newcell);
			}
		}

		// calculate the possible moves for the Rook piece and insert them into passed array
		private void GetRookMoves(Cell source, ArrayList moves)
		{
			Cell newcell;

			// Check all the move squars available in top direction
			newcell = m_Board.TopCell(source);
			while (newcell!=null)	// move as long as cell is available in this direction
			{
				if (newcell.IsEmpty())	//next cell is available for move
					moves.Add(newcell);

				if (newcell.IsOwnedByEnemy(source))	//next cell is owned by the enemy object
				{
					moves.Add(newcell);	// Add this to available location
					break;	// force quite the loop execution
				}

				if (newcell.IsOwned(source))	//next cell contains owner object
					break;	// force quite the loop execution

				newcell = m_Board.TopCell(newcell); // keep moving in the top direction
			}

			// Check all the move squars available in left direction
			newcell = m_Board.LeftCell(source);
			while (newcell!=null)	// move as long as cell is available in this direction
			{
				if (newcell.IsEmpty())	//next cell is available for move
					moves.Add(newcell);

				if (newcell.IsOwnedByEnemy(source))	//next cell is owned by the enemy object
				{
					moves.Add(newcell);	// Add this to available location
					break;	// force quite the loop execution
				}

				if (newcell.IsOwned(source))	//next cell contains owner object
					break;	// force quite the loop execution

				newcell = m_Board.LeftCell(newcell); // keep moving in the left direction
			}

			// Check all the move squars available in right direction
			newcell = m_Board.RightCell(source);
			while (newcell!=null)	// move as long as cell is available in this direction
			{
				if (newcell.IsEmpty())	//next cell is available for move
					moves.Add(newcell);

				if (newcell.IsOwnedByEnemy(source))	//next cell is owned by the enemy object
				{
					moves.Add(newcell);	// Add this to available location
					break;	// force quite the loop execution
				}

				if (newcell.IsOwned(source))	//next cell contains owner object
					break;	// force quite the loop execution

				newcell = m_Board.RightCell(newcell); // keep moving in the right direction
			}

			// Check all the move squars available in bottom direction
			newcell = m_Board.BottomCell(source);
			while (newcell!=null)	// move as long as cell is available in this direction
			{
				if (newcell.IsEmpty())	//next cell is available for move
					moves.Add(newcell);

				if (newcell.IsOwnedByEnemy(source))	//next cell is owned by the enemy object
				{
					moves.Add(newcell);	// Add this to available location
					break;	// force quite the loop execution
				}

				if (newcell.IsOwned(source))	//next cell contains owner object
					break;	// force quite the loop execution

				newcell = m_Board.BottomCell(newcell); // keep moving in the bottom direction
			}
		}

		// calculate the possible moves for the bishop piece and insert them into passed array
		private void GetBishopMoves(Cell source, ArrayList moves)
		{
			Cell newcell;

			// Check all the move squars available in top-left direction
			newcell = m_Board.TopLeftCell(source);
			while (newcell!=null)	// move as long as cell is available in this direction
			{
				if (newcell.IsEmpty())	//next cell is available for move
					moves.Add(newcell);

				if (newcell.IsOwnedByEnemy(source))	//next cell is owned by the enemy object
				{
					moves.Add(newcell);	// Add this to available location
					break;	// force quite the loop execution
				}

				if (newcell.IsOwned(source))	//next cell contains owner object
					break;	// force quite the loop execution

				newcell = m_Board.TopLeftCell(newcell); // keep moving in the top-left direction
			}

			// Check all the move squars available in top-right direction
			newcell = m_Board.TopRightCell(source);
			while (newcell!=null)	// move as long as cell is available in this direction
			{
				if (newcell.IsEmpty())	//next cell is available for move
					moves.Add(newcell);

				if (newcell.IsOwnedByEnemy(source))	//next cell is owned by the enemy object
				{
					moves.Add(newcell);	// Add this to available location
					break;	// force quite the loop execution
				}

				if (newcell.IsOwned(source))	//next cell contains owner object
					break;	// force quite the loop execution

				newcell = m_Board.TopRightCell(newcell); // keep moving in the top-right direction
			}

			// Check all the move squars available in bottom-left direction
			newcell = m_Board.BottomLeftCell(source);
			while (newcell!=null)	// move as long as cell is available in this direction
			{
				if (newcell.IsEmpty())	//next cell is available for move
					moves.Add(newcell);

				if (newcell.IsOwnedByEnemy(source))	//next cell is owned by the enemy object
				{
					moves.Add(newcell);	// Add this to available location
					break;	// force quite the loop execution
				}

				if (newcell.IsOwned(source))	//next cell contains owner object
					break;	// force quite the loop execution

				newcell = m_Board.BottomLeftCell(newcell); // keep moving in the bottom-left direction
			}

			// Check all the move squars available in the bottom-right direction
			newcell = m_Board.BottomRightCell(source);
			while (newcell!=null)	// move as long as cell is available in this direction
			{
				if (newcell.IsEmpty())	//next cell is available for move
					moves.Add(newcell);

				if (newcell.IsOwnedByEnemy(source))	//next cell is owned by the enemy object
				{
					moves.Add(newcell);	// Add this to available location
					break;	// force quite the loop execution
				}

				if (newcell.IsOwned(source))	//next cell contains owner object
					break;	// force quite the loop execution

				newcell = m_Board.BottomRightCell(newcell); // keep moving in the bottom-right direction
			}
		}

		// calculate the possible moves for the queen piece and insert them into passed array
		private void GetQueenMoves(Cell source, ArrayList moves)
		{
			// Queen has moves combination of both bishop and rook moves
			GetRookMoves(source, moves); // first get moves for the rook
			GetBishopMoves(source, moves); // then get moves for the bishop
		}

		// calculate the possible moves for the king piece and insert them into passed array
		private void GetKingMoves(Cell source, ArrayList moves)
		{
			Cell newcell;

			// King can move to any of it's neighbor cells at the distance of one cell
			
			// check if king can move to top
			newcell = m_Board.TopCell(source);
			if (newcell!=null && !newcell.IsOwned(source)) // target cell is empty or is owned by the enemy piece
				moves.Add(newcell);
			// check if king can move to left
			newcell = m_Board.LeftCell(source);
			if (newcell!=null && !newcell.IsOwned(source)) // target cell is empty or is owned by the enemy piece
				moves.Add(newcell);
			// check if king can move to right
			newcell = m_Board.RightCell(source);
			if (newcell!=null && !newcell.IsOwned(source)) // target cell is empty or is owned by the enemy piece
				moves.Add(newcell);
			// check if king can move to bottom
			newcell = m_Board.BottomCell(source);
			if (newcell!=null && !newcell.IsOwned(source)) // target cell is empty or is owned by the enemy piece
				moves.Add(newcell);
			// check if king can move to top-left
			newcell = m_Board.TopLeftCell(source);
			if (newcell!=null && !newcell.IsOwned(source)) // target cell is empty or is owned by the enemy piece
				moves.Add(newcell);
			// check if king can move to top-right
			newcell = m_Board.TopRightCell(source);
			if (newcell!=null && !newcell.IsOwned(source)) // target cell is empty or is owned by the enemy piece
				moves.Add(newcell);
			// check if king can move to bottom-left
			newcell = m_Board.BottomLeftCell(source);
			if (newcell!=null && !newcell.IsOwned(source)) // target cell is empty or is owned by the enemy piece
				moves.Add(newcell);
			// check if king can move to bottom-right
			newcell = m_Board.BottomRightCell(source);
			if (newcell!=null && !newcell.IsOwned(source)) // target cell is empty or is owned by the enemy piece
				moves.Add(newcell);

			// Check castling or tower moves for the king
			if (m_Board[source].piece.Moves == 0)
			{
				Cell CastlingTarget=null;	// The cell where king will be moved in case of castling

				// As king has not yet moved, so castling is possible
				newcell = m_Board.RightCell(source);
				if (newcell!=null && newcell.IsEmpty())	// cell is empty
				{
					if (!CauseCheck(new Move(source, newcell))) // Inbetween cell is not under check
					{
                        newcell = m_Board.RightCell(newcell);
						if (newcell!=null && newcell.IsEmpty())	// cell is empty
						{
							CastlingTarget = newcell;	// This will be the king destination position
                            newcell = m_Board.RightCell(newcell);
							if (newcell!=null && !newcell.IsEmpty()  && newcell.piece.Moves==0)	// check if the rook piece has not yet moved
								moves.Add(CastlingTarget);	// Add this as possible move
						} 
					}
				}

				// Check on the left side
				newcell = m_Board.LeftCell(source);
				if (newcell!=null && newcell.IsEmpty())	// cell is empty
				{
					if (!CauseCheck(new Move(source, newcell))) // Inbetween cell is not under check
					{
                        newcell = m_Board.LeftCell(newcell);
						if (newcell!=null && newcell.IsEmpty())	// cell is empty
						{
							CastlingTarget = newcell;	// This will be the king destination position
                            newcell = m_Board.LeftCell(newcell);
							if (newcell!=null && newcell.IsEmpty())	// cell is empty
							{
                                newcell = m_Board.LeftCell(newcell);
								if (newcell!=null && !newcell.IsEmpty() && newcell.piece.Moves==0)	// check if the rook piece has not yet moved
									moves.Add(CastlingTarget);	// Add this as possible move
							}
						}

					}
				}
			}
		}

		// Analyze the board and return back the evualted score for the given side
        public int AnalyzeBoard(Side.SideType PlayerSide)
		{
			int Score=0;
			ArrayList OwnerCells = m_Board.GetSideCell(PlayerSide);
			
			// loop all the owner squars and get his king cell
			foreach (string ChessCell in OwnerCells)
			{
				Score+=m_Board[ChessCell].piece.GetWeight();
			}

			//int iPossibleMoves = GetCountOfPossibleMoves(PlayerSide);
			//Score+=iPossibleMoves*5; // Each mobility has 5 points
			return Score;
		}

		// Evaulate the current board position and return the evaluation score
		public int Evaluate(Side PlayerSide)
		{
			int Score=0;

			Score=AnalyzeBoard(PlayerSide.type)-AnalyzeBoard(PlayerSide.Enemy())-25;

			if (IsCheckMate(PlayerSide.Enemy()))	// If the player is check mate
				Score=1000000;

			return Score;
		}

	}
}
