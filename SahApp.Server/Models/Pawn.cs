namespace SahApp.Server.Models
{
    public class Pawn : Piece
    {
        public Pawn(Piece boardPiece) : base(boardPiece.Type, boardPiece.Color, boardPiece.Image) { }

        public override bool IsValid(Board board, int fromI, int fromJ, int toI, int toJ)
        {
            if (board == null) return false;

            int direction = (toI - fromI > 0) ? 1 : -1;

            if (toI < 0 || toI >= 8 || toJ < 0 || toJ >= 8) return false;

            if (fromI == toI && fromJ == toJ) return false;

            if (fromJ == toJ && toI == fromI + direction && board.pieces[toI, toJ].Type == PieceType.NONE)
                return true;

            if (toI == fromI + direction && toJ == fromJ - direction)
            {
                Piece target = board.pieces[toI, toJ];
                if (target.Type != PieceType.NONE && target.Color != this.Color)
                    return true;
            }
            
            if (toI == fromI + direction && toJ == fromJ + direction)
            {
                Piece target = board.pieces[toI, toJ];
                if (target.Type != PieceType.NONE && target.Color != this.Color)
                    return true;
            }
            
            // 3. Optional: Initial 2-square move logic (if you want to implement it later)
            // int startRow = (direction == 1) ? 1 : 6; // Rough guess for standard board
            
            return false;
        }
    }
}
