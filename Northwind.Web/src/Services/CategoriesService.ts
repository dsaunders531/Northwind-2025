// Categories Service
import AppConfig from '../AppConfig';
import { SortBy, IPagedResponse } from '../Lib/IPagedResponse';
import { CategoryApi, ProductApi } from '../Models/ApiModels';
import { HttpClient } from '../Lib/HttpClient';

export class CategoriesService {
    static displayName = CategoriesService.name;

    static async Categories(page?: number, sort?: SortBy) {
        page = page ?? 1;
        sort = sort ?? SortBy.Ascending | SortBy.Name;

        const url = AppConfig.apiUrl + "Categories?page=" + page + "&sort=" + sort;

        try {
            return await HttpClient.Get<IPagedResponse<CategoryApi>>(url);    
            
        } catch (e) {            
            throw new Error('Cannot get data from Api!', { cause: e });
        }        
    }

    static async ProductsInCategories(categoryId: number, page?: number, sort?: SortBy) {
        page = page ?? 1;
        sort = sort ?? SortBy.Ascending | SortBy.Name;  

        const url = AppConfig.apiUrl + "Categories/" + categoryId + "/products?page=" + page + "&sort=" + sort;

        try {
            return await HttpClient.Get<IPagedResponse<ProductApi>>(url);

        } catch (e) {
            throw new Error('Cannot get data from Api!', { cause: e });
        }
    }
}