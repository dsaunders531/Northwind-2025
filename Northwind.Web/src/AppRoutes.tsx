// App Routes
// List out your routes here.

import React from 'react';
import { HelloWorld } from './Components/HelloWorld';
import { Products } from './Components/Products';
import { TestMe } from './Components/TestComponent';
import { AppRoute } from './Lib/AppRoute';
import { SortBy } from './Lib/IPagedResponse';

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