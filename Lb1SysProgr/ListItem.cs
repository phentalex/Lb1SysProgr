using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lb1SysProgr
{
    [ToolboxItem(true)]
    public partial class ListItem : UserControl
    {
        public ListItem()
        {
            InitializeComponent();
        }


        #region Properties
        private string _type;
        private string _director;
        private string _percent;
        private string _rating;
        private string _phone;
        private string _name;

        [Category("Custom Props")]
        public string Type
        {
            get { return _type; }
            set { _type = value; label1.Text = value; }
        }

        [Category("Custom Props")]
        public string Name
        {
            get { return _name; }
            set { _name = value; label3.Text = value; }
        }

        [Category("Custom Props")]
        public string Director
        {
            get { return _director; }
            set { _director = value; label4.Text = value; }
        }

        [Category("Custom Props")]
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; label5.Text = value; }
        }

        [Category("Custom Props")]
        public string Rating
        {
            get { return _rating; }
            set { _rating = value; label6.Text = Convert.ToString(value); }
        }

        [Category("Custom Props")]
        public string Percent
        {
            get { return _percent; }
            set { _percent = value; label7.Text = Convert.ToString(value); }
        }
#endregion
    }
}
