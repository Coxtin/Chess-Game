using Microsoft.AspNetCore.Mvc;

namespace SahApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {

        [HttpPost("request")]

        public IActionResult actionResult([FromBody] PlayerRequest request)
        {

            var color = new Random().Next(2);
            Player player1, player2;

            if (color == 1)
            {
                player1 = new Player(request.Player1, true);
                player2 = new Player(request.Player2, false);
            }
            else
            {
                player1 = new Player(request.Player1, false);
                player2 = new Player(request.Player2, true);
            }


            return Ok(new
            {
                message = $"bine ati venit bai {player1.GetName()} +  si {player2.GetName()} + {player2.GetColor()}",
                color1 = player1.GetColor() ? "white" : "black",
                color2 = player2.GetColor() ? "white" : "black"
            });

        }

    }
}
