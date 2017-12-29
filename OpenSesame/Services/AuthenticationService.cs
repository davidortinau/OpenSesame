using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using OpenSesame.Store;
using Flurl.Http;

namespace OpenSesame.Services
{
	public class AuthenticationService : IAuthenticationService
	{
		string subscriptionKey;
		string token;
		Timer accessTokenRenewer;
		const int RefreshTokenDuration = 9;

		public AuthenticationService(string apiKey)
		{
			subscriptionKey = apiKey;
		}

		public async Task InitializeAsync()
		{
			token = await FetchTokenAsync(ApplicationStore.CognitiveServicesEndpoint, subscriptionKey);
			accessTokenRenewer = new Timer(new TimerCallback(OnTokenExpiredCallback), this, TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
		}

		public string GetAccessToken()
		{
			return token;
		}

		async Task RenewAccessToken()
		{
			token = await FetchTokenAsync(ApplicationStore.CognitiveServicesEndpoint, subscriptionKey);
			Debug.WriteLine("Renewed token.");
		}

		async Task OnTokenExpiredCallback(object stateInfo)
		{
			try
			{
				await RenewAccessToken();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(string.Format("Failed to renew access token. Details: {0}", ex.Message));
			}
			finally
			{
				try
				{
					accessTokenRenewer.Change(TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
				}
				catch (Exception ex)
				{
					Debug.WriteLine(string.Format("Failed to reschedule the timer to renew access token. Details: {0}", ex.Message));
				}
			}
		}

		async Task<string> FetchTokenAsync(string fetchUri, string apiKey)
		{
			try{
				var response = await $"{fetchUri}/issueToken"
					.WithHeader("Ocp-Apim-Subscription-Key", apiKey)
					//.AppendPathSegment("issueToken")
					.PostAsync(null)
					.ReceiveString();

				return response;
			}catch (FlurlHttpTimeoutException) {
				//LogError("Request timed out.");
			}
			catch (FlurlHttpException ex) {
				Debug.WriteLine(ex.Message);
			}

			return string.Empty;
		}
	}

	internal delegate Task TimerCallback(object state);

	internal sealed class Timer : IDisposable
	{
		static Task CompletedTask = Task.FromResult(false);

		TimerCallback callback;
		Task delay;
		bool disposed;
		int period;
		object state;
		CancellationTokenSource tokenSource;

		public Timer(TimerCallback callback, object state, int dueTime, int period)
		{
			this.callback = callback;
			this.state = state;
			this.period = period;
			Reset(dueTime);
		}

		public Timer(TimerCallback callback, object state, TimeSpan dueTime, TimeSpan period)
			: this(callback, state, (int)dueTime.TotalMilliseconds, (int)period.TotalMilliseconds)
		{
		}

		~Timer()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		void Dispose(bool cleanUpManagedObjects)
		{
			if (cleanUpManagedObjects)
			{
				Cancel();
			}
			disposed = true;
		}

		public void Change(int dueTime, int period)
		{
			this.period = period;
			Reset(dueTime);
		}

		public void Change(TimeSpan dueTime, TimeSpan period)
		{
			Change((int)dueTime.TotalMilliseconds, (int)period.TotalMilliseconds);
		}

		void Reset(int due)
		{
			Cancel();
			if (due >= 0)
			{
				tokenSource = new CancellationTokenSource();
				Action tick = null;
				tick = () =>
				{
					Task.Run(() => callback(state));
					if (!disposed && period >= 0)
					{
						if (period > 0)
							delay = Task.Delay(period, tokenSource.Token);
						else
							delay.ContinueWith(t => tick(), tokenSource.Token);
					}
					if (due > 0)
						delay = Task.Delay(due, tokenSource.Token);
					else
						delay = CompletedTask;
					delay.ContinueWith(t => tick(), tokenSource.Token);
				};
			}
		}

		void Cancel()
		{
			if (tokenSource != null)
			{
				tokenSource.Cancel();
				tokenSource.Dispose();
				tokenSource = null;
			}
		}
	}
}
