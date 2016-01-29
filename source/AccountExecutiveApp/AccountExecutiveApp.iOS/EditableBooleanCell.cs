using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    partial class EditableBooleanCell : UITableViewCell
    {
        public const string CellIdentifier = "EditableBooleanCell";
        public UILabel MainTextLabel;
        public UISwitch RightSwitch;
        private bool shrinkRightDetailText = true;

        public EditableBooleanCell(IntPtr handle)
            : base(handle)
        {
            InitializeCell();
        }
        public EditableBooleanCell(string cellId)
            : base(UITableViewCellStyle.Default, cellId)
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

            CreateAndAddRightSwitch();

        }

        private void CreateAndAddRightSwitch()
        {
            RightSwitch = new UISwitch()
            {
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            AddSubview(RightSwitch);
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

        public void SetupConstraints()
        {
                AddRightSwitchConstraints();
                AddMainTextLabelConstraints();
        }

        private void AddMainTextLabelConstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.05f, 0f));
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1f, 0f));
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, RightSwitch, NSLayoutAttribute.Left, 1.0f, 0f));
        }

        private void AddRightSwitchConstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(RightSwitch, NSLayoutAttribute.Left, NSLayoutRelation.GreaterThanOrEqual, this, NSLayoutAttribute.Right, 0.6f, 0f));
            AddConstraint(NSLayoutConstraint.Create(RightSwitch, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(RightSwitch, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.90f, 0f));
        }

        public void UpdateCell(string mainText, string rightDetailText)
        {
            MainTextLabel.Text = mainText;
            //RightDetailTextField.Text = rightDetailText;
        }
    }
}
