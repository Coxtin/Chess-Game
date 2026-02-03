using System.Text.Json.Serialization;

namespace SahApp.Server
{

    public enum PieceType
    {
        KING, QUEEN, ROOK, BISHOP, KNIGHT, PAWN, NONE

    }

    public enum PieceColor
    {
        BLACK, WHITE, NONE
    }
    public class Piece
    {
        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PieceType Type { get; set; }

        [JsonPropertyName("color")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PieceColor Color { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }
        public Piece(PieceType _type , PieceColor _color)
        {
            this.Type = _type;
            this.Color = _color;
            this.Image = "";
        }

        public Piece() { 
        
            this.Type = PieceType.NONE;
            this.Color = PieceColor.NONE;
            this.Image = "";
        
        }

        public Piece(PieceType _type, PieceColor _color, string _image) : this(_type, _color)
        {
            Image = _image;
        }

        public string GetPieceSymbol()
        {
            string symbol = Type switch
            {

                PieceType.KING => "KING",
                PieceType.QUEEN => "QUEEN",
                PieceType.KNIGHT => "KNIGHT",
                PieceType.BISHOP => "BISHOP",
                PieceType.ROOK => "ROOK",
                PieceType.PAWN => "Pawn",
                PieceType.NONE => "/",
                _ => "?"

            };
            return symbol;
        }

        public char GetPieceColor()
        {
            char color = Color switch
            {
                PieceColor.BLACK => 'B',
                PieceColor.WHITE => 'W',
                PieceColor.NONE => 'N',
                _ => '?'
            };

            return color;
        }

        public string GetPieceImage()
        {
            return Image;
        }
        public virtual bool IsValid(Board board, int fromI, int fromj, int toI, int toJ) { return false; }
        //public virtual bool isKingInCheck(Board board, PieceColor color) { return false; }
    }
}
