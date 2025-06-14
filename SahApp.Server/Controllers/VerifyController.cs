using Microsoft.AspNetCore.Mvc;
using SahApp.Server.Models;

namespace SahApp.Server.Controllers
{

    [ApiController]
    [Route("verify")]
    public class VerifyController : ControllerBase
    {

        private readonly ILogger<VerifyController> _logger;

        public VerifyController(ILogger<VerifyController> logger)
        {
            _logger = logger;
        } 

        [HttpPost("move")]

        public ActionResult VerifyMove([FromBody] MoveRequest moveRequest)
        {

            try
            {

                _logger.LogInformation("Received Move Information");

                if (moveRequest == null)
                {
                    _logger.LogWarning("MoveRequest este null");
                    return BadRequest(new { error = "MoveRequest este null" });
                }

                if (moveRequest.piece == null)
                {
                    _logger.LogWarning("Piesa este nula");
                    return BadRequest(new { error = "Piesa este nula" });
                }

                _logger.LogInformation($"Piesa + coordonate: {moveRequest}");   

                var board = new Board();
            
                var piece = board.pieces[moveRequest.GetFromI(), moveRequest.GetFromJ()];

                if (piece.Type != moveRequest.GetPiece().Type || !Equals(piece.Color.ToString().ToLower(), moveRequest.GetPiece().Color.ToString().ToLower()))
                {
                    return BadRequest("Piesa selectata este necunoscuta!");
                }

                var PieceInstance = PieceFactory.PieceConstructors(piece);

                bool isValidMove = PieceInstance.IsValid(board, moveRequest.GetFromI(), moveRequest.GetFromJ(), moveRequest.GetToI(), moveRequest.GetToJ());
                _logger.LogInformation($"valoare isValidMove este {isValidMove}");

                if (isValidMove)
                    return Ok(new { valid = true });
                else
                    return Ok(new { valid = false });
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error in VerifyMove");
                return BadRequest(new {error = $"Eroare la controller: {e.Message}"});
            }
            
        }
    }
    
}
