// paging component
import React from 'react';
import { Link } from 'react-router-dom';
import { IPagedResponse, SortBy } from '../Lib/IPagedResponse';

export class Pager<T> extends React.Component<IPagedResponse<T>, IPagedResponse<T>>
{
    static displayName = Pager.name;

    constructor(props: IPagedResponse<T>) {
        super(props);

        this.onLinkClick = this.onLinkClick.bind(this);

        this.state = {
            currentPage: this.props.currentPage,
            itemsPerPage: this.props.itemsPerPage,
            totalPages: this.props.totalPages,
            totalItems: this.props.totalItems,
            page: [] as T[],
            searchTerm: this.props.searchTerm,
            sortOrder: this.props.sortOrder,
            onCurrentPageChanged: (page: number) => this.props.onCurrentPageChanged(page)
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
        onCurrentPageChanged: (page: number) => this.props.onCurrentPageChanged(page)
    }

    onLinkClick(page: number) {
        if ((page > 0 && page <= this.state.totalPages) && page != this.state.currentPage) {                        
            this.props.onCurrentPageChanged(page);

            this.setState((state) => ({ currentPage: page }));
        }
    }

    getPageNoAtPosition(pos: number) {
        // return the page number at a position in the list.
        const pageItems = 3;

        if (this.state.currentPage < pageItems) {
            return pos;
        }
        else if ((this.state.currentPage + (pageItems - 1)) > this.state.totalPages) {
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
                        <li className="page-item" title="Move to start" onClick={(e) => this.onLinkClick(1)}>
                            <Link className={this.state.currentPage == 1 ? 'page-link disabled' : 'page-link'} to={this.getPageUrl(1)}>
                                <i className="fa-solid fa-angles-left"></i>
                            </Link>
                        </li>
                        <li className="page-item" title="Move back" onClick={(e) => this.onLinkClick(this.state.currentPage - 1)}>
                            <Link className={this.state.currentPage == 1 ? 'page-link disabled' : 'page-link'} to={this.getPageUrl(this.state.currentPage - 1)}>
                                <i className="fa-solid fa-chevron-left"></i>
                            </Link>
                        </li>
                        <li className="page-item" onClick={(e) => this.onLinkClick(this.getPageNoAtPosition(1))}>
                            <Link className={this.state.currentPage == this.getPageNoAtPosition(1) ? 'page-link active' : 'page-link'} to={this.getPageUrl(this.getPageNoAtPosition(1))}>
                                {this.getPageNoAtPosition(1)}
                            </Link>                            
                        </li>
                        <li className="page-item" onClick={(e) => this.onLinkClick(this.getPageNoAtPosition(2))}>
                            <Link className={this.state.currentPage == this.getPageNoAtPosition(2) ? 'page-link active' : 'page-link'} to={this.getPageUrl(this.getPageNoAtPosition(2))}>
                                {this.getPageNoAtPosition(2)}
                            </Link>                           
                        </li>
                        <li className="page-item" onClick={(e) => this.onLinkClick(this.getPageNoAtPosition(3))}>
                            <Link className={this.state.currentPage == this.getPageNoAtPosition(3) ? 'page-link active' : 'page-link'} to={this.getPageUrl(this.getPageNoAtPosition(3))}>
                                {this.getPageNoAtPosition(3)}
                            </Link>
                        </li>
                        <li className="page-item" title="Move next" onClick={(e) => this.onLinkClick(this.state.currentPage + 1)}>
                            <Link className={this.state.currentPage == this.state.totalPages ? 'page-link disabled' : 'page-link'} to={this.getPageUrl(this.state.currentPage + 1)}>
                                <i className="fa-solid fa-chevron-right"></i>
                            </Link>
                        </li>
                        <li className="page-item" title="Move to end" onClick={(e) => this.onLinkClick(this.state.totalPages)}>
                            <Link className={this.state.currentPage == this.state.totalPages ? 'page-link disabled' : 'page-link'} to={this.getPageUrl(this.state.totalPages)}>
                                <i className="fa-solid fa-angles-right"></i>
                            </Link>
                        </li>
                    </ul>
                </nav>);            
        }        
    }    
}