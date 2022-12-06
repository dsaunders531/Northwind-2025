namespace Patterns
{
    /// <summary>
    /// The command pattern.
    /// </summary>
    /// <remarks>When implementing, the constructor contains the parameters.</remarks>    
    public interface ICommand
    {
        /// <summary>
        /// Run something
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
        /// Run something
        /// </summary>
        Task Run();

        /// <summary>
        /// Undo what run did.
        /// </summary>
        /// <remarks>Throw an exception if the process does not work correctly.</remarks>
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
        /// Run something
        /// </summary>
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
        /// Run something
        /// </summary>
        Task<TOutput> Run();

        /// <summary>
        /// Undo what run did.
        /// </summary>
        /// <remarks>Throw an exception if the process does not work correctly.</remarks>
        Task Undo();
    }
}
