namespace Entities.LinkModels
{
    public class LinkResourceBase
    {
        public LinkResourceBase()
        {
            
        }
        public List<Link> Links { get; set; } = new List<Link>(); // en çok yapılan hata ref tipli prop ın ınstance'ını vermemek
    }
}
