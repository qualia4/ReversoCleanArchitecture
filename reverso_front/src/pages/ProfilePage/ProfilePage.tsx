import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useMyContext } from '../../MyContext';
import { playService } from "../../api/playService";
import { getStatsService } from "../../api/getStatsService";
import {waitingForPlayerService} from "../../api/waitingForPlayerService";
import './ProfilePage.css';

const ProfilePage: React.FC = () => {
    const navigate = useNavigate();
    const { jsonData, updateJsonData } = useMyContext();
    const username = jsonData?.usernameJSON;
    const [showStatsModal, setShowStatsModal] = useState(false);
    const [stats, setStats] = useState({ gamesPlayed: 0, draws: 0, gamesWon: 0, gamesLost: 0 });
    const [loading, setLoading] = useState(false);  // New state for loading

    useEffect(() => {
        if (!username) {
            navigate("/");
        }
    }, [username, navigate]);

    const handlePlayClick = async (gameType: string) => {
        setLoading(true);  // Start loading
        try {
            const data = await playService(username, gameType);
            updateJsonData({lobbyIDJSON: data.lobbyId, usernameJSON: username });
            if (data.gameStarted) {
                setLoading(false);
                navigate('/game');
            } else {
                await waitingForPlayerService(data.lobbyId);
                setLoading(false);
                navigate("/game");
            }
        } catch (err) {
            console.error(err);
            setLoading(false);  // Stop loading if there is an error
        }
    };

    const handleShowStatsClick = async () => {
        setLoading(true);  // Start loading
        try {
            const stats = await getStatsService(username);
            setStats(stats);
            setShowStatsModal(true);
        } catch (error) {
            console.error('Failed to fetch statistics:', error);
        }
        setLoading(false);  // Stop loading
    };

    const closeModal = () => {
        setShowStatsModal(false);
    };

    return (
        <div className="page-container">
            <h1 className="page-title">Main Menu</h1>
            <button className="button" onClick={() => handlePlayClick("pvp")}>Play PvP</button>
            <button className="button" onClick={() => handlePlayClick("pve")}>Play PvE</button>
            <button className="button" onClick={() => handlePlayClick("pvmm")}>Play Minimax</button>
            <button className="button" onClick={handleShowStatsClick}>Show Stats</button>
            {showStatsModal && (
                <div className="modal-background" onClick={closeModal}>
                    <div className="modal" onClick={e => e.stopPropagation()}>
                        <h2 className="statistics-title">Statistics for {username}</h2>
                        <p>Games Played: {stats.gamesPlayed}</p>
                        <p>Games Won: {stats.gamesWon}</p>
                        <p>Games Lost: {stats.gamesLost}</p>
                        <p>Draws: {stats.draws}</p>
                        <button className="button" onClick={closeModal}>OK</button>
                    </div>
                </div>
            )}
            {loading && (
                <div className="modal-background">
                    <div className="loader"></div>
                </div>
            )}
        </div>
    );
}

export default ProfilePage;


