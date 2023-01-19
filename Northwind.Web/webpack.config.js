/// <binding AfterBuild='Run - Development' ProjectOpened='Watch - Development' />
// Generated using webpack-cli https://github.com/webpack/webpack-cli

const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const WorkboxWebpackPlugin = require('workbox-webpack-plugin');
const isProduction = process.env.NODE_ENV == 'production';
const stylesHandler = isProduction ? MiniCssExtractPlugin.loader : 'style-loader';

const config = {
    devtool: false,
    entry: './src/main.tsx',
    output: {
        path: path.resolve(__dirname, './wwwroot/app'),
        filename: 'main.js'
    },
    devServer: {
        static: {
            directory: path.resolve(__dirname, './wwwroot/app')
        }
        },
    plugins: [
        // Add your plugins here
        // Learn more about plugins from https://webpack.js.org/configuration/plugins/
    ],
    module: {
        rules: [
            //{
            //    test: /\.(ts|tsx)$/i,
            //    loader: 'ts-loader',
            //    exclude: ['/node_modules/'],
            //},
            {
                test: /\.css$/i,
                use: [stylesHandler,'css-loader'],
            },
            {
                test: /\.s[ac]ss$/i,
                use: [stylesHandler, 'css-loader', 'sass-loader'],
            },
            {
                test: /\.(eot|svg|ttf|woff|woff2|png|jpg|gif)$/i,
                type: 'asset',
            },

            // Add your rules for custom modules here
            // Learn more about loaders from https://webpack.js.org/loaders/     
            {
                test: /\.(js|ts)x?$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                },
            },
        ],
    },
    resolve: {
        extensions: ['.tsx', '.ts', '.jsx', '.js'],
    }
};

module.exports = () => {
    if (isProduction) {
        config.mode = 'production';
        
        config.plugins.push(new MiniCssExtractPlugin());                
        config.plugins.push(new WorkboxWebpackPlugin.GenerateSW());        
    } else {
        config.mode = 'development';
    }
    return config;
};
