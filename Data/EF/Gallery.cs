namespace Data.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Gallery")]
    public partial class Gallery
    {
        public long Id { get; set; }

        [StringLength(250)]
        public string Image1 { get; set; }

        [StringLength(250)]
        public string Image2 { get; set; }

        [StringLength(250)]
        public string Image3 { get; set; }
    }
}
