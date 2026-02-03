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
            Console.Write(color);
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
               message = $"{player1.ToString()} si {player2.ToString()}",
               player1 = player1.GetName(),
               player2 = player2.GetName(),
               color1 = player1.GetColor() ? "white" : "black",
               color2 = player2.GetColor() ? "white" : "black"
            });

        }

    }
}
