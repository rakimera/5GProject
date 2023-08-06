import axios from '../utils/axios';
import storeExtension from '../utils/storeExtension';

const userService = {

    async getUsers() {
        try {
            return await axios.get(`/api/users`);
        }
        catch (error){
            console.log(error)
        }
    },

    async getUser(oid) {
        try {
            return await axios.get(`/api/users/${oid}`);
        }
        catch (error){
            console.log(error)
        }
    },
    
    async createUser(user) {
        try {
            return await axios.post('/api/users', user);
        }
        catch (error){
            console.log(error)
        }
        
    },
    
    async updateUser(user) {
        try {
            return await axios.put('/api/users', user)
        }
        catch (error){
            console.log(error)
        }
    },

    async deleteUser(oid) {
        try {
            return await axios.delete(`/api/users/${oid}`)
        }
        catch (error){
            console.log(error)
        }
        
    },

    async getAllUsers(loadOptions) {
        try {
            let options = storeExtension.getParams(loadOptions);
            const response = await axios.get(`/api/users/Index/${options}`);
            console.log(response)
            return response.data;
        } catch (error) {
            console.log(error)
        }
    }
    
};

export default userService;
