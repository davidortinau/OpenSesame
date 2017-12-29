namespace OpenSesame.Models
{
	public interface IAppModel
	{
		string AuthToken { get; set; }
		string DeviceID { get; set; }
	}
}