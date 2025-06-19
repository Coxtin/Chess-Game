using System.Text.Json.Serialization;

namespace SahApp.Server
{
    public class MoveRequest
    {
        [JsonPropertyName("piece")]
       public Piece piece { get; set; }
        [JsonPropertyName("fromI")]
        public int FromI { get; set; }
        [JsonPropertyName("fromJ")]
        public int FromJ { get; set; }
        [JsonPropertyName("toI")]
        public int ToI { get; set; }
        [JsonPropertyName("toJ")]
        public int ToJ { get; set; }
        [JsonPropertyName("boardState")]
        public Piece[][] BoardState { get; set; }


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
            this.ToJ = toJ;
        }

        public Piece GetPiece() => piece;
        public int GetFromI() => FromI;
        public int GetFromJ() => FromJ;
        public int GetToI() => ToI;
        public int GetToJ() => ToJ;
    }

    /*public class PiecePosition
    {
        public int I { get; set; }
        public int J { get; set; }
        public Piece Piece { get; set; }
    }*/
}
