using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WafTraffic.Domain
{
    public class Node
    {
        public Node()
        {
            this.Nodes = new List<Node>();
            this.ParentID = -1;
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int ParentID { get; set; }
        public List<Node> Nodes { get; set; }
    }  
}
