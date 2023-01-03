// App.tsx
import { Layout } from './Layout';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import React, { Component, ReactNode } from 'react';
import { EmptyObject } from './Lib/EmptyObject';

export class App extends React.Component<EmptyObject, EmptyObject>
{
    static displayName = App.name;

    state: EmptyObject = {};

    render()
    {
        return (
            <Layout>
                <Routes>
                    {AppRoutes.map((route, index) => {
                        const { element, ...rest } = route;
                        return <Route key={index} {...rest} element={element} />;
                    })}
                </Routes>
            </Layout>
        );
    }
}