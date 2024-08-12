// Products Service
import AppConfig from "../AppConfig";
import { IPagedResponse } from '../Lib/IPagedResponse';
import { SortBy } from "../Lib/SortBy";
import { ProductApi } from '../Models/ApiModels';
import { HttpClient } from '../Lib/HttpClient';

export class ProductsService {
    static displayName = ProductsService.name;

    static async Product(productId: number) {

        const url = AppConfig.apiUrl + "Products/" + productId;

        try {
            return await HttpClient.Get<ProductApi>(url);

        } catch (e) {
            throw new Error('Cannot get Product from Api!', { cause: e });
        }
    }

    static async Products(searchTerm?: string, page?: number, sort?: SortBy) {
        searchTerm = searchTerm ?? '';
        page = page ?? 1;
        sort = sort ?? SortBy.Name | SortBy.Ascending;

        const url = AppConfig.apiUrl + "Products?page=" + page + "&sort=" + sort + (searchTerm.length > 0 ? "&searchTerm=" + searchTerm : '');

        try {
            return await HttpClient.Get<IPagedResponse<ProductApi>>(url);

        } catch (e) {
            throw new Error('Cannot get Products from Api!', { cause: e });
        }
    }

    static async Search(term?: string) {
        term = term ?? '';

        const url = AppConfig.apiUrl + "Products/search?term=" + term;

        try {
            return await HttpClient.Get<string[]>(url);

        } catch (e) {
            throw new Error('Cannot get data from Search', { cause: e });
        }
    }
}