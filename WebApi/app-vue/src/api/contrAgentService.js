import axios from '../utils/axios';
import storeExtension from "@/utils/storeExtension";

const contrAgentService = {
    async getContrAgents() {
        try {
            return await axios.get(`/api/contrAgents`);
        }
        catch (error){
            console.log(error)
        }
    },
    async createContrAgent(contrAgent) {
        try {
            return await axios.post('/api/contrAgents', contrAgent);
        }
        catch (error){
            console.log(error)
        }

    },

    async updateContrAgent(contrAgent) {
        try {
            return await axios.put('/api/contrAgents', contrAgent)
        }
        catch (error){
            console.log(error)
        }
    },

    async deleteContrAgent(oid) {
        try {
            return await axios.delete(`/api/contrAgents/${oid}`)
        }
        catch (error){
            console.log(error)
        }

    },

    async getAllContrAgents(loadOptions) {
        try {
            let options = storeExtension.getParams(loadOptions);
            const response = await axios.get(`/api/contrAgents/Index/${options}`);
            console.log(response)
            return response.data;
        } catch (error) {
            console.log(error)
        }
    }
};

export default contrAgentService;