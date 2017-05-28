namespace Repairis.Configuration
{
    public class SmsSettings
    {
        public bool IsEnabled { get; set; }
        public string Sid { get; set; }
        public string Token { get; set; }
        public string BaseUri { get; set; }
        public string RequestUri { get; set; }
        public string From { get; set; }
    }
}
