// Show the categories

import React from 'react';
import { IPagedResponse, SortBy } from '../Lib/IPagedResponse';
import { CategoryApi } from '../Models/ApiModels';
import { CategoriesService } from '../Services/CategoriesService';
import { Loading } from './Loading';
import { Pager } from './Pager';

type CategoriesState = {
    isLoading: boolean,
    currentPage?: IPagedResponse<CategoryApi>
}

type CategoriesProps = {
    page?: number
}

export class Categories extends React.Component<CategoriesProps, CategoriesState> {
    static displayName = Categories.name;

    constructor(props: CategoriesProps) {
        super(props);

        // get parameters from query string (or props)
        const query = new URLSearchParams(window.location.search);

        let page: number = query.get('page') as unknown as number ?? (props.page ?? 1);

        this.onCurrentPageChanged = this.onCurrentPageChanged.bind(this);
        this.onSortOrderChanged = this.onSortOrderChanged.bind(this);
        this.onSearchTermChanged = this.onSearchTermChanged.bind(this);

        this.state = {
            isLoading: true,
            currentPage: {
                currentPage: page,
                searchTerm: '',
                sortOrder: SortBy.Name | SortBy.Ascending,
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

    state: CategoriesState = {
        isLoading: true,
        currentPage: null
    }

    componentDidMount() {
        this.getData(this.state.currentPage.currentPage); // async
    }

    componentWillUnmount() {
        //this.setState((state) => ({ isLoading: true, currentPage: null }));
    }

    onSearchTermChanged(term: string) {
        // not supported.
        throw new Error('Operation Not Supported');
    }

    onCurrentPageChanged(page: number) {
        if (page != this.state.currentPage.currentPage) {
            console.info('Page is going to change to ' + page);

            this.getData(page)
                .then((value) => { console.info('Data updated'); })
                .catch((reason) => { console.error('Error getting data!' + reason); });
        }
    }

    onSortOrderChanged(sort: SortBy) {
        // not supported.
        throw new Error('Operation Not Supported');
    }

    getLinkUrl(categoryId: number) {
        return '/category/' + categoryId.toString();
    }

    render() {
        if (this.state.isLoading) {
            return <Loading />
        }
        else if (this.state.currentPage == null) {
            return (<div className="row">
                <div className="col-12">
                    <h3>Nothing Found</h3>
                    <p><small>You might want to check the console in case there are errors.</small></p>
                </div>
            </div>);
        }
        else {
            return (
                <div className="row">
                    <div className="col-12">
                        <table className="table table-responsive table-striped">
                            <thead>
                                <tr>                                    
                                    <td>Name</td>
                                    <td>Description</td>
                                </tr>
                            </thead>
                            <tbody>
                                {this.state.currentPage.page.length == 0 ?
                                    <tr><td>Nothing Found!</td></tr> :
                                    this.state.currentPage.page.map((value: CategoryApi, index: number) => {
                                        return <tr key={index}>                                            
                                            <td><a href={this.getLinkUrl(value.categoryId)} target="_self">{value.categoryName}</a></td>
                                            <td>{value.description}</td>
                                        </tr>
                                    })}
                            </tbody>
                        </table>
                    </div>  
                    <div className="col-12">
                        <Pager totalItems={this.state.currentPage.totalItems}
                            totalPages={this.state.currentPage.totalPages}
                            itemsPerPage={this.state.currentPage.itemsPerPage}
                            currentPage={this.state.currentPage.currentPage}
                            searchTerm={this.state.currentPage.searchTerm}
                            sortOrder={this.state.currentPage.sortOrder}
                            page={[] as CategoryApi[]}
                            onCurrentPageChanged={this.onCurrentPageChanged}
                            onSortChanged={this.onSortOrderChanged}
                            onSearchTermChanged={this.onSearchTermChanged} />
                    </div>
                </div>
                );
        }
    }

    async getData(page: number) {        
        try {            
            let sort: SortBy = this.state.currentPage?.sortOrder ?? SortBy.Ascending | SortBy.Name;

            this.setState((state) => ({ isLoading: true, currentPage: null }));

            const result: IPagedResponse<CategoryApi> = await CategoriesService.Categories(page, sort);

            this.setState((state) => ({ isLoading: false, currentPage: result}));
        } catch (e) {
            console.error('Could not get category data!' + e);
            this.setState((state) => ({ isLoading: false, currentPage: null }));
        }        
    }
}