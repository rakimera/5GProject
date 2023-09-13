import axios from "@/utils/axios";

const radiationZoneExelFileService = {

    async getAllByTranslatorSpecId(id) {
        try {
            return await axios.get(`/api/radiationZone-files/${id}`);
        }
        catch (error){
            console.log(error)
        }
    },

    async createExelFile(uploadedFile) {
        try {
            return await axios.post('/api/radiationZone-files', uploadedFile);
        }
        catch (error){
            console.log(error)
        }
    },

    async deleteExelFile(id) {
        try {
            return await axios.delete(`/api/radiationZone-files/${id}`)
        } catch (error) {
            console.log(error)
        }

    }
};
export default radiationZoneExelFileService;