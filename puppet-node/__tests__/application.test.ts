/**
 * Puppet Framework Application Controller Tests
 * 
 * Tests for the ApplicationController API
 */

import {
    setupTestEnvironment,
    cleanupTestEnvironment,
    assertEqual,
    assertTruthy
} from '../test-utils';

describe('ApplicationController', () => {
    let puppet: any;

    beforeEach(() => {
        puppet = setupTestEnvironment({ enableLogging: false, resetState: true });
    });

    afterEach(() => {
        cleanupTestEnvironment();
    });

    describe('close', () => {
        it('should close application', async () => {
            await puppet.application.close();
            // In mock environment, close doesn't do anything
            // Just verify it doesn't throw
            assertTruthy(true, 'close should not throw');
        });
    });

    describe('restart', () => {
        it('should restart application', async () => {
            await puppet.application.restart();
            // In mock environment, restart doesn't do anything
            // Just verify it doesn't throw
            assertTruthy(true, 'restart should not throw');
        });
    });

    describe('getWindowInfo', () => {
        it('should return window information', async () => {
            const info = await puppet.application.getWindowInfo();
            
            assertTruthy(info, 'Window info should not be null');
            assertEqual(typeof info.handle, 'number', 'Handle should be a number');
            assertEqual(typeof info.title, 'string', 'Title should be a string');
            assertEqual(typeof info.width, 'number', 'Width should be a number');
            assertEqual(typeof info.height, 'number', 'Height should be a number');
        });
    });

    describe('execute', () => {
        it('should execute command', async () => {
            await puppet.application.execute('calc.exe');
            // In mock environment, execute doesn't do anything
            // Just verify it doesn't throw
            assertTruthy(true, 'execute should not throw');
        });
    });

    describe('setConfig', () => {
        it('should set configuration value', async () => {
            await puppet.application.setConfig('test-key', 'test-value');
            
            const value = await puppet.application.getConfig('test-key');
            assertEqual(value, 'test-value', 'Config value should match');
        });

        it('should update existing configuration', async () => {
            await puppet.application.setConfig('theme', 'dark');
            
            const value = await puppet.application.getConfig('theme');
            assertEqual(value, 'dark', 'Config should be updated');
        });
    });

    describe('getConfig', () => {
        it('should get configuration value', async () => {
            await puppet.application.setConfig('test-key', 'test-value');
            
            const value = await puppet.application.getConfig('test-key');
            assertEqual(value, 'test-value', 'Should return config value');
        });

        it('should return null for non-existent key', async () => {
            const value = await puppet.application.getConfig('non-existent-key');
            assertEqual(value, null, 'Should return null for non-existent key');
        });
    });

    describe('getAssemblyDirectory', () => {
        it('should return assembly directory', async () => {
            const dir = await puppet.application.getAssemblyDirectory();
            
            assertTruthy(dir, 'Assembly directory should not be empty');
            assertEqual(typeof dir, 'string', 'Assembly directory should be a string');
        });
    });

    describe('getAppDataDirectory', () => {
        it('should return app data directory', async () => {
            const dir = await puppet.application.getAppDataDirectory();
            
            assertTruthy(dir, 'App data directory should not be empty');
            assertEqual(typeof dir, 'string', 'App data directory should be a string');
        });
    });

    describe('getCurrentUser', () => {
        it('should return current user information', async () => {
            const user = await puppet.application.getCurrentUser();
            
            assertTruthy(user, 'User should not be null');
            assertEqual(typeof user.name, 'string', 'User name should be a string');
            assertEqual(typeof user.domain, 'string', 'Domain should be a string');
            assertEqual(typeof user.homeDirectory, 'string', 'Home directory should be a string');
        });
    });

    describe('integration tests', () => {
        it('should manage configuration', async () => {
            // Set multiple config values
            await puppet.application.setConfig('theme', 'dark');
            await puppet.application.setConfig('language', 'en-US');
            await puppet.application.setConfig('fontSize', 16);
            
            // Verify all values
            const theme = await puppet.application.getConfig('theme');
            const language = await puppet.application.getConfig('language');
            const fontSize = await puppet.application.getConfig('fontSize');
            
            assertEqual(theme, 'dark', 'Theme should be dark');
            assertEqual(language, 'en-US', 'Language should be en-US');
            assertEqual(fontSize, 16, 'Font size should be 16');
        });
    });
});

// ============================================================================
// Manual Test Function
// ============================================================================

/**
 * Run application controller tests manually
 */
export async function runApplicationTests(): Promise<void> {
    console.log('=== Running Application Controller Tests ===\n');
    
    let passed = 0;
    let failed = 0;
    
    // Test 1: setConfig and getConfig
    try {
        const puppet = setupTestEnvironment({ enableLogging: false });
        await puppet.application.setConfig('test-key', 'test-value');
        const value = await puppet.application.getConfig('test-key');
        if (value === 'test-value') {
            console.log('✓ Test 1 passed: setConfig/getConfig');
            passed++;
        } else {
            console.log('✗ Test 1 failed: setConfig/getConfig');
            failed++;
        }
        cleanupTestEnvironment();
    } catch (error) {
        console.log('✗ Test 1 failed:', error);
        failed++;
    }
    
    // Test 2: getWindowInfo
    try {
        const puppet = setupTestEnvironment({ enableLogging: false });
        const info = await puppet.application.getWindowInfo();
        if (info && typeof info.handle === 'number') {
            console.log('✓ Test 2 passed: getWindowInfo');
            passed++;
        } else {
            console.log('✗ Test 2 failed: getWindowInfo');
            failed++;
        }
        cleanupTestEnvironment();
    } catch (error) {
        console.log('✗ Test 2 failed:', error);
        failed++;
    }
    
    // Test 3: getCurrentUser
    try {
        const puppet = setupTestEnvironment({ enableLogging: false });
        const user = await puppet.application.getCurrentUser();
        if (user && typeof user.name === 'string') {
            console.log('✓ Test 3 passed: getCurrentUser');
            passed++;
        } else {
            console.log('✗ Test 3 failed: getCurrentUser');
            failed++;
        }
        cleanupTestEnvironment();
    } catch (error) {
        console.log('✗ Test 3 failed:', error);
        failed++;
    }
    
    console.log(`\n=== Test Results ===`);
    console.log(`Passed: ${passed}`);
    console.log(`Failed: ${failed}`);
    console.log(`Total: ${passed + failed}`);
}

// Export for use in Node.js or browser
if (typeof module !== 'undefined' && module.exports) {
    module.exports = { runApplicationTests };
}
