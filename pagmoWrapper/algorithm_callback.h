#pragma once

#include <memory>
#include <stdexcept>
#include <string>
#include <utility>

#include "pagmo/algorithm.hpp"
#include "pagmo/population.hpp"
#include "pagmo/threading.hpp"

namespace pagmoWrap {

class algorithm_callback
{
public:
    virtual ~algorithm_callback() = default;

    virtual pagmo::population evolve(const pagmo::population &pop) const = 0;

    virtual void set_seed(unsigned) {}
    virtual bool has_set_seed() const { return true; }

    virtual void set_verbosity(unsigned) {}
    virtual bool has_set_verbosity() const { return true; }

    virtual std::string get_name() const { return "C# algorithm"; }
    virtual std::string get_extra_info() const { return ""; }
    virtual pagmo::thread_safety get_thread_safety() const { return pagmo::thread_safety::basic; }

    // Boundary-safe channel for managed callback failures.
    // Implementations should return the current deferred error and clear it.
    virtual std::string consume_deferred_exception() { return ""; }
};

class managed_algorithm
{
public:
    class null_algorithm_callback final : public algorithm_callback
    {
    public:
        pagmo::population evolve(const pagmo::population &pop) const override
        {
            return pop;
        }
    };

    managed_algorithm()
        : m_cb(std::make_shared<null_algorithm_callback>())
    {
    }

    explicit managed_algorithm(algorithm_callback *cb)
        : managed_algorithm(std::shared_ptr<algorithm_callback>(cb))
    {
        if (!cb) {
            throw std::invalid_argument("managed_algorithm: callback must not be null");
        }
    }

    explicit managed_algorithm(std::shared_ptr<algorithm_callback> cb)
        : m_cb(std::move(cb))
    {
        if (!m_cb) {
            throw std::invalid_argument("managed_algorithm: callback must not be null");
        }
    }

    pagmo::population evolve(const pagmo::population &pop) const
    {
        auto evolved = m_cb->evolve(pop);
        throw_if_deferred("managed algorithm evolve");
        return evolved;
    }

    void set_seed(unsigned seed)
    {
        m_cb->set_seed(seed);
        throw_if_deferred("managed algorithm set_seed");
    }

    bool has_set_seed() const
    {
        return m_cb->has_set_seed();
    }

    void set_verbosity(unsigned level)
    {
        m_cb->set_verbosity(level);
        throw_if_deferred("managed algorithm set_verbosity");
    }

    bool has_set_verbosity() const
    {
        return m_cb->has_set_verbosity();
    }

    std::string get_name() const
    {
        return m_cb->get_name();
    }

    std::string get_extra_info() const
    {
        return m_cb->get_extra_info();
    }

    pagmo::thread_safety get_thread_safety() const
    {
        return m_cb->get_thread_safety();
    }

private:
    void throw_if_deferred(const char *context) const
    {
        auto message = m_cb->consume_deferred_exception();
        if (!message.empty()) {
            throw std::runtime_error(std::string(context) + ": " + message);
        }
    }

    std::shared_ptr<algorithm_callback> m_cb;
};

} // namespace pagmoWrap
