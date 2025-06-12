using Microsoft.AspNetCore.Mvc;

namespace SahApp.Server.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class BoardController : ControllerBase
    {

        [HttpPost("state")]

        public ActionResult GetBoardState()
        {

            var board = new Board();
            var pieceData = new List<object>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var piece = board.pieces[i, j];
                    if (piece.Type != PieceType.NONE)
                    {
                        pieceData.Add(new
                        {
                            i,
                            j,
                            type = piece.Type.ToString(),
                            color = piece.Color.ToString().ToLower(),
                            image = piece.Image
                        });
                    }
                }
            }
            return Ok(pieceData);
        }
    }
}
