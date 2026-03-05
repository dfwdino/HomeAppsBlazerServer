"use strict";
// ─── Screen Wake Lock ────────────────────────────────────────────────────────
let wakeLock = null;
async function requestWakeLock() {
    if ('wakeLock' in navigator) {
        try {
            wakeLock = await navigator.wakeLock.request('screen');
        }
        catch (err) {
            // Device may not support wake lock or permission denied — fail silently
            console.warn('Wake lock request failed:', err);
        }
    }
}
async function releaseWakeLock() {
    if (wakeLock) {
        await wakeLock.release();
        wakeLock = null;
    }
}
// ─── Swipe to Mark as Got ────────────────────────────────────────────────────
function registerSwipeGestures(containerId, dotNetRef, threshold = 80) {
    const container = document.getElementById(containerId);
    if (!container)
        return;
    let startX = 0;
    container.addEventListener('touchstart', (e) => {
        startX = e.touches[0].clientX;
    }, { passive: true });
    container.addEventListener('touchend', (e) => {
        const deltaX = e.changedTouches[0].clientX - startX;
        if (deltaX > threshold) {
            const row = e.target.closest('tr');
            if (row === null || row === void 0 ? void 0 : row.id) {
                dotNetRef.invokeMethodAsync('HandleSwipeGot', parseInt(row.id));
            }
        }
    }, { passive: true });
}
