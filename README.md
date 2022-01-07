Pagmo is a useful library providing many high quality optimization routines in C++.  In an effort to learn more about C++ and to bring this ability into the .Net world, I'm creating this wrapper around pagmo for C# and other .Net languages.

There is a growing SWIG interface file in the swigInterfaceFileAndPagmoHeaders.  When edits are made to that interface file, run the createSwigWrappersAndPlaceThem.bat file one directory up to regenerate the wrapers.  Note that the path of swig.exe is hard-coded in that bat file.  After that, build and run the Visual Studio solution normally.

Although pagmo has embrased a type-errasure style of coding for their problems, I'm embrasing the OOP nature of C# and C++ to create a problem class that will have all the possible functions that a pagmo problem might impliment.  This is going to be a hybrid of manually written code on both sides of the [un]managed line, but with SWIG director feature to assist with implimenting UDP's in C#.  

Not every function in every type in pagmo's hpp file's are going to get ported over.  If I'm going to make my own optimizer I will do it on the managed side, and other members just don't need to be exposed in a wrapper like this.  Note that swig does not currently support varidec templates, so things like std::template<...> are not handeled well. Pagmo does use these features and currently I'm mostly ignoring them.

As for requirements, pagmo 2.18, C++ 17, swig 4.0.2, .Net Core 3.0, xUnit for unit testing.
