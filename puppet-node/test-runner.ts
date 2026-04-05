/**
 * Puppet Framework Test Runner
 * 
 * Simple test runner for running tests without Jest
 */

import { runWindowTests } from './__tests__/window.test';
import { runApplicationTests } from './__tests__/application.test';

/**
 * Run all tests
 */
export async function runAllTests(): Promise<void> {
    console.log('╔═══════════════════════════════════════════════════════════╗');
    console.log('║   Puppet Framework Test Suite                            ║');
    console.log('╚═══════════════════════════════════════════════════════════╝\n');
    
    // Run window tests
    console.log('━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━');
    await runWindowTests();
    console.log('');
    
    // Run application tests
    console.log('━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━');
    await runApplicationTests();
    console.log('');
    
    console.log('╔═══════════════════════════════════════════════════════════╗');
    console.log('║   All tests completed                                       ║');
    console.log('╚═══════════════════════════════════════════════════════════╝');
}

// Run tests if this file is executed directly
if (typeof require !== 'undefined' && require.main === module) {
    runAllTests().catch(error => {
        console.error('Test runner error:', error);
        process.exit(1);
    });
}

// Export for use in other environments
if (typeof window !== 'undefined') {
    (window as any).runPuppetTests = runAllTests;
}