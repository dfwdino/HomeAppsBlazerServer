// ─── Screen Wake Lock ────────────────────────────────────────────────────────

let wakeLock: WakeLockSentinel | null = null;

async function requestWakeLock(): Promise<void> {
    if ('wakeLock' in navigator) {
        try {
            wakeLock = await navigator.wakeLock.request('screen');
        } catch (err) {
            // Device may not support wake lock or permission denied — fail silently
            console.warn('Wake lock request failed:', err);
        }
    }
}

async function releaseWakeLock(): Promise<void> {
    if (wakeLock) {
        await wakeLock.release();
        wakeLock = null;
    }
}

// ─── Swipe to Mark as Got ────────────────────────────────────────────────────

let removeSwipeGestures: (() => void) | null = null;

function registerSwipeGestures(containerId: string, dotNetRef: any, threshold: number = 80): void {
    // Drop any listeners from a previous page instance so they never call a disposed reference.
    unregisterSwipeGestures();

    const container = document.getElementById(containerId);
    if (!container) return;

    let startX = 0;

    const onTouchStart = (e: TouchEvent) => {
        startX = e.touches[0].clientX;
    };

    const onTouchEnd = (e: TouchEvent) => {
        const deltaX = e.changedTouches[0].clientX - startX;
        if (deltaX > threshold) {
            const row = (e.target as HTMLElement).closest('tr');
            if (row?.id) {
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

function unregisterSwipeGestures(): void {
    if (removeSwipeGestures) {
        removeSwipeGestures();
        removeSwipeGestures = null;
    }
}
