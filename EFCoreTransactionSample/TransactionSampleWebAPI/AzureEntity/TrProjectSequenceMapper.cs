using System;
using System.Collections.Generic;

namespace TransactionSampleWebAPI.AzureEntity
{
    public partial class TrProjectSequenceMapper
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public int SequenceId { get; set; }
        public bool IsActive { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
