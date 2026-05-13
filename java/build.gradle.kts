plugins {
    java
    kotlin("jvm") version "2.1.20" apply false
}

subprojects {
    group = "io.github.samthegliderpilot"
    version = "1.0.0"

    repositories {
        mavenCentral()
    }
}
