using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	partial class ProposedContractorsTableViewCell : UITableViewCell
	{
		public UILabel MainTextLabel;
		public UILabel SubtitleTextLabel;
		public UILabel RightDetailTextLabel;

		private UILabel BillRateNameLabel;
		private UILabel PayRateNameLabel;
		private UILabel MarginNameLabel;
		private UILabel MarkupNameLabel;
		private UILabel GrossMarginNameLabel;

		private UILabel BillRateValueLabel;
		private UILabel PayRateValueLabel;
		private UILabel MarginValueLabel;
		private UILabel MarkupValueLabel;
		private UILabel GrossMarginValueLabel;

		public ProposedContractorsTableViewCell(IntPtr handle)
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

			CreateAndAddSubtitleTextLabel();

			CreateAndAddRightDetailTextLabel();

			CreateAndAddRateNameLabels ();
			CreateAndAddRateValueLabels ();
		}

		private void CreateAndAddRateNameLabels()
		{
			InitializeRateNameLabels ();

			PopulateAndAddRateNameLabel (BillRateNameLabel);
			PopulateAndAddRateNameLabel (PayRateNameLabel);
			PopulateAndAddRateNameLabel (MarginNameLabel);
			PopulateAndAddRateNameLabel (MarkupNameLabel);
			PopulateAndAddRateNameLabel (GrossMarginNameLabel);

			BillRateNameLabel.Text = "Bill Rate";
			PayRateNameLabel.Text = "Pay Rate";
			GrossMarginNameLabel.Text = "Gross Margin";
			MarginNameLabel.Text = "Margin";
			MarkupNameLabel.Text = "Markup";
		}
		private void InitializeRateNameLabels()
		{
			BillRateNameLabel = new UILabel ();
			PayRateNameLabel = new UILabel ();
			GrossMarginNameLabel = new UILabel ();
			MarginNameLabel = new UILabel ();
			MarkupNameLabel = new UILabel ();
		}
		private void PopulateAndAddRateNameLabel( UILabel label )
		{
			label.TranslatesAutoresizingMaskIntoConstraints = false;
			label.TextAlignment = UITextAlignment.Center;
			label.Font = UIFont.FromName("Helvetica", 10f);
			label.TextColor = StyleGuideConstants.MediumGrayUiColor;
	
			AddSubview(label);
		}

		private void CreateAndAddRateValueLabels()
		{
			InitializeRateValueLabels ();

			PopulateAndAddRateValueLabel (BillRateValueLabel);
			PopulateAndAddRateValueLabel (PayRateValueLabel);
			PopulateAndAddRateValueLabel (MarginValueLabel);
			PopulateAndAddRateValueLabel (MarkupValueLabel);
			PopulateAndAddRateValueLabel (GrossMarginValueLabel);
		}
		private void InitializeRateValueLabels()
		{
			BillRateValueLabel = new UILabel ();
			PayRateValueLabel = new UILabel ();
			GrossMarginValueLabel = new UILabel ();
			MarginValueLabel = new UILabel ();
			MarkupValueLabel = new UILabel ();
		}
		private void PopulateAndAddRateValueLabel( UILabel label )
		{
			label.TranslatesAutoresizingMaskIntoConstraints = false;
			label.TextAlignment = UITextAlignment.Center;
			label.Font = UIFont.FromName("Helvetica", 10f);
			label.TextColor = StyleGuideConstants.MediumGrayUiColor;

			AddSubview(label);
		}

		private void CreateAndAddRightDetailTextLabel()
		{
			RightDetailTextLabel = new UILabel
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				TextAlignment = UITextAlignment.Right,
				Font = UIFont.FromName("Helvetica", 14f),
				TextColor = StyleGuideConstants.MediumGrayUiColor,
				Hidden = true
			};
			AddSubview(RightDetailTextLabel);
		}

		private void CreateAndAddSubtitleTextLabel()
		{
			SubtitleTextLabel = new UILabel
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName("Helvetica", 12f),
				TextColor = StyleGuideConstants.MediumGrayUiColor,
				Hidden = true
			};
			AddSubview(SubtitleTextLabel);
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

		public ProposedContractorsTableViewCell(string cellId)
			: base(UITableViewCellStyle.Default, cellId)
		{
			InitializeCell();
		}

		public void SetupConstraints()
		{
			AddMainTextLabelConstraints();

			AddSubtitleTextLabelConstraints();

			//AddRightDetailTextLabelContstraints();

			AddRateNameLabelConstraints ();
			AddRateValueLabelConstraints ();
		}

		private void AddMainTextLabelConstraints()
		{
			AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.05f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1f, 3f));
		}

		private void AddSubtitleTextLabelConstraints()
		{
			AddConstraint(NSLayoutConstraint.Create(SubtitleTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, MainTextLabel, NSLayoutAttribute.Left, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(SubtitleTextLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, MainTextLabel, NSLayoutAttribute.Bottom, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(SubtitleTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, MainTextLabel, NSLayoutAttribute.Right, 1.0f, 0f));
		}

		private void AddRightDetailTextLabelContstraints()
		{

			AddConstraint(NSLayoutConstraint.Create(RightDetailTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.GreaterThanOrEqual, MainTextLabel, NSLayoutAttribute.Right, 1.0f, 15f));
			AddConstraint(NSLayoutConstraint.Create(RightDetailTextLabel, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(RightDetailTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.90f, 0f));
		}

		private void AddRateNameLabelConstraints()
		{
			float widthRatioPerLabel = 0.25f;

			AddConstraint(NSLayoutConstraint.Create(BillRateNameLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, SubtitleTextLabel, NSLayoutAttribute.Left, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(BillRateNameLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, SubtitleTextLabel, NSLayoutAttribute.Bottom, 1.0f, 4f));
			AddConstraint(NSLayoutConstraint.Create(BillRateNameLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, SubtitleTextLabel, NSLayoutAttribute.Width, widthRatioPerLabel, 0f));
			AddConstraint(NSLayoutConstraint.Create(BillRateNameLabel, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1.0f, 10));

			AddConstraint(NSLayoutConstraint.Create(PayRateNameLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, BillRateNameLabel, NSLayoutAttribute.Right, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(PayRateNameLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, BillRateNameLabel, NSLayoutAttribute.Top, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(PayRateNameLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, BillRateNameLabel, NSLayoutAttribute.Width, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(PayRateNameLabel, NSLayoutAttribute.Height, NSLayoutRelation.Equal, BillRateNameLabel, NSLayoutAttribute.Height, 1.0f, 0));

			AddConstraint(NSLayoutConstraint.Create(GrossMarginNameLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, PayRateNameLabel, NSLayoutAttribute.Right, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(GrossMarginNameLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, BillRateNameLabel, NSLayoutAttribute.Top, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(GrossMarginNameLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, BillRateNameLabel, NSLayoutAttribute.Width, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(GrossMarginNameLabel, NSLayoutAttribute.Height, NSLayoutRelation.Equal, BillRateNameLabel, NSLayoutAttribute.Height, 1.0f, 0));

			AddConstraint(NSLayoutConstraint.Create(MarginNameLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, GrossMarginNameLabel, NSLayoutAttribute.Right, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MarginNameLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, BillRateNameLabel, NSLayoutAttribute.Top, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MarginNameLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, BillRateNameLabel, NSLayoutAttribute.Width, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MarginNameLabel, NSLayoutAttribute.Height, NSLayoutRelation.Equal, BillRateNameLabel, NSLayoutAttribute.Height, 1.0f, 0));

			AddConstraint(NSLayoutConstraint.Create(MarkupNameLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, MarginNameLabel, NSLayoutAttribute.Right, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MarkupNameLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, BillRateNameLabel, NSLayoutAttribute.Top, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MarkupNameLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, BillRateNameLabel, NSLayoutAttribute.Width, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MarkupNameLabel, NSLayoutAttribute.Height, NSLayoutRelation.Equal, BillRateNameLabel, NSLayoutAttribute.Height, 1.0f, 0));
		}

		private void AddRateValueLabelConstraints()
		{
			float widthRatioPerLabel = 0.25f;

			AddConstraint(NSLayoutConstraint.Create(BillRateValueLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, BillRateNameLabel, NSLayoutAttribute.Left, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(BillRateValueLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, BillRateNameLabel, NSLayoutAttribute.Bottom, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(BillRateValueLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, BillRateNameLabel, NSLayoutAttribute.Width, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(BillRateValueLabel, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1.0f, 10));

			AddConstraint(NSLayoutConstraint.Create(PayRateValueLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, BillRateValueLabel, NSLayoutAttribute.Right, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(PayRateValueLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, BillRateValueLabel, NSLayoutAttribute.Top, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(PayRateValueLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, BillRateValueLabel, NSLayoutAttribute.Width, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(PayRateValueLabel, NSLayoutAttribute.Height, NSLayoutRelation.Equal, BillRateValueLabel, NSLayoutAttribute.Height, 1.0f, 0));

			AddConstraint(NSLayoutConstraint.Create(GrossMarginValueLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, PayRateValueLabel, NSLayoutAttribute.Right, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(GrossMarginValueLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, BillRateValueLabel, NSLayoutAttribute.Top, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(GrossMarginValueLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, BillRateValueLabel, NSLayoutAttribute.Width, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(GrossMarginValueLabel, NSLayoutAttribute.Height, NSLayoutRelation.Equal, BillRateValueLabel, NSLayoutAttribute.Height, 1.0f, 0));

			AddConstraint(NSLayoutConstraint.Create(MarginValueLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, GrossMarginValueLabel, NSLayoutAttribute.Right, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MarginValueLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, BillRateValueLabel, NSLayoutAttribute.Top, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MarginValueLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, BillRateValueLabel, NSLayoutAttribute.Width, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MarginValueLabel, NSLayoutAttribute.Height, NSLayoutRelation.Equal, BillRateValueLabel, NSLayoutAttribute.Height, 1.0f, 0));

			AddConstraint(NSLayoutConstraint.Create(MarkupValueLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, MarginValueLabel, NSLayoutAttribute.Right, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MarkupValueLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, BillRateValueLabel, NSLayoutAttribute.Top, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MarkupValueLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, BillRateValueLabel, NSLayoutAttribute.Width, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MarkupValueLabel, NSLayoutAttribute.Height, NSLayoutRelation.Equal, BillRateValueLabel, NSLayoutAttribute.Height, 1.0f, 0));
		}

		public void UpdateCell(string mainText, string subtitleText, string billRate, string payRate, string grossMargin, string margin, string markup)
		{
			MainTextLabel.Text = mainText;
			SubtitleTextLabel.Text = subtitleText;
			BillRateValueLabel.Text = billRate;
			PayRateValueLabel.Text = payRate;
			GrossMarginValueLabel.Text = grossMargin;
			MarginValueLabel.Text = margin;
			MarkupValueLabel.Text = markup;

		    HideRateInformation(billRate == string.Empty);
		}

	    private void HideRateInformation( bool hide )
	    {
	        BillRateNameLabel.Hidden = hide;
            PayRateNameLabel.Hidden = hide;
            GrossMarginNameLabel.Hidden = hide;
            MarkupNameLabel.Hidden = hide;
			MarginNameLabel.Hidden = hide;
	    }
	}
}
