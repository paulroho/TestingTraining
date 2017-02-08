using System;

namespace Foerder.Domain
{
    public class Foerderbewilligung
    {
        public Foerdermittelfreigabe Freigabe { get; set; }
        public DateTime? BewilligtVon { get; set; }
        public DateTime? BewilligtBis { get; set; }
    }
}