/**
 * Puppet Framework Test Utilities
 * 
 * This module provides utility functions for testing Puppet Framework applications.
 * 
 * @author Puppet Framework Team
 * @version 1.0.0
 */

import type { PuppetNamespace } from './puppet';
import { createMockPuppetNamespace, getMockState, resetMockState, simulateEvent, simulateTrayClick, simulateTrayDoubleClick } from './mock';

// ============================================================================
// Test Environment Setup
// ============================================================================

/**
 * Setup test environment with mock puppet
 * 
 * @param options - Test options
 * @returns The mock puppet namespace
 * 
 * @example
 * ```typescript
 * import { setupTestEnvironment } from '@puppet-framework/types/test-utils';
 * 
 * const puppet = setupTestEnvironment({ enableLogging: false });
 * 
 * // Run your tests
 * await testYourFunction(puppet);
 * ```
 */
export function setupTestEnvironment(options: {
    enableLogging?: boolean;
    resetState?: boolean;
} = {}): PuppetNamespace {
    const { enableLogging = true, resetState = true } = options;
    
    if (resetState) {
        resetMockState();
    }
    
    const mockPuppet = createMockPuppetNamespace(enableLogging);
    
    // Make puppet available globally for testing
    if (typeof window !== 'undefined') {
        (window as any).puppet = mockPuppet;
    }
    
    return mockPuppet;
}

/**
 * Cleanup test environment
 * 
 * @example
 * ```typescript
 * import { cleanupTestEnvironment } from '@puppet-framework/types/test-utils';
 * 
 * afterAll(() => {
 *     cleanupTestEnvironment();
 * });
 * ```
 */
export function cleanupTestEnvironment(): void {
    if (typeof window !== 'undefined') {
        delete (window as any).puppet;
    }
    resetMockState();
}

// ============================================================================
// Test Helpers
// ============================================================================

/**
 * Wait for a specified time
 * 
 * @param ms - Milliseconds to wait
 * @returns Promise that resolves after the specified time
 */
export function wait(ms: number): Promise<void> {
    return new Promise(resolve => setTimeout(resolve, ms));
}

/**
 * Wait for a condition to be true
 * 
 * @param condition - Function that returns true when condition is met
 * @param timeout - Maximum time to wait in milliseconds
 * @param interval - Check interval in milliseconds
 * @returns Promise that resolves when condition is met or rejects on timeout
 */
export async function waitForCondition(
    condition: () => boolean,
    timeout: number = 5000,
    interval: number = 100
): Promise<void> {
    const startTime = Date.now();
    
    while (Date.now() - startTime < timeout) {
        if (condition()) {
            return;
        }
        await wait(interval);
    }
    
    throw new Error(`Condition not met within ${timeout}ms`);
}

/**
 * Create a spy function that tracks calls
 * 
 * @param fn - Optional function to wrap
 * @returns Spy function
 * 
 * @example
 * ```typescript
 * const spy = createSpy();
 * await puppet.window.setBorderless(true);
 * expect(spy.calls).toHaveLength(1);
 * ```
 */
export function createSpy<T extends (...args: any[]) => any>(fn?: T): T & { calls: Array<Parameters<T>>; callCount: number } {
    const calls: Array<Parameters<T>> = [];
    
    const spy = ((...args: Parameters<T>) => {
        calls.push(args);
        return fn ? fn(...args) : undefined;
    }) as T & { calls: Array<Parameters<T>>; callCount: number };
    
    Object.defineProperty(spy, 'calls', {
        get: () => calls,
        enumerable: true
    });
    
    Object.defineProperty(spy, 'callCount', {
        get: () => calls.length,
        enumerable: true
    });
    
    return spy;
}

/**
 * Mock a method on an object
 * 
 * @param obj - Object to mock method on
 * @param methodName - Name of the method to mock
 * @param mockFn - Mock function
 * @returns Original method for restoration
 * 
 * @example
 * ```typescript
 * const original = mockMethod(puppet.window, 'setBorderless', async () => {});
 * // Run tests
 * restoreMethod(puppet.window, 'setBorderless', original);
 * ```
 */
export function mockMethod<T extends object, K extends keyof T>(
    obj: T,
    methodName: K,
    mockFn: T[K]
): T[K] {
    const original = obj[methodName];
    obj[methodName] = mockFn;
    return original;
}

/**
 * Restore a mocked method
 * 
 * @param obj - Object with mocked method
 * @param methodName - Name of the method to restore
 * @param original - Original method
 */
export function restoreMethod<T extends object, K extends keyof T>(
    obj: T,
    methodName: K,
    original: T[K]
): void {
    obj[methodName] = original;
}

// ============================================================================
// Assert Helpers
// ============================================================================

/**
 * Assert that a value is truthy
 * 
 * @param value - Value to check
 * @param message - Error message if assertion fails
 * @throws Error if assertion fails
 */
export function assertTruthy(value: any, message?: string): void {
    if (!value) {
        throw new Error(message || `Expected ${value} to be truthy`);
    }
}

/**
 * Assert that a value is falsy
 * 
 * @param value - Value to check
 * @param message - Error message if assertion fails
 * @throws Error if assertion fails
 */
export function assertFalsy(value: any, message?: string): void {
    if (value) {
        throw new Error(message || `Expected ${value} to be falsy`);
    }
}

/**
 * Assert that two values are equal
 * 
 * @param actual - Actual value
 * @param expected - Expected value
 * @param message - Error message if assertion fails
 * @throws Error if assertion fails
 */
export function assertEqual<T>(actual: T, expected: T, message?: string): void {
    if (actual !== expected) {
        throw new Error(message || `Expected ${expected} but got ${actual}`);
    }
}

/**
 * Assert that two values are not equal
 * 
 * @param actual - Actual value
 * @param unexpected - Unexpected value
 * @param message - Error message if assertion fails
 * @throws Error if assertion fails
 */
export function assertNotEqual<T>(actual: T, unexpected: T, message?: string): void {
    if (actual === unexpected) {
        throw new Error(message || `Expected ${actual} to not equal ${unexpected}`);
    }
}

/**
 * Assert that a function throws an error
 * 
 * @param fn - Function to test
 * @param message - Error message if assertion fails
 * @throws Error if assertion fails (function didn't throw)
 */
export async function assertThrows(fn: () => Promise<any> | any, message?: string): Promise<void> {
    try {
        await fn();
        throw new Error(message || 'Expected function to throw');
    } catch (error) {
        if (error instanceof Error && error.message === message) {
            throw error;
        }
        // Function threw, which is expected
    }
}

/**
 * Assert that an array contains an item
 * 
 * @param array - Array to check
 * @param item - Item to find
 * @param message - Error message if assertion fails
 * @throws Error if assertion fails
 */
export function assertContains<T>(array: T[], item: T, message?: string): void {
    if (!array.includes(item)) {
        throw new Error(message || `Expected array to contain ${item}`);
    }
}

/**
 * Assert that an object has a property
 * 
 * @param obj - Object to check
 * @param prop - Property name
 * @param message - Error message if assertion fails
 * @throws Error if assertion fails
 */
export function assertHasProperty<T extends object>(obj: T, prop: keyof T, message?: string): void {
    if (!(prop in obj)) {
        throw new Error(message || `Expected object to have property ${String(prop)}`);
    }
}

// ============================================================================
// Mock State Helpers
// ============================================================================

/**
 * Get the current mock state
 * 
 * @returns The mock state
 */
export function getTestState() {
    return getMockState();
}

/**
 * Reset mock state
 */
export function resetTestState(): void {
    resetMockState();
}

/**
 * Simulate an event
 * 
 * @param eventName - Event name
 * @param eventData - Event data
 */
export function emitTestEvent(eventName: string, eventData: any): void {
    simulateEvent(eventName, eventData);
}

/**
 * Simulate tray click
 * 
 * @param command - Command to simulate
 */
export function emitTrayClick(command: string): void {
    simulateTrayClick(command);
}

/**
 * Simulate tray double click
 */
export function emitTrayDoubleClick(): void {
    simulateTrayDoubleClick();
}

// ============================================================================
// Test Data Generators
// ============================================================================

/**
 * Generate a random string
 * 
 * @param length - Length of string to generate
 * @returns Random string
 */
export function randomString(length: number = 10): string {
    const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    let result = '';
    for (let i = 0; i < length; i++) {
        result += chars.charAt(Math.floor(Math.random() * chars.length));
    }
    return result;
}

/**
 * Generate a random number
 * 
 * @param min - Minimum value
 * @param max - Maximum value
 * @returns Random number between min and max
 */
export function randomNumber(min: number = 0, max: number = 100): number {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

/**
 * Generate a random boolean
 * 
 * @returns Random boolean value
 */
export function randomBoolean(): boolean {
    return Math.random() < 0.5;
}

/**
 * Generate a random file path
 * 
 * @returns Random file path
 */
export function randomFilePath(): string {
    const dirs = ['C:\\temp', 'D:\\data', 'E:\\backup'];
    const dir = dirs[Math.floor(Math.random() * dirs.length)];
    const filename = randomString(8) + '.txt';
    return `${dir}\\${filename}`;
}

/**
 * Generate a mock file system
 * 
 * @param files - Files to include in the mock filesystem
 * @returns Mock filesystem object
 */
export function createMockFileSystem(files: Record<string, string> = {}): Map<string, string> {
    const fs = new Map<string, string>();
    Object.entries(files).forEach(([path, content]) => {
        fs.set(path, content);
    });
    return fs;
}

// ============================================================================
// Re-exports
// ============================================================================

export {
    createMockPuppetNamespace,
    getMockState,
    resetMockState,
    simulateEvent,
    simulateTrayClick,
    simulateTrayDoubleClick
} from './mock';

export type {
    PuppetNamespace,
    WindowController,
    ApplicationController,
    FileSystemController,
    LogController,
    SystemController,
    TrayController,
    EventController,
    DeviceController,
    WindowInfo,
    UserInfo,
    SystemInfo,
    MousePosition,
    DeviceInfo
} from './puppet';