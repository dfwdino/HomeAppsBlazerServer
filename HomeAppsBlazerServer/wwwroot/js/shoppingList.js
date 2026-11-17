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
let removeSwipeGestures = null;
function registerSwipeGestures(containerId, dotNetRef, threshold = 80) {
    // Drop any listeners from a previous page instance so they never call a disposed reference.
    unregisterSwipeGestures();
    const container = document.getElementById(containerId);
    if (!container)
        return;
    let startX = 0;
    const onTouchStart = (e) => {
        startX = e.touches[0].clientX;
    };
    const onTouchEnd = (e) => {
        const deltaX = e.changedTouches[0].clientX - startX;
        if (deltaX > threshold) {
            const row = e.target.closest('tr');
            if (row === null || row === void 0 ? void 0 : row.id) {
                dotNetRef.invokeMethodAsync('HandleSwipeGot', parseInt(row.id))
                    .catch(() => { /* component already disposed — ignore */ });
            }
        }
    };
    container.addEventListener('touchstart', onTouchStart, { passive: true });
    container.addEventListener('touchend', onTouchEnd, { passive: true });
    removeSwipeGestures = () => {
        container.removeEventListener('touchstart', onTouchStart);
        container.removeEventListener('touchend', onTouchEnd);
    };
}
function unregisterSwipeGestures() {
    if (removeSwipeGestures) {
        removeSwipeGestures();
        removeSwipeGestures = null;
    }
}
