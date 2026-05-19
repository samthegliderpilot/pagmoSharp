package io.github.samthegliderpilot.pagmo4j.interop;

import io.github.samthegliderpilot.pagmo4j.SizeTInterop;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/** Direct unit tests for SizeTInterop conversion utilities. */
class SizeTInteropUnitTest {

    // ── toNativeUInt32 ────────────────────────────────────────────────────────

    @Test
    void toNativeUInt32AcceptsZero() {
        assertEquals(0L, SizeTInterop.toNativeUInt32(0L, "x"));
    }

    @Test
    void toNativeUInt32AcceptsMaxUint32() {
        assertEquals(0xFFFFFFFFL, SizeTInterop.toNativeUInt32(0xFFFFFFFFL, "x"));
    }

    @Test
    void toNativeUInt32AcceptsMidRange() {
        assertEquals(1024L, SizeTInterop.toNativeUInt32(1024L, "x"));
    }

    @Test
    void toNativeUInt32RejectsNegative() {
        IllegalArgumentException ex = assertThrows(IllegalArgumentException.class,
            () -> SizeTInterop.toNativeUInt32(-1L, "popSize"));
        assertTrue(ex.getMessage().contains("popSize"));
    }

    @Test
    void toNativeUInt32RejectsUint32Overflow() {
        // 0xFFFFFFFFL + 1 = 4294967296 — exceeds unsigned 32-bit max
        IllegalArgumentException ex = assertThrows(IllegalArgumentException.class,
            () -> SizeTInterop.toNativeUInt32(0xFFFFFFFFL + 1L, "popSize"));
        assertTrue(ex.getMessage().contains("popSize"));
    }

    @Test
    void toNativeUInt32RejectsLargePositive() {
        assertThrows(IllegalArgumentException.class,
            () -> SizeTInterop.toNativeUInt32(Long.MAX_VALUE, "x"));
    }

    @Test
    void toNativeUInt32IncludesParamNameInMessage() {
        IllegalArgumentException ex = assertThrows(IllegalArgumentException.class,
            () -> SizeTInterop.toNativeUInt32(-5L, "myParam"));
        assertTrue(ex.getMessage().contains("myParam"),
            "Message should identify the parameter: " + ex.getMessage());
    }

    // ── toNativeSizeT ─────────────────────────────────────────────────────────

    @Test
    void toNativeSizeTAcceptsZero() {
        assertEquals(0L, SizeTInterop.toNativeSizeT(0L, "x"));
    }

    @Test
    void toNativeSizeTAcceptsLargePositive() {
        assertEquals(Long.MAX_VALUE, SizeTInterop.toNativeSizeT(Long.MAX_VALUE, "x"));
    }

    @Test
    void toNativeSizeTRejectsNegative() {
        IllegalArgumentException ex = assertThrows(IllegalArgumentException.class,
            () -> SizeTInterop.toNativeSizeT(-1L, "idx"));
        assertTrue(ex.getMessage().contains("idx"));
    }
}
