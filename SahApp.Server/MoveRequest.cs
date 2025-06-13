namespace SahApp.Server
{
    public class MoveRequest
    {

       private Piece piece { get; set; }
        private int FromI { get; set; }
        private int FromJ { get; set; }
        private int ToI { get; set; }
        private int ToJ { get; set; }


        public MoveRequest()
        {
            this.piece = new Piece();
            this.FromI = 0;
            this.FromJ = 0;
            this.ToI = 0; 
            this.ToJ = 0;
        }

        public MoveRequest(Piece piece, int fromI, int fromJ, int toI, int toJ)
        {
            this.piece = piece;
            this.FromI = fromI;
            this.FromJ = fromJ;
            this.ToI = toI;
            this.FromJ = toJ;
        }

        public Piece GetPiece() => piece;
        public int GetFromI() => FromI;
        public int GetFromJ() => FromJ;
        public int GetToI() => ToI;
        public int GetToj() => ToJ;
    }
}
