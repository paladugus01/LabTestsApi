using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LabTests.Data.Models
{
    public class ApplicationUser
    {
        public ApplicationUser()
        {

        }

        /// <summary>
        /// List of patients created by this User
        /// </summary>
        public virtual List<Patient> Patients { get; set; }

        /// <summary>
        /// List of Lab Reports created by this User
        /// </summary>
        public virtual List<LabReport> LabReports { get; set; }

        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        public string DisplayName { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public int Flags { get; set; }

        [Required]
        public DateTime CreatedDateTime { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }

    }

}
