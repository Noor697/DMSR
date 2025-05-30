using ClosedXML.Excel;
using DMSR.Data;
using DMSR.Models;
using Microsoft.EntityFrameworkCore;

namespace DMSR.Services
{
    public class UserActivityLogService
    {
        private readonly ApplicationDbContext _context;

        public UserActivityLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogUserActivityAsync(string username, string activity)
        {
            var log = new UserActivityLog
            {
                UserName = username,
                Activity = activity,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Time = TimeOnly.FromDateTime(DateTime.Now),
            };

            await _context.user_logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserActivityLog>> GetAllAsync()
        {
            return await _context.user_logs
                .OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.Time)
                .ToListAsync();
        }

        public async Task<(List<UserActivityLog>, int)> GetPaginatedUsersAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.user_logs.CountAsync();
            var logs = await _context.user_logs
                .OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.Time)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (logs, totalItems);
        }

        public async Task<byte[]> GenerateSingleUserAsync(int logId)
        {
            var log = await _context.user_logs.FirstOrDefaultAsync(l => l.UserAId == logId);
            if (log == null) return Array.Empty<byte>();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("User Activity");

            // Headers
            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "User Name";
            worksheet.Cell(1, 3).Value = "Activity";
            worksheet.Cell(1, 4).Value = "Date";
            worksheet.Cell(1, 5).Value = "Time";

            // Data
            worksheet.Cell(2, 1).Value = log.UserAId;
            worksheet.Cell(2, 2).Value = log.UserName;
            worksheet.Cell(2, 3).Value = log.Activity;
            worksheet.Cell(2, 4).Value = log.Date.ToString("yyyy-MM-dd");
            worksheet.Cell(2, 5).Value = log.Time.ToString("hh:mm tt");

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public async Task<UserActivityLog?> GetUserActivityByIdAsync(int id)
        {
            return await _context.user_logs.FirstOrDefaultAsync(l => l.UserAId == id);
        }
    }
}
