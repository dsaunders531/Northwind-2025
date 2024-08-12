/** @type {import('jest').Config} */

const config = {
    verbose: true,
    bail: 1,
    collectCoverageFrom: [
        '**/*.{ts,tsx,js,jsx}'
    ],
    testEnvironment: 'jsdom',
    coveragePathIgnorePatterns: ['/node_modules/', '/bin/', '/obj/', '/wwwroot/'],
    roots: ['<rootDir>/React-app-src/'],   
};

module.exports = config;