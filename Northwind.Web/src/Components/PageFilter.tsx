// A filter for lists which use IPagedResponse

import React from 'react';
import { IPagedResponse, SortBy } from '../Lib/IPagedResponse';

export class PageFilter<T> extends React.Component<IPagedResponse<T>, IPagedResponse<T>> {
    static displayName = PageFilter.name;

    constructor(props: IPagedResponse<T>) {
        super(props);

        this.onInputChange = this.onInputChange.bind(this);

        this.state = {
            currentPage: this.props.currentPage,
            itemsPerPage: this.props.itemsPerPage,
            totalPages: this.props.totalPages,
            totalItems: this.props.totalItems,
            page: [] as T[],
            searchTerm: this.props.searchTerm,
            sortOrder: this.props.sortOrder,
            onCurrentPageChanged: (page: number) => this.props.onCurrentPageChanged(page),
            onSortChanged: (sort: SortBy) => this.props.onSortChanged(sort)
        };
    }

    state: IPagedResponse<T> = {
        totalItems: 0,
        totalPages: 0,
        itemsPerPage: 0,
        currentPage: 0,
        searchTerm: '',
        sortOrder: SortBy.Name | SortBy.Ascending,
        page: [],
        onCurrentPageChanged: (page: number) => this.props.onCurrentPageChanged(page),
        onSortChanged: (sort: SortBy) => this.props.onSortChanged(sort)
    }

    onInputChange(sort: SortBy) {
        // this needs to handle asc, desc or select list
        let newSort: SortBy = this.props.sortOrder;

        if (sort & SortBy.Ascending || sort & SortBy.Descending) {
            // the sort order has changed
            newSort = sort | this.getTypeSort(this.props.sortOrder);
        }
        else {
            // the sort field has changed
            newSort = sort | this.getAscDescSort(this.props.sortOrder);
        }          

        if (newSort != this.props.sortOrder) {
            this.props.onSortChanged(newSort);
        }
    }

    getTypeSort(sort: SortBy) {        
        if (sort & SortBy.Name) {
            return SortBy.Name;
        }
        else if (sort & SortBy.Popularity) {
            return SortBy.Popularity;
        }
        else if (sort & SortBy.Price) {
            return SortBy.Price;
        }
        else {
            return SortBy.Name;
        }
    }

    getAscDescSort(sort: SortBy) {
        if (sort & SortBy.Descending) {
            return SortBy.Descending;
        }
        else {
            return SortBy.Ascending;
        }
    }

    getDefaultSortOrder() {
        // extract just the type (excluding asc or desc from the sort order)
        let result: SortBy = (this.props.sortOrder ?? SortBy.Name);

        return this.getTypeSort(result);
    }

    render() {
        return (
            <div className="row"> 
                <div className="col-6 col-md-12">
                    <select className="form-select"
                        aria-label="Order the products list"
                        onChange={(e) => this.onInputChange(e.target.value as unknown as SortBy)}
                        defaultValue={this.getDefaultSortOrder()}>
                        <option value={SortBy.Name}>Name</option>
                        <option value={SortBy.Price}>Price</option>
                        <option value={SortBy.Popularity}>Popularity</option>
                    </select>
                </div>
                <div className="col-6 col-md-12">
                    <div className="form-check form-check-inline">
                        <input className="form-check-input"
                            type="radio"
                            name="sortAscDescOpts"
                            id="sortAsc"
                            value={SortBy.Ascending}
                            checked={(this.props.sortOrder ?? SortBy.Ascending) & SortBy.Ascending ? true : false}
                            onChange={(e) => this.onInputChange(e.target.value as unknown as SortBy)}>
                        </input>
                        <label className="form-check-label" htmlFor="sortAsc">
                            <i className="fa-solid fa-arrow-down-short-wide"></i>
                            <span className="d-none d-md-block">Low to High</span>
                        </label>
                    </div>
                    <div className="form-check form-check-inline">
                        <input className="form-check-input"
                            type="radio"
                            name="sortAscDescOpts"
                            id="sortDesc"
                            value={SortBy.Descending}
                            checked={(this.props.sortOrder ?? SortBy.Ascending) & SortBy.Descending ? true : false}
                            onChange={(e) => this.onInputChange(e.target.value as unknown as SortBy)}>
                        </input>
                        <label className="form-check-label" htmlFor="sortDesc">
                            <i className="fa-solid fa-arrow-up-wide-short"></i>
                            <span className="d-none d-md-block">High to Low</span>
                        </label>
                    </div>         
                </div>                      
            </div>           
            );
    }
}