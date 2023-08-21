import axios from '../utils/axios';
import storeExtension from "@/utils/storeExtension";

const executiveCompanyService = {
    async getExecutiveCompanies() {
        try {
            return await axios.get(`/api/ExecutiveCompanies`);
        }
        catch (error){
            console.log(error)
        }
    },

    async getExecutiveCompany(oid) {
        try {
            return await axios.get(`/api/ExecutiveCompanies/${oid}`);
        }
        catch (error){
            console.log(error)
        }
    },
    async createExecutiveCompany(executiveCompany) {
        try {
            return await axios.post('/api/ExecutiveCompanies', executiveCompany);
        }
        catch (error){
            console.log(error)
        }

    },

    async updateExecutiveCompany(executiveCompany) {
        try {
            return await axios.put('/api/ExecutiveCompanies', executiveCompany)
        }
        catch (error){
            console.log(error)
        }
    },

    async deleteExecutiveCompany(oid) {
        try {
            return await axios.delete(`/api/ExecutiveCompanies/${oid}`)
        }
        catch (error){
            console.log(error)
        }

    },

    async getAllExecutiveCompanies(loadOptions) {
        try {
            let options = storeExtension.getParams(loadOptions);
            const response = await axios.get(`/api/ExecutiveCompanies/Index/${options}`);
            console.log(response)
            return response.data;
        } catch (error) {
            console.log(error)
        }
    }
};

export default executiveCompanyService;