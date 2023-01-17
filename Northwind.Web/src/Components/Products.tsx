// A Product List View

import React, { Context, ContextType } from 'react';
import { Link } from 'react-router-dom';
import { IPagedResponse, SortBy } from '../Lib/IPagedResponse';
import { ProductApi } from '../Models/ApiModels';
import { ProductsService } from '../Services/ProductsService';
import { Loading } from './Loading';
import { Pager } from './Pager';
import { PageSort } from './PageSort';
import { PageSearch } from './PageSearch';
import { EmptyObject } from '../Lib/EmptyObject';

type ProductsState = {
    isLoading: boolean,
    currentPage?: IPagedResponse<ProductApi>
};

type ProductsProps = {
    page?: number,
    sort?: SortBy,
    searchTerm?: string,
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
        let searchTerm: string = query.get('searchTerm') ?? (props.searchTerm ?? '');

        this.onCurrentPageChanged = this.onCurrentPageChanged.bind(this);
        this.onSortOrderChanged = this.onSortOrderChanged.bind(this);
        this.onSearchTermChanged = this.onSearchTermChanged.bind(this);

        this.state = {
            isLoading: true,
            currentPage: {
                currentPage: page,
                searchTerm: searchTerm,
                sortOrder: sort,
                itemsPerPage: 10,
                totalItems: 0,
                page: [],
                totalPages: 0,
                onCurrentPageChanged: (page: number) => this.onCurrentPageChanged(page),
                onSortChanged: (sort: SortBy) => this.onSortOrderChanged(sort),
                onSearchTermChanged: (term: string) => this.onSearchTermChanged(term)
            }
        };
    }

    state: ProductsState = {
        isLoading: true,
        currentPage: null
    }

    onSearchTermChanged(term: string) {
        if (term != this.state.currentPage.searchTerm) {
            console.info('Searching for ' + term);

            this.getData(1, this.state.currentPage.sortOrder, term)
                .then((value) => { console.info('Data updated'); })
                .catch((reason) => { console.error('Error getting data!' + reason); });  
        }
    }

    onCurrentPageChanged(page: number) {        
        if (page != this.state.currentPage.currentPage) {
            console.info('Page is going to change to ' + page);

            this.getData(page, this.state.currentPage.sortOrder, this.state.currentPage.searchTerm)
                .then((value) => { console.info('Data updated'); })
                .catch((reason) => { console.error('Error getting data!' + reason); });            
        }              
    }

    onSortOrderChanged(sort: SortBy) {
        if (sort != this.state.currentPage.sortOrder) {
            console.info('Sort is going to change to ' + sort);            

            this.getData(1, sort, this.state.currentPage.searchTerm)
                .then((value) => { console.info('Data updated'); })
                .catch((reason) => { console.error('Error getting data!' + reason); });            
        }        
    }

    componentDidMount() {
        this.getData(this.state.currentPage.currentPage, this.state.currentPage.sortOrder, this.state.currentPage.searchTerm); // async
    }

    componentWillUnmount() {
        //this.setState({ isLoading: true, currentPage: null });
    }

    getTable() {
        if (this.state.isLoading) {
            return <Loading />
        }
        else {
            return (<table className="table table-responsive table-striped">
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
                        <tr><td colSpan={5}>Nothing Found!</td></tr> :
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
            </table>);
        }
    }

    render() {        
        return (<div className="row">                
                <div className="col-md-4 col-sm-12">
                    <PageSearch currentPage={this.state.currentPage.currentPage}
                        itemsPerPage={this.state.currentPage.itemsPerPage}
                        totalItems={this.state.currentPage.totalItems}
                        totalPages={this.state.currentPage.totalPages}
                        searchTerm={this.state.currentPage.searchTerm}
                        sortOrder={this.state.currentPage.sortOrder}
                        page={[] as ProductApi[]}
                        onCurrentPageChanged={this.onCurrentPageChanged}
                        onSortChanged={this.onSortOrderChanged}
                        onSearchTermChanged={this.onSearchTermChanged} />
                    <hr />
                    <PageSort<ProductApi> currentPage={this.state.currentPage.currentPage}
                        itemsPerPage={this.state.currentPage.itemsPerPage}
                        totalItems={this.state.currentPage.totalItems}
                        totalPages={this.state.currentPage.totalPages}
                        searchTerm={this.state.currentPage.searchTerm}
                        sortOrder={this.state.currentPage.sortOrder}
                        page={[] as ProductApi[]}
                        onCurrentPageChanged={this.onCurrentPageChanged}
                        onSortChanged={this.onSortOrderChanged}
                        onSearchTermChanged={this.onSearchTermChanged} />
                    <hr className="d-md-none" />
                </div>
                <div className="col-md-8 col-sm-12">
                    {this.getTable()}                    
                </div>
                <div className="col-12">
                    <Pager<ProductApi> currentPage={this.state.currentPage.currentPage}
                        itemsPerPage={this.state.currentPage.itemsPerPage}
                        totalItems={this.state.currentPage.totalItems}
                        totalPages={this.state.currentPage.totalPages}
                        searchTerm={this.state.currentPage.searchTerm}
                        sortOrder={this.state.currentPage.sortOrder}
                        page={[] as ProductApi[]}
                        onCurrentPageChanged={this.onCurrentPageChanged}
                        onSortChanged={this.onSortOrderChanged}
                        onSearchTermChanged={this.onSearchTermChanged} />                
                </div>               
            </div>);        
    }

    async getData(page: number, sort: SortBy, searchTerm: string) {
        console.info('Getting page ' + page + ' sorted by ' + sort);
                        
        try {
            this.setState((state) => ({
                isLoading: true,
                currentPage: {
                    currentPage: page,
                    searchTerm: searchTerm,
                    sortOrder: sort,
                    itemsPerPage: 0,
                    totalItems: 0,
                    page: [],
                    totalPages: 0,
                    onCurrentPageChanged: (page: number) => this.onCurrentPageChanged(page),
                    onSortChanged: (sort: SortBy) => this.onSortOrderChanged(sort),
                    onSearchTermChanged: (term: string) => this.onSearchTermChanged(term)
                }
            }));

            const result: IPagedResponse<ProductApi> = await ProductsService.Products(searchTerm, page, sort);

            this.setState({
                isLoading: false,
                currentPage: {
                    currentPage: result.currentPage,
                    searchTerm: result.searchTerm,
                    itemsPerPage: result.itemsPerPage,
                    totalItems: result.totalItems,
                    page: result.page,
                    totalPages: result.totalPages,
                    sortOrder: result.sortOrder,
                    onCurrentPageChanged: (page: number) => this.onCurrentPageChanged(page),
                    onSortChanged: (sort: SortBy) => this.onSortOrderChanged(sort),
                    onSearchTermChanged: (term: string) => this.onSearchTermChanged(term)
                }
            });            
        } catch (e) {
            console.error('Could not get product data!' + e);

            this.setState({
                isLoading: true,
                currentPage: {
                    currentPage: page,
                    searchTerm: searchTerm,
                    sortOrder: sort,
                    itemsPerPage: 10,
                    totalItems: 0,
                    page: [],
                    totalPages: 0,
                    onCurrentPageChanged: (page: number) => this.onCurrentPageChanged(page),
                    onSortChanged: (sort: SortBy) => this.onSortOrderChanged(sort),
                    onSearchTermChanged: (term: string) => this.onSearchTermChanged(term)
                }
            });
        }
        finally {            
            const query = '?page=' + this.state.currentPage.currentPage
                + '&sort=' + this.state.currentPage.sortOrder
                + (this.state.currentPage.searchTerm != undefined && this.state.currentPage.searchTerm.length > 0 ? '&searchTerm=' + this.state.currentPage.searchTerm : '');            
            // add this page to history with query parameters
            window.history.pushState({ page: this.state.currentPage.currentPage, sort: this.state.currentPage.sortOrder }, "Products Page " + this.state.currentPage.currentPage, query);
        }
    }
}


export class ProductTableRow extends React.Component<ProductApi, EmptyObject>
{
    static displayName = ProductTableRow.name;

    constructor(props: ProductApi) {
        super(props); 
    }

    state: EmptyObject = {};

    getLinkUrl(productId: number) {
        return "/Product/" + productId;
    }

    render() {
        return (
            <tr>
                <td>{this.props.productId}</td>
                <td>
                    <a href={this.getLinkUrl(this.props.productId)} target="_self">{this.props.productName}</a>
                </td>
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