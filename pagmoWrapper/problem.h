#pragma once

#include <memory>
#include <stdexcept>
#include <string>
#include <utility>
#include <vector>

#include "pagmo/problem.hpp"
#include "pagmo/threading.hpp"
#include "pagmo/types.hpp"

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

        // Required UDP surface
        virtual vector_double fitness(const vector_double& x) const = 0;
        virtual bounds_type get_bounds() const = 0;

        // Optional metadata
        virtual std::string get_name() const { return "C# problem"; }
        virtual std::string get_extra_info() const { return ""; }

        // Optional dimensions / constraints
        virtual vector_double::size_type get_nobj() const { return 1; }
        virtual vector_double::size_type get_nec() const { return 0; }
        virtual vector_double::size_type get_nic() const { return 0; }
        virtual vector_double::size_type get_nix() const { return 0; }

        // Optional batch/derivatives
        virtual vector_double batch_fitness(const vector_double&) const
        {
            throw std::runtime_error("batch_fitness() is not implemented in managed callback.");
        }
        virtual bool has_batch_fitness() const { return false; }

        virtual vector_double gradient(const vector_double&) const
        {
            throw std::runtime_error("gradient() is not implemented in managed callback.");
        }
        virtual bool has_gradient() const { return false; }

        virtual pagmo::sparsity_pattern gradient_sparsity() const
        {
            throw std::runtime_error("gradient_sparsity() is not implemented in managed callback.");
        }
        virtual bool has_gradient_sparsity() const { return false; }

        virtual std::vector<vector_double> hessians(const vector_double&) const
        {
            throw std::runtime_error("hessians() is not implemented in managed callback.");
        }
        virtual bool has_hessians() const { return false; }

        virtual std::vector<pagmo::sparsity_pattern> hessians_sparsity() const
        {
            throw std::runtime_error("hessians_sparsity() is not implemented in managed callback.");
        }
        virtual bool has_hessians_sparsity() const { return false; }

        // Optional stochastic hooks
        virtual void set_seed(unsigned)
        {
            throw std::runtime_error("set_seed() is not implemented in managed callback.");
        }
        virtual bool has_set_seed() const { return false; }

        // Optional thread safety metadata
        virtual pagmo::thread_safety get_thread_safety() const { return pagmo::thread_safety::none; }
    };

    class null_problem_callback : public problem_callback
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

    // -------------------------------------------------------------------------
    // 2) Copy-safe UDT that pagmo can store by value.
    //
    // This is the actual "problem implementation" in pagmo terms.
    //
    // It holds a shared_ptr to the director callback, so copies are safe.
    // -------------------------------------------------------------------------
    class managed_problem
    {

    public:
        managed_problem()
            : m_cb(std::make_shared<null_problem_callback>())
        {
        }

        explicit managed_problem(problem_callback* cb)
            : managed_problem(std::shared_ptr<problem_callback>(cb))
        {
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

        // Optional metadata / properties
        std::string get_name() const
        {
            return m_cb->get_name();
        }

        std::string get_extra_info() const
        {
            return m_cb->get_extra_info();
        }

        vector_double::size_type get_nobj() const
        {
            return m_cb->get_nobj();
        }

        vector_double::size_type get_nec() const { return m_cb->get_nec(); }
        vector_double::size_type get_nic() const { return m_cb->get_nic(); }
        vector_double::size_type get_nix() const { return m_cb->get_nix(); }

        vector_double batch_fitness(const vector_double& dvs) const
        {
            return m_cb->batch_fitness(dvs);
        }
        bool has_batch_fitness() const { return m_cb->has_batch_fitness(); }

        vector_double gradient(const vector_double& x) const
        {
            return m_cb->gradient(x);
        }
        bool has_gradient() const { return m_cb->has_gradient(); }

        pagmo::sparsity_pattern gradient_sparsity() const
        {
            return m_cb->gradient_sparsity();
        }
        bool has_gradient_sparsity() const { return m_cb->has_gradient_sparsity(); }

        std::vector<vector_double> hessians(const vector_double& x) const
        {
            return m_cb->hessians(x);
        }
        bool has_hessians() const { return m_cb->has_hessians(); }

        std::vector<pagmo::sparsity_pattern> hessians_sparsity() const
        {
            return m_cb->hessians_sparsity();
        }
        bool has_hessians_sparsity() const { return m_cb->has_hessians_sparsity(); }

        void set_seed(unsigned seed)
        {
            m_cb->set_seed(seed);
        }
        bool has_set_seed() const { return m_cb->has_set_seed(); }

        pagmo::thread_safety get_thread_safety() const
        {
            return m_cb->get_thread_safety();
        }

    private:
        std::shared_ptr<problem_callback> m_cb;
    };

} // namespace pagmoWrap
