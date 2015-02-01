using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Porter.Diagnostics.Decorator
{
	internal class ThreadDispatcher : IDisposable
	{
		private sealed class ThreadSafeQueue
		{
			private readonly Queue<Tuple<Func<object>, TaskCompletionSource<object>>> _queue = new Queue<Tuple<Func<object>, TaskCompletionSource<object>>>();
			private readonly object _lock = new object();
			public void Enqueue(Func<object> func, TaskCompletionSource<object> tcs)
			{
				lock (_lock)
				{
					_queue.Enqueue(new Tuple<Func<object>, TaskCompletionSource<object>>(func, tcs));
				}
			}

			public Tuple<Func<object>, TaskCompletionSource<object>> Dequeue()
			{
				lock (_lock)
				{
					return _queue.Dequeue();
				}
			}
		}

		private readonly Semaphore _semaphore = new Semaphore(0, Int32.MaxValue);

		private ThreadSafeQueue _queue = new ThreadSafeQueue();
		private bool _loop = true;
		private readonly Thread _thread;

		public ThreadDispatcher()
		{
			_thread = new Thread(Loop)
			{
				IsBackground = true,
				Name = "Thread Dispatcher "+ Guid.NewGuid().ToString()
			};
			_thread.SetApartmentState(ApartmentState.STA);
			_thread.Start();
		}

		private void Loop()
		{
			while (_loop)
			{
				_semaphore.WaitOne();
				var data = _queue.Dequeue();
				try
				{
					data.Item2.SetResult(data.Item1());
				}
				catch (Exception e)
				{
					data.Item2.SetException(e);
				}
			}
		}

		public Task<T> ProcessAsync<T>(Func<T> func) 
		{
			if (Thread.CurrentThread.ManagedThreadId == _thread.ManagedThreadId)
			{
				var result = func();
				return Task.FromResult(result);
			}

			var tcs = new TaskCompletionSource<object>();
			var task = tcs.Task.ContinueWith(p => Result<T>(p));
			_queue.Enqueue(() => (object)func(), tcs);
			_semaphore.Release();

			return task;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsCurrentThread()
		{
			return Thread.CurrentThread.ManagedThreadId == _thread.ManagedThreadId;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Process<T>(Func<T> func)
		{
			if (Thread.CurrentThread.ManagedThreadId == _thread.ManagedThreadId)
			{
				return func();
			}

			return ProcessAsync(func).Result;
		}

		private static T Result<T>(Task<object> p)
		{
			return (T)p.Result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Process(Action func)
		{
			Process<object>(() =>
			{
				func();
				return null;
			});
		}

		public void Dispose()
		{
			_loop = false;
			_thread.Abort();
			((IDisposable)_semaphore).Dispose();
		}
	}
}