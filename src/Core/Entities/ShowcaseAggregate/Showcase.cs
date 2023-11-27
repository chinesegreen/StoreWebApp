using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.ShowcaseAggregate
{
    public class Showcase : BaseEntity
    {
        public int Position { get; set; }
        [MaxLength(30)]
        public string Subtitle { get; set; }
        [MaxLength(40)]
        public string Title { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
    }
}
