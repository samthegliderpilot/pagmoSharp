# API Reference (Generated)

This document enumerates the public API visible to consumers of pagmoSharp.

- Generated from assembly: C:\src\pagmoSharp\pagmoSharp\bin\Debug\net10.0\pagmoSharp.dll
- Upstream pagmo C++ API index: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html
- Generation timestamp (UTC): 2026-04-15 03:04:40Z

## pagmo.BeeColonyLogLine

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `BeeColonyLogLine()`
- `BeeColonyLogLine(uint g, ulong f, double b, double cb)`

### Properties

- `double best { get; set }`
- `double cur_best { get; set }`
- `ulong fevals { get; set }`
- `uint gen { get; set }`

### Methods

- `void Dispose()`

## pagmo.BeeColonyLogLineVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `BeeColonyLogLineVector()`
- `BeeColonyLogLineVector(int capacity)`
- `BeeColonyLogLineVector(IEnumerable<BeeColonyLogLine> c)`
- `BeeColonyLogLineVector(IEnumerable c)`
- `BeeColonyLogLineVector(BeeColonyLogLineVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `BeeColonyLogLine Item { get; set }`

### Methods

- `void Add(BeeColonyLogLine x)`
- `void AddRange(BeeColonyLogLineVector values)`
- `void Clear()`
- `void CopyTo(BeeColonyLogLine[] array)`
- `void CopyTo(BeeColonyLogLine[] array, int arrayIndex)`
- `void CopyTo(int index, BeeColonyLogLine[] array, int arrayIndex, int count)`
- `void Dispose()`
- `BeeColonyLogLineVectorEnumerator GetEnumerator()`
- `BeeColonyLogLineVector GetRange(int index, int count)`
- `void Insert(int index, BeeColonyLogLine x)`
- `void InsertRange(int index, BeeColonyLogLineVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static BeeColonyLogLineVector Repeat(BeeColonyLogLine value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, BeeColonyLogLineVector values)`
- `BeeColonyLogLine[] ToArray()`

## pagmo.BeeColonyLogLineVector+BeeColonyLogLineVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `BeeColonyLogLineVectorEnumerator(BeeColonyLogLineVector collection)`

### Properties

- `BeeColonyLogLine Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.CmaesLogEntry

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `CmaesLogEntry()`
- `CmaesLogEntry(uint gen_, ulong fevals_, double best_, double sigma_, double min_variance_, double max_variance_)`

### Properties

- `double best { get; set }`
- `ulong fevals { get; set }`
- `uint gen { get; set }`
- `double max_variance { get; set }`
- `double min_variance { get; set }`
- `double sigma { get; set }`

### Methods

- `void Dispose()`

## pagmo.CmaesLogEntryVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `CmaesLogEntryVector()`
- `CmaesLogEntryVector(int capacity)`
- `CmaesLogEntryVector(IEnumerable<CmaesLogEntry> c)`
- `CmaesLogEntryVector(IEnumerable c)`
- `CmaesLogEntryVector(CmaesLogEntryVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `CmaesLogEntry Item { get; set }`

### Methods

- `void Add(CmaesLogEntry x)`
- `void AddRange(CmaesLogEntryVector values)`
- `void Clear()`
- `void CopyTo(CmaesLogEntry[] array)`
- `void CopyTo(CmaesLogEntry[] array, int arrayIndex)`
- `void CopyTo(int index, CmaesLogEntry[] array, int arrayIndex, int count)`
- `void Dispose()`
- `CmaesLogEntryVectorEnumerator GetEnumerator()`
- `CmaesLogEntryVector GetRange(int index, int count)`
- `void Insert(int index, CmaesLogEntry x)`
- `void InsertRange(int index, CmaesLogEntryVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static CmaesLogEntryVector Repeat(CmaesLogEntry value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, CmaesLogEntryVector values)`
- `CmaesLogEntry[] ToArray()`

## pagmo.CmaesLogEntryVector+CmaesLogEntryVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `CmaesLogEntryVectorEnumerator(CmaesLogEntryVector collection)`

### Properties

- `CmaesLogEntry Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.CompassSearchLogEntry

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `CompassSearchLogEntry()`

### Properties

- `double best { get; set }`
- `ulong fevals { get; set }`
- `double range { get; set }`
- `ulong violated { get; set }`
- `double violation_norm { get; set }`

### Methods

- `void Dispose()`

## pagmo.CompassSearchLogEntryVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `CompassSearchLogEntryVector()`
- `CompassSearchLogEntryVector(int capacity)`
- `CompassSearchLogEntryVector(IEnumerable<CompassSearchLogEntry> c)`
- `CompassSearchLogEntryVector(IEnumerable c)`
- `CompassSearchLogEntryVector(CompassSearchLogEntryVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `CompassSearchLogEntry Item { get; set }`

### Methods

- `void Add(CompassSearchLogEntry x)`
- `void AddRange(CompassSearchLogEntryVector values)`
- `void Clear()`
- `void CopyTo(CompassSearchLogEntry[] array)`
- `void CopyTo(CompassSearchLogEntry[] array, int arrayIndex)`
- `void CopyTo(int index, CompassSearchLogEntry[] array, int arrayIndex, int count)`
- `void Dispose()`
- `CompassSearchLogEntryVectorEnumerator GetEnumerator()`
- `CompassSearchLogEntryVector GetRange(int index, int count)`
- `void Insert(int index, CompassSearchLogEntry x)`
- `void InsertRange(int index, CompassSearchLogEntryVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static CompassSearchLogEntryVector Repeat(CompassSearchLogEntry value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, CompassSearchLogEntryVector values)`
- `CompassSearchLogEntry[] ToArray()`

## pagmo.CompassSearchLogEntryVector+CompassSearchLogEntryVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `CompassSearchLogEntryVectorEnumerator(CompassSearchLogEntryVector collection)`

### Properties

- `CompassSearchLogEntry Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.CstrsLogEntry

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `CstrsLogEntry()`
- `CstrsLogEntry(uint iter_, ulong fevals_, double best_, double infeasibility_, ulong violated_, double violation_norm_, ulong feasible_)`

### Properties

- `double best { get; set }`
- `ulong feasible { get; set }`
- `ulong fevals { get; set }`
- `double infeasibility { get; set }`
- `uint iter { get; set }`
- `ulong violated { get; set }`
- `double violation_norm { get; set }`

### Methods

- `void Dispose()`

## pagmo.CstrsLogEntryVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `CstrsLogEntryVector()`
- `CstrsLogEntryVector(int capacity)`
- `CstrsLogEntryVector(IEnumerable<CstrsLogEntry> c)`
- `CstrsLogEntryVector(IEnumerable c)`
- `CstrsLogEntryVector(CstrsLogEntryVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `CstrsLogEntry Item { get; set }`

### Methods

- `void Add(CstrsLogEntry x)`
- `void AddRange(CstrsLogEntryVector values)`
- `void Clear()`
- `void CopyTo(CstrsLogEntry[] array)`
- `void CopyTo(CstrsLogEntry[] array, int arrayIndex)`
- `void CopyTo(int index, CstrsLogEntry[] array, int arrayIndex, int count)`
- `void Dispose()`
- `CstrsLogEntryVectorEnumerator GetEnumerator()`
- `CstrsLogEntryVector GetRange(int index, int count)`
- `void Insert(int index, CstrsLogEntry x)`
- `void InsertRange(int index, CstrsLogEntryVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static CstrsLogEntryVector Repeat(CstrsLogEntry value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, CstrsLogEntryVector values)`
- `CstrsLogEntry[] ToArray()`

## pagmo.CstrsLogEntryVector+CstrsLogEntryVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `CstrsLogEntryVectorEnumerator(CstrsLogEntryVector collection)`

### Properties

- `CstrsLogEntry Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.De1220LogEntry

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `De1220LogEntry()`

### Properties

- `double best { get; set }`
- `double cr { get; set }`
- `double dx { get; set }`
- `double f { get; set }`
- `double feval_difference { get; set }`
- `ulong fevals { get; set }`
- `uint gen { get; set }`
- `uint variant { get; set }`

### Methods

- `void Dispose()`

## pagmo.De1220LogEntryVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `De1220LogEntryVector()`
- `De1220LogEntryVector(int capacity)`
- `De1220LogEntryVector(IEnumerable<De1220LogEntry> c)`
- `De1220LogEntryVector(IEnumerable c)`
- `De1220LogEntryVector(De1220LogEntryVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `De1220LogEntry Item { get; set }`

### Methods

- `void Add(De1220LogEntry x)`
- `void AddRange(De1220LogEntryVector values)`
- `void Clear()`
- `void CopyTo(De1220LogEntry[] array)`
- `void CopyTo(De1220LogEntry[] array, int arrayIndex)`
- `void CopyTo(int index, De1220LogEntry[] array, int arrayIndex, int count)`
- `void Dispose()`
- `De1220LogEntryVectorEnumerator GetEnumerator()`
- `De1220LogEntryVector GetRange(int index, int count)`
- `void Insert(int index, De1220LogEntry x)`
- `void InsertRange(int index, De1220LogEntryVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static De1220LogEntryVector Repeat(De1220LogEntry value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, De1220LogEntryVector values)`
- `De1220LogEntry[] ToArray()`

## pagmo.De1220LogEntryVector+De1220LogEntryVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `De1220LogEntryVectorEnumerator(De1220LogEntryVector collection)`

### Properties

- `De1220LogEntry Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.DeLogEntry

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `DeLogEntry()`
- `DeLogEntry(uint gen_, ulong fevals_, double best_, double feval_difference_, double dx_)`

### Properties

- `double best { get; set }`
- `double dx { get; set }`
- `double feval_difference { get; set }`
- `ulong fevals { get; set }`
- `uint gen { get; set }`

### Methods

- `void Dispose()`

## pagmo.DeLogEntryVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `DeLogEntryVector()`
- `DeLogEntryVector(int capacity)`
- `DeLogEntryVector(IEnumerable<DeLogEntry> c)`
- `DeLogEntryVector(IEnumerable c)`
- `DeLogEntryVector(DeLogEntryVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `DeLogEntry Item { get; set }`

### Methods

- `void Add(DeLogEntry x)`
- `void AddRange(DeLogEntryVector values)`
- `void Clear()`
- `void CopyTo(DeLogEntry[] array)`
- `void CopyTo(DeLogEntry[] array, int arrayIndex)`
- `void CopyTo(int index, DeLogEntry[] array, int arrayIndex, int count)`
- `void Dispose()`
- `DeLogEntryVectorEnumerator GetEnumerator()`
- `DeLogEntryVector GetRange(int index, int count)`
- `void Insert(int index, DeLogEntry x)`
- `void InsertRange(int index, DeLogEntryVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static DeLogEntryVector Repeat(DeLogEntry value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, DeLogEntryVector values)`
- `DeLogEntry[] ToArray()`

## pagmo.DeLogEntryVector+DeLogEntryVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `DeLogEntryVectorEnumerator(DeLogEntryVector collection)`

### Properties

- `DeLogEntry Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.DecompositionWeights

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `DecompositionWeights()`

### Properties

- `string METHOD_GRID { get }`
- `string METHOD_LOW_DISCREPANCY { get }`
- `string METHOD_RANDOM { get }`

### Methods

- `void Dispose()`
- `static VectorOfVectorOfDoubles decomposition_weights(uint n_f, uint n_w, string method)`

## pagmo.DoubleVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `DoubleVector()`
- `DoubleVector(double[] values)`
- `DoubleVector(int capacity)`
- `DoubleVector(IEnumerable<double> c)`
- `DoubleVector(IEnumerable c)`
- `DoubleVector(DoubleVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `double Item { get; set }`

### Methods

- `void Add(double x)`
- `void AddRange(DoubleVector values)`
- `void Clear()`
- `bool Contains(double value)`
- `void CopyTo(double[] array)`
- `void CopyTo(double[] array, int arrayIndex)`
- `void CopyTo(int index, double[] array, int arrayIndex, int count)`
- `void Dispose()`
- `DoubleVectorEnumerator GetEnumerator()`
- `DoubleVector GetRange(int index, int count)`
- `int IndexOf(double value)`
- `void Insert(int index, double x)`
- `void InsertRange(int index, DoubleVector values)`
- `int LastIndexOf(double value)`
- `bool Remove(double value)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static DoubleVector Repeat(double value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, DoubleVector values)`
- `double[] ToArray()`

## pagmo.DoubleVector+DoubleVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `DoubleVectorEnumerator(DoubleVector collection)`

### Properties

- `double Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.FNDSResult

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `FNDSResult()`

### Properties

- `ULongLongVector domination_counts { get; set }`
- `VectorOfVectorIndexes fronts { get; set }`
- `ULongLongVector rank_indices { get; set }`
- `VectorOfVectorIndexes ranks { get; set }`

### Methods

- `void Dispose()`

## pagmo.GacoLogEntry

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `GacoLogEntry()`
- `GacoLogEntry(uint gen_, ulong fevals_, double best_fit_, uint kernel_, double oracle_, double dx_, double dp_)`

### Properties

- `double best_fit { get; set }`
- `double dp { get; set }`
- `double dx { get; set }`
- `ulong fevals { get; set }`
- `uint gen { get; set }`
- `uint kernel { get; set }`
- `double oracle { get; set }`

### Methods

- `void Dispose()`

## pagmo.GacoLogEntryVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `GacoLogEntryVector()`
- `GacoLogEntryVector(int capacity)`
- `GacoLogEntryVector(IEnumerable<GacoLogEntry> c)`
- `GacoLogEntryVector(IEnumerable c)`
- `GacoLogEntryVector(GacoLogEntryVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `GacoLogEntry Item { get; set }`

### Methods

- `void Add(GacoLogEntry x)`
- `void AddRange(GacoLogEntryVector values)`
- `void Clear()`
- `void CopyTo(GacoLogEntry[] array)`
- `void CopyTo(GacoLogEntry[] array, int arrayIndex)`
- `void CopyTo(int index, GacoLogEntry[] array, int arrayIndex, int count)`
- `void Dispose()`
- `GacoLogEntryVectorEnumerator GetEnumerator()`
- `GacoLogEntryVector GetRange(int index, int count)`
- `void Insert(int index, GacoLogEntry x)`
- `void InsertRange(int index, GacoLogEntryVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static GacoLogEntryVector Repeat(GacoLogEntry value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, GacoLogEntryVector values)`
- `GacoLogEntry[] ToArray()`

## pagmo.GacoLogEntryVector+GacoLogEntryVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `GacoLogEntryVectorEnumerator(GacoLogEntryVector collection)`

### Properties

- `GacoLogEntry Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.GacoLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `GacoLogLine(uint Generation, ulong FunctionEvaluations, double BestFitness, uint KernelSize, double OracleValue, double Dx, double Dp)`

### Properties

- `string AlgorithmName { get }`
- `double BestFitness { get; set }`
- `double Dp { get; set }`
- `double Dx { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `uint Generation { get; set }`
- `uint KernelSize { get; set }`
- `double OracleValue { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`

### Methods

- `void Deconstruct(out uint Generation, out ulong FunctionEvaluations, out double BestFitness, out uint KernelSize, out double OracleValue, out double Dx, out double Dp)`
- `bool Equals(object obj)`
- `bool Equals(GacoLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.GradientsAndHessians

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Methods

- `static DoubleVector EstimateGradient(problem prob, DoubleVector x, double dx)`
- `static DoubleVector EstimateGradient(IProblem prob, DoubleVector x, double dx)`
- `static DoubleVector EstimateGradientHighOrder(problem prob, DoubleVector x, double dx)`
- `static DoubleVector EstimateGradientHighOrder(IProblem prob, DoubleVector x, double dx)`
- `static SparsityPattern EstimateSparsity(problem prob, DoubleVector x, double dx)`
- `static SparsityPattern EstimateSparsity(IProblem prob, DoubleVector x, double dx)`
- `static SparsityIndex[] EstimateSparsityEntries(problem prob, DoubleVector x, double dx)`
- `static SparsityIndex[] EstimateSparsityEntries(IProblem prob, DoubleVector x, double dx)`

## pagmo.GwoLogEntry

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `GwoLogEntry()`

### Properties

- `double alpha { get; set }`
- `double beta { get; set }`
- `double delta { get; set }`
- `uint gen { get; set }`

### Methods

- `void Dispose()`

## pagmo.GwoLogEntryVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `GwoLogEntryVector()`
- `GwoLogEntryVector(int capacity)`
- `GwoLogEntryVector(IEnumerable<GwoLogEntry> c)`
- `GwoLogEntryVector(IEnumerable c)`
- `GwoLogEntryVector(GwoLogEntryVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `GwoLogEntry Item { get; set }`

### Methods

- `void Add(GwoLogEntry x)`
- `void AddRange(GwoLogEntryVector values)`
- `void Clear()`
- `void CopyTo(GwoLogEntry[] array)`
- `void CopyTo(GwoLogEntry[] array, int arrayIndex)`
- `void CopyTo(int index, GwoLogEntry[] array, int arrayIndex, int count)`
- `void Dispose()`
- `GwoLogEntryVectorEnumerator GetEnumerator()`
- `GwoLogEntryVector GetRange(int index, int count)`
- `void Insert(int index, GwoLogEntry x)`
- `void InsertRange(int index, GwoLogEntryVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static GwoLogEntryVector Repeat(GwoLogEntry value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, GwoLogEntryVector values)`
- `GwoLogEntry[] ToArray()`

## pagmo.GwoLogEntryVector+GwoLogEntryVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `GwoLogEntryVectorEnumerator(GwoLogEntryVector collection)`

### Properties

- `GwoLogEntry Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.HvAlgorithmSharedPtr

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `HvAlgorithmSharedPtr()`

### Methods

- `void Dispose()`

## pagmo.IAlgorithm

- Kind: interface
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Methods

- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `population evolve(population pop)`
- `string get_extra_info()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_seed(uint seed)`
- `void set_verbosity(uint level)`

## pagmo.IAlgorithmLogLine

- Kind: interface
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Properties

- `string AlgorithmName { get }`
- `IReadOnlyDictionary<string, object> RawFields { get }`

### Methods

- `string ToDisplayString()`

## pagmo.IProblem

- Kind: interface
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Methods

- `DoubleVector batch_fitness(DoubleVector dvs)`
- `DoubleVector fitness(DoubleVector x)`
- `PairOfDoubleVectors get_bounds()`
- `string get_extra_info()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `DoubleVector gradient(DoubleVector x)`
- `SparsityPattern gradient_sparsity()`
- `bool has_batch_fitness()`
- `bool has_gradient()`
- `bool has_gradient_sparsity()`
- `bool has_hessians()`
- `bool has_hessians_sparsity()`
- `bool has_set_seed()`
- `VectorOfVectorOfDoubles hessians(DoubleVector x)`
- `VectorOfSparsityPattern hessians_sparsity()`
- `void set_seed(uint seed)`

## pagmo.IhsLogEntry

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `IhsLogEntry()`
- `IhsLogEntry(ulong fevals_, double ppar_, double bw_, double dx_, double df_, ulong violated_, double violation_norm_, DoubleVector ideal_)`

### Properties

- `double bw { get; set }`
- `double df { get; set }`
- `double dx { get; set }`
- `ulong fevals { get; set }`
- `DoubleVector ideal { get; set }`
- `double ppar { get; set }`
- `ulong violated { get; set }`
- `double violation_norm { get; set }`

### Methods

- `void Dispose()`

## pagmo.IhsLogEntryVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `IhsLogEntryVector()`
- `IhsLogEntryVector(int capacity)`
- `IhsLogEntryVector(IEnumerable<IhsLogEntry> c)`
- `IhsLogEntryVector(IEnumerable c)`
- `IhsLogEntryVector(IhsLogEntryVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `IhsLogEntry Item { get; set }`

### Methods

- `void Add(IhsLogEntry x)`
- `void AddRange(IhsLogEntryVector values)`
- `void Clear()`
- `void CopyTo(IhsLogEntry[] array)`
- `void CopyTo(IhsLogEntry[] array, int arrayIndex)`
- `void CopyTo(int index, IhsLogEntry[] array, int arrayIndex, int count)`
- `void Dispose()`
- `IhsLogEntryVectorEnumerator GetEnumerator()`
- `IhsLogEntryVector GetRange(int index, int count)`
- `void Insert(int index, IhsLogEntry x)`
- `void InsertRange(int index, IhsLogEntryVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static IhsLogEntryVector Repeat(IhsLogEntry value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, IhsLogEntryVector values)`
- `IhsLogEntry[] ToArray()`

## pagmo.IhsLogEntryVector+IhsLogEntryVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `IhsLogEntryVectorEnumerator(IhsLogEntryVector collection)`

### Properties

- `IhsLogEntry Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.IndividualsGroup

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `IndividualsGroup()`
- `IndividualsGroup(ULongLongVector ids_, VectorOfVectorOfDoubles xs_, VectorOfVectorOfDoubles fs_)`

### Properties

- `VectorOfVectorOfDoubles fs { get; set }`
- `ULongLongVector ids { get; set }`
- `VectorOfVectorOfDoubles xs { get; set }`

### Methods

- `void Dispose()`

## pagmo.IndividualsGroupVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `IndividualsGroupVector()`
- `IndividualsGroupVector(int capacity)`
- `IndividualsGroupVector(IEnumerable<IndividualsGroup> c)`
- `IndividualsGroupVector(IEnumerable c)`
- `IndividualsGroupVector(IndividualsGroupVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `IndividualsGroup Item { get; set }`

### Methods

- `void Add(IndividualsGroup x)`
- `void AddRange(IndividualsGroupVector values)`
- `void Clear()`
- `void CopyTo(IndividualsGroup[] array)`
- `void CopyTo(IndividualsGroup[] array, int arrayIndex)`
- `void CopyTo(int index, IndividualsGroup[] array, int arrayIndex, int count)`
- `void Dispose()`
- `IndividualsGroupVectorEnumerator GetEnumerator()`
- `IndividualsGroupVector GetRange(int index, int count)`
- `void Insert(int index, IndividualsGroup x)`
- `void InsertRange(int index, IndividualsGroupVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static IndividualsGroupVector Repeat(IndividualsGroup value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, IndividualsGroupVector values)`
- `IndividualsGroup[] ToArray()`

## pagmo.IndividualsGroupVector+IndividualsGroupVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `IndividualsGroupVectorEnumerator(IndividualsGroupVector collection)`

### Properties

- `IndividualsGroup Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.IpoptLogEntry

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `IpoptLogEntry()`

### Properties

- `bool feasible { get; set }`
- `double objective { get; set }`
- `ulong objective_evaluations { get; set }`
- `ulong violated { get; set }`
- `double violation_norm { get; set }`

### Methods

- `void Dispose()`

## pagmo.IpoptLogEntryVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `IpoptLogEntryVector()`
- `IpoptLogEntryVector(int capacity)`
- `IpoptLogEntryVector(IEnumerable<IpoptLogEntry> c)`
- `IpoptLogEntryVector(IEnumerable c)`
- `IpoptLogEntryVector(IpoptLogEntryVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `IpoptLogEntry Item { get; set }`

### Methods

- `void Add(IpoptLogEntry x)`
- `void AddRange(IpoptLogEntryVector values)`
- `void Clear()`
- `void CopyTo(IpoptLogEntry[] array)`
- `void CopyTo(IpoptLogEntry[] array, int arrayIndex)`
- `void CopyTo(int index, IpoptLogEntry[] array, int arrayIndex, int count)`
- `void Dispose()`
- `IpoptLogEntryVectorEnumerator GetEnumerator()`
- `IpoptLogEntryVector GetRange(int index, int count)`
- `void Insert(int index, IpoptLogEntry x)`
- `void InsertRange(int index, IpoptLogEntryVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static IpoptLogEntryVector Repeat(IpoptLogEntry value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, IpoptLogEntryVector values)`
- `IpoptLogEntry[] ToArray()`

## pagmo.IpoptLogEntryVector+IpoptLogEntryVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `IpoptLogEntryVectorEnumerator(IpoptLogEntryVector collection)`

### Properties

- `IpoptLogEntry Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.ManagedProblemBase

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Methods

- `void Dispose()`
- `DoubleVector batch_fitness(DoubleVector dvs)`
- `DoubleVector fitness(DoubleVector x)`
- `PairOfDoubleVectors get_bounds()`
- `string get_extra_info()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `DoubleVector gradient(DoubleVector x)`
- `SparsityPattern gradient_sparsity()`
- `bool has_batch_fitness()`
- `bool has_gradient()`
- `bool has_gradient_sparsity()`
- `bool has_hessians()`
- `bool has_hessians_sparsity()`
- `bool has_set_seed()`
- `VectorOfVectorOfDoubles hessians(DoubleVector x)`
- `VectorOfSparsityPattern hessians_sparsity()`
- `void set_seed(uint seed)`

## pagmo.MbhLogEntry

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `MbhLogEntry()`
- `MbhLogEntry(ulong fevals_, double best_, uint violated_, double violation_norm_, uint trial_)`

### Properties

- `double best { get; set }`
- `ulong fevals { get; set }`
- `uint trial { get; set }`
- `uint violated { get; set }`
- `double violation_norm { get; set }`

### Methods

- `void Dispose()`

## pagmo.MbhLogEntryVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `MbhLogEntryVector()`
- `MbhLogEntryVector(int capacity)`
- `MbhLogEntryVector(IEnumerable<MbhLogEntry> c)`
- `MbhLogEntryVector(IEnumerable c)`
- `MbhLogEntryVector(MbhLogEntryVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `MbhLogEntry Item { get; set }`

### Methods

- `void Add(MbhLogEntry x)`
- `void AddRange(MbhLogEntryVector values)`
- `void Clear()`
- `void CopyTo(MbhLogEntry[] array)`
- `void CopyTo(MbhLogEntry[] array, int arrayIndex)`
- `void CopyTo(int index, MbhLogEntry[] array, int arrayIndex, int count)`
- `void Dispose()`
- `MbhLogEntryVectorEnumerator GetEnumerator()`
- `MbhLogEntryVector GetRange(int index, int count)`
- `void Insert(int index, MbhLogEntry x)`
- `void InsertRange(int index, MbhLogEntryVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static MbhLogEntryVector Repeat(MbhLogEntry value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, MbhLogEntryVector values)`
- `MbhLogEntry[] ToArray()`

## pagmo.MbhLogEntryVector+MbhLogEntryVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `MbhLogEntryVectorEnumerator(MbhLogEntryVector collection)`

### Properties

- `MbhLogEntry Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.MigrationEntry

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `MigrationEntry()`
- `MigrationEntry(double t_, ulong island_id_, DoubleVector x_, DoubleVector f_, ulong migration_id_, ulong immigrant_id_)`

### Properties

- `DoubleVector f { get; set }`
- `ulong immigrant_id { get; set }`
- `ulong island_id { get; set }`
- `ulong migration_id { get; set }`
- `double t { get; set }`
- `DoubleVector x { get; set }`

### Methods

- `void Dispose()`

## pagmo.MigrationEntryVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `MigrationEntryVector()`
- `MigrationEntryVector(int capacity)`
- `MigrationEntryVector(IEnumerable<MigrationEntry> c)`
- `MigrationEntryVector(IEnumerable c)`
- `MigrationEntryVector(MigrationEntryVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `MigrationEntry Item { get; set }`

### Methods

- `void Add(MigrationEntry x)`
- `void AddRange(MigrationEntryVector values)`
- `void Clear()`
- `void CopyTo(MigrationEntry[] array)`
- `void CopyTo(MigrationEntry[] array, int arrayIndex)`
- `void CopyTo(int index, MigrationEntry[] array, int arrayIndex, int count)`
- `void Dispose()`
- `MigrationEntryVectorEnumerator GetEnumerator()`
- `MigrationEntryVector GetRange(int index, int count)`
- `void Insert(int index, MigrationEntry x)`
- `void InsertRange(int index, MigrationEntryVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static MigrationEntryVector Repeat(MigrationEntry value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, MigrationEntryVector values)`
- `MigrationEntry[] ToArray()`

## pagmo.MigrationEntryVector+MigrationEntryVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `MigrationEntryVectorEnumerator(MigrationEntryVector collection)`

### Properties

- `MigrationEntry Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.MoVectorLogEntry

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `MoVectorLogEntry()`

### Properties

- `ulong fevals { get; set }`
- `DoubleVector fitness { get; set }`
- `uint gen { get; set }`

### Methods

- `void Dispose()`

## pagmo.MoVectorLogEntryVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `MoVectorLogEntryVector()`
- `MoVectorLogEntryVector(int capacity)`
- `MoVectorLogEntryVector(IEnumerable<MoVectorLogEntry> c)`
- `MoVectorLogEntryVector(IEnumerable c)`
- `MoVectorLogEntryVector(MoVectorLogEntryVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `MoVectorLogEntry Item { get; set }`

### Methods

- `void Add(MoVectorLogEntry x)`
- `void AddRange(MoVectorLogEntryVector values)`
- `void Clear()`
- `void CopyTo(MoVectorLogEntry[] array)`
- `void CopyTo(MoVectorLogEntry[] array, int arrayIndex)`
- `void CopyTo(int index, MoVectorLogEntry[] array, int arrayIndex, int count)`
- `void Dispose()`
- `MoVectorLogEntryVectorEnumerator GetEnumerator()`
- `MoVectorLogEntryVector GetRange(int index, int count)`
- `void Insert(int index, MoVectorLogEntry x)`
- `void InsertRange(int index, MoVectorLogEntryVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static MoVectorLogEntryVector Repeat(MoVectorLogEntry value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, MoVectorLogEntryVector values)`
- `MoVectorLogEntry[] ToArray()`

## pagmo.MoVectorLogEntryVector+MoVectorLogEntryVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `MoVectorLogEntryVectorEnumerator(MoVectorLogEntryVector collection)`

### Properties

- `MoVectorLogEntry Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.MoeadLogEntry

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `MoeadLogEntry()`

### Properties

- `double decomposed_f { get; set }`
- `ulong fevals { get; set }`
- `uint gen { get; set }`
- `DoubleVector ideal_point { get; set }`

### Methods

- `void Dispose()`

## pagmo.MoeadLogEntryVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `MoeadLogEntryVector()`
- `MoeadLogEntryVector(int capacity)`
- `MoeadLogEntryVector(IEnumerable<MoeadLogEntry> c)`
- `MoeadLogEntryVector(IEnumerable c)`
- `MoeadLogEntryVector(MoeadLogEntryVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `MoeadLogEntry Item { get; set }`

### Methods

- `void Add(MoeadLogEntry x)`
- `void AddRange(MoeadLogEntryVector values)`
- `void Clear()`
- `void CopyTo(MoeadLogEntry[] array)`
- `void CopyTo(MoeadLogEntry[] array, int arrayIndex)`
- `void CopyTo(int index, MoeadLogEntry[] array, int arrayIndex, int count)`
- `void Dispose()`
- `MoeadLogEntryVectorEnumerator GetEnumerator()`
- `MoeadLogEntryVector GetRange(int index, int count)`
- `void Insert(int index, MoeadLogEntry x)`
- `void InsertRange(int index, MoeadLogEntryVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static MoeadLogEntryVector Repeat(MoeadLogEntry value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, MoeadLogEntryVector values)`
- `MoeadLogEntry[] ToArray()`

## pagmo.MoeadLogEntryVector+MoeadLogEntryVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `MoeadLogEntryVectorEnumerator(MoeadLogEntryVector collection)`

### Properties

- `MoeadLogEntry Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.NloptLogEntry

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `NloptLogEntry()`

### Properties

- `bool feasible { get; set }`
- `ulong fevals { get; set }`
- `double objective { get; set }`
- `ulong violated { get; set }`
- `double violation_norm { get; set }`

### Methods

- `void Dispose()`

## pagmo.NloptLogEntryVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `NloptLogEntryVector()`
- `NloptLogEntryVector(int capacity)`
- `NloptLogEntryVector(IEnumerable<NloptLogEntry> c)`
- `NloptLogEntryVector(IEnumerable c)`
- `NloptLogEntryVector(NloptLogEntryVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `NloptLogEntry Item { get; set }`

### Methods

- `void Add(NloptLogEntry x)`
- `void AddRange(NloptLogEntryVector values)`
- `void Clear()`
- `void CopyTo(NloptLogEntry[] array)`
- `void CopyTo(NloptLogEntry[] array, int arrayIndex)`
- `void CopyTo(int index, NloptLogEntry[] array, int arrayIndex, int count)`
- `void Dispose()`
- `NloptLogEntryVectorEnumerator GetEnumerator()`
- `NloptLogEntryVector GetRange(int index, int count)`
- `void Insert(int index, NloptLogEntry x)`
- `void InsertRange(int index, NloptLogEntryVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static NloptLogEntryVector Repeat(NloptLogEntry value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, NloptLogEntryVector values)`
- `NloptLogEntry[] ToArray()`

## pagmo.NloptLogEntryVector+NloptLogEntryVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `NloptLogEntryVectorEnumerator(NloptLogEntryVector collection)`

### Properties

- `NloptLogEntry Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.PairOfDoubleVectors

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `PairOfDoubleVectors()`
- `PairOfDoubleVectors(DoubleVector first, DoubleVector second)`
- `PairOfDoubleVectors(PairOfDoubleVectors other)`

### Properties

- `DoubleVector first { get; set }`
- `DoubleVector second { get; set }`

### Methods

- `void Dispose()`

## pagmo.ProblemManualFunctions

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Methods

- `static DoubleVector fitness(IProblem problem, double[] values)`
- `static DoubleVector fitness(IProblem problem, IEnumerable<double> values)`

## pagmo.PsoLogEntry

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `PsoLogEntry()`

### Properties

- `double best { get; set }`
- `double cognitive { get; set }`
- `ulong fevals { get; set }`
- `uint gen { get; set }`
- `double inertia { get; set }`
- `double social { get; set }`

### Methods

- `void Dispose()`

## pagmo.PsoLogEntryVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `PsoLogEntryVector()`
- `PsoLogEntryVector(int capacity)`
- `PsoLogEntryVector(IEnumerable<PsoLogEntry> c)`
- `PsoLogEntryVector(IEnumerable c)`
- `PsoLogEntryVector(PsoLogEntryVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `PsoLogEntry Item { get; set }`

### Methods

- `void Add(PsoLogEntry x)`
- `void AddRange(PsoLogEntryVector values)`
- `void Clear()`
- `void CopyTo(PsoLogEntry[] array)`
- `void CopyTo(PsoLogEntry[] array, int arrayIndex)`
- `void CopyTo(int index, PsoLogEntry[] array, int arrayIndex, int count)`
- `void Dispose()`
- `PsoLogEntryVectorEnumerator GetEnumerator()`
- `PsoLogEntryVector GetRange(int index, int count)`
- `void Insert(int index, PsoLogEntry x)`
- `void InsertRange(int index, PsoLogEntryVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static PsoLogEntryVector Repeat(PsoLogEntry value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, PsoLogEntryVector values)`
- `PsoLogEntry[] ToArray()`

## pagmo.PsoLogEntryVector+PsoLogEntryVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `PsoLogEntryVectorEnumerator(PsoLogEntryVector collection)`

### Properties

- `PsoLogEntry Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.RekSum

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `RekSum()`

### Methods

- `void Dispose()`
- `static void reksum(VectorOfVectorOfDoubles out_, ULongLongVector in_, ulong m, ulong s)`

## pagmo.SWIGTYPE_p_nlopt_result

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

## pagmo.SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

## pagmo.SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

## pagmo.SadeLogEntry

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SadeLogEntry()`

### Properties

- `double best { get; set }`
- `double cr { get; set }`
- `double df { get; set }`
- `double dx { get; set }`
- `double f { get; set }`
- `ulong fevals { get; set }`
- `uint gen { get; set }`

### Methods

- `void Dispose()`

## pagmo.SadeLogEntryVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SadeLogEntryVector()`
- `SadeLogEntryVector(int capacity)`
- `SadeLogEntryVector(IEnumerable<SadeLogEntry> c)`
- `SadeLogEntryVector(IEnumerable c)`
- `SadeLogEntryVector(SadeLogEntryVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `SadeLogEntry Item { get; set }`

### Methods

- `void Add(SadeLogEntry x)`
- `void AddRange(SadeLogEntryVector values)`
- `void Clear()`
- `void CopyTo(SadeLogEntry[] array)`
- `void CopyTo(SadeLogEntry[] array, int arrayIndex)`
- `void CopyTo(int index, SadeLogEntry[] array, int arrayIndex, int count)`
- `void Dispose()`
- `SadeLogEntryVectorEnumerator GetEnumerator()`
- `SadeLogEntryVector GetRange(int index, int count)`
- `void Insert(int index, SadeLogEntry x)`
- `void InsertRange(int index, SadeLogEntryVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static SadeLogEntryVector Repeat(SadeLogEntry value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, SadeLogEntryVector values)`
- `SadeLogEntry[] ToArray()`

## pagmo.SadeLogEntryVector+SadeLogEntryVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SadeLogEntryVectorEnumerator(SadeLogEntryVector collection)`

### Properties

- `SadeLogEntry Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.SeaLogEntry

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SeaLogEntry()`

### Properties

- `double best { get; set }`
- `ulong fevals { get; set }`
- `uint gen { get; set }`
- `double improvement { get; set }`
- `ulong offspring_evals { get; set }`

### Methods

- `void Dispose()`

## pagmo.SeaLogEntryVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SeaLogEntryVector()`
- `SeaLogEntryVector(int capacity)`
- `SeaLogEntryVector(IEnumerable<SeaLogEntry> c)`
- `SeaLogEntryVector(IEnumerable c)`
- `SeaLogEntryVector(SeaLogEntryVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `SeaLogEntry Item { get; set }`

### Methods

- `void Add(SeaLogEntry x)`
- `void AddRange(SeaLogEntryVector values)`
- `void Clear()`
- `void CopyTo(SeaLogEntry[] array)`
- `void CopyTo(SeaLogEntry[] array, int arrayIndex)`
- `void CopyTo(int index, SeaLogEntry[] array, int arrayIndex, int count)`
- `void Dispose()`
- `SeaLogEntryVectorEnumerator GetEnumerator()`
- `SeaLogEntryVector GetRange(int index, int count)`
- `void Insert(int index, SeaLogEntry x)`
- `void InsertRange(int index, SeaLogEntryVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static SeaLogEntryVector Repeat(SeaLogEntry value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, SeaLogEntryVector values)`
- `SeaLogEntry[] ToArray()`

## pagmo.SeaLogEntryVector+SeaLogEntryVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SeaLogEntryVectorEnumerator(SeaLogEntryVector collection)`

### Properties

- `SeaLogEntry Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.SgaLogEntry

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SgaLogEntry()`

### Properties

- `double best { get; set }`
- `ulong fevals { get; set }`
- `uint gen { get; set }`
- `double improvement { get; set }`

### Methods

- `void Dispose()`

## pagmo.SgaLogEntryVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SgaLogEntryVector()`
- `SgaLogEntryVector(int capacity)`
- `SgaLogEntryVector(IEnumerable<SgaLogEntry> c)`
- `SgaLogEntryVector(IEnumerable c)`
- `SgaLogEntryVector(SgaLogEntryVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `SgaLogEntry Item { get; set }`

### Methods

- `void Add(SgaLogEntry x)`
- `void AddRange(SgaLogEntryVector values)`
- `void Clear()`
- `void CopyTo(SgaLogEntry[] array)`
- `void CopyTo(SgaLogEntry[] array, int arrayIndex)`
- `void CopyTo(int index, SgaLogEntry[] array, int arrayIndex, int count)`
- `void Dispose()`
- `SgaLogEntryVectorEnumerator GetEnumerator()`
- `SgaLogEntryVector GetRange(int index, int count)`
- `void Insert(int index, SgaLogEntry x)`
- `void InsertRange(int index, SgaLogEntryVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static SgaLogEntryVector Repeat(SgaLogEntry value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, SgaLogEntryVector values)`
- `SgaLogEntry[] ToArray()`

## pagmo.SgaLogEntryVector+SgaLogEntryVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SgaLogEntryVectorEnumerator(SgaLogEntryVector collection)`

### Properties

- `SgaLogEntry Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.SimulatedAnnealingLogEntry

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SimulatedAnnealingLogEntry()`

### Properties

- `double best { get; set }`
- `double current { get; set }`
- `ulong fevals { get; set }`
- `double move_range { get; set }`
- `double temperature { get; set }`

### Methods

- `void Dispose()`

## pagmo.SimulatedAnnealingLogEntryVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SimulatedAnnealingLogEntryVector()`
- `SimulatedAnnealingLogEntryVector(int capacity)`
- `SimulatedAnnealingLogEntryVector(IEnumerable<SimulatedAnnealingLogEntry> c)`
- `SimulatedAnnealingLogEntryVector(IEnumerable c)`
- `SimulatedAnnealingLogEntryVector(SimulatedAnnealingLogEntryVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `SimulatedAnnealingLogEntry Item { get; set }`

### Methods

- `void Add(SimulatedAnnealingLogEntry x)`
- `void AddRange(SimulatedAnnealingLogEntryVector values)`
- `void Clear()`
- `void CopyTo(SimulatedAnnealingLogEntry[] array)`
- `void CopyTo(SimulatedAnnealingLogEntry[] array, int arrayIndex)`
- `void CopyTo(int index, SimulatedAnnealingLogEntry[] array, int arrayIndex, int count)`
- `void Dispose()`
- `SimulatedAnnealingLogEntryVectorEnumerator GetEnumerator()`
- `SimulatedAnnealingLogEntryVector GetRange(int index, int count)`
- `void Insert(int index, SimulatedAnnealingLogEntry x)`
- `void InsertRange(int index, SimulatedAnnealingLogEntryVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static SimulatedAnnealingLogEntryVector Repeat(SimulatedAnnealingLogEntry value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, SimulatedAnnealingLogEntryVector values)`
- `SimulatedAnnealingLogEntry[] ToArray()`

## pagmo.SimulatedAnnealingLogEntryVector+SimulatedAnnealingLogEntryVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SimulatedAnnealingLogEntryVectorEnumerator(SimulatedAnnealingLogEntryVector collection)`

### Properties

- `SimulatedAnnealingLogEntry Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.SizeTPair

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SizeTPair()`
- `SizeTPair(uint first, uint second)`
- `SizeTPair(SizeTPair other)`

### Properties

- `uint first { get; set }`
- `uint second { get; set }`

### Methods

- `void Dispose()`

## pagmo.SizeTVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SizeTVector()`
- `SizeTVector(int capacity)`
- `SizeTVector(IEnumerable<uint> c)`
- `SizeTVector(IEnumerable c)`
- `SizeTVector(SizeTVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `uint Item { get; set }`

### Methods

- `void Add(uint x)`
- `void AddRange(SizeTVector values)`
- `void Clear()`
- `void CopyTo(uint[] array)`
- `void CopyTo(uint[] array, int arrayIndex)`
- `void CopyTo(int index, uint[] array, int arrayIndex, int count)`
- `void Dispose()`
- `SizeTVectorEnumerator GetEnumerator()`
- `SizeTVector GetRange(int index, int count)`
- `void Insert(int index, uint x)`
- `void InsertRange(int index, SizeTVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static SizeTVector Repeat(uint value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, SizeTVector values)`
- `uint[] ToArray()`

## pagmo.SizeTVector+SizeTVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SizeTVectorEnumerator(SizeTVector collection)`

### Properties

- `uint Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.SparsityIndex

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SparsityIndex(uint Row, uint Column)`

### Properties

- `uint Column { get; set }`
- `uint Row { get; set }`

### Methods

- `void Deconstruct(out uint Row, out uint Column)`
- `bool Equals(object obj)`
- `bool Equals(SparsityIndex other)`
- `int GetHashCode()`
- `string ToString()`

## pagmo.SparsityPattern

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SparsityPattern()`
- `SparsityPattern(int capacity)`
- `SparsityPattern(IEnumerable<SizeTPair> c)`
- `SparsityPattern(IEnumerable c)`
- `SparsityPattern(SparsityPattern other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `SizeTPair Item { get; set }`

### Methods

- `void Add(SizeTPair x)`
- `void AddRange(SparsityPattern values)`
- `void Clear()`
- `void CopyTo(SizeTPair[] array)`
- `void CopyTo(SizeTPair[] array, int arrayIndex)`
- `void CopyTo(int index, SizeTPair[] array, int arrayIndex, int count)`
- `void Dispose()`
- `SparsityPatternEnumerator GetEnumerator()`
- `SparsityPattern GetRange(int index, int count)`
- `void Insert(int index, SizeTPair x)`
- `void InsertRange(int index, SparsityPattern values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static SparsityPattern Repeat(SizeTPair value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, SparsityPattern values)`
- `SizeTPair[] ToArray()`

## pagmo.SparsityPattern+SparsityPatternEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SparsityPatternEnumerator(SparsityPattern collection)`

### Properties

- `SizeTPair Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.TopologyConnectionData

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `TopologyConnectionData(uint[] NeighborIds, double[] Weights)`

### Properties

- `uint[] NeighborIds { get; set }`
- `double[] Weights { get; set }`

### Methods

- `void Deconstruct(out uint[] NeighborIds, out double[] Weights)`
- `bool Equals(object obj)`
- `bool Equals(TopologyConnectionData other)`
- `int GetHashCode()`
- `string ToString()`

## pagmo.TopologyConnectionExtensions

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Methods

- `static TopologyConnectionData GetConnectionsData(topology topologyInstance, uint vertexId)`
- `static TopologyConnectionData GetConnectionsData(fully_connected topologyInstance, uint vertexId)`
- `static TopologyConnectionData GetConnectionsData(ring topologyInstance, uint vertexId)`
- `static TopologyConnectionData GetConnectionsData(unconnected topologyInstance, uint vertexId)`

## pagmo.TopologyConnections

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `TopologyConnections()`
- `TopologyConnections(SizeTVector first, DoubleVector second)`
- `TopologyConnections(TopologyConnections other)`

### Properties

- `SizeTVector first { get; set }`
- `DoubleVector second { get; set }`

### Methods

- `void Dispose()`

## pagmo.UIntVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `UIntVector()`
- `UIntVector(int capacity)`
- `UIntVector(IEnumerable<uint> c)`
- `UIntVector(IEnumerable c)`
- `UIntVector(UIntVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `uint Item { get; set }`

### Methods

- `void Add(uint x)`
- `void AddRange(UIntVector values)`
- `void Clear()`
- `bool Contains(uint value)`
- `void CopyTo(uint[] array)`
- `void CopyTo(uint[] array, int arrayIndex)`
- `void CopyTo(int index, uint[] array, int arrayIndex, int count)`
- `void Dispose()`
- `UIntVectorEnumerator GetEnumerator()`
- `UIntVector GetRange(int index, int count)`
- `int IndexOf(uint value)`
- `void Insert(int index, uint x)`
- `void InsertRange(int index, UIntVector values)`
- `int LastIndexOf(uint value)`
- `bool Remove(uint value)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static UIntVector Repeat(uint value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, UIntVector values)`
- `uint[] ToArray()`

## pagmo.UIntVector+UIntVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `UIntVectorEnumerator(UIntVector collection)`

### Properties

- `uint Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.ULongLongVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `ULongLongVector()`
- `ULongLongVector(int capacity)`
- `ULongLongVector(IEnumerable<ulong> c)`
- `ULongLongVector(IEnumerable c)`
- `ULongLongVector(ULongLongVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `ulong Item { get; set }`

### Methods

- `void Add(ulong x)`
- `void AddRange(ULongLongVector values)`
- `void Clear()`
- `bool Contains(ulong value)`
- `void CopyTo(ulong[] array)`
- `void CopyTo(ulong[] array, int arrayIndex)`
- `void CopyTo(int index, ulong[] array, int arrayIndex, int count)`
- `void Dispose()`
- `ULongLongVectorEnumerator GetEnumerator()`
- `ULongLongVector GetRange(int index, int count)`
- `int IndexOf(ulong value)`
- `void Insert(int index, ulong x)`
- `void InsertRange(int index, ULongLongVector values)`
- `int LastIndexOf(ulong value)`
- `bool Remove(ulong value)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static ULongLongVector Repeat(ulong value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, ULongLongVector values)`
- `ulong[] ToArray()`

## pagmo.ULongLongVector+ULongLongVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `ULongLongVectorEnumerator(ULongLongVector collection)`

### Properties

- `ulong Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.VectorOfSparsityPattern

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `VectorOfSparsityPattern()`
- `VectorOfSparsityPattern(int capacity)`
- `VectorOfSparsityPattern(IEnumerable<SparsityPattern> c)`
- `VectorOfSparsityPattern(IEnumerable c)`
- `VectorOfSparsityPattern(VectorOfSparsityPattern other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `SparsityPattern Item { get; set }`

### Methods

- `void Add(SparsityPattern x)`
- `void AddRange(VectorOfSparsityPattern values)`
- `void Clear()`
- `void CopyTo(SparsityPattern[] array)`
- `void CopyTo(SparsityPattern[] array, int arrayIndex)`
- `void CopyTo(int index, SparsityPattern[] array, int arrayIndex, int count)`
- `void Dispose()`
- `VectorOfSparsityPatternEnumerator GetEnumerator()`
- `VectorOfSparsityPattern GetRange(int index, int count)`
- `void Insert(int index, SparsityPattern x)`
- `void InsertRange(int index, VectorOfSparsityPattern values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static VectorOfSparsityPattern Repeat(SparsityPattern value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, VectorOfSparsityPattern values)`
- `SparsityPattern[] ToArray()`

## pagmo.VectorOfSparsityPattern+VectorOfSparsityPatternEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `VectorOfSparsityPatternEnumerator(VectorOfSparsityPattern collection)`

### Properties

- `SparsityPattern Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.VectorOfVectorIndexes

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `VectorOfVectorIndexes()`
- `VectorOfVectorIndexes(int capacity)`
- `VectorOfVectorIndexes(IEnumerable<ULongLongVector> c)`
- `VectorOfVectorIndexes(IEnumerable c)`
- `VectorOfVectorIndexes(VectorOfVectorIndexes other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `ULongLongVector Item { get; set }`

### Methods

- `void Add(ULongLongVector x)`
- `void AddRange(VectorOfVectorIndexes values)`
- `void Clear()`
- `void CopyTo(ULongLongVector[] array)`
- `void CopyTo(ULongLongVector[] array, int arrayIndex)`
- `void CopyTo(int index, ULongLongVector[] array, int arrayIndex, int count)`
- `void Dispose()`
- `VectorOfVectorIndexesEnumerator GetEnumerator()`
- `VectorOfVectorIndexes GetRange(int index, int count)`
- `void Insert(int index, ULongLongVector x)`
- `void InsertRange(int index, VectorOfVectorIndexes values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static VectorOfVectorIndexes Repeat(ULongLongVector value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, VectorOfVectorIndexes values)`
- `ULongLongVector[] ToArray()`

## pagmo.VectorOfVectorIndexes+VectorOfVectorIndexesEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `VectorOfVectorIndexesEnumerator(VectorOfVectorIndexes collection)`

### Properties

- `ULongLongVector Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.VectorOfVectorOfDoubles

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `VectorOfVectorOfDoubles()`
- `VectorOfVectorOfDoubles(int capacity)`
- `VectorOfVectorOfDoubles(IEnumerable<DoubleVector> c)`
- `VectorOfVectorOfDoubles(IEnumerable c)`
- `VectorOfVectorOfDoubles(VectorOfVectorOfDoubles other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `DoubleVector Item { get; set }`

### Methods

- `void Add(DoubleVector x)`
- `void AddRange(VectorOfVectorOfDoubles values)`
- `void Clear()`
- `void CopyTo(DoubleVector[] array)`
- `void CopyTo(DoubleVector[] array, int arrayIndex)`
- `void CopyTo(int index, DoubleVector[] array, int arrayIndex, int count)`
- `void Dispose()`
- `VectorOfVectorOfDoublesEnumerator GetEnumerator()`
- `VectorOfVectorOfDoubles GetRange(int index, int count)`
- `void Insert(int index, DoubleVector x)`
- `void InsertRange(int index, VectorOfVectorOfDoubles values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static VectorOfVectorOfDoubles Repeat(DoubleVector value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, VectorOfVectorOfDoubles values)`
- `DoubleVector[] ToArray()`

## pagmo.VectorOfVectorOfDoubles+VectorOfVectorOfDoublesEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `VectorOfVectorOfDoublesEnumerator(VectorOfVectorOfDoubles collection)`

### Properties

- `DoubleVector Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.XnesLogEntry

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `XnesLogEntry()`

### Properties

- `double best { get; set }`
- `double df { get; set }`
- `double dx { get; set }`
- `ulong fevals { get; set }`
- `uint gen { get; set }`
- `double sigma { get; set }`

### Methods

- `void Dispose()`

## pagmo.XnesLogEntryVector

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `XnesLogEntryVector()`
- `XnesLogEntryVector(int capacity)`
- `XnesLogEntryVector(IEnumerable<XnesLogEntry> c)`
- `XnesLogEntryVector(IEnumerable c)`
- `XnesLogEntryVector(XnesLogEntryVector other)`

### Properties

- `int Capacity { get; set }`
- `int Count { get }`
- `bool IsEmpty { get }`
- `bool IsFixedSize { get }`
- `bool IsReadOnly { get }`
- `bool IsSynchronized { get }`
- `XnesLogEntry Item { get; set }`

### Methods

- `void Add(XnesLogEntry x)`
- `void AddRange(XnesLogEntryVector values)`
- `void Clear()`
- `void CopyTo(XnesLogEntry[] array)`
- `void CopyTo(XnesLogEntry[] array, int arrayIndex)`
- `void CopyTo(int index, XnesLogEntry[] array, int arrayIndex, int count)`
- `void Dispose()`
- `XnesLogEntryVectorEnumerator GetEnumerator()`
- `XnesLogEntryVector GetRange(int index, int count)`
- `void Insert(int index, XnesLogEntry x)`
- `void InsertRange(int index, XnesLogEntryVector values)`
- `void RemoveAt(int index)`
- `void RemoveRange(int index, int count)`
- `static XnesLogEntryVector Repeat(XnesLogEntry value, int count)`
- `void Reverse()`
- `void Reverse(int index, int count)`
- `void SetRange(int index, XnesLogEntryVector values)`
- `XnesLogEntry[] ToArray()`

## pagmo.XnesLogEntryVector+XnesLogEntryVectorEnumerator

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `XnesLogEntryVectorEnumerator(XnesLogEntryVector collection)`

### Properties

- `XnesLogEntry Current { get }`

### Methods

- `void Dispose()`
- `bool MoveNext()`
- `void Reset()`

## pagmo.ackley

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/ackley.html

### Constructors

- `ackley()`
- `ackley(uint dim)`

### Properties

- `uint m_dim { get; set }`

### Methods

- `void Dispose()`
- `DoubleVector best_known()`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `bool has_batch_fitness()`
- `problem to_problem()`

## pagmo.algorithm

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithm.html

### Constructors

- `algorithm(IAlgorithm managedAlgorithm)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `string get_name()`
- `int get_thread_safety()`
- `bool has_set_seed()`
- `bool has_set_verbosity()`
- `bool is_stochastic()`
- `bool is_valid()`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint arg0)`

## pagmo.algorithm_callback

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `algorithm_callback()`

### Methods

- `void Dispose()`
- `string consume_deferred_exception()`
- `population evolve(population pop)`
- `string get_extra_info()`
- `string get_name()`
- `int get_thread_safety()`
- `bool has_set_seed()`
- `bool has_set_verbosity()`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint arg0)`

## pagmo.algorithm_callback+SwigDelegatealgorithm_callback_0

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegatealgorithm_callback_0(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(IntPtr pop, AsyncCallback callback, object object)`
- `IntPtr EndInvoke(IAsyncResult result)`
- `IntPtr Invoke(IntPtr pop)`

## pagmo.algorithm_callback+SwigDelegatealgorithm_callback_1

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegatealgorithm_callback_1(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(uint arg0, AsyncCallback callback, object object)`
- `void EndInvoke(IAsyncResult result)`
- `void Invoke(uint arg0)`

## pagmo.algorithm_callback+SwigDelegatealgorithm_callback_2

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegatealgorithm_callback_2(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `bool EndInvoke(IAsyncResult result)`
- `bool Invoke()`

## pagmo.algorithm_callback+SwigDelegatealgorithm_callback_3

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegatealgorithm_callback_3(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(uint arg0, AsyncCallback callback, object object)`
- `void EndInvoke(IAsyncResult result)`
- `void Invoke(uint arg0)`

## pagmo.algorithm_callback+SwigDelegatealgorithm_callback_4

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegatealgorithm_callback_4(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `bool EndInvoke(IAsyncResult result)`
- `bool Invoke()`

## pagmo.algorithm_callback+SwigDelegatealgorithm_callback_5

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegatealgorithm_callback_5(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `string EndInvoke(IAsyncResult result)`
- `string Invoke()`

## pagmo.algorithm_callback+SwigDelegatealgorithm_callback_6

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegatealgorithm_callback_6(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `string EndInvoke(IAsyncResult result)`
- `string Invoke()`

## pagmo.algorithm_callback+SwigDelegatealgorithm_callback_7

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegatealgorithm_callback_7(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `int EndInvoke(IAsyncResult result)`
- `int Invoke()`

## pagmo.algorithm_callback+SwigDelegatealgorithm_callback_8

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegatealgorithm_callback_8(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `string EndInvoke(IAsyncResult result)`
- `string Invoke()`

## pagmo.archipelago

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/archipelago.html

### Constructors

- `archipelago()`
- `archipelago(archipelago arg0)`

### Properties

- `IReadOnlyList<IndividualsGroup> MigrantsDb { get }`
- `IReadOnlyList<MigrationEntry> MigrationLog { get }`

### Methods

- `void Dispose()`
- `island GetIsland(ulong index)`
- `island GetIslandCopy(ulong index)`
- `ulong PushBackIsland(IAlgorithm algorithm, IProblem problem, ulong populationSize, uint seed, thread_island islandType)`
- `ulong PushBackIsland(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, uint seed, thread_island islandType)`
- `ulong PushBackIsland(IAlgorithm algorithm, IProblem problem, ulong populationSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed, thread_island islandType)`
- `ulong PushBackIsland(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed, thread_island islandType)`
- `void evolve()`
- `void evolve(uint n)`
- `island get_island_copy(uint idx)`
- `int get_migrant_handling()`
- `IndividualsGroupVector get_migrants_db()`
- `MigrationEntryVector get_migration_log_entries()`
- `int get_migration_type()`
- `string get_topology_name()`
- `uint push_back_island(algorithm algo, problem prob, uint pop_size, uint seed)`
- `ulong push_back_island(algorithm algorithm, IProblem problem, ulong populationSize, uint seed, thread_island islandType)`
- `ulong push_back_island(IAlgorithm algorithm, IProblem problem, ulong populationSize, uint seed, thread_island islandType)`
- `uint push_back_island(algorithm algo, problem prob, bfe b, uint pop_size, uint seed)`
- `uint push_back_island(thread_island isl, algorithm algo, problem prob, uint pop_size, uint seed)`
- `ulong push_back_island(algorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, uint seed, thread_island islandType)`
- `ulong push_back_island(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, uint seed, thread_island islandType)`
- `uint push_back_island(algorithm algo, problem prob, uint pop_size, fair_replace r, select_best s, uint seed)`
- `uint push_back_island(algorithm algo, problem prob, uint pop_size, r_policyPagmoWrapper r, s_policyPagmoWrapper s, uint seed)`
- `uint push_back_island(thread_island isl, algorithm algo, problem prob, bfe b, uint pop_size, uint seed)`
- `ulong push_back_island(algorithm algorithm, IProblem problem, ulong populationSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed, thread_island islandType)`
- `ulong push_back_island(algorithm algorithm, IProblem problem, ulong populationSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed, thread_island islandType)`
- `ulong push_back_island(IAlgorithm algorithm, IProblem problem, ulong populationSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed, thread_island islandType)`
- `ulong push_back_island(IAlgorithm algorithm, IProblem problem, ulong populationSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed, thread_island islandType)`
- `uint push_back_island(algorithm algo, problem prob, bfe b, uint pop_size, fair_replace r, select_best s, uint seed)`
- `uint push_back_island(algorithm algo, problem prob, bfe b, uint pop_size, r_policyPagmoWrapper r, s_policyPagmoWrapper s, uint seed)`
- `uint push_back_island(thread_island isl, algorithm algo, problem prob, uint pop_size, fair_replace r, select_best s, uint seed)`
- `uint push_back_island(thread_island isl, algorithm algo, problem prob, uint pop_size, r_policyPagmoWrapper r, s_policyPagmoWrapper s, uint seed)`
- `ulong push_back_island(algorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed, thread_island islandType)`
- `ulong push_back_island(algorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed, thread_island islandType)`
- `ulong push_back_island(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed, thread_island islandType)`
- `ulong push_back_island(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed, thread_island islandType)`
- `uint push_back_island(thread_island isl, algorithm algo, problem prob, bfe b, uint pop_size, fair_replace r, select_best s, uint seed)`
- `uint push_back_island(thread_island isl, algorithm algo, problem prob, bfe b, uint pop_size, r_policyPagmoWrapper r, s_policyPagmoWrapper s, uint seed)`
- `void set_migrant_handling(int m)`
- `void set_migrants_db_items(IndividualsGroupVector v)`
- `void set_migration_type(int t)`
- `void set_topology_free_form(free_form t)`
- `void set_topology_fully_connected(fully_connected t)`
- `void set_topology_ring(ring t)`
- `void set_topology_unconnected(unconnected t)`
- `uint size()`
- `int status()`
- `void wait()`
- `void wait_check()`

## pagmo.bee_colony

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/bee_colony.html

### Constructors

- `bee_colony()`
- `bee_colony(uint gen)`
- `bee_colony(uint gen, uint limit)`
- `bee_colony(uint gen, uint limit, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<BeeColonyLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `uint get_gen()`
- `BeeColonyLogLineVector get_log_lines()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint level)`
- `algorithm to_algorithm()`

## pagmo.bee_colony+BeeColonyLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `BeeColonyLogLine(uint Generation, ulong FunctionEvaluations, double BestFitness, double CurrentBestFitness)`

### Properties

- `string AlgorithmName { get }`
- `double BestFitness { get; set }`
- `double CurrentBestFitness { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `uint Generation { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`

### Methods

- `void Deconstruct(out uint Generation, out ulong FunctionEvaluations, out double BestFitness, out double CurrentBestFitness)`
- `bool Equals(object obj)`
- `bool Equals(BeeColonyLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.bfe

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/bfe.html

### Methods

- `void Dispose()`

## pagmo.cec2006

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/cec2006.html

### Constructors

- `cec2006()`
- `cec2006(uint prob_id)`

### Methods

- `void Dispose()`
- `DoubleVector best_known()`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `bool has_batch_fitness()`
- `problem to_problem()`

## pagmo.cec2009

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/cec2009.html

### Constructors

- `cec2009()`
- `cec2009(uint prob_id)`
- `cec2009(uint prob_id, bool is_constrained)`
- `cec2009(uint prob_id, bool is_constrained, uint dim)`

### Methods

- `void Dispose()`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `bool has_batch_fitness()`
- `problem to_problem()`

## pagmo.cec2013

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/cec2013.html

### Constructors

- `cec2013()`
- `cec2013(uint prob_id)`
- `cec2013(uint prob_id, uint dim)`

### Methods

- `void Dispose()`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `bool has_batch_fitness()`
- `problem to_problem()`

## pagmo.cec2014

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/cec2014.html

### Constructors

- `cec2014()`
- `cec2014(uint prob_id)`
- `cec2014(uint prob_id, uint dim)`

### Methods

- `void Dispose()`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `bool has_batch_fitness()`
- `problem to_problem()`

## pagmo.cmaes

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/cmaes.html

### Constructors

- `cmaes()`
- `cmaes(uint gen)`
- `cmaes(uint gen, double cc)`
- `cmaes(uint gen, double cc, double cs)`
- `cmaes(uint gen, double cc, double cs, double c1)`
- `cmaes(uint gen, double cc, double cs, double c1, double cmu)`
- `cmaes(uint gen, double cc, double cs, double c1, double cmu, double sigma0)`
- `cmaes(uint gen, double cc, double cs, double c1, double cmu, double sigma0, double ftol)`
- `cmaes(uint gen, double cc, double cs, double c1, double cmu, double sigma0, double ftol, double xtol)`
- `cmaes(uint gen, double cc, double cs, double c1, double cmu, double sigma0, double ftol, double xtol, bool memory)`
- `cmaes(uint gen, double cc, double cs, double c1, double cmu, double sigma0, double ftol, double xtol, bool memory, bool force_bounds)`
- `cmaes(uint gen, double cc, double cs, double c1, double cmu, double sigma0, double ftol, double xtol, bool memory, bool force_bounds, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<CmaesLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `uint get_gen()`
- `CmaesLogEntryVector get_log_entries()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_bfe(bfe b)`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint level)`
- `algorithm to_algorithm()`

## pagmo.cmaes+CmaesLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `CmaesLogLine(uint Generation, ulong FunctionEvaluations, double BestFitness, double Sigma, double MinVariance, double MaxVariance)`

### Properties

- `string AlgorithmName { get }`
- `double BestFitness { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `uint Generation { get; set }`
- `double MaxVariance { get; set }`
- `double MinVariance { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`
- `double Sigma { get; set }`

### Methods

- `void Deconstruct(out uint Generation, out ulong FunctionEvaluations, out double BestFitness, out double Sigma, out double MinVariance, out double MaxVariance)`
- `bool Equals(object obj)`
- `bool Equals(CmaesLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.compass_search

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/compass_search.html

### Constructors

- `compass_search()`
- `compass_search(uint max_fevals)`
- `compass_search(uint max_fevals, double start_range)`
- `compass_search(uint max_fevals, double start_range, double stop_range)`
- `compass_search(uint max_fevals, double start_range, double stop_range, double reduction_coeff)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<CompassSearchLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `uint get_gen()`
- `CompassSearchLogEntryVector get_log_entries()`
- `double get_max_fevals()`
- `string get_name()`
- `double get_reduction_coeff()`
- `uint get_seed()`
- `double get_start_range()`
- `double get_stop_range()`
- `uint get_verbosity()`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint level)`
- `algorithm to_algorithm()`

## pagmo.compass_search+CompassSearchLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `CompassSearchLogLine(ulong FunctionEvaluations, double BestFitness, ulong ViolatedConstraints, double ViolationNorm, double Range)`

### Properties

- `string AlgorithmName { get }`
- `double BestFitness { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `double Range { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`
- `ulong ViolatedConstraints { get; set }`
- `double ViolationNorm { get; set }`

### Methods

- `void Deconstruct(out ulong FunctionEvaluations, out double BestFitness, out ulong ViolatedConstraints, out double ViolationNorm, out double Range)`
- `bool Equals(object obj)`
- `bool Equals(CompassSearchLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.cstrs_self_adaptive

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/cstrs_self_adaptive.html

### Constructors

- `cstrs_self_adaptive()`
- `cstrs_self_adaptive(uint iters)`
- `cstrs_self_adaptive(uint iters, algorithm a)`
- `cstrs_self_adaptive(uint iters, algorithm a, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<CstrsLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `algorithm get_inner_algorithm()`
- `CstrsLogEntryVector get_log_entries()`
- `string get_name()`
- `uint get_seed()`
- `int get_thread_safety()`
- `uint get_verbosity()`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint level)`
- `algorithm to_algorithm()`

## pagmo.cstrs_self_adaptive+CstrsLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `CstrsLogLine(uint Iteration, ulong FunctionEvaluations, double BestFitness, double Infeasibility, ulong ViolatedConstraints, double ViolationNorm, ulong FeasibleCount)`

### Properties

- `string AlgorithmName { get }`
- `double BestFitness { get; set }`
- `ulong FeasibleCount { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `double Infeasibility { get; set }`
- `uint Iteration { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`
- `ulong ViolatedConstraints { get; set }`
- `double ViolationNorm { get; set }`

### Methods

- `void Deconstruct(out uint Iteration, out ulong FunctionEvaluations, out double BestFitness, out double Infeasibility, out ulong ViolatedConstraints, out double ViolationNorm, out ulong FeasibleCount)`
- `bool Equals(object obj)`
- `bool Equals(CstrsLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.de

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/de.html

### Constructors

- `de()`
- `de(uint gen)`
- `de(uint gen, double F)`
- `de(uint gen, double F, double CR)`
- `de(uint gen, double F, double CR, uint variant)`
- `de(uint gen, double F, double CR, uint variant, double ftol)`
- `de(uint gen, double F, double CR, uint variant, double ftol, double xtol)`
- `de(uint gen, double F, double CR, uint variant, double ftol, double xtol, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<DeLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `uint get_gen()`
- `DeLogEntryVector get_log_entries()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint level)`
- `algorithm to_algorithm()`

## pagmo.de+DeLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `DeLogLine(uint Generation, ulong FunctionEvaluations, double BestFitness, double FunctionEvaluationDifference, double Dx)`

### Properties

- `string AlgorithmName { get }`
- `double BestFitness { get; set }`
- `double Dx { get; set }`
- `double FunctionEvaluationDifference { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `uint Generation { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`

### Methods

- `void Deconstruct(out uint Generation, out ulong FunctionEvaluations, out double BestFitness, out double FunctionEvaluationDifference, out double Dx)`
- `bool Equals(object obj)`
- `bool Equals(DeLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.de1220

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/de1220.html

### Constructors

- `de1220()`
- `de1220(uint gen)`
- `de1220(uint gen, UIntVector allowed_variants)`
- `de1220(uint gen, UIntVector allowed_variants, uint variant_adptv)`
- `de1220(uint gen, UIntVector allowed_variants, uint variant_adptv, double ftol)`
- `de1220(uint gen, UIntVector allowed_variants, uint variant_adptv, double ftol, double xtol)`
- `de1220(uint gen, UIntVector allowed_variants, uint variant_adptv, double ftol, double xtol, bool memory)`
- `de1220(uint gen, UIntVector allowed_variants, uint variant_adptv, double ftol, double xtol, bool memory, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<De1220LogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `uint get_gen()`
- `De1220LogEntryVector get_log_entries()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint arg0)`
- `algorithm to_algorithm()`

## pagmo.de1220+De1220LogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `De1220LogLine(uint Generation, ulong FunctionEvaluations, double BestFitness, double FunctionEvaluationDifference, double Dx, uint Variant, double F, double Cr)`

### Properties

- `string AlgorithmName { get }`
- `double BestFitness { get; set }`
- `double Cr { get; set }`
- `double Dx { get; set }`
- `double F { get; set }`
- `double FunctionEvaluationDifference { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `uint Generation { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`
- `uint Variant { get; set }`

### Methods

- `void Deconstruct(out uint Generation, out ulong FunctionEvaluations, out double BestFitness, out double FunctionEvaluationDifference, out double Dx, out uint Variant, out double F, out double Cr)`
- `bool Equals(object obj)`
- `bool Equals(De1220LogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.decompose

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/decompose.html

### Constructors

- `decompose()`
- `decompose(problem arg0, DoubleVector arg1, DoubleVector arg2)`
- `decompose(problem arg0, DoubleVector arg1, DoubleVector arg2, string method)`
- `decompose(problem arg0, DoubleVector arg1, DoubleVector arg2, string method, bool adapt_ideal)`

### Methods

- `static decompose Create(IProblem innerProblem, DoubleVector weight, DoubleVector z, string method, bool adaptIdeal)`
- `static decompose Create(IProblem innerProblem, double[] weight, double[] z, string method, bool adaptIdeal)`
- `void Dispose()`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_extra_info()`
- `problem get_inner_problem()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `DoubleVector get_z()`
- `bool has_batch_fitness()`
- `bool has_set_seed()`
- `DoubleVector original_fitness(DoubleVector arg0)`
- `void set_seed(uint arg0)`
- `problem to_problem()`

## pagmo.default_bfe

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/batch_evaluators/default_bfe.html

### Constructors

- `default_bfe()`
- `default_bfe(default_bfe arg0)`

### Methods

- `void Dispose()`
- `DoubleVector Operator(IProblem problem, DoubleVector batchX)`
- `string get_name()`
- `bfe to_bfe()`

## pagmo.dtlz

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/dtlz.html

### Constructors

- `dtlz()`
- `dtlz(uint prob_id)`
- `dtlz(uint prob_id, uint dim)`
- `dtlz(uint prob_id, uint dim, uint fdim)`
- `dtlz(uint prob_id, uint dim, uint fdim, uint alpha)`

### Methods

- `void Dispose()`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `bool has_batch_fitness()`
- `double p_distance(population arg0)`
- `double p_distance(DoubleVector arg0)`
- `problem to_problem()`

## pagmo.evolve_status

- Kind: enum
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Values

- idle
- busy
- idle_error
- busy_error

## pagmo.fair_replace

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `fair_replace()`

### Methods

- `void Dispose()`
- `string get_extra_info()`
- `string get_name()`
- `IndividualsGroup replace_wrapped(IndividualsGroup a, uint b, uint c, uint d, uint e, uint f, DoubleVector g, IndividualsGroup h)`

## pagmo.free_form

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/topologies/free_form.html

### Constructors

- `free_form()`

### Methods

- `void Dispose()`
- `string get_name()`
- `void push_back()`

## pagmo.fully_connected

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/topologies/fully_connected.html

### Constructors

- `fully_connected()`
- `fully_connected(double arg0)`
- `fully_connected(uint arg0, double arg1)`

### Methods

- `void Dispose()`
- `TopologyConnections get_connections(uint arg0)`
- `string get_extra_info()`
- `string get_name()`
- `double get_weight()`
- `uint num_vertices()`
- `void push_back()`

## pagmo.gaco

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/gaco.html

### Constructors

- `gaco()`
- `gaco(uint gen)`
- `gaco(uint gen, uint ker)`
- `gaco(uint gen, uint ker, double q)`
- `gaco(uint gen, uint ker, double q, double oracle)`
- `gaco(uint gen, uint ker, double q, double oracle, double acc)`
- `gaco(uint gen, uint ker, double q, double oracle, double acc, uint threshold)`
- `gaco(uint gen, uint ker, double q, double oracle, double acc, uint threshold, uint n_gen_mark)`
- `gaco(uint gen, uint ker, double q, double oracle, double acc, uint threshold, uint n_gen_mark, uint impstop)`
- `gaco(uint gen, uint ker, double q, double oracle, double acc, uint threshold, uint n_gen_mark, uint impstop, uint evalstop)`
- `gaco(uint gen, uint ker, double q, double oracle, double acc, uint threshold, uint n_gen_mark, uint impstop, uint evalstop, double focus)`
- `gaco(uint gen, uint ker, double q, double oracle, double acc, uint threshold, uint n_gen_mark, uint impstop, uint evalstop, double focus, bool memory)`
- `gaco(uint gen, uint ker, double q, double oracle, double acc, uint threshold, uint n_gen_mark, uint impstop, uint evalstop, double focus, bool memory, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<GacoLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `uint get_gen()`
- `GacoLogEntryVector get_log_entries()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_bfe(bfe b)`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint arg0)`
- `algorithm to_algorithm()`

## pagmo.golomb_ruler

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/golomb_ruler.html

### Constructors

- `golomb_ruler()`
- `golomb_ruler(uint order)`
- `golomb_ruler(uint order, uint upper_bound)`

### Methods

- `void Dispose()`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `bool has_batch_fitness()`
- `problem to_problem()`

## pagmo.grid_search

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `grid_search(uint uniformStepsPerDimension)`
- `grid_search(uint[] stepsPerDimension)`

### Methods

- `void Dispose()`
- `population evolve(population pop)`
- `string get_extra_info()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_seed(uint seed)`
- `void set_verbosity(uint level)`

## pagmo.griewank

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/griewank.html

### Constructors

- `griewank()`
- `griewank(uint dim)`

### Properties

- `uint m_dim { get; set }`

### Methods

- `void Dispose()`
- `DoubleVector best_known()`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `bool has_batch_fitness()`
- `problem to_problem()`

## pagmo.gwo

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/gwo.html

### Constructors

- `gwo()`
- `gwo(uint gen)`
- `gwo(uint gen, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<GwoLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `uint get_gen()`
- `GwoLogEntryVector get_log_entries()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint level)`
- `algorithm to_algorithm()`

## pagmo.gwo+GwoLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `GwoLogLine(uint Generation, double Alpha, double Beta, double Delta)`

### Properties

- `string AlgorithmName { get }`
- `double Alpha { get; set }`
- `double Beta { get; set }`
- `double Delta { get; set }`
- `uint Generation { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`

### Methods

- `void Deconstruct(out uint Generation, out double Alpha, out double Beta, out double Delta)`
- `bool Equals(object obj)`
- `bool Equals(GwoLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.hock_schittkowski_71

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/hock_schittkowski_71.html

### Constructors

- `hock_schittkowski_71()`

### Methods

- `void Dispose()`
- `DoubleVector best_known()`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_extra_info()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `DoubleVector gradient(DoubleVector arg0)`
- `bool has_batch_fitness()`
- `VectorOfVectorOfDoubles hessians(DoubleVector arg0)`
- `problem to_problem()`

## pagmo.hv_algorithm

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Methods

- `void Dispose()`
- `HvAlgorithmSharedPtr clone()`
- `double compute(VectorOfVectorOfDoubles points, DoubleVector r_point)`
- `DoubleVector contributions(VectorOfVectorOfDoubles arg0, DoubleVector arg1)`
- `double exclusive(uint p_idx, VectorOfVectorOfDoubles arg1, DoubleVector arg2)`
- `string get_name()`
- `ulong greatest_contributor(VectorOfVectorOfDoubles arg0, DoubleVector arg1)`
- `ulong least_contributor(VectorOfVectorOfDoubles arg0, DoubleVector arg1)`
- `void verify_before_compute(VectorOfVectorOfDoubles points, DoubleVector r_point)`
- `static double volume_between(DoubleVector a, DoubleVector b)`
- `static double volume_between(DoubleVector a, DoubleVector b, uint dim_bound)`

## pagmo.hypervolume

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `hypervolume()`
- `hypervolume(VectorOfVectorOfDoubles points)`
- `hypervolume(VectorOfVectorOfDoubles points, bool verify)`
- `hypervolume(hypervolume arg0)`
- `hypervolume(population pop)`
- `hypervolume(population pop, bool verify)`

### Methods

- `void Dispose()`
- `double compute(DoubleVector arg0)`
- `double compute(DoubleVector arg0, hv_algorithm arg1)`
- `double compute_via_best_compute(DoubleVector reference_point)`
- `DoubleVector contributions(DoubleVector arg0)`
- `DoubleVector contributions(DoubleVector arg0, hv_algorithm arg1)`
- `DoubleVector contributions_via_best_contributions(DoubleVector reference_point)`
- `double exclusive(uint arg0, DoubleVector arg1)`
- `double exclusive(uint arg0, DoubleVector arg1, hv_algorithm arg2)`
- `double exclusive_via_best_exclusive(uint point_index, DoubleVector reference_point)`
- `HvAlgorithmSharedPtr get_best_compute(DoubleVector arg0)`
- `HvAlgorithmSharedPtr get_best_contributions(DoubleVector arg0)`
- `HvAlgorithmSharedPtr get_best_exclusive(uint arg0, DoubleVector arg1)`
- `bool get_copy_points()`
- `VectorOfVectorOfDoubles get_points()`
- `bool get_verify()`
- `ulong greatest_contributor(DoubleVector arg0)`
- `ulong greatest_contributor(DoubleVector arg0, hv_algorithm arg1)`
- `ulong least_contributor(DoubleVector arg0)`
- `ulong least_contributor(DoubleVector arg0, hv_algorithm arg1)`
- `DoubleVector refpoint()`
- `DoubleVector refpoint(double offset)`
- `void set_copy_points(bool arg0)`
- `void set_verify(bool arg0)`

## pagmo.ihs

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/ihs.html

### Constructors

- `ihs()`
- `ihs(uint gen)`
- `ihs(uint gen, double phmcr)`
- `ihs(uint gen, double phmcr, double ppar_min)`
- `ihs(uint gen, double phmcr, double ppar_min, double ppar_max)`
- `ihs(uint gen, double phmcr, double ppar_min, double ppar_max, double bw_min)`
- `ihs(uint gen, double phmcr, double ppar_min, double ppar_max, double bw_min, double bw_max)`
- `ihs(uint gen, double phmcr, double ppar_min, double ppar_max, double bw_min, double bw_max, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<IhsLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `IhsLogEntryVector get_log_entries()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint arg0)`
- `algorithm to_algorithm()`

## pagmo.ihs+IhsLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `IhsLogLine(ulong FunctionEvaluations, double PitchAdjustmentRate, double Bandwidth, double DecisionFlatness, double FitnessFlatness, ulong ViolatedConstraints, double ViolationNorm, IReadOnlyList<double> IdealPoint)`

### Properties

- `string AlgorithmName { get }`
- `double Bandwidth { get; set }`
- `double DecisionFlatness { get; set }`
- `double FitnessFlatness { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `IReadOnlyList<double> IdealPoint { get; set }`
- `double PitchAdjustmentRate { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`
- `ulong ViolatedConstraints { get; set }`
- `double ViolationNorm { get; set }`

### Methods

- `void Deconstruct(out ulong FunctionEvaluations, out double PitchAdjustmentRate, out double Bandwidth, out double DecisionFlatness, out double FitnessFlatness, out ulong ViolatedConstraints, out double ViolationNorm, out IReadOnlyList<double> IdealPoint)`
- `bool Equals(object obj)`
- `bool Equals(IhsLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.inventory

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/inventory.html

### Constructors

- `inventory()`
- `inventory(uint weeks)`
- `inventory(uint weeks, uint sample_size)`
- `inventory(uint weeks, uint sample_size, uint seed)`

### Methods

- `void Dispose()`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_extra_info()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `bool has_batch_fitness()`
- `void set_seed(uint seed)`
- `problem to_problem()`

## pagmo.ipopt

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/ipopt.html

### Constructors

- `ipopt()`

### Methods

- `void Dispose()`
- `int GetLastOptimizationResultCode()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<IpoptLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `int get_last_opt_result_code()`
- `IpoptLogEntryVector get_log_entries()`
- `string get_name()`
- `uint get_seed()`
- `int get_thread_safety()`
- `uint get_verbosity()`
- `void reset_integer_options()`
- `void reset_numeric_options()`
- `void reset_string_options()`
- `void set_integer_option(string name, ulong value)`
- `void set_integer_option_u64(string name, ulong value)`
- `void set_numeric_option(string arg0, double arg1)`
- `void set_seed(uint seed)`
- `void set_string_option(string arg0, string arg1)`
- `void set_verbosity(uint n)`
- `algorithm to_algorithm()`

## pagmo.ipopt+IpoptLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `IpoptLogLine(ulong ObjectiveEvaluations, double Objective, ulong ViolatedConstraints, double ViolationNorm, bool Feasible)`

### Properties

- `string AlgorithmName { get }`
- `bool Feasible { get; set }`
- `double Objective { get; set }`
- `ulong ObjectiveEvaluations { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`
- `ulong ViolatedConstraints { get; set }`
- `double ViolationNorm { get; set }`

### Methods

- `void Deconstruct(out ulong ObjectiveEvaluations, out double Objective, out ulong ViolatedConstraints, out double ViolationNorm, out bool Feasible)`
- `bool Equals(object obj)`
- `bool Equals(IpoptLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.island

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/island.html

### Constructors

- `island(island arg0)`

### Methods

- `static island Create(algorithm a, problem prob, uint pop_size, uint seed)`
- `static island Create(algorithm algorithm, IProblem problem, ulong popSize, Nullable<uint> seed, thread_island islandType)`
- `static island Create(IAlgorithm algorithm, IProblem problem, ulong popSize, Nullable<uint> seed, thread_island islandType)`
- `static island CreateFromPopulation(algorithm a, population p)`
- `static island CreateFromPopulationWithPolicies(algorithm a, population p, fair_replace r, select_best s)`
- `static island CreateFromPopulationWithPolicies(algorithm a, population p, r_policyPagmoWrapper r, s_policyPagmoWrapper s)`
- `static island CreateWithBfe(algorithm a, problem prob, bfe b, uint pop_size, uint seed)`
- `static island CreateWithBfe(algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, Nullable<uint> seed, thread_island islandType)`
- `static island CreateWithBfe(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, Nullable<uint> seed, thread_island islandType)`
- `static island CreateWithBfeAndPolicies(algorithm a, problem prob, bfe b, uint pop_size, fair_replace r, select_best s, uint seed)`
- `static island CreateWithBfeAndPolicies(algorithm a, problem prob, bfe b, uint pop_size, r_policyPagmoWrapper r, s_policyPagmoWrapper s, uint seed)`
- `static island CreateWithBfeAndPolicies(algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, Nullable<uint> seed, thread_island islandType)`
- `static island CreateWithBfeAndPolicies(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, Nullable<uint> seed, thread_island islandType)`
- `static island CreateWithBfeAndPolicies(algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, Nullable<uint> seed, thread_island islandType)`
- `static island CreateWithBfeAndPolicies(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, Nullable<uint> seed, thread_island islandType)`
- `static island CreateWithBfeAndPolicies(algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, Nullable<uint> seed, thread_island islandType)`
- `static island CreateWithBfeAndPolicies(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, Nullable<uint> seed, thread_island islandType)`
- `static island CreateWithPolicies(algorithm a, problem prob, uint pop_size, fair_replace r, select_best s, uint seed)`
- `static island CreateWithPolicies(algorithm a, problem prob, uint pop_size, r_policyPagmoWrapper r, s_policyPagmoWrapper s, uint seed)`
- `static island CreateWithPolicies(algorithm algorithm, IProblem problem, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, Nullable<uint> seed, thread_island islandType)`
- `static island CreateWithPolicies(IAlgorithm algorithm, IProblem problem, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, Nullable<uint> seed, thread_island islandType)`
- `static island CreateWithPolicies(algorithm algorithm, IProblem problem, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, Nullable<uint> seed, thread_island islandType)`
- `static island CreateWithPolicies(IAlgorithm algorithm, IProblem problem, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, Nullable<uint> seed, thread_island islandType)`
- `static island CreateWithPolicies(algorithm algorithm, IProblem problem, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, Nullable<uint> seed, thread_island islandType)`
- `static island CreateWithPolicies(IAlgorithm algorithm, IProblem problem, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, Nullable<uint> seed, thread_island islandType)`
- `static island CreateWithThreadIsland(thread_island islandType, algorithm algorithm, IProblem problem, ulong popSize, Nullable<uint> seed)`
- `static island CreateWithThreadIsland(thread_island islandType, IAlgorithm algorithm, IProblem problem, ulong popSize, Nullable<uint> seed)`
- `static island CreateWithThreadIsland(thread_island isl, algorithm a, problem prob, uint pop_size, uint seed)`
- `static island CreateWithThreadIslandAndBfe(thread_island islandType, algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, Nullable<uint> seed)`
- `static island CreateWithThreadIslandAndBfe(thread_island islandType, IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, Nullable<uint> seed)`
- `static island CreateWithThreadIslandAndBfe(thread_island isl, algorithm a, problem prob, bfe b, uint pop_size, uint seed)`
- `static island CreateWithThreadIslandAndBfeAndPolicies(thread_island islandType, algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, Nullable<uint> seed)`
- `static island CreateWithThreadIslandAndBfeAndPolicies(thread_island islandType, IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, Nullable<uint> seed)`
- `static island CreateWithThreadIslandAndBfeAndPolicies(thread_island islandType, algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, Nullable<uint> seed)`
- `static island CreateWithThreadIslandAndBfeAndPolicies(thread_island islandType, IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, Nullable<uint> seed)`
- `static island CreateWithThreadIslandAndBfeAndPolicies(thread_island islandType, algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, Nullable<uint> seed)`
- `static island CreateWithThreadIslandAndBfeAndPolicies(thread_island islandType, IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, Nullable<uint> seed)`
- `static island CreateWithThreadIslandAndBfeAndPolicies(thread_island isl, algorithm a, problem prob, bfe b, uint pop_size, fair_replace r, select_best s, uint seed)`
- `static island CreateWithThreadIslandAndBfeAndPolicies(thread_island isl, algorithm a, problem prob, bfe b, uint pop_size, r_policyPagmoWrapper r, s_policyPagmoWrapper s, uint seed)`
- `static island CreateWithThreadIslandAndPolicies(thread_island islandType, algorithm algorithm, IProblem problem, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, Nullable<uint> seed)`
- `static island CreateWithThreadIslandAndPolicies(thread_island islandType, IAlgorithm algorithm, IProblem problem, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, Nullable<uint> seed)`
- `static island CreateWithThreadIslandAndPolicies(thread_island islandType, algorithm algorithm, IProblem problem, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, Nullable<uint> seed)`
- `static island CreateWithThreadIslandAndPolicies(thread_island islandType, IAlgorithm algorithm, IProblem problem, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, Nullable<uint> seed)`
- `static island CreateWithThreadIslandAndPolicies(thread_island islandType, algorithm algorithm, IProblem problem, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, Nullable<uint> seed)`
- `static island CreateWithThreadIslandAndPolicies(thread_island islandType, IAlgorithm algorithm, IProblem problem, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, Nullable<uint> seed)`
- `static island CreateWithThreadIslandAndPolicies(thread_island isl, algorithm a, problem prob, uint pop_size, fair_replace r, select_best s, uint seed)`
- `static island CreateWithThreadIslandAndPolicies(thread_island isl, algorithm a, problem prob, uint pop_size, r_policyPagmoWrapper r, s_policyPagmoWrapper s, uint seed)`
- `void Dispose()`
- `void evolve()`
- `void evolve(uint n)`
- `algorithm get_algorithm()`
- `string get_extra_info()`
- `string get_name()`
- `population get_population()`
- `bool is_valid()`
- `void set_algorithm(algorithm arg0)`
- `void set_population(population arg0)`
- `int status()`
- `void wait()`
- `void wait_check()`

## pagmo.lennard_jones

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/lennard_jones.html

### Constructors

- `lennard_jones()`
- `lennard_jones(uint atoms)`

### Methods

- `void Dispose()`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `bool has_batch_fitness()`
- `problem to_problem()`

## pagmo.luksan_vlcek1

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/luksan_vlcek1.html

### Constructors

- `luksan_vlcek1()`
- `luksan_vlcek1(uint dim)`

### Properties

- `uint m_dim { get; set }`

### Methods

- `void Dispose()`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `DoubleVector gradient(DoubleVector arg0)`
- `bool has_batch_fitness()`
- `problem to_problem()`

## pagmo.maco

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/maco.html

### Constructors

- `maco()`
- `maco(uint gen)`
- `maco(uint gen, uint ker)`
- `maco(uint gen, uint ker, double q)`
- `maco(uint gen, uint ker, double q, uint threshold)`
- `maco(uint gen, uint ker, double q, uint threshold, uint n_gen_mark)`
- `maco(uint gen, uint ker, double q, uint threshold, uint n_gen_mark, uint evalstop)`
- `maco(uint gen, uint ker, double q, uint threshold, uint n_gen_mark, uint evalstop, double focus)`
- `maco(uint gen, uint ker, double q, uint threshold, uint n_gen_mark, uint evalstop, double focus, bool memory)`
- `maco(uint gen, uint ker, double q, uint threshold, uint n_gen_mark, uint evalstop, double focus, bool memory, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<MacoLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `uint get_gen()`
- `MoVectorLogEntryVector get_log_entries()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_bfe(bfe b)`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint level)`
- `algorithm to_algorithm()`

## pagmo.maco+MacoLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `MacoLogLine(uint Generation, ulong FunctionEvaluations, IReadOnlyList<double> FitnessVector)`

### Properties

- `string AlgorithmName { get }`
- `IReadOnlyList<double> FitnessVector { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `uint Generation { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`

### Methods

- `void Deconstruct(out uint Generation, out ulong FunctionEvaluations, out IReadOnlyList<double> FitnessVector)`
- `bool Equals(object obj)`
- `bool Equals(MacoLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.managed_algorithm

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `managed_algorithm()`
- `managed_algorithm(algorithm_callback cb)`

### Methods

- `void Dispose()`
- `population evolve(population pop)`
- `string get_extra_info()`
- `string get_name()`
- `int get_thread_safety()`
- `bool has_set_seed()`
- `bool has_set_verbosity()`
- `void set_seed(uint seed)`
- `void set_verbosity(uint level)`

## pagmo.managed_algorithm+null_algorithm_callback

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `null_algorithm_callback()`

### Methods

- `population evolve(population pop)`

## pagmo.managed_problem

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `managed_problem()`
- `managed_problem(problem_callback cb)`

### Methods

- `void Dispose()`
- `SparsityIndex[] GetGradientSparsityEntries()`
- `SparsityIndex[][] GetHessiansSparsityEntries()`
- `DoubleVector batch_fitness(DoubleVector dvs)`
- `DoubleVector fitness(DoubleVector x)`
- `PairOfDoubleVectors get_bounds()`
- `string get_extra_info()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `DoubleVector gradient(DoubleVector x)`
- `SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t gradient_sparsity()`
- `bool has_batch_fitness()`
- `bool has_gradient()`
- `bool has_gradient_sparsity()`
- `bool has_hessians()`
- `bool has_hessians_sparsity()`
- `bool has_set_seed()`
- `VectorOfVectorOfDoubles hessians(DoubleVector x)`
- `SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t hessians_sparsity()`
- `void set_seed(uint seed)`

## pagmo.mbh

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/mbh.html

### Constructors

- `mbh()`
- `mbh(algorithm a, uint stop, double perturb)`
- `mbh(algorithm a, uint stop, double perturb, uint seed)`
- `mbh(algorithm a, uint stop, DoubleVector perturb)`
- `mbh(algorithm a, uint stop, DoubleVector perturb, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<MbhLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `algorithm get_inner_algorithm()`
- `MbhLogEntryVector get_log_entries()`
- `string get_name()`
- `DoubleVector get_perturb()`
- `uint get_seed()`
- `int get_thread_safety()`
- `uint get_verbosity()`
- `void set_perturb(DoubleVector arg0)`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint level)`
- `algorithm to_algorithm()`

## pagmo.mbh+MbhLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `MbhLogLine(ulong FunctionEvaluations, double BestFitness, uint ViolatedConstraints, double ViolationNorm, uint Trial)`

### Properties

- `string AlgorithmName { get }`
- `double BestFitness { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`
- `uint Trial { get; set }`
- `uint ViolatedConstraints { get; set }`
- `double ViolationNorm { get; set }`

### Methods

- `void Deconstruct(out ulong FunctionEvaluations, out double BestFitness, out uint ViolatedConstraints, out double ViolationNorm, out uint Trial)`
- `bool Equals(object obj)`
- `bool Equals(MbhLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.member_bfe

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/batch_evaluators/member_bfe.html

### Constructors

- `member_bfe()`
- `member_bfe(member_bfe arg0)`

### Methods

- `void Dispose()`
- `DoubleVector Operator(IProblem problem, DoubleVector batchX)`
- `string get_name()`
- `bfe to_bfe()`

## pagmo.migrant_handling

- Kind: enum
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Values

- preserve
- evict

## pagmo.migration_type

- Kind: enum
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Values

- p2p
- broadcast

## pagmo.minlp_rastrigin

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/minlp_rastrigin.html

### Constructors

- `minlp_rastrigin()`
- `minlp_rastrigin(uint dim_c)`
- `minlp_rastrigin(uint dim_c, uint dim_i)`

### Methods

- `void Dispose()`
- `SparsityIndex[][] GetHessiansSparsityEntries()`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_extra_info()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `DoubleVector gradient(DoubleVector arg0)`
- `bool has_batch_fitness()`
- `VectorOfVectorOfDoubles hessians(DoubleVector arg0)`
- `SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t hessians_sparsity()`
- `problem to_problem()`

## pagmo.moead

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/moead.html

### Constructors

- `moead()`
- `moead(uint gen)`
- `moead(uint gen, string weight_generation)`
- `moead(uint gen, string weight_generation, string decomposition)`
- `moead(uint gen, string weight_generation, string decomposition, uint neighbours)`
- `moead(uint gen, string weight_generation, string decomposition, uint neighbours, double CR)`
- `moead(uint gen, string weight_generation, string decomposition, uint neighbours, double CR, double F)`
- `moead(uint gen, string weight_generation, string decomposition, uint neighbours, double CR, double F, double eta_m)`
- `moead(uint gen, string weight_generation, string decomposition, uint neighbours, double CR, double F, double eta_m, double realb)`
- `moead(uint gen, string weight_generation, string decomposition, uint neighbours, double CR, double F, double eta_m, double realb, uint limit)`
- `moead(uint gen, string weight_generation, string decomposition, uint neighbours, double CR, double F, double eta_m, double realb, uint limit, bool preserve_diversity)`
- `moead(uint gen, string weight_generation, string decomposition, uint neighbours, double CR, double F, double eta_m, double realb, uint limit, bool preserve_diversity, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<MoeadLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `uint get_gen()`
- `MoeadLogEntryVector get_log_entries()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint level)`
- `algorithm to_algorithm()`

## pagmo.moead+MoeadLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `MoeadLogLine(uint Generation, ulong FunctionEvaluations, double DecomposedFitness, IReadOnlyList<double> IdealPoint)`

### Properties

- `string AlgorithmName { get }`
- `double DecomposedFitness { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `uint Generation { get; set }`
- `IReadOnlyList<double> IdealPoint { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`

### Methods

- `void Deconstruct(out uint Generation, out ulong FunctionEvaluations, out double DecomposedFitness, out IReadOnlyList<double> IdealPoint)`
- `bool Equals(object obj)`
- `bool Equals(MoeadLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.moead_gen

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/moead_gen.html

### Constructors

- `moead_gen()`
- `moead_gen(uint gen)`
- `moead_gen(uint gen, string weight_generation)`
- `moead_gen(uint gen, string weight_generation, string decomposition)`
- `moead_gen(uint gen, string weight_generation, string decomposition, uint neighbours)`
- `moead_gen(uint gen, string weight_generation, string decomposition, uint neighbours, double CR)`
- `moead_gen(uint gen, string weight_generation, string decomposition, uint neighbours, double CR, double F)`
- `moead_gen(uint gen, string weight_generation, string decomposition, uint neighbours, double CR, double F, double eta_m)`
- `moead_gen(uint gen, string weight_generation, string decomposition, uint neighbours, double CR, double F, double eta_m, double realb)`
- `moead_gen(uint gen, string weight_generation, string decomposition, uint neighbours, double CR, double F, double eta_m, double realb, uint limit)`
- `moead_gen(uint gen, string weight_generation, string decomposition, uint neighbours, double CR, double F, double eta_m, double realb, uint limit, bool preserve_diversity)`
- `moead_gen(uint gen, string weight_generation, string decomposition, uint neighbours, double CR, double F, double eta_m, double realb, uint limit, bool preserve_diversity, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<MoeadGenLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `uint get_gen()`
- `MoeadLogEntryVector get_log_entries()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_bfe(bfe b)`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint level)`
- `algorithm to_algorithm()`

## pagmo.moead_gen+MoeadGenLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `MoeadGenLogLine(uint Generation, ulong FunctionEvaluations, double DecomposedFitness, IReadOnlyList<double> IdealPoint)`

### Properties

- `string AlgorithmName { get }`
- `double DecomposedFitness { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `uint Generation { get; set }`
- `IReadOnlyList<double> IdealPoint { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`

### Methods

- `void Deconstruct(out uint Generation, out ulong FunctionEvaluations, out double DecomposedFitness, out IReadOnlyList<double> IdealPoint)`
- `bool Equals(object obj)`
- `bool Equals(MoeadGenLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.nlopt

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/nlopt.html

### Constructors

- `nlopt()`
- `nlopt(string arg0)`
- `nlopt(nlopt arg0)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<NloptLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `double get_ftol_abs()`
- `double get_ftol_rel()`
- `SWIGTYPE_p_nlopt_result get_last_opt_result()`
- `nlopt get_local_optimizer()`
- `NloptLogEntryVector get_log_entries()`
- `int get_maxeval()`
- `int get_maxtime()`
- `string get_name()`
- `uint get_seed()`
- `string get_solver_name()`
- `double get_stopval()`
- `uint get_verbosity()`
- `double get_xtol_abs()`
- `double get_xtol_rel()`
- `void set_ftol_abs(double arg0)`
- `void set_ftol_rel(double arg0)`
- `void set_maxeval(int n)`
- `void set_maxtime(int n)`
- `void set_seed(uint seed)`
- `void set_stopval(double arg0)`
- `void set_verbosity(uint n)`
- `void set_xtol_abs(double arg0)`
- `void set_xtol_rel(double arg0)`
- `algorithm to_algorithm()`
- `void unset_local_optimizer()`

## pagmo.nlopt+NloptLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `NloptLogLine(ulong FunctionEvaluations, double Objective, ulong ViolatedConstraints, double ViolationNorm, bool Feasible)`

### Properties

- `string AlgorithmName { get }`
- `bool Feasible { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `double Objective { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`
- `ulong ViolatedConstraints { get; set }`
- `double ViolationNorm { get; set }`

### Methods

- `void Deconstruct(out ulong FunctionEvaluations, out double Objective, out ulong ViolatedConstraints, out double ViolationNorm, out bool Feasible)`
- `bool Equals(object obj)`
- `bool Equals(NloptLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.not_population_based

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `not_population_based()`

### Methods

- `void Dispose()`
- `uint replacement_count()`
- `string replacement_policy()`
- `bool replacement_uses_count()`
- `uint selection_count()`
- `string selection_policy()`
- `bool selection_uses_count()`
- `void set_random_sr_seed(uint arg0)`
- `void set_replacement(string arg0)`
- `void set_replacement(uint n)`
- `void set_selection(string arg0)`
- `void set_selection(uint n)`

## pagmo.nsga2

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/nsga2.html

### Constructors

- `nsga2()`
- `nsga2(uint gen)`
- `nsga2(uint gen, double cr)`
- `nsga2(uint gen, double cr, double eta_c)`
- `nsga2(uint gen, double cr, double eta_c, double m)`
- `nsga2(uint gen, double cr, double eta_c, double m, double eta_m)`
- `nsga2(uint gen, double cr, double eta_c, double m, double eta_m, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<Nsga2LogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `MoVectorLogEntryVector get_log_entries()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_bfe(bfe b)`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint level)`
- `algorithm to_algorithm()`

## pagmo.nsga2+Nsga2LogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `Nsga2LogLine(uint Generation, ulong FunctionEvaluations, IReadOnlyList<double> FitnessVector)`

### Properties

- `string AlgorithmName { get }`
- `IReadOnlyList<double> FitnessVector { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `uint Generation { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`

### Methods

- `void Deconstruct(out uint Generation, out ulong FunctionEvaluations, out IReadOnlyList<double> FitnessVector)`
- `bool Equals(object obj)`
- `bool Equals(Nsga2LogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.nspso

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/nspso.html

### Constructors

- `nspso()`
- `nspso(uint gen)`
- `nspso(uint gen, double omega)`
- `nspso(uint gen, double omega, double c1)`
- `nspso(uint gen, double omega, double c1, double c2)`
- `nspso(uint gen, double omega, double c1, double c2, double chi)`
- `nspso(uint gen, double omega, double c1, double c2, double chi, double v_coeff)`
- `nspso(uint gen, double omega, double c1, double c2, double chi, double v_coeff, uint leader_selection_range)`
- `nspso(uint gen, double omega, double c1, double c2, double chi, double v_coeff, uint leader_selection_range, string diversity_mechanism)`
- `nspso(uint gen, double omega, double c1, double c2, double chi, double v_coeff, uint leader_selection_range, string diversity_mechanism, bool memory)`
- `nspso(uint gen, double omega, double c1, double c2, double chi, double v_coeff, uint leader_selection_range, string diversity_mechanism, bool memory, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<NspsoLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `uint get_gen()`
- `MoVectorLogEntryVector get_log_entries()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_bfe(bfe b)`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint level)`
- `algorithm to_algorithm()`

## pagmo.nspso+NspsoLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `NspsoLogLine(uint Generation, ulong FunctionEvaluations, IReadOnlyList<double> FitnessVector)`

### Properties

- `string AlgorithmName { get }`
- `IReadOnlyList<double> FitnessVector { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `uint Generation { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`

### Methods

- `void Deconstruct(out uint Generation, out ulong FunctionEvaluations, out IReadOnlyList<double> FitnessVector)`
- `bool Equals(object obj)`
- `bool Equals(NspsoLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.null_algorithm

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/null_algorithm.html

### Constructors

- `null_algorithm()`

### Methods

- `void Dispose()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_seed(uint seed)`
- `void set_verbosity(uint level)`
- `algorithm to_algorithm()`

## pagmo.null_problem

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/null_problem.html

### Constructors

- `null_problem()`
- `null_problem(uint nobj)`
- `null_problem(uint nobj, uint nec)`
- `null_problem(uint nobj, uint nec, uint nic)`
- `null_problem(uint nobj, uint nec, uint nic, uint nix)`

### Methods

- `void Dispose()`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `bool has_batch_fitness()`
- `problem to_problem()`

## pagmo.null_problem_callback

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `null_problem_callback()`

### Methods

- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`

## pagmo.pagmo

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/utils/multi_objective.html

### Constructors

- `pagmo()`

### Methods

- `static CmaesLogEntryVector Cmaes_GetLogEntries(cmaes algo)`
- `static CompassSearchLogEntryVector CompassSearch_GetLogEntries(compass_search algo)`
- `static CstrsLogEntryVector Cstrs_GetLogEntries(cstrs_self_adaptive algo)`
- `static De1220LogEntryVector De1220_GetLogEntries(de1220 algo)`
- `static DeLogEntryVector De_GetLogEntries(de algo)`
- `static double[] DecomposeObjectiveValues(DoubleVector objectiveValues, DoubleVector weights, DoubleVector referencePoint, string method)`
- `static FNDSResult FastNonDominatedSorting(VectorOfVectorOfDoubles input)`
- `static GwoLogEntryVector Gwo_GetLogEntries(gwo algo)`
- `static double[] IdealValues(VectorOfVectorOfDoubles fitnessValues)`
- `static IhsLogEntryVector Ihs_GetLogEntries(ihs algo)`
- `static IpoptLogEntryVector Ipopt_GetLogEntries(ipopt algo)`
- `static MoVectorLogEntryVector Maco_GetLogEntries(maco algo)`
- `static MoeadLogEntryVector MoeadGen_GetLogEntries(moead_gen algo)`
- `static MoeadLogEntryVector Moead_GetLogEntries(moead algo)`
- `static double[] NadirValues(VectorOfVectorOfDoubles fitnessValues)`
- `static NloptLogEntryVector Nlopt_GetLogEntries(nlopt algo)`
- `static ulong[] NonDominatedFront2DIndices(VectorOfVectorOfDoubles fitnessValues)`
- `static MoVectorLogEntryVector Nsga2_GetLogEntries(nsga2 algo)`
- `static MoVectorLogEntryVector Nspso_GetLogEntries(nspso algo)`
- `static bool ParetoDominates(DoubleVector lhsFitness, DoubleVector rhsFitness)`
- `static PsoLogEntryVector PsoGen_GetLogEntries(pso_gen algo)`
- `static PsoLogEntryVector Pso_GetLogEntries(pso algo)`
- `static SadeLogEntryVector Sade_GetLogEntries(sade algo)`
- `static SeaLogEntryVector Sea_GetLogEntries(sea algo)`
- `static ulong[] SelectBestNMoIndices(VectorOfVectorOfDoubles fitnessValues, ulong selectionCount)`
- `static SgaLogEntryVector Sga_GetLogEntries(sga algo)`
- `static SimulatedAnnealingLogEntryVector SimulatedAnnealing_GetLogEntries(simulated_annealing algo)`
- `static ulong[] SortPopulationMoIndices(VectorOfVectorOfDoubles fitnessValues)`
- `static XnesLogEntryVector Xnes_GetLogEntries(xnes algo)`
- `static DoubleVector crowding_distance(VectorOfVectorOfDoubles arg0)`
- `static DoubleVector decompose_objectives(DoubleVector arg0, DoubleVector arg1, DoubleVector arg2, string arg3)`
- `static DoubleVector ideal(VectorOfVectorOfDoubles arg0)`
- `static uint max_stream_output_length()`
- `static DoubleVector nadir(VectorOfVectorOfDoubles arg0)`
- `static ULongLongVector non_dominated_front_2d(VectorOfVectorOfDoubles arg0)`
- `static bool pareto_dominance(DoubleVector arg0, DoubleVector arg1)`
- `static ULongLongVector select_best_N_mo(VectorOfVectorOfDoubles arg0, ulong arg1)`
- `static ULongLongVector sort_population_mo(VectorOfVectorOfDoubles arg0)`

## pagmo.population

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/population.html

### Constructors

- `population(IProblem problem, ulong popSize)`
- `population(IProblem problem, ulong popSize, uint seed)`
- `population(problem prob)`
- `population(problem prob, uint pop_size)`
- `population(problem prob, uint pop_size, uint seed)`

### Methods

- `void Dispose()`
- `uint best_idx()`
- `uint best_idx(DoubleVector arg0)`
- `uint best_idx(double arg0)`
- `DoubleVector champion_f()`
- `DoubleVector champion_x()`
- `ULongLongVector get_ID()`
- `VectorOfVectorOfDoubles get_f()`
- `problem get_problem()`
- `uint get_seed()`
- `VectorOfVectorOfDoubles get_x()`
- `void push_back(DoubleVector arg0)`
- `void push_back(DoubleVector arg0, DoubleVector arg1)`
- `DoubleVector random_decision_vector()`
- `void set_x(uint arg0, DoubleVector arg1)`
- `void set_xf(uint arg0, DoubleVector arg1, DoubleVector arg2)`
- `uint size()`
- `uint worst_idx()`
- `uint worst_idx(DoubleVector arg0)`
- `uint worst_idx(double arg0)`

## pagmo.problem

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problem.html

### Constructors

- `problem()`
- `problem(IProblem managedProblem)`
- `problem(problem arg0)`

### Methods

- `void Dispose()`
- `SparsityIndex[] GetGradientSparsityEntries()`
- `SparsityIndex[][] GetHessiansSparsityEntries()`
- `SparsityPattern GradientSparsity()`
- `VectorOfSparsityPattern HessiansSparsity()`
- `DoubleVector batch_fitness(DoubleVector arg0)`
- `bool feasibility_f(DoubleVector arg0)`
- `bool feasibility_x(DoubleVector arg0)`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `DoubleVector get_c_tol()`
- `string get_extra_info()`
- `ulong get_fevals()`
- `ulong get_gevals()`
- `ulong get_hevals()`
- `DoubleVector get_lb()`
- `string get_name()`
- `uint get_nc()`
- `uint get_ncx()`
- `uint get_nec()`
- `uint get_nf()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `uint get_nx()`
- `int get_thread_safety()`
- `DoubleVector get_ub()`
- `DoubleVector gradient(DoubleVector arg0)`
- `SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t gradient_sparsity()`
- `bool has_batch_fitness()`
- `bool has_gradient()`
- `bool has_gradient_sparsity()`
- `bool has_hessians()`
- `bool has_hessians_sparsity()`
- `bool has_set_seed()`
- `VectorOfVectorOfDoubles hessians(DoubleVector arg0)`
- `SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t hessians_sparsity()`
- `bool is_stochastic()`
- `bool is_valid()`
- `void set_c_tol(DoubleVector arg0)`
- `void set_c_tol(double arg0)`
- `void set_seed(uint arg0)`

## pagmo.problem_callback

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `problem_callback()`

### Methods

- `void Dispose()`
- `DoubleVector batch_fitness(DoubleVector arg0)`
- `DoubleVector fitness(DoubleVector x)`
- `PairOfDoubleVectors get_bounds()`
- `string get_extra_info()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `DoubleVector gradient(DoubleVector arg0)`
- `SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t gradient_sparsity()`
- `bool has_batch_fitness()`
- `bool has_gradient()`
- `bool has_gradient_sparsity()`
- `bool has_hessians()`
- `bool has_hessians_sparsity()`
- `bool has_set_seed()`
- `VectorOfVectorOfDoubles hessians(DoubleVector arg0)`
- `SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t hessians_sparsity()`
- `void set_seed(uint arg0)`

## pagmo.problem_callback+SwigDelegateproblem_callback_0

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_0(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(IntPtr x, AsyncCallback callback, object object)`
- `IntPtr EndInvoke(IAsyncResult result)`
- `IntPtr Invoke(IntPtr x)`

## pagmo.problem_callback+SwigDelegateproblem_callback_1

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_1(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `IntPtr EndInvoke(IAsyncResult result)`
- `IntPtr Invoke()`

## pagmo.problem_callback+SwigDelegateproblem_callback_10

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_10(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(IntPtr arg0, AsyncCallback callback, object object)`
- `IntPtr EndInvoke(IAsyncResult result)`
- `IntPtr Invoke(IntPtr arg0)`

## pagmo.problem_callback+SwigDelegateproblem_callback_11

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_11(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `bool EndInvoke(IAsyncResult result)`
- `bool Invoke()`

## pagmo.problem_callback+SwigDelegateproblem_callback_12

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_12(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `IntPtr EndInvoke(IAsyncResult result)`
- `IntPtr Invoke()`

## pagmo.problem_callback+SwigDelegateproblem_callback_13

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_13(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `bool EndInvoke(IAsyncResult result)`
- `bool Invoke()`

## pagmo.problem_callback+SwigDelegateproblem_callback_14

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_14(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(IntPtr arg0, AsyncCallback callback, object object)`
- `IntPtr EndInvoke(IAsyncResult result)`
- `IntPtr Invoke(IntPtr arg0)`

## pagmo.problem_callback+SwigDelegateproblem_callback_15

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_15(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `bool EndInvoke(IAsyncResult result)`
- `bool Invoke()`

## pagmo.problem_callback+SwigDelegateproblem_callback_16

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_16(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `IntPtr EndInvoke(IAsyncResult result)`
- `IntPtr Invoke()`

## pagmo.problem_callback+SwigDelegateproblem_callback_17

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_17(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `bool EndInvoke(IAsyncResult result)`
- `bool Invoke()`

## pagmo.problem_callback+SwigDelegateproblem_callback_18

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_18(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(uint arg0, AsyncCallback callback, object object)`
- `void EndInvoke(IAsyncResult result)`
- `void Invoke(uint arg0)`

## pagmo.problem_callback+SwigDelegateproblem_callback_19

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_19(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `bool EndInvoke(IAsyncResult result)`
- `bool Invoke()`

## pagmo.problem_callback+SwigDelegateproblem_callback_2

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_2(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `string EndInvoke(IAsyncResult result)`
- `string Invoke()`

## pagmo.problem_callback+SwigDelegateproblem_callback_20

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_20(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `int EndInvoke(IAsyncResult result)`
- `int Invoke()`

## pagmo.problem_callback+SwigDelegateproblem_callback_3

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_3(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `string EndInvoke(IAsyncResult result)`
- `string Invoke()`

## pagmo.problem_callback+SwigDelegateproblem_callback_4

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_4(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `uint EndInvoke(IAsyncResult result)`
- `uint Invoke()`

## pagmo.problem_callback+SwigDelegateproblem_callback_5

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_5(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `uint EndInvoke(IAsyncResult result)`
- `uint Invoke()`

## pagmo.problem_callback+SwigDelegateproblem_callback_6

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_6(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `uint EndInvoke(IAsyncResult result)`
- `uint Invoke()`

## pagmo.problem_callback+SwigDelegateproblem_callback_7

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_7(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `uint EndInvoke(IAsyncResult result)`
- `uint Invoke()`

## pagmo.problem_callback+SwigDelegateproblem_callback_8

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_8(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(IntPtr arg0, AsyncCallback callback, object object)`
- `IntPtr EndInvoke(IAsyncResult result)`
- `IntPtr Invoke(IntPtr arg0)`

## pagmo.problem_callback+SwigDelegateproblem_callback_9

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegateproblem_callback_9(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `bool EndInvoke(IAsyncResult result)`
- `bool Invoke()`

## pagmo.pso

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/pso.html

### Constructors

- `pso()`
- `pso(uint gen)`
- `pso(uint gen, double omega)`
- `pso(uint gen, double omega, double eta1)`
- `pso(uint gen, double omega, double eta1, double eta2)`
- `pso(uint gen, double omega, double eta1, double eta2, double max_vel)`
- `pso(uint gen, double omega, double eta1, double eta2, double max_vel, uint variant)`
- `pso(uint gen, double omega, double eta1, double eta2, double max_vel, uint variant, uint neighb_type)`
- `pso(uint gen, double omega, double eta1, double eta2, double max_vel, uint variant, uint neighb_type, uint neighb_param)`
- `pso(uint gen, double omega, double eta1, double eta2, double max_vel, uint variant, uint neighb_type, uint neighb_param, bool memory)`
- `pso(uint gen, double omega, double eta1, double eta2, double max_vel, uint variant, uint neighb_type, uint neighb_param, bool memory, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<PsoLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `PsoLogEntryVector get_log_entries()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint level)`
- `algorithm to_algorithm()`

## pagmo.pso+PsoLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `PsoLogLine(uint Generation, ulong FunctionEvaluations, double BestFitness, double Inertia, double Cognitive, double Social)`

### Properties

- `string AlgorithmName { get }`
- `double BestFitness { get; set }`
- `double Cognitive { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `uint Generation { get; set }`
- `double Inertia { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`
- `double Social { get; set }`

### Methods

- `void Deconstruct(out uint Generation, out ulong FunctionEvaluations, out double BestFitness, out double Inertia, out double Cognitive, out double Social)`
- `bool Equals(object obj)`
- `bool Equals(PsoLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.pso_gen

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/pso_gen.html

### Constructors

- `pso_gen()`
- `pso_gen(uint gen)`
- `pso_gen(uint gen, double omega)`
- `pso_gen(uint gen, double omega, double eta1)`
- `pso_gen(uint gen, double omega, double eta1, double eta2)`
- `pso_gen(uint gen, double omega, double eta1, double eta2, double max_vel)`
- `pso_gen(uint gen, double omega, double eta1, double eta2, double max_vel, uint variant)`
- `pso_gen(uint gen, double omega, double eta1, double eta2, double max_vel, uint variant, uint neighb_type)`
- `pso_gen(uint gen, double omega, double eta1, double eta2, double max_vel, uint variant, uint neighb_type, uint neighb_param)`
- `pso_gen(uint gen, double omega, double eta1, double eta2, double max_vel, uint variant, uint neighb_type, uint neighb_param, bool memory)`
- `pso_gen(uint gen, double omega, double eta1, double eta2, double max_vel, uint variant, uint neighb_type, uint neighb_param, bool memory, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<PsoGenLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `PsoLogEntryVector get_log_entries()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_bfe(bfe b)`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint level)`
- `algorithm to_algorithm()`

## pagmo.pso_gen+PsoGenLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `PsoGenLogLine(uint Generation, ulong FunctionEvaluations, double BestFitness, double Inertia, double Cognitive, double Social)`

### Properties

- `string AlgorithmName { get }`
- `double BestFitness { get; set }`
- `double Cognitive { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `uint Generation { get; set }`
- `double Inertia { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`
- `double Social { get; set }`

### Methods

- `void Deconstruct(out uint Generation, out ulong FunctionEvaluations, out double BestFitness, out double Inertia, out double Cognitive, out double Social)`
- `bool Equals(object obj)`
- `bool Equals(PsoGenLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.r_policy

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/r__policy.html

### Constructors

- `r_policy()`
- `r_policy(r_policyBase basePolicy)`

## pagmo.r_policyBase

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `r_policyBase()`

### Methods

- `void Dispose()`
- `string get_extra_info()`
- `string get_name()`
- `bool is_valid()`
- `IndividualsGroup replace(IndividualsGroup a, uint b, uint c, uint d, uint e, uint f, DoubleVector g, IndividualsGroup h)`

## pagmo.r_policyBase+SwigDelegater_policyBase_0

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegater_policyBase_0(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(IntPtr a, uint b, uint c, uint d, uint e, uint f, IntPtr g, IntPtr h, AsyncCallback callback, object object)`
- `IntPtr EndInvoke(IAsyncResult result)`
- `IntPtr Invoke(IntPtr a, uint b, uint c, uint d, uint e, uint f, IntPtr g, IntPtr h)`

## pagmo.r_policyBase+SwigDelegater_policyBase_1

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegater_policyBase_1(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `string EndInvoke(IAsyncResult result)`
- `string Invoke()`

## pagmo.r_policyBase+SwigDelegater_policyBase_2

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegater_policyBase_2(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `string EndInvoke(IAsyncResult result)`
- `string Invoke()`

## pagmo.r_policyBase+SwigDelegater_policyBase_3

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegater_policyBase_3(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `bool EndInvoke(IAsyncResult result)`
- `bool Invoke()`

## pagmo.r_policyPagmoWrapper

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `r_policyPagmoWrapper()`
- `r_policyPagmoWrapper(r_policyBase base_)`
- `r_policyPagmoWrapper(r_policyPagmoWrapper arg0)`

### Methods

- `void Dispose()`
- `r_policyBase getBasePolicy()`
- `string get_extra_info()`
- `string get_name()`
- `bool is_valid()`
- `void setBasePolicy(r_policyBase b)`

## pagmo.random_device

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `random_device()`

### Methods

- `void Dispose()`
- `uint next()`
- `void set_seed(uint seed)`

## pagmo.rastrigin

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/rastrigin.html

### Constructors

- `rastrigin()`
- `rastrigin(uint dim)`

### Properties

- `uint m_dim { get; set }`

### Methods

- `void Dispose()`
- `DoubleVector best_known()`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `DoubleVector gradient(DoubleVector arg0)`
- `bool has_batch_fitness()`
- `VectorOfVectorOfDoubles hessians(DoubleVector arg0)`
- `problem to_problem()`

## pagmo.ring

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/topologies/ring.html

### Constructors

- `ring()`
- `ring(double arg0)`
- `ring(uint arg0, double arg1)`

### Methods

- `void Dispose()`
- `TopologyConnections get_connections(uint arg0)`
- `string get_name()`
- `double get_weight()`
- `uint num_vertices()`
- `void push_back()`

## pagmo.rosenbrock

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/rosenbrock.html

### Constructors

- `rosenbrock()`
- `rosenbrock(uint dim)`

### Properties

- `uint m_dim { get; set }`

### Methods

- `void Dispose()`
- `DoubleVector best_known()`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `DoubleVector gradient(DoubleVector arg0)`
- `bool has_batch_fitness()`
- `problem to_problem()`

## pagmo.s_policy

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/s__policy.html

### Constructors

- `s_policy()`
- `s_policy(s_policyBase basePolicy)`

## pagmo.s_policyBase

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `s_policyBase()`

### Methods

- `void Dispose()`
- `string get_extra_info()`
- `string get_name()`
- `bool is_valid()`
- `IndividualsGroup select(IndividualsGroup a, uint b, uint c, uint d, uint e, uint f, DoubleVector g)`

## pagmo.s_policyBase+SwigDelegates_policyBase_0

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegates_policyBase_0(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(IntPtr a, uint b, uint c, uint d, uint e, uint f, IntPtr g, AsyncCallback callback, object object)`
- `IntPtr EndInvoke(IAsyncResult result)`
- `IntPtr Invoke(IntPtr a, uint b, uint c, uint d, uint e, uint f, IntPtr g)`

## pagmo.s_policyBase+SwigDelegates_policyBase_1

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegates_policyBase_1(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `string EndInvoke(IAsyncResult result)`
- `string Invoke()`

## pagmo.s_policyBase+SwigDelegates_policyBase_2

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegates_policyBase_2(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `string EndInvoke(IAsyncResult result)`
- `string Invoke()`

## pagmo.s_policyBase+SwigDelegates_policyBase_3

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SwigDelegates_policyBase_3(object object, IntPtr method)`

### Methods

- `IAsyncResult BeginInvoke(AsyncCallback callback, object object)`
- `bool EndInvoke(IAsyncResult result)`
- `bool Invoke()`

## pagmo.s_policyPagmoWrapper

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `s_policyPagmoWrapper()`
- `s_policyPagmoWrapper(s_policyBase base_)`
- `s_policyPagmoWrapper(s_policyPagmoWrapper arg0)`

### Methods

- `void Dispose()`
- `s_policyBase getBasePolicy()`
- `string get_extra_info()`
- `string get_name()`
- `bool is_valid()`
- `void setBasePolicy(s_policyBase b)`

## pagmo.sade

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/sade.html

### Constructors

- `sade()`
- `sade(uint gen)`
- `sade(uint gen, uint variant)`
- `sade(uint gen, uint variant, uint variant_adptv)`
- `sade(uint gen, uint variant, uint variant_adptv, double ftol)`
- `sade(uint gen, uint variant, uint variant_adptv, double ftol, double xtol)`
- `sade(uint gen, uint variant, uint variant_adptv, double ftol, double xtol, bool memory)`
- `sade(uint gen, uint variant, uint variant_adptv, double ftol, double xtol, bool memory, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<SadeLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `uint get_gen()`
- `SadeLogEntryVector get_log_entries()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint arg0)`
- `algorithm to_algorithm()`

## pagmo.sade+SadeLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SadeLogLine(uint Generation, ulong FunctionEvaluations, double BestFitness, double F, double Cr, double Dx, double Df)`

### Properties

- `string AlgorithmName { get }`
- `double BestFitness { get; set }`
- `double Cr { get; set }`
- `double Df { get; set }`
- `double Dx { get; set }`
- `double F { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `uint Generation { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`

### Methods

- `void Deconstruct(out uint Generation, out ulong FunctionEvaluations, out double BestFitness, out double F, out double Cr, out double Dx, out double Df)`
- `bool Equals(object obj)`
- `bool Equals(SadeLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.schwefel

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/schwefel.html

### Constructors

- `schwefel()`
- `schwefel(uint dim)`

### Properties

- `uint m_dim { get; set }`

### Methods

- `void Dispose()`
- `DoubleVector best_known()`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `bool has_batch_fitness()`
- `problem to_problem()`

## pagmo.sea

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/sea.html

### Constructors

- `sea()`
- `sea(uint gen)`
- `sea(uint gen, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<SeaLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `SeaLogEntryVector get_log_entries()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint level)`
- `algorithm to_algorithm()`

## pagmo.sea+SeaLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SeaLogLine(uint Generation, ulong FunctionEvaluations, double BestFitness, double Improvement, ulong OffspringEvaluations)`

### Properties

- `string AlgorithmName { get }`
- `double BestFitness { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `uint Generation { get; set }`
- `double Improvement { get; set }`
- `ulong OffspringEvaluations { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`

### Methods

- `void Deconstruct(out uint Generation, out ulong FunctionEvaluations, out double BestFitness, out double Improvement, out ulong OffspringEvaluations)`
- `bool Equals(object obj)`
- `bool Equals(SeaLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.select_best

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `select_best()`

### Methods

- `void Dispose()`
- `string get_extra_info()`
- `string get_name()`
- `IndividualsGroup select(IndividualsGroup a, uint b, uint c, uint d, uint e, uint f, DoubleVector g)`

## pagmo.sga

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/sga.html

### Constructors

- `sga()`
- `sga(uint gen)`
- `sga(uint gen, double cr)`
- `sga(uint gen, double cr, double eta_c)`
- `sga(uint gen, double cr, double eta_c, double m)`
- `sga(uint gen, double cr, double eta_c, double m, double param_m)`
- `sga(uint gen, double cr, double eta_c, double m, double param_m, uint param_s)`
- `sga(uint gen, double cr, double eta_c, double m, double param_m, uint param_s, string crossover)`
- `sga(uint gen, double cr, double eta_c, double m, double param_m, uint param_s, string crossover, string mutation)`
- `sga(uint gen, double cr, double eta_c, double m, double param_m, uint param_s, string crossover, string mutation, string selection)`
- `sga(uint gen, double cr, double eta_c, double m, double param_m, uint param_s, string crossover, string mutation, string selection, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<SgaLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `SgaLogEntryVector get_log_entries()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint level)`
- `algorithm to_algorithm()`

## pagmo.sga+SgaLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SgaLogLine(uint Generation, ulong FunctionEvaluations, double BestFitness, double Improvement)`

### Properties

- `string AlgorithmName { get }`
- `double BestFitness { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `uint Generation { get; set }`
- `double Improvement { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`

### Methods

- `void Deconstruct(out uint Generation, out ulong FunctionEvaluations, out double BestFitness, out double Improvement)`
- `bool Equals(object obj)`
- `bool Equals(SgaLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.sga_crossover

- Kind: enum
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Values

- EXPONENTIAL
- BINOMIAL
- SINGLE
- SBX

## pagmo.sga_mutation

- Kind: enum
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Values

- GAUSSIAN
- UNIFORM
- POLYNOMIAL

## pagmo.sga_selection

- Kind: enum
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Values

- TOURNAMENT
- TRUNCATED

## pagmo.simulated_annealing

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/simulated_annealing.html

### Constructors

- `simulated_annealing()`
- `simulated_annealing(double Ts)`
- `simulated_annealing(double Ts, double Tf)`
- `simulated_annealing(double Ts, double Tf, uint n_T_adj)`
- `simulated_annealing(double Ts, double Tf, uint n_T_adj, uint n_range_adj)`
- `simulated_annealing(double Ts, double Tf, uint n_T_adj, uint n_range_adj, uint bin_size)`
- `simulated_annealing(double Ts, double Tf, uint n_T_adj, uint n_range_adj, uint bin_size, double start_range)`
- `simulated_annealing(double Ts, double Tf, uint n_T_adj, uint n_range_adj, uint bin_size, double start_range, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<SimulatedAnnealingLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `SimulatedAnnealingLogEntryVector get_log_entries()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint level)`
- `algorithm to_algorithm()`

## pagmo.simulated_annealing+SimulatedAnnealingLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `SimulatedAnnealingLogLine(ulong FunctionEvaluations, double BestFitness, double CurrentFitness, double Temperature, double MoveRange)`

### Properties

- `string AlgorithmName { get }`
- `double BestFitness { get; set }`
- `double CurrentFitness { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `double MoveRange { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`
- `double Temperature { get; set }`

### Methods

- `void Deconstruct(out ulong FunctionEvaluations, out double BestFitness, out double CurrentFitness, out double Temperature, out double MoveRange)`
- `bool Equals(object obj)`
- `bool Equals(SimulatedAnnealingLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.thread_bfe

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/batch_evaluators/thread_bfe.html

### Constructors

- `thread_bfe()`
- `thread_bfe(thread_bfe arg0)`

### Methods

- `void Dispose()`
- `DoubleVector Operator(IProblem problem, DoubleVector batchX)`
- `string get_name()`
- `bfe to_bfe()`

## pagmo.thread_island

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `thread_island()`

### Methods

- `void Dispose()`
- `string get_extra_info()`
- `string get_name()`
- `void run_evolve(island isl)`

## pagmo.thread_safety

- Kind: enum
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Values

- none
- basic
- constant

## pagmo.topology

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/topology.html

### Constructors

- `topology(topology arg0)`

### Methods

- `void Dispose()`
- `TopologyConnections get_connections(uint arg0)`
- `string get_extra_info()`
- `string get_name()`
- `IntPtr get_ptr()`
- `bool is_valid()`
- `void push_back()`
- `void push_back(uint arg0)`

## pagmo.translate

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/translate.html

### Constructors

- `translate()`
- `translate(problem arg0, DoubleVector arg1)`

### Methods

- `static translate Create(IProblem innerProblem, DoubleVector translationVector)`
- `static translate Create(IProblem innerProblem, double[] translationVector)`
- `void Dispose()`
- `DoubleVector batch_fitness(DoubleVector arg0)`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_extra_info()`
- `problem get_inner_problem()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `DoubleVector get_translation()`
- `bool has_batch_fitness()`
- `bool has_set_seed()`
- `void set_seed(uint arg0)`
- `problem to_problem()`

## pagmo.unconnected

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/topologies/unconnected.html

### Constructors

- `unconnected()`

### Methods

- `void Dispose()`
- `TopologyConnections get_connections(uint arg0)`
- `string get_name()`
- `void push_back()`

## pagmo.unconstrain

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/unconstrain.html

### Constructors

- `unconstrain()`
- `unconstrain(problem arg0)`
- `unconstrain(problem arg0, string method)`
- `unconstrain(problem arg0, string method, DoubleVector weights)`

### Methods

- `static unconstrain Create(IProblem innerProblem, string method)`
- `static unconstrain Create(IProblem innerProblem, string method, DoubleVector weights)`
- `static unconstrain Create(IProblem innerProblem, string method, double[] weights)`
- `void Dispose()`
- `DoubleVector batch_fitness(DoubleVector arg0)`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_extra_info()`
- `problem get_inner_problem()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `bool has_batch_fitness()`
- `bool has_set_seed()`
- `void set_seed(uint arg0)`
- `problem to_problem()`

## pagmo.wfg

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/wfg.html

### Constructors

- `wfg()`
- `wfg(uint prob_id)`
- `wfg(uint prob_id, uint dim_dvs)`
- `wfg(uint prob_id, uint dim_dvs, uint dim_obj)`
- `wfg(uint prob_id, uint dim_dvs, uint dim_obj, uint dim_k)`

### Methods

- `void Dispose()`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `bool has_batch_fitness()`
- `problem to_problem()`

## pagmo.xnes

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/algorithms/xnes.html

### Constructors

- `xnes()`
- `xnes(uint gen)`
- `xnes(uint gen, double eta_mu)`
- `xnes(uint gen, double eta_mu, double eta_sigma)`
- `xnes(uint gen, double eta_mu, double eta_sigma, double eta_b)`
- `xnes(uint gen, double eta_mu, double eta_sigma, double eta_b, double sigma0)`
- `xnes(uint gen, double eta_mu, double eta_sigma, double eta_b, double sigma0, double ftol)`
- `xnes(uint gen, double eta_mu, double eta_sigma, double eta_b, double sigma0, double ftol, double xtol)`
- `xnes(uint gen, double eta_mu, double eta_sigma, double eta_b, double sigma0, double ftol, double xtol, bool memory)`
- `xnes(uint gen, double eta_mu, double eta_sigma, double eta_b, double sigma0, double ftol, double xtol, bool memory, bool force_bounds)`
- `xnes(uint gen, double eta_mu, double eta_sigma, double eta_b, double sigma0, double ftol, double xtol, bool memory, bool force_bounds, uint seed)`

### Methods

- `void Dispose()`
- `IReadOnlyList<IAlgorithmLogLine> GetLogLines()`
- `IReadOnlyList<XnesLogLine> GetTypedLogLines()`
- `population evolve(population arg0)`
- `string get_extra_info()`
- `uint get_gen()`
- `XnesLogEntryVector get_log_entries()`
- `string get_name()`
- `uint get_seed()`
- `uint get_verbosity()`
- `void set_seed(uint arg0)`
- `void set_verbosity(uint level)`
- `algorithm to_algorithm()`

## pagmo.xnes+XnesLogLine

- Kind: struct
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html

### Constructors

- `XnesLogLine(uint Generation, ulong FunctionEvaluations, double BestFitness, double Dx, double Df, double Sigma)`

### Properties

- `string AlgorithmName { get }`
- `double BestFitness { get; set }`
- `double Df { get; set }`
- `double Dx { get; set }`
- `ulong FunctionEvaluations { get; set }`
- `uint Generation { get; set }`
- `IReadOnlyDictionary<string, object> RawFields { get }`
- `double Sigma { get; set }`

### Methods

- `void Deconstruct(out uint Generation, out ulong FunctionEvaluations, out double BestFitness, out double Dx, out double Df, out double Sigma)`
- `bool Equals(object obj)`
- `bool Equals(XnesLogLine other)`
- `int GetHashCode()`
- `string ToDisplayString()`
- `string ToString()`

## pagmo.zdt

- Kind: class
- Upstream reference: https://esa.github.io/pagmo2/docs/cpp/problems/zdt.html

### Constructors

- `zdt()`
- `zdt(uint prob_id)`
- `zdt(uint prob_id, uint param)`

### Methods

- `void Dispose()`
- `DoubleVector fitness(DoubleVector arg0)`
- `PairOfDoubleVectors get_bounds()`
- `string get_name()`
- `uint get_nec()`
- `uint get_nic()`
- `uint get_nix()`
- `uint get_nobj()`
- `int get_thread_safety()`
- `bool has_batch_fitness()`
- `double p_distance(population arg0)`
- `double p_distance(DoubleVector arg0)`
- `problem to_problem()`

