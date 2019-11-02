namespace WebTesting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TestResult")]
    public partial class TestResult
    {
        [Key]
        [StringLength(100)]
        public string SessionId { get; set; }

        public int TestId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        public DateTime FinishTime { get; set; }

        public decimal WonPoints { get; set; }

        public virtual Test Test { get; set; }
    }
}
