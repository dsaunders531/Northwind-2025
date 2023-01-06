// A Product List View

import React from 'react';
import { IPagedResponse, SortBy } from '../Lib/IPagedResponse';
import { ProductApi } from '../Models/ApiModels';
import { ProductsService } from '../Services/ProductsService';
import { Loading } from './Loading';
import { Pager } from './Pager';

type ProductsState = {
    isLoading: boolean,
    currentPage?: IPagedResponse<ProductApi>
};

type ProductsProps = {
    page?: number,
    sort?: SortBy,
    searchTerm?: string
};

export class Products extends React.Component<ProductsProps, ProductsState>
{
    static displayName = Products.name;

    constructor(props: ProductsProps) {
        super(props);

        // get parameters from query string (or props)
        const query = new URLSearchParams(window.location.search);

        let page: number = query.get('page') as unknown as number ?? (props.page ?? 1);
        let sort: SortBy = query.get('sort') as unknown as SortBy ?? (props.sort ?? SortBy.Name | SortBy.Ascending);
        let searchTerm: string = query.get('searchTerm') ?? (props.searchTerm ?? '' );

        this.state = {
            isLoading: true,
            currentPage: {
                currentPage: page,
                searchTerm: searchTerm,
                sortOrder: sort,
                itemsPerPage: 10,
                totalItems: 0,
                page: [],
                totalPages: 0
            }
        };
    }

    state: ProductsState = {
        isLoading: true,
        currentPage: null
    }

    componentDidMount() {
        this.getData(); // async
    }

    componentWillUnmount() {
        this.setState({ isLoading: true, currentPage: null });
    }

    render() {
        if (this.state.isLoading) {
            return <Loading />
        }
        else {
            return (<div>
                <table className="table table-responsive table-striped">
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Name</th>
                            <th>Quantity Per Unit</th>
                            <th>Unit Price</th>
                            <th></th>
                        </tr>                       
                    </thead>
                    <tbody>
                        {this.state.currentPage.page.length == 0 ?
                            <tr><td>Nothing Found!</td></tr> :
                            this.state.currentPage.page.map((value: ProductApi, index: number) => {
                                return <ProductTableRow key={index}
                                    productId={value.productId}
                                    productName={value.productName}
                                    quantityPerUnit={value.quantityPerUnit}
                                    discontinued={value.discontinued}
                                    categoryId={value.categoryId}
                                    unitPrice={value.unitPrice}
                                    unitsInStock={value.unitsInStock}                                    
                                />
                            })
                        }
                    </tbody>
                </table>
                <Pager<ProductApi> currentPage={this.state.currentPage.currentPage}
                    itemsPerPage={this.state.currentPage.itemsPerPage}
                    totalItems={this.state.currentPage.totalItems}
                    totalPages={this.state.currentPage.totalPages}
                    searchTerm={this.state.currentPage.searchTerm}
                    sortOrder={this.state.currentPage.sortOrder}
                    page={[] as ProductApi[]}
                />
            </div>);
        }
    }

    async getData() {
        let page: number = this.state.currentPage?.currentPage ?? 1;
        let sort: SortBy = this.state.currentPage?.sortOrder ?? SortBy.Ascending | SortBy.Name;
        let searchTerm: string = this.state.currentPage?.searchTerm ?? '';

        try {           
            this.setState({
                isLoading: true,
                currentPage: {
                    currentPage: page,
                    searchTerm: searchTerm,
                    sortOrder: sort,
                    itemsPerPage: 10,
                    totalItems: 0,
                    page: [],
                    totalPages: 0
                }
            });

            const result: IPagedResponse<ProductApi> = await ProductsService.Products(searchTerm, page, sort);

            this.setState({isLoading: false, currentPage: result});

        } catch (e) {
            console.error('Could not get product data!', + e);

            this.setState({
                isLoading: true,
                currentPage: {
                    currentPage: page,
                    searchTerm: searchTerm,
                    sortOrder: sort,
                    itemsPerPage: 10,
                    totalItems: 0,
                    page: [],
                    totalPages: 0
                }
            });
        }
    }
}


export class ProductTableRow extends React.Component<ProductApi, ProductApi>
{
    static displayName = ProductTableRow.name;

    constructor(props: ProductApi) {
        super(props); 

        this.state = {
            productId: props.productId,
            discontinued: props.discontinued,
            productName: props.productName,
            categoryId: props.categoryId,
            quantityPerUnit: props.quantityPerUnit,
            unitPrice: props.unitPrice,
            unitsInStock: props.unitsInStock            
        };
    }

    state: ProductApi = {
        discontinued: true,
        productId: 0,
        productName: ''
    };

    render() {
        return (
            <tr>
                <td>{this.props.productId}</td>
                <td>{this.props.productName}</td>
                <td>{this.props.quantityPerUnit}</td>
                <td>{this.props.unitPrice.toFixed(2)}</td>
                <td>
                    {this.props.discontinued ? <i className="text-error fa-solid circle-xmark" title="This product has been discontinued"></i> : ''}
                    {this.props.unitsInStock < 5 ? <i className="text-warning fa-solid triangle-exclamation" title="Not many of these left!"></i> : '' }
                </td>
            </tr>
        );
    }
}