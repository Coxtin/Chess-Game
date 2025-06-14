namespace SahApp.Server.Models
{
    public class Queen : Piece
    {

        public Queen(Piece boardPiece) : base(boardPiece.Type, boardPiece.Color, boardPiece.Image) { }
        public Queen(PieceType type, PieceColor color) : base(type, color) { }
        public Queen(PieceColor color, string image) : base(PieceType.QUEEN, color, image) { }

        public override bool IsValid(Board board, int fromI, int fromj, int toI, int toJ)
        {
            return base.IsValid(board, fromI, fromj, toI, toJ);
        }

    }
}
