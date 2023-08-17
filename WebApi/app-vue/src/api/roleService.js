import axios from '../utils/axios';

const roleService = {

    async getRoles() {
        try {
            return await axios.get(`/api/roles`);
        } catch (error) {
            console.log(error)
        }
    },

};

export default roleService;
