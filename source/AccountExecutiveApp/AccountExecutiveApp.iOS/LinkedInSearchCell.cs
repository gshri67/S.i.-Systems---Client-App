using Foundation;
using System;
using System.CodeDom.Compiler;
using CoreGraphics;
using UIKit;


namespace AccountExecutiveApp.iOS
{
    partial class LinkedInSearchCell : UITableViewCell
    {
        public const string CellIdentifier = "LinkedInSearchCell";
        public UIImageView ImageView;

        public LinkedInSearchCell(IntPtr handle)
            : base(handle)
        {
            InitializeCell();
        }

        private void InitializeCell()
        {
            this.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            CreateAndAddViews();
        }

        private void CreateAndAddViews()
        {
            CreateAndAddImageView();
        }

        private void CreateAndAddImageView()
        {
            UIImage LinkedInLogo = new UIImage("LinkedInLogo.png");
            ImageView = new UIImageView(LinkedInLogo);

            NeedsUpdateConstraints();
            LayoutIfNeeded();

            float aspectRatio = (float)LinkedInLogo.Size.Width/(float)LinkedInLogo.Size.Height;

            ImageView.Frame = new CGRect(ContentView.Frame.Width * 0.05f + 3, ContentView.Frame.Height * 0.1f, aspectRatio*ContentView.Frame.Height*0.8f, ContentView.Frame.Height * 0.8f);

            ContentView.AddSubview(ImageView);
        }
    }
}