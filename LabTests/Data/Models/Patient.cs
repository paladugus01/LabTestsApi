using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace LabTests.Data.Models
{
    public class Patient
    {
        public Patient()
        {

        }

        #region EF_Properties
        /// <summary>
        /// The Creator of Patient in the system
        /// </summary>
        [ForeignKey("CreatedByUserId")]
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// List of all the LabReports related to this patient
        /// </summary>
        public virtual List<LabReport> LabReports { get; set; }

        #endregion

        #region Properties
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Required]
        public PatientGender Gender { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }

        public Guid CreatedByUserId { get; set; } //Created By user
        #endregion
    }
}
