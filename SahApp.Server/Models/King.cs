namespace SahApp.Server.Models
{
    public class King : Piece
    {

        public King(Piece boardPiece) : base(boardPiece.Type, boardPiece.Color, boardPiece.Image) { }
        public King(PieceType type, PieceColor color) : base(type, color) { }
        public King(PieceColor color, string image) : base(PieceType.KING, color, image) { }

        public override bool IsValid(Board board, int fromI, int fromJ, int toI, int toJ)
        {
            if (board == null) return false;

            if (toI < 0 || toI >= 8 || toJ < 0 || toJ >= 8) return false;

            if (fromI == toI && fromJ == toJ) return false;

            int diffI = Math.Abs(toI - fromI);
            int diffJ = Math.Abs(toJ - fromJ);

            if (diffI <= 1 && diffJ <=1){

                Piece target = board.pieces[toI, toJ];

                if (target.Color != this.Color || target.Type == PieceType.NONE)
                    return true;

            }

            return base.IsValid(board, fromI, fromJ, toI, toJ);
        }

    }
}
