using System;
using System.Collections.Generic;

namespace TransactionSampleWebAPI.AzureEntity
{
    public partial class TrProjectMapper
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public bool IsActive { get; set; }

        public virtual TrProjectDetails Project { get; set; }
    }
}
