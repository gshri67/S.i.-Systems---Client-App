using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	partial class EditableFullTextFieldCell : UITableViewCell
	{
		public const string CellIdentifier = "EditableFullTextFieldCell";
	    public UILabel DescriptionLabel;
		public UITextView TextView;
		
		public delegate void EditableCellDelegate(string newValue);
		public EditableCellDelegate OnValueChanged;
        public EditableCellDelegate OnValueFinalized;

		public EditableFullTextFieldCell(IntPtr handle)
			: base(handle)
		{
			InitializeCell();
		}

		public EditableFullTextFieldCell(string cellId)
			: base(UITableViewCellStyle.Default, cellId)
		{
			InitializeCell();
		}

		private void InitializeCell()
		{
			CreateAndAddLabels();

			SetupConstraints();

		   // BackgroundColor = UIColor.FromWhiteAlpha(0.98f, 1.0f);
		}

		private void CreateAndAddLabels()
		{
            CreateAndAddDescriptionLabel();
			CreateAndAddTextView();
		}


        private void CreateAndAddDescriptionLabel()
        {
            DescriptionLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Left,
                Font = UIFont.FromName("Helvetica", 16f),
                TextColor = UIColor.Black,
                Text = "Description:"
            };
            AddSubview(DescriptionLabel);
        }

	    private void CreateAndAddTextView()
	    {
	        TextView = new UITextView()
	        {
	            TranslatesAutoresizingMaskIntoConstraints = false,
	            TextAlignment = UITextAlignment.Left,
	            Font = UIFont.FromName("Helvetica", 14f),
	            TextColor = StyleGuideConstants.MediumGrayUiColor,
                ScrollEnabled = false
	        };

            TextView.TextContainerInset = UIEdgeInsets.Zero;
            TextView.TextContainer.LineFragmentPadding = 0;
            TextView.Text = "Text Text \n Text";

	        AddSubview(TextView);

	        var toolbar = new UIToolbar(new CoreGraphics.CGRect(0.0f, 0.0f, Frame.Size.Width, 44.0f));

	        toolbar.Items = new[]
	        {
	            new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
	            new UIBarButtonItem(UIBarButtonSystemItem.Done, doneButtonTapped)
	        };

	        TextView.InputAccessoryView = toolbar;

	        TextView.Changed += delegate
	        {
	            OnValueChanged(TextView.Text);
	        };

	        TextView.Ended += delegate
	        {
                if( OnValueFinalized != null )
    	            OnValueFinalized(TextView.Text);
	        };
	    }

	    public void doneButtonTapped(object sender, EventArgs args)
		{
			TextView.ResignFirstResponder();
		}

		public void SetupConstraints()
		{
            AddSubjectTextLabelConstraints();
			AddTextLabelConstraints();

            AddConstraint(NSLayoutConstraint.Create(this, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, TextView, NSLayoutAttribute.Bottom, 1.0f, 0f));
		}

        private void AddSubjectTextLabelConstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(DescriptionLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.05f, 0f));
            AddConstraint(NSLayoutConstraint.Create(DescriptionLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(DescriptionLabel, NSLayoutAttribute.Width, NSLayoutRelation.GreaterThanOrEqual, null, NSLayoutAttribute.NoAttribute, 1.0f, 40f));
            AddConstraint(NSLayoutConstraint.Create(DescriptionLabel, NSLayoutAttribute.Width, NSLayoutRelation.LessThanOrEqual, null, NSLayoutAttribute.NoAttribute, 1.0f, 70f));
        }

		private void AddTextLabelConstraints()
		{
            AddConstraint(NSLayoutConstraint.Create(TextView, NSLayoutAttribute.Left, NSLayoutRelation.Equal, DescriptionLabel, NSLayoutAttribute.Right, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(TextView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, DescriptionLabel, NSLayoutAttribute.Top, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(TextView, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.9f, 0f));
            AddConstraint(NSLayoutConstraint.Create(TextView, NSLayoutAttribute.Bottom, NSLayoutRelation.GreaterThanOrEqual, DescriptionLabel, NSLayoutAttribute.Bottom, 1.0f, 0f));
		}

		public void UpdateCell(string description, string textfieldText)
		{
		    DescriptionLabel.Text = description;
			TextView.Text = textfieldText;
		}
	}
}
