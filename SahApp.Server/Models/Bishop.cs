namespace SahApp.Server.Models
{
    public class Bishop : Piece
    {
        public Bishop(Piece boardPiece) : base(boardPiece.Type, boardPiece.Color, boardPiece.Image) { }
        public Bishop(PieceType type, PieceColor color) : base(type, color) { }
        public Bishop(PieceColor color, string image) : base(PieceType.BISHOP, color, image) { }

        public override bool IsValid(Board board, int fromI, int fromJ, int toI, int toJ)
        {
            if (board == null) return false;

            if (toI < 0 || toI >= 8 || toJ < 0 || toJ >= 8) return false;

            int diffI = Math.Abs(toI - fromI);
            int diffJ = Math.Abs(toJ - fromJ);

            if (diffI != diffJ) return false;

            int stepI = (toI > fromI) ? 1 : -1;
            int stepJ = (toJ > fromJ) ? 1 : -1;

            int currentI = fromI + stepI;
            int currentJ = fromJ + stepJ;

            while (currentI != toI)
            {
                if (board.pieces[currentI, currentJ].Type != PieceType.NONE)
                    return false; 
                currentI += stepI;
                currentJ += stepJ;
            }

            Piece target = board.pieces[toI, toJ];

            if (target.Type == PieceType.NONE || target.Color != this.Color)
            {
                return true;
            }

            return false;
        }
    }
}
