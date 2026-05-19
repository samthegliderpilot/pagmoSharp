plugins {
    java
    kotlin("jvm")
    application
}

kotlin {
    jvmToolchain(26)
}

// Kotlin 2.1.x falls back to JVM_23 when given JDK 26; align Java to match.
tasks.withType<JavaCompile>().configureEach {
    options.release.set(23)
}

dependencies {
    implementation(project(":core"))
    implementation(project(":kotlin-ext"))
}

application {
    mainClass.set("io.github.samthegliderpilot.pagmo4j.examples.Main")
}

tasks.run.configure {
    systemProperty("java.library.path", System.getenv("PAGMO4J_NATIVE_DIR") ?: ".")
}
