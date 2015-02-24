using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ClientApp.iOS
{
	partial class CustomAlumniCell : UITableViewCell
	{
		public CustomAlumniCell (IntPtr handle) : base (handle)
		{
		}

        public CustomAlumniCell(string cellId)
            : base(UITableViewCellStyle.Default, cellId)
        {

        }


        public void UpdateCell(string specialization, string numAlumni)
        {
            specializationLabel.Text = specialization;
            alumniCountLabel.Text = numAlumni;
        }
	}
}
