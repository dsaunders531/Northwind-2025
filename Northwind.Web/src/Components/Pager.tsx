// paging component
import React from 'react';
import { IPagedResponse, SortBy } from '../Lib/IPagedResponse';

export class Pager<T> extends React.Component<IPagedResponse<T>, IPagedResponse<T>>
{
    static displayName = Pager.name;

    constructor(props: IPagedResponse<T>) {
        super(props);

        this.state = {
            currentPage: props.currentPage,
            itemsPerPage: props.itemsPerPage,
            totalPages: props.totalPages,
            totalItems: props.totalItems,
            page: [] as T[],
            searchTerm: props.searchTerm,
            sortOrder: props.sortOrder
        };        
    }

    state: IPagedResponse<T> = {
        totalItems: 0,
        totalPages: 0,
        itemsPerPage: 0,
        currentPage: 0,
        searchTerm: '',
        sortOrder: SortBy.Name | SortBy.Ascending,
        page: []
    }


    getPageNoAtPosition(pos: number) {
        // return the page number at a position in the list.
        const pageItems = 3;

        if (this.state.currentPage < pageItems) {
            return pos;
        }
        else if ((this.state.currentPage + (pageItems -1)) > this.state.totalPages) {
            switch (pos) {
                case 1:
                    return this.state.totalPages - 2;
                case 2:
                    return this.state.totalPages - 1;
                default:
                    return this.state.totalPages;
            }
        }
        else {            
            switch (pos) {
                case 1:
                    return this.state.currentPage - 1;                    
                case 2:
                    return this.state.currentPage;                    
                default:
                    return this.state.currentPage + 1;
            }
        }

    }

    getPageUrl(pageNo: number) {
        let url: string = '';

        const params = new URLSearchParams(window.location.search);

        if (params.has('page')) {
            params.delete('page');            
        }

        params.append('page', pageNo.toString());

        return url + '?' + params.toString();
    }

    render() {  
        if (this.state.totalPages <= 1) {
            return <div></div>
        }
        else {            
            return (
                <nav aria-label="Page Navigation">
                    <ul className="pagination justify-content-center">
                        <li className="page-item" title="Move to start">
                            <a className={this.state.currentPage == 1 ? 'page-link disabled' : 'page-link'}
                                href={this.getPageUrl(1)}>
                                <i className="fa-solid fa-angles-left"></i>
                            </a>
                        </li>
                        <li className="page-item" title="Move back">
                            <a className={this.state.currentPage == 1 ? 'page-link disabled' : 'page-link'}
                                href={this.getPageUrl(this.state.currentPage - 1)}>
                                <i className="fa-solid fa-chevron-left"></i>
                            </a>
                        </li>
                        <li className="page-item">
                            <a className={this.state.currentPage == this.getPageNoAtPosition(1) ? 'page-link active' : 'page-link'} 
                                href={this.getPageUrl(this.getPageNoAtPosition(1))}>
                                {this.getPageNoAtPosition(1)}
                            </a>
                        </li>
                        <li className="page-item">
                            <a className={this.state.currentPage == this.getPageNoAtPosition(2) ? 'page-link active' : 'page-link'} 
                                href={this.getPageUrl(this.getPageNoAtPosition(2))}>
                                {this.getPageNoAtPosition(2)}
                            </a>
                        </li>
                        <li className="page-item">
                            <a className={this.state.currentPage == this.getPageNoAtPosition(3) ? 'page-link active' : 'page-link'} 
                                href={this.getPageUrl(this.getPageNoAtPosition(3))}>
                                {this.getPageNoAtPosition(3)}
                            </a>
                        </li>
                        <li className="page-item" title="Move next">
                            <a className={this.state.currentPage == this.state.totalPages ? 'page-link disabled' : 'page-link'}
                                href={this.getPageUrl(this.state.currentPage + 1)}>
                                <i className="fa-solid fa-chevron-right"></i>
                            </a>
                        </li>
                        <li className="page-item" title="Move to end">
                            <a className={this.state.currentPage == this.state.totalPages ? 'page-link disabled' : 'page-link'}
                                href={this.getPageUrl(this.state.totalPages)}>
                                <i className="fa-solid fa-angles-right"></i>
                            </a>
                        </li>
                    </ul>
                </nav>);            
        }        
    }    
}