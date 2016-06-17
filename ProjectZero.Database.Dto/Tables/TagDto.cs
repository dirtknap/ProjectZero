using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dto.Tables
{
    [Table("Tags")]
    public class TagDto
    {
        [TableField("Id", IsPk = true, IsIdentity = true)]
        public int Id { get; set; }

        [TableField("Text")]
        public string Text { get; set; }

    }
}
