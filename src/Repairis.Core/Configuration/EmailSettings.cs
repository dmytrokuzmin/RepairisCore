namespace Repairis.Configuration
{
    public class EmailSettings
    {
        public bool IsEnabled { get; set; }
        public string ApiKey { get; set; }
        public string ApiBaseUri { get; set; }
        public string RequestUri { get; set; }
        public string From { get; set; }
    }
}
