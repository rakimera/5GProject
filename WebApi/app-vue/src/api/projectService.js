import axios from "@/utils/axios";
import storeExtension from "@/utils/storeExtension";

const projectService = {
    async getProjects() {
        try {
            return await axios.get(`/api/projects`);
        }
        catch (error){
            console.log(error)
        }
    },

    async createProject(user) {
        try {
            return await axios.post('/api/projects', user);
        }
        catch (error){
            console.log(error)
        }

    },

    async updateProject(user) {
        try {
            return await axios.put('/api/projects', user)
        }
        catch (error){
            console.log(error)
        }
    },

    async deleteProject(oid) {
        try {
            return await axios.delete(`/api/projects/${oid}`)
        }
        catch (error){
            console.log(error)
        }

    },

    async getProjectsForGrid(loadOptions) {
        try {
            let options = storeExtension.getParams(loadOptions);
            const response = await axios.get(`/api/projects/Index/${options}`);
            console.log(response)
            return response.data;
        } catch (error) {
            console.log(error)
        }
    }
};
export default projectService;