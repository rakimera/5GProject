import axios from '../utils/axios';
import storeExtension from "@/utils/storeExtension";

const townService = {
    async getTowns() {
        try {
            return await axios.get(`/api/towns`);
        }
        catch (error){
            console.log(error)
        }
    },

    async getTown(oid) {
        try {
            return await axios.get(`/api/towns/${oid}`);
        }
        catch (error){
            console.log(error)
        }
    },
    async createTown(town) {
        try {
            return await axios.post('/api/towns', town);
        }
        catch (error){
            console.log(error)
        }

    },

    async updateTown(town) {
        try {
            return await axios.put('/api/towns', town)
        }
        catch (error){
            console.log(error)
        }
    },

    async deleteTown(oid) {
        try {
            return await axios.delete(`/api/towns/${oid}`)
        }
        catch (error){
            console.log(error)
        }

    },

    async getAllTowns(loadOptions) {
        try {
            let options = storeExtension.getParams(loadOptions);
            const response = await axios.get(`/api/towns/Index/${options}`);
            console.log(response)
            return response.data;
        } catch (error) {
            console.log(error)
        }
    }
};

export default townService;