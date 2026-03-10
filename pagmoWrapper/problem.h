#pragma once

#include <memory>
#include <stdexcept>
#include <string>
#include <utility>
#include <vector>

#include "pagmo/problem.hpp"
#include "pagmo/types.hpp"
#include "pagmo/threading.hpp" // for pagmo::thread_safety (depending on your pagmo includes)

// NOTE: Keep everything in your existing namespace to minimize changes elsewhere.
namespace pagmoWrap {

    using vector_double = std::vector<double>;
    using bounds_type = std::pair<vector_double, vector_double>;

    // -------------------------------------------------------------------------
    // 1) Director interface: C# will subclass/override this.
    //
    // IMPORTANT:
    // - This type must NOT be what pagmo stores/copies.
    // - This is only for callbacks into managed code.
    // -------------------------------------------------------------------------
    class problem_callback
    {
    public:
        virtual ~problem_callback() = default;

        // Required for v0.1
        virtual vector_double fitness(const vector_double& x) const = 0;
        virtual bounds_type get_bounds() const = 0;

        // Optional metadata (v0.1)
        virtual std::string get_name() const { return "C# problem"; }

        // v0.1: single-objective only
        virtual vector_double::size_type get_nobj() const { return 1; }

        // v0.1: no constraints
        virtual vector_double::size_type get_nec() const { return 0; }
        virtual vector_double::size_type get_nic() const { return 0; }

        // v0.1: no integer variables
        virtual vector_double::size_type get_nix() const { return 0; }

        // v0.1: no thread safety guarantees
        virtual pagmo::thread_safety get_thread_safety() const { return pagmo::thread_safety::none; }
    };

    // -------------------------------------------------------------------------
    // 2) Copy-safe UDT that pagmo can store by value.
    //
    // This is the actual "problem implementation" in pagmo terms.
    //
    // It holds a shared_ptr to the director callback, so copies are safe.
    // -------------------------------------------------------------------------
    class managed_problem
    {
        class null_problem_callback final : public problem_callback
        {
        public:
            vector_double fitness(const vector_double&) const override
            {
                return {0.0};
            }

            bounds_type get_bounds() const override
            {
                return bounds_type{vector_double{0.0}, vector_double{1.0}};
            }
        };

    public:
        managed_problem()
            : m_cb(std::make_shared<null_problem_callback>())
        {
        }

        explicit managed_problem(problem_callback* cb)
            : managed_problem(std::shared_ptr<problem_callback>(cb))
        {
            if (!cb) {
                throw std::invalid_argument("managed_problem: callback must not be null");
            }
        }

        explicit managed_problem(std::shared_ptr<problem_callback> cb)
            : m_cb(std::move(cb))
        {
            if (!m_cb) {
                throw std::invalid_argument("managed_problem: callback must not be null");
            }
        }

        // Required UDT methods
        vector_double fitness(const vector_double& x) const
        {
            auto f = m_cb->fitness(x);

            // v0.1 enforce single objective
            if (f.size() != 1) {
                throw std::runtime_error("managed_problem: v0.1 supports only fitness size == 1");
            }
            return f;
        }

        bounds_type get_bounds() const
        {
            auto b = m_cb->get_bounds();

            // Basic validation (helps catch errors early)
            if (b.first.size() != b.second.size()) {
                throw std::runtime_error("managed_problem: bounds lower/upper size mismatch");
            }
            if (b.first.empty()) {
                throw std::runtime_error("managed_problem: bounds must not be empty");
            }
            return b;
        }

        // Optional metadata
        std::string get_name() const
        {
            return m_cb->get_name();
        }

        vector_double::size_type get_nobj() const
        {
            // v0.1: enforce single objective
            const auto n = m_cb->get_nobj();
            if (n != 1) {
                throw std::runtime_error("managed_problem: v0.1 supports only nobj == 1");
            }
            return 1;
        }

        vector_double::size_type get_nec() const { return m_cb->get_nec(); }
        vector_double::size_type get_nic() const { return m_cb->get_nic(); }
        vector_double::size_type get_nix() const { return m_cb->get_nix(); }

        pagmo::thread_safety get_thread_safety() const
        {
            return m_cb->get_thread_safety();
        }

    private:
        std::shared_ptr<problem_callback> m_cb;
    };

} // namespace pagmoWrap
