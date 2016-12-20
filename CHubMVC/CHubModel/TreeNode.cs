using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel
{
    public class TreeNode
    {
        public string text { get; set; }

        public string icon { get; set; }

        public string selectedIcon { get; set; }

        public string Color { get; set; }

        public string backColor { get; set; }

        public string href { get; set; }

        public bool selectable { get; set; }

        public Nodestates state { get; set; }

        public string tags { get; set; }

        public List<TreeNode> nodes { get; set; }

    }

    public class Nodestates
    {
        public Nodestates()
        {
            this.@checked = false;
            this.disabled = false;
            this.expanded = false;
            this.selected = false;
        }
        public bool @checked { get; set; }
        public bool disabled { get; set; }
        public bool expanded { get; set; }
        public bool selected { get; set; }
    }
}
