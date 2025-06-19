import { useLocation } from 'react-router-dom';
import { useState, useEffect } from 'react';
import './App.css';

const App = () => {

    //----------------- DECLARARE VARIABILE -------------------------

    const location = useLocation();
    const { player1, player2, color1, color2 } = location.state || {};
    const [currentPlayer, setCurrentPlayer] = useState(color1 == "white" ? player1 : player2);
    const [currentColor, setCurrentColor] = useState(color1 == "white" ? "white" : "black");

    const [tiles, setTiles] = useState([]);
    const [pieces, setPieces] = useState([]);

    const tileSize = 100;

    const [draggedPiece, setDraggedPiece] = useState(null);
    const [draggedPieceIndex, setDraggedPieceIndex] = useState(null);
    const [dragOffset, setDragOffset] = useState({ x: 0, y: 0 });
    


    //---------------------INITIALIZARE TABLA DE SAH----------------------

    useEffect(() => {
        const tempBoard = [];

        for (let i = 0; i < 8; i++) {
            for (let j = 0; j < 8; j++) {
                const isWhite = (i + j) % 2 === 0;
                tempBoard.push(
                    <div
                        key={`${i}-${j}`}
                        className={`tile ${isWhite ? "white-tile" : "black-tile"}`}
                    />
                );
            }
        }

        setTiles(tempBoard);
        initializeBoard();
    }, []);


    useEffect(() => {
        document.addEventListener('mousemove', handleMouseMove);
        document.addEventListener('mouseup', handleMouseUp);

        return () => {
            document.removeEventListener('mousemove', handleMouseMove);
            document.removeEventListener('mouseup', handleMouseUp);
        };

    }, [draggedPiece, draggedPieceIndex, dragOffset, pieces]); 

    //-------------------FUNCTIA DE INITIALIZARE TABLA DE SAH CU PRELUARE DIN BACKEND A PIESELOR----------------------

    const initializeBoard = async () => {
        try {
            const response = await fetch("https://localhost:7122/board/state", {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error(`eroare cu exitcode: ${response.status}`);
            }

            const pieceData = await response.json();
            setPieces(pieceData);

        }
        catch (err) {
            console.error("am primit eroare de la board: ", err);
        }
    }

    //-------------------------CONTROL AL MOUSE_ULUI------------------

    const handleMouseDown = (e, piece, index) => {

        if (piece.color !== currentColor)
            return;

        console.log("am apasat click pe piesa:", piece, "la index:", index);

        const rect = e.target.getBoundingClientRect();
        setDraggedPiece(piece);
        setDraggedPieceIndex(index);
        setDragOffset({
            x: e.clientX - rect.left,
            y: e.clientY - rect.top
        });
    };

    const handleMouseMove = (e) => {
        if (draggedPieceIndex === null) return;

        console.log("misc mouse-ul pentru piesa la index:", draggedPieceIndex);

        const boardRect = document.querySelector(".chessboard").getBoundingClientRect();
        const newX = e.clientX - boardRect.left - dragOffset.x;
        const newY = e.clientY - boardRect.top - dragOffset.y;

        setPieces(prevPieces => {
            const updatedPieces = [...prevPieces];
            updatedPieces[draggedPieceIndex] = {
                ...updatedPieces[draggedPieceIndex],
                pixelX: newX,
                pixelY: newY
            };
            return updatedPieces;
        });

        setDraggedPiece(prevPiece => ({
            ...prevPiece,
            pixelX: newX,
            pixelY: newY
        }));
    }

    const handleMouseUp = async (e) => {

        if (draggedPieceIndex === null) return;

        console.log("am ridicat click-ul pentru piesa la index:", draggedPieceIndex);

        const boardRect = document.querySelector(".chessboard").getBoundingClientRect();

        const dropX = e.clientX - boardRect.left;
        const dropY = e.clientY - boardRect.top;

        const newI = Math.floor(dropY / tileSize);
        const newJ = Math.floor(dropX / tileSize);

        const piece = pieces[draggedPieceIndex];
        const isValidMove = await verifyMove(piece, piece.i, piece.j, newI, newJ);

        if (!isValidMove.isValid) {
            console.log("mutare invalida");
            resetMove();
            setDraggedPiece(null);
            setDraggedPieceIndex(null);
            setDragOffset({ x: 0, y: 0 });
            return;
        }


        console.log(`Noile coordonate: i=${newI}, j=${newJ}`);

        
        if (newI >= 0 && newI < 8 && newJ >= 0 && newJ < 8) {
            setPieces(prevPieces => {
                const updatedPieces = [...prevPieces];
                updatedPieces[draggedPieceIndex] = {
                    ...updatedPieces[draggedPieceIndex],
                    i: newI,
                    j: newJ,
                    pixelX: undefined, 
                    pixelY: undefined
                };
                return updatedPieces;
            });


        } else {
            resetMove();
        }

        if (isValidMove.updatedBoard) {
            updatePieceFromBoard(isValidMove.updatedBoard);
        }

        setDraggedPiece(null);
        setDraggedPieceIndex(null);
        setDragOffset({ x: 0, y: 0 });

        const nextColor = currentColor === "white" ? "black" : "white";
        const nextPlayer = currentPlayer === player1 ? player2 : player1;

        setCurrentPlayer(nextPlayer);
        setCurrentColor(nextColor);
    };

    //--------------------------FUNCTIA CARE IMI VERIFICA MUTARIILE-----------------------

    const verifyMove = async (piece, fromI, fromJ, toI, toJ) => {
        console.log("=== verifyMove START ===");
        console.log("Input parameters:", {
            piece: piece,
            fromI: fromI,
            fromJ: fromJ,
            toI: toI,
            toJ: toJ
        });

        try {

            const boardState = Array(8).fill(null).map(() => Array(8).fill(null));
            pieces.forEach(p => {
                boardState[p.i][p.j] = {
                    type: p.type.toUpperCase(),
                    color: p.color.toUpperCase(),
                    image: p.image
                };
            });

            const requestPayload = {
                piece: {
                    type: piece.type.toUpperCase(),
                    color: piece.color.toUpperCase(),
                    image: piece.image
                },
                fromI: fromI,
                fromJ: fromJ,
                toI: toI,
                toJ: toJ,
                boardState: boardState 
            };
            console.log("Request payload:", JSON.stringify(requestPayload, null, 2));

            console.log("Sending request to: https://localhost:7122/verify/move");

            const response = await fetch("https://localhost:7122/verify/move", {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify(requestPayload)
            });

            console.log("Response received:");
            console.log("- Status:", response.status);
            console.log("- Status Text:", response.statusText);
            console.log("- Headers:", Object.fromEntries(response.headers.entries()));

            const resultText = await response.text();
            console.log("raw response", resultText);

            if (!response.ok) {
                console.error("Response not OK! Status:", response.status, "StatusText:", response.statusText);
                console.error("Error response body: ", resultText);
                throw new Error(`HTTP error! status:  ${response.status}, status text: ${response.statusText}`);
            }

            console.log("mut piesa", piece);

            let data;
            try {
                data = JSON.parse(resultText);
            }
            catch (err) {
                console.error("Failed to parse : ", err);
                console.error("Response was : ", responseText);
                throw new Error("Invalid JSON response from server");
            }

            console.log("Attempting to parse response as JSON...");
            console.log("Parsed response data:", data);
            console.log("data.valid:", data.valid);
            console.log("data.valid === true:", data.valid === true);

            const result = {

                isValid: data.valid === true,
                updatedBoard: data.updatedBoard || null

            };

            console.log("Final result:", result);
            console.log("=== verifyMove END (SUCCESS) ===");

            return result;
        }
        catch (err) {
            console.error("=== ERROR CAUGHT ===");
            console.error("Error type:", err.constructor.name);
            console.error("Error message:", err.message);

            if (err instanceof TypeError) {
                console.error("This is a TypeError - likely network/fetch issue");
            }
            if (err instanceof SyntaxError) {
                console.error("This is a SyntaxError - likely JSON parsing issue");
            }

            console.error("Full error object:", err);
            console.error("Error stack:", err.stack);

            // Additional network-specific debugging
            if (err.message.includes('fetch')) {
                console.error("Fetch-related error detected");
                console.error("Possible causes:");
                console.error("- Server not running on https://localhost:7122");
                console.error("- CORS issues");
                console.error("- SSL certificate issues (localhost with https)");
                console.error("- Network connectivity issues");
            }

            console.log("=== verifyMove END (ERROR) ===");
            return {isValid : false, updatedBoard : null};
        }
    }

    const resetMove = () => {
        setPieces(prevPieces => {
            const updatedPieces = [...prevPieces];
            updatedPieces[draggedPieceIndex] = {
                ...updatedPieces[draggedPieceIndex],
                pixelX: undefined,
                pixelY: undefined,
            }
            return updatedPieces;
        })
    };

    const updatePieceFromBoard = (boardState) => {

        const newPieces = [];

        for (let i = 0; i < 8; i++) {
            for (let j = 0; j < 8; j++) {
                var piece = boardState[i][j];
                if (piece && piece.type !== "NONE" && piece.color !== "NONE") {
                    newPieces.push({
                        type: piece.type.toLowerCase(),
                        color: piece.color.toLowerCase(),
                        image: piece.image,
                        i: i,
                        j: j
                    });
                }
            }
        }
        setPieces(newPieces);
    };

    //---------------------------RETURN-UL CARE AFISEAZA IN FRONTEND-------------------

    return (
        <div className="container">
            <p>Este randul tau <strong> {currentPlayer} </strong> si ai culoarea {currentColor}</p>

            <div className="chessboard">
               
                {tiles}

                {pieces.map((piece, index) => {
                    const isDragging = draggedPieceIndex === index;
                    const allowedDragging = piece.color === currentColor;
                    const top = isDragging && piece.pixelY !== undefined ? piece.pixelY : piece.i * tileSize;
                    const left = isDragging && piece.pixelX !== undefined ? piece.pixelX : piece.j * tileSize;

                    return (
                        <img
                            key={index}
                            src={`${piece.image}`}
                            alt={piece.type}
                            draggable={false}
                            className="piece"
                            onMouseDown={(e) => handleMouseDown(e, piece, index)}
                            style={{
                                top: `${top}px`,
                                left: `${left}px`,
                                cursor: isDragging ? "grabbing" : allowedDragging ? "grab" : "not-allowed",
                                zIndex: isDragging ? 10 : 1,
                                position: "absolute"
                            }}
                        />
                    );
                })}
            </div>
        </div>
    );
};

export default App;