import axios from "@/utils/axios";
import storeExtension from "@/utils/storeExtension";

const antennaTranslatorService = {
    async getAntennaTranslators() {
        try {
            return await axios.get(`/api/antenna-translator`);
        } catch (error) {
            console.log(error)
        }
    },

    async getAntennaTranslator(oid) {
        try {
            return await axios.get(`/api/antenna-translator/${oid}`);
        }
        catch (error){
            console.log(error)
        }
    },

    async getAllByProjectAntennaId(id) {
        try {
            return await axios.get(`/api/antenna-translator/getAll/${id}`);
        }
        catch (error){
            console.log(error)
        }
    },

    async createAntennaTranslator(antennaTranslator) {
        try {
            return await axios.post('/api/antenna-translator', antennaTranslator);
        }
        catch (error){
            console.log(error)
        }
    },

    async updateAntennaTranslator(antennaTranslator) {
        try {
            return await axios.put('/api/antenna-translator', antennaTranslator)
        } catch (error) {
            console.log(error)
        }
    },

    async deleteAntennaTranslator(oid) {
        try {
            return await axios.delete(`/api/antenna-translator/${oid}`)
        } catch (error) {
            console.log(error)
        }

    },

    async getAntennaTranslatorForGrid(loadOptions, id) {
        try {
            let options = storeExtension.getParams(loadOptions);
            const response = await axios.get(`/api/antenna-translator/index/${id}/${options}`);
            console.log(response)
            return response.data;
        } catch (error) {
            console.log(error)
        }
    }
};
export default antennaTranslatorService;