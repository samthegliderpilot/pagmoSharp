package io.github.samthegliderpilot.pagmo4j;

import java.io.*;
import java.nio.file.*;

/**
 * Loads the pagmo4j JNI native library.
 *
 * <p>Resolution order:
 * <ol>
 *   <li>Environment variable {@code PAGMO4J_NATIVE_DIR} — directory containing
 *       the platform native library.
 *   <li>System property {@code java.library.path} — standard JVM lookup.
 *   <li>Bundled resource in the JAR under {@code /natives/<platform>/} — extracted
 *       to a temp directory on first use.
 * </ol>
 *
 * <p>Call {@link #load()} once before using any pagmo4j class. The {@link pagmo4j}
 * module class calls this automatically in its static initializer.
 */
public final class NativeLoader {

    private static final String LIB_NAME = "pagmo4j";
    private static volatile boolean loaded = false;

    private NativeLoader() {}

    public static synchronized void load() {
        if (loaded) return;

        String nativeDir = System.getenv("PAGMO4J_NATIVE_DIR");
        if (nativeDir != null && !nativeDir.isBlank()) {
            System.load(resolveLibPath(nativeDir));
            loaded = true;
            return;
        }

        try {
            System.loadLibrary(LIB_NAME);
            loaded = true;
            return;
        } catch (UnsatisfiedLinkError ignored) {}

        extractAndLoad();
        loaded = true;
    }

    private static String resolveLibPath(String dir) {
        String os = System.getProperty("os.name", "").toLowerCase();
        String name;
        if (os.contains("win")) {
            name = LIB_NAME + ".dll";
        } else if (os.contains("mac")) {
            name = "lib" + LIB_NAME + ".dylib";
        } else {
            name = "lib" + LIB_NAME + ".so";
        }
        return Paths.get(dir, name).toAbsolutePath().toString();
    }

    private static void extractAndLoad() {
        String resourcePath = "/natives/" + platformRid() + "/" + nativeFileName();
        try (InputStream in = NativeLoader.class.getResourceAsStream(resourcePath)) {
            if (in == null) {
                throw new UnsatisfiedLinkError(
                    "pagmo4j native library not found. Set PAGMO4J_NATIVE_DIR or ensure the " +
                    "native library is on java.library.path. Resource tried: " + resourcePath);
            }
            Path tmp = Files.createTempDirectory("pagmo4j-native");
            tmp.toFile().deleteOnExit();
            Path dest = tmp.resolve(nativeFileName());
            Files.copy(in, dest, StandardCopyOption.REPLACE_EXISTING);
            dest.toFile().deleteOnExit();
            System.load(dest.toAbsolutePath().toString());
        } catch (IOException e) {
            throw new UnsatisfiedLinkError("Failed to extract pagmo4j native library: " + e.getMessage());
        }
    }

    private static String platformRid() {
        String os = System.getProperty("os.name", "").toLowerCase();
        String arch = System.getProperty("os.arch", "").toLowerCase();
        if (os.contains("win"))   return "win-x64";
        if (os.contains("mac"))   return arch.contains("aarch64") ? "osx-arm64" : "osx-x64";
        return "linux-x64";
    }

    private static String nativeFileName() {
        String os = System.getProperty("os.name", "").toLowerCase();
        if (os.contains("win")) return LIB_NAME + ".dll";
        if (os.contains("mac")) return "lib" + LIB_NAME + ".dylib";
        return "lib" + LIB_NAME + ".so";
    }
}
