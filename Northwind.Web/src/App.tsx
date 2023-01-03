// App.tsx
import { Layout } from './Layout';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';

import React from 'react';

export const App: React.FC = () => (
    <Layout>
        <Routes>
            {AppRoutes.map((route, index) => {
                const { element, ...rest } = route;
                return <Route key={index} {...rest} element={element} />;
            })}
        </Routes>
    </Layout>    
);