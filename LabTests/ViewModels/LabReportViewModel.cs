using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace LabTests.ViewModels
{
    public class LabReportViewModel
    {
        #region Constructor
        public LabReportViewModel()
        {

        }
        #endregion

        #region Properties
        public Guid Id{ get; set; }
        public Guid PatientId { get; set; }
        public LabTestResult Result{ get; set; }
        public LabReportStatus Status { get; set; }
        public string Physician { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string  LabReportType{ get; set; }
        public DateTime LabTestTime { get; set; }
        public Guid CreatedByUserId { get; set; }
        #endregion
    }
}
