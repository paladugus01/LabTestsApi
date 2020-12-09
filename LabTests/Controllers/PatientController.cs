using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabTests.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Mapster;
using LabTests.Data;
using LabTests.Data.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace LabTests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        #region PrivateFields
        private ApplicationDbContext DbContext;
        #endregion

        public PatientController(ApplicationDbContext context)
        {
            DbContext = context;

        }
        // GET: api/Patient
        /// <summary>
        /// GET: api/Patient
        /// Retrieves {num} of patients
        /// </summary>
        /// <param name="num">The number of patients to retrieve</param>
        /// <returns>{num} patients</returns>
        [HttpGet("{num:int?}")]
        public IActionResult Get(int num =10)
        {
            var patients = DbContext.Patients.Take(num).ToArray();

            if (patients == null || patients.Length == 0)
            {
                return NotFound(new
                {
                    message = String.Format("There are no patients in the system.")
                });
            }

            return new JsonResult(patients.Adapt<PatientViewModel[]>(), new JsonSerializerOptions { 
                WriteIndented = true
            });
        }

        // GET: api/Patient/5
        /// <summary>
        /// Retrieves single patient with given ID or returns nul if not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Null or Patient with {id} </returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var patients = DbContext.Patients.Where(p => p.Id.Equals(id)).FirstOrDefault();

            if (patients == null)
            {
                return NotFound(new
                {
                    message = $"There are no patients in the system with ID = {id}."
                });
            }

            return new JsonResult(patients.Adapt<PatientViewModel>(), new JsonSerializerOptions
            {
                WriteIndented = true
            }); 
        }

        // POST: api/Patient
        ///<summary>
        /// Update a patient to the database
        ///</summary>
        ///<param name="m">The Patient view model containing the data </param>

        [HttpPost]
        public IActionResult Post([FromBody] PatientViewModel m)
        {
            // return generic HTTP Status 500 if client payload is invalid
            if (m == null) return new StatusCodeResult(500);

            //retrieve the patient to edit
            var patient = DbContext.Patients.Where(q => q.Id == m.Id).FirstOrDefault();

            //handle if the patient does not exist

            if( patient == null)
            {
                return NotFound(new
                {
                    Error = $"Patient ID {m.Id} is not found"
                });
            }

            //properties taken from request
            patient.Name = m.Name;
            patient.DOB = m.DOB;
            patient.Gender = m.Gender;
            patient.Address = m.Address;

            //set on serverside
            patient.LastModifiedDate = DateTime.Now;

            //persist to DB
            DbContext.SaveChanges();

            // return updated patient back to client

            return new JsonResult(patient.Adapt<PatientViewModel>(),
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });
        }

        // PUT: api/Patient
        ///<summary>
        /// Add a patient to the database
        ///</summary>
        ///<param name="m">The Patient view model containing the data </param>

        [HttpPut]
        public IActionResult Put([FromBody] PatientViewModel m)
        {
            // return generic HTTP Status 500 if client payload is invalid
            if (m == null) return new StatusCodeResult(500);

            //handling the insert 
            var patient = new Patient();

            //properties taken from request
            patient.Name = m.Name;
            patient.DOB = m.DOB;
            patient.Gender = m.Gender;
            patient.Address = m.Address;

            //properties set from server-side
            //TODO : Change when user module is introduced
            patient.Id = Guid.NewGuid();
            patient.CreatedDate = DateTime.Now;
            patient.LastModifiedDate = DateTime.Now;

            //Adding the admin as foreign key
            patient.CreatedByUserId = DbContext.Users.Where(u => u.UserName == "Admin")
                   .FirstOrDefault().Id;

            //adding the patient to the database
            DbContext.Patients.Add(patient);

            //persist to DB
            DbContext.SaveChanges();

            // return created patient back to client

            return new JsonResult(patient.Adapt<PatientViewModel>(),
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });
        }

        // DELETE: api/ApiWithActions/5
        ///<summary>
        /// Delete a patient from the database
        ///</summary>
        ///<param name="id">The id of the patient to delete</param>

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            // return generic HTTP Status 500 if client payload is invalid
            if (id == null) return new StatusCodeResult(500);

            //retrieve the patient to edit
            var patient = DbContext.Patients.Where(q => q.Id == id).FirstOrDefault();

            //handle if the patient does not exist

            if (patient == null)
            {
                return NotFound(new
                {
                    Error = $"Patient ID {id} is not found"
                });
            }

            //remove patient from DbContext
            DbContext.Patients.Remove(patient);

            //persist changes to db
            DbContext.SaveChanges();

            //return ok (200)
            return new OkResult();
        }

        //  GET: api/Patient?reportType=regular&startDate=2019-11-01&endDate=2020-12-10
        /// <summary>
        /// Retrieve Patients with Filters
        /// </summary>
        /// <param name="reportType">The type of LabReport</param>
        /// <param name="startDate">The start date filter</param>
        /// <param name="endDate">The end date filter</param>
        /// <returns>The list of patients with given {reportType} in the interval</returns>
        [HttpGet, Route("{reportType=reportType}/{startDate=startDate}/{endDate=endDate}")]
        public IActionResult Get([FromQuery]string reportType, [FromQuery] DateTime? startDate, 
            [FromQuery]DateTime? endDate)
        {


            var patients = DbContext.Patients.Include("LabReports")
                .Where(p => p.LabReports.Any(l => l.LabReportType == reportType && l.CreatedDate < endDate && l.CreatedDate > startDate)).ToArray();

            if (patients == null || patients.Length == 0)
            {
                return NotFound(new
                {
                    message = String.Format("There are no patients in the system.")
                });
            }

            return new JsonResult(patients.Adapt<PatientViewModel[]>(), new JsonSerializerOptions
            {
                WriteIndented = true
            });

        }

    }
}
