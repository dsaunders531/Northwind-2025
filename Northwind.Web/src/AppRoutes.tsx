// App Routes
// List out your routes here.

import React from 'react';
import { Products } from './Components/Products';
import { AppRoute } from './Lib/AppRoute';
import { Categories } from './Components/Categories';

const AppRoutes: AppRoute[] = [
    {
        name: "home",
        path: "/",
        index: true,
        element: <Products />,
        requireAuth: false,
        iconClass: "fa-solid fa-house",
        sortOrder: 0        
    },
    {
        name: "departments",
        path: "/departments",
        index: false,
        element: <Categories />,
        requireAuth: false,
        iconClass: '',
        sortOrder: 1
    }
];

export default AppRoutes;