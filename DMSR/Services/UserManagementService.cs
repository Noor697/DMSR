using System;
using DMSR.Components.Pages;
using DMSR.Data;
using DMSR.Models;
using Microsoft.EntityFrameworkCore;

namespace DMSR.Services
{
    public class UserManagementService
    {
        private readonly ApplicationDbContext _context;

        public UserManagementService(ApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<User_Management>> GetAllUsersAsync()
        {
            var result = await _context.user_managements.ToListAsync();

            return result;
        }
        public async Task AddUsersAsync(User_Management user)
        {
            _context.user_managements.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User_Management> GetUsersByIdAsync(int id)
        {
            var user =  await _context.user_managements.Include(u => u.Documents)
                           .FirstOrDefaultAsync(u => u.UserId == id);
            return user;


        }
        public async Task<bool> AddDupUsersAsync(User_Management user_Management)
        {
            // Check if CNIC or Roll Number already exists
            bool isDuplicate = await _context.user_managements
                .AnyAsync(s => s.MobileNo == user_Management.MobileNo);

            if (isDuplicate)
            {
                return false; // Indicate that a duplicate record exists
            }

            _context.user_managements.Add(user_Management);
            await _context.SaveChangesAsync();
            return true; // Successfully added
        }

        public async Task UpdateUsersAsync(User_Management user_Management, int id)
        {
            var dbuser = await _context.user_managements.FindAsync(id);
            if (dbuser != null)
            {
                dbuser.UserName = user_Management.UserName;
                dbuser.Role = user_Management.Role;
                dbuser.MobileNo = user_Management.MobileNo;
                dbuser.Email = user_Management.Email;
                dbuser.Status = user_Management.Status;
                dbuser.UserImage = user_Management.UserImage;


                await _context.SaveChangesAsync();

                          }


        }
       // simple pagination
        public async Task<(List<User_Management>, int)> GetPaginatedUsersAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.user_managements.CountAsync();
            var users = await _context.user_managements
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (users, totalItems);
        }


     
    }
}
