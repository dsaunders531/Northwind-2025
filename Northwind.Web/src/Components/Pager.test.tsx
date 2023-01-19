import { SortBy } from "../Lib/IPagedResponse";
import { Pager } from "./Pager";

import React from "react";
import { act, getByText, fireEvent } from "@testing-library/react";
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

it("pager renders links", async () => {
    act(() => {
        root.render(<MemoryRouter><Pager
            totalItems={50}
            totalPages={5}
            itemsPerPage={10}
            currentPage={4}
            searchTerm=""
            sortOrder={SortBy.Name | SortBy.Ascending}
            page={[]}
            onCurrentPageChanged={(page: number) => { }}
            onSearchTermChanged={(term: string) => { }}
            onSortChanged={(sort: SortBy) => { }}
        /></MemoryRouter>);
    });

    expect(container.querySelectorAll('a').length).toEqual(7);      
});

it("pager has an active link", async () => {
    act(() => {
        root.render(<MemoryRouter><Pager
            totalItems={50}
            totalPages={5}
            itemsPerPage={10}
            currentPage={4}
            searchTerm=""
            sortOrder={SortBy.Name | SortBy.Ascending}
            page={[]}
            onCurrentPageChanged={(page: number) => { }}
            onSearchTermChanged={(term: string) => { }}
            onSortChanged={(sort: SortBy) => { }}
        /></MemoryRouter>);
    });

    expect(container.querySelector('a.page-link.active')).not.toBeNull();
});

it("pager has correct active page", async () => {
    const activePage: number = 4;

    const mockOnCurrentPageChanged = jest.fn();

    act(() => {
        root.render(<MemoryRouter><Pager
            totalItems={50}
            totalPages={5}
            itemsPerPage={10}
            currentPage={activePage}
            searchTerm=""
            sortOrder={SortBy.Name | SortBy.Ascending}
            page={[]}
            onCurrentPageChanged={mockOnCurrentPageChanged}
            onSearchTermChanged={(term: string) => { }}
            onSortChanged={(sort: SortBy) => { }}
        /></MemoryRouter>);
    });

    expect(container.querySelector('a.page-link.active').textContent).toEqual(activePage.toString());

    // check that clicking a link fires the event to get a different page.
    const firstLink: HTMLAnchorElement = container.querySelector('a.page-link');

    let eventDispatched: boolean = false;

    act(() => {
        eventDispatched = firstLink.dispatchEvent(new MouseEvent('click', { bubbles: true}));
    });

    expect(eventDispatched).toBeTruthy();

    expect(mockOnCurrentPageChanged).toHaveBeenCalledTimes(1);   
});