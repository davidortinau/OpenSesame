using System;
using System.Threading.Tasks;
using OpenSesame.Models;

namespace OpenSesame.Services
{
	public interface IBingSpeechService
	{
		Task<SpeechResult> RecognizeSpeechAsync(string filename);
	}
}
