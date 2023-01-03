// HelloWorld.tsx
import React from 'react';

export class HelloWorld extends React.Component {
    static displayName = HelloWorld.name;

    render() {
        return (
            <h1><i className="fa-solid fa-sun"></i> Hello World!</h1>
        );
    }
}