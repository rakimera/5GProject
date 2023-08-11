export default [
    {
        text: "Главная",
        path: "/home",
        icon: "home"
    },
    {
        text: "Администрирование",
        icon: "preferences",
        items: [
            {
                text: "Профиль",
                path: "/profile"
            },
            {
                text: "Журнал пользователей",
                path: "/users_table"
            },
            {
                text: "ContrAgents",
                path: "/contrAgents"
            },
            {
                text: "Роли",
                path: "/roles"
            },
            {
                text: "Журнал контрагентов",
                path: "/ContrAgentsJournal"
            }
        ]
    },
    {
        text: "Работа с проектами",
        icon: "folder",
        items: [
            {
                text: "Проекты",
                path: "/projects"
            },
            {
                text: "Новый проект",
                path: "/create-projects"
            }
        ]
    },
    {
        text: "Антенны",
        icon: "folder",
        items: [
            {
                text: "Список антенн",
                path: "/antennae_table"
            },
            {
                text: "Тест",
                path: "/antennae"
            }
        ]
    }
];
