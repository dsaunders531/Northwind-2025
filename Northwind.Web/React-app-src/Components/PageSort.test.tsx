import { SortBy } from "../Lib/SortBy";
import { ProductApi } from "../Models/ApiModels";
import { PageSort } from "./PageSort";

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

it('Page sort renders without error', async () => {
    act(() => {
        root.render(<PageSort<ProductApi>
            currentPage={4}
            totalItems={100}
            itemsPerPage={5}
            totalPages={20}
            sortOrder={SortBy.Name | SortBy.Ascending}
            searchTerm=""
            page={[]}
            onCurrentPageChanged={(page: number) => {}}
            onSearchTermChanged={(term: string) => {}}
            onSortChanged={(sort: SortBy) => {}}
        />);
    });
   
    expect(container.querySelector('select.form-select')).not.toBeNull;     
});

it('Page sort has an input', async () => {
    act(() => {
        root.render(<PageSort<ProductApi>
            currentPage={4}
            totalItems={100}
            itemsPerPage={5}
            totalPages={20}
            sortOrder={SortBy.Name | SortBy.Ascending}
            searchTerm=""
            page={[]}
            onCurrentPageChanged={(page: number) => {}}
            onSearchTermChanged={(term: string) => {}}
            onSortChanged={(sort: SortBy) => {}}
        />);
    });

    expect(container.querySelector("[data-testid=sort-select]")).not.toBeNull;
});