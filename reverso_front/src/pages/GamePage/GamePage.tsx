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
            cellClass += gameState?.currentPlayerName === jsonData?.usernameJSON ? ' game-cell-playable' : '';
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
            <div style={{ position: 'fixed', top: 0, left: 0, width: '100%', height: '100%', backgroundColor: 'rgba(0, 0, 0, 0.5)', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                <div style={{ backgroundColor: 'white', padding: '20px', borderRadius: '5px', boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)' }}>
                    <h2>Game Over</h2>
                    <p>{`Winner: ${winner}`}</p>
                    <p>{`Score: ${Object.entries(points).map(([player, score]) => `${player}: ${score}`).join(', ')}`}</p>
                    <button className="game-button" onClick={() => navigate('/profile')}>Go to Profile</button>
                </div>
            </div>
        );
    };

    return (
        <div className="game-container">
            <h1 className="game-title">Game Page</h1>
            {renderModal()}
            {error && <p className="error">{error}</p>}
            <div className="game-board">
                {gameState?.field.map((row: string[], y: number) => (
                    <div key={y} style={{ display: 'flex' }}>
                        {row.map((cell, x) => renderCell(cell, x, y))}
                    </div>
                ))}
            </div>
            <div>Score - {gameState?.points && Object.keys(gameState.points).map(player => `${player}: ${gameState.points[player]}`).join(' ')} </div>
            <div>Current Player: {gameState?.currentPlayerName}</div>
            <div className="chat-section">
                <h3 className="chat-title">Chat</h3>
                <input className="chat-input" type="text" value={message} onChange={(e) => setMessage(e.target.value)} placeholder="Type a message..." />
                <button className="game-button" onClick={handleSendMessage}>Send</button>
                <div className="chat-messages">
                    {chat.slice().reverse().map((msg, index) => (
                        <p key={index}><strong>{msg.username}</strong> [{msg.time}]: {msg.text}</p>
                    ))}
                </div>
            </div>
        </div>
    );
}

export default GamePage;













