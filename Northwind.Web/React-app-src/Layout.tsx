// Put any layout used on every page (like menus, help links etc here)

import React, { Component, ReactNode } from 'react';
import { EmptyObject } from './Lib/EmptyObject';
import { NavMenu } from './NavMenu';

type LayoutProps = { children: ReactNode };

export class Layout extends React.Component<LayoutProps, EmptyObject>
{
    static displayName:string = Layout.name;

    state: EmptyObject = {};

    render() {
        return (            
            <div>
                <NavMenu />
                <br />
                {this.props.children}
            </div>
        );
    }
}