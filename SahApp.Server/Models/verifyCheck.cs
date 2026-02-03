using SahApp.Server.Models;

namespace SahApp.Server.Models{

    public static class VerifyCheck{

        public static bool isKingInCheck(Board board, PieceColor kingColor) {

            int kingI = -1, kingJ = -1;

            for (int i = 0; i < 8; i++){
                for (int j = 0; j < 8; j++){
                    Piece p = board.pieces[i, j];
                    if (p.Type == PieceType.KING && p.Color == kingColor){
                        kingI = i;
                        kingJ = j;
                        break;
                    }
                }
            }

            PieceColor opponentColor = (kingColor == PieceColor.WHITE) ? PieceColor.BLACK : PieceColor.WHITE;

            Console.WriteLine($"Valorile pentru kingI si kingJ sunt: {kingI} si {kingJ} si culoarea este {opponentColor}");

            for (int i = 0; i < 8; i++){
                for (int j = 0; j < 8; j++){
                    Piece boardPiece = board.pieces[i, j];
                    if (boardPiece.Color == opponentColor && boardPiece.Type != PieceType.NONE){

                        Piece attacker = PieceFactory.PieceConstructors(boardPiece);
                        if (attacker.IsValid(board, i, j, kingI, kingJ)){
                            Console.WriteLine($"CHECK DETECTED! Attacker: {boardPiece.Type} ({boardPiece.Color}) at [{i},{j}] hits King at [{kingI},{kingJ}]");
                            return true;
                        }
                    }
                }
            }

            return false;
        }

    }

}