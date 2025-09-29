using Microsoft.VisualStudio.TestTools.UnitTesting;

// Configure test parallelization - disable parallel execution due to file operations
[assembly: Parallelize(Workers = 1, Scope = ExecutionScope.MethodLevel)]