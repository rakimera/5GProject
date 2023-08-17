import axios from "@/utils/axios";
import storeExtension from "@/utils/storeExtension";

const antennaService = {
    async getAntennae() {
        try {
            return await axios.get(`/api/antennae`);
        }
        catch (error){
            console.log(error)
        }
    },

    async createAntenna(antenna) {
        try {
            return await axios.post('/api/antennae', antenna);
        }
        catch (error){
            console.log(error)
        }

    },

    async updateAntenna(antenna) {
        try {
            return await axios.put('/api/antennae', antenna)
        }
        catch (error){
            console.log(error)
        }
    },

    async deleteAntenna(oid) {
        try {
            return await axios.delete(`/api/antennae/${oid}`)
        }
        catch (error){
            console.log(error)
        }

    },

    async getAntennaeForGrid(loadOptions) {
        try {
            let options = storeExtension.getParams(loadOptions);
            const response = await axios.get(`/api/antennae/index/${options}`);
            console.log(response)
            return response.data;
        } catch (error) {
            console.log(error)
        }
    }
};
export default antennaService;