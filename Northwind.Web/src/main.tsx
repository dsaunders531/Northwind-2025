// main.ts

import React from 'react';
import { createRoot } from 'react-dom/client';
import { App } from './App';

try {    
    console.info("Hello World!");    

    const rootNode: HTMLElement = document.getElementById('app');

    if (rootNode) {
        if (rootNode)
        {
            createRoot(rootNode).render(<App />);
        }
    }
    else
    {
        console.error("Cannot find an element to render the app!");
    }    
} catch (e) {
    console.error(e);
}