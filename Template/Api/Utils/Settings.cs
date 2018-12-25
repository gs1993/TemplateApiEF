namespace Api.Utils
{
    public class Settings
    {
        public string Secret { get; set; }
        public string ConnectionString { get; set; }


        public const string ClientAppUrl = "http://localhost:4200"; //TODO: Move to appsettings.json
    }
}
