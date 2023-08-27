import axios from "@/utils/axios";
import storeExtension from "@/utils/storeExtension";

const radiationZoneService = {
    async getRadiationZones() {
        try {
            return await axios.get(`api/radiationZones`);
        }
        catch (error){
            console.log(error)
        }
    },

    async getRadiationZone(oid) {
        try {
            return await axios.get(`/api/radiationZones/${oid}`);
        }
        catch (error){
            console.log(error)
        }
    },

    async createRadiationZone(radiationZone) {
        try {
            return await axios.post('/api/radiationZones', radiationZone);
        }
        catch (error){
            console.log(error)
        }
    },

    async updateRadiationZone(radiationZone) {
        try {
            return await axios.put('/api/radiationZones', radiationZone)
        }
        catch (error){
            console.log(error)
        }
    },

    async deleteRadiationZone(oid) {
        try {
            return await axios.delete(`/api/radiationZones/${oid}`)
        }
        catch (error){
            console.log(error)
        }

    },

    async getRadiationZonesForGrid(loadOptions) {
        try {
            let options = storeExtension.getParams(loadOptions);
            const response = await axios.get(`/api/radiationZones/index/${options}`);
            console.log(response)
            return response.data;
        } catch (error) {
            console.log(error)
        }
    }
};
export default radiationZoneService;