namespace SahApp.Server.Models
{
    public class Rook : Piece
    {

        public Rook(Piece piece)
        {
            this.Type = piece.Type;
            this.Color = piece.Color;
            this.Image = piece.Image;
        }

        public override bool IsValid(Board board, int fromI, int fromJ, int toI, int toJ)
        {

            if (fromI == toI && fromJ == toJ)
                return false;



            return true;
        }

    }
}
