using Microsoft.AspNetCore.Mvc;

namespace SahApp.Server.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class VerifyController : Controller
    {
        [HttpPost("verify")]


        public ActionResult VerifyMove([FromBody] MoveRequest moveRequest)
        {

            var board = new Board();

            var piece = board.pieces[moveRequest.GetFromI(), moveRequest.GetFromJ()];

            if (piece.Type != moveRequest.GetPiece().Type || !Equals(piece.Color.ToString().ToLower() , moveRequest.GetPiece().Color.ToString().ToLower()))
            {
                return BadRequest("Piesa selectata este necunoscuta!");
            }

            bool isValidMove = piece.IsValid(board, moveRequest.GetFromI(), moveRequest.GetFromJ(), moveRequest.GetToI(), moveRequest.GetToj());

            if (isValidMove)
                return Ok(new { valid = true });
            else
                return Ok(new { valid = false });
        }
    }
    
}
