import {Addresses} from "./Addresses";

export const playService = async (username: string, gametype: string) => {
    try {
        const response = await fetch(`${Addresses.REVERSO}/play`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                username: username,
                gameType: gametype
            })
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const data = await response.json();
        console.log(data);
        return data;
    } catch (error) {
        console.error('There was an error!', error);
        throw error;
    }
}