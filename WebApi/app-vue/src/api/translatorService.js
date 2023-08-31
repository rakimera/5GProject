import axios from '../utils/axios';
import storeExtension from "@/utils/storeExtension";

const translatorService = {
    async getTranslators() {
        try {
            return await axios.get(`/api/translators`);
        }
        catch (error){
            console.log(error)
        }
    },

    async getTranslator(oid) {
        try {
            return await axios.get(`/api/translators/${oid}`);
        }
        catch (error){
            console.log(error)
        }
    },
    async createTranslators(town) {
        try {
            return await axios.post('/api/translators', town);
        }
        catch (error){
            console.log(error)
        }

    },

    async updateTranslators(town) {
        try {
            return await axios.put('/api/translators', town)
        }
        catch (error){
            console.log(error)
        }
    },

    async deleteTown(oid) {
        try {
            return await axios.delete(`/api/translators/${oid}`)
        }
        catch (error){
            console.log(error)
        }

    },

    async getAllByAntennaId(id) {
        try {
            return await axios.get(`/api/translators/getAll/${id}`);
        }
        catch (error){
            console.log(error)
        }
    },

    async getAllTowns(loadOptions) {
        try {
            let options = storeExtension.getParams(loadOptions);
            const response = await axios.get(`/api/translators/Index/${options}`);
            console.log(response)
            return response.data;
        } catch (error) {
            console.log(error)
        }
    }
};

export default translatorService;