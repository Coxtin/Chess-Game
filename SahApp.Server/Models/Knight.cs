namespace SahApp.Server.Models
{
    public class Knight : Piece
    {

        public Knight(Piece boardPiece) : base(boardPiece.Type, boardPiece.Color, boardPiece.Image) { }
        public Knight(PieceType type, PieceColor color) : base(type, color) { } 
        public Knight(PieceColor color, string image) : base(PieceType.KNIGHT, color, image) { }

        public override bool IsValid(Board board, int fromI, int fromj, int toI, int toJ)
        {
            return base.IsValid(board, fromI, fromj, toI, toJ);
        }
    }
}
