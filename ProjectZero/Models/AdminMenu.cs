using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.String;

namespace ProjectZero.Models
{
    public class AdminMenu
    {
        private int counter;

        public int IdCounter
        {
            get
            {
                counter++;
                if (counter > 100) counter = 0;
                return counter;
            }
        }

        public List<AdminSection> AdminSections { get; set; }
    }

    public class AdminSection : IComparable<AdminSection>
    {
        public string SectionName { get; set; }
        public List<AdminPage> AdminPages { get; set; }
        public int CompareTo(AdminSection other)
        {
            return other == null ? 1 : Compare(SectionName, other.SectionName, StringComparison.Ordinal);
        }
    }

    public class AdminPage : IComparable<AdminPage>
    {
        public string DisplayName { get; set; }
        public string ControllerAction { get; set; }

        public int CompareTo(AdminPage other)
        {
            return other == null ? 1 : Compare(DisplayName, other.DisplayName, StringComparison.Ordinal);
        }
    }

}
