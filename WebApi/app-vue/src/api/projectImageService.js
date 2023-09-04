import axios from "@/utils/axios";

const projectImageService = {

    async getAllByProjectId(id) {
        try {
            return await axios.get(`/api/project-images/getAll/${id}`);
        }
        catch (error){
            console.log(error)
        }
    },

    async createProjectImage(projectImage ,image) {
        try {
            return await axios.post('/api/project-images', projectImage ,image);
        }
        catch (error){
            console.log(error)
        }
    },

    async deleteProjectImage(id) {
        try {
            return await axios.delete(`/api/project-images/${id}`)
        } catch (error) {
            console.log(error)
        }

    }
};
export default projectImageService;