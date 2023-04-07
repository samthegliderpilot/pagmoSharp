[Pagmo](https://esa.github.io/pagmo2/) is a powerful library providing many high quality optimization routines in C++.  In an effort to learn more about C++ and to bring this ability into the .Net world, I'm creating this wrapper around pagmo for C# and other .Net languages.

There is a growing (aka. incomplete) [SWIG](https://www.swig.org/) interface file in the swigInterfaceFileAndPagmoHeaders folder.  When edits are made to that interface file, run the createSwigWrappersAndPlaceThem.bat file one directory up to regenerate the wrapers.  Note that the path of swig.exe is hard-coded in that bat file.  After that, build and run the Visual Studio solution normally.  The C++ project runs that bat file as a pre-build step.

Although pagmo has embraced a type-erasure style of coding for their problems, I'm embracing the OOP nature of C# and C++ to create a problem class that will have all the possible functions that a pagmo problem might implement.  This is going to be a hybrid of manually written code on both sides of the [un]managed line, but with SWIG director feature to assist with implementing UDP's in C#.  Right now, the problem class is the only one with custom C++ code, the rest of pagmo (so far) is working well with the swig .i file.

Not every function in every type in pagmo's hpp file's are going to get ported over.  Note that swig does not currently support varidec templates, so things like std::template<...> are not handled well. Pagmo does use these features and currently I'm mostly ignoring them until I can't.

This is still a work in progress with only a handful of types wrapped.  There are still some oddities that I am not sure are things I need to live with or if there are better ways to deal with.  I want to take my time before going nuts putting everything in the .i file.

As for requirements, pagmo 2.18, C++ 17, swig 4.0.2, .Net Core 3.0 (however nothing I'm doing should really require it and I want to look into downgrading it at some point), nUnit for unit testing.

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

Some of the pygmo functions are difficult to make SWIG wrappers for (the varidec templates).  Some I think don't need to be wrapped (archiving for example).  But yes, there are many functions and types that are not completed yet.  I've spent a good deal of time trying to figure out how to do inheritance of some of the types in pygmo, and I'm not really happy with the solution I've settled on (which is handling it on the .Net side with partial classes).  I hope that maybe someone will look at the .i file and say "if you do it this way, it will be a lot better".  

Also, SWIG currently has problems with move and copy constructors, but a change submitted in late 2022 might fix it.  Until that fix is in a release, nlopt wrapping solvers is not possible.

### Your automated tests are not really testing meaningful optimization problems.

True, but they don't have to.  These tests need to only test the wrappers; they do not need to test that the algorithms in pygmo work as well as they do.


Also, this is made completely independently of the base pagmo and the team that makes and maintains it.  This is independent of ESA and the original developers of pagmo.
