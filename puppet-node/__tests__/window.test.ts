/**
 * Puppet Framework Window Controller Tests
 * 
 * Tests for the WindowController API
 */

import {
    setupTestEnvironment,
    cleanupTestEnvironment,
    wait,
    waitForCondition,
    assertEqual,
    assertTruthy,
    assertFalsy,
    createSpy
} from '../test-utils';

describe('WindowController', () => {
    let puppet: any;

    beforeEach(() => {
        puppet = setupTestEnvironment({ enableLogging: false, resetState: true });
    });

    afterEach(() => {
        cleanupTestEnvironment();
    });

    describe('setBorderless', () => {
        it('should set window to borderless mode', async () => {
            await puppet.window.setBorderless(true);
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            assertEqual(state?.window.borderless, true, 'Window should be borderless');
        });

        it('should restore window border', async () => {
            await puppet.window.setBorderless(true);
            await puppet.window.setBorderless(false);
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            assertEqual(state?.window.borderless, false, 'Window should have border');
        });
    });

    describe('setDraggable', () => {
        it('should enable window dragging', async () => {
            await puppet.window.setDraggable(true);
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            assertEqual(state?.window.draggable, true, 'Window should be draggable');
        });

        it('should disable window dragging', async () => {
            await puppet.window.setDraggable(false);
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            assertEqual(state?.window.draggable, false, 'Window should not be draggable');
        });
    });

    describe('setResizable', () => {
        it('should enable window resizing', async () => {
            await puppet.window.setResizable(true);
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            assertEqual(state?.window.resizable, true, 'Window should be resizable');
        });

        it('should disable window resizing', async () => {
            await puppet.window.setResizable(false);
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            assertEqual(state?.window.resizable, false, 'Window should not be resizable');
        });
    });

    describe('setOpacity', () => {
        it('should set window opacity to 0.5', async () => {
            await puppet.window.setOpacity(0.5);
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            assertEqual(state?.window.opacity, 0.5, 'Window opacity should be 0.5');
        });

        it('should set window opacity to 1.0 (fully opaque)', async () => {
            await puppet.window.setOpacity(1.0);
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            assertEqual(state?.window.opacity, 1.0, 'Window should be fully opaque');
        });

        it('should set window opacity to 0.0 (fully transparent)', async () => {
            await puppet.window.setOpacity(0.0);
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            assertEqual(state?.window.opacity, 0.0, 'Window should be fully transparent');
        });
    });

    describe('moveWindow', () => {
        it('should move window to specified position', async () => {
            await puppet.window.moveWindow(100, 200);
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            assertEqual(state?.window.x, 100, 'Window x position should be 100');
            assertEqual(state?.window.y, 200, 'Window y position should be 200');
        });

        it('should handle negative coordinates', async () => {
            await puppet.window.moveWindow(-50, -100);
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            assertEqual(state?.window.x, -50, 'Window should handle negative x');
            assertEqual(state?.window.y, -100, 'Window should handle negative y');
        });
    });

    describe('resizeWindow', () => {
        it('should resize window to specified dimensions', async () => {
            await puppet.window.resizeWindow(1024, 768);
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            assertEqual(state?.window.width, 1024, 'Window width should be 1024');
            assertEqual(state?.window.height, 768, 'Window height should be 768');
        });

        it('should handle very small dimensions', async () => {
            await puppet.window.resizeWindow(100, 50);
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            assertEqual(state?.window.width, 100, 'Window should handle small width');
            assertEqual(state?.window.height, 50, 'Window should handle small height');
        });
    });

    describe('centerWindow', () => {
        it('should center window on screen', async () => {
            await puppet.window.resizeWindow(800, 600);
            await puppet.window.centerWindow();
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            const expectedX = Math.floor((1920 - 800) / 2); // (screenWidth - windowWidth) / 2
            const expectedY = Math.floor((1080 - 600) / 2); // (screenHeight - windowHeight) / 2
            
            assertEqual(state?.window.x, expectedX, 'Window should be centered horizontally');
            assertEqual(state?.window.y, expectedY, 'Window should be centered vertically');
        });
    });

    describe('setTopmost', () => {
        it('should set window to always on top', async () => {
            await puppet.window.setTopmost(true);
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            assertTruthy(state?.window.topmost, 'Window should be on top');
        });

        it('should disable always on top', async () => {
            await puppet.window.setTopmost(false);
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            assertFalsy(state?.window.topmost, 'Window should not be on top');
        });
    });

    describe('showInTaskbar', () => {
        it('should show window in taskbar', async () => {
            await puppet.window.showInTaskbar(true);
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            assertTruthy(state?.window.inTaskbar, 'Window should be in taskbar');
        });

        it('should hide window from taskbar', async () => {
            await puppet.window.showInTaskbar(false);
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            assertFalsy(state?.window.inTaskbar, 'Window should not be in taskbar');
        });
    });

    describe('mountMovableElement', () => {
        it('should mount element as movable', async () => {
            await puppet.window.mountMovableElement('test-element');
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            assertTruthy(
                state?.window.movableElements.has('test-element'),
                'Element should be mounted as movable'
            );
        });

        it('should mount multiple elements', async () => {
            await puppet.window.mountMovableElement('element-1');
            await puppet.window.mountMovableElement('element-2');
            await puppet.window.mountMovableElement('element-3');
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            assertEqual(state?.window.movableElements.size, 3, 'Should have 3 movable elements');
        });
    });

    describe('unmountMovableElement', () => {
        it('should unmount movable element', async () => {
            await puppet.window.mountMovableElement('test-element');
            await puppet.window.unmountMovableElement('test-element');
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            assertFalsy(
                state?.window.movableElements.has('test-element'),
                'Element should not be movable'
            );
        });
    });

    describe('integration tests', () => {
        it('should setup window with custom configuration', async () => {
            await puppet.window.setBorderless(true);
            await puppet.window.setDraggable(true);
            await puppet.window.setResizable(false);
            await puppet.window.setOpacity(0.9);
            await puppet.window.setTopmost(true);
            await puppet.window.moveWindow(200, 300);
            await puppet.window.resizeWindow(600, 400);
            await puppet.window.centerWindow();
            
            const state = puppet.__getMockState?.() ?? (global as any).__mockState;
            
            assertEqual(state?.window.borderless, true, 'Should be borderless');
            assertEqual(state?.window.draggable, true, 'Should be draggable');
            assertEqual(state?.window.resizable, false, 'Should not be resizable');
            assertEqual(state?.window.opacity, 0.9, 'Opacity should be 0.9');
            assertEqual(state?.window.topmost, true, 'Should be on top');
        });
    });
});

// ============================================================================
// Manual Test Function
// ============================================================================

/**
 * Run window controller tests manually
 */
export async function runWindowTests(): Promise<void> {
    console.log('=== Running Window Controller Tests ===\n');
    
    let passed = 0;
    let failed = 0;
    
    // Test 1: setBorderless
    try {
        const puppet = setupTestEnvironment({ enableLogging: false });
        await puppet.window.setBorderless(true);
        const state = (global as any).__mockState;
        if (state?.window.borderless === true) {
            console.log('✓ Test 1 passed: setBorderless(true)');
            passed++;
        } else {
            console.log('✗ Test 1 failed: setBorderless(true)');
            failed++;
        }
        cleanupTestEnvironment();
    } catch (error) {
        console.log('✗ Test 1 failed:', error);
        failed++;
    }
    
    // Test 2: setOpacity
    try {
        const puppet = setupTestEnvironment({ enableLogging: false });
        await puppet.window.setOpacity(0.5);
        const state = (global as any).__mockState;
        if (state?.window.opacity === 0.5) {
            console.log('✓ Test 2 passed: setOpacity(0.5)');
            passed++;
        } else {
            console.log('✗ Test 2 failed: setOpacity(0.5)');
            failed++;
        }
        cleanupTestEnvironment();
    } catch (error) {
        console.log('✗ Test 2 failed:', error);
        failed++;
    }
    
    // Test 3: moveWindow
    try {
        const puppet = setupTestEnvironment({ enableLogging: false });
        await puppet.window.moveWindow(100, 200);
        const state = (global as any).__mockState;
        if (state?.window.x === 100 && state?.window.y === 200) {
            console.log('✓ Test 3 passed: moveWindow(100, 200)');
            passed++;
        } else {
            console.log('✗ Test 3 failed: moveWindow(100, 200)');
            failed++;
        }
        cleanupTestEnvironment();
    } catch (error) {
        console.log('✗ Test 3 failed:', error);
        failed++;
    }
    
    // Test 4: resizeWindow
    try {
        const puppet = setupTestEnvironment({ enableLogging: false });
        await puppet.window.resizeWindow(800, 600);
        const state = (global as any).__mockState;
        if (state?.window.width === 800 && state?.window.height === 600) {
            console.log('✓ Test 4 passed: resizeWindow(800, 600)');
            passed++;
        } else {
            console.log('✗ Test 4 failed: resizeWindow(800, 600)');
            failed++;
        }
        cleanupTestEnvironment();
    } catch (error) {
        console.log('✗ Test 4 failed:', error);
        failed++;
    }
    
    // Test 5: centerWindow
    try {
        const puppet = setupTestEnvironment({ enableLogging: false });
        await puppet.window.resizeWindow(800, 600);
        await puppet.window.centerWindow();
        const state = (global as any).__mockState;
        const expectedX = 560; // (1920 - 800) / 2
        const expectedY = 240; // (1080 - 600) / 2
        if (state?.window.x === expectedX && state?.window.y === expectedY) {
            console.log('✓ Test 5 passed: centerWindow()');
            passed++;
        } else {
            console.log('✗ Test 5 failed: centerWindow()');
            failed++;
        }
        cleanupTestEnvironment();
    } catch (error) {
        console.log('✗ Test 5 failed:', error);
        failed++;
    }
    
    console.log(`\n=== Test Results ===`);
    console.log(`Passed: ${passed}`);
    console.log(`Failed: ${failed}`);
    console.log(`Total: ${passed + failed}`);
}

// Export for use in Node.js or browser
if (typeof module !== 'undefined' && module.exports) {
    module.exports = { runWindowTests };
}