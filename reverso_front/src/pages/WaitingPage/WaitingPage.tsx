import React, {useState} from 'react';
import { useMyContext } from '../../MyContext';
import { useNavigate } from 'react-router-dom';
import { CSSProperties } from 'react';

const WaitingPage = () => {
    return (
        <div>Waiting for player...</div>
    );
}

export default WaitingPage;