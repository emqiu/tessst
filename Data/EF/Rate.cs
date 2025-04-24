namespace Data.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Rate")]
    public partial class Rate
    {
        [StringLength(500)]
        public string Content { get; set; }

        public int Star { get; set; }

        public DateTime CreatedDate { get; set; }

        public long UserId { get; set; }

        public long ProductId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
    }
}
