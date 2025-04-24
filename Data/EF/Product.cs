namespace Data.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        public long Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string MetaTitle { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        public decimal? Price { get; set; }

        public int? Promotion { get; set; }

        public decimal? PromotionPrice { get; set; }

        public long? CategoryId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool Status { get; set; }

        public long? GalleryId { get; set; }

        public string Details { get; set; }

        public decimal? OriginalPrice { get; set; }
    }
}
