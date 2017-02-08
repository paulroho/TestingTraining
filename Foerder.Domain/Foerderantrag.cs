namespace Foerder.Domain
{
    public class Foerderantrag
    {
        public Foerderbewilligung Bewilligung { get; set; }
        public string DataSource { get; set; }
        public string Status { get; set; }
    }
}