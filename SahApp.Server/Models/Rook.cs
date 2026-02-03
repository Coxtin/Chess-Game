namespace SahApp.Server.Models
{
    public class Rook : Piece
    {
        public Rook(Piece boardPiece) : base(boardPiece.Type, boardPiece.Color, boardPiece.Image) { }
        public Rook(PieceType type, PieceColor color) : base(type, color) { }
        public Rook(PieceColor color, string image) : base(PieceType.ROOK, color, image) { }

        public override bool IsValid(Board board, int fromI, int fromJ, int toI, int toJ)
        {
            if (board == null) return false;

            if (toI < 0 || toI >= 8 || toJ < 0 || toJ >= 8) return false;
            
            if (toJ != fromJ && toI != fromI) return false;

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

            return false;
        }
    }
}
