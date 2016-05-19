using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using UIKit;
using MonoTouch;

namespace AccountExecutiveApp.iOS
{
    partial class EmailCell : UITableViewCell
    {
        public const string CellIdentifier = "EmailCell";
        public UILabel SubjectTextLabel;
        public UILabel BodyTextLabel;
        public UILabel SubjectDetailsTextLabel;
        public UITextView BodyDetailsTextLabel;

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
                Text = "Body:"
            };
            AddSubview(BodyTextLabel);
        }
        private void CreateAndAddBodyDetailsTextLabel()
        {
            BodyDetailsTextLabel = new UITextView()
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Left,
                Font = UIFont.FromName("Helvetica", 16f),
                TextColor = UIColor.Black,
                ScrollEnabled = false,
                Selectable = true,
                Editable = false,
                UserInteractionEnabled = true
            };

            BodyDetailsTextLabel.DataDetectorTypes = UIDataDetectorType.Link;
            /*
            BodyDetailsTextLabel.ShouldInteractWithUrl =
                delegate(UITextView textView, NSUrl url, NSRange range)
                {
                    Console.WriteLine("Tapped URL!");

                    return false;
                };
            */

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
            AddConstraint(NSLayoutConstraint.Create(SubjectTextLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1.0f, 0f));
            //AddConstraint(NSLayoutConstraint.Create(SubjectTextLabel, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1.0f, 30f));
            AddConstraint(NSLayoutConstraint.Create(SubjectTextLabel, NSLayoutAttribute.Width, NSLayoutRelation.GreaterThanOrEqual, null, NSLayoutAttribute.NoAttribute, 1.0f, 60f));
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
            //BodyDetailsTextLabel.Text = bodyDetailsText;

            NSMutableAttributedString attributedDetails = new NSMutableAttributedString(bodyDetailsText);

            string textForUrl = "Click here";
            List<int> indicies = AllIndexesOf(bodyDetailsText, textForUrl);

            List<string> urlList = new List<string>(){"http://www.google.com", "http://www.google.com"};

            for(int i = 0; i < indicies.Count; i ++ )
            {
                int startIndex = indicies[i];
                NSUrl url = NSUrl.FromString(urlList[i]);

                attributedDetails.AddAttribute(UIStringAttributeKey.Link, url, new NSRange(startIndex, textForUrl.Length));    
            }

            BodyDetailsTextLabel.AttributedText = attributedDetails;
        }

        public static List<int> AllIndexesOf(string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
    }
}
