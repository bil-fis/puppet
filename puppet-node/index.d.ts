/**
 * Puppet Framework Type Definitions
 * 
 * This file provides TypeScript type definitions and runtime support for the Puppet Framework.
 * It allows for:
 * - Full TypeScript support with IntelliSense
 * - Development in browser/Node.js environments with mock implementations
 * - Automatic detection of Puppet Framework environment
 * - Type-safe API usage
 * 
 * @author Puppet Framework Team
 * @version 1.0.0
 */

// Import all type definitions
export * from './puppet';
export { PuppetNamespace as default } from './puppet';

// Import types for runtime use
import type { PuppetNamespace } from './puppet';

// ============================================================================
// Global Declarations
// ============================================================================

declare global {
    interface Window {
        puppet?: PuppetNamespace;
        __PUPPET_FRAMEWORK__?: boolean;
    }
}

// ============================================================================
// Environment Detection
// ============================================================================

/**
 * Check if running in Puppet Framework environment
 */
export function isPuppetEnvironment(): boolean {
    return typeof window !== 'undefined' && 
           typeof (window as any).chrome !== 'undefined' && 
           typeof (window as any).chrome.webview !== 'undefined';
}

/**
 * Check if running in browser environment
 */
export function isBrowserEnvironment(): boolean {
    return typeof window !== 'undefined' && typeof window.document !== 'undefined';
}

/**
 * Check if running in Node.js environment
 */
export function isNodeEnvironment(): boolean {
    return typeof process !== 'undefined' && 
           process.versions && 
           process.versions.node;
}

// ============================================================================
// Puppet Access
// ============================================================================

/**
 * Get the puppet namespace
 * 
 * This function returns the puppet namespace based on the current environment:
 * - In Puppet Framework: returns the real puppet namespace injected by C# application
 * - In browser/Node.js: returns a mock implementation for development/testing
 * 
 * @returns The puppet namespace
 * 
 * @example
 * ```typescript
 * import { getPuppet } from '@puppet-framework/types';
 * 
 * const puppet = getPuppet();
 * await puppet.window.setBorderless(true);
 * ```
 */
export function getPuppet(): PuppetNamespace {
    if (typeof window !== 'undefined' && (window as any).puppet) {
        return (window as any).puppet;
    }
    
    // Return a mock namespace for non-Puppet environments
    // This will be provided by the runtime puppet.js file
    throw new Error('Puppet namespace not found. Make sure puppet.js is loaded.');
}

/**
 * Get the puppet namespace (alias for getPuppet)
 * 
 * @returns The puppet namespace
 */
export function getPuppetNamespace(): PuppetNamespace {
    return getPuppet();
}

/**
 * Check if puppet is available
 * 
 * @returns true if puppet is available, false otherwise
 */
export function isPuppetAvailable(): boolean {
    return typeof window !== 'undefined' && 
           typeof (window as any).puppet !== 'undefined';
}

// ============================================================================
// Type Guards
// ============================================================================

/**
 * Type guard for WindowInfo
 */
export function isWindowInfo(obj: any): obj is import('./puppet').WindowInfo {
    return obj && 
           typeof obj.handle === 'number' &&
           typeof obj.title === 'string' &&
           typeof obj.width === 'number' &&
           typeof obj.height === 'number';
}

/**
 * Type guard for SystemInfo
 */
export function isSystemInfo(obj: any): obj is import('./puppet').SystemInfo {
    return obj && 
           typeof obj.osName === 'string' &&
           typeof obj.osVersion === 'string' &&
           typeof obj.cpuCores === 'number' &&
           typeof obj.totalMemory === 'number';
}

/**
 * Type guard for DeviceInfo
 */
export function isDeviceInfo(obj: any): obj is import('./puppet').DeviceInfo {
    return obj && 
           typeof obj.DeviceId === 'string' &&
           typeof obj.DeviceType === 'number' &&
           typeof obj.TotalSize === 'number';
}

// ============================================================================
// Utility Types
// ============================================================================

/**
 * Promise return type for async puppet methods
 */
export type PuppetAsyncMethod = (...args: any[]) => Promise<any>;

/**
 * Event callback type
 */
export type EventCallback = (event: any) => void | Promise<void>;

/**
 * Menu item type
 */
export type MenuItem = {
    Text: string;
    Command: string;
};

// ============================================================================
// Default Export
// ============================================================================

/**
 * Default export for easy import
 * 
 * @example
 * ```typescript
 * import puppet from '@puppet-framework/types';
 * 
 * await puppet.window.setBorderless(true);
 * ```
 */
export default getPuppet;