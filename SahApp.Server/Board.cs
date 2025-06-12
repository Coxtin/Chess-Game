using System.IO.Pipelines;

namespace SahApp.Server
{
    public class Board
    {

        public Piece[,] pieces; 

        public Board()
        {
            pieces = new Piece[8, 8];
            InitializeBoard();
        }

        public void Print()
        {
            for(int row = 0; row < 8; row++)
            {
                for(int col = 0; col < 8; col++)
                {
                    Console.Write(pieces[row, col]);
                }
                Console.WriteLine();
            }
        }

        private void InitializeBoard()
        {

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    pieces[row, col] = new Piece(PieceType.NONE, PieceColor.NONE);

                }
            }
            // piesele negre
            pieces[0, 0] = new Piece(PieceType.ROOK, PieceColor.BLACK, "../ChessPieces/rook_b.png");
            pieces[0, 1] = new Piece(PieceType.KNIGHT, PieceColor.BLACK, "../ChessPieces/knight_b.png");
            pieces[0, 2] = new Piece(PieceType.BISHOP, PieceColor.BLACK, "../ChessPieces/bishop_b.png");
            pieces[0, 3] = new Piece(PieceType.QUEEN, PieceColor.BLACK, "../ChessPieces/queen_b.png");
            pieces[0, 4] = new Piece(PieceType.KING, PieceColor.BLACK, "../ChessPieces/king_b.png");
            pieces[0, 5] = new Piece(PieceType.BISHOP, PieceColor.BLACK, "../ChessPieces/bishop_b.png");
            pieces[0, 6] = new Piece(PieceType.KNIGHT, PieceColor.BLACK, "../ChessPieces/knight_b.png");
            pieces[0, 7] = new Piece(PieceType.ROOK, PieceColor.BLACK, "../ChessPieces/rook_b.png");

            for (int i = 0; i < 8; i++)
            {
                pieces[1, i] = new Piece(PieceType.PAWN, PieceColor.BLACK, "../ChessPieces/pawn_b.png");
            }

            //piesele albe

            pieces[7, 0] = new Piece(PieceType.ROOK, PieceColor.WHITE, "../ChessPieces/rook_w.png");
            pieces[7, 1] = new Piece(PieceType.KNIGHT, PieceColor.WHITE, "../ChessPieces/knight_w.png");
            pieces[7, 2] = new Piece(PieceType.BISHOP, PieceColor.WHITE, "../ChessPieces/bishop_w.png");
            pieces[7, 3] = new Piece(PieceType.QUEEN, PieceColor.WHITE, "../ChessPieces/queen_w.png");
            pieces[7, 4] = new Piece(PieceType.KING, PieceColor.WHITE, "../ChessPieces/king_w.png");
            pieces[7, 5] = new Piece(PieceType.BISHOP, PieceColor.WHITE, "../ChessPieces/bishop_w.png");
            pieces[7, 6] = new Piece(PieceType.KNIGHT, PieceColor.WHITE, "../ChessPieces/knight_w.png");
            pieces[7, 7] = new Piece(PieceType.ROOK, PieceColor.WHITE, "../ChessPieces/rook_w.png");

            for (int i = 0; i < 8; i++)
            {
                pieces[6, i] = new Piece(PieceType.PAWN, PieceColor.WHITE, "../ChessPieces/pawn_w.png");
            }

        }

    }
}
