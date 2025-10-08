using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Staff_Management.Classes;
using Staff_Management.Models;
using Staff_Management.Repositories;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;

namespace Staff_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffsController : ControllerBase
    {
        private readonly IStaffRepository _iStaffRepo;

        public StaffsController(IStaffRepository iStaffRepo)
        {
            _iStaffRepo = iStaffRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] StaffFilter filter)
        {
            var staffs = _iStaffRepo.GetAll();

            staffs = staffs.Where(x =>
                    (string.IsNullOrEmpty(filter.StaffId) || x.StaffId == filter.StaffId) &&
                    (!filter.Gender.HasValue || x.Gender == filter.Gender.Value) &&
                    (!filter.StartDate.HasValue || x.Birthday >= filter.StartDate.Value) &&
                    (!filter.EndDate.HasValue || x.Birthday <= filter.EndDate.Value)
                ).ToList();

            if(filter.Export == "EXCEL")
            {
                var fileBytes = Helper.StaffExportAsExcel(staffs);
                return File(
                    fileBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Staffs_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
                );
            }
            else if (filter.Export == "PDF")
            {
                var pdfBytes = Helper.StaffExportAsPdf(staffs);
                return File(
                    pdfBytes,
                    "application/pdf",
                    $"Staffs_{DateTime.Now:yyyyMMddHHmmss}.pdf"
                );
            }
            else
                return Ok(staffs);
        }

        [HttpGet("{staffId}")]
        public async Task<IActionResult> GetStaff(string staffId)
        {
            var note = _iStaffRepo.GetById(staffId);
            if (note == null)
                return NotFound("Note not found");

            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StaffModel staff)
        {
            _iStaffRepo.Add(staff);
            return Ok(staff);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] StaffModel staff)
        {
            var isSuccess = _iStaffRepo.Update(staff.StaffId, staff);
            return isSuccess ? Ok("Staff updated") : NotFound("Staff not found !!!");
        }

        [HttpDelete("{staffId}")]
        public async Task<IActionResult> Delete(string staffId)
        {
            var isSuccess = _iStaffRepo.Delete(staffId);
            return isSuccess ? Ok("Staff deleted") : NotFound("Staff not found !!!");
        }
    }
}
