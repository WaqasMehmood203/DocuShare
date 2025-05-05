using DMS.Backend.Models.Entities;

namespace DMS.Backend.API.Models.Entities
{
    public class Tag
    {
            public Guid Id { get; set; }
            public string Name { get; set; }

            public ICollection<DocumentTag> DocumentTags { get; set; }
    }
}

