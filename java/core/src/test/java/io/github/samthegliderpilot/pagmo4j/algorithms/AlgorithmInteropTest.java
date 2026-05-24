package io.github.samthegliderpilot.pagmo4j.algorithms;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.OptionalSolverAvailability;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
import static org.junit.jupiter.api.Assumptions.assumeTrue;

/**
 * Verifies that every concrete algorithm class is routed through its native
 * {@code to_algorithm()} path in {@link AlgorithmInterop#normalizeToTypeErased}.
 */
class AlgorithmInteropTest {

    private static void assertNormalized(algorithm source) {
        assertNotNull(source, "normalizeToTypeErased returned null");
        assertNotNull(source.get_name(), "normalized algorithm should expose a stable name");
    }

    private static void assertNormalized(IAlgorithm source) {
        try (algorithm result = AlgorithmInterop.normalizeToTypeErased(source)) {
            assertNotNull(result, "normalizeToTypeErased returned null for " + source.getClass().getSimpleName());
            assertEquals(source.get_name(), result.get_name(),
                "normalizeToTypeErased should preserve the algorithm identity");
        }
    }

    @Test void nullThrows()            { assertThrows(NullPointerException.class, () -> AlgorithmInterop.normalizeToTypeErased(null)); }
    @Test void alreadyTypeErased()     { try (de d = new de(1L); algorithm a = d.to_algorithm()) { assertNormalized(a); } }
    @Test void beeColony()             { try (bee_colony a = new bee_colony(2L))            { assertNormalized(a); } }
    @Test void cmaes()                 { try (cmaes a = new cmaes(2L))                      { assertNormalized(a); } }
    @Test void compassSearch()         { try (compass_search a = new compass_search(10L))   { assertNormalized(a); } }
    @Test void ihs()                   { try (ihs a = new ihs(5L))                          { assertNormalized(a); } }
    @Test void nsga2()                 { try (nsga2 a = new nsga2(2L))                      { assertNormalized(a); } }
    @Test void moead()                 { try (moead a = new moead(2L))                      { assertNormalized(a); } }
    @Test void moeadGen()              { try (moead_gen a = new moead_gen(2L))              { assertNormalized(a); } }
    @Test void maco()                  { try (maco a = new maco(2L))                        { assertNormalized(a); } }
    @Test
    void mbh() {
        try (de d = new de(1L); algorithm inner = d.to_algorithm(); mbh a = new mbh(inner, 2L, 0.1)) {
            assertNormalized(a);
        }
    }
    @Test
    void cstrsSelfAdaptive() {
        try (de d = new de(1L); algorithm inner = d.to_algorithm(); cstrs_self_adaptive a = new cstrs_self_adaptive(2L, inner)) {
            assertNormalized(a);
        }
    }
    @Test void de()                    { try (de a = new de(2L))                            { assertNormalized(a); } }
    @Test void de1220()                { try (de1220 a = new de1220(2L))                   { assertNormalized(a); } }
    @Test void gaco()                  { try (gaco a = new gaco(2L))                       { assertNormalized(a); } }
    @Test void gwo()                   { try (gwo a = new gwo(2L))                         { assertNormalized(a); } }
    @Test void nspso()                 { try (nspso a = new nspso(2L))                     { assertNormalized(a); } }
    @Test void nullAlgorithm()         { try (null_algorithm a = new null_algorithm())      { assertNormalized(a); } }
    @Test void pso()                   { try (pso a = new pso(2L))                         { assertNormalized(a); } }
    @Test void psoGen()                { try (pso_gen a = new pso_gen(2L))                 { assertNormalized(a); } }
    @Test void sade()                  { try (sade a = new sade(2L))                       { assertNormalized(a); } }
    @Test void sea()                   { try (sea a = new sea(2L))                         { assertNormalized(a); } }
    @Test void sga()                   { try (sga a = new sga(2L))                         { assertNormalized(a); } }
    @Test void simulatedAnnealing()    { try (simulated_annealing a = new simulated_annealing()) { assertNormalized(a); } }
    @Test void xnes()                  { try (xnes a = new xnes(2L))                       { assertNormalized(a); } }

    @Test
    void nlopt() {
        assumeTrue(OptionalSolverAvailability.isNloptAvailable(), "nlopt not available");
        try (nlopt a = new nlopt("lbfgs")) { assertNormalized(a); }
    }

    @Test
    void ipopt() {
        assumeTrue(OptionalSolverAvailability.isIpoptAvailable(), "ipopt not available");
        try (ipopt a = new ipopt()) { assertNormalized(a); }
    }

    @Test
    void customManagedAlgorithmUsesDirectorPath() {
        IAlgorithm custom = new IAlgorithm() {
            @Override public population evolve(population pop) { return pop; }
            @Override public String get_name() { return "CustomAlgo"; }
        };
        try (algorithm result = AlgorithmInterop.normalizeToTypeErased(custom)) {
            assertNotNull(result, "custom managed algorithm must produce a valid algorithm wrapper");
            assertEquals("CustomAlgo", result.get_name());
        }
    }
}
