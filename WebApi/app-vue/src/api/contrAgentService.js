import axios from '../utils/axios';
import authHeader from "@/api/AuthHeader";

const header = await authHeader();
const contrAgentService = {
    getContrAgents() {
        return axios.get('/api/contrAgents', { headers: header});
    },
    createContrAgent(contrAgent) {
        return axios.post('/api/contrAgents', contrAgent, { headers: header});
    },
    updateContrAgent(contrAgent) {
        return axios.put('/api/contrAgents', contrAgent, { headers: header});
    },
    deleteContrAgent(oid) {
        return axios.delete(`/api/contrAgents/${oid}`, { headers: header});
    }
};

export default contrAgentService;