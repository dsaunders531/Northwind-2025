// paging component for lists which use IPagedResponse
import React from 'react';
import { Link } from 'react-router-dom';
import { EmptyObject } from '../Lib/EmptyObject';
import { IPagedResponse, SortBy } from '../Lib/IPagedResponse';

export class Pager<T> extends React.Component<IPagedResponse<T>, EmptyObject>
{
    static displayName = Pager.name;

    constructor(props: IPagedResponse<T>) {
        super(props);

        this.onLinkClick = this.onLinkClick.bind(this);
    }

    state: EmptyObject = {};
        
    onLinkClick(page: number) {
        if ((page > 0 && page <= this.props.totalPages) && page != this.props.currentPage) {                        
            this.props.onCurrentPageChanged(page);            
        }
    }

    getPageNoAtPosition(pos: number) {
        // return the page number at a position in the list.
        const pageItems = 3;

        if (this.props.currentPage < pageItems) {
            return pos;
        }
        else if ((this.props.currentPage + (pageItems - 1)) > this.props.totalPages) {
            switch (pos) {
                case 1:
                    return this.props.totalPages - 2;
                case 2:
                    return this.props.totalPages - 1;
                default:
                    return this.props.totalPages;
            }
        }
        else {            
            switch (pos) {
                case 1:
                    return this.props.currentPage - 1;                    
                case 2:
                    return this.props.currentPage;                    
                default:
                    return this.props.currentPage + 1;
            }
        }
    }

    getPageUrl(pageNo: number) {
        let url: string = '';

        const params = new URLSearchParams(window.location.search);

        if (params.has('page')) {
            params.delete('page');            
        }

        // also add the sort parameter if not there. It is part of the interface.
        if (!params.has('sort')) {
            params.append('sort', this.props.sortOrder.toString());
        }

        params.append('page', pageNo.toString());

        return url + '?' + params.toString();
    }

    getLinkClassesForPosition(position: number) {
        let result: string = 'page-link';

        if (this.props.currentPage == this.getPageNoAtPosition(position)) {
            result += ' active';
        }
        else if (this.props.totalPages < position) {
            result += ' disabled';
        }

        return result;
    }

    render() {  
        if (this.props.totalPages <= 1) {
            console.warn("No pages to render...");
            return <div></div>
        }
        else {            
            return (
                <nav aria-label="Page Navigation">
                    <ul className="pagination justify-content-center">
                        <li className="page-item" title="Move to start" onClick={(e) => this.onLinkClick(1)}>
                            <Link className={this.props.currentPage == 1 ? 'page-link disabled' : 'page-link'} to={this.getPageUrl(1)}>
                                <i className="fa-solid fa-angles-left"></i>
                            </Link>
                        </li>
                        <li className="page-item" title="Move back" onClick={(e) => this.onLinkClick(this.props.currentPage - 1)}>
                            <Link className={this.props.currentPage == 1 ? 'page-link disabled' : 'page-link'} to={this.getPageUrl(this.props.currentPage - 1)}>
                                <i className="fa-solid fa-chevron-left"></i>
                            </Link>
                        </li>
                        <li className="page-item" onClick={(e) => this.onLinkClick(this.getPageNoAtPosition(1))}>
                            <Link className={this.getLinkClassesForPosition(1)} to={this.getPageUrl(this.getPageNoAtPosition(1))}>
                                {this.getPageNoAtPosition(1)}
                            </Link>                            
                        </li>
                        <li className="page-item" onClick={(e) => this.onLinkClick(this.getPageNoAtPosition(2))}>
                            <Link className={this.getLinkClassesForPosition(2)} to={this.getPageUrl(this.getPageNoAtPosition(2))}>
                                {this.getPageNoAtPosition(2)}
                            </Link>                           
                        </li>
                        <li className="page-item" onClick={(e) => this.onLinkClick(this.getPageNoAtPosition(3))}>
                            <Link className={this.getLinkClassesForPosition(3)} to={this.getPageUrl(this.getPageNoAtPosition(3))}>
                                {this.getPageNoAtPosition(3)}
                            </Link>
                        </li>
                        <li className="page-item" title="Move next" onClick={(e) => this.onLinkClick(this.props.currentPage + 1)}>
                            <Link className={this.props.currentPage == this.props.totalPages ? 'page-link disabled' : 'page-link'} to={this.getPageUrl(this.props.currentPage + 1)}>
                                <i className="fa-solid fa-chevron-right"></i>
                            </Link>
                        </li>
                        <li className="page-item" title="Move to end" onClick={(e) => this.onLinkClick(this.props.totalPages)}>
                            <Link className={this.props.currentPage == this.props.totalPages ? 'page-link disabled' : 'page-link'} to={this.getPageUrl(this.props.totalPages)}>
                                <i className="fa-solid fa-angles-right"></i>
                            </Link>
                        </li>
                    </ul>
                </nav>);            
        }        
    }    
}