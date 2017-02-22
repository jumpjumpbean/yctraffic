using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WafTraffic.Domain
{
    public partial class ZdtzConfigTable
    {
        #region Constructors

        public ZdtzConfigTable()
        {
            this.Children = new List<ZdtzConfigTable>();
        }

        #endregion

        #region Properties

        public List<ZdtzConfigTable> Children { get; set; }

        #endregion

        #region Members

        public void AddChildren(List<ZdtzConfigTable> nodes)
        {
            if (nodes == null) return;
            this.Children.AddRange(nodes.FindAll(q => q.ParentId == this.Id));
        }

        public void AddToParent(List<ZdtzConfigTable> nodes, int id)
        {
            ZdtzConfigTable parent = FindParent(nodes, id);
            if (parent != null) parent.Children.Add(this);
        }

        /// <summary>
        /// 递归向下查找
        /// </summary>
        private ZdtzConfigTable FindParent(List<ZdtzConfigTable> nodes, int id)
        {
            if (nodes == null) return null;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Id == id)
                {
                    return nodes[i];
                }
                ZdtzConfigTable node = FindParent(nodes[i].Children, id);
                if (node != null)
                {
                    return node;
                }
            }
            return null;
        }

        #endregion
    }
}
