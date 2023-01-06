
// Show the categories

// TODO this needs paging, filter and sort options (as components which can be reused for the product things)

import React from 'react';
import { IPagedResponse, SortBy } from '../Lib/IPagedResponse';
import { CategoryApi } from '../Models/ApiModels';
import { CategoriesService } from '../Services/CategoriesService';
import { Loading } from './Loading';


type CategoriesState = {
    isLoading: boolean,
    currentPage?: IPagedResponse<CategoryApi>
}

export class Categories extends React.Component {
    static displayName = Categories.name;

    state: CategoriesState = {
        isLoading: true,
        currentPage: null
    }

    componentDidMount() {
        this.getData(); // async
    }

    componentWillUnmount() {
        this.setState((state) => ({ isLoading: true, currentPage: null }));
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
                <div>
                    <table className="table table-responsive table-striped">
                        <thead>
                            <tr>
                                <th>Id</th>
                                <td>Name</td>
                                <td>Description</td>
                            </tr>
                        </thead>
                        <tbody>
                            {this.state.currentPage.page.length == 0 ?
                                <tr><td>Nothing Found!</td></tr> :
                                this.state.currentPage.page.map((value: CategoryApi, index: number) => {
                                    return <tr key={index}>
                                        <td>{value.categoryId}</td>
                                        <td>{value.categoryName}</td>
                                        <td>{value.description}</td>
                                    </tr>
                                })}
                        </tbody>
                    </table>
                </div>
                );
        }
    }

    async getData() {        
        try {
            let page: number = this.state.currentPage?.currentPage ?? 1;
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