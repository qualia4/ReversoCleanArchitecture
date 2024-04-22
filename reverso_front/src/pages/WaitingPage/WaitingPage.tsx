import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useMyContext } from '../../MyContext';
import { waitingForPlayerService } from "../../api/waitingForPlayerService";

const WaitingPage: React.FC = () => {
    const navigate = useNavigate();
    const { jsonData, updateJsonData } = useMyContext();
    const username = jsonData?.usernameJSON;
    const lobbyId = jsonData?.lobbyIDJSON;

    useEffect(() => {
        const fetchWaitingForPlayer = async () => {
            if (!lobbyId || !username) {
                navigate("/profile");
            } else {
                try {
                    await waitingForPlayerService(lobbyId);
                    navigate("/game");
                } catch (error) {
                    console.error('Failed to fetch:', error);
                    updateJsonData({usernameJSON: username});
                    navigate("/profile")
                }
            }
        };
        fetchWaitingForPlayer();
    }, [username, lobbyId, navigate]); // Added lobbyId to dependencies

    return (
        <div>
            <h1>Waiting for player...</h1>
            <h2>{username}</h2>
            <h2>{lobbyId}</h2>
        </div>
    );
}

export default WaitingPage;