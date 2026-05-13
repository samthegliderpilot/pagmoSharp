plugins {
    kotlin("jvm")
    `maven-publish`
}

kotlin {
    jvmToolchain(26)
}

// Kotlin 2.1.x falls back to JVM_23 when given JDK 26; align Java to match.
tasks.withType<JavaCompile>().configureEach {
    options.release.set(23)
}

dependencies {
    api(project(":core"))
    testImplementation(platform("org.junit:junit-bom:5.11.3"))
    testImplementation("org.junit.jupiter:junit-jupiter")
    testImplementation(kotlin("test"))
}

tasks.test {
    useJUnitPlatform()
    systemProperty("java.library.path", System.getenv("PAGMO4J_NATIVE_DIR") ?: ".")
}

publishing {
    publications {
        create<MavenPublication>("maven") {
            artifactId = "pagmo4j-kotlin"
            from(components["java"])
            pom {
                name.set("pagmo4j-kotlin")
                description.set("Kotlin extension functions for pagmo4j")
                url.set("https://github.com/samthegliderpilot/pagmo4j")
            }
        }
    }
}
