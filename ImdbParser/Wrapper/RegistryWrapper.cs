using System;
using System.Diagnostics;
using System.IO;
using System.Security;

// ReSharper disable once CheckNamespace
namespace Microsoft.Win32 {
    /// <summary>Provides <see cref="RegistryKey" /> <see cref="object" />s that represent the root keys in the Windows registry, and methods to access key/value pairs.</summary>
    interface IRegistry {
        // ReSharper disable CommentTypo
        /// <summary>Retrieves the value associated with the specified name, in the specified registry key. If the name is not found in the specified key, returns a default value that you provide, or
        /// null if the specified key does not exist.</summary>
        /// <param name="keyName">The full registry path of the key, beginning with a valid registry root, such as "HKEY_CURRENT_USER".</param>
        /// <param name="valueName">The name of the name/value pair.</param>
        /// <param name="defaultValue">The value to return if valueName does not exist.</param>
        /// <returns>null if the subkey specified by <paramref name="keyName" /> does not exist; otherwise, the value associated with <paramref name="valueName" />, or <paramref name="defaultValue" />
        /// if <paramref name="valueName" /> is not found.</returns>
        /// <exception cref="SecurityException">The user does not have the permissions required to read from the registry key.</exception>
        /// <exception cref="IOException">The <see cref="RegistryKey" /> that contains the specified value has been marked for deletion.</exception>
        /// <exception cref="ArgumentException"><paramref name="keyName" /> does not begin with a valid registry root.</exception>
        // ReSharper restore CommentTypo
        object GetValue(string keyName, string valueName, object? defaultValue);
    }

    /// <summary>Provides <see cref="RegistryKey" /> <see cref="object" />s that represent the root keys in the Windows registry, and methods to access key/value pairs.</summary>
    /// <remarks><see cref="Registry" /> wrapper.</remarks>
    [DebuggerStepThrough]
    sealed class RegistryWrapper : IRegistry {
        // ReSharper disable CommentTypo
        /// <summary>Retrieves the value associated with the specified name, in the specified registry key. If the name is not found in the specified key, returns a default value that you provide, or
        /// null if the specified key does not exist.</summary>
        /// <param name="keyName">The full registry path of the key, beginning with a valid registry root, such as "HKEY_CURRENT_USER".</param>
        /// <param name="valueName">The name of the name/value pair.</param>
        /// <param name="defaultValue">The value to return if valueName does not exist.</param>
        /// <returns>null if the subkey specified by <paramref name="keyName" /> does not exist; otherwise, the value associated with <paramref name="valueName" />, or <paramref name="defaultValue" />
        /// if <paramref name="valueName" /> is not found.</returns>
        /// <exception cref="SecurityException">The user does not have the permissions required to read from the registry key.</exception>
        /// <exception cref="IOException">The <see cref="RegistryKey" /> that contains the specified value has been marked for deletion.</exception>
        /// <exception cref="ArgumentException"><paramref name="keyName" /> does not begin with a valid registry root.</exception>
        // ReSharper restore CommentTypo
        public object GetValue(string keyName, string valueName, object? defaultValue) => Registry.GetValue(keyName, valueName, defaultValue);
    }
}