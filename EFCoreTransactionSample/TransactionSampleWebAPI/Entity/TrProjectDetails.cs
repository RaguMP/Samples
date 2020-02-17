using System;
using System.Collections.Generic;

namespace TransactionSampleWebAPI.Entity
{
    public partial class TrProjectDetails
    {
        public TrProjectDetails()
        {
            TrProjectMapper = new HashSet<TrProjectMapper>();
        }

        public int Id { get; set; }
        public int BrandId { get; set; }
        public int BrandProjectId { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<TrProjectMapper> TrProjectMapper { get; set; }
    }
}
