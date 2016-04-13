using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	partial class MultiSelectDescriptionCell : UITableViewCell
	{
		public const string CellIdentifier = "MultiSelectDescriptionCell";
		public UILabel MainTextLabel;
		public UITextView RightDetailTextView;
		private bool shrinkRightDetailText = true;

		public delegate void EditableCellDelegate(string newValue);
		//public EditableCellDelegate OnValueChanged;

		public MultiSelectDescriptionCell(IntPtr handle)
			: base(handle)
		{
			InitializeCell();
		}

		public MultiSelectDescriptionCell(string cellId)
			: base(UITableViewCellStyle.Default, cellId)
		{
			InitializeCell();
		}

		private void InitializeCell()
		{
			CreateAndAddLabels();

			SetupConstraints();
		}

		private void CreateAndAddLabels()
		{
			CreateAndAddMainTextLabel();

			CreateAndAddRightDetailTextField();
		}

		private void CreateAndAddRightDetailTextField()
		{
			RightDetailTextView = new UITextView
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				TextAlignment = UITextAlignment.Right,
				Font = UIFont.FromName("Helvetica", 14f),
				TextColor = StyleGuideConstants.MediumGrayUiColor,
                ScrollEnabled = false
			};

			RightDetailTextView.Text = "Text Field";
            RightDetailTextView.TextContainerInset = UIEdgeInsets.Zero;
		    RightDetailTextView.TextContainer.LineFragmentPadding = 0;
		    RightDetailTextView.UserInteractionEnabled = false;

			AddSubview(RightDetailTextView);

			var toolbar = new UIToolbar(new CoreGraphics.CGRect(0.0f, 0.0f, Frame.Size.Width, 44.0f));

			toolbar.Items = new[]
			{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, doneButtonTapped)
			};

			RightDetailTextView.InputAccessoryView = toolbar;
		}

		private void CreateAndAddMainTextLabel()
		{
			MainTextLabel = new UILabel
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName("Helvetica", 16f),
				TextColor = UIColor.Black,
				//Lines = 0
			};
			AddSubview(MainTextLabel);
		}

		public void doneButtonTapped(object sender, EventArgs args)
		{
			RightDetailTextView.ResignFirstResponder();
		}

		public void SetupConstraints()
		{
			AddRightDetailTextLabelConstraints();
			AddMainTextLabelConstraints();
		    AddConstraint(NSLayoutConstraint.Create(Self, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, RightDetailTextView, NSLayoutAttribute.Bottom, 1.0f, 0f));
		}

		private void AddMainTextLabelConstraints()
		{
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.05f, 0f));
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1f, 0f));
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.LessThanOrEqual, this, NSLayoutAttribute.Right, 0.7f, 0f));
		}

		private void AddRightDetailTextLabelConstraints()
		{
            AddConstraint(NSLayoutConstraint.Create(RightDetailTextView, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.90f, 0f));
            AddConstraint(NSLayoutConstraint.Create(RightDetailTextView, NSLayoutAttribute.Left, NSLayoutRelation.Equal, MainTextLabel, NSLayoutAttribute.Right, 1.0f, 15f));
            AddConstraint(NSLayoutConstraint.Create(RightDetailTextView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1.0f, 0f));
		}

		public void UpdateCell(string mainText, string rightDetailText)
		{
			MainTextLabel.Text = mainText;
			RightDetailTextView.Text = rightDetailText;
		}

        public void UpdateCell(string mainText, List<string> entries)
        {
            MainTextLabel.Text = mainText;

            if (entries != null && entries.Count > 0)
            {
                RightDetailTextView.Text = entries[0];

                foreach (var entry in entries.GetRange(1, entries.Count - 1))
                    RightDetailTextView.Text += "\n" + entry;
            }
            else
                RightDetailTextView.Text = "None";
        }
	}
}
