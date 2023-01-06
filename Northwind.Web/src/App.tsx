// App.tsx
// Note the fallback route - it just navigates to / - you might want a not found component.
import { Layout } from './Layout';
import { Route, Routes, Navigate } from 'react-router-dom';
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
                    <Route path="*" element={<Navigate to="/" />} />
                </Routes>                
            </Layout>
        );
    }
}