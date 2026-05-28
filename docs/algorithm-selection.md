# Algorithm Selection Guide

This table maps each wrapped algorithm to its problem category. If you know what pagmo2 algorithm you want by name, find it here to get the pagmo.NET class name and understand when to use it.

## Single-objective, continuous, unconstrained

These algorithms minimize a scalar objective over a continuous decision space with box bounds only.

| pagmo2 name | pagmo.NET class | Notes |
|-------------|-----------------|-------|
| Differential Evolution | `de` | Robust all-rounder; good first choice for smooth and noisy functions |
| Self-Adaptive DE (jDE/iDE) | `sade` | DE variant that adapts its own parameters; less tuning required |
| DE/1220 | `de1220` | Extended DE with more mutation strategies |
| Particle Swarm Optimization | `pso` | Swarm-based; good for multimodal landscapes |
| PSO Generational | `pso_gen` | Generational (synchronous) variant of PSO |
| Grey Wolf Optimizer | `gwo` | Population-based; no hyperparameters to tune |
| Bee Colony | `bee_colony` | Artificial bee colony metaheuristic |
| Simple Evolutionary Algorithm | `sea` | (1+1)-ES style; lightweight baseline |
| Simple Genetic Algorithm | `sga` | Classic GA with configurable crossover/mutation |
| Improved Harmony Search | `ihs` | Music-inspired; low population size works |
| Simulated Annealing | `simulated_annealing` | Single-agent; good for escaping local optima |
| Compass Search | `compass_search` | Deterministic local search; gradient-free |
| Extended NES | `xnes` | Natural Evolution Strategies; covariance-adaptive |
| CMA-ES | `cmaes` | Covariance Matrix Adaptation ES; state-of-the-art for smooth functions |
| NLopt (various) | `nlopt` | Wraps NLopt library: local and global gradient-free and gradient-based methods |
| Monotone Basin Hopping | `mbh` | Meta-algorithm; wraps any inner solver with random restarts |
| GACO | `gaco` | Ant Colony Optimization generalized to continuous spaces |

## Multi-objective, continuous

These algorithms return a Pareto-front approximation.

| pagmo2 name | pagmo.NET class | Notes |
|-------------|-----------------|-------|
| NSGA-II | `nsga2` | Classic non-dominated sorting GA; general purpose |
| MOEA/D | `moead` | Decomposition-based; fast on well-structured fronts |
| MOEA/D-GEN | `moead_gen` | Generational variant of MOEA/D |
| Non-dominated PSO | `nspso` | PSO extended to multi-objective |
| Multi-objective ACO | `maco` | Ant Colony Optimization for multi-objective |

## Constrained (equality and/or inequality constraints)

Wrap any supported algorithm using `cstrs_self_adaptive` to handle `get_nec()` / `get_nic()` constraints.

| pagmo2 name | pagmo.NET class | Notes |
|-------------|-----------------|-------|
| Self-Adaptive Constraints | `cstrs_self_adaptive` | Meta-algorithm; wraps any inner solver; implements Coello-Mezura approach |
| NLopt | `nlopt` | Several NLopt methods natively support equality/inequality constraints |

## Mixed-integer (MINLP)

Problems where some decision variables are integers (`get_nix() > 0`).

| pagmo2 name | pagmo.NET class | Notes |
|-------------|-----------------|-------|
| Simple Genetic Algorithm | `sga` | Handles integer variables directly via rounding |
| GACO | `gaco` | Supports MINLP natively |
| DE/1220 | `de1220` | Handles integer variables |

## Gradient-based (requires `has_gradient() = true`)

| pagmo2 name | pagmo.NET class | Notes |
|-------------|-----------------|-------|
| NLopt | `nlopt` | LBFGS, MMA, SLSQP, and many others; pass the NLopt solver name to the constructor |
| IPOPT | via `pagmoNet.ipopt` add-on | Interior-point; for large-scale constrained problems; requires separate package |

## Meta-algorithms (wrap another algorithm)

| pagmo2 name | pagmo.NET class | Notes |
|-------------|-----------------|-------|
| Monotone Basin Hopping | `mbh` | Random restart wrapper; use around any local solver |
| Self-Adaptive Constraints | `cstrs_self_adaptive` | Constraint handling wrapper |
| Decompose | `decompose` | Scalarizes multi-objective into single-objective for island-based runs |
| Unconstrain | `unconstrain` | Converts constrained to unconstrained via penalty; use as last resort |

## Decision guide

```
Is the problem single-objective?
├── Yes, continuous, smooth → CMA-ES or NLopt (gradient-based if gradient available)
├── Yes, continuous, noisy/multimodal → DE (sade) or PSO
├── Yes, has integer variables → SGA or GACO
└── No (multi-objective) → NSGA-II (general) or MOEA/D (structured fronts)

Has constraints?
├── Equality + Inequality → cstrs_self_adaptive wrapping DE/PSO, or NLopt/IPOPT
└── Box bounds only → any single-objective algorithm above
```
