// A search componenet for lists which use IPagedResponse

import React, { useState } from 'react';
import { IPagedResponse } from '../Lib/IPagedResponse';
import { SortBy } from "../Lib/SortBy";
import { ProductsService } from '../Services/ProductsService';

type SearchState = {
    timerRef: number,
    term: string
}

export class PageSearch<T> extends React.Component<IPagedResponse<T>, SearchState> {
    static displayName = PageSearch.name;

    constructor(props: IPagedResponse<T>) {
        super(props);

        this.onSearchChange = this.onSearchChange.bind(this);
    }

    state: SearchState = { timerRef: -1, term: '' };

    onSearchChange(term: string) {
        window.clearTimeout(this.state.timerRef);

        this.getData(term);
    }

    onSearchSelected(term: string) {
        window.clearTimeout(this.state.timerRef);

        if (term != this.props.searchTerm) {
            window.setTimeout(() => { this.props.onSearchTermChanged(this.state.term); }, 1200);
            this.setState((state) => ({
                term: term,
                timerRef: window.setTimeout(() => { this.props.onSearchTermChanged(this.state.term); }, 1200)
            }));
        }      
    }

    async getData(term: string) {
        try {
            if (term.length >= 3) {
                let result: string[] = await ProductsService.Search(term);
                
                let ele: HTMLElement = document.getElementById('pageSearchItems');
                ele.innerHTML = '';                

                if (result.length > 0) {
                    this.onSearchSelected(term);
                }
                else {
                    // always show some products.
                    this.onSearchSelected('');
                }

                for (var i = 0; i < result.length; i++) {
                    ele.innerHTML += '<option value="' + result[i] + '">' + result[i] + '</option>';
                }
            }   
            else {
                let ele: HTMLElement = document.getElementById('pageSearchItems');
                ele.innerHTML = '';   
                this.onSearchSelected('');
            }            
        } catch (e) {
            console.error('Cannot get any matching products' + e);
        }        
    }

    render() {
        return (
            <div className="row">
                <div className="col-12">
                    <div className="input-group">
                        <span className="input-group-text" id="pageSearch">
                            <i className="fa-solid fa-magnifying-glass"></i>
                        </span>
                        <input id="search-term" aria-label="Search for a product"
                            type="search"
                            onChange={(e) => this.onSearchChange(e.target.value)}                                                        
                            defaultValue={this.props.searchTerm}
                            className="form-control"
                            placeholder="Search for a product"
                            aria-describedby="pageSearch"
                            list="pageSearchItems"
                            spellCheck={false}
                            autoComplete='on'  
                            data-testid="search-input"
                        ></input>
                    </div>                    
                </div>
                <datalist id="pageSearchItems">                    
                </datalist>
            </div>            
            );
    }
}