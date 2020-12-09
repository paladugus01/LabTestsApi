using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabTests.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using LabTests.Data;
using Mapster;
using LabTests.Data.Models;

namespace LabTests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabReportController : ControllerBase
    {
        #region PrivateFields
        private ApplicationDbContext DbContext;
        #endregion

        public LabReportController(ApplicationDbContext context)
        {
            DbContext = context;

        }

        // GET: api/LabReport
        /// <summary>
        /// Retrieve latest {num} of reports
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        [HttpGet("{num:int?}")]
        public IActionResult Get(int num = 10)
        {
            var labReports = DbContext.LabReports.OrderByDescending(l => l.LastModifiedDate).Take(num).ToArray();

            if(labReports.Length == 0)
            {
                return NotFound(new
                {
                    message = String.Format("There are no labreports in the system.")
                });
            }

            return new JsonResult(labReports.Adapt<LabReportViewModel[]>(), new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        // GET: api/LabReport/5
        /// <summary>
        /// Retrieve the lab report with given GUID
        /// </summary>
        /// <param name="id">The GUID of the LabReport to retrieve</param>
        /// <returns>The LabReport with the specified Id</returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var labReports = DbContext.LabReports.Where(p => p.Id.Equals(id)).FirstOrDefault();

            if (labReports == null)
            {
                return NotFound(new
                {
                    message = $"There are no labreports in the system with ID = {id}."
                });
            }
            return new JsonResult(labReports.Adapt<LabReportViewModel>(), new JsonSerializerOptions
            {
                WriteIndented = true
            });

        }

        // POST: api/LabReport
        /// <summary>
        /// Update existing Lab Report in the system
        /// </summary>
        /// <param name="m">The labreport to be updated</param>
        [HttpPost]
        public IActionResult Post([FromBody] LabReportViewModel m)
        {
            // return generic HTTP Status 500 if client payload is invalid
            if (m == null) return new StatusCodeResult(500);

            var labReport = DbContext.LabReports.Where(q => q.Id == m.Id).FirstOrDefault();

            //handle if the patient does not exist

            if (labReport == null)
            {
                return NotFound(new
                {
                    Error = $"Patient ID {m.Id} is not found"
                });
            }

            //properties taken from request
            labReport.PatientId = m.PatientId;
            labReport.Result = m.Result;
            labReport.Status = m.Status;
            labReport.Physician = m.Physician;
            labReport.LabReportType = m.LabReportType;
            labReport.LabTestTime = m.LabTestTime;

            //set on serverside
            labReport.LastModifiedDate = DateTime.Now;

            //persist to DB
            DbContext.SaveChanges();

            // return updated patient back to client

            return new JsonResult(labReport.Adapt<LabReportViewModel>(),
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

        }

        // PUT: api/LabReport/5
        /// <summary>
        /// Create a new Labreport in the system
        /// </summary>
        /// <param name="m"></param>
        [HttpPut]
        public IActionResult Put([FromBody] LabReportViewModel m)
        {
            // return generic HTTP Status 500 if client payload is invalid
            if (m == null) return new StatusCodeResult(500);

            var labReport = new LabReport();

            //properties taken from request
            labReport.PatientId = m.PatientId;
            labReport.Result = m.Result;
            labReport.Status = m.Status;
            labReport.Physician = m.Physician;
            labReport.LabReportType = m.LabReportType;
            labReport.CreatedByUserId = m.CreatedByUserId;
            labReport.LabTestTime = m.LabTestTime;

            //server side properties
            labReport.Id = Guid.NewGuid();
            labReport.CreatedDate = DateTime.Now;
            labReport.LastModifiedDate = DateTime.Now;

            //Created by admin
            labReport.CreatedByUserId = DbContext.Users.Where(u => u.UserName == "Admin")
                   .FirstOrDefault().Id;

            //adding the patient to the database
            DbContext.LabReports.Add(labReport);

            //persist to DB
            DbContext.SaveChanges();

            // return created patient back to client

            return new JsonResult(labReport.Adapt<LabReportViewModel>(),
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

        }

        // DELETE: api/ApiWithActions/5
        /// <summary>
        /// Delete a alb report with the given id
        /// </summary>
        /// <param name="id">The Id of the report to delete</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            // return generic HTTP Status 500 if client payload is invalid
            if (id == null) return new StatusCodeResult(500);

            //retrieve the patient to edit
            var labReport = DbContext.LabReports.Where(q => q.Id == id).FirstOrDefault();

            //handle if the patient does not exist

            if (labReport == null)
            {
                return NotFound(new
                {
                    Error = $"Patient ID {id} is not found"
                });
            }

            //remove patient from DbContext
            DbContext.LabReports.Remove(labReport);

            //persist changes to db
            DbContext.SaveChanges();

            //return ok (200)
            return new OkResult();
        }
    }
}
