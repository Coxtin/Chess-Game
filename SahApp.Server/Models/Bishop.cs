namespace SahApp.Server.Models
{
    public class Bishop : Piece
    {

        public Bishop(Piece boardPiece) : base(boardPiece.Type, boardPiece.Color, boardPiece.Image) { }
        public Bishop(PieceType type, PieceColor color) : base(type, color) { }
        public Bishop(PieceColor color, string image) : base(PieceType.BISHOP, color, image) { }

        public override bool IsValid(Board board, int fromI, int fromj, int toI, int toJ)
        {
            return base.IsValid(board, fromI, fromj, toI, toJ);
        }

    }
}
