import { createRouter, createWebHashHistory } from "vue-router";

import Home from "./views/home-page";
import Profile from "./views/profile-page";
import defaultLayout from "./layouts/side-nav-outer-toolbar";
import simpleLayout from "./layouts/single-card";
import Users from './views/Users-page.vue';
import GetAllUsersPage from "@/views/get-all-users-page.vue";
import ContrAgentJournal from "@/views/get-all-contrAgents-page.vue";
import authorizationService from "@/api/AuthorizationService";
import UserDetail from '@/views/users_detail_info.vue';
import ContrAgentDetail from '@/views/contrAgent-detail-info.vue';
import ProjectDetail from '@/views/project-detail-info.vue';
import ProjectJournal from '@/views/get-all-projects.vue';
import Antennae from './views/Antenna-page.vue';
import GetAllAntennaePage from "@/views/get-all-antennae-page";

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
      path: '/projects',
      name: 'projects',
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: ProjectJournal
    },
    {
      path: '/project/:mode/:id?',
      name: 'projectDetail',
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: ProjectDetail
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
      path: "/ContrAgentsJournal",
      name: "Журнал контрагентов",
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: ContrAgentJournal
    },
    {
      path: "/antennae_table",
      name: "antenna_table",
      meta: {
        requiresAuth: false,
        layout: defaultLayout
      },
      component: GetAllAntennaePage
    },
    {
      path: "/antennae",
      name: "antennae",
      meta: {
        requiresAuth: false,
        layout: defaultLayout
      },
      component: Antennae
    },
      {
          path: '/user/:mode/:id?',
          name: 'userDetail',
          meta: {
              requiresAuth: true,
              layout: defaultLayout
          },
          component: UserDetail
      }
    ,
    {
      path: '/contrAgent/:mode/:id?',
      name: 'contrAgentDetail',
      meta: {
        requiresAuth: true,
        layout: defaultLayout
      },
      component: ContrAgentDetail
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
