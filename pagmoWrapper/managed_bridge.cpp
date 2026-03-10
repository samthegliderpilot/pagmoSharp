#include <cstddef>
#include <memory>
#include <utility>

#include <pagmo/problem.hpp>
#include <pagmo/population.hpp>
#include <pagmo/batch_evaluators/default_bfe.hpp>
#include <pagmo/batch_evaluators/thread_bfe.hpp>
#include <pagmo/batch_evaluators/member_bfe.hpp>
#include <pagmo/types.hpp>

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

} // extern "C"
