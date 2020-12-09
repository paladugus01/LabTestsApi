using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace LabTests.Data.Models
{
    public class LabReport
    {
        #region Constructor
        public LabReport()
        {

        }
        #endregion

        #region EF_Properties
        /// <summary>
        /// The related Patient
        /// </summary>
        #endregion
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual ApplicationUser User { get; set; }

        #region Properties

        [Key]
        [Required]
        public Guid Id { get; set; }

        public Guid PatientId { get; set; }

        [Required]
        public LabTestResult Result { get; set; }

        [Required]
        public LabReportStatus Status { get; set; }

        [Required]
        public string Physician { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }

        [Required]
        public string LabReportType { get; set; }

        [Required]
        public DateTime LabTestTime { get; set; }

        public Guid CreatedByUserId { get; set; }
        #endregion

    }
}
