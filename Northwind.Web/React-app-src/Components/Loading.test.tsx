import { Loading } from "./Loading";

import React from "react";
import { act } from "@testing-library/react";
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
    act(() => { root.unmount() });        
    container.remove();
    container = null;    
    root = null;
});

it('loading renders loading text', async () => {    
    act(() => {
        root.render(<Loading />);    
    });

    expect(container.textContent).toBe('Loading...');              
});

it('renders a spinner', async () => {
    act(() => {
        root.render(<Loading />);
    });
    
    expect(container.querySelectorAll('.spinner-border')).not.toBeNull();
});