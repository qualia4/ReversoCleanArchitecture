import { Addresses } from "./Addresses";

export const makeMoveService = async (username: string, coordX: number, coordY: number,) => {
    try {
        const response = await fetch(`${Addresses.REVERSO}/makeMove`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                username: username,
                coordinateX: coordX,
                coordinateY: coordY
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