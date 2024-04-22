import { Addresses } from "./Addresses";

export const writeMessageService = async (lobbyId: string, username: string, text: string) => {
    try {
        const response = await fetch(`${Addresses.REVERSO}/writeMessageToChat/${lobbyId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                username: username,
                text: text
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