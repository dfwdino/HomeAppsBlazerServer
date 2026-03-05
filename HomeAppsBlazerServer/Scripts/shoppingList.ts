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

function registerSwipeGestures(containerId: string, dotNetRef: any, threshold: number = 80): void {
    const container = document.getElementById(containerId);
    if (!container) return;

    let startX = 0;

    container.addEventListener('touchstart', (e: TouchEvent) => {
        startX = e.touches[0].clientX;
    }, { passive: true });

    container.addEventListener('touchend', (e: TouchEvent) => {
        const deltaX = e.changedTouches[0].clientX - startX;
        if (deltaX > threshold) {
            const row = (e.target as HTMLElement).closest('tr');
            if (row?.id) {
                dotNetRef.invokeMethodAsync('HandleSwipeGot', parseInt(row.id));
            }
        }
    }, { passive: true });
}
