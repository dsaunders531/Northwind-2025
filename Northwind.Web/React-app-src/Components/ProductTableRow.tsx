import React from 'react';
import { ProductApi } from '../Models/ApiModels';
import { EmptyObject } from '../Lib/EmptyObject';


export class ProductTableRow extends React.Component<ProductApi, EmptyObject>
{
    static displayName = ProductTableRow.name;

    constructor(props: ProductApi) {
        super(props);
    }

    state: EmptyObject = {};

    getLinkUrl(productId: number) {
        return "/Product/" + productId;
    }

    render() {
        return (
            <tr>
                <td>{this.props.productId}</td>
                <td>
                    <a href={this.getLinkUrl(this.props.productId)} target="_self">{this.props.productName}</a>
                </td>
                <td>{this.props.quantityPerUnit}</td>
                <td>{this.props.unitPrice.toFixed(2)}</td>
                <td>
                    {this.props.discontinued ? <i className="text-error fa-solid circle-xmark" title="This product has been discontinued"></i> : ''}
                    {this.props.unitsInStock < 5 ? <i className="text-warning fa-solid triangle-exclamation" title="Not many of these left!"></i> : ''}
                </td>
            </tr>
        );
    }
}
