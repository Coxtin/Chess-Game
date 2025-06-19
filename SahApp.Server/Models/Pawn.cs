namespace SahApp.Server.Models
{
    public class Pawn : Piece
    {

        public Pawn(Piece boardPiece) : base(boardPiece.Type, boardPiece.Color, boardPiece.Image) { }
        public Pawn(PieceType type, PieceColor color) : base(type, color) { }

        public override bool IsValid(Board board, int fromI, int fromJ, int toI, int toJ)
        {
            
            int direction = (Color == PieceColor.WHITE) ? -1 : 1;

            if (board == null) return false;

            if (toI < 0 || toI >= 8 || toJ < 0 || toJ >= 8) return false;

            if (fromI == toI && fromJ == toJ) return false;

            if (fromJ == toJ && toI == fromI + direction && board.pieces[toI, toJ].Type == PieceType.NONE)
                return true;

            if (toI == fromI + direction && toJ == fromJ - direction && board.pieces[toI, toJ].Color == PieceColor.BLACK)
            {
                
                board.pieces[toI, toJ] = new Piece();
                return true;

            }            
            
            return false;

        }

    }
}
