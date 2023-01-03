// The menu for the app.
// You do not need to do anything here.
// Put your items in AppRoutes and they will be drawn by this component.

import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { AppRoute } from './Lib/AppRoute';
import { EmptyObject } from './Lib/EmptyObject';

type MenuState = {
    activeTab: string;
};

export class NavMenu extends React.Component<EmptyObject, MenuState>
{
    static displayName = NavMenu.name;

    state: MenuState = {
        activeTab: ''
    };

    UpdateActiveTab(tab: string) {
        // if the activetab is not set - use the first route[].index = true value.
        if (tab.length == 0) {
            tab = AppRoutes.filter((route: AppRoute, index: number) => { return route.index })[0].name;
        }

        this.setState((state) => ({ activeTab: tab }));        
    }

    componentDidMount() {
        // work out what the active tab is...
        this.UpdateActiveTab('');
    }

    getLinkClasses(routeName: string) {
        var result: string = 'nav-link string-transform-title-case';

        if (routeName == this.state.activeTab) {
            result += ' active';
        }

        return result;
    }

    render() {
        return (
            <ul className="nav nav-tabs">
                {AppRoutes.sort((a: AppRoute, b: AppRoute) => { return Math.pow(b.sortOrder, a.sortOrder) })
                    .map((route: AppRoute, index: number) => {                     
                    return <li key={index} className='nav-item'>
                        <Link
                            className={this.getLinkClasses(route.name)}
                            to={route.path}
                            onClick={() => this.UpdateActiveTab(route.name)}>
                            {route.iconClass.length > 0 ? <i className={route.iconClass}></i> : route.name}
                        </Link>
                    </li>;
                })}
            </ul>            
        );
    }
}