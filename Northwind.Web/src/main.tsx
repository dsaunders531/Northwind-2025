// main.ts

import React from 'react';
import { createRoot } from 'react-dom/client';
import { App } from './App';
import { BrowserRouter } from 'react-router-dom';

try {    
    console.info("Starting up...");    

    const rootNode: HTMLElement = document.getElementById('app');

    if (rootNode) {
        if (rootNode)
        {
            const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');

            createRoot(rootNode).render(<BrowserRouter basename={baseUrl}><App /></BrowserRouter>);
        }
    }
    else
    {
        console.error("Cannot find an element to render the app!");
    }    
} catch (e) {
    console.error(e);
}