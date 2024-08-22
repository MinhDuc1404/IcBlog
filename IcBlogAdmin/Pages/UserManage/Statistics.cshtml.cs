using IcBlog.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IcBlogAdmin.Pages.UserManage
{
    [Authorize(Roles = "admin")]
    public class StatisticsModel : PageModel
    {
        private readonly BlogContext _blogContext;

        public StatisticsModel(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        public List<string> Dates { get; set; }
        public List<int> AccountCounts { get; set; }
        public List<int> LoginCount { get; set; }

        public async Task OnGetAsync()
        {
            // Fetch account creation data
            var accountData = await _blogContext.ApplicationUsers
                .GroupBy(u => u.CreatedDate.Date)
                .OrderBy(g => g.Key)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync();

            // Fetch login attempts data
            var loginData = await _blogContext.LoginAttempts
                .Where(l => l.Success)
                .GroupBy(l => l.AttemptedAt.Date)
                .OrderBy(g => g.Key)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync();

            // Get the earliest and latest dates
            var earliestDate = accountData.Any() ? accountData.Min(a => a.Date) : DateTime.Today;
            var latestDate = DateTime.Today; // Include today's date

            // Generate continuous date range from the earliest to the latest date
            Dates = Enumerable.Range(0, (latestDate - earliestDate).Days + 1)
                .Select(offset => earliestDate.AddDays(offset).ToString("yyyy-MM-dd"))
                .ToList();

            // Initialize counts with zeros
            AccountCounts = new List<int>(new int[Dates.Count]);
            LoginCount = new List<int>(new int[Dates.Count]);

            // Fill in the data for accounts created
            foreach (var data in accountData)
            {
                int index = Dates.IndexOf(data.Date.ToString("yyyy-MM-dd"));
                if (index >= 0)
                {
                    AccountCounts[index] = data.Count;
                }
            }

            // Fill in the data for successful login attempts
            foreach (var data in loginData)
            {
                int index = Dates.IndexOf(data.Date.ToString("yyyy-MM-dd"));
                if (index >= 0)
                {
                    LoginCount[index] = data.Count;
                }
            }
        }
    }
}
