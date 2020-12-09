using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabTests.ViewModels
{
    public class PatientViewModel
    {
        #region Constructor
        public PatientViewModel()
        {
          
        }
        #endregion

        #region Properties
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public PatientGender Gender { get; set; }
        public string Address{ get; set; }
        public DateTime CreatedDate{ get; set; }
        public DateTime LastModifiedDate{ get; set; }
        #endregion
    }
}
