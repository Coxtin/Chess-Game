namespace SahApp.Server.Models
{
    public class Knight : Piece
    {
        public Knight(Piece boardPiece) : base(boardPiece.Type, boardPiece.Color, boardPiece.Image) { }
        public Knight(PieceType type, PieceColor color) : base(type, color) { } 
        public Knight(PieceColor color, string image) : base(PieceType.KNIGHT, color, image) { }

        public override bool IsValid(Board board, int fromI, int fromJ, int toI, int toJ)
        {
            if (board == null) return false;

            if (toI < 0 || toI >= 8 || toJ < 0 || toJ >= 8) return false;

            int dI = Math.Abs(toI - fromI);
            int dJ = Math.Abs(toJ - fromJ);

            bool isLMove = (dI == 2 && dJ == 1) || (dI == 1 && dJ == 2);

            if (!isLMove) return false;

            Piece target = board.pieces[toI, toJ];

            if (target.Type == PieceType.NONE || target.Color != this.Color)
            {
                return true;
            }

            return false;
        }
    }
}
