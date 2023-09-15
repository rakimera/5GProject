import axios from "@/utils/axios";

const exportService = {
    async getEnergyFlow(id){
        try {
            return await axios.get(`/api/energy-flows/${id}`);
        }catch (error){
            console.log(error)
        }
    }
}

export default exportService;