import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useMyContext } from '../../MyContext';
import { loginService } from '../../api/loginService';
import { registerService } from '../../api/registerService';
import './LoginPage.css'; // Ensure the CSS file is correctly imported

const LoginPage: React.FC = () => {
    const navigate = useNavigate();
    const { updateJsonData } = useMyContext();
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [displayedText, setDisplayedText] = useState('');
    const [loading, setLoading] = useState(false);

    const handleLoginClick = async () => {
        setLoading(true);
        try {
            await loginService(username, password);
            updateJsonData({ usernameJSON: username });
            navigate('/profile');
        } catch (error) {
            setDisplayedText(error instanceof Error ? "Login failed. Wrong username or password" : "An unexpected error occurred");
        }
        setLoading(false);
    };

    const handleRegisterClick = async () => {
        setLoading(true);
        try {
            await registerService(username, password);
            updateJsonData({ usernameJSON: username });
            navigate('/profile');
        } catch (error) {
            setDisplayedText(error instanceof Error ? "Register failed. Username already exists" : "An unexpected error occurred");
        }
        setLoading(false);
    };

    return (
        <div className="login-container">
            <h2 className="login-title">Login Page</h2>
            <input
                className="login-input"
                type="text"
                value={username}
                onChange={e => setUsername(e.target.value)}
                placeholder="Username"
            />
            <input
                className="login-input"
                type="password"
                value={password}
                onChange={e => setPassword(e.target.value)}
                placeholder="Password"
            />
            <button className="login-button" onClick={handleLoginClick} disabled={loading}>
                {loading ? 'Loading...' : 'Log In'}
            </button>
            <button className="login-button" onClick={handleRegisterClick} disabled={loading}>
                {loading ? 'Loading...' : 'Register'}
            </button>
            {displayedText && <p className="error-message">{displayedText}</p>}
            {loading && <div className="loader"></div>}
        </div>
    );
}

export default LoginPage;



