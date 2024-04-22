import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useMyContext } from '../../MyContext';
import { playService } from "../../api/playService";
import { getStatsService } from "../../api/getStatsService";

const ProfilePage: React.FC = () => {
    const navigate = useNavigate();
    const { jsonData, updateJsonData } = useMyContext();
    const username = jsonData?.usernameJSON;
    const [showStatsModal, setShowStatsModal] = useState(false);
    const [stats, setStats] = useState({ gamesPlayed: 0, draws: 0, gamesWon: 0, gamesLost: 0 });

    useEffect(() => {
        if (!username) {
            navigate("/");
        }
    }, [username, navigate]);

    const handlePlayClick = async (gameType: string) => {
        try {
            const data = await playService(username, gameType);
            updateJsonData({lobbyIDJSON: data.lobbyId, usernameJSON: username });
            if (data.gameStarted) {
                navigate('/game');
            } else {
                navigate('/waiting');
            }
        } catch (err) {
            console.error(err);
        }
    };

    const handleShowStatsClick = async () => {
        try {
            const stats = await getStatsService(username);
            setStats(stats);
            setShowStatsModal(true);
        } catch (error) {
            console.error('Failed to fetch statistics:', error);
        }
    };

    const closeModal = () => {
        setShowStatsModal(false);
    };

    return (
        <div>
            <h1>Main Menu</h1>
            <p>
                <button onClick={() => handlePlayClick("pvp")}>Play PvP</button>
            </p>
            <p>
                <button onClick={() => handlePlayClick("pve")}>Play PvE</button>
            </p>
            <p>
                <button onClick={handleShowStatsClick}>Show Stats</button>
            </p>
            {showStatsModal && (
                <div style={{ position: 'fixed', top: '20%', left: '50%', transform: 'translate(-50%, -50%)', backgroundColor: 'white', padding: '20px', zIndex: 1000 }}>
                    <h2>Statistics for {username}</h2>
                    <p>Games Played: {stats.gamesPlayed}</p>
                    <p>Games Won: {stats.gamesWon}</p>
                    <p>Games Lost: {stats.gamesLost}</p>
                    <p>Draws: {stats.draws}</p>
                    <button onClick={closeModal}>OK</button>
                </div>
            )}
            {showStatsModal && (
                <div style={{ position: 'fixed', top: 0, left: 0, right: 0, bottom: 0, backgroundColor: 'rgba(0, 0, 0, 0.5)' }} onClick={closeModal}></div>
            )}
        </div>
    );
}

export default ProfilePage;
