using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	partial class RightDetailCell : UITableViewCell
	{
        public const string CellIdentifier = "RightDetailCell";
		public UILabel MainTextLabel;
		public UILabel RightDetailTextLabel;

		public RightDetailCell(IntPtr handle)
			: base(handle)
		{
			InitializeCell();
		}

		private void InitializeCell()
		{
			this.Accessory = UITableViewCellAccessory.DisclosureIndicator;

			CreateAndAddLabels();

			SetupConstraints();
		}

		private void CreateAndAddLabels()
		{
			CreateAndAddMainTextLabel();

			CreateAndAddRightDetailTextLabel();
		}

		private void CreateAndAddRightDetailTextLabel()
		{
			RightDetailTextLabel = new UILabel
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				TextAlignment = UITextAlignment.Right,
				Font = UIFont.FromName("Helvetica", 14f),
				TextColor = StyleGuideConstants.MediumGrayUiColor
			};
			AddSubview(RightDetailTextLabel);
		}

		private void CreateAndAddMainTextLabel()
		{
			MainTextLabel = new UILabel
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName("Helvetica", 16f),
				TextColor = UIColor.Black
			};
			AddSubview(MainTextLabel);
		}

		public RightDetailCell(string cellId)
			: base(UITableViewCellStyle.Default, cellId)
		{
			InitializeCell();
		}

		public void SetupConstraints()
		{
			AddRightDetailTextLabelConstraints();
			AddMainTextLabelConstraints();
		}

		private void AddMainTextLabelConstraints()
		{
			AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.05f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, RightDetailTextLabel, NSLayoutAttribute.Left, 1.0f, 0f));
		}

		private void AddRightDetailTextLabelConstraints()
		{

			AddConstraint(NSLayoutConstraint.Create(RightDetailTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.GreaterThanOrEqual, this, NSLayoutAttribute.Right, 0.65f, 0f));
			AddConstraint(NSLayoutConstraint.Create(RightDetailTextLabel, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(RightDetailTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.90f, 0f));
		}

		public void UpdateCell(string mainText, string rightDetailText)
		{
			MainTextLabel.Text = mainText;
			RightDetailTextLabel.Text = rightDetailText;
		}
	}
}
