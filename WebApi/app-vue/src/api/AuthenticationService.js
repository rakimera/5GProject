import jwt_decode from "jwt-decode";
import tokenService from "@/api/tokenService";


const AuthenticationService = {
    async getLogin() {
        const accessToken = await tokenService.getAccessToken()
        const decode = this.getDecode(accessToken)
        return decode.login;
    },
    async getRole() {
        const accessToken = await tokenService.getAccessToken()
        const decode = this.getDecode(accessToken)
        return decode.role;
    },
    getDecode(token) {
        return jwt_decode(token);
    }
}

export default AuthenticationService