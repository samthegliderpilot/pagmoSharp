#include <cstddef>
#include <exception>
#include <memory>
#include <string>
#include <utility>

#include <pagmo/problem.hpp>
#include <pagmo/algorithm.hpp>
#include <pagmo/population.hpp>
#include <pagmo/batch_evaluators/default_bfe.hpp>
#include <pagmo/batch_evaluators/thread_bfe.hpp>
#include <pagmo/batch_evaluators/member_bfe.hpp>
#include <pagmo/types.hpp>
#include <pagmo/utils/gradients_and_hessians.hpp>

#include "problem.h"
#include "algorithm_callback.h"

#ifndef PAGMONET_EXPORT
#define PAGMONET_EXPORT __declspec(dllexport)
#endif

extern "C" {
namespace {
thread_local std::string g_pagmonet_last_error;

void clear_last_error() noexcept
{
    g_pagmonet_last_error.clear();
}

void set_last_error(std::string message) noexcept
{
    g_pagmonet_last_error = std::move(message);
}

void set_last_error_from_exception(const char *context, const std::exception &ex) noexcept
{
    set_last_error(std::string(context) + ": " + ex.what());
}

void set_unknown_last_error(const char *context) noexcept
{
    set_last_error(std::string(context) + ": Unknown C++ exception");
}
} // namespace

PAGMONET_EXPORT const char *pagmonet_get_last_error()
{
    if (g_pagmonet_last_error.empty()) {
        return nullptr;
    }
    return g_pagmonet_last_error.c_str();
}

PAGMONET_EXPORT void pagmonet_clear_last_error()
{
    clear_last_error();
}

PAGMONET_EXPORT void *pagmonet_problem_from_callback(void *callback_ptr)
{
    clear_last_error();
    if (callback_ptr == nullptr) {
        set_last_error("pagmonet_problem_from_callback: callback_ptr is null");
        return nullptr;
    }

    try {
        auto *raw = static_cast<pagmoWrap::problem_callback *>(callback_ptr);
        std::shared_ptr<pagmoWrap::problem_callback> callback_owner(raw);
        auto *problem = new pagmo::problem(pagmoWrap::managed_problem(std::move(callback_owner)));
        return static_cast<void *>(problem);
    } catch (const std::exception &ex) {
        set_last_error_from_exception("pagmonet_problem_from_callback", ex);
        return nullptr;
    } catch (...) {
        set_unknown_last_error("pagmonet_problem_from_callback");
        return nullptr;
    }
}

PAGMONET_EXPORT void *pagmonet_algorithm_from_callback(void *callback_ptr)
{
    clear_last_error();
    if (callback_ptr == nullptr) {
        set_last_error("pagmonet_algorithm_from_callback: callback_ptr is null");
        return nullptr;
    }

    try {
        auto *raw = static_cast<pagmoWrap::algorithm_callback *>(callback_ptr);
        std::shared_ptr<pagmoWrap::algorithm_callback> callback_owner(raw);
        auto *algorithm = new pagmo::algorithm(pagmoWrap::managed_algorithm(std::move(callback_owner)));
        return static_cast<void *>(algorithm);
    } catch (const std::exception &ex) {
        set_last_error_from_exception("pagmonet_algorithm_from_callback", ex);
        return nullptr;
    } catch (...) {
        set_unknown_last_error("pagmonet_algorithm_from_callback");
        return nullptr;
    }
}

PAGMONET_EXPORT void pagmonet_problem_delete(void *problem_ptr)
{
    if (problem_ptr == nullptr) {
        return;
    }

    auto *problem = static_cast<pagmo::problem *>(problem_ptr);
    delete problem;
}

PAGMONET_EXPORT void *pagmonet_population_new(void *problem_ptr, std::size_t pop_size, unsigned seed)
{
    clear_last_error();
    if (problem_ptr == nullptr) {
        set_last_error("pagmonet_population_new: problem_ptr is null");
        return nullptr;
    }

    try {
        auto *problem = static_cast<pagmo::problem *>(problem_ptr);
        auto *population = new pagmo::population(*problem, pop_size, seed);
        return static_cast<void *>(population);
    } catch (const std::exception &ex) {
        set_last_error_from_exception("pagmonet_population_new", ex);
        return nullptr;
    } catch (...) {
        set_unknown_last_error("pagmonet_population_new");
        return nullptr;
    }
}

PAGMONET_EXPORT void *pagmonet_default_bfe_operator(void *bfe_ptr, void *problem_ptr, void *batch_x_ptr)
{
    clear_last_error();
    if (bfe_ptr == nullptr || problem_ptr == nullptr || batch_x_ptr == nullptr) {
        set_last_error("pagmonet_default_bfe_operator: bfe_ptr/problem_ptr/batch_x_ptr must be non-null");
        return nullptr;
    }

    try {
        auto *bfe = static_cast<pagmo::default_bfe *>(bfe_ptr);
        auto *problem = static_cast<pagmo::problem *>(problem_ptr);
        auto *batch_x = static_cast<pagmo::vector_double *>(batch_x_ptr);

        auto result = (*bfe)(*problem, *batch_x);
        return static_cast<void *>(new pagmo::vector_double(std::move(result)));
    } catch (const std::exception &ex) {
        set_last_error_from_exception("pagmonet_default_bfe_operator", ex);
        return nullptr;
    } catch (...) {
        set_unknown_last_error("pagmonet_default_bfe_operator");
        return nullptr;
    }
}

PAGMONET_EXPORT void *pagmonet_thread_bfe_operator(void *bfe_ptr, void *problem_ptr, void *batch_x_ptr)
{
    clear_last_error();
    if (bfe_ptr == nullptr || problem_ptr == nullptr || batch_x_ptr == nullptr) {
        set_last_error("pagmonet_thread_bfe_operator: bfe_ptr/problem_ptr/batch_x_ptr must be non-null");
        return nullptr;
    }

    try {
        auto *bfe = static_cast<pagmo::thread_bfe *>(bfe_ptr);
        auto *problem = static_cast<pagmo::problem *>(problem_ptr);
        auto *batch_x = static_cast<pagmo::vector_double *>(batch_x_ptr);

        auto result = (*bfe)(*problem, *batch_x);
        return static_cast<void *>(new pagmo::vector_double(std::move(result)));
    } catch (const std::exception &ex) {
        set_last_error_from_exception("pagmonet_thread_bfe_operator", ex);
        return nullptr;
    } catch (...) {
        set_unknown_last_error("pagmonet_thread_bfe_operator");
        return nullptr;
    }
}

PAGMONET_EXPORT void *pagmonet_member_bfe_operator(void *bfe_ptr, void *problem_ptr, void *batch_x_ptr)
{
    clear_last_error();
    if (bfe_ptr == nullptr || problem_ptr == nullptr || batch_x_ptr == nullptr) {
        set_last_error("pagmonet_member_bfe_operator: bfe_ptr/problem_ptr/batch_x_ptr must be non-null");
        return nullptr;
    }

    try {
        auto *bfe = static_cast<pagmo::member_bfe *>(bfe_ptr);
        auto *problem = static_cast<pagmo::problem *>(problem_ptr);
        auto *batch_x = static_cast<pagmo::vector_double *>(batch_x_ptr);

        auto result = (*bfe)(*problem, *batch_x);
        return static_cast<void *>(new pagmo::vector_double(std::move(result)));
    } catch (const std::exception &ex) {
        set_last_error_from_exception("pagmonet_member_bfe_operator", ex);
        return nullptr;
    } catch (...) {
        set_unknown_last_error("pagmonet_member_bfe_operator");
        return nullptr;
    }
}

PAGMONET_EXPORT void *pagmonet_estimate_gradient_problem(void *problem_ptr, void *x_ptr, double dx)
{
    clear_last_error();
    if (problem_ptr == nullptr || x_ptr == nullptr) {
        set_last_error("pagmonet_estimate_gradient_problem: problem_ptr/x_ptr must be non-null");
        return nullptr;
    }

    try {
        auto *problem = static_cast<pagmo::problem *>(problem_ptr);
        auto *x = static_cast<pagmo::vector_double *>(x_ptr);
        auto result = pagmo::estimate_gradient([problem](const pagmo::vector_double &dv) { return problem->fitness(dv); }, *x, dx);
        return static_cast<void *>(new pagmo::vector_double(std::move(result)));
    } catch (const std::exception &ex) {
        set_last_error_from_exception("pagmonet_estimate_gradient_problem", ex);
        return nullptr;
    } catch (...) {
        set_unknown_last_error("pagmonet_estimate_gradient_problem");
        return nullptr;
    }
}

PAGMONET_EXPORT void *pagmonet_estimate_gradient_h_problem(void *problem_ptr, void *x_ptr, double dx)
{
    clear_last_error();
    if (problem_ptr == nullptr || x_ptr == nullptr) {
        set_last_error("pagmonet_estimate_gradient_h_problem: problem_ptr/x_ptr must be non-null");
        return nullptr;
    }

    try {
        auto *problem = static_cast<pagmo::problem *>(problem_ptr);
        auto *x = static_cast<pagmo::vector_double *>(x_ptr);
        auto result
            = pagmo::estimate_gradient_h([problem](const pagmo::vector_double &dv) { return problem->fitness(dv); }, *x, dx);
        return static_cast<void *>(new pagmo::vector_double(std::move(result)));
    } catch (const std::exception &ex) {
        set_last_error_from_exception("pagmonet_estimate_gradient_h_problem", ex);
        return nullptr;
    } catch (...) {
        set_unknown_last_error("pagmonet_estimate_gradient_h_problem");
        return nullptr;
    }
}

PAGMONET_EXPORT void *pagmonet_estimate_sparsity_problem(void *problem_ptr, void *x_ptr, double dx)
{
    clear_last_error();
    if (problem_ptr == nullptr || x_ptr == nullptr) {
        set_last_error("pagmonet_estimate_sparsity_problem: problem_ptr/x_ptr must be non-null");
        return nullptr;
    }

    try {
        auto *problem = static_cast<pagmo::problem *>(problem_ptr);
        auto *x = static_cast<pagmo::vector_double *>(x_ptr);
        auto result = pagmo::estimate_sparsity([problem](const pagmo::vector_double &dv) { return problem->fitness(dv); }, *x, dx);
        return static_cast<void *>(new pagmo::sparsity_pattern(std::move(result)));
    } catch (const std::exception &ex) {
        set_last_error_from_exception("pagmonet_estimate_sparsity_problem", ex);
        return nullptr;
    } catch (...) {
        set_unknown_last_error("pagmonet_estimate_sparsity_problem");
        return nullptr;
    }
}

} // extern "C"
