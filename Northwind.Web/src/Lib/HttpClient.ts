// HttpClient
// Base class for making api requests

export class HttpClient {
    static displayName = HttpClient.name;

    static async Get<T>(url: string) {
        try {
            const response: Response = await window.fetch(url,
                {
                    mode: 'no-cors',
                    headers: { 'Content-Type': 'application/json' },
                    credentials: 'same-origin',
                });
            console.info(response.statusText);

            const result: T = await response.json();

            return result;     
        } catch (e) {
            console.error('Error getting data from ' + url);
            
            throw new Error('Cannot get data from Api!', { cause: e });
        }          
    }
}