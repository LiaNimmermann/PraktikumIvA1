using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroVerseMaui.Models
{
    public class NewPost
    {

        public NewPost(string body)
        {
            Body = body;
        }

        public string Body { get; set; }
    }
}
