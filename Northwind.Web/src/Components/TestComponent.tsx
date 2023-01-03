// A component to test getting values from an api.
import React from 'react';
import { Loading } from './Loading';

type TestThing = {
    id: number,
    name: string
};

type TestMeState = {
    isLoading: boolean,
    values: TestThing[]
}

export class TestMe extends React.Component {
    static displayName = TestMe.name;

    state: TestMeState = { isLoading: true, values: []};

    componentDidMount() {
        this.getData(); // async
    }

    componentWillUnmount() {
        this.setState((state) => ({ isLoading: true, values: [] }));
    }

    render() {
        if (this.state.isLoading) {
            return <Loading />
        }
        else {
            return (
                <div>
                <table className='table table-responsive table-striped'>
                    <thead>
                        <tr>
                                <th>Id</th>
                                <td>Name</td>
                        </tr>
                    </thead>
                        <tbody>
                             {this.state.values.map((value: TestThing, index: number) => {
                                return <tr key={index}>
                                    <td>{value.id}</td>
                                    <td>{value.name}</td>
                                </tr>
                            })}
                        </tbody>
                </table>
                </div>
            );
        }        
    }

    async getData() {
        this.setState((state) => ({ isLoading: true, values: [] }));

        const response = await window.fetch('/api/test');

        if (response.ok) {
            const result: TestThing[] = await response.json();
            
            this.setState((state) => ({ isLoading: false, values: result }));            
        }
        else {
            console.error(response.statusText);
        }        
    }
}