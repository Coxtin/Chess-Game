namespace SahApp.Server.Models
{
    public class Rook : Piece
    {

        public Rook(Piece boardPiece) : base(boardPiece.Type, boardPiece.Color, boardPiece.Image) { }
        public Rook(PieceType type, PieceColor color) : base(type, color) { }
        public Rook(PieceColor color, string image) : base(PieceType.ROOK, color, image) { }

        public override bool IsValid(Board board, int fromI, int fromJ, int toI, int toJ)
        {

            if (fromI == toI && fromJ == toJ)
                return false;



            return true;
        }

    }
}
