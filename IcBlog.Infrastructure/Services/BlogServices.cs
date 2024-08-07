using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IcBlog.Infrastructure.Data;
using IcBlog.Infrastructure.Models;
using IcBlog.Infrastructure.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace IcBlog.Infrastructure.Services
{
    public class BlogServices : IBlogServices
    {
        private readonly BlogContext _blogContext;

        public BlogServices(BlogContext blogContext)
        {  
            _blogContext = blogContext; 
        }
        public async Task<List<Blog>> GetListBlogAsync()
        {
            return await _blogContext.Blogs
                .Include(b => b.Category)
                .Include(b=>b.Author)
                .OrderByDescending(b=>b.DateTime)
                .ToListAsync();
        }
        public async Task<List<Blog>> GetBlogWithCateAsync(int? category)
        {
            return await _blogContext.Blogs
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Where(b => b.Category.CategoryID == category)
                .OrderByDescending(b=>b.DateTime)
                .ToListAsync();
        }
        public async Task<List<Blog>> GetBlogSearchingAsync(string searching)
        {
            return await _blogContext.Blogs
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Where(b=>b.Title.Contains(searching)).ToListAsync();
        }
        public async Task<Blog> GetBlogAsync(int id)
        {
            return await _blogContext.Blogs
                 .Include(b => b.Category) // Bao gồm Category
                 .Include(b => b.Author)   // Bao gồm Author
                  .Include(b => b.Comments)
                .FirstOrDefaultAsync(b => b.BlogID == id);
        }
        public async Task<Blog> AddBlog(Blog blog)
        {
            
            _blogContext.Add(blog);
           await _blogContext.SaveChangesAsync();
            return blog;  
        }
        public async Task<List<Blog>> GetblogByUserIDAsync(string userID)
        {
            return await _blogContext.Blogs
                .Where(b => b.Author.Id == userID)
                .OrderByDescending(b=>b.DateTime)
                .ToListAsync();
        }
        public async Task<Blog> UpdateBlog(Blog blog)
        {

            _blogContext.Update(blog);
            await _blogContext.SaveChangesAsync();
            return blog;
        }
        public async Task<Comment> AddComment(Comment comment)
        {
            _blogContext.Add(comment);
            await _blogContext.SaveChangesAsync();
            return comment;
        }
        public async Task<Comment> GetCommentAsync(int commentId)
        {
            return await _blogContext.Comments
                .Include(comment => comment.Author)
                .Include(comment => comment.Blog)
                .Include(comment => comment.CommentParent)
                .FirstOrDefaultAsync(comment => comment.Id == commentId);
        }

    }
}
