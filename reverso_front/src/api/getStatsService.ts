import {Addresses} from "./Addresses";

export const getStatsService = async (username: string) => {
    try
    {
        const response = await fetch(`${Addresses.REVERSO}/getUserInfo/${username}`);
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