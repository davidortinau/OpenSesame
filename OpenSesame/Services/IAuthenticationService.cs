using System;
using System.Threading.Tasks;

namespace OpenSesame.Services
{
	public interface IAuthenticationService
	{
		Task InitializeAsync();
		string GetAccessToken();
	}
}
