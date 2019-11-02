namespace WebTesting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Testing")]
    public partial class Testing
    {
        public int TestingId { get; set; }

        [Required]
        [StringLength(100)]
        public string SessionId { get; set; }

        public int QuestionId { get; set; }

        public int UserAnswer { get; set; }

        public bool AnswerCorrectly { get; set; }

        public virtual Question Question { get; set; }
    }
}
