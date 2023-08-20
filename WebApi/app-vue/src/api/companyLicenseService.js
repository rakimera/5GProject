import axios from '../utils/axios';
import storeExtension from "@/utils/storeExtension";

const companyLicenseService = {

    async getLicenses() {
        try {
            return await axios.get(`/api/CompanyLicenses`);
        } catch (error) {
            console.log(error)
        }
    },

    async createLicense(license) {
        try {
            return await axios.post('/api/CompanyLicenses', license);
        } catch (error) {
            console.log(error);
        }
    },

    async updateLicense(license) {
        try {
            return await axios.put(`/api/CompanyLicenses/`, license);
        } catch (error) {
            console.log(error);
        }
    },

    async deleteLicense(oid) {
        try {
            return await axios.delete(`/api/CompanyLicenses/${oid}`);
        } catch (error) {
            console.log(error);
        }
    },

    async getAllLicenses(loadOptions) {
        try {
            let options = storeExtension.getParams(loadOptions);
            return await axios.get(`/api/CompanyLicenses/Index/${options}`);
        } catch (error) {
            console.log(error);
        }
    }
};

export default companyLicenseService;
