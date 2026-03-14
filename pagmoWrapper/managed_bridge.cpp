#include <cstddef>
#include <memory>
#include <utility>

#include <pagmo/problem.hpp>
#include <pagmo/population.hpp>
#include <pagmo/batch_evaluators/default_bfe.hpp>
#include <pagmo/batch_evaluators/thread_bfe.hpp>
#include <pagmo/batch_evaluators/member_bfe.hpp>
#include <pagmo/types.hpp>
#include <pagmo/utils/gradients_and_hessians.hpp>

#include "problem.h"

#ifndef PAGMOSHARP_EXPORT
#define PAGMOSHARP_EXPORT __declspec(dllexport)
#endif

extern "C" {

PAGMOSHARP_EXPORT void *pagmosharp_problem_from_callback(void *callback_ptr)
{
    if (callback_ptr == nullptr) {
        return nullptr;
    }

    try {
        auto *raw = static_cast<pagmoWrap::problem_callback *>(callback_ptr);
        std::shared_ptr<pagmoWrap::problem_callback> callback_owner(raw);
        auto *problem = new pagmo::problem(pagmoWrap::managed_problem(std::move(callback_owner)));
        return static_cast<void *>(problem);
    } catch (...) {
        return nullptr;
    }
}

PAGMOSHARP_EXPORT void pagmosharp_problem_delete(void *problem_ptr)
{
    if (problem_ptr == nullptr) {
        return;
    }

    auto *problem = static_cast<pagmo::problem *>(problem_ptr);
    delete problem;
}

PAGMOSHARP_EXPORT void *pagmosharp_population_new(void *problem_ptr, std::size_t pop_size, unsigned seed)
{
    if (problem_ptr == nullptr) {
        return nullptr;
    }

    try {
        auto *problem = static_cast<pagmo::problem *>(problem_ptr);
        auto *population = new pagmo::population(*problem, pop_size, seed);
        return static_cast<void *>(population);
    } catch (...) {
        return nullptr;
    }
}

PAGMOSHARP_EXPORT void *pagmosharp_default_bfe_operator(void *bfe_ptr, void *problem_ptr, void *batch_x_ptr)
{
    if (bfe_ptr == nullptr || problem_ptr == nullptr || batch_x_ptr == nullptr) {
        return nullptr;
    }

    try {
        auto *bfe = static_cast<pagmo::default_bfe *>(bfe_ptr);
        auto *problem = static_cast<pagmo::problem *>(problem_ptr);
        auto *batch_x = static_cast<pagmo::vector_double *>(batch_x_ptr);

        auto result = (*bfe)(*problem, *batch_x);
        return static_cast<void *>(new pagmo::vector_double(std::move(result)));
    } catch (...) {
        return nullptr;
    }
}

PAGMOSHARP_EXPORT void *pagmosharp_thread_bfe_operator(void *bfe_ptr, void *problem_ptr, void *batch_x_ptr)
{
    if (bfe_ptr == nullptr || problem_ptr == nullptr || batch_x_ptr == nullptr) {
        return nullptr;
    }

    try {
        auto *bfe = static_cast<pagmo::thread_bfe *>(bfe_ptr);
        auto *problem = static_cast<pagmo::problem *>(problem_ptr);
        auto *batch_x = static_cast<pagmo::vector_double *>(batch_x_ptr);

        auto result = (*bfe)(*problem, *batch_x);
        return static_cast<void *>(new pagmo::vector_double(std::move(result)));
    } catch (...) {
        return nullptr;
    }
}

PAGMOSHARP_EXPORT void *pagmosharp_member_bfe_operator(void *bfe_ptr, void *problem_ptr, void *batch_x_ptr)
{
    if (bfe_ptr == nullptr || problem_ptr == nullptr || batch_x_ptr == nullptr) {
        return nullptr;
    }

    try {
        auto *bfe = static_cast<pagmo::member_bfe *>(bfe_ptr);
        auto *problem = static_cast<pagmo::problem *>(problem_ptr);
        auto *batch_x = static_cast<pagmo::vector_double *>(batch_x_ptr);

        auto result = (*bfe)(*problem, *batch_x);
        return static_cast<void *>(new pagmo::vector_double(std::move(result)));
    } catch (...) {
        return nullptr;
    }
}

PAGMOSHARP_EXPORT void *pagmosharp_estimate_gradient_problem(void *problem_ptr, void *x_ptr, double dx)
{
    if (problem_ptr == nullptr || x_ptr == nullptr) {
        return nullptr;
    }

    try {
        auto *problem = static_cast<pagmo::problem *>(problem_ptr);
        auto *x = static_cast<pagmo::vector_double *>(x_ptr);
        auto result = pagmo::estimate_gradient([problem](const pagmo::vector_double &dv) { return problem->fitness(dv); }, *x, dx);
        return static_cast<void *>(new pagmo::vector_double(std::move(result)));
    } catch (...) {
        return nullptr;
    }
}

PAGMOSHARP_EXPORT void *pagmosharp_estimate_gradient_h_problem(void *problem_ptr, void *x_ptr, double dx)
{
    if (problem_ptr == nullptr || x_ptr == nullptr) {
        return nullptr;
    }

    try {
        auto *problem = static_cast<pagmo::problem *>(problem_ptr);
        auto *x = static_cast<pagmo::vector_double *>(x_ptr);
        auto result
            = pagmo::estimate_gradient_h([problem](const pagmo::vector_double &dv) { return problem->fitness(dv); }, *x, dx);
        return static_cast<void *>(new pagmo::vector_double(std::move(result)));
    } catch (...) {
        return nullptr;
    }
}

PAGMOSHARP_EXPORT void *pagmosharp_estimate_sparsity_problem(void *problem_ptr, void *x_ptr, double dx)
{
    if (problem_ptr == nullptr || x_ptr == nullptr) {
        return nullptr;
    }

    try {
        auto *problem = static_cast<pagmo::problem *>(problem_ptr);
        auto *x = static_cast<pagmo::vector_double *>(x_ptr);
        auto result = pagmo::estimate_sparsity([problem](const pagmo::vector_double &dv) { return problem->fitness(dv); }, *x, dx);
        return static_cast<void *>(new pagmo::sparsity_pattern(std::move(result)));
    } catch (...) {
        return nullptr;
    }
}

} // extern "C"
