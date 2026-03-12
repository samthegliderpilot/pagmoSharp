[Pagmo](https://esa.github.io/pagmo2/) is a powerful library providing many high quality optimization routines in C++.  In an effort to learn more about C++ and to bring this ability into the .Net world, I'm creating this wrapper around pagmo for C# and other .Net languages.

There is a growing (aka. incomplete) [SWIG](https://www.swig.org/) interface file in the swigInterfaceFileAndPagmoHeaders folder.  When edits are made to that interface file, run the createSwigWrappersAndPlaceThem.bat file one directory up to regenerate the wrapers.  Note that the path of swig.exe is hard-coded in that bat file.  After that, build and run the Visual Studio solution normally.  The C++ project runs that bat file as a pre-build step.

Although pagmo has embraced a type-erasure style of coding for their problems, I'm embracing the OOP nature of C# and C++ to create a problem class that will have all the possible functions that a pagmo problem might implement.  This is going to be a hybrid of manually written code on both sides of the [un]managed line, but with SWIG director feature to assist with implementing UDP's in C#.  Right now, the problem class is the only one with custom C++ code, the rest of pagmo (so far) is working well with the swig .i file.

Not every function in every type in pagmo's hpp file's are going to get ported over.  Note that swig does not currently support varidec templates, so things like std::template<...> are not handled well. Pagmo does use these features and currently I'm mostly ignoring them until I can't.

This is still a work in progress with only a handful of types wrapped.  There are still some oddities that I am not sure are things I need to live with or if there are better ways to deal with.  I want to take my time before going nuts putting everything in the .i file.

As for requirements, pagmo 2.18, C++ 17, swig 4.0.2, .Net Core 6.0 (however nothing I'm doing should really require it and I want to look into downgrading it at some point), nUnit for unit testing.

Note that I am developing this on Windows 10, and used vcpkg to setup pagmo.  Although I hope to make the wrapper library cross-platform (hence choosing pinvoke instead of something like C++ CLI) this project isn't there yet.

## FAQ

### Why .Net and C#?
I think that the .Net ecosystem and languages are a bit under appreciated for scientific computing.  Although raw C/C++ code written by an expert will be faster, C# can get pretty close.  And with Microsoft open-sourcing so much of .Net with .Net Core... it has a lot going for it.

### Aren't you just making a wrapper of a wrapper?
Pagmo is more than just a wrapper.  Pygmo adds a consistent interface that wraps several other optimizers, as well as multithreading and multi-process support when available and appropriate.  That makes it more than just a wrapper.

### Why SWIG and not C++/CLI?
Several related reasons.  First, I wanted a P-Invoke wrapper to allow for the possibility of cross-platform support.  Also, SWIG takes care of all of the repetitive wrapping that a library like this needed.  Once I realized it exists, I just couldn't not use it.
Also, if someone wants to make wrappers for another language, the SWIG .i file will be a great start to that endeavor.  

### This hasn't implemented most of pagmo, why release it in such an incomplete state?
It's a work in progress.  Getting eyes on it sooner than later is worthwhile.

### Your automated tests are not really testing meaningful optimization problems.
True, but they don't have to.  These tests need to only test the wrappers; they do not need to test that the algorithms in pygmo work as well as they do.

### Where's IPOPT?
I need to learn how to build pagmo2 completely locally first, I'm using vcpkg to get a nlopt binary and it doesn't include IPOPT.

Also, this is made completely independently of the base pagmo and the team that makes and maintains it.  This is independent of ESA and the original developers of pagmo.

## VS Code workflow

Repo now includes VS Code tasks/launch config in `.vscode/`:

- `pagmoSharp: regenerate SWIG wrappers`
- `pagmoSharp: build native (Debug x64)`
- `pagmoSharp: build tests (Debug x64)`
- `pagmoSharp: test (Debug x64)`

Native build task uses `scripts/build-native.ps1` and finds `MSBuild.exe` via `vswhere`.
SWIG regen task uses `scripts/regen-swig.ps1`.
Test build/run tasks use `scripts/test.ps1` with staged execution (`build` then `test`).

### Requirements for local VS Code test runs

- Visual Studio Build Tools 2022 (or VS 2022) with MSBuild + C++ toolchain
- .NET SDK (net6-targeting project; newer SDKs also work)
- NuGet connectivity
- `pagmo2` headers/libs available at paths configured in `pagmoWrapper/pagmoWrapper.vcxproj`
- VS Code extensions:
  - `ms-dotnettools.csharp`
  - `ms-dotnettools.csdevkit`
  - `ms-vscode.cpptools`
  - `ms-vscode.powershell`

## Managed problem architecture (C# UDP support)

The core C# problem pipeline is:

1. User implements `IProblem` / `problemBase` in C#
2. A SWIG director adapter (`problem_callback`) forwards calls to managed code
3. Native bridge wraps callback into `managed_problem` (`std::shared_ptr` owned)
4. A real `pagmo::problem` is built from `managed_problem`
5. `population`, `archipelago`, and BFE operator helpers consume that `pagmo::problem`

This keeps ownership on the native side with `shared_ptr`, avoiding raw-pointer lifetime bugs for managed UDPs.

### Managed UDP authoring defaults

- Minimal managed UDPs can implement just:
  - `fitness(DoubleVector x)`
  - `get_bounds()`
- Optional capabilities (`batch_fitness`, gradients, hessians, seed hooks, metadata, thread safety) have defaults on `IProblem` / `problemBase`.
- `problemBase` includes helper methods for concise authoring:
  - `vec(...)`
  - `bounds(lower, upper)`

### Threading policy for managed UDPs

- `thread_bfe.Operator(IProblem, ...)` and `archipelago.push_back_island(..., IProblem, ...)` require explicit parallel-safety opt-in.
- Managed UDPs declaring `thread_safety.none` are rejected on those threaded entrypoints.
- Declare `thread_safety.basic` or `thread_safety.constant` when your managed UDP is safe for concurrent evaluation.

## Code style preferences

- Keep code lean and readable; avoid defensive scaffolding unless it provides clear operational value.
- Prefer direct checks near callsites over indirection-heavy policy layers when there are only a few callsites.
- Throw exceptions only when:
  - the message adds useful, novel context beyond the default exception/debugger signal, or
  - there is a strong boundary contract that benefits from explicit validation.
- Favor tight exception usage and concise error messages.
