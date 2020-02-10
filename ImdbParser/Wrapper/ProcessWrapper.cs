using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace System.Diagnostics {
    /// <summary>Provides access to local and remote processes and enables you to start and stop local system processes.</summary>
    interface IProcess : IDisposable {
        /// <summary>Instructs the <see cref="IProcess" /> component to wait indefinitely for the associated process to exit.</summary>
        /// <exception cref="Win32Exception">The wait setting could not be accessed.</exception>
        /// <exception cref="SystemException">No process <see cref="Process.Id" /> has been set, and a <see cref="Process.Handle" /> from which the <see cref="Process.Id" /> property can be determined
        /// does not exist. -or- There is no process associated with this <see cref="IProcess" /> <see cref="object" />. -or- You are attempting to call <see cref="WaitForExit" /> for a process that
        /// is running on a remote computer. This method is available only for processes that are running on the local computer.</exception>
        void WaitForExit();
    }

    /// <summary>Provides access to local and remote processes and enables you to start and stop local system processes.</summary>
    /// <remarks><see cref="Process" /> wrapper.</remarks>
    [DebuggerStepThrough]
    sealed class ProcessWrapper : IProcess {
        /// <summary><see cref="Process" /> to wrap.</summary>
        readonly Process _process;

        /// <summary>Initialize new <see cref="ProcessWrapper" /> instance.</summary>
        /// <param name="process"><see cref="Process" /> to wrap.</param>
        internal ProcessWrapper(Process process) => _process = process;

        /// <summary>Releases all resources used by the <see cref="ProcessWrapper" />.</summary>
        public void Dispose() => _process.Dispose();

        /// <summary>Instructs the <see cref="IProcess" /> component to wait indefinitely for the associated process to exit.</summary>
        /// <exception cref="Win32Exception">The wait setting could not be accessed.</exception>
        /// <exception cref="SystemException">No process <see cref="Process.Id" /> has been set, and a <see cref="Process.Handle" /> from which the <see cref="Process.Id" /> property can be determined
        /// does not exist. -or- There is no process associated with this <see cref="IProcess" /> <see cref="object" />. -or- You are attempting to call <see cref="WaitForExit" /> for a process that
        /// is running on a remote computer. This method is available only for processes that are running on the local computer.</exception>
        [ExcludeFromCodeCoverage]
        public void WaitForExit() => _process.WaitForExit();
    }
}