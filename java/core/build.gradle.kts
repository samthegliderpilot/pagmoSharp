import java.time.Duration

plugins {
    java
    `maven-publish`
}

java {
    sourceCompatibility = JavaVersion.VERSION_17
    targetCompatibility = JavaVersion.VERSION_17
    withSourcesJar()
    withJavadocJar()
}

// SWIG-generated sources live in src/generated/java — committed like pagmoSharp's generated .cs files.
sourceSets {
    main {
        java {
            srcDirs("src/main/java", "src/generated/java")
        }
    }
}

dependencies {
    testImplementation(platform("org.junit:junit-bom:5.11.3"))
    testImplementation("org.junit.jupiter:junit-jupiter")
    testRuntimeOnly("org.junit.platform:junit-platform-launcher")
}

tasks.test {
    useJUnitPlatform()
    // Native library must be on java.library.path for tests.
    // Set via -Djava.library.path=<path> or PAGMO4J_NATIVE_DIR env var.
    systemProperty("java.library.path", System.getenv("PAGMO4J_NATIVE_DIR") ?: ".")
    // JNI-heavy tests are more stable when each test class gets a fresh worker process.
    forkEvery = 1
    maxParallelForks = 1
    // Hard per-test timeout so a hung test fails in 15s instead of blocking the suite.
    systemProperty("junit.jupiter.execution.timeout.default", "15s")
    // Process-level timeout kills the JVM if native code hangs (JUnit timeout can't interrupt native threads).
    timeout.set(Duration.ofMinutes(5))
}

publishing {
    publications {
        create<MavenPublication>("maven") {
            artifactId = "pagmo4j"
            from(components["java"])
            pom {
                name.set("pagmo4j")
                description.set("Java/Kotlin bindings for pagmo2 — multi-island metaheuristic optimization")
                url.set("https://github.com/samthegliderpilot/pagmo4j")
                licenses {
                    license {
                        name.set("GPL-3.0-only")
                        url.set("https://www.gnu.org/licenses/gpl-3.0.html")
                    }
                }
            }
        }
    }
}
