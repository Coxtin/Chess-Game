using System.Drawing;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using SahApp.Server.Models;
using SahApp.Server.Utilities;

namespace SahApp.Server.Controllers
{

    [ApiController]
    [Route("verify")]
    public class VerifyController : ControllerBase
    {

        private readonly ILogger<VerifyController> _logger;
        private readonly IGameLogger _fileLogger; 

        public VerifyController(ILogger<VerifyController> logger)
        {
            _logger = logger;
            _fileLogger = new FileLogger();
        }

        [HttpPost("move")]

        public ActionResult VerifyMove([FromBody] MoveRequest moveRequest)
        {

            try
            {

                _logger.LogInformation("Received Move Information");

                if (moveRequest == null)
                {
                    throw new InvalidChessDataException("MoveRequest este null!");
                }
                if (moveRequest.piece == null)
                {
                    throw new InvalidChessDataException("Piesa este nula!");
                }

                _logger.LogInformation($"Piesa + coordonate: {moveRequest}");

                Func<int, bool> isValidCoordinate = coord => coord >= 0 && coord < 8;

                if (!isValidCoordinate(moveRequest.GetFromI()) || !isValidCoordinate(moveRequest.GetToI()))
                {
                    _logger.LogWarning("Coordinates outside valid range.");
                    return Ok(new { valid = false });
                }

                var board = new Board();

                if (moveRequest.BoardState != null)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (moveRequest.BoardState[i][j] != null)
                                board.pieces[i, j] = moveRequest.BoardState[i][j];
                            else
                                board.pieces[i, j] = new Piece(PieceType.NONE, PieceColor.NONE);
                        }
                    }
                }

                var piece = board.pieces[moveRequest.GetFromI(), moveRequest.GetFromJ()];

                if (piece.Type != moveRequest.GetPiece().Type || !Equals(piece.Color.ToString().ToLower(), moveRequest.GetPiece().Color.ToString().ToLower()))
                {
                    return BadRequest("Piesa selectata este necunoscuta!");
                }

                var PieceInstance = PieceFactory.PieceConstructors(piece);

                bool isValidMove = PieceInstance.IsValid(board, moveRequest.GetFromI(), moveRequest.GetFromJ(), moveRequest.GetToI(), moveRequest.GetToJ());
                _logger.LogInformation($"valoare isValidMove este {isValidMove}");

                if (isValidMove)
                {

                    _fileLogger.LogMove($"Move validated: {piece.Color} {piece.Type} from [{moveRequest.GetFromI()},{moveRequest.GetFromJ()}] to [{moveRequest.GetToI()},{moveRequest.GetToJ()}]");

                    board.pieces[moveRequest.GetToI(), moveRequest.GetToJ()] = board.pieces[moveRequest.GetFromI(), moveRequest.GetFromJ()];
                    board.pieces[moveRequest.GetFromI(), moveRequest.GetFromJ()] = new Piece(PieceType.NONE, PieceColor.NONE);

                    PieceColor opponentColor = (moveRequest.GetPiece().Color == PieceColor.WHITE) ? PieceColor.BLACK : PieceColor.WHITE;
                    bool isCheck = VerifyCheck.isKingInCheck(board, opponentColor);

                    if (isCheck)
                    {
                        Console.WriteLine("ESTI IN CHECK !");
                        _fileLogger.LogMove($"CHECK DETECTED against {opponentColor}");
                    }

                    Console.WriteLine();

                    return Ok(new { valid = true, updatedBoard = ConvertBoardToJaggedArray(ReverseBoard(board)) });
                }
                else
                {
                    return Ok(new { valid = false });
                }
            }
            
            catch (InvalidChessDataException customEx)
            {
                _logger.LogError(customEx, "Custom Data Error");
                return BadRequest(new { error = customEx.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in VerifyMove");
                return BadRequest(new { error = $"Eroare la controller: {e.Message}" });
            }
        }


        private static Piece[][] ConvertBoardToJaggedArray(Board board)
        {
            var result = new Piece[8][];
            for (int i = 0; i < 8; i++)
            {
                result[i] = new Piece[8];
                for (int j = 0; j < 8; j++)
                {
                    result[i][j] = board.pieces[i, j];
                }
            }
            return result;
        }

        private static Board ReverseBoard(Board board){

            Board reversedBoard = new Board();

            for (int i = 7; i >= 0; i--){
                for (int j = 7; j >= 0; j--){
                    reversedBoard.pieces[7 - i, 7 - j] = board.pieces[i, j];
                }
            }

            return reversedBoard;

        }
    }
}