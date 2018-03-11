using System;
using System.Threading;
using System.Threading.Tasks;

namespace CmdMessenger {
    /// <summary>
    /// Extension methods for tasks
    /// </summary>
    public static class TaskExtension {
        /// <summary>
        /// Cancels the task when the cancellation token times out
        /// </summary>
        /// <typeparam name="T">The type for the task</typeparam>
        /// <param name="task">The task to add the cancellation token to</param>
        /// <param name="cancellationToken">Cancellation token with time out</param>
        /// <returns>The completed or cancelled task</returns>
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken) {
            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(
                        s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
                if (task != await Task.WhenAny(task, tcs.Task))
                    throw new OperationCanceledException(cancellationToken);
            return await task;
        }
    }
}
