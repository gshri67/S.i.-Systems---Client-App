using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ConsultantApp.iOS
{
	partial class SubmitTimeSheetViewController : UIViewController
	{
		private UIView editApproverView;
		private UIButton submitButton;
		private UIScrollView backgroundScrollLayer;
		private UIView scrollView;
		private CalendarTimeEntryView calendarView;
		private ProjectCodeSummaryView projectCodesView;

		public SubmitTimeSheetViewController (IntPtr handle) : base (handle)
		{
			editApproverView = new UIView ();
			editApproverView.BackgroundColor = UIColor.Gray;
			editApproverView.TranslatesAutoresizingMaskIntoConstraints = false;

			submitButton = new UIButton ();
			submitButton.BackgroundColor = UIColor.Orange;
			submitButton.TranslatesAutoresizingMaskIntoConstraints = false;
			submitButton.SetTitle ("Submit", UIControlState.Normal);

			calendarView = new CalendarTimeEntryView ();
			calendarView.BackgroundColor = UIColor.Green;
			calendarView.TranslatesAutoresizingMaskIntoConstraints = false;

			projectCodesView = new ProjectCodeSummaryView ();
			projectCodesView.BackgroundColor = UIColor.Cyan;
			projectCodesView.TranslatesAutoresizingMaskIntoConstraints = false;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			View.BackgroundColor = UIColor.Blue;

			backgroundScrollLayer = new UIScrollView( View.Bounds );
			backgroundScrollLayer.TranslatesAutoresizingMaskIntoConstraints = false;
			backgroundScrollLayer.BackgroundColor = UIColor.Purple;
			//backgroundScrollLayer.ContentSize = new CoreGraphics.CGSize (300, 350 );
			View.AddSubview (backgroundScrollLayer);

			scrollView = new UIView (View.Bounds);//new CoreGraphics.CGRect(0, 0, 200, 300));
			scrollView.TranslatesAutoresizingMaskIntoConstraints = false;
			//scrollView = new UIScrollView ( new CoreGraphics.CGRect( 0, 0, View.Frame.Size.Width, View.Frame.Size.Height ));
			scrollView.BackgroundColor = UIColor.Red;

			backgroundScrollLayer.AddSubview (scrollView);
			scrollView.AddSubview (editApproverView);
			scrollView.AddSubview (submitButton);
			scrollView.AddSubview (calendarView);
			scrollView.AddSubview (projectCodesView);

			//scrollView.Frame = new CoreGraphics.CGRect (0, 0, scrollView.Bounds.Width, 1000);
			//scrollView.ContentSize = new CoreGraphics.CGSize(scrollView.Bounds.Width, scrollView.Bounds.Height*2);
			setupConstraints();
		}

		public override void ViewDidLayoutSubviews ()
		{
			base.ViewDidLayoutSubviews ();

			backgroundScrollLayer.ContentSize = new CoreGraphics.CGSize (View.Bounds.Width, View.Bounds.Height+100);
		}

		public void setupConstraints() 
		{
			//scroll view

			View.AddConstraint(NSLayoutConstraint.Create(backgroundScrollLayer, NSLayoutAttribute.Left, NSLayoutRelation.Equal, View, NSLayoutAttribute.Left, 1.0f, 0f));
			View.AddConstraint(NSLayoutConstraint.Create(backgroundScrollLayer, NSLayoutAttribute.Top, NSLayoutRelation.Equal, View, NSLayoutAttribute.Top, 1.0f, 0f));
			View.AddConstraint(NSLayoutConstraint.Create(backgroundScrollLayer, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, View, NSLayoutAttribute.Bottom, 1.0f, 0f));
			View.AddConstraint(NSLayoutConstraint.Create(backgroundScrollLayer, NSLayoutAttribute.Right, NSLayoutRelation.Equal, View, NSLayoutAttribute.Right, 1.0f, 0f));

			backgroundScrollLayer.AddConstraint(NSLayoutConstraint.Create(scrollView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, backgroundScrollLayer, NSLayoutAttribute.CenterX, 1.0f, 0f));
			backgroundScrollLayer.AddConstraint(NSLayoutConstraint.Create(scrollView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, backgroundScrollLayer, NSLayoutAttribute.Top, 1.0f, 0f));
			backgroundScrollLayer.AddConstraint(NSLayoutConstraint.Create(scrollView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, backgroundScrollLayer, NSLayoutAttribute.Bottom, 1.0f, 0f));
			backgroundScrollLayer.AddConstraint(NSLayoutConstraint.Create(scrollView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, backgroundScrollLayer, NSLayoutAttribute.Width, 1.0f, 0f));

			//edit approver
			scrollView.AddConstraint(NSLayoutConstraint.Create(editApproverView, NSLayoutAttribute.Left, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Left, 1.0f, 0f));
			scrollView.AddConstraint(NSLayoutConstraint.Create(editApproverView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Top, 1.0f, 0f));
			scrollView.AddConstraint(NSLayoutConstraint.Create(editApproverView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Bottom, 0.1f, 0f));
			scrollView.AddConstraint(NSLayoutConstraint.Create(editApproverView, NSLayoutAttribute.Right, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Right, 1.0f, 0f));

			//calendar
			scrollView.AddConstraint(NSLayoutConstraint.Create(calendarView, NSLayoutAttribute.Left, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Left, 1.0f, 0f));
			scrollView.AddConstraint(NSLayoutConstraint.Create(calendarView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, editApproverView, NSLayoutAttribute.Bottom, 1.0f, 0f));
			scrollView.AddConstraint(NSLayoutConstraint.Create(calendarView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Bottom, 0.75f, 0f));
			scrollView.AddConstraint(NSLayoutConstraint.Create(calendarView, NSLayoutAttribute.Right, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Right, 1.0f, 0f));

			//submit
			scrollView.AddConstraint(NSLayoutConstraint.Create(submitButton, NSLayoutAttribute.Left, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Left, 1.0f, 0f));
			scrollView.AddConstraint(NSLayoutConstraint.Create(submitButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Bottom, 0.9f, 0f));
			scrollView.AddConstraint(NSLayoutConstraint.Create(submitButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Bottom, 1.0f, 0f));
			scrollView.AddConstraint(NSLayoutConstraint.Create(submitButton, NSLayoutAttribute.Right, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Right, 1f, 0f));

			//project codes
			scrollView.AddConstraint(NSLayoutConstraint.Create(projectCodesView, NSLayoutAttribute.Left, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Left, 1.0f, 0f));
			scrollView.AddConstraint(NSLayoutConstraint.Create(projectCodesView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, calendarView, NSLayoutAttribute.Bottom, 1.0f, 0f));
			scrollView.AddConstraint(NSLayoutConstraint.Create(projectCodesView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, submitButton, NSLayoutAttribute.Top, 1.0f, 0f));
			scrollView.AddConstraint(NSLayoutConstraint.Create(projectCodesView, NSLayoutAttribute.Right, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Right, 1.0f, 0f));
		}
	}
}
