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
        public PieceType Type { get; set; }
        public PieceColor Color { get; set; }

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

        public char GetPieceSymbol()
        {
            char symbol = Type switch
            {

                PieceType.KING => 'K',
                PieceType.QUEEN => 'Q',
                PieceType.KNIGHT => 'N',
                PieceType.BISHOP => 'B',
                PieceType.ROOK => 'R',
                PieceType.NONE => '/',
                _ => '?'

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
    }
}
