using DMSR.Data;
using DMSR.Components.Pages;
using DMSR.Models;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;

namespace DMSR.Services
{
    public class DocManagementService
    {
        private readonly DocActivityLogService _docActivityService;
        private readonly ApplicationDbContext _context;

        public DocManagementService(ApplicationDbContext context, DocActivityLogService docActivityService)
        {
            _context = context;
            _docActivityService = docActivityService;

        }
        public async Task<IEnumerable<Doc_Management>> GetAlldocsAsync()
        {
            var result = await _context.doc_managements
                         .Include(s => s.Users)      // ✅ Include users
                 .ToListAsync();

            return result;

        }
        public async Task AdddocsAsync(Doc_Management doc)
        {
            _context.doc_managements.Add(doc);
            await _context.SaveChangesAsync();
        }

        public async Task<Doc_Management> GetdocsByIdAsync(int id)
        {
            var doc = await _context.doc_managements
                .Include(s => s.Users)
                .FirstOrDefaultAsync(s => s.DocId == id);   
            return doc;
        }

        public async Task<bool> AddDupdocsAsync(Doc_Management doc_Management)
        {
            // Check if document title already exists
            bool isDuplicate = await _context.doc_managements
                .AnyAsync(s => s.Document_title == doc_Management.Document_title);

            if (isDuplicate)
            {
                return false; // Duplicate found
            }

          
            var selectedUserIds = doc_Management.Users.Select(c => c.UserId).ToList();
            doc_Management.Users = await _context.user_managements
                .Where(c => selectedUserIds.Contains(c.UserId))
                .ToListAsync();

          

            _context.doc_managements.Add(doc_Management);
            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task UpdatedocsAsync(Doc_Management doc_Management, int id)
        //{
        //    var dbuser = await _context.doc_managements.FindAsync(id);

        //    if (dbuser != null)
        //    {
        //        dbuser.Document_title = doc_Management.Document_title;
        //        dbuser.Document_type = doc_Management.Document_type;
        //      //  dbuser.Author = doc_Management.Author;
        //        dbuser.CreatedDate = doc_Management.CreatedDate;
        //        dbuser.LastModifiedDate = doc_Management.LastModifiedDate;
        //        dbuser.Version = doc_Management.Version;
        //        dbuser.FileTypeList = doc_Management.FileTypeList;
        //        dbuser.FileNames = doc_Management.FileNames;
        //        dbuser.FileDataList = doc_Management.FileDataList;
        //        dbuser.Docfile = doc_Management.Docfile;


        //        //// ✅ Update many-to-many users
        //        var selectedUserIds = doc_Management.Users.Select(c => c.UserId).ToList();
        //        dbuser.Users = await _context.user_managements
        //            .Where(c => selectedUserIds.Contains(c.UserId))
        //            .ToListAsync();


        //        await _context.SaveChangesAsync();
        //    }
        //}

        public async Task UpdatedocsAsync(Doc_Management doc_Management, int id, string performedBy = "System")
        {
            var dbuser = await _context.doc_managements.FindAsync(id);

            if (dbuser != null)
            {
                dbuser.Document_title = doc_Management.Document_title;
                dbuser.Document_type = doc_Management.Document_type;
                dbuser.CreatedDate = doc_Management.CreatedDate;
                dbuser.LastModifiedDate = doc_Management.LastModifiedDate;
                dbuser.Version = doc_Management.Version;
                dbuser.FileTypeList = doc_Management.FileTypeList;
                dbuser.FileNames = doc_Management.FileNames;
                dbuser.FileDataList = doc_Management.FileDataList;
                dbuser.Docfile = doc_Management.Docfile;

                var selectedUserIds = doc_Management.Users.Select(c => c.UserId).ToList();
                dbuser.Users = await _context.user_managements
                    .Where(c => selectedUserIds.Contains(c.UserId))
                    .ToListAsync();

                await _context.SaveChangesAsync();

                await _docActivityService.LogDocumentActivityAsync(
                    dbuser.Document_title,
                    "Edit",
                    performedBy,
                    dbuser.Docfile
                );
            }
        }

        //// simple pagination
        public async Task<(List<Doc_Management>, int)> GetPaginatedDocsAsync(int pageNumber, int pageSize)
        {

            var totalItems = await _context.doc_managements.CountAsync();

            var docs = await _context.doc_managements
                .Include(s => s.Users)  // ✅ Include users so they appear in table
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (docs, totalItems);
        }

        //    // Generate Excel for a single document
        public async Task<byte[]> GenerateSingleDocAsync(int docId)
        {
            try
            {
                var doc = await _context.doc_managements.FirstOrDefaultAsync(d => d.DocId == docId);
                if (doc == null)
                    return Array.Empty<byte>();

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Document");

                // Headers
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Title";
                worksheet.Cell(1, 3).Value = "Type";
                worksheet.Cell(1, 4).Value = "Author";
                worksheet.Cell(1, 5).Value = "CreatedDate";
                worksheet.Cell(1, 6).Value = "LastModifiedDate";
                worksheet.Cell(1, 7).Value = "Version";
                worksheet.Cell(1, 8).Value = "FileType";
                worksheet.Cell(1, 9).Value = "Department";
                worksheet.Cell(1, 10).Value = "Description";




                // Data
                worksheet.Cell(2, 1).Value = doc.DocId;
                worksheet.Cell(2, 2).Value = doc.Document_title;
                worksheet.Cell(2, 3).Value = doc.Document_type;
                worksheet.Cell(2, 4).Value = doc.Author;
                worksheet.Cell(2, 5).Value = doc.CreatedDate.ToString("yyyy-MM-dd"); // ✅ Converts DateOnly to string
                worksheet.Cell(2, 6).Value = doc.LastModifiedDate.ToString("HH:mm");       // ✅ Converts TimeOnly to string
                worksheet.Cell(2, 7).Value = doc.Version;
                worksheet.Cell(2, 8).Value = doc.FileType;
                worksheet.Cell(2, 9).Value = doc.Department;
                worksheet.Cell(2, 10).Value = doc.Description;

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

        public async Task<List<User_Management>> GetUsersAsync()
        {
            var result = await _context.user_managements.ToListAsync();
            return result;

        }
        public async Task<User_Management?> GetUserByIdAsync(int userId)
        {
            return await _context.user_managements.FirstOrDefaultAsync(u => u.UserId == userId);
        }
        public async Task<bool> AddDocActivityAsync(DocActivityLogs log)
        {
            _context.activity_logs.Add(log);
            return await _context.SaveChangesAsync() > 0;
        }


    }
}
