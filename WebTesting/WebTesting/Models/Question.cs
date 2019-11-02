namespace WebTesting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Question")]
    public partial class Question
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question()
        {
            Testings = new HashSet<Testing>();
        }

        public int QuestionId { get; set; }

        public int TestId { get; set; }

        [Required]
        [StringLength(400)]
        public string QuestionText { get; set; }

        [Required]
        [StringLength(400)]
        public string Answer1 { get; set; }

        [Required]
        [StringLength(400)]
        public string Answer2 { get; set; }

        [Required]
        [StringLength(400)]
        public string Answer3 { get; set; }

        public int CorrectAnswer { get; set; }

        public int QuestionNumber { get; set; }

        public virtual Test Test { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Testing> Testings { get; set; }
    }
}
