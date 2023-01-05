// App Routes
// List out your routes here.

import React from 'react';
import { HelloWorld } from './Components/HelloWorld';
import { TestMe } from './Components/TestComponent';
import { AppRoute } from './Lib/AppRoute';

const AppRoutes: AppRoute[] = [
    {
        name: "home",
        path: "/",
        index: true,
        element: <HelloWorld />,
        requireAuth: false,
        iconClass: "fa-solid fa-house",
        sortOrder: 0
    },
    {
        name: "test",
        path: "/test",
        index: false,
        element: <TestMe />,
        requireAuth: false,
        iconClass: '',
        sortOrder: 1
    }
];

export default AppRoutes;