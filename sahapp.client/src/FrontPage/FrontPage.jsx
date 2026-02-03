import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './FrontPage-css/FrontPage.css'

const FrontPage = () => {

    const [Player1, setPlayer1] = useState('');
    const [Color1, setColor1] = useState('');
    const [Player2, setPlayer2] = useState('');
    const [Color2, setColor2] = useState('');


    const navigate = useNavigate();
  

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await fetch("https://localhost:7122/game/request", {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify({
                    player1: Player1,
                    player2: Player2,
                    color1: "",
                    color2: ""
                })
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const data = await response.json();
            console.log("am primit: ", data);

            //setPlayer1(Player1);
            //setPlayer2(Player2);

            navigate('/game', {
                state: {
                    player1: data.player1,
                    player2: data.player2,
                    color1: data.color1,
                    color2: data.color2
            }});
        } catch (err) {
            console.error("am primit eroarea la fetch: ", err);
        }
    }


  return (
    <div className="front-page">
      <h1>Welcome to SahApp</h1>
          <h3>Who will win?</h3>
          <div>
              <form onSubmit={handleSubmit}>

                <label htmlFor="nume-player1">Jucator 1: </label>
                  <input type="text"
                      name="nume-player1"
                      id="nume-player1"
                      placeholder="Nume jucator"
                      method="POST"
                      onChange={(e) => setPlayer1(e.target.value)}
                      required
                      >
                  </input> <br></br>
                <label htmlFor="nume-player2">Jucator 2: </label>
                  <input type="text"
                      name="nume-player2"
                      id="nume-player2"
                      placeholder="Nume jucator"
                      method="POST"
                      onChange={(e) => setPlayer2(e.target.value) }
                      required></input> <br></br>
                  <input type="submit" name="trimite" id="trimte" value="Intra in joc!" ></input> 

              </form>
          </div>
    </div>
  );
};

export default FrontPage;