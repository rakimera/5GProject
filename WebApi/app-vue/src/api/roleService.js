import axios from '../utils/axios';
import storeExtension from "@/utils/storeExtension";

const roleService = {

    async getRoles() {
        try {
            return await axios.get(`/api/roles`);
        } catch (error) {
            console.log(error)
        }
    },

    async createRole(role) {
        try {
            return await axios.post('/api/roles', role);
        } catch (error) {
            console.log(error);
        }
    },

    async updateRole(role) {
        try {
            return await axios.put(`/api/roles/`, role);
        } catch (error) {
            console.log(error);
        }
    },

    async deleteRole(oid) {
        try {
            return await axios.delete(`/api/roles/${oid}`);
        } catch (error) {
            console.log(error);
        }
    },

    async getAllRoles(loadOptions) {
        try {
            let options = storeExtension.getParams(loadOptions);
            return await axios.get(`/api/roles/Index/${options}`);
        } catch (error) {
            console.log(error);
        }
    }
};

export default roleService;
