/**
 * Puppet Framework Type Definitions
 * 
 * This file provides TypeScript type definitions for the Puppet Framework.
 * It allows for:
 * - Full TypeScript support with IntelliSense
 * - Development in browser/Node.js environments with mock implementations
 * - Automatic detection of Puppet Framework environment
 * - Type-safe API usage
 * 
 * @author Puppet Framework Team
 * @version 1.0.0
 */

// Environment detection
declare const __PUPPET_FRAMEWORK__: boolean;
declare const __BROWSER__: boolean;
declare const __NODEJS__: boolean;

// Global puppet namespace
declare global {
    interface Window {
        puppet?: PuppetNamespace;
    }
}

// Re-export puppet types
export * from './puppet';
export { PuppetNamespace as default } from './puppet';

// Import types
import { PuppetNamespace } from './puppet';

// Global puppet declaration
declare const puppet: PuppetNamespace;

// Environment-aware puppet access
declare function getPuppet(): PuppetNamespace;

export default getPuppet;