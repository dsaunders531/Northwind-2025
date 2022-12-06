// <copyright file="ICommand.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace Patterns
{
    /// <summary>
    /// The command pattern.
    /// </summary>
    /// <remarks>When implementing, the constructor contains the parameters.</remarks>
    public interface ICommand
    {
        /// <summary>
        /// Run something.
        /// </summary>
        void Run();

        /// <summary>
        /// Undo what run did.
        /// </summary>
        /// <remarks>Throw an exception if the process does not work correctly.</remarks>
        void Undo();
    }

    /// <summary>
    /// The command pattern.
    /// </summary>
    /// <remarks>When implementing, the constructor contains the parameters.</remarks>
    public interface IAsyncCommand
    {
        /// <summary>
        /// Run something.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task Run();

        /// <summary>
        /// Undo what run did.
        /// </summary>
        /// <remarks>Throw an exception if the process does not work correctly.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task Undo();
    }

    /// <summary>
    /// The command pattern.
    /// </summary>
    /// <remarks>When implementing, the constructor contains the parameters.</remarks>
    public interface ICommand<TOutput>
        where TOutput : class
    {
        /// <summary>
        /// Run something.
        /// </summary>
        /// <returns></returns>
        TOutput Run();

        /// <summary>
        /// Undo what run did.
        /// </summary>
        /// <remarks>Throw an exception if the process does not work correctly.</remarks>
        void Undo();
    }

    /// <summary>
    /// The command pattern.
    /// </summary>
    /// <remarks>When implementing, the constructor contains the parameters.</remarks>
    public interface IAsyncCommand<TOutput>
        where TOutput : class
    {
        /// <summary>
        /// Run something.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<TOutput> Run();

        /// <summary>
        /// Undo what run did.
        /// </summary>
        /// <remarks>Throw an exception if the process does not work correctly.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task Undo();
    }
}
