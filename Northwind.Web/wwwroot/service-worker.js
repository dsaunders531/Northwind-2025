'use strict';

const cacheName = "v1";

const devmode = true; // do not store files locally.

// A list of paths not to cache

self.addEventListener("install", (event) => {
    console.info('Installing Service Worker');
    event.waitUntil(
        addResourcesToCache([
            "/css/bootstrap.min.css",
            "/lib/bootstrap/5.3/dist/js/bootstrap.bundle.min.js",
            "/lib/fontawesome-free-6.2.1-web/css/solid.min.css",
            "/lib/fontawesome-free-6.2.1-web/css/fontawesome.min.css"
        ])
    );
});

self.addEventListener('fetch', (event) => {
    console.trace("Fetching Service Worker Content");
    event.respondWith(
        cacheFirst({ request: event.request, preloadReponsePromise: event.preloadReponse })
    );
});

self.addEventListener('activate', (event) => {
    console.info("Activating Cache Sevice Worker");
    event.waitUntil(enableNavigationPreload());
    event.waitUntil(deleteOldCaches());
});

const addResourcesToCache = async (resources) => {
    // store everything in cache
    const cache = await caches.open(cacheName);
    await cache.addAll(resources);
};

const putInCache = async (request, response) => {
    // store new in cache
    const cache = await caches.open(cacheName);
    await cache.put(request, response);
}

const cacheFirst = async ({ request, preloadReponsePromise }) => {
    var blocked = false;

    if (blocked) {
        console.info("Url is blocked from cache - getting from network");
        try {
            await fetch(request);
        } catch (e) {
            console.error("Getting from network failed: " + JSON.stringify(e));
            // when even the fallback response is not available,
            // there is nothing we can do, but we must always
            // return a Response object
            return new Response('Network error happened', {
                status: 408,
                headers: { 'Content-Type': 'text/plain' },
            });
        }        
    }
    else {
        const responseFromCache = await caches.match(request);
        if (responseFromCache) {
            return responseFromCache;
        }

        const preloadResponse = await preloadReponsePromise;
        if (preloadResponse) {
            console.trace('Using preload response', preloadResponse);
            putInCache(request, preloadResponse.clone());
            return preloadResponse;
        }

        try {
            console.trace("Getting " + request + " from network");
            const responseFromNetwork = await fetch(request);

            if (!devmode) {
                // this stores the request for next time.
                putInCache(request, responseFromNetwork.clone())
            }
            
            return responseFromNetwork;
        } catch (e) {
            console.error("Getting from network failed: " + JSON.stringify(e));
            // when even the fallback response is not available,
            // there is nothing we can do, but we must always
            // return a Response object
            return new Response('Network error happened', {
                status: 408,
                headers: { 'Content-Type': 'text/plain' },
            });
        }
    }    
};

const enableNavigationPreload = async () => {
    console.info("Enable preload");
    if (self.registration.navigationPreload) {
        // Enable navigation preloads!
        await self.registration.navigationPreload.enable();
    }
};

const deleteCache = async key => {
    console.warn("Deleting content from cache");
    await caches.delete(key)
}

const deleteOldCaches = async () => {
    console.warn("Deleting caches");
    const cacheKeepList = [cacheName];
    const keyList = await caches.keys()
    const cachesToDelete = keyList.filter(key => !cacheKeepList.includes(key))
    await Promise.all(cachesToDelete.map(deleteCache));
}