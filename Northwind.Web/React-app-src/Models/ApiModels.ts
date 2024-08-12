// Models emitted by the api
// These are plain data objects

export class CategoryApi {
    categoryId: number;
    categoryName: string;
    description?: string;
}

export class ProductApi {
    categoryId?: number;
    discontinued: boolean;
    productId: number;
    productName: string;
    quantityPerUnit?: string;
    unitPrice?: number;
    unitsInStock?: number;
}