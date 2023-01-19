import { IPagedResponse, SortBy } from "../Lib/IPagedResponse";
import { Products } from "./Products";

import React from "react";
import { act } from "@testing-library/react";
import { createRoot, Root } from "react-dom/client";
import { ProductApi } from "../Models/ApiModels";
import { ProductsService } from "../Services/ProductsService";

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

it('renders without error', async () => {
    // fake the result from the api
    const testData: IPagedResponse<ProductApi> = {
        totalItems: 3,
        totalPages: 1,
        itemsPerPage: 3,
        currentPage: 1,
        searchTerm: "",
        sortOrder: SortBy.Ascending | SortBy.Name,
        page: [{ productId: 1, productName: "test 1", discontinued: false, categoryId: 1, quantityPerUnit: '10 things', unitPrice: 12.34, unitsInStock: 5 },
            { productId: 2, productName: "test 2", discontinued: false, categoryId: 1, quantityPerUnit: '3 things', unitPrice: 0.12, unitsInStock: 124 },
            { productId: 3, productName: "test 3", discontinued: false, categoryId: 1, quantityPerUnit: '2 things', unitPrice: 5.99, unitsInStock: 1 }],
        onCurrentPageChanged: function(page: number): void {
            throw new Error("Function not implemented.");
        },
        onSortChanged: function(sort: SortBy): void {
            throw new Error("Function not implemented.");
        },
        onSearchTermChanged: function(term: string): void {
            throw new Error("Function not implemented.");
        }
    };

    jest.spyOn(ProductsService, "Products")
        .mockImplementation((searchTerm?: string, page?: number, sort?: SortBy) => Promise.resolve(testData));

    act(() => {
        root.render(<Products
            page={1}
            sort={SortBy.Name | SortBy.Ascending}
        />);
    });

    expect(container.querySelectorAll('tbody')).not.toBeNull();

    expect(container.querySelectorAll('tr')).not.toBeNull();
});