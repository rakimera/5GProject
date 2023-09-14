import axios from "@/utils/axios";
import storeExtension from "@/utils/storeExtension";

const radiationZoneService = {

    async getRadiationZonesForGrid(loadOptions, id) {
        try {
            let options = storeExtension.getParams(loadOptions);
            const response = await axios.get(`/api/radiationZones/index/translator/${id}/${options}`);
            console.log(response)
            return response.data;
        } catch (error) {
            console.log(error)
        }
    }
};
export default radiationZoneService;