using System;
using UIKit;
using Foundation;

namespace AccountExecutiveApp.iOS
{
	public class SubtitleHeaderView : UIView
	{
		private string _titleText;
		private string _subtitleText;
	    private NSAttributedString _attributedTitleText;

		public string TitleText{ get{ return _titleText; } set{ _titleText = value; titleLabel.Text = TitleText; updateUI(); } }

	    public string SubtitleText
	    {
	        get{ return _subtitleText; }
	        set
	        {
	            _subtitleText = value;

                if ( _subtitleText != null && _subtitleText.Length > 35)
                    _subtitleText = _subtitleText.Substring(0, 34);

                subtitleLabel.Text = SubtitleText; 
                updateUI();
	        }
	    }

        public NSAttributedString AttributedTitleText { get { return _attributedTitleText; } set { _attributedTitleText = value; titleLabel.AttributedText = _attributedTitleText; updateUI(); } }
	    public UIFont TitleFont {
            get { return _titleFont; }
	        private set { _titleFont = value; }
	    }

	    private UIFont _titleFont = UIFont.FromName("Arial", 20);

	    private UILabel titleLabel;
		private UILabel subtitleLabel;

		public SubtitleHeaderView ()
		{
			titleLabel = new UILabel ( new CoreGraphics.CGRect(0, 0, 0, 0) );
			titleLabel.BackgroundColor = UIColor.Clear;
			titleLabel.TextColor = UIColor.DarkGray;
			titleLabel.Text = "Title";
			titleLabel.Font = UIFont.FromName ("Arial", 20);
			//titleLabel.Font = UIFont.BoldSystemFontOfSize (17);
			titleLabel.SizeToFit ();
			titleLabel.TextAlignment = UITextAlignment.Center;


			subtitleLabel = new UILabel ( new CoreGraphics.CGRect(0, 22, 0, 0) );
			subtitleLabel.BackgroundColor = UIColor.Clear;
			subtitleLabel.TextColor = UIColor.DarkGray;
			subtitleLabel.Text = "Subtitle";
			subtitleLabel.Font = UIFont.SystemFontOfSize (12);
			subtitleLabel.TextAlignment = UITextAlignment.Center;
			subtitleLabel.SizeToFit ();

			BackgroundColor = UIColor.Clear;
			AddSubview (titleLabel);
			AddSubview (subtitleLabel);	

			updateUI ();
		}

		public void updateUI()
		{
			titleLabel.SizeToFit ();
			subtitleLabel.SizeToFit ();

			float maxWidth;

			if (titleLabel.Frame.Width >= subtitleLabel.Frame.Width) 
			{
				maxWidth = (float)titleLabel.Frame.Width;

				subtitleLabel.Frame = new CoreGraphics.CGRect ( subtitleLabel.Frame.X, subtitleLabel.Frame.Y, maxWidth, subtitleLabel.Frame.Height );
			} 
			else 
			{
				maxWidth = (float)subtitleLabel.Frame.Width;

				titleLabel.Frame = new CoreGraphics.CGRect ( titleLabel.Frame.X, titleLabel.Frame.Y, maxWidth, titleLabel.Frame.Height );
			}
				
			Frame = new CoreGraphics.CGRect( 0, 0, maxWidth, 30 );
		}
	}
}

