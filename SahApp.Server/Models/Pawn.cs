namespace SahApp.Server.Models
{
    public class Pawn : Piece
    {

        public Pawn(Piece piece)
        {
            this.Type = piece.Type; 
            this.Color = piece.Color;
            this.Image = piece.Image;
        }

        public override bool IsValid(Board board, int fromI, int fromJ, int toI, int toJ)
        {
            
            int direction = (Color == PieceColor.WHITE) ? 1 : -1;

            if (board == null) return false;

            if (fromI == toI && fromJ == toJ) return false;

            if (fromJ == toJ && toI == fromI + direction && board.pieces[toI, toJ].Type == PieceType.NONE)
                return true;

            return true;

        }

    }
}
