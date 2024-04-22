import React, {useState} from 'react';
import { useMyContext } from '../../MyContext';
import { useNavigate } from 'react-router-dom';
import {registerService} from "../../api/registerService";
import {loginService} from "../../api/loginService";
import { CSSProperties } from 'react';

const LoginPage : React.FC = () => {
    const navigate = useNavigate();
    const { updateJsonData } = useMyContext();
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [displayedText, setDisplayedText] = useState('');

    const handleLoginClick = async () => {
        try {
            await loginService(username, password);
            updateJsonData({
                usernameJSON: username,
            })
            navigate('profile')
        } catch (error) {
            if (error instanceof Error) { // Type-safe error handling
                setDisplayedText("Login failed. Wrong username or password"); // Display error message
            } else {
                setDisplayedText('An unexpected error occurred');
            }
        }
    };

    const handleRegisterClick = async () => {
        try {
            await registerService(username, password);
            updateJsonData({
                usernameJSON: username,
            })
            navigate('profile')
        } catch (error) {
            if (error instanceof Error) { // Type-safe error handling
                setDisplayedText("Register failed. Username already exists"); // Display error message
            } else {
                setDisplayedText('An unexpected error occurred');
            }
        }
    };

    return (
        <div>
            <h1>Login Page</h1>
            <p></p>
            <input
                type="text"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                placeholder="Username"
            />
            <p></p>
            <input
                type="text"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="Password"
            />
            <p>
                <button onClick={handleLoginClick}>Login</button>
            </p>
            <p>
                <button onClick={handleRegisterClick}>Register</button>
            </p>
            {displayedText &&<p>{displayedText}</p>}
        </div>

    );
}

export default LoginPage;