// Put any layout used on every page (like menus, help links etc here)

import React, { Component, ReactNode } from 'react';

type LayoutProps = { children: ReactNode };

type LayoutState = {}; // empty object.

export class Layout extends React.Component<LayoutProps, LayoutState> {
    static displayName:string = Layout.name;

    state: LayoutState = {};

    render() {
        return (<div>{this.props.children}</div>);
    }
}