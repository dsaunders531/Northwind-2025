﻿// Loading component
import React from 'react';

export class Loading extends React.Component {
    static displayName = Loading.name;

    render() {
        return (<div className="d-flex justify-content-center">
            <div className="spinner-border" role="status">
                <span className="visually-hidden">Loading...</span>
            </div>
        </div>);
    }
}