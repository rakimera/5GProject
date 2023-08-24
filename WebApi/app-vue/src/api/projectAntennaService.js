import axios from "@/utils/axios";
import storeExtension from "@/utils/storeExtension";

const projectAntennaService = {
    async getProjectAntennae() {
        try {
            return await axios.get(`/api/projects-antenna`);
        } catch (error) {
            console.log(error)
        }
    },

    async getProjectAntenna(oid) {
        try {
            return await axios.get(`/api/projects-antenna/${oid}`);
        }
        catch (error){
            console.log(error)
        }
    },
    
    async getAllByProjectId(id) {
        try {
            return await axios.get(`/api/getAllFromThisProject/projects-antenna/${id}`);
        }
        catch (error){
            console.log(error)
        }
    },

    async createProjectAntenna(antenna) {
        try {
            return await axios.post('/api/projects-antenna', antenna);
        }
        catch (error){
            console.log(error)
        }
    },

    async updateProjectAntenna(antenna) {
        try {
            return await axios.put('/api/projects-antenna', antenna)
        } catch (error) {
            console.log(error)
        }
    },

    async deleteProjectAntenna(oid) {
        try {
            return await axios.delete(`/api/projects-antenna/${oid}`)
        } catch (error) {
            console.log(error)
        }

    },

    async getProjectAntennaeForGrid(loadOptions) {
        try {
            let options = storeExtension.getParams(loadOptions);
            const response = await axios.get(`/api/projects-antenna/index/${options}`);
            console.log(response)
            return response.data;
        } catch (error) {
            console.log(error)
        }
    }
};
export default projectAntennaService;