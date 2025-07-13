self.importScripts('service-worker-assets.js');

const CACHE_NAME = 'hotelme-cache-v1';
const ASSETS = self.assetsManifest.assets
    .map(a => a.url)
    .concat([
        '/',
        'index.html',
        'manifest.json'
    ]);

self.addEventListener('install', evt => {
    evt.waitUntil(
        caches.open(CACHE_NAME).then(cache => cache.addAll(ASSETS))
    );
    self.skipWaiting();
});

self.addEventListener('activate', evt => {
    evt.waitUntil(self.clients.claim());
});

self.addEventListener('fetch', evt => {
    if (evt.request.method !== 'GET') return;
    evt.respondWith(
        caches.match(evt.request)
            .then(cacheRes => cacheRes || fetch(evt.request))
    );
});
