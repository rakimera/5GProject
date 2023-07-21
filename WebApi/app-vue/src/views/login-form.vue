<template>
    <div>
        <h2>Форма логирования</h2>
        <dx-form :formData="formData">
            <dxi-item dataField="username" editorType="dxTextBox" :editorOptions="usernameOptions" />
            <dxi-item dataField="password" editorType="dxTextBox" :editorOptions="passwordOptions" />
            <dx-button text="Войти" @onClick="login" />
        </dx-form>
    </div>
</template>

<script>
import authService from "@/api/AuthService";

export default {
    data() {
        return {
            formData: {
                username: '',
                password: ''
            },
            usernameOptions: {
                placeholder: 'Введите имя пользователя'
            },
            passwordOptions: {
                placeholder: 'Введите пароль',
                mode: 'password'
            }
        };
    },
    methods: {
        login() {
            const result = authService.login(this.formData);
            console.log(this.formData);
            console.log(result);
            console.log('Имя пользователя:', this.formData.username);
            console.log('Пароль:', this.formData.password);
        }
    }
};
</script>