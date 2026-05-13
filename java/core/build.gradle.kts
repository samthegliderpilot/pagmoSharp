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
}

tasks.test {
    useJUnitPlatform()
    // Native library must be on java.library.path for tests.
    // Set via -Djava.library.path=<path> or PAGMO4J_NATIVE_DIR env var.
    systemProperty("java.library.path", System.getenv("PAGMO4J_NATIVE_DIR") ?: ".")
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
