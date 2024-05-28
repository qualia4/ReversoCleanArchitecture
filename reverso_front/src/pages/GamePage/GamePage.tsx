import React, { useEffect, useState } from 'react';
import { useMyContext } from '../../MyContext';
import { useNavigate } from 'react-router-dom';
import { getGameInfo } from "../../api/getGameInfo";
import { makeMoveService } from "../../api/makeMoveService";
import { getChatService } from "../../api/getChatService";
import { writeMessageService } from "../../api/writeMessageService";
import './GamePage.css';

interface ChatMessage {
    username: string;
    text: string;
    time: string;
}

const GamePage: React.FC = () => {
    const navigate = useNavigate();
    const { jsonData } = useMyContext();
    const [gameState, setGameState] = useState<any>(null);
    const [chat, setChat] = useState<ChatMessage[]>([]);
    const [message, setMessage] = useState("");
    const [error, setError] = useState<string | null>(null);
    const [intervalId, setIntervalId] = useState<NodeJS.Timeout | null>(null);

    useEffect(() => {
        const fetchGameInfo = async () => {
            try {
                const data = await getGameInfo(jsonData.lobbyIDJSON);
                setGameState(data);
            } catch (err) {
                console.error(err);
                setError("Failed to fetch game info.");
            }
        };

        const fetchChat = async () => {
            try {
                const chatData = await getChatService(jsonData.lobbyIDJSON);
                setChat(chatData);
            } catch (err) {
                console.error("Failed to fetch chat.", err);
            }
        };

        if (!jsonData?.lobbyIDJSON || !jsonData?.usernameJSON) {
            navigate("/profile");
            return;
        }

        fetchGameInfo();
        fetchChat();
        const id = setInterval(() => {
            fetchGameInfo();
            fetchChat();
        }, 500);
        setIntervalId(id);

        // Cleanup function to clear the interval when component unmounts
        return () => {
            clearInterval(id);
        };
    }, [jsonData, navigate]);

    const handleMove = async (x: number, y: number) => {
        if (!jsonData?.usernameJSON || gameState?.currentPlayerName !== jsonData.usernameJSON) return;
        try {
            await makeMoveService(jsonData.usernameJSON, y, x);
            setGameState(await getGameInfo(jsonData.lobbyIDJSON)); // Refetch immediately after making a move
        } catch (err) {
            console.error(err);
            setError("Failed to make move.");
        }
    };

    const handleSendMessage = async () => {
        if (message.trim() !== "") {
            try {
                await writeMessageService(jsonData.lobbyIDJSON, jsonData.usernameJSON, message);
                setMessage(""); // Clear message input after sending
                const chatData = await getChatService(jsonData.lobbyIDJSON);
                setChat(chatData);
            } catch (err) {
                console.error("Failed to send message.", err);
            }
        }
    };

    const renderCell = (cell: string, x: number, y: number) => {
        const playerKeys = gameState && Object.keys(gameState.points);
        const firstPlayer = playerKeys && playerKeys[0];

        // Determine the CSS class based on the cell value
        let cellClass = 'game-cell';
        if (cell === '*') {
            if(gameState?.currentPlayerName === jsonData?.usernameJSON){
                cellClass += ' game-cell-playable'
                if (gameState?.currentPlayerName === firstPlayer) {
                    cellClass += ' first-player-border';
                } else {
                    cellClass += ' second-player-border';
                }
            }
        } else if (cell !== '-' && cell !== '*') {
            cellClass += (cell === firstPlayer ? ' game-cell-first-player' : ' game-cell-second-player');
        }

        return (
            <button key={`${x}-${y}`} className={cellClass} onClick={() => cell === '*' && handleMove(x, y)}>
                {' '}
            </button>
        );
    };


    const renderModal = () => {
        if (!gameState || !gameState.gameEnded) return null;
        const points = gameState.points;
        const winner = Object.keys(points).reduce((a, b) => points[a] > points[b] ? a : b);
        return (
            <div className="modal-background">
                <div className="modal-content">
                    <h2 className="modal-header" style={{
                        backgroundColor: gameState && Object.keys(gameState.points || {})[0] === winner ? '#3498db' : '#e74c3c'
                    }}>
                        {winner === jsonData.usernameJSON ? "You won!" : `${winner} won!`}
                    </h2>
                    <h3>Score:</h3>
                    <p style={{ fontWeight: "bold" }}>{Object.entries(points).map(([player, score]) => `${player}: ${score}`).join(' ')}</p>
                    <button className="game-button" onClick={() => navigate('/profile')}>Go to Profile</button>
                </div>
            </div>
        );
    };


    return (
        <div className="game-container">
            <div className="game-stats">
                <h3>Score:</h3>
                <div className="score-container">
                    {gameState && Object.keys(gameState.points || {}).map((player, index) => (
                        <p key={player} className="score-box" style={{
                            backgroundColor: index === 0 ? '#3498db' : '#e74c3c',
                        }}>
                            {gameState.points[player]}
                        </p>
                    ))}
                    {gameState && Object.keys(gameState.points || {}).length === 2}
                </div>
            </div>
            {renderModal()}
            {error && <p className="error">{error}</p>}
            <div className="game-board">
                {gameState?.field.map((row: string[], y: number) => (
                    <div key={y} style={{ display: 'flex' }}>
                        {row.map((cell, x) => renderCell(cell, x, y))}
                    </div>
                ))}
            </div>
            <div className="game-stats">
                <p style={{
                    color: 'white',
                    padding: '5px',
                    borderRadius: '5px',
                    backgroundColor: gameState && Object.keys(gameState.points || {})[0] === gameState.currentPlayerName ? '#3498db' : '#e74c3c'
                }}>
                    {gameState?.currentPlayerName === jsonData.usernameJSON ? "Your turn" : `${gameState?.currentPlayerName}'s turn`}
                </p>
            </div>
            <div className="chat-section">
                <h3 className="chat-title">Chat</h3>
                <div className="chat-controls">
                    <input className="chat-input" type="text" value={message} onChange={(e) => setMessage(e.target.value)} placeholder="Type a message..." />
                    <button className="chat-button" onClick={handleSendMessage}>Send</button>
                </div>
                <div className="chat-messages">
                    {chat.slice().reverse().map((msg, index) => (
                        <p key={index}>
                            <span style={{
                                backgroundColor: (gameState && Object.keys(gameState.points || {})[0] === msg.username) ? '#3498db' : '#e74c3c',
                                color: 'white',
                                padding: '2px 4px',
                                borderRadius: '4px',
                                fontWeight: "normal"
                            }}>
                                {msg.username}
                            </span> [{msg.time}]: {msg.text}
                        </p>
                    ))}
                </div>
            </div>
        </div>
    );
}

export default GamePage;













