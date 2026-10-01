using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace TelecomBackend;

[ApiController]
[Route("api/[controller]")]
public class LogsController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<Logs>> GetLogs()
    {
        try
        {
            var logs = DatabaseService.GetLogs();
            return Ok(logs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpGet("{id}/points")]
    public async Task<ActionResult<IEnumerable<LogPoints>>> GetLogPoints(int id)
    {
        var points = DatabaseService.GetLogPoints(id);

        if (points == null || !points.Any())
        {
            return NotFound();
        }

        return Ok(points);
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> PostLog([FromForm] Trip trip)
    {
        if (trip.GPXFile == null || trip.CSVFile == null)
        {
            return BadRequest("You have to insert both GPX and CSV files.");
        }

        string tempGpxPath = Path.GetTempFileName();
        string tempCsvPath = Path.GetTempFileName();

        try
        {
            using (var stream = new FileStream(tempGpxPath, FileMode.Create))
            {
                await trip.GPXFile.CopyToAsync(stream);
            }
            using (var stream = new FileStream(tempCsvPath, FileMode.Create))
            {
                await trip.CSVFile.CopyToAsync(stream);
            }

            var tripId = DatabaseService.InsertTrip(tempCsvPath, tempGpxPath);
            return Ok(new {id = tripId, message = "The trip was successfully uploaded!"});
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error processing files: {ex.Message}");
        }
        finally
        {
            if(System.IO.File.Exists(tempGpxPath)) System.IO.File.Delete(tempGpxPath);
            if(System.IO.File.Exists(tempCsvPath)) System.IO.File.Delete(tempCsvPath);
        }
    }
}