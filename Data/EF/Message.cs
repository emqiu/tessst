using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF
{
    [Table("Message")]
    public partial class Message
    {
        public long Id { get; set; }

        [StringLength(100)]
        public string Sender { get; set; }

        [StringLength(100)]
        public string Receiver { get; set; }

        public string MessageContent { get; set; }
        public DateTime? Timestamp { get; set; }

    }
}
