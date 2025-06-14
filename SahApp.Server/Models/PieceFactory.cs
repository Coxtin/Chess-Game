namespace SahApp.Server.Models
{
    public class PieceFactory
    {

        public static Piece PieceConstructors(Piece boardPiece)
        {
          
            return boardPiece.Type switch
            {

                PieceType.PAWN => new Pawn(boardPiece),
                PieceType.ROOK => new Rook(boardPiece),
                PieceType.BISHOP => new Bishop(boardPiece),
                PieceType.KNIGHT => new Knight(boardPiece),
                PieceType.KING => new King(boardPiece),
                PieceType.QUEEN => new Queen(boardPiece),
                PieceType.NONE => boardPiece,
                _ => boardPiece

            };

        }

    }
}
