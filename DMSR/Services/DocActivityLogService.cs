using ClosedXML.Excel;
using DMSR.Data;
using DMSR.Models;
using Microsoft.EntityFrameworkCore;

namespace DMSR.Services
{
    // DocActivityLogsService.cs
    public class DocActivityLogService
    {
        private readonly ApplicationDbContext _context;

        public DocActivityLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogDocumentActivityAsync(string title, string activity, string performedBy, byte[]? actfile = null)
        {
            // Try to find an existing log for this title and user
            var existingLog = await _context.activity_logs
                .FirstOrDefaultAsync(l => l.Document_title == title && l.Performedby == performedBy);

            if (existingLog != null)
            {
                // Update existing log
                existingLog.Document_activity = activity;
                existingLog.Date = DateOnly.FromDateTime(DateTime.Now);
                existingLog.Time = TimeOnly.FromDateTime(DateTime.Now);
                existingLog.Actfile = actfile ?? Array.Empty<byte>();
                _context.activity_logs.Update(existingLog);
            }
            else
            {
                // Insert new log
                var log = new DocActivityLogs
                {
                    Document_title = title,
                    Document_activity = activity,
                    Performedby = performedBy,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Time = TimeOnly.FromDateTime(DateTime.Now),
                    Actfile = actfile ?? Array.Empty<byte>()
                };

                await _context.activity_logs.AddAsync(log);
            }

            await _context.SaveChangesAsync();
        }
        public async Task<List<DocActivityLogs>> GetAllAsync()
        {
            return await _context.activity_logs
                .OrderByDescending(x => x.Date).ThenByDescending(x => x.Time)
                .ToListAsync();
        }
        public async Task<(List<DocActivityLogs>, int)> GetPaginatedDocsAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.activity_logs.CountAsync();
            var docs = await _context.activity_logs
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (docs, totalItems);
        }

        // Generate Excel for a single document
        public async Task<byte[]> GenerateSingleDocAsync(int docId)
        {
            try
            {
                var doc = await _context.activity_logs.FirstOrDefaultAsync(d => d.DocAId == docId);
                if (doc == null)
                    return Array.Empty<byte>();

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Document");

                // Headers
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Title";
                worksheet.Cell(1, 3).Value = "Activity";
                worksheet.Cell(1, 4).Value = "PerformedBy";
                worksheet.Cell(1, 5).Value = "Date";
                worksheet.Cell(1, 6).Value = "Time";




                // Data
                worksheet.Cell(2, 1).Value = doc.DocAId;
                worksheet.Cell(2, 2).Value = doc.Document_title;
                worksheet.Cell(2, 3).Value = doc.Document_activity;
                worksheet.Cell(2, 4).Value = doc.Performedby;
                worksheet.Cell(2, 5).Value = doc.Date.ToString("yyyy-MM-dd"); // ✅ Converts DateOnly to string
                worksheet.Cell(2, 6).Value = doc.Time.ToString("HH:mm");       // ✅ Converts TimeOnly to string

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return stream.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating Excel file: {ex.Message}");
            }

            // Return an empty byte array to ensure all code paths return a value
            return Array.Empty<byte>();
        }
        public async Task<DocActivityLogs?> GetDocActivityByIdAsync(int id)
        {
            return await _context.activity_logs.FirstOrDefaultAsync(d => d.DocAId == id);
        }

    }


}
