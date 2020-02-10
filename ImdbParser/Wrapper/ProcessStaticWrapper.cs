using System.ComponentModel;
using System.IO;

// ReSharper disable once CheckNamespace
namespace System.Diagnostics {
    /// <summary>Provides access to local and remote processes and enables you to start and stop local system processes.</summary>
    interface IProcessStatic {
        // ReSharper disable once CommentTypo
        /// <summary>Starts the process resource that is specified by the parameter containing process start information (for example, the file name of the process to start) and associates the
        /// resource with a new <see cref="IProcess" /> component.</summary>
        /// <param name="startInfo">The <see cref="ProcessStartInfo" /> that contains the information that is used to start the process, including the file name and any command-line arguments.</param>
        /// <returns>A new <see cref="IProcess" /> that is associated with the process resource, or null if no process resource is started. Note that a new process that's started alongside already
        /// running instances of the same process will be independent from the others. In addition, <see cref="ProcessStaticWrapper.Start" /> may return a non-null <see cref="IProcess" /> with its
        /// <see cref="Process.HasExited" /> property already set to true. In this case, the started process may have activated an existing instance of itself and then exited.</returns>
        /// <exception cref="InvalidOperationException">No file name was specified in the startInfo parameter's <see cref="ProcessStartInfo.FileName" /> property. -or- The
        /// <see cref="ProcessStartInfo.UseShellExecute" /> property of the <paramref name="startInfo" /> parameter is true and the <see cref="ProcessStartInfo.RedirectStandardInput" />,
        /// <see cref="ProcessStartInfo.RedirectStandardOutput" />, or <see cref="ProcessStartInfo.RedirectStandardError" /> property is also true. -or- The
        /// <see cref="ProcessStartInfo.UseShellExecute" /> property of the <paramref name="startInfo" /> parameter is true and the <see cref="ProcessStartInfo.UserName" /> property is not null or
        /// empty or the <see cref="ProcessStartInfo.Password" /> property is not null.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="startInfo" /> parameter is null.</exception>
        /// <exception cref="ObjectDisposedException">The process <see cref="object" /> has already been disposed.</exception>
        /// <exception cref="FileNotFoundException">The file specified in the <paramref name="startInfo" /> parameter's <see cref="ProcessStartInfo.FileName" /> property could not be
        /// found.</exception>
        /// <exception cref="Win32Exception">An error occurred when opening the associated file. -or- The sum of the length of the arguments and the length of the full path to the process exceeds
        /// 2080. The error message associated with this exception can be one of the following: "The data area passed to a system call is too small." or "Access is denied."</exception>
        /// <exception cref="PlatformNotSupportedException">Method not supported on operating systems without shell support such as Nano Server (.NET Core only).</exception>
        IProcess Start(ProcessStartInfo startInfo);
    }

    /// <summary>Provides access to local and remote processes and enables you to start and stop local system processes.</summary>
    /// <remarks><see cref="Process" /> static wrapper.</remarks>
    [DebuggerStepThrough]
    sealed class ProcessStaticWrapper : IProcessStatic {
        // ReSharper disable once CommentTypo
        /// <summary>Starts the process resource that is specified by the parameter containing process start information (for example, the file name of the process to start) and associates the
        /// resource with a new <see cref="IProcess" /> component.</summary>
        /// <param name="startInfo">The <see cref="ProcessStartInfo" /> that contains the information that is used to start the process, including the file name and any command-line arguments.</param>
        /// <returns>A new <see cref="IProcess" /> that is associated with the process resource, or null if no process resource is started. Note that a new process that's started alongside already
        /// running instances of the same process will be independent from the others. In addition, <see cref="Start" /> may return a non-null <see cref="IProcess" /> with its
        /// <see cref="Process.HasExited" /> property already set to true. In this case, the started process may have activated an existing instance of itself and then exited.</returns>
        /// <exception cref="InvalidOperationException">No file name was specified in the startInfo parameter's <see cref="ProcessStartInfo.FileName" /> property. -or- The
        /// <see cref="ProcessStartInfo.UseShellExecute" /> property of the <paramref name="startInfo" /> parameter is true and the <see cref="ProcessStartInfo.RedirectStandardInput" />,
        /// <see cref="ProcessStartInfo.RedirectStandardOutput" />, or <see cref="ProcessStartInfo.RedirectStandardError" /> property is also true. -or- The
        /// <see cref="ProcessStartInfo.UseShellExecute" /> property of the <paramref name="startInfo" /> parameter is true and the <see cref="ProcessStartInfo.UserName" /> property is not null or
        /// empty or the <see cref="ProcessStartInfo.Password" /> property is not null.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="startInfo" /> parameter is null.</exception>
        /// <exception cref="ObjectDisposedException">The process <see cref="object" /> has already been disposed.</exception>
        /// <exception cref="FileNotFoundException">The file specified in the <paramref name="startInfo" /> parameter's <see cref="ProcessStartInfo.FileName" /> property could not be
        /// found.</exception>
        /// <exception cref="Win32Exception">An error occurred when opening the associated file. -or- The sum of the length of the arguments and the length of the full path to the process exceeds
        /// 2080. The error message associated with this exception can be one of the following: "The data area passed to a system call is too small." or "Access is denied."</exception>
        /// <exception cref="PlatformNotSupportedException">Method not supported on operating systems without shell support such as Nano Server (.NET Core only).</exception>
        public IProcess Start(ProcessStartInfo startInfo) => new ProcessWrapper(Process.Start(startInfo));
    }
}
