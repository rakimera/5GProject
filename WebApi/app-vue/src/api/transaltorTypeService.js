import axios from '../utils/axios';
import storeExtension from "@/utils/storeExtension";

const translatorTypeService = {
    async getTranslatorTypes() {
        try {
            return await axios.get(`/api/translatorTypes`);
        }
        catch (error){
            console.log(error)
        }
    },

    async getTranslatorType(id) {
        try {
            return await axios.get(`/api/translatorTypes/${id}`);
        }
        catch (error){
            console.log(error)
        }
    },
    async createTranslatorType(translatorType) {
        try {
            return await axios.post('/api/translatorTypes', translatorType);
        }
        catch (error){
            console.log(error)
        }

    },

    async updateTranslatorType(translatorType) {
        try {
            return await axios.put('/api/translatorTypes', translatorType)
        }
        catch (error){
            console.log(error)
        }
    },

    async deleteTranslatorType(id) {
        try {
            return await axios.delete(`/api/translatorTypes/${id}`)
        }
        catch (error){
            console.log(error)
        }

    },

    async getAllTranslatorTypes(loadOptions) {
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

export default translatorTypeService;