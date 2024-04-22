import {Addresses} from "./Addresses";

export const getGameInfo = async (lobbyId: string) => {
    try
    {
        const response = await fetch(`${Addresses.REVERSO}/getGameInfo/${lobbyId}`);
        if(!response.ok){
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