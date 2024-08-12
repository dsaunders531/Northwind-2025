// A sort component for lists which use IPagedResponse

import React from 'react';
import { EmptyObject } from '../Lib/EmptyObject';
import { IPagedResponse } from '../Lib/IPagedResponse';
import { SortBy } from "../Lib/SortBy";

export class PageSort<T> extends React.Component<IPagedResponse<T>, EmptyObject> {
    static displayName = PageSort.name;

    constructor(props: IPagedResponse<T>) {
        super(props);

        this.onInputChange = this.onInputChange.bind(this);            
    }

    state: EmptyObject = {};
    
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
                <div className="col-12 col-sm-6 col-md-12">
                    <select id="sort-order" className="form-select"
                        aria-label="Order the list"
                        onChange={(e) => this.onInputChange(e.target.value as unknown as SortBy)}
                        defaultValue={this.getDefaultSortOrder()}
                        data-testid="sort-select">
                        <option value={SortBy.Name} data-testid="sort-by-name">Name</option>
                        <option value={SortBy.Price} data-testid="sort-by-price">Price</option>
                        <option value={SortBy.Popularity} data-testid="sort-by-popularity">Popularity</option>
                    </select>
                </div>                
                <div className="col-12 col-sm-6 col-md-12">
                    <hr className="d-block d-sm-none d-md-block" />
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