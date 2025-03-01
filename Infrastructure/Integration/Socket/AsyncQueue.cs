namespace Infrastructure.Integration.Socket {
    public class AsyncQueue<T> {
        private readonly List<T> _queue = new List<T>();
        private TaskCompletionSource<bool> _queueIsNoLongerEmpty = new TaskCompletionSource<bool>();

        public void enqueue(T data) {
            lock (_queue) {
                _queue.Add(data);
                if (_queue.Count == 1) {
                    _queueIsNoLongerEmpty.SetResult(true);
                }
            }
        }

        public void enqueueIfEmpty(T data) {
            lock (_queue) {
                if (_queue.Count > 0) return;
                enqueue(data);
            }
        }

        public async Task<T[]> dequeueAsync() {
            await _queueIsNoLongerEmpty.Task;

            T[] result;

            lock (_queue) {
                if (_queue.Count == 0) {
                    // something went very wrong
                    throw new InvalidOperationException(
                        "Logical error: queue should not be empty when _queueIsNoLongerEmpty is finished");
                }

                result = _queue.ToArray();
                _queue.Clear();

                _queueIsNoLongerEmpty = new TaskCompletionSource<bool>();
            }

            return result;
        }
    }
}
