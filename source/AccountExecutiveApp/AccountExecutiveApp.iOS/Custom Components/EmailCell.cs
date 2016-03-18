using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    partial class EmailCell : UITableViewCell
    {
        public const string CellIdentifier = "EmailCell";
        public UILabel SubjectTextLabel;
        public UILabel BodyTextLabel;
        public UILabel SubjectDetailsTextLabel;
        public UILabel BodyDetailsTextLabel;

        public EmailCell(IntPtr handle)
            : base(handle)
        {
            InitializeCell();
        }

        public EmailCell(string cellId)
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
            CreateAndAddSubjectTextLabel();
            CreateAndAddSubjectDetailsTextLabel();
            CreateAndAddBodyTextLabel();
            CreateAndAddBodyDetailsTextLabel();
        }

        private void CreateAndAddSubjectTextLabel()
        {
            SubjectTextLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Left,
                Font = UIFont.FromName("Helvetica", 16f),
                TextColor = UIColor.Black,
                Lines = 0,
                Text = "Subject:"
            };
            AddSubview(SubjectTextLabel);
        }
        private void CreateAndAddSubjectDetailsTextLabel()
        {
            SubjectDetailsTextLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Left,
                Font = UIFont.FromName("Helvetica", 16f),
                TextColor = UIColor.Black,
                Lines = 0
            };
            AddSubview(SubjectDetailsTextLabel);
        }
        private void CreateAndAddBodyTextLabel()
        {
            BodyTextLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Left,
                Font = UIFont.FromName("Helvetica", 16f),
                TextColor = UIColor.Black,
                Lines = 0,
                Text = "Body:"
            };
            AddSubview(BodyTextLabel);
        }
        private void CreateAndAddBodyDetailsTextLabel()
        {
            BodyDetailsTextLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Left,
                Font = UIFont.FromName("Helvetica", 16f),
                TextColor = UIColor.Black,
                Lines = 0
            };
            AddSubview(BodyDetailsTextLabel);
        }

        public void SetupConstraints()
        {
            AddSubjectTextLabelConstraints();
            AddSubjectDetailsTextLabelConstraints();
            AddBodyTextLabelConstraints();
            AddBodyDetailsTextLabelConstraints();
        }

        private void AddSubjectTextLabelConstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(SubjectTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.05f, 0f));
            AddConstraint(NSLayoutConstraint.Create(SubjectTextLabel, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1.0f, 40f));
            AddConstraint(NSLayoutConstraint.Create(SubjectTextLabel, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1.0f, 30f));
        }

        private void AddSubjectDetailsTextLabelConstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(SubjectDetailsTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, SubjectTextLabel, NSLayoutAttribute.Right, 1.0f, 8f));
            AddConstraint(NSLayoutConstraint.Create(SubjectDetailsTextLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, SubjectTextLabel, NSLayoutAttribute.Top, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(SubjectDetailsTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(SubjectDetailsTextLabel, NSLayoutAttribute.Bottom, NSLayoutRelation.GreaterThanOrEqual, SubjectTextLabel, NSLayoutAttribute.Bottom, 1.0f, 0f));
        }

        private void AddBodyTextLabelConstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(BodyTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, SubjectTextLabel, NSLayoutAttribute.Left, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(BodyTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, SubjectTextLabel, NSLayoutAttribute.Right, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(BodyTextLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, SubjectDetailsTextLabel, NSLayoutAttribute.Bottom, 1.0f, 4f));
        }

        private void AddBodyDetailsTextLabelConstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(BodyDetailsTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, SubjectDetailsTextLabel, NSLayoutAttribute.Left, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(BodyDetailsTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, SubjectDetailsTextLabel, NSLayoutAttribute.Right, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(BodyDetailsTextLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, BodyTextLabel, NSLayoutAttribute.Top, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(BodyDetailsTextLabel, NSLayoutAttribute.Bottom, NSLayoutRelation.GreaterThanOrEqual, BodyTextLabel, NSLayoutAttribute.Bottom, 1.0f, 0f));

            AddConstraint(NSLayoutConstraint.Create(this, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, BodyDetailsTextLabel, NSLayoutAttribute.Bottom, 1.0f, 10f));

        }

        public void UpdateCell(string subjectDetailsText, string bodyDetailsText)
        {
            SubjectDetailsTextLabel.Text = subjectDetailsText;
            BodyDetailsTextLabel.Text = bodyDetailsText;
        }
    }
}
