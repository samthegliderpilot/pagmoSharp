package io.github.samthegliderpilot.pagmo4j.algorithms;

import java.util.Map;

/** Algorithm-agnostic view of a single log entry from a pagmo algorithm. */
public interface IAlgorithmLogLine {
    String getAlgorithmName();
    Map<String, Object> getRawFields();
    String toDisplayString();
}
