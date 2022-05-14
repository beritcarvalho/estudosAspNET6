namespace Blog
{
    public static class Configuration
    {
        public static string JwtKey = "QwerAsdfZxcvqwersadfxzcvqwerasdfxzcvqwerasdfzxcv";
        public static string ApiKeyName = "api_key";
        public static string ApiKey = "asdfasdfasdfjlkjlkjlkjlkj";
        public static SmtpConfiguration Smtp = new();

        public class SmtpConfiguration
        {
            public string Host { get; set; }
            public int Port { get; set; } = 25;
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}


