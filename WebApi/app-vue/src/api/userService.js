import axios from '../utils/axios';
import storeExtension from '../utils/storeExtension';


const header = await authHeader();
const userService = {

    async getUsers() {
        try {
            
            return await axios.get(`/api/users`, { headers: header});
        }
        catch (error){
            console.log(error)
        }
    },
    
    async createUser(user) {
        try {
            return await axios.post('/api/users', user, { headers: header});
        }
        catch (error){
            console.log(error)
        }
        
    },
    
    async updateUser(user) {
        try {
            return await axios.put('/api/users', user, { headers: header})
        }
        catch (error){
            console.log(error)
        }
    },

    async deleteUser(oid) {
        try {
            return await axios.delete(`/api/users/${oid}`, { headers: header})
        }
        catch (error){
            console.log(error)
        }
        
    },

    async getAllUsers(loadOptions) {
        try {
            let options = storeExtension.getParams(loadOptions);
            const response = await axios.get(`/api/users/Index/${options}`, { headers: header});
            return response.data;
        } catch (error) {
            console.log(error)
        }
    }
    
};

export default userService;
