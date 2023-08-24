import axios from "@/utils/axios";
import storeExtension from "@/utils/storeExtension";

const translatorSpecsService = {
    async getTranslatorSpecs() {
        try {
            return await axios.get(`/api/translators`);
        }
        catch (error){
            console.log(error)
        }
    },

    async getTranslatorSpec(oid) {
        try {
            return await axios.get(`/api/translators/${oid}`);
        }
        catch (error){
            console.log(error)
        }
    },

    async createTranslatorSpec(translatorSpec) {
        try {
            return await axios.post('/api/translators', translatorSpec);
        }
        catch (error){
            console.log(error)
        }
    },

    async updateTranslatorSpec(translatorSpec) {
        try {
            return await axios.put('/api/translators', translatorSpec)
        }
        catch (error){
            console.log(error)
        }
    },

    async deleteTranslatorSpec(oid) {
        try {
            return await axios.delete(`/api/translators/${oid}`)
        }
        catch (error){
            console.log(error)
        }

    },

    async getTranslatorSpecsForGrid(loadOptions) {
        try {
            let options = storeExtension.getParams(loadOptions);
            const response = await axios.get(`/api/translators/index/${options}`);
            console.log(response)
            return response.data;
        } catch (error) {
            console.log(error)
        }
    }
};
export default translatorSpecsService;