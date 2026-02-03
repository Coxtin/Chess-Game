namespace SahApp.Server.Models
{
    public class Queen : Piece
    {

        public Queen(Piece boardPiece) : base(boardPiece.Type, boardPiece.Color, boardPiece.Image) { }
        public Queen(PieceType type, PieceColor color) : base(type, color) { }
        public Queen(PieceColor color, string image) : base(PieceType.QUEEN, color, image) { }

        public override bool IsValid(Board board, int fromI, int fromJ, int toI, int toJ)
        {

            if (board == null) return false;

            if (toI < 0 || toI >= 8 || toJ < 0 || toJ >= 8) return false;

            if (toI == fromI && toJ == fromJ) return false;

            int diffI = Math.Abs(toI - fromI);
            int diffJ = Math.Abs(toJ - fromJ);

            bool isDiagonal = (diffI == diffJ);
            bool isStraight = (fromI == toI || fromJ == toJ);

            if (isDiagonal)
            {

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
            } else if (isStraight){
                if (toJ == fromJ)
                {
                    int direction = (toI - fromI) > 0 ? 1 : -1;
                    for (var i = fromI + direction; i != toI; i += direction)
                        if (board.pieces[i, fromJ].Type != PieceType.NONE)
                            return false;
                }

                else if (toI == fromI)
                {
                    int direction = (toJ - fromJ) > 0 ? 1 : -1;
                    for (var j = fromJ + direction; j != toJ; j += direction)
                        if (board.pieces[fromI, j].Type != PieceType.NONE)
                            return false;
                }

                Piece target = board.pieces[toI, toJ];

                if (target.Type == PieceType.NONE || target.Color != this.Color)
                {
                    return true;
                }
            }

                return base.IsValid(board, fromI, fromJ, toI, toJ);
        }

    }
}
