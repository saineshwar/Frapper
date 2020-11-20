using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frapper.Entities.ProfileImage
{
    [Table("ProfileImageProperty")]
    public class ProfileImageProperty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProfileImageId { get; set; }
        public string ImageName { get; set; }
        public string ImageType { get; set; }
        public string Medium { get; set; }
        public string Small { get; set; }
        public string Tiny { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public bool? Status { get; set; }
    }

}