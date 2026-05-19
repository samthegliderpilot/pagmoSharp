package io.github.samthegliderpilot.pagmo4j;

import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/**
 * Tests for NativeLoader — same package to access package-private class.
 * Relies on PAGMO4J_NATIVE_DIR being set (same requirement as all JNI tests).
 */
class NativeLoaderTest {

    @Test
    void loadIsIdempotent() {
        // Calling load() multiple times must not throw or double-load.
        NativeLoader.load();
        NativeLoader.load();
        NativeLoader.load();
        // If we reach here without UnsatisfiedLinkError or double-load crash, test passes.
    }

    @Test
    void loadedFlagIsSetAfterFirstCall() {
        NativeLoader.load();
        // Verify the library is actually loaded by performing a trivial JNI call.
        // pagmo4jJNI.PAGMO_VERSION_get() invokes native code.
        assertNotNull(pagmo4jJNI.PAGMO_VERSION_get(), "native call should succeed after load()");
    }
}
