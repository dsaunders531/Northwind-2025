import React from "react";

export function BoilingVerdict(props: any) {
    if (props.celsius >= 100) {
        return <p>The water would boil.</p>;
    }
    return <p>The water would not boil.</p>;
}

export function toCelsius(fahrenheit: number) {
    return (fahrenheit - 32) * 5 / 9;
}

export function toFahrenheit(celsius: number) {
    return (celsius * 9 / 5) + 32;
}

function tryConvert(temperature: string, convert: CallableFunction) {
    const input = parseFloat(temperature);
    if (Number.isNaN(input)) {
        return '';
    }
    const output = convert(input);
    const rounded = Math.round(output * 1000) / 1000;
    return rounded.toString();
}

enum ScaleNames {
    c = 'Celsius',
    f = 'Fahrenheit'
};

export type CalculatorState = {
    temperature: string,
    scale: ScaleNames,
    onTemperatureChange(value: string): void
}

export class TemperatureInput extends React.Component<CalculatorState, CalculatorState> {
    constructor(props: CalculatorState) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
        this.state = {
            temperature: props.temperature,
            scale: props.scale,
            onTemperatureChange: (value: string) => ({})
        }
    }

    handleChange(e: { target: { value: any; }; }) {
        this.props.onTemperatureChange(e.target.value);
    }

    render() {        
        return (
            <fieldset>
                <legend>Enter temperature in {this.props.scale}:</legend>
                <input value={this.props.temperature}
                    onChange={this.handleChange} />
            </fieldset>
        );
    }
}

export class Calculator extends React.Component<any, CalculatorState> {
    constructor(props: any) {
        super(props);
        //this.handleChange = this.handleChange.bind(this);
        this.handleCelsiusChange = this.handleCelsiusChange.bind(this);
        this.handleFahrenheitChange = this.handleFahrenheitChange.bind(this);
        this.state = { temperature: '', scale: ScaleNames.c, onTemperatureChange: (value: string) => ({}) };
    }

    //handleChange(e: { target: { value: any; }; }) {
    //    this.setState({ temperature: e.target.value });
    //}

    handleCelsiusChange(temperature: string) {
        this.setState({ scale: ScaleNames.c, temperature });
    }

    handleFahrenheitChange(temperature: string) {
        this.setState({ scale: ScaleNames.f, temperature });
    }

    render() {
        const temperature = this.state.temperature;
        const scale = this.state.scale;
        const celsius = scale === ScaleNames.c ? tryConvert(temperature, toCelsius) : temperature;
        const fahrenheit = scale === ScaleNames.f ? tryConvert(temperature, toFahrenheit) : temperature;

        return (
            <div>
                <TemperatureInput scale={ScaleNames.c} onTemperatureChange={this.handleCelsiusChange} temperature={celsius} />
                <TemperatureInput scale={ScaleNames.f} onTemperatureChange={this.handleFahrenheitChange} temperature={fahrenheit} />
                <BoilingVerdict celcius={parseFloat(celsius)} />
            </div>            
        );
    }
}