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

		    BackgroundColor = UIColor.FromWhiteAlpha(0.98f, 1.0f);
		}

		private void CreateAndAddLabels()
		{
			CreateAndAddTextView();
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


	        TextView.Text = string.Empty;

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
	    }

	    public void doneButtonTapped(object sender, EventArgs args)
		{
			TextView.ResignFirstResponder();
		}

		public void SetupConstraints()
		{
			AddTextLabelConstraints();
		}


		private void AddTextLabelConstraints()
		{
			AddConstraint(NSLayoutConstraint.Create(TextView, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.1f, 0f));
			AddConstraint(NSLayoutConstraint.Create(TextView, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(TextView, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.9f, 0f));
            AddConstraint(NSLayoutConstraint.Create(TextView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this, NSLayoutAttribute.Height, 1.0f, 0f));
		}

		public void UpdateCell(string textfieldText)
		{
			TextView.Text = textfieldText;
		}
	}
}
