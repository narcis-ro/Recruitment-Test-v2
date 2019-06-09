namespace JG.Infrastructure.Logging.Configuration
{
    public class JgLoggingConfiguration
    {
        public bool WriteToSeq { get; set; } = true;
        public bool WriteToColoredConsole { get; set; } = true;
        public bool WriteToFile { get; set; } = true;
    }
}
