/*const defaultUser = {
  email: 'sandra@example.com',
  avatarUrl: 'https://js.devexpress.com/Demos/WidgetsGallery/JSDemos/images/employees/06.png'
};*/
import AuthService from '../api/AuthService';
import login from "@/components/Login.vue";

export default {
  data(){
    return{
      loginModel:{
        login: '',
        password: ''
      }
    }
  },
  methods: {
    login(){
      this.loginModel = {login: this.loginModel.login}
      return
        try {
          const response =  new Promise((resolve, reject) => {
            login(loginModel).then()
          })
        }
        
        
      }
    },
    
  }
  
  /*
  _user: defaultUser,
  loggedIn() {
    return !!this._user;
  },*/

  async logIn(email, password) {
    try {
      // Send request
      console.log(email, password);
      this._user = { ...defaultUser, email };

      return {
        isOk: true,
        data: this._user
      };
    }
    catch {
      return {
        isOk: false,
        message: "Authentication failed"
      };
    }
  },

  async logOut() {
    this._user = null;
  },

  async getUser() {
    try {
      // Send request

      return {
        isOk: true,
        data: this._user
      };
    }
    catch {
      return {
        isOk: false
      };
    }
  },

  async resetPassword(email) {
    try {
      // Send request
      console.log(email);

      return {
        isOk: true
      };
    }
    catch {
      return {
        isOk: false,
        message: "Failed to reset password"
      };
    }
  },

  async changePassword(email, recoveryCode) {
    try {
      // Send request
      console.log(email, recoveryCode);

      return {
        isOk: true
      };
    }
    catch {
      return {
        isOk: false,
        message: "Failed to change password"
      }
    }
  },

  async createAccount(email, password) {
    try {
      // Send request
      console.log(email, password);

      return {
        isOk: true
      };
    }
    catch {
      return {
        isOk: false,
        message: "Failed to create account"
      };
    }
  }
};
