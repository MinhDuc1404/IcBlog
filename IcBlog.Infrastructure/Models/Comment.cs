using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcBlog.Infrastructure.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public Blog Blog { get; set; }
        public ApplicationUser Author { get; set; }
        public string Content { get; set; }
        public int? CommentParentID { get; set; }
        public Comment CommentParent { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual List<Comment> Replies { get; set; }
    }
}
