using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Services;
using SocialMedia.Core.DTO.Report;
using Swashbuckle.AspNetCore.Annotations;
using Social_Media.Helpers;

namespace Social_Media.Controllers
{
    [Route("api/report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new report")]
        [ProducesResponseType(typeof(RetriveReportDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateReport([FromBody] ReportDTO dto)
        {
            if (dto == null)
            {
                return ApiResponseHelper.BadRequest("Report data is required.");
            }
            try
            {
                var createdReport = await _reportService.CreateReportAsync(dto);
                if (createdReport == null)
                {
                    return ApiResponseHelper.NotFound("Failed to create report.");
                }
                return CreatedAtAction(nameof(GetReportById), new { Id = createdReport.Id }, createdReport);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
            }
        }

        [HttpGet("{Id:int}")]
        [SwaggerOperation(Summary = "Retrieves a report by its Id")]
        [ProducesResponseType(typeof(RetriveReportDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReportById(int Id)
        {
            try
            {
                var report = await _reportService.GetReportAsync(Id);
                if (report == null)
                {
                    return ApiResponseHelper.NotFound($"Report with Id {Id} not found.");
                }
                return ApiResponseHelper.Success(report, "Report retrieved successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
            }

        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retrieves all reports")]
        [ProducesResponseType(typeof(IEnumerable<RetriveReportDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllReports()
        {
            try
            {
                var reports = await _reportService.GetAllReportsAsync();
                return ApiResponseHelper.Success(reports, "Reports retrieved successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.InternalServerError(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        [SwaggerOperation(Summary = "Retrieves reports by user Id")]
        [ProducesResponseType(typeof(IEnumerable<RetriveReportDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReportsByUserId(string userId)
        {
            try
            {
                var reports = await _reportService.GetReportsByUserIdAsync(userId);
                return ApiResponseHelper.Success(reports, "Reports retrieved successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
            }
        }

        [HttpGet("status/{Id:int}")]
        [SwaggerOperation(Summary = "Updates the status of a report")]
        [ProducesResponseType(typeof(RetriveReportDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateReportStatus(int Id, UpdateReportStatusDTO dto)
        {
            try
            {
                var updatedReport = await _reportService.UpdateStatusAsync(Id, dto.ReportStatus);
                if (updatedReport == null)
                {
                    return ApiResponseHelper.NotFound($"Report with Id {Id} not found.");
                }
                return ApiResponseHelper.Success(updatedReport, "Report status updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
            }
        }

        [HttpGet("action/{Id:int}")]
        [SwaggerOperation(Summary = "Takes action on a report")]
        [ProducesResponseType(typeof(RetriveReportDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateReportAction(int Id, UpdateReportActionDTO dto)
        {
            try
            {
                var updatedReport = await _reportService.UpdateActionAsync(Id, dto.Action);
                if (updatedReport == null)
                {
                    return ApiResponseHelper.NotFound($"Report with Id {Id} not found.");
                }
                return ApiResponseHelper.Success(updatedReport, "Report action updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
            }
        }

        [HttpDelete("{Id:int}")]
        [SwaggerOperation(Summary = "Deletes a report by its Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReport(int Id)
        {
            try
            {
                await _reportService.DeleteAsync(Id);
                return ApiResponseHelper.Success("Report deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.InnerException?.Message}");
            }
        }
    }
}
