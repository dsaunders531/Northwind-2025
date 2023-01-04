import React from "react";
import { createRoot, Root } from 'react-dom/client';
import { Loading } from "./Loading";

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
    container = null;    
    root = null;
});

it('loading renders loading text', async () => {    
    root.render(<Loading />);    

    expect(container.textContent).toBe('');          
    expect(container.querySelectorAll('.spinner-border')).not.toBeNull();
});