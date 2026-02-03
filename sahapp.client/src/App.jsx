import { useLocation } from 'react-router-dom';
import React, { useState, useEffect, useRef } from 'react';
import './App.css';

const App = () => {

    //----------------- DECLARARE VARIABILE -------------------------

    const location = useLocation();
    const { player1, player2, color1, color2 } = location.state || {};
    const [currentPlayer, setCurrentPlayer] = useState(color1.toLowerCase() == "white" ? player1 : player2);
    const [currentColor, setCurrentColor] = useState("white");

    //console.log("Date initializare joc", {
    //player1, player2, color1, color2});

    const [tiles, setTiles] = useState([]);
    const [pieces, setPieces] = useState([]);

    const piecesRef = useRef(pieces);

    useEffect(() => {
        piecesRef.current = pieces;
    }, [pieces]);

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
        const handleMouseMoveWrapper = (e) => handleMouseMove(e);
        const handleMouseUpWrapper = (e) => handleMouseUp(e);

        document.addEventListener('mousemove', handleMouseMoveWrapper);
        document.addEventListener('mouseup', handleMouseUpWrapper);

        return () => {
            document.removeEventListener('mousemove', handleMouseMoveWrapper);
            document.removeEventListener('mouseup', handleMouseUpWrapper);
        };

    }, [draggedPiece, draggedPieceIndex, dragOffset]); 

    //-------------------FUNCTIA DE INITIALIZARE TABLA DE SAH CU PRELUARE DIN BACKEND A PIESELOR----------------------

    const initializeBoard = async () => {
        try {
            console.log("Initializing board...");
            const response = await fetch("https://localhost:7122/board/state", {
                method: "GET",
                headers: {
                    //'Content-Type': 'application/json',
                    'Accept': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error(`eroare cu exitcode: ${response.status}`);
            }

            const pieceData = await response.json();
            
            // Normalize the data to match i/j format instead of row/col
            const formattedPieces = pieceData.map(p => ({
                ...p,
                // Map row/col from server to i/j used in frontend
                i: p.row !== undefined ? p.row : p.i, 
                j: p.col !== undefined ? p.col : p.j,
                // Ensure types are lowercase to match updatePieceFromBoard logic
                type: p.type ? p.type.toLowerCase() : p.type,
                color: p.color ? p.color.toLowerCase() : p.color
            }));

            setPieces(formattedPieces);
        }
        catch (err) {
            console.error("am primit eroare de la board: ", err);
        }
    }

    //-------------------------CONTROL AL MOUSE_ULUI------------------

    const handleMouseDown = (e, piece, index) => {

        if (piece.color !== currentColor)
            return;

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
    }

    const handleMouseUp = async (e) => {

        if (draggedPieceIndex === null) return;

        const boardRect = document.querySelector(".chessboard").getBoundingClientRect();

        const dropX = e.clientX - boardRect.left;
        const dropY = e.clientY - boardRect.top;

        const newI = Math.floor(dropY / tileSize);
        const newJ = Math.floor(dropX / tileSize);

        // Check bounds
        if (newI < 0 || newI >= 8 || newJ < 0 || newJ >= 8) {
            resetMove();
            setDraggedPiece(null);
            setDraggedPieceIndex(null);
            return;
        }

        const currentPieces = piecesRef.current;
        const piece = currentPieces[draggedPieceIndex];
        
        const moveResult = await verifyMove(piece, piece.i, piece.j, newI, newJ);

        if (!moveResult.isValid) {
            console.log("mutare invalida");
            resetMove();
        } else {
            console.log(`Mutare valida! Noile coordonate: i=${newI}, j=${newJ}`);
            if (moveResult.updatedBoard) {
                updatePieceFromBoard(moveResult.updatedBoard);
            }
            
            const nextColor = currentColor === "white" ? "black" : "white";
            const nextPlayer = currentPlayer === player1 ? player2 : player1;
            setCurrentPlayer(nextPlayer);
            setCurrentColor(nextColor);
        }

        setDraggedPiece(null);
        setDraggedPieceIndex(null);
        setDragOffset({ x: 0, y: 0 });
    };

    //--------------------------FUNCTIA CARE IMI VERIFICA MUTARIILE-----------------------

    const verifyMove = async (piece, fromI, fromJ, toI, toJ) => {
        
        try {
            const currentPieces = piecesRef.current;
            const boardState = Array(8).fill(null).map(() => Array(8).fill(null));
            
            // Build the board state from current pieces
            currentPieces.forEach(p => {
                // Ensure p.i and p.j exist before using them
                if (p.i !== undefined && p.j !== undefined) {
                    boardState[p.i][p.j] = {
                        type: p.type.toUpperCase(),
                        color: p.color.toUpperCase(),
                        image: p.image
                    };
                }
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
                boardState: boardState,
                //currentColor: currentColor
            };

            const response = await fetch("https://localhost:7122/verify/move", {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify(requestPayload)
            });

            if (!response.ok) {
                const errorText = await response.text();
                return { isValid: false, updatedBoard: null };
            }

            const data = await response.json();
            return {
                isValid: data.valid === true,
                updatedBoard: data.updatedBoard || null
            };
        }
        catch (err) {
            console.error("Move validation error:", err);
            return { isValid: false, updatedBoard: null };
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
                // Handle different possible casing from server ("NONE" or "none")
                if (piece && 
                    piece.type.toUpperCase() !== "NONE" && 
                    piece.color.toUpperCase() !== "NONE") {
                    
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
               
                { tiles }

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