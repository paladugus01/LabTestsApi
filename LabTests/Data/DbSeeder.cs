using LabTests.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabTests.Data
{
    public class DbSeeder
    {
        #region Public Methods
        public static void Seed(ApplicationDbContext dbContext)
        {
            //create default Users
            if (!dbContext.Users.Any())
                CreateUsers(dbContext);
            //create default Patients
            if (!dbContext.Patients.Any())
                CreatePatients(dbContext);
            // create default lab reports
            if (!dbContext.LabReports.Any())
                CreateLabReports(dbContext);

        }
        #endregion

        private static void CreateLabReports(ApplicationDbContext dbContext)
        {
            //local variables
            DateTime createdDate = new DateTime(2020, 01, 01, 12, 30, 00);
            DateTime lastModifiedDate = DateTime.Now;

            //default User who created patients
            var userId = dbContext.Users.Where(u => u.UserName == "Admin")
                .FirstOrDefault().Id;

            //Create 2 type of lab reports
            string[] labReportTypes =  { "regular", "expedited" };

            //Collet 10 patient ids to insert into the foreign key
            var patientIds = dbContext.Patients.Take(10).Select(p => p.Id).ToList();

            //#if DEBUG
            foreach(var id in patientIds)
            {
                //create 2 lab reports for each patient
                var labReport_1 = new LabReport
                {
                    Id = Guid.NewGuid(),
                    PatientId = id,
                    Result = LabTestResult.Positive,
                    Status = LabReportStatus.Routine,
                    Physician = "Test Physician",
                    CreatedDate = createdDate,
                    LastModifiedDate = lastModifiedDate,
                    LabReportType = labReportTypes[0],
                    LabTestTime = DateTime.Now,
                    CreatedByUserId = userId
                };
                var labReport_2 = new LabReport
                {
                    Id = Guid.NewGuid(),
                    PatientId = id,
                    Result = LabTestResult.Negative,
                    Status = LabReportStatus.Stat,
                    Physician = "Test Physician 2",
                    CreatedDate = createdDate,
                    LastModifiedDate = lastModifiedDate,
                    LabReportType = labReportTypes[1],
                    LabTestTime = DateTime.Now,
                    CreatedByUserId = userId
                };

                dbContext.LabReports.Add(labReport_1);
                dbContext.LabReports.Add(labReport_2);

                //saving the changes
                dbContext.SaveChanges();
            }
            //#endif
        }

        private static void CreatePatients(ApplicationDbContext dbContext)
        {
            //local variables
            DateTime createdDate = new DateTime(2020, 01, 01, 12, 30, 00);
            DateTime lastModifiedDate = DateTime.Now;

            //default User who created patients
            var userId = dbContext.Users.Where(u => u.UserName == "Admin")
                .FirstOrDefault().Id;

            //#if DEBUG
            //Add 10 patients to the Database
            for (int i = 0; i < 15; i++)
            {
                var patient = new Patient
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Patient " + i,
                    DOB = new DateTime(1980,01,25),
                    Gender = PatientGender.Female,
                    Address = "Test Address 1",
                    CreatedDate = createdDate,
                    LastModifiedDate = lastModifiedDate,
                    CreatedByUserId = userId
                };

                dbContext.Patients.Add(patient);

                //save changes to db
                dbContext.SaveChanges();
            }
            for (int i = 0; i < 5; i++)
            {
                var patient = new Patient
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Patient " + (15 +i),
                    DOB = new DateTime(1980, 01, 25),
                    Gender = PatientGender.Male,
                    Address = "Test Address 1",
                    CreatedDate = createdDate,
                    LastModifiedDate = lastModifiedDate,
                    CreatedByUserId = userId
                };

                dbContext.Patients.Add(patient);

                //save changes to db
                dbContext.SaveChanges();
            }
            //#endif
        }


        private static void  CreateUsers(ApplicationDbContext dbContext)
        {
            //local variables
            DateTime createdDate = new DateTime(2019, 12, 01, 12, 30, 00);
            DateTime lastModifiedDate = DateTime.Now;

            //create the "Admin" Application User

            var user_Admin = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = "Admin",
                Email = "admin@labtests.com",
                CreatedDateTime = createdDate,
                LastModifiedDate = lastModifiedDate
            };
            dbContext.Users.Add(user_Admin);
            dbContext.SaveChanges();
        }
    }
}
