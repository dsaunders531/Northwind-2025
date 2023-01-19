import { App } from "./App";

import React from "react";
import { act } from "@testing-library/react";
import { MemoryRouter } from 'react-router-dom';
import { createRoot, Root } from "react-dom/client";

let container: HTMLDivElement = null;
let root: Root = null;

beforeEach(() => {
    // setup a DOM element as a render target
    container = document.createElement("div");
    document.body.appendChild(container);
    root = createRoot(container);
});

afterEach(() => {
    // cleanup on exiting
    root.unmount();
    container.remove();
    container = null;
    root = null;
});

it('renders without crashing', async () => {
    act(() => {
        root.render(
            <MemoryRouter>
                <App />
            </MemoryRouter>);
    });
    
    await new Promise(resolve => setTimeout(resolve, 1000));       
});