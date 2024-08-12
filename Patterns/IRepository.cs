namespace Patterns
{
    /// <summary>
    /// The IRepository interface
    /// </summary>
    /// <typeparam name="T"></typeparam>   
    /// <typeparam name="TId"></typeparam>
    public interface IRepository<T, TId>
        where T : class
    {
        /// <summary>
        /// Get a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> Fetch(TId id);

        /// <summary>
        /// Get several records as a result of a query.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<T[]> Fetch(Func<T, bool> predicate);

        /// <summary>
        /// Update a record.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        Task<bool> Update(T record);

        /// <summary>
        /// Create and save the record.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        Task<T> Create(T record);

        /// <summary>
        /// Delete record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(TId id);
    }
}
