import { createRouter, createWebHashHistory } from "vue-router";

import Home from "./views/home-page";
import Profile from "./views/profile-page";
import Tasks from "./views/tasks-page";
import defaultLayout from "./layouts/side-nav-outer-toolbar";
import simpleLayout from "./layouts/single-card";
import Users from './views/Users-page.vue';
import GetAllUsersPage from "@/views/get-all-users-page.vue";
import CreateProject from "@/views/create-project.vue";
import ContrAgents from './views/ContrAgent-page.vue';
import authorizationService from "@/api/AuthorizationService";
import UserDetail from '@/views/users_detail_info.vue';
import CreateUser from '@/views/create-user.vue';

function loadView(view) {
  return () => import (/* webpackChunkName: "login" */ `./views/${view}.vue`)
}

const router = new createRouter({
  routes: [
    {
      path: "/home",
      name: "home",
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: Home
    },
    {
      path: "/profile",
      name: "profile",
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: Profile
    },
    {
      path: "/projects",
      name: "projects",
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: Tasks
    },
    {
      path: "/create-projects",
      name: "create-projects",
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: CreateProject
    },
    {
      path: "/login",
      name: "login",
      meta: {
        requiresAuth: false,
        layout: simpleLayout,
        title: "Войти в аккаунт"
      },
      component: loadView("login-form")
    },
    {
      path: "/reset-password",
      name: "reset-password",
      meta: {
        requiresAuth: false,
        layout: simpleLayout,
        title: "Сбросить пароль",
        description: "Please enter the email address that you used to register, and we will send you a link to reset your password via Email."
      },
      component: loadView("reset-password-form")
    },
    {
      path: "/create-account",
      name: "create-account",
      meta: {
        requiresAuth: false,
        layout: simpleLayout,
        title: "Создать аккаунт"
      },
      component: loadView("create-account-form"),
    },
    {
      path: "/change-password/:recoveryCode",
      name: "change-password",
      meta: {
        requiresAuth: false,
        layout: simpleLayout,
        title: "Change Password"
      },
      component: loadView("change-password-form")
    },
    {
      path: "/",
      redirect: "/home"
    },
    {
      path: "/recovery",
      redirect: "/home"
    },
    {
      path: "/:pathMatch(.*)*",
      redirect: "/home"
    },
    {
      path: "/users",
      name: "users",
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: Users
    },
    {
      path: "/users_table",
      name: "users_table",
      meta: {
        requiresAuth: false,
        layout: defaultLayout
      },
      component: GetAllUsersPage
    }
    ,
    {
      path: "/contrAgents",
      name: "contrAgents",
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: ContrAgents
    },
      {
          path: '/user/:mode/:id',
          name: 'userDetail',
          meta: {
              requiresAuth: true,
              layout: defaultLayout
          },
          component: UserDetail
      },
    {
      path: '/user/:mode',
      name: 'userCreate',
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: UserDetail
    },
    {
      path: '/user/:mode',
      name: 'createUser',
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: CreateUser
    }
  ],
  history: createWebHashHistory()
});

router.beforeEach((to, from, next) => {
  if (to.name === "login" && authorizationService.loggedIn()) {
    next({ name: "home" });
  }

  if (to.matched.some(record => record.meta.requiresAuth)) {
    if (!authorizationService.loggedIn()) {
      next({
        name: "login",
        query: { redirect: to.fullPath }
      });
    } else {
      next();
    }
  } else {
    next();
  }
});

export default router;
