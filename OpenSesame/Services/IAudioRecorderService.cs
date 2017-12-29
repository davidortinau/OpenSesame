using System;
namespace OpenSesame.Services
{
	public interface IAudioRecorderService
	{
		void StartRecording();
		void StopRecording();
	}
}
